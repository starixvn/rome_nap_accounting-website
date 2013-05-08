<%@ WebHandler Language="C#" Class="GenMixParlay" %>

using System;
using System.Web;
using DNAide.Core;
using System.Web.SessionState;
using Sunplus.Accounting.Business.BetList;

public class GenMixParlay : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {

        try
        {
            int type = CastAide.AsInt32(context.Request["type"]);
            string refno = CastAide.AsString(context.Request["refno"]);
            string strMPRow = string.Empty;
            if (type == 1)
            {
                int listType = CastAide.AsInt32(context.Request["listtype"]);
                BetListMain betlist = new BetListMain();
                strMPRow = betlist.GenMPRow(refno, listType);
                context.Response.Write(strMPRow);
            }
            else if (type == 2)
            {
                BetlistGenerateEvent BG = new BetlistGenerateEvent();
                strMPRow = BG.GenMixParlay(refno);
                context.Response.Write(strMPRow);
            }
            else
            {
                GotoErroPage(context);
            }
        }
        catch(Exception ex)
        {
            GotoErroPage(context);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
