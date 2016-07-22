using System;
using System.Collections;
using Fanex.Tarus.Admin.Enums;

namespace Fanex.Tarus.Admin.Criteria.SMAManagement
{
    public class UpdateHeadlineTypeCriteria
    {
        public int HeadlineId { get; set; }

        public string HeadlineType { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string GetSettingKey()
        {
            return "Acc_CustomerUpdGroupHeadlineType";
        }

        public Hashtable GetSPParams()
        {
            return new Hashtable
            {
                { "headlineid", HeadlineId },
                { "headlinetype", (int)Enum.Parse(typeof(HeadlineType), HeadlineType, true) },
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
