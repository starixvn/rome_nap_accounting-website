using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.Web;
using IBCUtils;


public partial class Statement_AllStatement : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            string fdate = Request["fdate"];
            string tdate = Request["tdate"];
            DateTime fdt, tdt;
            if (!Validator.ValidDateMMDDYYYYNotNull(fdate, out fdt).IsSuccess())
                if (Session["fdate"] != null)
                    fdt = (DateTime)Session["fdate"];
                else
                    fdt = DateTime.Now.AddDays(-1);
            if (!Validator.ValidDateMMDDYYYYNotNull(tdate, out tdt).IsSuccess())
                if (Session["tdate"] != null)
                    tdt = (DateTime)Session["tdate"];
                else
                    tdt = DateTime.Now;
            Session["fdate"] = fdt;
            Session["tdate"] = tdt;

            //StringBuilder betlistHtml = new StringBuilder("<form>" + AccCommon.CreateDateRangeSelector("", 7, "fdate", "tdate", fdt, tdt) + "</form>");
            StringBuilder betlistHtml = new StringBuilder("<form>" + Calendar.CreateDateRangeSelector("",  "fdate", "tdate", fdt, tdt,false, 3, 0, "") + "</form>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        
    }
}
