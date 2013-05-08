using System;
using System.Configuration;
using Fanex.Tarus.Infrastructure.Configurations;
using Fanex.Tarus.Infrastructure.Resource;
using Sunplus.Accounting.Core.Common;

public partial class _Default : PageBase
{
    #region local variables

    public string accUsername = "";
    protected string fromDate = string.Empty;
    protected string toDate = string.Empty;
    protected string getReportLegendUrl = string.Empty;
    public string listMenuIgnore = string.Empty;
    protected int noteViewCountToCollapse;
    protected bool isLegendEnable;
    protected int currentUserId;
    protected string inlineImageExpand = string.Empty;
    protected string inlineImageCollapse = string.Empty;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        fromDate = DateTime.Now.ToShortDateString();
        toDate = DateTime.Now.ToShortDateString();
        getReportLegendUrl = AppSettings.GetReportLegendUrl;
        noteViewCountToCollapse = AppSettings.NoteViewCountToCollapse;
        isLegendEnable = AppSettings.IsLegendEnable;

        currentUserId = AppSession.AccId;
        inlineImageExpand = Labels.SubTabHeadLineIcon_Expand_Image;
        inlineImageCollapse = Labels.SubTabHeadLineIcon_Collapse_Image;

        if (!Page.IsPostBack)
        {
            try
            {
                listMenuIgnore = ConfigurationManager.AppSettings["ListIgnoreMenu"].ToString();
                AccCommon.LeaveSSL();
                accUsername = AppSession.AccUserName;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }
    }

    public string GetUTCOffSet()
    {
        if (TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours > 0)
        {
            return "+" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours.ToString();
        }
        else
        {
            return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours.ToString();
        }
    }
}

