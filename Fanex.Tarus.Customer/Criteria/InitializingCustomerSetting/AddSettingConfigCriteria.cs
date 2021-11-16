using System.Collections.Generic;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;

namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting
{
    public class AddSettingConfigCriteria : CriteriaBase
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

        public AddSettingConfigCriteria(ConfigModel request)
        {
            ProductID = request.Config.ProductId;
            SportTypeList = string.Join(",", request.Config.SportTypeIdList);
            BetTypeList = string.Join(",", request.Config.BetTypeIdList);
            Site = string.Join(",", request.Config.SiteIdList);
            Currency = string.Join(",", request.Config.CurrencyIdList);
            StartTime = request.Config.StartTime;
            IsTest = request.Config.IsTest ? 1 : 0;
            Remark = request.Config.Remark;
            XMLConfig = BuildXmlConfig(request.Settings);
        }

        public override string GetSettingKey()
            => "Acc_Initial_DCS_SettingConfig_Ins";

        private string BuildXmlConfig(List<SettingInforModel> settings)
        {
            var xmlConfig = new StringBuilder();
            xmlConfig.Append("<ROOT>");
            for (int i = 0; i < settings.Count; i++)
            {
                var setting = settings[i];
                if(!setting.IsCopy && !setting.IsDefault)
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
                if(!setting.IsDefault && setting.CopyFromSportType > 0) xmlConfig.Append("CopyFromSportType = \"" + setting.CopyFromSportType + "\" ");
                if (!setting.IsDefault && setting.CopyFromBetType > 0) xmlConfig.Append("CopyFromBetType = \"" + setting.CopyFromBetType + "\" ");
                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueSuper)) xmlConfig.Append("DefaultValueSuper = \"" + setting.DefaultValueSuper + "\" ");
                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueMaster)) xmlConfig.Append("DefaultValueMaster = \"" + setting.DefaultValueMaster + "\" ");
                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueAgent)) xmlConfig.Append("DefaultValueAgent = \"" + setting.DefaultValueAgent + "\" ");
                if (setting.IsDefault && !string.IsNullOrEmpty(setting.DefaultValueMember)) xmlConfig.Append("DefaultValueMember = \"" + setting.DefaultValueMember + "\" ");
                if ((setting.IsDefault && !string.IsNullOrEmpty(setting.SpecialRequirement)) || setting.IsRequiredSpecialRequirement) xmlConfig.Append("SpecialRequirement = \"" + setting.SpecialRequirement + "\" ");
                xmlConfig.Append("/>");
            }

            xmlConfig.Append("</ROOT>");

            return xmlConfig.ToString();
        }
    }
}
