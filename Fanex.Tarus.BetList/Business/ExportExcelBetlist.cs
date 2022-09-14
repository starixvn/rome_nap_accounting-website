namespace Fanex.Tarus.BetList.Business
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Builders.ExcelBuilders;
    using Constants;
    using Criteria;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.BetList.Core;
    using Models;
    using NPOI.SS.UserModel;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.DataAccess.Report;
    using AccBetListCore = Fanex.Tarus.BetList.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class ExportExcelBetlist
    {
        public MemoryStream ExportExcelBetListStatement(
            int custId,
            int transType,
            DateTime fromDate,
            ref string fileName)
        {
            var outputLiveCasino = new List<ITicketData>();
            var ticketHelper = new TicketHelper();

            GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;
            var factory = new BetListExcelFactory();
            var builder = factory.CreateBuilder();
            var tickets = new List<ITicket>();
            var betDetails = AccBetListCore.SessionManager.Get<IList<TicketDetail>>(BetListConstants.BetListByStatementSessionKey);
            if (betDetails != null && betDetails.Count > 0)
            {
                string reportTitle = "Daily Statement - " + fromDate.ToShortDateString() + " - " + betDetails[0].Ticket.UserName;
                fileName = "DailyStatement-" + fromDate.ToShortDateString() + "-" + betDetails[0].Ticket.UserName + ".xls";
                int index = 0;
                ticketHelper.HideStatusCashOutHistory = true;

                builder.BuildWorksheet("BetList", 8);
                builder.AddCaption(reportTitle, 8);
                builder.AddHeaderStatement(true, false, false, false, true);
                foreach (var betdetail in betDetails)
                {
                    index++;
                    ticketHelper.Index = index;
                    builder.AddNo(index);

                    builder.AddTrans(betdetail.Ticket, ticketHelper);

                    builder.AddChoice(betdetail.Ticket, ticketHelper, outputLiveCasino)
                           .AddOdds(betdetail.Ticket, outputLiveCasino, funcGetOddsTypeName)
                           .AddStake(betdetail.Ticket, ticketHelper);

                    builder.AddWinLoss(betdetail.Ticket);

                    builder.AddStatusStatement(betdetail.Ticket, ticketHelper, outputLiveCasino);

                    builder.AddCreator(betdetail, ticketHelper);
                    tickets.Add(betdetail.Ticket);
                }

                if (tickets.Count > 0)
                {
                    builder.AddFooterHeader(isShowMember: false);
                    builder.AddFooter(new Footer(tickets), false, 8);
                }

                var ms = new MemoryStream();
                IWorkbook resultWorkbook = builder.GetBetList();
                resultWorkbook.Write(ms);
                return ms;
            }


            return null;
        }
    }
}
