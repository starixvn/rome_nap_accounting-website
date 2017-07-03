using System.Collections;
using Fanex.Tarus.Admin.Enums;

namespace Fanex.Tarus.Admin.Criteria.SiteManagement
{
    public class AddSiteCriteria : CriteriaBase
    {
        public int Type { get; set; }

        public string SiteName { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string GetSettingKey()
        {
            return "NAP_Admin_AddNewSite";
        }

        public Hashtable GetSPParams()
        {
            return new Hashtable
            {
                ["isdeposit"] = Type == SiteType.Deposit,
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
