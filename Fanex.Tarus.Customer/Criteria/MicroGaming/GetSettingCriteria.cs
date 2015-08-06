namespace Fanex.Tarus.Customer.Criteria.MicroGaming
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Infrastructure.Constants;

    public class GetSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(SqlParamName = "SportType")]
        public int SportId
        {
            get
            {
                return Sports.MicroGaming;
            }
        }

        [SpParam(Ignored = true)]
        public int ParentId { get; set; }

        [SpParam(Ignored = true)]
        public int RoleId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string Currency { get; set; }

        public string SiteName { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_ThirdParty_GetCustomerSetting";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
