namespace Fanex.Tarus.Statements.Services
{
    using System.Collections.Generic;
    using System.Web;
    using Fanex.Tarus.Statements.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public abstract class BaseStatementExcel<T>
    {
        protected readonly HSSFWorkbook HSSFWorkbook;
        protected readonly ISheet Worksheet;

        public BaseStatementExcel(IList<T> data, string sheetname = null)
        {
            HSSFWorkbook = new HSSFWorkbook();
            Worksheet = HSSFWorkbook.CreateSheet(sheetname ?? "Sheet1");

            ReportData = data;
            StartColumnIndex = 0;
            CurrentRowIndex = 0;
        }

        protected IList<T> ReportData { get; set; }

        protected int StartColumnIndex { get; set; }

        protected int CurrentRowIndex { get; set; }

        public void ResponseStreamExcel(
            HttpResponse response,
            string filename,
            string title)
        {
            // Render excel.
            RenderTitle(title);
            RenderHeader();
            RenderBody();
            RenderFooter();

            // Response to client.
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", filename));
            
            HSSFWorkbook.Write(response.OutputStream);
        }

        protected abstract void RenderTitle(string title);

        protected abstract void RenderHeader();

        protected abstract void RenderBody();

        protected abstract void RenderFooter();

        protected void RenderNoDataFound(int columnNumber)
        {
            string nodata = "There is no information available.";

            ICellStyle textCenter = NpoiExcelUtils.TextStyle(HSSFWorkbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Center);
            ExcelCellRange cellrange = new ExcelCellRange(CurrentRowIndex, CurrentRowIndex, StartColumnIndex, columnNumber, nodata, textCenter);
            cellrange.RenderCellRange(Worksheet);
        }
    }
}
