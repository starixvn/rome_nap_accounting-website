namespace Fanex.Tarus.Excels.CrossCurrencyTransfer
{
    using System;
    using System.Data;
    using NPOI.SS.UserModel;

    public class CrossCurrencyTransferExcel : ICrossCurrencyTransferExcel
    {
        private readonly ITarusExcel _tarusExcel;

        public CrossCurrencyTransferExcel(string filePath)
        {
            _tarusExcel = new TarusExcel(filePath);
        }

        public bool ValidateTemplateFile()
        {
            if (_tarusExcel.FirstRow == _tarusExcel.LastRow)
            {
                return false;
            }

            if (_tarusExcel.LastCell != 5)
            {
                return false;
            }

            IRow headerRow = _tarusExcel.Sheet.GetRow(_tarusExcel.FirstRow);
            if (!headerRow.Cells[3].StringCellValue.Trim().Equals("to amount", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public DataTable ExcelToDataTable()
        {
            return _tarusExcel.ExcelToDataTable();
        }
    }
}
