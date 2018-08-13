<%@ WebHandler Language="C#" Class="GetDailyTransaction" %>

using System;
using System.Web;
using DNAide.Web;
using DNAide.Core;
using System.Text;
using DNAide.Data;
using System.Data;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;


public class GetDailyTransaction : BaseHandler {
    
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    int PageIndex = 1, TotalPages = 0;
    
    public override void ProcessRequest (HttpContext context) {
        string _s = string.Empty;
        try
        {
            _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("DailyTransaction.htm");

            DateTime _date = DateTime.Parse(context.Request["txndate"].ToString(), cult);
            string Type = context.Request["txtType"];
            string Creator = context.Request["txtCreator"];
            if (context.Request["PageIndex"] != null)
            {
                int.TryParse(context.Request["PageIndex"], out PageIndex);
                }
            int.TryParse(context.Request["TotalPages"], out TotalPages);

            _date = IBCUtils.FormatHandler.ResetStartDate(_date);

            TransactionContent(_date, ref _s, Type, Creator);
            
        }
        catch (DNAide.Utils.AccException ex)
        {
            _s = AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);
        }
        catch (Exception ex)
        {
           LogExtensions.ExceptionHandler(ex);
            //_s = Resources.Language.thereisnodata;
            GotoErroPage(context);
            context.Response.End();
        }
        context.Response.Write(_s);
    }

    private void TransactionContent(DateTime _date, ref string _s, string Type, string Creator)
    {
        StringBuilder str = new StringBuilder();
        try
        {
            DbResult dataResult = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Transactions.DailyTransactions(_date, PageIndex, ref TotalPages, Type, Creator);
            string sRow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("DailyTxnRow.htm");
            string _newrow = string.Empty;
            string odd_even = "";
            if (dataResult.ResultSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dataResult.ResultSet.Tables[0].Rows)
                {
                    _newrow = sRow;
                    string date_ = string.Format(cult, "{0:M/d/yyyy hh:mm:ss tt}", dr["transdate"]);
                    TemplateHandler.Assign(ref _newrow, "{date}", date_);
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref _newrow, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref _newrow, "{username}", (string)dr["username"]);
                    TemplateHandler.Assign(ref _newrow, "{description}", (string)dr["transdesc"]);
                    TemplateHandler.Assign(ref _newrow, "{type}", (string)dr["typenamee"]);
                    double amount_ = (Convert.ToDouble(dr["amount"])); // decimal ~> double
                    TemplateHandler.Assign(ref _newrow, "{amount}", amount_.ToString(cult));
                    TemplateHandler.Assign(ref _newrow, "{creator}", (string)dr["creator"]);
                    TemplateHandler.Assign(ref _newrow, "{no}", dr["rownumber"].ToString());
                    str.Append(_newrow);
                }
                TemplateHandler.Assign(ref _s, "{content}", str.ToString());
                if (dataResult.ResultSet.Tables[0].Rows.Count > 30)
                {
                    TemplateHandler.RemoveBlock(ref _s, "nodata");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "havingdata");
                }
                TemplateHandler.Assign(ref _s, "{footerpager}", AccCommon.Acc_GenPager(TotalPages, PageIndex, "GetDailyTransactionByPager"));
            }
            else
            {
                TemplateHandler.Assign(ref _s, "{content}", AccCommon.GenNoInformation("7"));
                TemplateHandler.RemoveBlock(ref _s, "havingdata");
                TemplateHandler.Assign(ref _s, "{footerpager}", "");

            }
        }
        catch
        {

            throw;
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
