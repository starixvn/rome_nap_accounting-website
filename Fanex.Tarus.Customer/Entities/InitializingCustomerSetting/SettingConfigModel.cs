namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class SettingConfigModel
    {
        public int ConfigId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public IEnumerable<int> SportTypeIdList { get; set; }

        public IEnumerable<string> SportTypeList { get; set; }

        public IEnumerable<int> BetTypeIdList { get; set; }

        public IEnumerable<string> BetTypeList { get; set; }

        public IEnumerable<int> SiteIdList { get; set; }

        public IEnumerable<string> SiteList { get; set; }

        public IEnumerable<int> CurrencyIdList { get; set; }

        public IEnumerable<string> CurrencyList { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsTest { get; set; }

        public int Status { get; set; }

        public string ProcessedSteps { get; set; }

        public string CurrentStep { get; set; }

        public string Remark { get; set; }

        public bool IsExistBetSetting { get; set; }

        public SettingConfigModel()
        {
            SportTypeIdList = new List<int>();
            BetTypeIdList = new List<int>();
            SiteIdList = new List<int>();
            CurrencyIdList = new List<int>();
        }
    }
}
