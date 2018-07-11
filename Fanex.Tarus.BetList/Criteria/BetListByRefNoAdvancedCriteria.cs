namespace Fanex.Tarus.BetList.Criteria
{
    using System;

    public class BetListByProductParam
    {
        private string _gameId;
        private string _username;
        private string _betId;

        public long UserId { get; set; }

        public byte ProductId { get; set; }

        public int VendorId { get; set; }

        public int CustId { get; set; }

        public string PlayerId3rd { get; set; }

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
        public byte SiteType { get; set; }

    }
}