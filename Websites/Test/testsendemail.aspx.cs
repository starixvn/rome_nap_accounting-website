using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Core.Common;
using Fanex.Tarus.Infrastructure.Extensions;


public partial class Test_testsendemail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool f = SendMailExtension.AccSendMail("robert.job@outlook.com", "", "", "Testing", "Testing", "testing", null);
        

     

        if(f)
            Response.Write("ok ");
        else
        {
            Response.Write("fail");
        }
    }

    
}
