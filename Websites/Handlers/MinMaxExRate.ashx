<%@ WebHandler Language="C#" Class="MinMaxExRate" %>

using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using System.Web;

public class MinMaxExRate : BaseHandler
{
    public override void ProcessRequest(HttpContext context)
    {
        try
        {
            if (Permissions.HasHostPermission(Enumerations.Functions.ExRates.MinMaxExRate))
            {
                context.Response.ContentType = "text/html";
                string status = string.IsNullOrEmpty(context.Request.Form["sta"]) ? "Add" : context.Request.Form["sta"];
                int fromcurId = string.IsNullOrEmpty(context.Request.Form["frmId"])
                    ? 0
                    : CastAide.AsInt32(context.Request.Form["frmId"]);
                int tocurId = string.IsNullOrEmpty(context.Request.Form["toId"])
                    ? 0
                    : CastAide.AsInt32(context.Request.Form["toId"]);
                double min = string.IsNullOrEmpty(context.Request.Form["min"])
                    ? 0
                    : CastAide.AsDouble(context.Request.Form["min"]);
                double max = string.IsNullOrEmpty(context.Request.Form["max"])
                    ? 0
                    : CastAide.AsDouble(context.Request.Form["max"]);
                min = min / 100.00;
                max = max / 100.00;

                int errcode = 0;
                DbResult dbr = null;
                switch (status)
                {
                    case "Add": //insert
                        dbr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Balance.ExChangeRateIns(fromcurId, tocurId, min, max,
                            CastAide.AsInt32(context.Session["AccId"]),
                            context.Request.UserHostAddress);

                        break;

                    case "Edit": //update
                        dbr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Balance.ExChangeRateUpd(fromcurId, tocurId, min, max,
                            CastAide.AsInt32(context.Session["AccId"]),
                            context.Request.UserHostAddress);
                        break;
                }

                errcode = CastAide.AsInt32(dbr.Outputs["errcode"]);
                if (errcode == 0)
                {
                    context.Response.Write("Ok");
                }
                else
                {
                    context.Response.Write(dbr.Outputs["errmsg"].ToString());
                }
            }
            else
            {
                context.Response.Write("You are not authorized on this function");
            }
        }
        catch (System.Exception)
        {
            GotoErroPage(context);
        }
    }

    public override bool IsReusable
    {
        get
        {
            return false;
        }
    }
}