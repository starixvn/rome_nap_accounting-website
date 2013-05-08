using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Entities.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Services.ReportServices;
using System.Text;
using System.Web.UI.HtmlControls;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Web;

public partial class Report_WinLoss_Default : WinLossReporPageBase
{


    public override string CurrentUrl
    {
        get
        {
            return "Default.aspx";
        }
    }

    public override string ReportTitle
    {
        get
        {
            
            return (UplineId <= 0) ? "Super Win/Loss Detail" : "Master Win/Loss Detail";
        }
    }

    public override string DownlineUrl
    {
        get
        {
            return (UplineId <= 0) ? "Default.aspx" : "MasterWinLossDetail.aspx";
        }
    }

    public override Repeater ReportControl
    {
        get
        {
            return rptWinLoss;
        }
    }

    public override string ExportFileName
    {
        get
        {
            return  string.Format((UplineId <= 0) ? WinLossService.ReportNameSuperWinLossDetail : 
                WinLossService.ReportNameMasterWinLossDetail, FromDate.ToString("yyyyMMdd") + "_" + ToDate.ToString("yyyyMMdd")) ;
        }
    }

    public override ExcelFile CreateExcelFile()
    {
        return (UplineId <= 0) ? (new WinLossService()).CreateExcelFileSuperWinloss(Constants.SystemCurrency.Thai, FromDate, ToDate, Option)
            : (new WinLossService()).CreateExcelFileMasterWinloss(UplineId, FromDate, ToDate, Option);
    }

    public override HtmlGenericControl NoInfoPanel
    {
        get
        {
            return noInfoPanel;
        }
    }

    public override System.Collections.IList GetReportData()
    {
        List<MasterWinLoss> entityList;
        var winlossService = new WinLossService();
        int uplineId = UplineId;
        if (uplineId <= 0)
        {
            entityList = winlossService.GetSuperReportData(Constants.SystemCurrency.Thai, this.FromDate, this.ToDate, this.Option);
        }
        else
        {
            entityList = winlossService.GetMastertReportData(uplineId, FromDate, ToDate, Option);
        }

        if (entityList.Count > 0)
        {
            lastItem = winlossService.CreateTotalLastItem(entityList);
        }

        return entityList;
    }


    protected override string RenderWinLossCommTotalByReport(IWinLoss winloss)
    {
        var itemHtml = new StringBuilder();

        //render winloss comm and total
        itemHtml.Append(RenderWinLossCommTotal((winloss as MasterWinLoss).Member));
        itemHtml.Append(RenderWinLossCommTotal((winloss as MasterWinLoss).Agent));
        itemHtml.Append(RenderWinLossCommTotal((winloss as MasterWinLoss).Master));
        itemHtml.Append(RenderWinLossCommTotal((winloss as MasterWinLoss).Super));

        return itemHtml.ToString();
    }      
   
}
