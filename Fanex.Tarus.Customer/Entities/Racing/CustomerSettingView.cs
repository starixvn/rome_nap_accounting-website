
namespace Fanex.Tarus.Customer.Entities.Racing
{
    public class CustomerSettingView
    {
        public string TitlePage { get; set; }

        public bool IsSuperSetting { get; set; }

        public bool IsMasterSetting { get; set; }

        public bool IsAgentSetting { get; set; }        

        public bool IsMemberSetting { get; set; }

        public int CustId { get; set; }

        public int UplineCustId { get; set; }

        public string CustUserName { get; set; }

        public bool CanEditCredit { get; set; }
    }
}
