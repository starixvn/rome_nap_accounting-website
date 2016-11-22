using NPOI.SS.UserModel;
using System.Web;

namespace Fanex.Tarus.Infrastructure.Extensions
{
    public static class ExcelExtension
    {
        public static void ResponseStreamExcel(this IWorkbook workbook, string fileName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
            workbook.Write(response.OutputStream);
        }
    }
}
