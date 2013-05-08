
namespace Fanex.Tarus.Finance.Entities.Statement
{
    public class FinaceStatementParams
    {
        public string RefNo { get; set; }

        public string CustName { get; set; }

        public string FilterBy { get; set; } // WinLossDate or TransDate

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string BalanceUpDay { get; set; }

        public int IncludeAccBal { get; set; } // : default 0.
        // Ref No: 0 . Not have Accumulated because it have many custid
        // Username: 0 or 1: 1 if FromDate < BalanceUpDay
    }
    public class VerifyParams
    {
        public string RefNo { get; set;}

        public bool IsVerify { get; set; }
    }
}
