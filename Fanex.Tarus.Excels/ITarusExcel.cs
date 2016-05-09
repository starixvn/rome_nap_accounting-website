namespace Fanex.Tarus.Excels
{
    using System.Data;
    using NPOI.SS.UserModel;

    internal interface ITarusExcel
    {
        IWorkbook Workbook { get; }

        ISheet Sheet { get; }

        int FirstRow { get; }

        int LastRow { get; }

        int FirstCell { get; }

        int LastCell { get; }

        bool IsValidExtension(string fileName);

        DataTable ExcelToDataTable(int? headerRowIndex = null);
    }
}
