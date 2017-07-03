using System.Collections;

namespace Fanex.Tarus.Admin.Criteria.SiteManagement
{
    public class ValidateSiteCriteria : CriteriaBase
    {
        public string SiteName { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string GetSettingKey()
        {
            return "NAP_Admin_VerifySiteName";
        }

        public Hashtable GetSPParams()
        {
            return new Hashtable
            {
                ["sitename"] = SiteName,
                ["errcode"] = ErrCode,
                ["errmsg"] = ErrMsg
            };
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(SiteName);
        }
    }
}
