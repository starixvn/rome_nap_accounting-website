namespace Fanex.Tarus.Customer.Requests.ParlayMaxPayout
{
    using Fanex.Tarus.Customer.ViewModels.ParlayMaxPayout;
    using Fanex.Tarus.Infrastructure.Helper;
    using Sunplus.Accounting.Core.Common;
    using System.Collections.Generic;

    public class ParlayMaxPayoutUpdateRequest
    {
        public int CustId { get; set; }

        public int SubAccId => 0;

        public int UserId => AppSession.AccId;

        public string IP => Utils.GetClientIP();

        public byte SystemId => AccCommon.SystemId;

        public List<ParlayMaxPayoutUpdateEntityModel> ParlayMaxPayoutList { get; set; }
    }

    public class ParlayMaxPayoutUpdateEntityModel
    {
        public int SportType { get; set; }

        public int Bettype { get; set; }

        public decimal SingleMatchParlay { get; set; }

        public decimal Combo2 { get; set; }

        public decimal Combo35 { get; set; }

        public decimal ComboOver6 { get; set; }
    }
}
