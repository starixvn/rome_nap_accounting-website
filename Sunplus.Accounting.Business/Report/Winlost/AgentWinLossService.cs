using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report;

namespace Sunplus.Accounting.Services.ReportServices
{
    /// <summary>
    /// Summary description for AgentWinLossService
    /// </summary>
    public class AgentWinLossService : IWinLossService<AgentWinLoss>
    {
        #region IWinLossService<AgentWinLoss> Members

        public void AppendTotalEntity(ref List<AgentWinLoss> list)
        {
            AgentWinLoss lastItem = CreateTotalItem(list);

            list.Insert(list.Count, lastItem);
        }

        #endregion IWinLossService<AgentWinLoss> Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static AgentWinLoss CreateTotalItem(List<AgentWinLoss> list)
        {
            var lastItem = new AgentWinLoss
                               {
                                   Username = "Total",
                                   CustId = -1,
                                   Member = new WinLossComm(),
                                   Agent = new WinLossComm(),
                                   Master = new WinLossComm()
                               };

            if (list != null)
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

                                     //master
                                     lastItem.Master.Comm += item.Master.Comm;
                                     lastItem.Master.WinLoss += item.Master.WinLoss;
                                     lastItem.Master.Total += item.Master.Total;

                                     //upline
                                     lastItem.UplineTotal += item.UplineTotal;
                                     lastItem.GrossComm += item.GrossComm;
                                     lastItem.Turnover += item.Turnover;
                                 });
            return lastItem;
        }
    }
}