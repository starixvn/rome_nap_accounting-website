namespace Fanex.Tarus.Excels.CrossCurrencyTransfer
{
    using System.Data;

    public interface ICrossCurrencyTransferExcel
    {
        bool ValidateTemplateFile();

        DataTable ExcelToDataTable();
    }
}
