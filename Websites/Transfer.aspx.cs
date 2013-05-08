using System;
using System.Collections;

using DNAide.Utils;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using IBCUtils;
using DNAide.Web;


using DNAide.Core;
using DNAide.Data;


public partial class Transfer : System.Web.UI.Page
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    public bool IsHadPermissionOnCrossCurrencyTransfer = false;
    public bool IsHadPermissionOnYesterdayTransfer = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        string _s = string.Empty;
        try
        {
            int _accid = Convert.ToInt32(Session["AccId"] as object ?? -1);
            if (_accid == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);

            string _custname = Request["fuser"] != null ? Request["fuser"] : Session["CustUname"] as string ?? string.Empty;
            string custId = Request["fcusid"] != null ? Request["fcusid"] : "0";

            if (string.IsNullOrEmpty(_custname)) throw new AccException(IbcError.USERNAME_UNAVAILABLE_MSG);
            string _amount = Request["amount"] as string ?? string.Empty;
            string _sHeader = "Transfer Yesterday's Transactions";
            
            this.sHeader.Text = _sHeader;
            _amount = (string.IsNullOrEmpty(_amount) || Convert.ToDouble(_amount) == 0.0) ? "0.00" : (Convert.ToDouble(_amount) * -1).ToString();
            _amount = (_amount == "0.0") ? "0.00" : _amount;
            _s = "<form method='post' action='Handlers/Transfer.ashx?fromCustName=" + AppSession.CustUName + "&fromCustID=" + AppSession.CustID + "' target='frmTransfer'>" 
                + Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Transfer.htm") + "</form>";

            var isTransferFromSuperBalance = false;
            if (Request["isTransferFromSuperBalance"] != null)
                isTransferFromSuperBalance =
                    CastAide.AsBoolean(Request["isTransferFromSuperBalance"], false);

            //Do transfer from Super Balance page
            if(isTransferFromSuperBalance)
            {
                var hash = new Hashtable {{"keyword", _custname}};
                string db = (CastAide.AsString(Request["IsRep"], "0").Equals("1")) ? "rep" : "main";
                bool isYesterday = CastAide.AsString(Request["IsYesterDay"], "0").Equals("1");

                var superBalance = IBCProvider.ReportProvider.DownLineBalance().RptSABalance(db, isYesterday, hash);//super
                if(superBalance != null && superBalance.ResultSet != null && superBalance.ResultSet.Tables.Count >0 && superBalance.ResultSet.Tables[0].Rows.Count >0)
                {
                    _amount = (-CastAide.AsDouble(superBalance.ResultSet.Tables[0].Rows[0]["balance"], 0)).ToString();
                    _amount = NumberFormat.DecFormat(_amount, 2);
                }
            }

            IsHadPermissionOnCrossCurrencyTransfer = Sunplus.Accounting.Business.Security.Permissions.HasPermission("FinanceTransfer");
            IsHadPermissionOnYesterdayTransfer = Sunplus.Accounting.Business.Security.Permissions.HasPermission("OtherTrans");

            if (!IsHadPermissionOnYesterdayTransfer && !IsHadPermissionOnCrossCurrencyTransfer)
            {
                this.Response.Redirect(Constants.Page.AccessDenied, true);
            }

            TemplateHandler.Assign(ref _s, "{title}", Resources.Language.Transfer);
            TemplateHandler.Assign(ref _s, "{amount}", _amount);
          
            TemplateHandler.Assign(ref _s, "{custname}", _custname);
            TemplateHandler.Assign(ref _s, "{hdCustId}", custId);

            hiddenSearchedName.Value = _custname;
            hiddenLoginID.Value = AppSession.CustID.ToString();
            
        }
        catch (AccException ex)
        {
            _s = AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);            
        }
        catch (Exception eex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(eex);
        }
        this.sMain.Text = _s;
    }

    

}
