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
            IRow row = sheet.CreateRow(FirstRow);
            if (rowHeight > 0)
            {
                row.Height = rowHeight;
            }

            ICell cell = row.CreateCell(FirstColumn);
            if (CellStyle != null)
            {
                cell.CellStyle = CellStyle;
            }

            SetCellValue(cell);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(FirstRow, LastRow, FirstColumn, LastColumn);
            sheet.AddMergedRegion(cellRangeAddress);
            return ++LastColumn;
        }
    }
}
