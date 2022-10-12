namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using System.Collections.Generic;
    using System.Text;

    public class AddDepositSettingConfigCriteria : AddSettingConfigCriteria
    {
        public string BetSettingXML { get; set; }

        public AddDepositSettingConfigCriteria(ConfigModel request, string ip) : base(request, ip)
        {
            BetSettingXML = BuildBetSettingXML(request.BetSettings);
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

        protected override string BuildXmlConfig(List<SettingInforModel> settings)
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
