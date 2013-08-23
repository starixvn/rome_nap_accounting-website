using System;
using System.Data;
using System.Text;

using DNAide.Web;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;

public partial class NewCustomer_SelectType : PageBase
{
    private string content = "";
    private string strSelect = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["listType"] == null)
        {
            SelectTypeContentHtml.Text = GenContent(true);
        }
        else
        {
            strSelect = Request["listType"];
            SelectTypeContentHtml.Text = GenContentInput();
        }
    }

    private string GenContent(bool isFirstLoad)
    {
        StringBuilder html = new StringBuilder();
        try
        {
            content = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/SelectType.htm");
            string currAccount = Session["CustUname"] as string ?? string.Empty;
            string strMember = "member", strAgent = "agent", strMaster = "master", strSuper = "super";
            if (isFirstLoad)
            {
                TemplateHandler.Assign(ref content, "{fSuper}", "none");
                TemplateHandler.Assign(ref content, "{fInput}", "");
                TemplateHandler.Assign(ref content, "{accUsername}", "Agent's username");
                TemplateHandler.Assign(ref content, "{next}", "Next");
            }
            else
            {
                switch (strSelect)
                {
                    case "member":
                        strMember = "member\" selected=\"true";
                        break;

                    case "agent":
                        strAgent = "agent\" selected=\"true";
                        break;

                    case "master":
                        strMaster = "master\" selected=\"true";
                        break;

                    case "super":
                        strSuper = "super\" selected=\"true";
                        break;

                    default:
                        break;
                }
            }
            if (string.IsNullOrEmpty(currAccount))
            {
                TemplateHandler.Assign(ref content, "{curnone}", "none");
            }
            else
            {
                string custRoleId = Session["custRoleId"].ToString();
                string valueBtn = "";
                switch (custRoleId)
                {
                    case "4":
                        valueBtn = "Add New Master to ";
                        break;

                    case "3":
                        valueBtn = "Add New Agent to ";
                        break;

                    case "2":
                        valueBtn = "Add New Member to ";
                        break;

                    case "1":
                        TemplateHandler.Assign(ref content, "{curnone}", "none");
                        break;

                    default:
                        break;
                }
                TemplateHandler.Assign(ref content, "{accCurrent}", valueBtn + currAccount);
            }

            TemplateHandler.Assign(ref content, "{vmember}", strMember);
            TemplateHandler.Assign(ref content, "{vagent}", strAgent);
            TemplateHandler.Assign(ref content, "{vmaster}", strMaster);
            TemplateHandler.Assign(ref content, "{vsuper}", strSuper);
            TemplateHandler.Assign(ref content, "{super}", Resources.Language.Super);
            TemplateHandler.Assign(ref content, "{newmember}", Resources.Language.AddNewMember);
            TemplateHandler.Assign(ref content, "{newagent}", Resources.Language.AddNewAgent);
            TemplateHandler.Assign(ref content, "{newmaster}", Resources.Language.AddNewMaster);
            TemplateHandler.Assign(ref content, "{newsuper}", Resources.Language.AddNewSuper);
            TemplateHandler.Assign(ref content, "{selecttype}", Resources.Language.SelectType);
            html.Append(content);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }

    private string GenContentInput()
    {
        StringBuilder html = new StringBuilder();
        try
        {
            GenContent(false);
            switch (strSelect)
            {
                case "member":
                    TemplateHandler.Assign(ref content, "{fSuper}", "none");
                    TemplateHandler.Assign(ref content, "{fInput}", "");
                    TemplateHandler.Assign(ref content, "{accUsername}", Resources.Language.AgUsername);
                    break;

                case "agent":
                    TemplateHandler.Assign(ref content, "{fSuper}", "none");
                    TemplateHandler.Assign(ref content, "{fInput}", "");
                    TemplateHandler.Assign(ref content, "{accUsername}", Resources.Language.MAgUsername);
                    break;

                case "master":
                    TemplateHandler.Assign(ref content, "{fSuper}", "");
                    TemplateHandler.Assign(ref content, "{fInput}", "none");
                    TemplateHandler.Assign(ref content, "{createmaster}", Resources.Language.CreateMasterAgent);
                    TemplateHandler.Assign(ref content, "{listSuperValue}", GetListSuper(AppSession.SYSTEM));
                    TemplateHandler.Assign(ref content, "{listCurrValue}", CustomerUtils.GetListCurrencyBySystem(AppSession.SYSTEM, false, false, false));
                    TemplateHandler.Assign(ref content, "{listSiteValue}", CustomerUtils.GetListSite(false, AppSession.SYSTEM));
                    TemplateHandler.Assign(ref content, "{curency}", Resources.Language.Currency);
                    TemplateHandler.Assign(ref content, "{site}", Resources.Language.Site);
                    TemplateHandler.Assign(ref content, "{companydirect}", Resources.Language.CompanyDirect);
                    break;

                default:
                    TemplateHandler.Assign(ref content, "{fSuper}", "none");
                    TemplateHandler.Assign(ref content, "{fInput}", "none");
                    break;
            }
            TemplateHandler.Assign(ref content, "{next}", Resources.Language.Next);
            html.Append(content);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }

    private static string GetListSuper(string accountingSystem)
    {
        var html = new StringBuilder();
        try
        {
            var ds = SelectCustomer.CustomerSelSAgListRpt(accountingSystem);
            if (ds != null & ds.Tables.Count > 0)
            {
                DataTable tbl = ds.Tables[0];
                foreach (DataRow dr in tbl.Rows)
                {
                    html.Append("<option value=\"" + dr["custid"] + "\">" + dr["username"] + "</option>");
                }
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
}