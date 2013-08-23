using System;
using System.Collections.Generic;
using DNAide.Core;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Transaction;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.Entities.ViewModel;

public partial class DailyTransaction : PageBase
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    public string sTitle = "";
    public string strCalendar = "";
    protected string bettypesList = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string _s = string.Empty;
        try
        {
            sTitle = Resources.Language.DailyFinanceTitle;

            //why reset startdate .AddDays(-1)
            DateTime _date;
            _date = DateTime.Now;
            _date = IBCUtils.FormatHandler.ResetStartDate(_date);

            //Phuong.Nguyen@Updated: <2009-01-03>, show other days of next month
            //Anders IRP 444.4 Do now show future days
            strCalendar = Calendar.GenCalendarControl(
                "01012008",
                _date.ToString("yyyyMMdd"),
                "txndate",
                "../",
                "btndate",
                string.Format(cult, "{0:MM/dd/yyyy}", _date.AddDays(-1)),
                false,
                false,
                string.Empty,
                string.Empty,
                string.Empty);

            #region get bettyles

            var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
            List<FinanceBettype> entityList = userService.GetFinanceBettyles();
            foreach (FinanceBettype fb in entityList)
            {
                bettypesList += string.Format("<option value=\"{0}\">{0}</option>", fb.TypeName);
            }
            if (Request["IsExport"] == "1")
            {
                var DailyAllTxn = CommonBootStrapper.Locator.GetInstance<IServiceDailyTran>();
                DailyAllTxn.ExportToExcelDailyFinance(CastAide.AsString(Request["transDate"]), CastAide.AsInt32(Request["status"]), CastAide.AsString(Request["type"]), CastAide.AsString(Request["creator"]), CastAide.AsString(Request["verifyBy"]));
            }

            #endregion get bettyles
        }
        catch (AccException ex)
        {
            _s = AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);
            Logs.Write(ex.Message);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }
}