namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class BettypeModel
    {
        public BettypeModel(int bettypeId, string bettypeName)
        {
            BettypeId = bettypeId;
            BettypeName = bettypeName;
            SettingTypes = new List<SettingTypeModel>();
        }

        public int BettypeId { get; set; }

        public string BettypeName { get; set; }

        public List<SettingTypeModel> SettingTypes { get; set; }
    }
}
