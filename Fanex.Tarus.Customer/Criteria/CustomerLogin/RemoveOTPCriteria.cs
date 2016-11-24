namespace Fanex.Tarus.Customer.Criteria.CustomerLogin
{
    using System.Diagnostics.CodeAnalysis;
    using NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class RemoveOTPCriteria : CriteriaBase
    {
        public int Custid { get; set; }

        public int Userid { get; set; }

        public string Language
        {
            get
            {
                return "en";
            }
        }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        public override string GetSettingKey()
        {
            return "RemoveCustomerOTP";
        }
    }
}
