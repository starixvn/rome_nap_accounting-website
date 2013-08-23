<%@ WebHandler Language="C#" Class="UploadVirtualRate" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Data;
using DNAide.Core;
using System.Text;
using Sunplus.Accounting.DataAccess.IBCDLL;


public class UploadVirtualRate : IHttpHandler, IRequiresSessionState
{
    private string Index="";
        
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        bool IsForceLogout = !UserController.OnlineUserUpdate(AppSession.OnAccId);
      
        {

            string htmlData = "", strLocation = "";
            string FileSize = "<input type='hidden' id='FileSize' value='" + context.Request.ContentLength + "'/>";
            bool isvalid = true;
            try
            {
                if (context.Request.ContentLength < 1024000)
                {
                    string strFileName = AppSession.Username;
                    string newfilename = context.Request.Files[0].FileName;
                    string ext = string.Empty;
                    if (!string.IsNullOrEmpty(newfilename))
                    {
                        if (IsValidExtension(newfilename, ref ext))
                        {
                            strLocation = context.Server.MapPath("../Others") + "\\" + strFileName + ".xls";
                            context.Request.Files[0].SaveAs(strLocation);
                            DataTable dtb = VirtualRate.GetDataFromExcel(strLocation);

                            FileInfo file = new FileInfo(strLocation);
                            if (file.Exists) file.Delete();
                            
                            htmlData = ListUpdVirtualRate(dtb);
                        }
                        else
                            htmlData = AlertMessage.GetMsg("Sorry, You choose invalid file extension. Please choose an excel file.", AlertMessage.MsgType.Error);
                    }
                    else
                        htmlData = AlertMessage.GetMsg("Sorry, You choose invalid file extension. Please choose an excel file.", AlertMessage.MsgType.Error);
                }
                else
                    htmlData = AlertMessage.GetMsg("Your upload file Size must be <= 1 MB", AlertMessage.MsgType.Error);
            }
            catch (Exception ex)
            {
                isvalid = false;
                if (strLocation != "")
                {
                    FileInfo file = new FileInfo(strLocation);
                    if (file.Exists) file.Delete();
                }

                if (ex.Message == "'Sheet1$' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long."
                    || ex.Message == "String or binary data would be truncated.\r\nThe statement has been terminated.")
                    htmlData = AlertMessage.GetMsg("The Excel file is not in correct format.", AlertMessage.MsgType.Error) + "<div class='b' style='color:#808080;margin-bottom:5px'>Notes: Your Excel file must contain 2 columns named Member and New Rate. Additionally, Sheet name must be Sheet1.</div>";
                else
                {
                    htmlData = AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "<div class='b' style='color:#808080;margin-bottom:5px'>Notes: Your Excel file must contain 2 columns named Member and New Rate. Additionally, Sheet name must be Sheet1.</div>";
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            context.Response.Write(isvalid.ToString().ToLower() + "|" + htmlData + FileSize + Index + "|" + IsForceLogout.ToString().ToLower());
        }
    }

    private string ListUpdVirtualRate(DataTable dtb)
    {
        string tplFail = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Others/VirtualRate.htm");
        string tplHeadLine = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Others/VirtualRateHeadLine.htm");
        //Asign Header
        TemplateHandler.Assign(ref tplFail, "{lblUsername}", Resources.Language.UserName);
        TemplateHandler.Assign(ref tplFail, "{lblVirtualRate}", "New Rate");
        TemplateHandler.Assign(ref tplFail, "{lblReason}", "Reason");

        string tplSucceed = tplFail;
        TemplateHandler.RemoveBlock(ref tplSucceed, "ReasonUpdFail");

        string strNoRow = "";
        StringBuilder htmlContent = new StringBuilder();        
        DbResult rs = VirtualRate.DelDataTmpExcel();
        if (rs.Outputs["errcode"].ToString() == "0")
        {
            StringBuilder arrUsernames = new StringBuilder();
            StringBuilder arrVirtualRates = new StringBuilder();
            // Insert Data From Excel into DB
            DataTable dtbFail = new DataTable();
            dtbFail.Columns.Add("username");
            dtbFail.Columns.Add("virtualrate");
            dtbFail.Columns.Add("reason");

            //dtbFail.NewRow();
            foreach (DataRow row in dtb.Rows)
            {
                arrUsernames.Append(row[0] + "");
                arrVirtualRates.Append(row[1] + "");
                if (arrUsernames.Length > 3980)
                {
                    rs = VirtualRate.InsDataTmpExcel(arrUsernames.ToString(), arrVirtualRates.ToString());
                    arrUsernames = new StringBuilder(); arrVirtualRates = new StringBuilder();
                    dtbFail.Merge(rs.ResultSet.Tables[0]);
                }
            }
            // Insert last time.
            if (arrUsernames.Length > 0)
            {
                rs = VirtualRate.InsDataTmpExcel(arrUsernames.ToString(), arrVirtualRates.ToString());
                dtbFail.Merge(rs.ResultSet.Tables[0]);
            }

            // Check Data
            rs = VirtualRate.CheckData();
            if (rs.Outputs["errcode"].ToString() == "0")
            {
                DataTable dtbSucceed = rs.ResultSet.Tables[0];
                dtbFail.Merge(rs.ResultSet.Tables[1]);
                if (dtbSucceed.Rows.Count == 0) strNoRow="<span class='errMsg'></span>";
                Index = "<input type='hidden' id='Index' value='" + CastAide.AsString(dtbSucceed.Rows.Count / 501 + 1) + "'/>";
                TemplateHandler.Assign(ref tplHeadLine, "{countSucceed}", CastAide.AsString(dtbSucceed.Rows.Count));
                TemplateHandler.Assign(ref tplHeadLine, "{countFail}", CastAide.AsString(dtbFail.Rows.Count));

                htmlContent.Append("<div style='overflow-x: hidden; overflow-y: scroll; height: 550px;'><table id='data' class='collap'><tr><td valign='top'>");
                htmlContent.Append(LoadData(true, tplSucceed, dtbSucceed));
                htmlContent.Append("</td><td style='padding-left:40px'></td><td valign='top'>");
                htmlContent.Append(LoadData(false, tplFail, dtbFail));
                htmlContent.Append("</td></tr></table></div>");
            }
            else
                htmlContent.Append(AlertMessage.GetMsg(rs.Outputs["errmsg"].ToString(), AlertMessage.MsgType.Error));
        }
        else
        {
            TemplateHandler.Assign(ref tplHeadLine, "{countSucceed}", "0");
            TemplateHandler.Assign(ref tplHeadLine, "{countFail}", "0");
            htmlContent.Append(AlertMessage.GetMsg(rs.Outputs["errmsg"].ToString(), AlertMessage.MsgType.Error));
        }
        if (htmlContent.ToString().ToUpper().Substring(0, 11).Equals("<SPAN CLASS"))
            return htmlContent.ToString();
        return strNoRow + tplHeadLine + htmlContent.ToString();
    }

    private string LoadData(Boolean bolSucceed, string tplContent, DataTable dtb)
    {
        string tplRow = TemplateHandler.CopyBlock(tplContent, "row");
        DataRowCollection dtRow = dtb.Rows;
        if (dtRow != null && dtRow.Count > 0)
        {
            //Gen data
            int i = 1;
            StringBuilder rowContent = new StringBuilder();
            string odd_even = "evenrow";
            if (bolSucceed)
            {
                foreach (DataRow row in dtRow)
                {
                    string sRow = tplRow;
                    odd_even = (i % 2 == 0) ? "evenrow" : "oddrow";
                    TemplateHandler.Assign(ref sRow, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref sRow, "{no}", i.ToString());
                    TemplateHandler.Assign(ref sRow, "{username}", CastAide.AsString(row["username"]));
                    TemplateHandler.Assign(ref sRow, "{virtualrate}", CastAide.AsString(CastAide.AsDouble(row["virtualrate"]) * 100) + "%");
                    rowContent.Append(sRow);
                    i++;
                }
            }
            else
            {
                foreach (DataRow row in dtb.Select(null, "username"))
                {
                    string sRow = tplRow;
                    odd_even = (i % 2 == 0) ? "evenrow" : "oddrow";
                    TemplateHandler.Assign(ref sRow, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref sRow, "{no}", i.ToString());
                    TemplateHandler.Assign(ref sRow, "{username}", CastAide.AsString(row["username"]));
                    TemplateHandler.Assign(ref sRow, "{virtualrate}", (CastAide.AsDouble(row["virtualrate"]) != 0) ? CastAide.AsString(CastAide.AsDouble(row["virtualrate"]) * 100) + "%" : CastAide.AsString(row["virtualrate"]));                    
                    TemplateHandler.Assign(ref sRow, "{reason}", CastAide.AsString(row["reason"]));
                    rowContent.Append(sRow);
                    i++;
                }
            }
            TemplateHandler.ReplaceBlock(ref tplContent, "row", rowContent.ToString());
        }
        else
        {
            string strWidth=(bolSucceed)?"style='width:230px'":"style='width:614px'";
            TemplateHandler.ReplaceBlock(ref tplContent, "row", "<tr bgColor='#fff'><td class='c' bgColor='WHITE' " + strWidth + " colspan='4'>" + Resources.Language.thereisnodata + "</td></tr>");
        }
        return tplContent;
    }

    public static bool IsValidExtension(string fileName, ref string ext)
    {
        string[] arr = fileName.Split('\\');
        string[] arr1 = arr[arr.Length - 1].Split('.');
        ext = arr1[arr1.Length - 1].ToLower();
        //return ext.Equals("xls") == true || ext.Equals("xlsx") == true || ext.Equals("ods") == true;
        return ext.Equals("xls") == true;
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}
