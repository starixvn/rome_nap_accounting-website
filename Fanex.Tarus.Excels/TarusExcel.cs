namespace Fanex.Tarus.Excels
{
    using System;
    using System.Data;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    internal class TarusExcel : ITarusExcel
    {
        public TarusExcel(string filePath)
            : this(filePath, "Sheet1")
        {
        }

        public TarusExcel(string filePath, string sheetName)
        {
            Initialize(filePath, sheetName);
        }

        public int FirstRow { get; private set; }

        public IWorkbook Workbook { get; private set; }

        public ISheet Sheet { get; private set; }

        public int LastRow { get; private set; }

        public int FirstCell { get; private set; }

        public int LastCell { get; private set; }

        public bool IsValidExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }

            return ext.Equals("xls", StringComparison.OrdinalIgnoreCase);
        }

        public DataTable ExcelToDataTable(int? headerRowIndex = null)
        {
            if (Workbook == null || Sheet == null || FirstRow == LastRow)
            {
                return null;
            }

            DataTable table = new DataTable();

            // Add DataColumn.
            FirstRow = headerRowIndex ?? FirstRow;
            IRow row = Sheet.GetRow(FirstRow);

            for (int i = FirstCell; i < LastCell; i++)
            {
                table.Columns.Add(new DataColumn(row.GetCell(i).StringCellValue.Trim()));
            }

            DataRow dataRow;
            
            // Add DataRow.
            for (int i = FirstRow + 1; i <= LastRow; i++)
            {
                row = Sheet.GetRow(i);
                dataRow = table.NewRow();

                for (int j = FirstCell; j < LastCell; j++)
                {
                    dataRow[j] = GetCellValue(row.GetCell(j));
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }

        private void Initialize(string filePath, string sheetName)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            Stream fileStream = File.OpenRead(filePath);

            Workbook = new HSSFWorkbook(fileStream);
            fileStream.Close();

            Sheet = Workbook.GetSheet(sheetName);
            if (Sheet == null)
            {
                return;
            }

            FirstRow = Sheet.FirstRowNum;
            LastRow = Sheet.LastRowNum;

            if (FirstRow == LastRow)
            {
                return;
            }

            IRow headerRow = Sheet.GetRow(FirstRow);

            FirstCell = headerRow.FirstCellNum;
            LastCell = headerRow.LastCellNum;
        }

        private object GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            object value;
            if (cell.CellType == CellType.Numeric)
            {
                value = cell.NumericCellValue;
            }
            else
            {
                value = cell.StringCellValue.Trim();
            }

            return value;
        }
    }
}
