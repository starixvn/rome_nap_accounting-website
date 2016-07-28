using System.Collections;

namespace Fanex.Tarus.Admin.Criteria.SMAManagement
{
    public class UpdateOwnerCriteria
    {
        public int HeadlineId { get; set; }

        public string OwnerName { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string GetSettingKey()
        {
            return "Acc_CustomerUpdGroupHeadlineOwner";
        }

        public Hashtable GetSPParams()
        {
            return new Hashtable
            {
                { "headlineid", HeadlineId },
                { "ownername", OwnerName ?? string.Empty },
                { "userid", UserId },
                { "ip", IP },
                { "errcode", ErrCode },
                { "errmsg", ErrMsg }
            };
        }

        public bool IsValid()
        {
            return HeadlineId > 0;
        }
    }
}
