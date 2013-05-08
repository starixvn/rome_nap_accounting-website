 /// <summary>
/// Summary description for HRGroupDailyCommon
/// </summary>

using Sunplus.Accounting.Core.Common;

namespace BLL.Report.HR.GroupDaily
{
    public class HRGroupDailyCommon
    {
        #region getHRType

        public static string getHRType(int type)
        {
            string partner = "";
            switch (type)
            {
                case 1:
                    partner = "PHU";
                    break;
                case 2:
                    partner = "RUK";
                    break;
                case 3:
                    partner = "ATR";
                    break;
                case 4:
                    partner = "AUS";
                    break;
                default:
                    partner = "Unknow";
                    break;
            }
            return partner;
        }

        #endregion

        #region CalcAmountDueto3rd

        public static decimal CalcAmountDueto3rd(int type, decimal turnover3rd, decimal DividendWon3rd,
                                                 decimal ExpComm3rd, decimal Fees3rd)
        {
            decimal AmountDueto3rd = 0;
            switch (type)
            {
                case 1: //PHU
                    AmountDueto3rd = turnover3rd - DividendWon3rd - ExpComm3rd + Fees3rd;
                    break;
                default:
                    AmountDueto3rd = turnover3rd - DividendWon3rd - ExpComm3rd - Fees3rd;
                    break;
            }
            return AmountDueto3rd;
        }

        #endregion

        /// <summary>
        /// For PHUMELELA: Amount Due to = Turnover - Dividend Won - Comm Expense + Fees
        /// For RUK and ATR: Amount Due to = Turnover - Dividend Won - Comm Expense - Fees
        /// </summary>
        /// <param name="type"></param>
        /// <param name="turnover3rd"></param>
        /// <param name="DividendWon3rd"></param>
        /// <param name="ExpComm3rd"></param>
        /// <param name="Fees3rd"></param>
        /// <returns></returns>

        public static string keyCacheHRGroupDaily = "accrpthrgroupdaily." + AppSession.Username;
        public static string keyCacheHRGroupDaily_ToteOnly = "accrpthrgroupdailytoteonly." + AppSession.Username;
    }
}