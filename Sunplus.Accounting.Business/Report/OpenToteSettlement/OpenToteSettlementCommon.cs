using System;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for OpenToteSettlementCommon
/// </summary>

namespace BLL.Report.OpenToteSettlement
{
    public class OpenToteSettlementCommon
    {
        public static string cachekeyreport = "accrptopentotesettlementreport." + AppSession.Username;
        public static string cachekeycompare = "accrptopentotesettlementcomparision." + AppSession.Username;
        public static string cachekeyibcopentotecompare = "accrptopentotesettlementibcopentotecompare" + AppSession.Username;

        public static bool IsMatched(decimal num1, decimal num2)
        {
            decimal result = Math.Abs(num1 - num2);
            return result <= 1;
        }
    }
}