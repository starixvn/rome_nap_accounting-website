using System;
using Sunplus.Accounting.Business.Customer;
using System.Collections;
using DNAide.Core;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Extensions;

public partial class CustInfo : PageBase
{
    //protected override object FunctionName
    //{
    //    get
    //    {
    //        return Enumerations.Functions.Customer.ViewInfo;

    //    }
    //}
    public bool isBA, isEnabled;
    public string arrAbnomalBet="";
    public string username = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        username = System.Web.HttpUtility.UrlDecode(Request.QueryString["username"]);
        if (string.IsNullOrEmpty(username))
        {
            HtmlHolder.Text = string.Empty;
            return;
        }
        CustomerManager customer = new CustomerManager();
        try {
          Hashtable hash = customer.GetHtmlCustInfoContent(username.Trim());
            //HtmlHolder.Text = customer.GetHtmlCustInfoContent(username.Trim());
            HtmlHolder.Text = CastAide.AsString(hash["strcontent"]);
            isBA = hash.ContainsKey("isba") ? CastAide.AsBoolean(hash["isba"]):false;
        }
        catch(Exception ex){
            HtmlHolder.Text = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
        }
        
       
    }
}

