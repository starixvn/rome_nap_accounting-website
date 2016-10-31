using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Web.Models.Admin
{
    public class TicketToHeadline
    {
        public string GroupHeadline { get; set; }

        public string HeadlineType { get; set; }

        public string Currency { get; set; }

        public string Super { get; set; }

        public string Master { get; set; }

        public string Agent { get; set; }

        public string Member { get; set; }

        public long RefNo { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage {
            get {
                switch (ErrorCode)
                {
                    case 0:
                        return "Deposit Customer";
                    case 1:
                        return "Invalid Username";
                    case 2:
                        return "Invalid Ref No";
                    case 3:
                        return "Mismatched Username and Ref No";
                    default:
                        return "Invalid Username";
                }
            }
        }
    }
}
