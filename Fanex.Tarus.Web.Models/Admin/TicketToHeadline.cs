using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Web.Models.Admin
{
    public class TicketToHeadline
    {
        public int RowNum { get; set; }

        public string GroupHeadline { get; set; }

        public int MergeRowNumGroupHeadline { get; set; }

        public string HeadlineType { get; set; }

        public int MergeRowNumHeadlineType
        {
            get
            {
                return MergeRowNumGroupHeadline;
            }
        }

        public string Currency { get; set; }

        public int MergeRowNumCurrency
        {
            get
            {
                return MergeRowNumSuper;
            }
        }

        public string Super { get; set; }

        public int MergeRowNumSuper { get; set; }

        public string Master { get; set; }

        public int MergeRowNumMaster { get; set; }

        public string Agent { get; set; }

        public int MergeRowNumAgent { get; set; }

        public string Member { get; set; }

        public string RefNo { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage
        {
            get
            {
                switch (ErrorCode)
                {
                    case 1:
                        return "Deposit Customer";
                    case 2:
                        return "Invalid Ref No";
                    case 3:
                        return "Invalid Username";
                    case 4:
                        return "Mismatched Username and Ref No";
                    default:
                        return string.Empty;
                }
            }
        }

        public void Clone(TicketToHeadline source)
        {
            this.Agent = source.Agent;
            this.Currency = source.Currency;
            this.ErrorCode = source.ErrorCode;
            this.GroupHeadline = source.GroupHeadline;
            this.HeadlineType = source.HeadlineType;
            this.Master = source.Master;
            this.Member = source.Member;
            this.RefNo = source.RefNo;
            this.RowNum = source.RowNum;
            this.Super = source.Super;
        }
    }
}
