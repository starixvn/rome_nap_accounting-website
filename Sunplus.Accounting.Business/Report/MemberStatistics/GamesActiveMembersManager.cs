using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using IBCUtils;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.MemberStatistics;

namespace Sunplus.Accounting.Business.Report.MemberStatistics
{
    public class GamesActiveMembersManager : IGamesActiveMembersManager
    {
        private string ActiveMembersSessionKey = "activememberssessionkey";

        public List<GamesActiveMembers> GetNumberOfActiveMembers(string fromDate, string toDate)
        {
            var result = new List<GamesActiveMembers>();
            var balanceUpDay = ReportUtils.BalanceUp.BalanceUpDay;
            DateTime fDate, tDate;

            if (!Validator.ValidDateMMDDYYYYNotNull(fromDate, out fDate).IsSuccess() ||
                !Validator.ValidDateMMDDYYYYNotNull(toDate, out tDate).IsSuccess())
                return result;

            var dbResult = new DataAccess.Report.MemberStatistics.GamesActiveMembers().GetNumberOfActiveMembers(fDate, tDate, balanceUpDay);
            if (dbResult != null && dbResult.ResultSet.Tables[0].Rows.Count > 0)
            {
                result.AddRange(from DataRow dr in dbResult.ResultSet.Tables[0].Rows
                                select new GamesActiveMembers()
                                           {
                                               GameName = dr["gamename"].ToString(),
                                               NoActives = CastAide.AsInt32(dr["noactive"].ToString())
                                           });
            }

            SessionExtension.AddSession(HttpContext.Current.Session, ActiveMembersSessionKey, result);
            return result;
        }

        public void NumberOfActiveMembersExportExcel(string fromDate, string toDate)
        {
            var membersActive = (List<GamesActiveMembers>)SessionExtension.GetSession(HttpContext.Current.Session, ActiveMembersSessionKey) ??
                     GetNumberOfActiveMembers(fromDate, toDate);

            #region local variables

            const string reportName = "ActiveMembers";
            var excelFile = new ExcelFile();
            const int startRowIndex = 2;
            const int startColumnIndex = 0;
            var nextRow = 0;
            var wsActiveMembers = excelFile.Worksheets.Add(reportName);
            var bgFormat = ExcelFormat(Color.FromArgb(205, 222, 243), HorizontalAlignmentStyle.Left, ExcelFont.BoldWeight);
            var bgNormalFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight);

            #endregion local variables

            wsActiveMembers.Cells[startRowIndex - 2, startColumnIndex].Style.Font.Weight = ExcelFont.BoldWeight;
            wsActiveMembers.Cells[startRowIndex - 2, startColumnIndex].Value = string.Format("Number of Active Members from {0} to {1}", fromDate, toDate);

            #region Body

            wsActiveMembers.Columns[startColumnIndex].Width = 25 * 256;
            if (membersActive != null)
            {
                foreach (var activemember in membersActive)
                {
                    //wsActiveMembers.Cells[startRowIndex, startColumnIndex].Style = bgFormat;
                    //wsActiveMembers.Cells[startRowIndex, startColumnIndex + 1].Style = bgNormalFormat;
                    //wsActiveMembers.Cells[startRowIndex, startColumnIndex].Value = activemember.GameName;
                    //wsActiveMembers.Cells[startRowIndex + (nextRow++), startColumnIndex + 1].Value = activemember.NoActives;

                    wsActiveMembers.Cells[startRowIndex + (nextRow), startColumnIndex].Style = bgFormat;
                    wsActiveMembers.Cells[startRowIndex + (nextRow), startColumnIndex + 1].Style = bgNormalFormat;
                    wsActiveMembers.Cells[startRowIndex + (nextRow), startColumnIndex].Value = activemember.GameName;
                    wsActiveMembers.Cells[startRowIndex + (nextRow++), startColumnIndex + 1].Value = activemember.NoActives;
                }
            }
            else
            {
                wsActiveMembers.Cells[startRowIndex, startColumnIndex].Value = "There is no data.";
            }

            #endregion Body

            #region Response ExcelFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion Response ExcelFile
        }

        private static CellStyle ExcelFormat(Color backgroundColor, HorizontalAlignmentStyle alignmentStyle, int font)
        {
            var excelFormat = new CellStyle();

            excelFormat.FillPattern.SetSolid(backgroundColor);
            excelFormat.Borders.SetBorders(MultipleBorders.Outside, Color.FromArgb(65, 116, 177), LineStyle.Thin);
            excelFormat.Font.Weight = font;
            excelFormat.HorizontalAlignment = alignmentStyle;
            excelFormat.WrapText = true;
            excelFormat.Font.Name = "Tahoma";
            excelFormat.Font.Size = 160;

            return excelFormat;
        }
    }
}