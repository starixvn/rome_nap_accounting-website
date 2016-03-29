namespace Fanex.Tarus.Excels.CrossCurrencyTransfer
{
    using System;
    using System.Data;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class CrossCurrencyTransferExcel : ICrossCurrencyTransferExcel
    {
        public DataTable ExcelToDataTable(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            IWorkbook workbook;
            using (Stream fileStream = File.OpenRead(filePath))
            {
                workbook = new HSSFWorkbook(fileStream);
            }

            string sheetName = "Sheet1";
            ISheet sheet = workbook.GetSheet(sheetName);

            int firstRowNum = sheet.FirstRowNum;
            int lastRowNum = sheet.LastRowNum;
            if (firstRowNum == lastRowNum)
            {
                return null;
            }

            IRow headerRow = sheet.GetRow(firstRowNum);
            int lastCellNum = headerRow.LastCellNum;
            if (lastCellNum != 5 || !headerRow.Cells[3].StringCellValue.Trim().Equals("to amount", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            IRow row;
            DataRow dataRow;
            DataTable table = new DataTable();

            // Add DataColumn.
            int firstCellNum = headerRow.FirstCellNum;
            for (int i = firstCellNum; i < lastCellNum; i++)
            {
                table.Columns.Add(new DataColumn(headerRow.GetCell(i).StringCellValue.Trim()));
            }

            // Add DataRow.
            ICell cell;
            for (int i = ++firstRowNum; i <= lastRowNum; i++)
            {
                row = sheet.GetRow(i);
                dataRow = table.NewRow();

                for (int j = firstCellNum; j < lastCellNum; j++)
                {
                    cell = row.GetCell(j);
                    if (cell != null)
                    {
                        if (cell.CellType == CellType.Numeric)
                        {
                            dataRow[j] = cell.NumericCellValue;
                        }
                        else
                        {
                            dataRow[j] = cell.StringCellValue.Trim();
                        }
                    }
                }

                table.Rows.Add(dataRow);
            }

            return table;
        } 
    }
}
