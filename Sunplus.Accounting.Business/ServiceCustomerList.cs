namespace Sunplus.Accounting.Business
{
    using System;
    using System.Collections;
    using System.Data;
    using System.ServiceModel.Activation;
    using System.Text;
    using DNAide.Core;
    using DNAide.Data;
    using DNAide.Web;
    using IBCUtils;
    using Sunplus.Accounting.Business.Customer;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;
    using Sunplus.Accounting.Core.Web;
    using Sunplus.Accounting.Core.Web.Controls;
    using Sunplus.Accounting.DataAccess.IBCDLL.Customer;

    /// <summary>
    /// Summary description for ServiceCustomerList
    /// </summary>
    ///
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceCustomerList : IServiceCustomerList
    {
        public string GenSuperListContent(string searchText, string selectAlpha, string selectCurreny, int type, string status, int PageIndex)
        {
            #region private varialbes

            int TotalPages = 0;
            int tblWidth = 40 + 60 + 40 + 50 + 50 + 120 + 120 + 120 + 120 + 40 + 50 + 50 + 50 + 50 + 50 + 50 + 50 + 80;
            int divwidth = 0;
            int colNo = 18;
            int colinfo = 10;
            int colComm = 7;
            string odd_even = "";
            string strpager = "";
            string contentTpl = "";
            var html = new StringBuilder();
            var html2 = new StringBuilder();

            #endregion private varialbes

            try
            {
                var hash = new Hashtable();

                if (!string.IsNullOrEmpty(searchText))
                {
                    string keyword = "%" + searchText.Replace("'", "") + "%";
                    hash.Add("keyword", keyword);
                }

                if (!string.IsNullOrEmpty(selectAlpha))
                {
                    string firstchar = selectAlpha;
                    if (string.Compare(firstchar, "0-9", true) == 0)
                    {
                        firstchar = "[" + firstchar + "]";
                    }
                    firstchar = firstchar + "%";
                    hash.Add("firstchar", firstchar);
                }
                if (!string.IsNullOrEmpty(selectCurreny))
                    hash.Add("currency", selectCurreny);
                if (!string.IsNullOrEmpty(status))
                    hash.Add("closed", DNAide.Core.CastAide.AsBoolean(DNAide.Core.CastAide.AsInt32(status)));

                hash.Add("isinternal", type);

                string strCurrency = selectCurreny as string ?? string.Empty;
                string close = status as string ?? string.Empty;

                var tplURL = "Customer/AgentList.htm";               

                contentTpl = WebUtils.LoadTemplateBySystem(tplURL, true);

                TemplateHandler.Assign(ref contentTpl, "{roleid}", "4");
                TemplateHandler.Assign(ref contentTpl, "{parentid}", "0");  

                #region start code

                if (!Functions.Customer.IsVisibleMaxExposure) { TemplateHandler.RemoveBlock(ref contentTpl, "maxExposure"); colinfo--; colNo--; tblWidth = tblWidth - 50; }
                if (!Functions.Customer.IsVisibleVirtualRate) { TemplateHandler.RemoveBlock(ref contentTpl, "virtualRate"); colinfo--; colNo--; tblWidth = tblWidth - 50; }

                //casino
                if (!WebSettings.disableCS)
                {
                    TemplateHandler.Assign(ref contentTpl, "{lblCSHeader}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCSHeader"));
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref contentTpl, Globals.TAG_CASINO_EDIT);
                }

                TemplateHandler.Assign(ref contentTpl, "{colcom}", colComm.ToString());
                TemplateHandler.Assign(ref contentTpl, "{colinfo}", colinfo.ToString());
                TemplateHandler.Assign(ref contentTpl, "{isAgent}", "0");
                string row = TemplateHandler.CopyBlock(contentTpl, "row");
                TemplateHandler.RemoveBlock(ref contentTpl, "row");

                string row2 = TemplateHandler.CopyBlock(contentTpl, "row2");
                TemplateHandler.RemoveBlock(ref contentTpl, "row2");

                TemplateHandler.Assign(ref contentTpl, "{Group}", "View");
                TemplateHandler.Assign(ref contentTpl, "{groupa}", "Group A");
                TemplateHandler.Assign(ref contentTpl, "{groupb}", "Group B");
                TemplateHandler.Assign(ref contentTpl, "{groupc}", "Group C");
                TemplateHandler.Assign(ref contentTpl, "{groupd}", "Group D");
                TemplateHandler.Assign(ref contentTpl, "{12}", "1x2");
                TemplateHandler.Assign(ref contentTpl, "{Others}", "Others");
                TemplateHandler.Assign(ref contentTpl, "{NG}", "Number Game");
                TemplateHandler.Assign(ref contentTpl, "{minmaxPT}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMinPT"));

                #endregion start code

                var ds = new DataSet();
                var outputParams = new Hashtable();

                hash.Add("pageindex", PageIndex);
                hash.Add("totalpages", TotalPages);
                hash.Add("pagesize", AccCommon.pageSize);
                ds = SelectCustomer.GetSuperList_1(hash, AppSession.SYSTEM, ref outputParams);
                odd_even = (odd_even == "") ? "evenrow" : "";
                if (ds != null)
                {
                    TotalPages = CastAide.AsInt32(outputParams["totalpages"]);
                    if (PageIndex < 1 || PageIndex > TotalPages)
                    {
                        PageIndex = 1;
                    }
                    DataTable tbl = ds.Tables[0];

                    #region if (tbl.Rows.Count > 0)

                    if (tbl.Rows.Count > 0)
                    {
                        TemplateHandler.RemoveBlock(ref contentTpl, "noinfomation");
                        int rowNo = (PageIndex - 1) * AccCommon.pageSize + 1;
                        if (PageIndex == 1) rowNo = 1;
                        DataRowCollection rows = tbl.Rows;

                        foreach (DataRow dr in rows)
                        {
                            string _username = dr["username"].ToString();
                            string strRow = (string)row.Clone();
                            string strRow2 = (string)row2.Clone();
                            string currency = dr["currency"].ToString();
                            TemplateHandler.Assign(ref strRow, "{no}", rowNo.ToString());
                            TemplateHandler.Assign(ref strRow2, "{no}", rowNo.ToString());
                            rowNo++;
                            string strStatus = "";
                            if (!FormatHandler.GetBool(dr["enabled"]))
                            {
                                TemplateHandler.RemoveBlock(ref strRow, "statusEnabled");
                                TemplateHandler.RemoveBlock(ref strRow2, "statusEnabled");
                                strStatus = "Disabled";
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref strRow, "statusDisabled");
                                TemplateHandler.RemoveBlock(ref strRow2, "statusDisabled");
                                if (FormatHandler.GetBool(dr["closed"]))
                                {
                                    strStatus = "Closed";
                                }
                                else if (CastAide.AsBoolean(dr["suspended"]))
                                {
                                    strStatus = "Suspended";
                                }
                                else if (DataUtils.IsExistKey(dr, "isinternal") && CastAide.AsBoolean(dr["isinternal"]))
                                {
                                    strStatus = "Open";
                                }
                                else
                                {
                                    strStatus = "Open";
                                    if (odd_even == "") { odd_even = "evenrow"; } else { odd_even = ""; }
                                }
                            }
                            TemplateHandler.Assign(ref strRow, "{status}", strStatus);
                            TemplateHandler.Assign(ref strRow2, "{status}", strStatus);
                            double maxEx = 0;

                            #region assign template

                            maxEx = FormatHandler.GetDouble(dr["matchlimit"]) * 100;
                            TemplateHandler.Assign(ref strRow, "{maxEx}", FormatHandler.FormatNumber2(maxEx));
                            TemplateHandler.Assign(ref strRow2, "{maxEx}", FormatHandler.FormatNumber2(maxEx));
                            TemplateHandler.Assign(ref strRow, "{custid}", dr["custid"].ToString());
                            TemplateHandler.Assign(ref strRow2, "{custid}", dr["custid"].ToString());
                            TemplateHandler.Assign(ref strRow, "{uplineid}", string.Empty);
                            TemplateHandler.Assign(ref strRow2, "{uplineid}", string.Empty);
                            TemplateHandler.Assign(ref strRow, "{GroupView}", "<span class='canchange'><a href='{link}' target='_self'>View</a></span>");
                            TemplateHandler.Assign(ref strRow, "{link}", "MasterAgentList.aspx?superId=" + dr["custid"].ToString() + "&isSynccasino=" +
                                (dr.Table.Columns.Contains("synccasino") ? dr["synccasino"] : false));
                            TemplateHandler.Assign(ref strRow2, "{link}", "MasterAgentList.aspx?superId=" + dr["custid"].ToString() + "&isSynccasino=" +
                                (dr.Table.Columns.Contains("synccasino") ? dr["synccasino"] : false));
                            TemplateHandler.Assign(ref strRow, "{exval}", maxEx.ToString());
                            TemplateHandler.Assign(ref strRow2, "{exval}", maxEx.ToString());
                            TemplateHandler.Assign(ref strRow, "{virExrate}", CustomerUtils.GetVirtualRate(dr["virtualmultiplier"]));
                            TemplateHandler.Assign(ref strRow2, "{virExrate}", CustomerUtils.GetVirtualRate(dr["virtualmultiplier"]));
                            TemplateHandler.Assign(ref strRow, "{type}", "Super");
                            TemplateHandler.Assign(ref strRow2, "{type}", "Super");

                            TemplateHandler.Assign(ref strRow, "{username}", string.IsNullOrEmpty(_username) ? "&nbsp;" : _username);
                            TemplateHandler.Assign(ref strRow2, "{username}", string.IsNullOrEmpty(_username) ? "&nbsp;" : _username);
                            TemplateHandler.Assign(ref strRow, "{nickname}", dr.IsNull("username2") ? "&nbsp;" : Convert.ToString(dr["username2"]));
                            TemplateHandler.Assign(ref strRow2, "{nickname}", dr.IsNull("username2") ? "&nbsp;" : Convert.ToString(dr["username2"]));
                            TemplateHandler.Assign(ref strRow, "{firstname}", string.IsNullOrEmpty(dr["firstname"].ToString()) ? "&nbsp;" : dr["firstname"].ToString());
                            TemplateHandler.Assign(ref strRow, "{lastname}", string.IsNullOrEmpty(dr["lastname"].ToString()) ? "&nbsp;" : dr["lastname"].ToString());
                            TemplateHandler.Assign(ref strRow, "{groupa}", FormatHandler.FormatNumber4(dr["groupa"]));
                            TemplateHandler.Assign(ref strRow, "{groupb}", FormatHandler.FormatNumber4(dr["groupb"]));
                            TemplateHandler.Assign(ref strRow, "{groupc}", FormatHandler.FormatNumber4(dr["groupc"]));
                            TemplateHandler.Assign(ref strRow, "{groupd}", FormatHandler.FormatNumber4(dr["groupd"]));
                            TemplateHandler.Assign(ref strRow, "{1X2}", FormatHandler.FormatNumber4(dr["discount1x2"]));
                            TemplateHandler.Assign(ref strRow, "{othercom}", FormatHandler.FormatNumber5(dr["discountcs"]));
                            TemplateHandler.Assign(ref strRow, "{loginip}", string.IsNullOrEmpty(IpManagement.ReplaceVNIP(dr["lastloginip"].ToString())) ? "&nbsp;" : dr["lastloginip"].ToString());
                            TemplateHandler.Assign(ref strRow2, "{currency}", currency);

                            #endregion assign template

                            //casino
                            if (!WebSettings.disableCS)
                            {
                                string htmlOpenCS = "onclick=\"OpenEditCS('" + dr["custid"] + "');\"";
                                TemplateHandler.Assign(ref strRow, "{onclickCS}", htmlOpenCS);
                                TemplateHandler.Assign(ref strRow2, "{onclickCS}", htmlOpenCS);
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref strRow, "casino-edit");
                                TemplateHandler.RemoveBlock(ref strRow2, "casino-edit");
                            }


                            if (!WebSettings.EnableVirtualSports)
                            {
                                TemplateHandler.RemoveBlock(ref strRow, "edit-virtualSports");
                                TemplateHandler.RemoveBlock(ref strRow2, "edit-virtualSports");
                            }
                            html.Append(strRow);
                            html2.Append(strRow2);
                        }
                        strpager = Pager.InitPager(TotalPages, PageIndex, "");
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref contentTpl, "haveinfomation");
                        string nohtml = "";
                        nohtml = WebUtils.LoadTemplate("NoInformation.htm");
                        TemplateHandler.Assign(ref nohtml, "{tdno}", colNo.ToString());
                        TemplateHandler.Assign(ref nohtml, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                        html.Append(nohtml);
                        html2.Append(nohtml);
                    }

                    #endregion if (tbl.Rows.Count > 0)
                }
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            TemplateHandler.Assign(ref contentTpl, "{content}", html.ToString());
            TemplateHandler.Assign(ref contentTpl, "{content2}", html2.ToString());
            tblWidth = tblWidth + colNo * 4 + colNo;
            divwidth = tblWidth + 18;
            TemplateHandler.Assign(ref contentTpl, "{tblwidth}", tblWidth.ToString());
            TemplateHandler.Assign(ref contentTpl, "{divwidth}", divwidth.ToString());
            contentTpl += strpager;
            return contentTpl;
        }

        public string GenOperatorListContent(string searchText, string selectAlpha, int PageIndex)
        {
            StringBuilder html = new StringBuilder();
            int TotalPages = 0;
            try
            {
                searchText = (!string.IsNullOrEmpty(searchText) && searchText.Equals("All")) ? "" : searchText;
                selectAlpha = (!string.IsNullOrEmpty(selectAlpha) && selectAlpha.Equals("All")) ? "" : selectAlpha;

                Hashtable hash = new Hashtable();

                if (!string.IsNullOrEmpty(searchText))
                {
                    string keyword = "%" + searchText.Replace("'", "") + "%";
                    hash.Add("keyword", keyword);
                }
                if (!string.IsNullOrEmpty(selectAlpha))
                {
                    string firstchar = selectAlpha;
                    if (string.Compare(firstchar, "0-9", true) == 0)
                    {
                        firstchar = "[" + firstchar + "]";
                    }
                    firstchar = firstchar + "%";
                    hash.Add("firstchar", firstchar);
                }

                DbResult ds = null;
                if (PageIndex < 1)
                {
                    PageIndex = 1;
                }
                hash.Add("pageindex", PageIndex);
                hash.Add("totalpages", TotalPages);
                hash.Add("pagesize", AccCommon.pageSize);
                ds = SelectCustomer.CustomerSelAddOperatorRpt_1(hash);
                TotalPages = CastAide.AsInt32(ds.Outputs["totalpages"]);
                string contentTpl = WebUtils.LoadTemplate("Customer/OperatorListRow.htm");
                string odd_even = "";

                if (ds != null)
                {
                    DataTable tbl = ds.ResultSet.Tables[0];

                    html.Append(WebUtils.LoadHtml("Customer/OperatorListHeader.htm", null, null));

                    #region if (tbl.Rows.Count > 0)

                    if (tbl.Rows.Count > 0)
                    {
                        int rowNo = (PageIndex - 1) * AccCommon.pageSize + 1;
                        if (PageIndex == 1) rowNo = 1;
                        DataRowCollection rows = tbl.Rows;
                        foreach (DataRow dr in rows)
                        {
                            string strRow = contentTpl;
                            odd_even = (odd_even == "") ? "evenrow" : "";
                            TemplateHandler.Assign(ref strRow, "{odd_even}", odd_even);
                            TemplateHandler.Assign(ref strRow, "{no}", rowNo.ToString());
                            TemplateHandler.Assign(ref strRow, "{username}", dr["username"].ToString());
                            TemplateHandler.Assign(ref strRow, "{firstname}", dr["firstname"].ToString());
                            TemplateHandler.Assign(ref strRow, "{lastname}", dr["lastname"].ToString());
                            TemplateHandler.Assign(ref strRow, "{phone}", dr["phone"].ToString());
                            TemplateHandler.Assign(ref strRow, "{mobile}", dr["mobilephone"].ToString());
                            TemplateHandler.Assign(ref strRow, "{email}", dr["email"].ToString());
                            TemplateHandler.Assign(ref strRow, "{role}", dr["roleid"].ToString());
                            TemplateHandler.Assign(ref strRow, "{creator}", dr["creator"].ToString());
                            TemplateHandler.Assign(ref strRow, "{createdate}", dr["creatdate"].ToString());
                            TemplateHandler.Assign(ref strRow, "{role}", dr["username"].ToString());
                            TemplateHandler.Assign(ref strRow, "{operatorId}", dr["custid"].ToString());

                            html.Append(strRow);
                            rowNo++;
                        }
                        html.Append("</table>");
                        html.Append(Pager.InitPager(TotalPages, PageIndex, ""));
                    }
                    else
                    {
                        html.Append(WebUtils.LoadHtml("NoInformation.htm", new string[] { "{tdno}", "{nodata}" },
                                                      new string[] { "13", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") }));
                        html.Append("</table>");
                    }

                    #endregion if (tbl.Rows.Count > 0)
                }
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return html.ToString();
        }
    }
}
