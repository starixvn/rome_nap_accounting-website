namespace Fanex.Tarus.Statements.Models
{
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class ExcelCellRange : ExcelCell
    {
        public ExcelCellRange(int firstRow, int lastRow, int firstColumn, int lastColumn, object cellValue, ICellStyle cellStyle)
            : base(firstColumn, cellValue, cellStyle)
        {
            FirstRow = firstRow;
            LastRow = lastRow;
            FirstColumn = firstColumn;
            LastColumn = lastColumn;
        }

        protected int FirstRow { get; set; }

        protected int LastRow { get; set; }

        protected int FirstColumn { get; set; }

        protected int LastColumn { get; set; }

        public virtual int RenderCellRange(ISheet sheet, short rowHeight = 0)
        {
            IRow row;
            ICell cell;
            CellRangeAddress cellRangeAddress = new CellRangeAddress(FirstRow, LastRow, FirstColumn, LastColumn);
            sheet.AddMergedRegion(cellRangeAddress);
            for (var i = FirstRow; i <= LastRow; i++)
            {
                row = sheet.CreateRow(i);
                for (var j = FirstColumn; j <= LastColumn; j++)
                {
                    cell = row.CreateCell(j);
                    if (i == FirstRow || i == LastRow || j == FirstColumn || j == LastColumn)
                    {
                        cell.CellStyle = CellStyle;
                    }

                    if (i == FirstRow && j == FirstColumn)
                    {
                        SetCellValue(cell);
                    }
                }
            }

            return ++LastColumn;
        }
    }
}
