using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.IBCDLL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for ExportStatement
/// </summary>
namespace Sunplus.Accounting.Business.ExportStatement
{
    public class ExportStatementManager : BaseManager
    {
        private readonly string key = "accexportstatementtoexcel." + AppSession.Username;

        #region ReadUsers

        public string ReadUsers()
        {
            string userslistvalid = "";
            bool IsUploadFileSuccess = true;
            var html = new StringBuilder();
            int rows = 0;
            int success = 1;
            if (!UserController.OnlineUserUpdate(AppSession.OnAccId))
            {
                string appPath = HttpContext.Current.Request.ApplicationPath;
                if (appPath != "/") appPath += "/";
                html.Append(
                    "<script type='text/javascript'>alert('Another session has been logged in or your session is expired. Forced to logout!');top.location.href='" +
                    appPath + "PreLogIn/Logout.ashx';</script>");
            }
            else
            {
                string htmlData = "", strLocation = "";
                string FileSize = "<input type='hidden' id='FileSize' value='" +
                                  HttpContext.Current.Request.ContentLength + "'/>";
                try
                {
                    if (HttpContext.Current.Request.ContentLength < 1024000)
                    {
                        string strFileName = AppSession.Username;

                        if (HttpContext.Current.Request.Files.Count == 0)
                        {
                            htmlData =
                                "<span class='error-message'>Your upload file don't successfull, Please try again</span>";
                            success = 0;
                            html.Append(htmlData);
                            return html + "|" + userslistvalid + "|" + success;
                        }
                        string newfilename = HttpContext.Current.Request.Files[0].FileName;
                        string ext = string.Empty;
                        if (!string.IsNullOrEmpty(newfilename))
                        {
                            if (IsValidExtension(newfilename, ref ext))
                            {
                                strLocation = HttpContext.Current.Server.MapPath("../Others") + "\\" + strFileName +
                                              ".xls";
                                HttpContext.Current.Request.Files[0].SaveAs(strLocation);

                                if (!File.Exists(strLocation))
                                    IsUploadFileSuccess = false;

                                if (IsUploadFileSuccess)
                                {
                                    DataTable dtb = VirtualRate.GetDataFromExcel(strLocation);
                                    rows = dtb.Rows.Count;
                                    if (rows > 100)
                                    {
                                        htmlData =
                                            "<span class='error-message'>You are allowed to export statements of maximum 100 accounts.</span>";
                                        success = 0;
                                    }
                                    else
                                    {
                                        var file = new FileInfo(strLocation);
                                        if (file.Exists) file.Delete();

                                        htmlData = ListUsers(dtb, ref userslistvalid);
                                    }
                                }
                                else
                                {
                                    htmlData =
                                        "<span class='error-message'>Your upload file don't successfull, Please try again</span>";
                                    success = 0;
                                }
                            }
                            else
                            {
                                htmlData =
                                    "<span class='error-message'>Sorry, You chose invalid file extension. Please choose an excel file.</span>";
                                success = 0;
                            }
                        }
                        else
                        {
                            htmlData =
                                "<span class='error-message'>Sorry, You chose invalid file extension. Please choose an excel file.</span>";
                            success = 0;
                        }
                    }
                    else
                    {
                        htmlData = "<span class='error-message'>Your upload file Size must be <= 1 MB</span>";
                        success = 0;
                    }

                    html.Append(htmlData);
                }
                catch (Exception ex)
                {
                    success = 0;
                    if (strLocation != "")
                    {
                        var file = new FileInfo(strLocation);
                        if (file.Exists) file.Delete();
                    }

                    if (ex.Message ==
                        "'Sheet1$' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long."
                        ||
                        ex.Message == "String or binary data would be truncated.\r\nThe statement has been terminated.")
                        htmlData =
                            "<span class='error-message'>The Excel file is not in correct format. Sheet name must be Sheet1. </span>";
                    else
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                        throw;
                    }

                    html.Append(htmlData);
                }
            }

            return html + "|" + userslistvalid + "|" + success;
        }

        #endregion ReadUsers

        #region IsValidExtension

        private bool IsValidExtension(string fileName, ref string ext)
        {
            bool f = false;
            try
            {
                string[] arr = fileName.Split('\\');
                string[] arr1 = arr[arr.Length - 1].Split('.');
                ext = arr1[arr1.Length - 1].ToLower();
                //return ext.Equals("xls") == true || ext.Equals("xlsx") == true || ext.Equals("ods") == true;
                f = ext.Equals("xls") == true;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return f;
        }

        #endregion IsValidExtension

        #region ListUsers

        private string ListUsers(DataTable dtb, ref string userlistvalid)
        {
            StringBuilder userlist = new StringBuilder();
            StringBuilder RowValid = new StringBuilder();
            StringBuilder RowInValid = new StringBuilder();

            string tpl = WebUtils.LoadTemplate("Statement/ExportStatementUsersList.htm");
            string tplValidUsers = TemplateHandler.CopyBlock(tpl, "ValidUsers");
            string tplInValidUsers = TemplateHandler.CopyBlock(tpl, "InValidUsers");

            DataRow[] Rows = dtb.Select();

            foreach (DataRow dr in Rows)
            {
                userlist.Append(dr[0] + ",");
            }

            dtb = IBCProvider.ExportStatement.AllStatement_SelCheckByUser(userlist.ToString()).ResultSet.Tables[0];
            Rows = dtb.Select();

            int IndexInValid = 0, IndexValid = 0;
            userlist = new StringBuilder();
            string row = "", row1 = "";
            foreach (DataRow dr in Rows)
            {
                if (CastAide.AsBoolean(dr["Isvalid"]))
                {
                    IndexValid++;
                    row = tplValidUsers;
                    TemplateHandler.Assign(ref row, "Order", IndexValid);
                    TemplateHandler.Assign(ref row, "UserName", dr["username"]);
                    RowValid.Append(row);
                    userlist.Append(dr["username"] + ",");
                }
                else
                {
                    IndexInValid++;
                    row1 = tplInValidUsers;
                    TemplateHandler.Assign(ref row1, "Order1", IndexInValid);
                    TemplateHandler.Assign(ref row1, "UserName1", dr["username"]);
                    RowInValid.Append(row1);
                }
            }
            userlistvalid = userlist.ToString();

            TemplateHandler.Assign(ref tpl, "TotalValidUsers", IndexValid);
            TemplateHandler.Assign(ref tpl, "TotalInValidUsers", IndexInValid);
            TemplateHandler.ReplaceBlock(ref tpl, "ValidUsers", RowValid.ToString());
            TemplateHandler.ReplaceBlock(ref tpl, "InValidUsers", RowInValid.ToString());

            DataTable ValidUsers = dtb.Clone();
            DataView dv = dtb.DefaultView;
            dv.RowFilter = "Isvalid=1";
            ValidUsers = dv.ToTable();
            CacheData(key, ValidUsers, DateTime.Today.AddHours(5));
            return tpl;
        }

        #endregion ListUsers

        #region AllStatement_UsersList

        public string AllStatement_UsersList(string userslistvalid, string fdate, string tdate)
        {
            string tpl = WebUtils.LoadTemplate("Statement/ExportStatementResult.htm");
            string tplContent = TemplateHandler.CopyBlock(tpl, "Rows");
            var html = new StringBuilder();
            var param = new Hashtable();
            param.Add("userlist", userslistvalid);
            param.Add("fromdate", fdate);
            param.Add("todate", tdate);
            param.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);

            try
            {
                DbResult rs = IBCProvider.ExportStatement.AllStatement_UsersList(param);
                DataRow[] Rows = rs.ResultSet.Tables[0].Select();
                decimal Total = 0, Balance = 0;
                string oddeven = "odd";
                string username = "", username_p = "", transtype = "";
                int index = 0;
                string strColspan = "<tr bgcolor='white'><td colspan='9'>&nbsp;</td></tr>";
                foreach (DataRow dr in Rows)
                {
                    username_p = CastAide.AsString(dr["username"]);
                    index++;
                    string row = tplContent;

                    if (username != username_p)
                    {
                        if (index > 1)
                            html.Append(strColspan);

                        username = username_p;
                        Balance = 0;
                    }

                    oddeven = (oddeven == "even") ? "odd" : "even";
                    TemplateHandler.Assign(ref row, "oddeven", oddeven);
                    TemplateHandler.Assign(ref row, "Date", CastAide.AsDateTime(dr["accdate"]).ToShortDateString());
                    TemplateHandler.Assign(ref row, "Role", GetRoleName(CastAide.AsInt32(dr["roleid"])));
                    TemplateHandler.Assign(ref row, "Account", username_p);
                    TemplateHandler.Assign(ref row, "Currency",
                                           CacheUtils.GetExchangeName(CastAide.AsInt32(dr["currency"])));

                    transtype = CacheRef.GetBettype(dr["transtype"]);
                    if (transtype.Length == 0)
                        transtype = CastAide.AsString(dr["transtype"]);

                    TemplateHandler.Assign(ref row, "Remark", transtype + " - " + CastAide.AsString(dr["transdesc"]));

                    Total = CastAide.AsDecimal(dr["totalamt"]);
                    TemplateHandler.Assign(ref row, "Total", NumberFormat.DecFormat(Total, 2));
                    if (Total > 0)
                    {
                        TemplateHandler.Assign(ref row, "Credit", NumberFormat.DecFormat(Total, 2));
                        TemplateHandler.Assign(ref row, "Debit", "");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref row, "Credit", "");
                        TemplateHandler.Assign(ref row, "Debit", NumberFormat.DecFormat(Total * (-1), 2));
                    }
                    Balance += Total;
                    TemplateHandler.Assign(ref row, "Balance", NumberFormat.DecFormat(Balance, 2));

                    html.Append(row);
                }

                if (Rows.Length == 0)
                {
                    string str =
                        "<tr bgcolor='white'><td colspan='9' class='c'>There is no information available</td></tr>";
                    html.Append(str);
                }
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(invalidOperationException);
                throw;
            }
            catch (Exception ex)
            {
                string str = "<tr bgcolor='white'><td colspan='9' class='c error-message'>" + ex.Message + "</td></tr>";
                html.Append(str);
            }

            TemplateHandler.ReplaceBlock(ref tpl, "Rows", html.ToString());

            return tpl;
        }

        #endregion AllStatement_UsersList

        #region GetRoleName

        private string GetRoleName(int RoleId)
        {
            string str = "";
            switch (RoleId)
            {
                case 1:
                    str = "Member";
                    break;

                case 2:
                    str = "Agent";
                    break;

                case 3:
                    str = "Master";
                    break;

                case 4:
                    str = "Super";
                    break;

                default:
                    str = "Unknow";
                    break;
            }
            return str;
        }

        #endregion GetRoleName

        #region Export2Excel

        public ExcelFile Export2Excel(string userslistvalid, string fdate, string todate)
        {
            var excelFile = new ExcelFile();
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            string filename = "Export-Multi-Statement-" + DateTime.Today.ToShortDateString().Replace("/", "-");
            ExcelWorksheet ws = worksheets.Add(filename);

            #region declare format cell

            var celltitle = new CellStyle();
            celltitle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            celltitle.VerticalAlignment = VerticalAlignmentStyle.Center;
            celltitle.FillPattern.SetSolid(Color.FromArgb(82, 106, 128));
            celltitle.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            celltitle.Font.Weight = ExcelFont.BoldWeight;
            celltitle.Font.Color = Color.White;
            celltitle.WrapText = true;

            var bgcolorText = new CellStyle();
            var bgcolorNumber = new CellStyle();

            var cellNumberNormalEven = new CellStyle();
            cellNumberNormalEven.VerticalAlignment = VerticalAlignmentStyle.Center;
            cellNumberNormalEven.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            cellNumberNormalEven.NumberFormat = "#,##0.00";
            cellNumberNormalEven.FillPattern.SetSolid(Color.FromArgb(223, 234, 255));

            var cellTextNormalEven = new CellStyle();
            cellTextNormalEven.VerticalAlignment = VerticalAlignmentStyle.Center;
            cellTextNormalEven.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cellTextNormalEven.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            cellTextNormalEven.WrapText = true;
            cellTextNormalEven.FillPattern.SetSolid(Color.FromArgb(223, 234, 255));

            var cellNumberNormal = new CellStyle();
            cellNumberNormal.VerticalAlignment = VerticalAlignmentStyle.Center;
            cellNumberNormal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            cellNumberNormal.NumberFormat = "#,##0.00";

            var cellTextNormal = new CellStyle();
            cellTextNormal.VerticalAlignment = VerticalAlignmentStyle.Center;
            cellTextNormal.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cellTextNormal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            cellTextNormal.WrapText = true;

            #endregion declare format cell

            #region columnwidth

            ws.Columns[0].Width = 10 * 256;
            ws.Columns[1].Width = 8 * 256;
            ws.Columns[2].Width = 20 * 256;
            ws.Columns[3].Width = 10 * 256;
            ws.Columns[4].Width = 40 * 256;
            ws.Columns[5].Width = 10 * 256;
            ws.Columns[6].Width = 10 * 256;
            ws.Columns[7].Width = 10 * 256;
            ws.Columns[8].Width = 10 * 256;

            #endregion columnwidth

            #region Header

            ws.Cells[0, 0].Value = "Date";
            ws.Cells[0, 1].Value = "Role";
            ws.Cells[0, 2].Value = "Account";
            ws.Cells[0, 3].Value = "Currency";
            ws.Cells[0, 4].Value = "Remark";
            ws.Cells[0, 5].Value = "Total";
            ws.Cells[0, 6].Value = "Debit";
            ws.Cells[0, 7].Value = "Credit";
            ws.Cells[0, 8].Value = "Balance";
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 8).Style = celltitle;

            #endregion Header

            var param = new Hashtable();
            param.Add("userlist", userslistvalid);
            param.Add("fromdate", fdate);
            param.Add("todate", todate);
            param.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);

            DbResult rs = IBCProvider.ExportStatement.AllStatement_UsersList(param);
            DataRow[] Rows = rs.ResultSet.Tables[0].Select();

            #region Content

            int i = 1, countOpenBalance = 0;
            decimal Balance = 0;
            string username = "", username_p = "", transtype = "", transdesc = "";
            foreach (DataRow dr in Rows)
            {
                username_p = CastAide.AsString(dr["username"]);
                transtype = CacheRef.GetBettype(dr["transtype"]);
                transdesc = CastAide.AsString(dr["transdesc"]);

                if (username.ToLower() != username_p.ToLower())
                {
                    if (i >= 2)
                    {
                        ws.Cells.GetSubrangeAbsolute(i, 0, i, 8).Merged = true;
                        i++;
                    }

                    username = username_p;
                    Balance = 0;
                    countOpenBalance = 0;
                }

                if (transdesc.ToUpper().Contains("OPENING BALANCE"))
                {
                    countOpenBalance++;

                    if (countOpenBalance > 1) continue;
                }

                if (i % 2 == 0)
                {
                    bgcolorText = cellTextNormal;
                    bgcolorNumber = cellNumberNormal;
                }
                else
                {
                    bgcolorText = cellTextNormalEven;
                    bgcolorNumber = cellNumberNormalEven;
                }

                ws.Cells[i, 0].Value = CastAide.AsDateTime(dr["accdate"]).ToShortDateString();
                ws.Cells[i, 0].Style = bgcolorText;

                ws.Cells[i, 1].Value = GetRoleName(CastAide.AsInt32(dr["roleid"]));
                ws.Cells[i, 1].Style = bgcolorText;

                ws.Cells[i, 2].Value = CastAide.AsString(dr["username"]);
                ws.Cells[i, 2].Style = bgcolorText;

                ws.Cells[i, 3].Value = CacheUtils.GetExchangeName(CastAide.AsInt32(dr["currency"]));
                ws.Cells[i, 3].Style = bgcolorText;

                if (transtype.Length == 0)
                    transtype = CastAide.AsString(dr["transtype"]);

                ws.Cells[i, 4].Value = transtype + " - " + transdesc;
                ws.Cells[i, 4].Style = bgcolorText;

                decimal Total = CastAide.AsDecimal(dr["totalamt"]);
                ws.Cells[i, 5].Value = Total;
                ws.Cells[i, 5].Style = bgcolorNumber;

                if (Total > 0)
                {
                    ws.Cells[i, 7].Value = Total;
                    ws.Cells[i, 7].Style = bgcolorNumber;

                    ws.Cells[i, 6].Value = "";
                    ws.Cells[i, 6].Style = bgcolorText;
                }
                else
                {
                    ws.Cells[i, 7].Value = "";
                    ws.Cells[i, 7].Style = bgcolorText;

                    ws.Cells[i, 6].Value = Total * (-1);
                    ws.Cells[i, 6].Style = bgcolorNumber;
                }
                Balance += Total;
                ws.Cells[i, 8].Value = Balance;
                ws.Cells[i, 8].Style = bgcolorNumber;

                i++;
            }

            return excelFile;

            #endregion Content
        }

        #endregion Export2Excel
    }
}