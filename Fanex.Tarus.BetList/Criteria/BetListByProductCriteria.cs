namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using NAP.Framework.Data;

    public class BetListByProductCriteria : CriteriaBase
    {
        private string _gameId;
        private string _username;
        private string _betId;

        public string DeniedList { get; set; }

        public byte ProductId { get; set; }

        public int CustId { get; set; }

        public string Username
        {
            get { return _username ?? ""; }
            set { _username = value; }
        }

        public string GameId
        {
            get { return _gameId ?? ""; }
            set { _gameId = value; }
        }

        public string BetId
        {
            get { return _betId ?? ""; }
            set { _betId = value; }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string Permission { get; set; }

        public override string GetSettingKey()
            => "BetList_SelByProduct";

        public override bool IsValid()
            => true;
    }
}