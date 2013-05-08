<%@ WebHandler Language="C#" Class="GenBetList" %>

using System;
using System.Web;
using DNAide.Core;
using IBCUtils;
using System.Web.SessionState;
using Sunplus.Accounting.Business.BetList;

public class GenBetList : BaseHandler{
    
    public override void ProcessRequest (HttpContext context) {
        try
        {
            int requestType = CastAide.AsInt32(context.Request["requesttype"], 0);
            if (requestType == 1) //filter by bettype
            {
                BetListMain betlist = new BetListMain();
                string fdate = context.Request["fdate"];
                string tdate = context.Request["tdate"];
                DateTime fdt, tdt;
                if (!Validator.ValidDateMMDDYYYYNotNull(fdate, out fdt).IsSuccess())
                    if (context.Session["fdate"] != null)
                        fdt = (DateTime) context.Session["fdate"];
                    else
                        fdt = DateTime.Now.AddDays(-1);
                if (!Validator.ValidDateMMDDYYYYNotNull(tdate, out tdt).IsSuccess())
                    if (context.Session["tdate"] != null)
                        tdt = (DateTime) context.Session["tdate"];
                    else
                        tdt = DateTime.Now;

                context.Session["fdate"] = fdt;
                context.Session["tdate"] = tdt;
                int Bettype = CastAide.AsInt32(context.Request["bettype"], 0);
                context.Response.Write(betlist.GenMainReport(fdt, tdt, Bettype));
            }
            else if (requestType == 2) //filter by transid
            {
                BetListMain betlist = new BetListMain();
                string transID = CastAide.AsString(context.Request["transid"]);
                if (context.Request["custId"] != null)
                {
                    
                    int custId = CastAide.AsInt32(context.Request["custId"]);// add param to view betlist of refno for checkbet
                    context.Response.Write(betlist.GenMainReport(transID, custId));
                }
                else
                    context.Response.Write(betlist.GenMainReport(transID));
            }
            else if (requestType == 3) //filter by IP & date (GET Bet list By IP)
            {
                BetlistByIP betlist = new BetlistByIP();
                string ip = CastAide.AsString(context.Request["ip"]);
                DateTime fdt;
                fdt = CastAide.AsDateTime(context.Request["fdate"]);
                context.Response.Write(betlist.GenReportByIP(fdt, ip));
            }
            else if (requestType == 4) //Get Bet List By IP Detail of each customer
            {
                BetlistByIP betlist = new BetlistByIP();
                string ip = CastAide.AsString(context.Request["ip"]);
                DateTime fdt;
                fdt = CastAide.AsDateTime(context.Request["fdate"]);
                string type = CastAide.AsString(context.Request["type"]);
                int custid = CastAide.AsInt32(context.Request["custid"]);
                context.Response.Write(betlist.GenReportByIPDetail(custid, ip, fdt, type));
            }
            else if (requestType == 5) // Get Statement Betlist
            {
                DateTime fdt;
                fdt = CastAide.AsDateTime(context.Request["fdate"]);
                var userName = context.Request["username"];
                var transTypeId = CastAide.AsInt32(context.Request["transtypeid"]);
                BetlistByStatement betlist = new BetlistByStatement();
                var betListHtml = betlist.GenReportStatement(fdt, userName, transTypeId);
                if (!string.IsNullOrEmpty(betListHtml))
                    context.Session["statement.betlist." + userName + ".fdate" + fdt + "." + transTypeId] = betListHtml;
                
                context.Response.Write(betListHtml);
            }
            else if (requestType == 6) // Get all Bettypes for a customer
            {
                DateTime fdt, tdt;
                fdt = CastAide.AsDateTime(context.Request["fdate"]);
                tdt = CastAide.AsDateTime(context.Request["tdate"]);
                BetListMain betlist = new BetListMain();
                context.Response.Write(betlist.GenAllBettype(fdt, tdt));
            }
            else
            {
                GotoErroPage(context);
            }
        }
        catch(Exception ex)
        {
            GotoErroPage();
        }

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
