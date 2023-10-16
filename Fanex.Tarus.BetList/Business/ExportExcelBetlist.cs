namespace Fanex.Tarus.BetList.Business
{
    using BetListNPOI.SS.UserModel;
    using Builders.ExcelBuilders;
    using Constants;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
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

                    builder.AddPlatform(ticketHelper, betdetail.Platform);

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
                resultWorkbook.Write(ms, true);
                return ms;
            }


            return null;
        }
    }
}
