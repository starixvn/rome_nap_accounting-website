using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using DNAide.Utils;
using System.Text;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

using BLL.Report.FI;

using Sunplus.Core.LoggingLib;

public partial class Report_FIWLDetails : PageBase
{

    public StringBuilder html = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        try
        {
            string bettype = Request["bettype"];

            DateTime fdate = CastAide.AsDateTime(Request["fdate"], DateTime.Today);
            DateTime tdate = CastAide.AsDateTime(Request["tdate"], DateTime.Today);

            if (Request["type"] == "Direct")
            {
                string script = "<script type ='text/javascript'>";
                script += "parent.showProgressBar(false);";
                script += "</script>";
                Response.Write(script);
            }

            FIWLDetails fi = new FIWLDetails();

            Hashtable hash = new Hashtable();
            hash.Add("fromdate", fdate);
            hash.Add("todate", tdate);

            if (Request["IsExport"] == "1")
            {
                fi.Export(hash);
            }
            else
            {
                html.Append(fi.RenderHTML(hash).Replace("{border}", "0"));
            }
        }
        catch (InvalidOperationException invalidOEx)
        {


            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(invalidOEx);
            RedirectDeny();
        }
    }

    
}
