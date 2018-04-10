// --------------------------------------------------------------------------
// <copyright file="BetListExtension.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.12.11">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Extensions
{
    using Constants;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Models;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using Resources;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class BetListExtension
    {
        /// <summary>
        /// Builds the stake.
        /// </summary>
        /// <param name="betDetail">The bet detail.</param>
        /// <param name="builder">The builder.</param>
        /// <returns>The stake.</returns>
        public static string BuildStake(this TicketDetail betDetail, ref BetListHTMLBuilder builder)
        {
            string stakeBuilder = builder.BuildStake(betDetail.Ticket);

            if (betDetail.IsShowOtExRate)
            {
                stakeBuilder +=
                    "<br />" +
                    "<span class=\"r exrate2\">" + betDetail.VirtualRate + "</span>" +
                    "<br />" +
                    "<span class=\"r exrate2\">(" + BetListResources.Ex_Rate_2 + ")</span>";
            }

            return stakeBuilder;
        }

        public static BetListExcelBuilder AddCreator(this BetListExcelBuilder factory, TicketDetail ticketDetail, ITicketHelper ticketHelper)
        {
            IRichTextString rtfCreator = new HSSFRichTextString(ticketDetail.Creator + "\n" + ticketDetail.Ticket.IP);
            factory.AddCell(rtfCreator, ticketHelper.Index % 2 == 0 ? factory.OddCellStyleCenterAligned : factory.EvenCellStyleCenterAligned);

            return factory;
        }

        /// <summary>
        /// Adds the status statement.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticketDetail">The ticket detail.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <returns>The status statement.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static BetListExcelBuilder AddStatusStatement(
            this BetListExcelBuilder factory,
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData)
        {
            var excelFactory = new BetListExcelFactory();
            var builder = excelFactory.CreateBuilder();
            ticketHelper.IsVnIp = (ip) => true;

            IRichTextString rtfCreator = builder
                .CreateStatusBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, ticketHelper, ticketData, false, builder.RTFHelper);

            factory.AddCell(rtfCreator, ticketHelper.Index % 2 == 0 ? factory.OddCellStyleCenterAligned : factory.EvenCellStyleCenterAligned);
            return factory;
        }

        /// <summary>
        /// Gets the name of the score status.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <returns>The name of the score status.</returns>
        public static string GetScoreStatusName(int score)
        {
            switch (score)
            {
                case ScoreStatusConstants.Won:
                    return BetListResources.Won.ToUpper();

                case ScoreStatusConstants.Lose:
                    return BetListResources.Lose.ToUpper();

                case ScoreStatusConstants.Refund:
                    return BetListResources.Refund.ToUpper();
            }

            return string.Empty;
        }

        /// <summary>
        /// Adds the header statement.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="isShowWinLost">If set to <c>true</c> [is show win lost].</param>
        /// <param name="isShowSuperPT">If set to <c>true</c> [is show super position taking].</param>
        /// <param name="isShowMasterPT">If set to <c>true</c> [is show master position taking].</param>
        /// <param name="isShowAgentPT">If set to <c>true</c> [is show agent position taking].</param>
        /// <param name="isShowMember">If set to <c>true</c> [is show member].</param>
        /// <returns>The header statement.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static BetListExcelBuilder AddHeaderStatement(
            this BetListExcelBuilder factory,
            bool isShowWinLost,
            bool isShowSuperPT,
            bool isShowMasterPT,
            bool isShowAgentPT,
            bool isShowMember)
        {
            ICellStyle headerStyle = factory.CreateCellStyle();
            headerStyle.CloneStyleFrom(factory.EvenCellStyle);
            headerStyle.FillForegroundColor = 44;
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.SetFont(factory.RTFHelper.PosFont);

            CellAddress cellAddr;
            cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.No), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 1200);

            cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Trans_Time), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 5400);

            cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Choice), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 15500);

            cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Odds), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 2400);

            cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Stake), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 2400);

            if (isShowWinLost)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Win_Loss), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
            }

            cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Status), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);

            if (isShowSuperPT)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Super + " " + BetListResources.PT + " / " + BetListResources.WinLoss), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
            }

            if (isShowMasterPT)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Master + " " + BetListResources.PT + " /  " + BetListResources.WinLoss), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
            }

            if (isShowAgentPT)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Agent + " " + BetListResources.PT + " /  " + BetListResources.WinLoss), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
            }

            if (isShowMember)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetListResources.Operator), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 4800);
            }

            return factory;
        }

        /// <summary>
        /// Add footer for excel file.
        /// </summary>
        /// <param name="factory">Factory object Excel builder.</param>
        /// <param name="betlistDetail">Object bet list details.</param>
        /// <returns>Return object Bet list excel builder.</returns>
        public static BetListExcelBuilder AddFooter(
            this BetListExcelBuilder factory,
            BetListDetail betlistDetail)
        {
            ICellStyle style = factory.CreateCellStyle();
            style.CloneStyleFrom(factory.OddCellStyle);
            style.FillForegroundColor = (short)47;
            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.SetFont(factory.RTFHelper.PosFont);

            factory.AddCell(new HSSFRichTextString("Total"), style);
            factory.AddCell(new HSSFRichTextString("Total"), style);
            factory.AddCell(new HSSFRichTextString("Total"), style);
            factory.AddCell(new HSSFRichTextString("Total"), style);
            factory.AddCell(new HSSFRichTextString("Total"), style);

            CellRangeAddress cellRange = new CellRangeAddress(factory.LastContentRow, factory.LastContentRow, 0, 4);

            factory.AddCell(new HSSFRichTextString("\n" + betlistDetail.TotalWinLoss.Format(2) + "\n"), style);
            factory.AddCell(new HSSFRichTextString("\n" + betlistDetail.TotalLicenseeWinLoss.Format(2) + "\n"), style);
            factory.AddCell(new HSSFRichTextString(string.Empty), style);

            factory.MergeCell(cellRange, style);

            return factory;
        }

        /// <summary>
        /// Set value for cell in excel file.
        /// </summary>
        /// <param name="factory">Factory object Excel builder.</param>
        /// <param name="value">Value need to set at cell.</param>
        /// <param name="alignment">Set alignment style.</param>
        /// <param name="font">Set font style.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        /// <returns>Return object Bet list excel builder.</returns>
        public static BetListExcelBuilder AddValue(
            this BetListExcelBuilder factory,
            object value,
            HorizontalAlignment alignment,
            IFont font,
            ITicketHelper ticketHelper)
        {
            ICellStyle style = factory.CreateCellStyle();
            style.CloneStyleFrom(ticketHelper.Index % 2 == 0 ? factory.OddCellStyleCenterAligned : factory.EvenCellStyleCenterAligned);
            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.SetFont(font);

            string valueHtml = value.Format(2) + "\n";
            IRichTextString rtfCreator = new HSSFRichTextString(valueHtml);
            factory.AddCell(rtfCreator, style);

            return factory;
        }

        #region For Cash Out

        private static string BuildCashOutStatus(ITicket ticket)
        {
            string statusLabel = string.Empty;
            string upperStatus = ticket.Status.ToUpperInvariant();

            if (ticket.BetTypeId == BetTypeContants.CashOut)
            {
                statusLabel = BuildCashOutStatus(ticket, BetListResources.CashOut, upperStatus);
            }
            else if (ticket.BetTypeId == BetTypeContants.CashOutBought)
            {
                statusLabel = BuildCashOutStatus(ticket, BetListResources.Bought, upperStatus);
            }
            else if (IsCashOutOriginalTicket(ticket))
            {
                if (upperStatus != "VOID" && upperStatus != "REFUND")
                {
                    statusLabel = BuildCashOutStatus(ticket, BetListResources.CashOut);
                }
            }

            return statusLabel;
        }

        private static string BuildCashOutStatus(ITicket ticket, string cashOutLabel, string upperStatus = null)
        {
            string statusLabel = string.Empty;

            if (upperStatus == null || upperStatus == "WON" || upperStatus == "LOSE" || upperStatus == "DRAW")
            {
                statusLabel = cashOutLabel;
            }
            else if (upperStatus == "VOID" || upperStatus == "REFUND")
            {
                statusLabel = string.Format("{0}\n({1})", upperStatus, cashOutLabel);
            }

            return statusLabel;
        }

        private static bool IsCashOutOriginalTicket(ITicket ticket)
        {
            return ticket.BetdaqId == 0 && ticket.Stake == 0 && HasCashOutExtraInfo(ticket.ExtraInfo);
        }

        private static bool HasCashOutExtraInfo(string extraInfo)
        {
            if (!string.IsNullOrEmpty(extraInfo))
            {
                return extraInfo.Trim().StartsWith("<p25 ");
            }

            return false;
        }

        #endregion
    }
}