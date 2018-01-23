<%@ WebHandler Language="C#" Class="GetDataOutright" %>

using System;
using System.Web;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Core;
using System.Data;
using System.Text;
using IBCUtils.DAL;
using System.Collections;
using DNAide.Utils;
using DNAide.Web;
using System.Web.SessionState;


public class GetDataOutright : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) 
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);  
        StringBuilder Options = new StringBuilder();

        try
        {
            context.Response.ContentType = "text/html";
            int LeagueID = CastAide.AsInt32(context.Request["LeagueID"], 0);
            int mode = CastAide.AsInt32(context.Request["mode"], 1);
            Sunplus.Accounting.Business.SpecialDelete.SpecialDelete_Outright outright = new Sunplus.Accounting.Business.SpecialDelete.SpecialDelete_Outright();
            if (mode == 1)
            {
                context.Response.Write(outright.GetOutrightByLeagueID(LeagueID));
            }
            if (mode == 2)
            {

                Hashtable hash = new Hashtable();
                hash.Add("outrightid", CastAide.AsInt32(context.Request["OutrightID"], -1));
                hash.Add("fromrefno", CastAide.AsDouble(context.Request["frefno"].Trim(), 0));
                hash.Add("torefno", CastAide.AsDouble(context.Request["trefno"].Trim(), 0));
                hash.Add("username", AppSession.Username.ToLower());

                Sunplus.Accounting.Business.SpecialDelete.SpecialDelete_Outright or = new Sunplus.Accounting.Business.SpecialDelete.SpecialDelete_Outright();
                context.Response.Write(or.GetSpecialDelete(hash));

            }


        }
        catch (InvalidOperationException invalidEx)
        {
            GotoErroPage(context);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            Options.Append("<span style='color:red; font-weight:bold'>" + ex.Message + "</span>");
            context.Response.Write(Options.ToString());
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
