

using System;
using System.Web;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.DataAccess.Report;

namespace Fanex.Tarus.Reports.Helpers
{
    public class ReportHelpers
    {
        #region IFields

        public string BetTrans14SPSuffix = "_14";
        public string WareHouseSPSuffix = "_WH";

        #endregion IFields
        #region IMethods

        public string GetStoreMappingSubfix(DateTime reportDate)
        {
            if (reportDate > ReportUtils.BalanceUp.MoveBetsDay)
            {
                return string.Empty;
            }
            else if (reportDate >= ReportUtils.BalanceUp.BalanceUpDay)
            {
                return BetTrans14SPSuffix;
            }
            else
            {
                return WareHouseSPSuffix;
            }
        }

        public void ReponseExcelFile(ExcelFile excelFile, string reportName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
        }

        #endregion IMethods
    }
}
