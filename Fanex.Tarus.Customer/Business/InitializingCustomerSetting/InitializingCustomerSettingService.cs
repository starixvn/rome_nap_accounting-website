namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using DNAide.Web;
    using System.Text;
    using Fanex.Tarus.Infrastructure.Caching;
    using System.Linq;

    public interface IInitializingCustomerSettingService
    {
        InitializingCustomerSettingViewModel GetSetting();

        Result DeleteConfigSetting(int configId, int userId, string ip);

        Result AbortConfigSetting(int configId, int userId, string ip);

        IEnumerable<ProductModel> GetProductSportBettype();

        ConfigSettingViewModel GetConfigDetail(int configId);
    }
    public class InitializingCustomerSettingService : IInitializingCustomerSettingService
    {
        private readonly IInitializingCustomerSettingDataAccess dataAccess;

        private const string FormatDisplay = "[{0}] {1}";
        private const string OptionDisplay = "<option value={0}>{1}</option>";
        private const string ConfigTempalte = "../../App_Templates//Default/InitializingCustomerSetting/ConfigDetail.htm";
        private const string SettingHeaderTemplate = "../../App_Templates/Default/InitializingCustomerSetting/SettingHeader.htm";
        private const string SettingTempalte = "../../App_Templates/Default/InitializingCustomerSetting/Setting{0}.htm";

        public InitializingCustomerSettingService()
        {
            dataAccess = new InitializingCustomerSettingDataAccess();
        }

        public Result AbortConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new UpdateConfigStatusCriteria
            {
                ConfigId = configId,
                UserId = userId,
                Status = 3,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public Result DeleteConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new DeleteConfigCriteria
            {
                ConfigId = configId,
                UserId = userId,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public ConfigSettingViewModel GetConfigDetail(int configId)
        {
            var criteria = new GetConfigDetailCriteria(configId);
            var config = dataAccess.GetConfigDetail(criteria);
            var sportbettypesetting = dataAccess.GetSportAndBettypeWithSetting(new DcsProductMatrixCriteria(), config.Config.ProductId);
            var configSettingHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(ConfigTempalte);
            var settingsHtml = new StringBuilder();
            var groupSettings = config.Settings.GroupBy(setting => setting.SettingType);
            foreach (var settingGroup in groupSettings)
            {
                var sportBettype = sportbettypesetting.FirstOrDefault(st => st.SettingId == settingGroup.Key);
                var sportOptions = BuildSportOptions(sportBettype.Sports);
                settingsHtml.Append(BuildSettingHeader(settingGroup.Key, settingGroup.FirstOrDefault().SettingName));
                foreach (var setting in settingGroup)
                {
                    var bettypeOptions = BuildBettypeOptions(sportBettype.Sports.FirstOrDefault(sp => sp.SportId == setting.SportType).Bettypes);
                    settingsHtml.Append(BuildSettingTempalte(setting, sportOptions, bettypeOptions));
                }
            }

            TemplateHandler.Assign(ref configSettingHtml, "{settings}", settingsHtml.ToString());

            return new ConfigSettingViewModel
            {
                ConfigDetail = config,
                ConfigSettingHtml = configSettingHtml
            };
        }

        public IEnumerable<ProductModel> GetProductSportBettype()
        {
            var criteria = new DcsProductMatrixCriteria();

            return dataAccess.GetProductSportBettype(criteria);
        }

        public InitializingCustomerSettingViewModel GetSetting()
        {
            var currencies = dataAccess.GetExchangeCurrency();
            var sites = dataAccess.GetSites();
            return dataAccess.GetSetting(new GetSettingCriteria(), currencies, sites);
        }

        private string BuildSportOptions(IEnumerable<SportModel> sports)
        {
            return BuildOptions(sports.Select(sp => new BaseModel { Id = sp.SportId, Name = sp.SportName }));
        }

        private string BuildBettypeOptions(IEnumerable<BettypeModel> bettypes)
        {
            return BuildOptions(bettypes.Select(sp => new BaseModel { Id = sp.BettypeId, Name = sp.BettypeName }));
        }

        private string BuildOptions(IEnumerable<BaseModel> items)
        {
            var result = new StringBuilder();
            foreach (var item in items)
            {
                result.Append(string.Format(OptionDisplay, item.Id, item.Name));
            }

            return result.ToString();
        }

        private string BuildSettingHeader(int settingtypeid, string settingName)
        {

            var header = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(SettingHeaderTemplate);

            TemplateHandler.Assign(ref header, "{settingname}", settingName);
            TemplateHandler.Assign(ref header, "{settingtypeid}", settingtypeid);

            return header;
        }

        private string BuildSettingTempalte(SettingInforModel setting, string sporttypeOptions = "", string bettypeOptions = "")
        {
            var settingtemplate = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(string.Format(SettingTempalte, setting.SettingType));
            TemplateHandler.Assign(ref settingtemplate, "{sport-type}", string.Format(FormatDisplay, setting.SportType, CacheRef.GetSportName(setting.SportType)));
            TemplateHandler.Assign(ref settingtemplate, "{bet-type}", string.Format(FormatDisplay, setting.BetType, CacheRef.GetBettype(setting.BetType)));
            TemplateHandler.Assign(ref settingtemplate, "{ckbCopy}", setting.IsDefault ? string.Empty : "checked");
            TemplateHandler.Assign(ref settingtemplate, "{ckbDefault}", setting.IsDefault ? "checked" : string.Empty);
            TemplateHandler.Assign(ref settingtemplate, "{sporttypeopts}", sporttypeOptions);
            TemplateHandler.Assign(ref settingtemplate, "{bettypeopts}", bettypeOptions); 
            TemplateHandler.Assign(ref settingtemplate, "{ckbAutoResetDaily}", setting.IsLive ? "checked" : string.Empty);


            TemplateHandler.Assign(ref settingtemplate, "{status-super}", setting.DefaultValueSuper == "1"? "<p class=\"status-enable\">Enable</p>" : "<p class=\"status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{status-master}", setting.DefaultValueMaster == "1" ? "<p class=\"status-enable\">Enable</p>" : "<p class=\"status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{status-agent}", setting.DefaultValueAgent == "1" ? "<p class=\"status-enable\">Enable</p>" : "<p class=\"status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{status-member}", setting.DefaultValueMember == "1" ? "<p class=\"status-enable\">Enable</p>" : "<p class=\"status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{txtHeadline}", setting.SpecialRequirement);

            TemplateHandler.Assign(ref settingtemplate, "{comSuper}", setting.DefaultValueSuper);
            TemplateHandler.Assign(ref settingtemplate, "{comMaster}", setting.DefaultValueMaster);
            TemplateHandler.Assign(ref settingtemplate, "{comAgent}", setting.DefaultValueAgent);
            TemplateHandler.Assign(ref settingtemplate, "{comMember}", setting.DefaultValueMember);

            return settingtemplate;
        }
        
    }
}
