namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class SportModel
    {
        public SportModel(int sportId, string sportName)
        {
            SportId = sportId;
            SportName = sportName;
            Bettypes = new List<BettypeModel>();
        }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public List<BettypeModel> Bettypes { get; set; }
    }
}
