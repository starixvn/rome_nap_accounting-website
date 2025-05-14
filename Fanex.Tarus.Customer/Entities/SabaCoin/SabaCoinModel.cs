namespace Fanex.Tarus.Customer.Entities.SabaCoin
{
    public class SabaCoinModel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public int SuperID { get; set; }

        public string SuperName { get; set; }

        public int MasterID { get; set; }

        public string MasterName { get; set; }

        public int AgentID { get; set; }

        public string AgentName { get; set; }

        public byte CustType { get; set; }

        public bool IsRealAccount => CustType == 0;

        public long SabaId { get; set; }
    }
}
