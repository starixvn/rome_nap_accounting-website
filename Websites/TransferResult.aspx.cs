using System;
using System.Net;

using DNAide.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Helper;

public partial class TransferResult : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.sTitle.Text = "Transfer";
            double exrate = IBCUtils.FormatHandler.GetDouble(Request["exrate"]);

            var fromUser = WebUtility.HtmlDecode(Request["fuser"]);
            var toUser = WebUtility.HtmlDecode(Request["touser"]);
            DbResult dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Transfer.BettransInsertTransfer_Confirm(fromUser, toUser, exrate);

            string s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("TransferResult.htm");
            TemplateHandler.Assign(ref s, "{fuser}", fromUser);
            TemplateHandler.Assign(ref s, "{fusercurr}", dr.Outputs["fromcurrency"].ToString());
            TemplateHandler.Assign(ref s, "{tuser}", toUser);
            TemplateHandler.Assign(ref s, "{tusercurr}", dr.Outputs["tocurrency"].ToString());

            string _message = dr.Outputs["errmsg"].ToString();
            if (DNAide.Core.CastAide.AsInt16(dr.Outputs["errcode"]) != 0)
            {
                if (!string.IsNullOrEmpty(_message)) _message = AlertMessage.GetMsg(_message, AlertMessage.MsgType.Error);
            }
            else
            {
                if (!string.IsNullOrEmpty(_message)) _message = AlertMessage.GetMsg(_message, AlertMessage.MsgType.Success);
            }
            if (string.IsNullOrWhiteSpace(_message.Trim()))
            {
                //TemplateHandler.RemoveBlock(ref s, "message");
                TemplateHandler.Assign(ref s, "{message}", string.Empty);
            }
            else
            {
                TemplateHandler.Assign(ref s, "{message}", _message);
            }
            this.sMain.Text = s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }
}