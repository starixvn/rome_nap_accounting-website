using System;
using System.Collections;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Report;

/// <summary>
/// Summary description for DailyCreatedCustomer
/// </summary>

namespace BLL.Report
{
    public class rptDailyCreatedCustomer : ReportBase
    {
        private readonly string REPORT_DailyCreatedCustomer = "accRptDailyCreatedCustomer." + AppSession.Username;

        private readonly string REPORT_DailyCreatedCustomerDetail = "accRptDailyCreatedCustomerDetail." +
                                                                    AppSession.Username;

        public override string CreateHeader(Hashtable input)
        {
            return GetDailyCreatedCustomer(input);
        }

        public override string RenderHTML(Hashtable input)
        {
            return GetDailyCreatedCustomerDetails(input);
        }

        public string GetDailyCreatedCustomer(Hashtable input)
        {
            string tpl = WebUtils.LoadTemplate("Report/DailyCreatedCustomer.htm");
            try
            {
                //Load Header
                TemplateHandler.Assign(ref tpl, "{Title}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DailyCreatedCustomerReport"));
                TemplateHandler.Assign(ref tpl, "{Date}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date"));
                TemplateHandler.Assign(ref tpl, "{Super}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"));
                TemplateHandler.Assign(ref tpl, "{Master}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DirectMaster"));
                TemplateHandler.Assign(ref tpl, "{Agent}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DirectAgent"));

                //Get data
                DbResult rs = DailyCreatedCustomer.GetDailyCreatedCustomer(input);
                DataRowCollection dtRow = rs.ResultSet.Tables[0].Rows;

                //Load Rows
                if (dtRow != null && dtRow.Count > 0)
                {
                    var html = new StringBuilder();
                    string tpl_row = TemplateHandler.CopyBlock(tpl, "Rows");
                    string odd_even = "";
                    int Super_TT = 0, Master_TT = 0, Agent_TT = 0;
                    foreach (DataRow dr in dtRow)
                    {
                        string row = tpl_row;
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                        string CreateDate = CastAide.AsDateTime(dr["CreateDate"]).ToShortDateString();
                        TemplateHandler.Assign(ref row, "{Date_vl}", CreateDate);
                        if (CastAide.AsInt16(dr["AmountSAg"]) != 0)
                            TemplateHandler.Assign(ref row, "{Super_vl}",
                                                   "<a onclick=View_DL(4,'" + CreateDate + "')>" +
                                                   CastAide.AsString(dr["AmountSAg"]) + "</a>");
                        else
                            TemplateHandler.Assign(ref row, "{Super_vl}", CastAide.AsString(dr["AmountSAg"]));
                        if (CastAide.AsInt16(dr["AmountMAg"]) != 0)
                            TemplateHandler.Assign(ref row, "{Master_vl}",
                                                   "<a onclick=View_DL(3,'" + CreateDate + "')>" +
                                                   CastAide.AsString(dr["AmountMAg"]) + "</a>");
                        else
                            TemplateHandler.Assign(ref row, "{Master_vl}", CastAide.AsString(dr["AmountMAg"]));
                        if (CastAide.AsInt16(dr["AmountAg"]) != 0)
                            TemplateHandler.Assign(ref row, "{Agent_vl}",
                                                   "<a onclick=View_DL(2,'" + CreateDate + "')>" +
                                                   CastAide.AsString(dr["AmountAg"]) + "</a>");
                        else
                            TemplateHandler.Assign(ref row, "{Agent_vl}", CastAide.AsString(dr["AmountAg"]));
                        Super_TT += CastAide.AsInt16(dr["AmountSAg"]);
                        Master_TT += CastAide.AsInt16(dr["AmountMAg"]);
                        Agent_TT += CastAide.AsInt16(dr["AmountAg"]);
                        html.Append(row);
                    }
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", html.ToString());
                    TemplateHandler.Assign(ref tpl, "{Super_TT}", CastAide.AsString(Super_TT));
                    TemplateHandler.Assign(ref tpl, "{Master_TT}", CastAide.AsString(Master_TT));
                    TemplateHandler.Assign(ref tpl, "{Agent_TT}", CastAide.AsString(Agent_TT));
                }
                else
                {
                    string norow = WebUtils.LoadTemplate("NoInformation.htm");
                    TemplateHandler.Assign(ref norow, "{tdno}", "4");
                    TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", norow);
                    TemplateHandler.RemoveBlock(ref tpl, "Total");
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            SetCacheReport(REPORT_DailyCreatedCustomer, tpl, DateTime.Now.AddMinutes(5));
            SetCacheReport(REPORT_DailyCreatedCustomerDetail, "", DateTime.Now.AddMinutes(5));
            return tpl;
        }

        public string GetDailyCreatedCustomerDetails(Hashtable input)
        {
            string tpl = WebUtils.LoadTemplate("Report/DailyCreatedCustomerDetails.htm");
            try
            {
                //Load Header
                string strTitle;
                if (CastAide.AsInt16(input["roleid"]) == 4)
                    strTitle = "Super List (" + CastAide.AsDateTime(input["date"]).ToShortDateString() + ")";
                else if (CastAide.AsInt16(input["roleid"]) == 3)
                    strTitle = "Master List (" + CastAide.AsDateTime(input["date"]).ToShortDateString() + ")";
                else
                    strTitle = "Agent List (" + CastAide.AsDateTime(input["date"]).ToShortDateString() + ")";

                TemplateHandler.Assign(ref tpl, "{Title}", strTitle);
                TemplateHandler.Assign(ref tpl, "{UserName}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UserName"));
                TemplateHandler.Assign(ref tpl, "{CreateDate}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreateDate"));
                TemplateHandler.Assign(ref tpl, "{CreatedBy}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreatedBy"));

                //Get data
                DbResult rs = DailyCreatedCustomer.GetDailyCreatedCustomerDetails(input);
                DataRowCollection dtRow = rs.ResultSet.Tables[0].Rows;

                //Load Rows
                if (dtRow != null && dtRow.Count > 0)
                {
                    var html = new StringBuilder();
                    string tpl_row = TemplateHandler.CopyBlock(tpl, "Rows");
                    string odd_even = "";
                    int i = 1;
                    foreach (DataRow dr in dtRow)
                    {
                        string row = tpl_row;
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                        TemplateHandler.Assign(ref row, "{No}", i.ToString());
                        TemplateHandler.Assign(ref row, "{UserName_vl}", CastAide.AsString(dr["UserName"]));
                        TemplateHandler.Assign(ref row, "{CreateDate_vl}", CastAide.AsString(dr["CreatDate"]));
                        TemplateHandler.Assign(ref row, "{CreatedBy_vl}", CastAide.AsString(dr["Creator"]));
                        html.Append(row);
                        i++;
                    }
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", html.ToString());
                }
                else
                {
                    string norow = WebUtils.LoadTemplate("NoInformation.htm");
                    TemplateHandler.Assign(ref norow, "{tdno}", "4");
                    TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", norow);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            SetCacheReport(REPORT_DailyCreatedCustomerDetail, tpl, DateTime.Now.AddMinutes(5));
            return tpl;
        }

        public override void Export2Excel(Hashtable input)
        {
            string data = GetCachedReport(REPORT_DailyCreatedCustomer);
            string dataDetail = GetCachedReport(REPORT_DailyCreatedCustomerDetail);
            if (string.IsNullOrEmpty(data))
            {
                GetDailyCreatedCustomer(input);
                data = GetCachedReport(REPORT_DailyCreatedCustomer);
            }
            data = "<div style='width:800px;'><span style='float:left;'>" + data + "</span><span style='float:right;'>" +
                   dataDetail + "</span></div>";
            var content = new StringBuilder();
            content.Append("<html>");
            content.Append("<head>");
            content.Append("<META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>");
            content.Append("</head>");
            content.Append(ReportHTML.CSSExcelReport());
            content.Append(data);
            content.Append("<table><tr><td class='tdhidden'></td></tr></table>");
            content.Append("</html>");

            ExcelContent = content.ToString();
            base.Export2Excel(input);
        }
    }
}