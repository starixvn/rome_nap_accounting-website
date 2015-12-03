namespace Fanex.Tarus.Statements.Models
{
    using System;
    using NPOI.SS.UserModel;

    public class ExcelCell
    {
        public ExcelCell()
        { 
        }

        public ExcelCell(int columnIndex, object cellValue, ICellStyle cellStyle)
        {
            ColumnIndex = columnIndex;
            CellValue = cellValue;
            CellStyle = cellStyle;
        }

        protected int ColumnIndex { get; set; }

        protected object CellValue { get; set; }

        protected ICellStyle CellStyle { get; set; }

        public virtual int RenderCell(IRow row)
        {
            ICell cell = row.CreateCell(ColumnIndex);
            SetCellValue(cell);
            cell.CellStyle = CellStyle;

            return ++ColumnIndex;
        }

        protected virtual void SetCellValue(ICell cell)
        {
            if (CellValue is string)
            {
                cell.SetCellValue(Convert.ToString(CellValue));
            }
            else if (CellValue is int)
            {
                cell.SetCellValue(Convert.ToInt32(CellValue));
            }
            else if (CellValue is long)
            {
                cell.SetCellValue(Convert.ToInt64(CellValue));
            }
            else if (CellValue is double)
            {
                cell.SetCellValue(Convert.ToDouble(CellValue));
            }
            else if (CellValue is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(CellValue));
            }
            else if (CellValue is bool)
            {
                cell.SetCellValue(Convert.ToBoolean(CellValue));
            }
            else if (CellValue is System.DateTime)
            {
                cell.SetCellValue(Convert.ToDateTime(CellValue));
            }
        }
    }
}
