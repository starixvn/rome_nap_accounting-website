using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report;

namespace Sunplus.Accounting.Services.ReportServices
{
    /// <summary>
    /// Summary description for WinLossReportService
    /// </summary>
    public class MemberWinLossReportService : IWinLossService<MemberWinLoss>
    {
        #region IWinLossService<MemberWinLoss> Members

        public void AppendTotalEntity(ref List<MemberWinLoss> list)
        {
            MemberWinLoss lastItem = CreateTotalRow(list);

            list.Insert(list.Count, lastItem);
        }

        #endregion IWinLossService<MemberWinLoss> Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static MemberWinLoss CreateTotalRow(List<MemberWinLoss> list)
        {
            var lastItem = new MemberWinLoss
                               {
                                   Username = "Total",
                                   CustId = -1,
                                   Member = new WinLossComm(),
                                   Agent = new WinLossComm()
                               };

            list.ForEach(item =>
                             {
                                 //member
                                 lastItem.Member.Comm += item.Member.Comm;
                                 lastItem.Member.WinLoss += item.Member.WinLoss;
                                 lastItem.Member.Total += item.Member.Total;

                                 //agent
                                 lastItem.Agent.Comm += item.Agent.Comm;
                                 lastItem.Agent.WinLoss += item.Agent.WinLoss;
                                 lastItem.Agent.Total += item.Agent.Total;

                                 //upline
                                 lastItem.UplineTotal += item.UplineTotal;
                                 lastItem.GrossComm += item.GrossComm;
                                 lastItem.Turnover += item.Turnover;
                             });
            return lastItem;
        }
    }
}