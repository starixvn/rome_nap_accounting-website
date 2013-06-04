<%@ WebHandler Language="C#" Class="DeleteBet" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Core.Web;


public class DeleteBet : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {
        try
        {
            string betId = string.Empty;
            string betFrom = string.Empty;
            int matchId = 0;

            betId = CastAide.AsString(context.Request["betid"]);
            betFrom = CastAide.AsString(context.Request["betfrom"]);
            matchId = CastAide.AsInt32(context.Request["matchid"]);
            var bettype = CastAide.AsInt32(context.Request["bettype"]);
            string reason = CastAide.AsString(context.Request["reason"]);
            
            context.Response.Write(Sunplus.Accounting.Business.BetList.DeleteBet.DelBet(betId, betFrom, matchId, bettype, reason));
        }
        catch (System.Exception e)
        {
            //context.Response.End();
            GotoErroPage(context);
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
