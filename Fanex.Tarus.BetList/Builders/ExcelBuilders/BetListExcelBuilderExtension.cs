namespace Fanex.Tarus.BetList.Builders.ExcelBuilders
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Models;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using PTComm;
    using Resources;

    public static class BetListExcelBuilderExtension
    {
        public static BetListExcelBuilder AddCaption(this BetListExcelBuilder builder, string caption, int columnsNum)
        {
            var cellStyle = builder.CreateCellStyle();
            cellStyle.CloneStyleFrom(builder.OddCellStyle);
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.SetFont(builder.RTFHelper.PosFont);

            var rtfString = new HSSFRichTextString("        " + caption.ToUpper());
            var cellAddress = builder.AddCell(rtfString, cellStyle);
            var mergeRange = new CellRangeAddress(cellAddress.RowIndex, cellAddress.RowIndex, 0, columnsNum - 1);
            builder.MergeCell(mergeRange, cellStyle);

            return builder;
        }

        public static BetListExcelBuilder AddFooterHeader(this BetListExcelBuilder builder, bool isShowMember)
        {
            var rtfHelper = builder.RTFHelper;
            rtfHelper.RTFRenderer.AddText($"{BetListResources.subtotalWinlost}:\n");
            rtfHelper.RTFRenderer.AddText($"{BetListResources.subtotalCom}:\n");
            rtfHelper.RTFRenderer.AddText($"{BetListResources.Total}:");

            var footerHeaderRtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            var cellStyle = CreateFooterCellStyle(builder);
            var cellAddress = builder.AddCell(footerHeaderRtfText, cellStyle);
            var mergeRange = new CellRangeAddress(cellAddress.RowIndex, cellAddress.RowIndex, 0, isShowMember ? 5 : 4);
            builder.MergeCell(mergeRange, cellStyle);

            return builder;
        }

        public static BetListExcelBuilder AddFooter(this BetListExcelBuilder builder, Footer footer, bool isShowMember, int totalColumns)
        {
            var rtfHelper = builder.RTFHelper;
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);
            rtfNumber.Render(footer.SubTotalWinLoss);
            rtfNumber.Render("\n" + footer.SubTotalCommission);
            rtfNumber.Render("\n" + footer.Total);

            var footerRtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            var cellStyle = CreateFooterCellStyle(builder);
            var cellAddress = builder.AddCell(footerRtfText, cellStyle);
            builder.AddCell(new HSSFRichTextString(string.Empty), cellStyle);
            var mergeRange = new CellRangeAddress(cellAddress.RowIndex, cellAddress.RowIndex, isShowMember ? 7 : 6, totalColumns - 1);
            builder.MergeCell(mergeRange, cellStyle);

            return builder;
        }

        public static BetListExcelBuilder AddNo(this BetListExcelBuilder builder, int index)
        {
            var rtfString = new HSSFRichTextString(index.ToString());
            builder.AddCell(rtfString);

            return builder;
        }

        public static BetListExcelBuilder AddWinLoss(this BetListExcelBuilder builder, ITicket ticket)
        {
            var winlossModel = new WinLossModel(ticket);

            var rtfHelper = builder.RTFHelper;
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            if (!string.IsNullOrEmpty(winlossModel.WinLoss))
            {
                rtfNumber.Render(winlossModel.WinLoss + "\n");
            }

            if (winlossModel.ShowComm && !string.IsNullOrEmpty(winlossModel.Comm))
            {
                rtfHelper.RTFRenderer.AddText(winlossModel.Comm, rtfHelper.PosFont);
            }

            var rtfWinloss = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            builder.AddCell(rtfWinloss);

            return builder;
        }

        public static BetListExcelBuilder AddPTComm(this BetListExcelBuilder builder, ITicket ticket, List<ITicketData> ticketData, int roleId)
        {
            var rtfHelper = builder.RTFHelper;
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);
            var ptComm = PTCommFactory.CreateInstance(ticket, ticketData, roleId);

            if (ticket.BetTypeId == 33 || ticket.BetTypeId == 1233)
            {
                RenderRacingPtValue(rtfNumber, rtfHelper, ptComm as PTComm33);
            }
            else
            {
                RenderPTPercent(rtfNumber, ptComm);
            }

            RenderPTValue(rtfNumber, ptComm);
            RenderCommPercent(rtfHelper, ptComm);
            RenderCommValue(rtfHelper, ptComm);

            var rtfPTComm = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            builder.AddCell(rtfPTComm);

            return builder;
        }

        private static void RenderRacingPtValue(RTFNumber rtfNumber, RTFHelper rtfHelper, PTComm33 ptComm)
        {
            rtfHelper.RTFRenderer.AddText($"{BetListResources.win}: ", rtfHelper.PosFont);
            rtfNumber.Render(ptComm.PositionTakingWin + "\n");

            rtfHelper.RTFRenderer.AddText($"{BetListResources.place}: ", rtfHelper.PosFont);
            rtfNumber.Render(ptComm.PositionTakingPlace + "\n");
        }

        private static void RenderPTPercent(RTFNumber rtfNumber, PTComm ptComm)
        {
            if (!string.IsNullOrEmpty(ptComm.PositionTakingPercent) && ptComm.VisibilePositionTakingPercent)
            {
                rtfNumber.Render(ptComm.PositionTakingPercent + "\n");
            }
        }

        private static void RenderPTValue(RTFNumber rtfNumber, PTComm ptComm)
        {
            if (!string.IsNullOrEmpty(ptComm.PositionTaking) && ptComm.VisiblePositionTaking)
            {
                rtfNumber.Render(ptComm.PositionTaking + "\n");
            }
        }

        private static void RenderCommPercent(RTFHelper rtfHelper, PTComm ptComm)
        {
            if (!string.IsNullOrEmpty(ptComm.CommissionPercent) && ptComm.VisibileCommisionPercent)
            {
                rtfHelper.RTFRenderer.AddText(ptComm.CommissionPercent + "\n");
            }
        }

        private static void RenderCommValue(RTFHelper rtfHelper, PTComm ptComm)
        {
            if (!string.IsNullOrEmpty(ptComm.Commission) && ptComm.VisibleCommision)
            {
                rtfHelper.RTFRenderer.AddText(ptComm.Commission);
            }
        }

        private static ICellStyle CreateFooterCellStyle(IBetListExcelBuilder builder)
        {
            var cellStyle = builder.CreateCellStyle();
            cellStyle.CloneStyleFrom(builder.EvenCellStyle);
            cellStyle.FillForegroundColor = 47;
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.SetFont(builder.RTFHelper.PosFont);

            return cellStyle;
        }
    }
}
