namespace Fanex.Tarus.Statements.Services
{
    using System.Collections.Generic;
    using Fanex.Tarus.Statements.Models;
    using NPOI.SS.UserModel;

    public class OldStatementExcel : BaseStatementExcel<OldStatement>
    {
        public OldStatementExcel(IList<OldStatement> data, string sheetname = null)
            : base(data, sheetname)
        {
        }

        protected override void RenderTitle(string title)
        {
            int[] columnsWidth = new int[] { 250, 350, 160, 160, 180, 180, 180 };            
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

            cell = new ExcelCell(columnIndex++, "Remark", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Total", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Accumulated Total", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Creator", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Verified by", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Checked Time", headerStyle);
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
            OldStatement statement;
            bool even = false;
            int columnIndex, statementLength;

            ICellStyle textCenter = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Center);
            ICellStyle evenTextCenter = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.EvenRowColorIndex, false, HorizontalAlignment.Center);

            ICellStyle textLeftBold = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, true, HorizontalAlignment.Left);
            ICellStyle evenTextLeftBold = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.EvenRowColorIndex, true, HorizontalAlignment.Left);

            ICellStyle textRight = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Right);
            ICellStyle evenTextRight = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.EvenRowColorIndex, false, HorizontalAlignment.Right);

            ICellStyle negativeNumber = NpoiExcelUtils.DecimalStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false);
            ICellStyle evenNegativeNumber = NpoiExcelUtils.DecimalStyle(HSSFWorkbook, NpoiExcelUtils.EvenRowColorIndex, false);

            NpoiExcelUtils.SetColorAtIndex(HSSFWorkbook, NpoiExcelUtils.EvenRowColor, NpoiExcelUtils.EvenRowColorIndex);

            statementLength = ReportData.Count;
            for (int i = 0; i < statementLength; i++)
            {
                statement = ReportData[i];
                even = i % 2 == 0;
                columnIndex = StartColumnIndex;
                row = Worksheet.CreateRow(CurrentRowIndex);

                cell = new ExcelCell(columnIndex++, statement.Date, even ? evenTextCenter : textCenter);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Remark, even ? evenTextLeftBold : textLeftBold);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Total, even ? evenNegativeNumber : negativeNumber);
                cell.RenderCell(row);

                if (!string.IsNullOrEmpty(statement.AccumulatedTotalText))
                {
                    cell = new ExcelCell(columnIndex++, statement.AccumulatedTotalText, even ? evenTextRight : textRight);
                }
                else
                {
                    cell = new ExcelCell(columnIndex++, statement.AccumulatedTotal, even ? evenNegativeNumber : negativeNumber);
                }

                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Creator, even ? evenTextCenter : textCenter);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.VerifiedBy, even ? evenTextCenter : textCenter);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.CheckedTime, even ? evenTextCenter : textCenter);
                cell.RenderCell(row);

                CurrentRowIndex++;
            }
        }

        protected override void RenderFooter()
        {
        }
    }
}
