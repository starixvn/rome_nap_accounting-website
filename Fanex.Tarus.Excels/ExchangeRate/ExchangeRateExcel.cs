namespace Fanex.Tarus.Excels.ExchangeRate
{
    using System.Data;

    public class ExchangeRateExcel : IExchangeRateExcel
    {
        private readonly ITarusExcel _tarusExcel;

        public ExchangeRateExcel(string filePath)
        {
            _tarusExcel = new TarusExcel(filePath, "ExRate");
        }

        public DataTable ExcelToDataTable()
        {
            return _tarusExcel.ExcelToDataTable(_tarusExcel.FirstRow + 1);
        }
    }
}
