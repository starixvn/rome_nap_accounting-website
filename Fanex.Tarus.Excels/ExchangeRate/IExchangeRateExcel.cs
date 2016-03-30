namespace Fanex.Tarus.Excels.ExchangeRate
{
    using System.Data;

    public interface IExchangeRateExcel
    {
        DataTable ExcelToDataTable();
    }
}
