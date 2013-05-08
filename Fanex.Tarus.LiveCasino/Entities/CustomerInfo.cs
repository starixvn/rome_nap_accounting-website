
namespace Fanex.Tarus.LiveCasino.Entities
{
    public class CustomerInfoDTO
    {
        public int CustId { get; set; }
        public string CustName { get; set; }
        public int ExchangeId { get; set; }
        public int SuperId { get; set; }
        public int MasterId { get; set; }
        public int AgentId { get; set; }
        public int RoleId { get; set; }
        public string Ip { get; set; }
        public bool DisableLiveCs { get; set; }
        public bool IsEnableDownLines { get; set; }
        public bool SyncLiveCasino { get; set; }
        public int SubAccountId { get; set; }
        public string Language { get; set; }
        public string CustSite { get; set; }
    }

    public class CustomerStatusInfo
    {
        public bool SyncLiveCasino { get; set; }

        public bool DisableLiveCs { get; set; }

        public bool IsEnableDownLines { get; set; }

        public string CustSite { get; set; }


    }


}
