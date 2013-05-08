using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DNAide.Core;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Entities.Report.Others;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.Report.Other
{
    public class NumberOfBetsManager : INumberOfBetsManager
    {
        private string _numberOfBetsForCreditSiteSessionKey = "numberofbetsforcreditsitesessionkey";
        public NumberOfBetsDTO SelectNumberOfBetsForCreditSite(DateTime fromDate, DateTime toDate)
        {
            var result = new NumberOfBetsDTO();
            try
            {
                var strapper = CommonBootStrapper.Locator.GetInstance<IOthersReportDAO>();
                var db = strapper.SelectNumberOfBetsForCreditSites(fromDate, toDate);
                if (db != null && db.Rows.Count > 0)
                {
                    result.rows = new List<NumberOfBets>();
                    result.rows.AddRange(from DataRow dr in db.Rows
                                    select new NumberOfBets()
                                               {
                                                   Site = dr["site"].ToString(),
                                                   BetCount = CastAide.AsInt32(dr["betcount"])
                                               });

                    
                    var totalBecount = (from rs in result.rows select rs.BetCount).Sum();
                    result.total =new NumberOfBets() { 
                            Site = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total"),
                            BetCount = totalBecount
                        };

                    SessionExtension.AddSession(HttpContext.Current.Session, _numberOfBetsForCreditSiteSessionKey, result);
                }
            }
            catch(Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public void ExportExcelNumberOfBetsForCreditSites(DateTime fromDate, DateTime toDate)
        {
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session,
                                                         _numberOfBetsForCreditSiteSessionKey);
            var reportInfo = getSession != null
                                 ? (NumberOfBetsDTO)getSession
                                 : SelectNumberOfBetsForCreditSite(fromDate, toDate);
            #region local variables
            var reportName = string.Format("NumberOfBetsForCreditSites-From-{0}-To-{1}", fromDate.ToShortDateString(), toDate.ToShortDateString());
            const int startRow = 2;
            const int startCol = 0;
            int nextRow = 0;
            var excelFile = new ExcelFile();
            var wsBetCount = excelFile.Worksheets.Add("NumberOfBetsForCreditSites");
            var bgHeader = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            #endregion

            #region header
            wsBetCount.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Merged = true;
            wsBetCount.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Value = 
                string.Format("{0} from {1} to {2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNumberOfBets"), fromDate.ToShortDateString(), toDate.ToShortDateString());
            wsBetCount.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Style.Font.Weight =
                ExcelFont.BoldWeight;

            wsBetCount.Cells[startRow - 1, startCol].Style = bgHeader;
            wsBetCount.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomerSite");
            wsBetCount.Cells[startRow - 1, startCol + 1].Style = bgHeader;
            wsBetCount.Cells[startRow - 1, startCol + 1].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetCount");
            #endregion

            #region Content
            if(reportInfo != null && reportInfo.rows.Count >0)
            {
                var normalBg = new Core.Helper.ExcelGemboxUtils().GetStyleText(false);
                foreach (NumberOfBets numberOfBets in reportInfo.rows)
                {

                    wsBetCount.Cells[startRow + nextRow, startCol].Style = normalBg;
                    wsBetCount.Cells[startRow + nextRow, startCol].Value = numberOfBets.Site;
                    wsBetCount.Cells[startRow + nextRow, startCol + 1].Style = normalBg;
                    wsBetCount.Cells[startRow + nextRow, startCol + 1].Value = numberOfBets.BetCount;

                    nextRow++;
                }
                var footerFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(205, 222, 255), true, false);
                wsBetCount.Cells[startRow + nextRow, startCol].Style = footerFormat;                
                wsBetCount.Cells[startRow + nextRow, startCol].Value = reportInfo.total.Site;
                wsBetCount.Cells[startRow + nextRow, startCol + 1].Style = footerFormat;
                wsBetCount.Cells[startRow + nextRow, startCol + 1].Value = reportInfo.total.BetCount;
            }
            #endregion

            #region response
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
            #endregion
        }
    }
}
