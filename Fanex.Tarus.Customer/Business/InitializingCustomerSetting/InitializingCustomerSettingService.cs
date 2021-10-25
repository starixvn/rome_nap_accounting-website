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
        private const string OptionSelectedDisplay = "<option value={0} selected>{1}</option>";
        private const string ConfigTempalte = "../../App_Templates//Default/InitializingCustomerSetting/ConfigDetail.htm";
        private const string SettingHeaderTemplate = "../../App_Templates/Default/InitializingCustomerSetting/SettingHeader.htm";
        private const string SettingGeneralInfoTemplate = "../../App_Templates/Default/InitializingCustomerSetting/GeneralInfo.htm";
        private const string SettingTempalte = "../../App_Templates/Default/InitializingCustomerSetting/Setting{0}.htm";
        private const string IdKey = "{0}{1}{2}{3}{4}{5}";

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
            var sportbettypesetting = dataAccess.GetSportAndBettypeWithSetting(new DcsProductMatrixCriteria());
            var configSettingHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(ConfigTempalte);
            var generalInfoHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(SettingGeneralInfoTemplate);
            TemplateHandler.Assign(ref configSettingHtml, "{general-info}", generalInfoHtml);
            var settingsHtml = new StringBuilder();
            var groupSettings = config.Settings.GroupBy(setting => setting.SettingType);
            foreach (var settingGroup in groupSettings)
            {
                var sportBettype = sportbettypesetting.FirstOrDefault(st => st.SettingId == settingGroup.Key);
                settingsHtml.Append(BuildSettingHeader(settingGroup.Key, settingGroup.FirstOrDefault().SettingName));
                foreach (var setting in settingGroup)
                {
                    var sportOptions = BuildSportOptions(sportBettype.Sports, setting.CopyFromSportType);
                    var settingSport = sportBettype.Sports.FirstOrDefault(sp => sp.SportId == setting.CopyFromSportType);
                    if(settingSport != null)
                    {
                        var bettypeOptions = BuildBettypeOptions(settingSport.Bettypes, setting.CopyFromBetType);
                        settingsHtml.Append(BuildSettingTempalte(setting, sportOptions, bettypeOptions));
                    }
                }
            }
            TemplateHandler.Assign(ref configSettingHtml, "{settings}", settingsHtml.ToString());

            var sites = dataAccess.GetSites();

            var exchangeCurrencies = dataAccess.GetExchangeCurrency();

            return new ConfigSettingViewModel
            {
                ConfigDetail = config,
                ConfigSettingHtml = configSettingHtml,
                SportBettypeBySettings = sportbettypesetting,
                Sites = sites,
                Currencies = exchangeCurrencies
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

        private string BuildSportOptions(IEnumerable<SportModel> sports, int selectedSport)
        {
            return BuildOptions(sports.Select(sp => new BaseModel { Id = sp.SportId, Name = sp.SportName }), selectedSport);
        }

        private string BuildBettypeOptions(IEnumerable<BettypeModel> bettypes, int selectedBettype)
        {
            return BuildOptions(bettypes.Select(sp => new BaseModel { Id = sp.BettypeId, Name = sp.BettypeName }), selectedBettype);
        }

        private string BuildOptions(IEnumerable<BaseModel> items, int selectedItem = -1)
        {
            var result = new StringBuilder();
            foreach (var item in items)
            {
                if(item.Id == selectedItem)
                {
                    result.Append(string.Format(OptionSelectedDisplay, item.Id, item.Name));
                    
                } else
                {
                    result.Append(string.Format(OptionDisplay, item.Id, item.Name));
                }
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
            var sportName = CacheRef.GetSportName(setting.SportType);
            var bettypename = CacheRef.GetBettype(setting.BetType);
            TemplateHandler.Assign(ref settingtemplate, "{sport-type}", string.Format(FormatDisplay, setting.SportType, sportName));
            TemplateHandler.Assign(ref settingtemplate, "{bet-type}", string.Format(FormatDisplay, setting.BetType, bettypename));
            TemplateHandler.Assign(ref settingtemplate, "{ckbCopy}", setting.IsDefault ? string.Empty : "checked");
            TemplateHandler.Assign(ref settingtemplate, "{ckbDefault}", setting.IsDefault ? "checked" : string.Empty);
            TemplateHandler.Assign(ref settingtemplate, "{sporttypeopts}", sporttypeOptions);
            TemplateHandler.Assign(ref settingtemplate, "{bettypeopts}", bettypeOptions); 
            TemplateHandler.Assign(ref settingtemplate, "{ckbAutoResetDaily}", setting.SpecialRequirement == "1" ? "checked" : string.Empty);


            TemplateHandler.Assign(ref settingtemplate, "{target-sporttype}", string.Format(IdKey, "sport-", setting.SettingName.Replace(" ", string.Empty), setting.SportType,  sportName.Replace(" ", string.Empty), string.Empty, string.Empty));
            TemplateHandler.Assign(ref settingtemplate, "{setting-type}", setting.SettingType);
            TemplateHandler.Assign(ref settingtemplate, "{target-bettype}", string.Format(IdKey,"bettype-", setting.SettingName.Replace(" ", string.Empty), setting.SportType, sportName.Replace(" ", string.Empty), setting.BetType, bettypename.Replace(" ", string.Empty)));


            TemplateHandler.Assign(ref settingtemplate, "{status-super}", setting.DefaultValueSuper == "1"? "<p class=\"setting-status status-enable\">Enable</p>" : "<p class=\"setting-status status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{status-master}", setting.DefaultValueMaster == "1" ? "<p class=\"setting-status status-enable\">Enable</p>" : "<p class=\"setting-status status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{status-agent}", setting.DefaultValueAgent == "1" ? "<p class=\"setting-status status-enable\">Enable</p>" : "<p class=\"setting-status status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{status-member}", setting.DefaultValueMember == "1" ? "<p class=\"setting-status status-enable\">Enable</p>" : "<p class=\"setting-status status-disable\">Disable</p>");
            TemplateHandler.Assign(ref settingtemplate, "{txtHeadline}", setting.SpecialRequirement);

            TemplateHandler.Assign(ref settingtemplate, "{comSuper}", setting.DefaultValueSuper);
            TemplateHandler.Assign(ref settingtemplate, "{comMaster}", setting.DefaultValueMaster);
            TemplateHandler.Assign(ref settingtemplate, "{comAgent}", setting.DefaultValueAgent);
            TemplateHandler.Assign(ref settingtemplate, "{comMember}", setting.DefaultValueMember);

            return settingtemplate;
        }
        
    }
}
