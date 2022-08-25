namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    using Fanex.NAP.Framework.Data;

    public class InsertMemberCriteria : CriteriaBase
    {
        public int Recommend { get; set; }

        public string Username { get; set; }

        public int Creator { get; set; }

        public string Userpwd { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public  int SubAccID { get; set; }

        public int CustId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NewLicensee_AdNewMember";

        public override bool IsValid()
            => Recommend > 0 && !string.IsNullOrWhiteSpace(Username) && Creator > 0; 
    }
}
