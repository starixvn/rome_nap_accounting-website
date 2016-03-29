namespace Fanex.Tarus.Excels.CrossCurrencyTransfer
{
    using System.Data;

    public interface ICrossCurrencyTransferExcel
    {
        DataTable ExcelToDataTable(string filePath);
    }
}
