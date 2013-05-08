using System;
using System.Web.UI;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities.Report;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Services.ReportServices;
using System.Text;

using Sunplus.Accounting.Core.Web;

using Sunplus.Accounting.Services.ReportServices;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web;
using System.Web.UI.WebControls;
using Sunplus.Core.LoggingLib;


public partial class Report_WinLoss_DailyWinLoss : WinLossReporPageBase
{

    public enum ReportLevel
    { 
        Company = 0,
        Super = 1,
        Master = 2,
        Agent = 3,
        Member = 4
    }

    public override ImageButton ReportButton
    {
        get { return btnExport; }
    }

    protected override int CustId
    {
        get
        {
            return Convert.ToInt32(Request.QueryString["uplineid"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtils.SetNoCache();
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["noautorender"] == null)
            {
                if (IsPageValid())
                {
                    BindData();
                }
                else
                {
                    NoInfoPanel.Visible = true;
                    IsExportEnabled = false;
                }
            }
            else
            {
                IsExportEnabled = false;
            }
        }
    }

    

    /// <summary>
    /// report level 
    /// 0: Company
    /// 1: Super
    /// 2: Maser
    /// 3: Agent
    /// if query string contain non-numeric return 0
    /// or level > 4 return 0
    /// </summary>
    /// <returns></returns>
    protected int GetCurrentReportLevel()
    {
        string reportLevel = Request.QueryString["level"];
        int levelDefault = 0;
        if (!string.IsNullOrEmpty(reportLevel))
        {
            int.TryParse(reportLevel, out levelDefault);
        }

        return (levelDefault > Convert.ToInt32(ReportLevel.Member)) ? 0 : levelDefault;
    }

    /// <summary>
    /// validate date to view daily winloss report
    /// it must be in range [BalanceUpDay, Now]
    /// </summary>
    /// <returns></returns>
    public override bool IsPageValid()
    {
        return FromDate <= DateTime.Now
            && FromDate >= ReportUtils.BalanceUp.BalanceUpDay;
    }

 

    private WinLossDailyView _totalItem;
    private static int i;
    protected new readonly string CurrencyFormat = WinLossReporPageBase.CurrencyFormat;
    protected new readonly string TemplateColumn = WinLossReporPageBase.TemplateColumn;
    protected new readonly string TemplateStyleColumn = WinLossReporPageBase.TemplateStyleColumn;

    private void BindData()
    {
        try
        {
            i = 0;

            var entityList = (new WinLossService()).GetDailyWinlossReport(FromDate, GetCurrentReportLevel(), UplineId);
            if (entityList != null && entityList.Count > 0)
            {
                rptWinLoss.DataSource = entityList;
                rptWinLoss.DataBind();
                NoInfoPanel.Visible = false;

                _totalItem = (new WinLossService()).CreateDailyWinLossTotalItem(entityList);
                btnExport.Enabled = true;
            }
            else
            {
                NoInfoPanel.Visible = true;
                btnExport.Enabled = false;
            }
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    
    

   
    protected string RenderFooter()
    {
        if (_totalItem != null)
        {
            return RenderRowItem(_totalItem);
        }

        return string.Empty;
    }
    protected string RenderUserName(object dataItem)
    {
        var entity = (WinLossDailyView)dataItem;
        var html = new StringBuilder();
        i++;      
        html.AppendFormat(WinLossReporPageBase.TemplateStyleColumn, i, "class=No");
        if (GetCurrentReportLevel()+1 == Convert.ToInt32(ReportLevel.Member))
        {
            html.AppendFormat(TemplateStyleColumn, entity.Username, "align='center'");
        }
        else
        {
            html.AppendFormat(TemplateLinkColumn, entity.CustId, entity.Username);
        }

        return html.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataItem"></param>
    /// <returns></returns>
    protected new string RenderRowItem(object dataItem)
    {
        var html = new StringBuilder();
        var entity = (WinLossDailyView)dataItem;
        #region Turnover & Position Taking

        
        html.AppendFormat(TemplateColumn, 
            entity.TotalTO.ToString(CurrencyFormat));
                
        //company
        html.AppendFormat(TemplateColumn,
            entity.Company.Turnover.ToString(CurrencyFormat));
        html.AppendFormat(TemplateColumn,
            entity.Company.GetPositionTaking(entity.TotalTO).ToString(CurrencyFormat));
        //SA
        html.AppendFormat(TemplateColumn,
            entity.SA.Turnover.ToString(CurrencyFormat));
        html.AppendFormat(TemplateColumn,
                    entity.SA.GetPositionTaking(entity.TotalTO).ToString(CurrencyFormat));
        //MA
        html.AppendFormat(TemplateColumn,
           entity.MA.Turnover.ToString(CurrencyFormat));
        html.AppendFormat(TemplateColumn,
                    entity.MA.GetPositionTaking(entity.TotalTO).ToString(CurrencyFormat));

        //Agent
        html.AppendFormat(TemplateColumn,
           entity.Agent.Turnover.ToString(CurrencyFormat));
        html.AppendFormat(TemplateColumn,
                    entity.Agent.GetPositionTaking(entity.TotalTO).ToString(CurrencyFormat));
        #endregion

        #region Win/Loss and Margin
        //Company
        html.AppendFormat(TemplateColumn, entity.Company.Winloss.ToString(CurrencyFormat));
        html.AppendFormat(TemplateStyleColumn, entity.Company.Margin.ToString(CurrencyFormat), "class='Percent'");
        //SA
        html.AppendFormat(TemplateColumn, entity.SA.Winloss.ToString(CurrencyFormat));
        html.AppendFormat(TemplateStyleColumn, entity.SA.Margin.ToString(CurrencyFormat), "class='Percent'");
        //MA        
        html.AppendFormat(TemplateColumn, entity.MA.Winloss.ToString(CurrencyFormat));
        html.AppendFormat(TemplateStyleColumn, entity.MA.Margin.ToString(CurrencyFormat), "class='Percent'");
        //Agent
        html.AppendFormat(TemplateColumn, entity.Agent.Winloss.ToString(CurrencyFormat));
        html.AppendFormat(TemplateStyleColumn, entity.Agent.Margin.ToString(CurrencyFormat), "class='Percent'");

        #endregion
        return html.ToString();
    }

    public override string ReportTitle
    {
        get
        {
            int level = GetCurrentReportLevel();
            return (new WinLossService()).GetDailyReportTitle(level);
        }       
    }


    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        //allow export if and only if export button is enabled
        //prevent user cheat using firebug edit button attribute
        if (IsExportEnabled)
        {
            if (Request.QueryString["noautorender"] == null)
                (this as WinLossReporPageBase).ExportExcelFile();
        }
    }

    public override ExcelFile CreateExcelFile()
    {
        return (new WinLossService()).CreateDailyWinlossReportExcelFile(FromDate, GetCurrentReportLevel(), UplineId);
    }

    public override string ExportFileName
    {
        get
        {
            string fileName = (new WinLossService()).GetDailyReportTitle(GetCurrentReportLevel());
            fileName = fileName.Replace("/", string.Empty).Replace(" ", string.Empty);

            return string.Format("{0}_{1}", fileName, FromDate.ToString("yyyyMMdd"));
        }
    }
}
