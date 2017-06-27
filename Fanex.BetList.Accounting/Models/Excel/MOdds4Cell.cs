using System;
using NPOI.SS.UserModel;

namespace Fanex.BetList.Accounting.Models.Excel
{
    public class MOdds4Cell : ExcelCell
    {
        public MOdds4Cell(int columnIndex, object cellValue, ICellStyle cellStyle)
            : base(columnIndex, cellValue, cellStyle)
        {
        }

        public override int RenderCell(IRow row)
        {
            var cell = row.CreateCell(ColumnIndex);
            cell.SetCellValue(Convert.ToDouble(CellValue));
            cell.CellStyle = CellStyle;

            return ++ColumnIndex;
        }
    }
}
