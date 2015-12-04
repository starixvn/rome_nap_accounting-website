namespace Fanex.Tarus.Statements.Services
{
    using System.Collections.Generic;
    using Fanex.Tarus.Statements.Models;
    using NPOI.SS.UserModel;

    public class DetailStatementExcel : BaseStatementExcel<DetailStatement>
    {
        public DetailStatementExcel(IList<DetailStatement> data, string sheetname = null)
            : base(data, sheetname)
        {
        }

        protected override void RenderTitle(string title)
        {
            int[] columnsWidth = new int[] { 180, 160, 400, 130, 130, 130, 180 };
            NpoiExcelUtils.InitColumnWidthByFontWidth(Worksheet, StartColumnIndex, columnsWidth);

            // Title.
            ICellStyle titleStyle = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, true, HorizontalAlignment.Left);
            ExcelCellRange cellrange = new ExcelCellRange(CurrentRowIndex, CurrentRowIndex + 1, StartColumnIndex, columnsWidth.Length - 1, title, titleStyle);
            cellrange.RenderCellRange(Worksheet);
            CurrentRowIndex += 2;

            // One more empty row.
            ExcelCellRange emptyRow = new ExcelCellRange(CurrentRowIndex, CurrentRowIndex, StartColumnIndex, columnsWidth.Length - 1, string.Empty, null);
            CurrentRowIndex++;
        }

        protected override void RenderHeader()
        {
            int columnIndex = StartColumnIndex;
            IRow row = Worksheet.CreateRow(CurrentRowIndex);
            row.Height = 400;
            ICellStyle headerStyle = NpoiExcelUtils.HeaderStyle(HSSFWorkbook);

            ExcelCell cell = new ExcelCell(columnIndex++, "Date", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Ref.no", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Remark", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Debit", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Credit", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Total", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Creator", headerStyle);
            cell.RenderCell(row);

            CurrentRowIndex++;
        }

        protected override void RenderBody()
        {
            if (ReportData == null || ReportData.Count == 0)
            {
                RenderNoDataFound(6);
                return;
            }

            IRow row;
            ExcelCell cell;
            DetailStatement statement;
            int columnIndex, statementLength;

            ICellStyle textCenter = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Center);
            ICellStyle textLeftBold = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, true, HorizontalAlignment.Left);
            ICellStyle negativeNumber = NpoiExcelUtils.DecimalStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false);

            decimal total = 0;
            statementLength = ReportData.Count;
            for (int i = 0; i < statementLength; i++)
            {
                statement = ReportData[i];
                columnIndex = StartColumnIndex;
                row = Worksheet.CreateRow(CurrentRowIndex);

                cell = new ExcelCell(columnIndex++, statement.Date, textCenter);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.RefNo, textCenter);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Remark, textLeftBold);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Debit, negativeNumber);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Credit, negativeNumber);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Total, negativeNumber);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Creator, textCenter);
                cell.RenderCell(row);

                CurrentRowIndex++;
                total += statement.Credit - statement.Debit;
            }

            // Total.
            ICellStyle footerStyle = NpoiExcelUtils.HeaderStyle(HSSFWorkbook);
            ExcelCellRange cellrange = new ExcelCellRange(CurrentRowIndex, CurrentRowIndex, StartColumnIndex, 6, "Total:    " + total.ToString("N4"), footerStyle);
            cellrange.RenderCellRange(Worksheet);
        }

        protected override void RenderFooter()
        {
        }
    }
}
