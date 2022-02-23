using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using System;
using System.Web.UI;

public partial class CheckEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SendMailExtension.AccSendMail(MailSection.HostName.Gmail, txtMailTo.Value,
                                      string.Empty, string.Empty,
                                      "Nap Acc - Test", "Accounting Test");
    }


}