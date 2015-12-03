namespace Fanex.Tarus.Statements.Services
{
    using System.Collections.Generic;
    using Fanex.Tarus.Statements.Models;
    using NPOI.SS.UserModel;

    public class AuditStatementExcel : BaseStatementExcel<AuditStatement>
    {
        public AuditStatementExcel(IList<AuditStatement> data, string sheetname = null)
            : base(data, sheetname)
        {
        }

        protected override void RenderTitle(string title)
        {
            int[] columnsWidth = new int[] { 250, 300, 100, 180, 180 };
            NpoiExcelUtils.InitColumnWidthByFontWidth(Worksheet, StartColumnIndex, columnsWidth);

            // Title.
            ICellStyle titleStyle = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, true, HorizontalAlignment.Left);
            ExcelCellRange cellrange = new ExcelCellRange(CurrentRowIndex, CurrentRowIndex, StartColumnIndex, columnsWidth.Length - 1, title, titleStyle);
            cellrange.RenderCellRange(Worksheet, 400);
            CurrentRowIndex++;

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

            cell = new ExcelCell(columnIndex++, "Type", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Total", headerStyle);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, "Accumulated Total", headerStyle);
            cell.RenderCell(row);

            CurrentRowIndex++;
        }

        protected override void RenderBody()
        {
            if (ReportData == null || ReportData.Count == 0)
            {
                RenderNoDataFound(4);
                return;
            }

            IRow row;
            ExcelCell cell;
            AuditStatement statement;
            bool even = false;
            int columnIndex, statementLength;

            ICellStyle textCenter = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Center);
            ICellStyle evenTextCenter = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.EvenRowColorIndex, false, HorizontalAlignment.Center);

            ICellStyle textLeft = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Left);
            ICellStyle evenTextLeft = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.EvenRowColorIndex, false, HorizontalAlignment.Left);

            ICellStyle number = NpoiExcelUtils.DecimalStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false, false);
            ICellStyle evenNumber = NpoiExcelUtils.DecimalStyle(HSSFWorkbook, NpoiExcelUtils.EvenRowColorIndex, false, false);

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

                cell = new ExcelCell(columnIndex++, statement.Remark, even ? evenTextLeft : textLeft);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Type, even ? evenTextCenter : textCenter);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.Total, even ? evenNumber : number);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, statement.AccumulatedTotal, even ? evenNumber : number);
                cell.RenderCell(row);

                CurrentRowIndex++;
            }
        }

        protected override void RenderFooter()
        {
        }
    }
}
