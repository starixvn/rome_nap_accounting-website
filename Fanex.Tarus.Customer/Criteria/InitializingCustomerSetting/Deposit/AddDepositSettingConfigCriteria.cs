namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    public class AddDepositSettingConfigCriteria : CriteriaBase
    {
        public int ProductID { get; set; }
        public string SportTypeList { get; set; }
        public string BetTypeList { get; set; }
        public string Site { get; set; }
        public string Currency { get; set; }
        public string StartTime { get; set; }
        public int IsTest { get; set; }
        public string Remark { get; set; }
        public string XMLConfig { get; set; }
        public int UserId { get; set; }
        public string IP { get; set; }
        public int IsWarning { get; set; }
        public bool IsConfirmed { get; set; }
        public string BetSettingXML { get; set; }

        public AddDepositSettingConfigCriteria(ConfigModel request, string ip, int userId)
        {
            ProductID = request.Config.ProductId;
            SportTypeList = string.Join(",", request.Config.SportTypeIdList);
            BetTypeList = string.Join(",", request.Config.BetTypeIdList);
            Site = string.Join(",", request.Config.SiteIdList);
            Currency = string.Join(",", request.Config.CurrencyIdList);
            StartTime = request.Config.StartTime;
            IsTest = request.Config.IsTest ? 1 : 0;
            Remark = HttpUtility.HtmlDecode(request.Config.Remark);
            XMLConfig = BuildXmlConfig(request.Settings);
            IP = ip;
            BetSettingXML = BuildBetSettingXML(request.BetSettings);
            UserId = userId;
        }

        public override string GetSettingKey()
         => "Acc_Dep_Initial_DCS_SettingConfig_Ins";

        private string BuildBetSettingXML(List<BetSettingModel> betSettings)
        {
            if(betSettings == null)
            {
                return null;
            }
            var xmlConfig = new StringBuilder();
            xmlConfig.Append("<BS>");
            for (int i = 0; i < betSettings.Count; i++)
            {
                var setting = betSettings[i];

                xmlConfig.Append("<R ");
                xmlConfig.Append("ID = \"" + setting.RowPos + "\" ");
                xmlConfig.Append("SportType = \"" + setting.SportTypeId + "\" ");
                xmlConfig.Append("BetType = \"" + setting.BetTypeId + "\" ");
                xmlConfig.Append("MinBet = \"" + setting.MinBet + "\" ");
                xmlConfig.Append("MaxBet = \"" + setting.MaxBet + "\" ");
                if(setting.MaxPerMatch != null) xmlConfig.Append("MaxPerMatch = \"" + setting.MaxPerMatch + "\" ");
                if (setting.MaxPerNumber != null) xmlConfig.Append("MaxPerNumber = \"" + setting.MaxPerNumber + "\" ");
                if (setting.MaxPayoutPerMatch != null) xmlConfig.Append("MaxPayoutPerMatch = \"" + setting.MaxPayoutPerMatch + "\" ");
                xmlConfig.Append("Currency = \"" + setting.Currency + "\" ");
                xmlConfig.Append("SettingType = \"" + setting.SettingType + "\" ");
                xmlConfig.Append("/>");
            }

            xmlConfig.Append("</BS>");

            return xmlConfig.ToString();
        }

        private string BuildXmlConfig(List<SettingInforModel> settings)
        {
            var xmlConfig = new StringBuilder();
            xmlConfig.Append("<ROOT>");
            for (int i = 0; i < settings.Count; i++)
            {
                var setting = settings[i];
                if (!setting.IsCopy && !setting.IsDefault && setting.SettingName != SettingInforModel.BetSetting
                    || setting.SettingName == SettingInforModel.BetSetting && !setting.IsDefault)
                {
                    continue;
                }

                xmlConfig.Append("<R ");
                xmlConfig.Append("ID = \"" + setting.Id + "\" ");
                xmlConfig.Append("SportType = \"" + setting.SportType + "\" ");
                xmlConfig.Append("BetType = \"" + setting.BetType + "\" ");
                xmlConfig.Append("IsLive = \"" + (setting.IsLive ? "1" : "0") + "\" ");
                xmlConfig.Append("SettingType = \"" + setting.SettingType + "\" ");
                xmlConfig.Append("IsDefault = \"" + (setting.IsDefault ? "1" : "0") + "\" ");

                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueSuper))
                {
                    xmlConfig.Append("DefaultValueSuper = \"" + setting.DefaultValueSuper + "\" ");
                }

                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueMaster))
                {
                    xmlConfig.Append("DefaultValueMaster = \"" + setting.DefaultValueMaster + "\" ");
                }

                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueAgent))
                {
                    xmlConfig.Append("DefaultValueAgent = \"" + setting.DefaultValueAgent + "\" ");
                }

                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueMember))
                {
                    xmlConfig.Append("DefaultValueMember = \"" + setting.DefaultValueMember + "\" ");
                }

                xmlConfig.Append("/>");
            }

            xmlConfig.Append("</ROOT>");

            return xmlConfig.ToString();
        }
    }
}
