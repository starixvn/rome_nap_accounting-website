<%@ WebHandler Language="C#" Class="GetData" %>

using System;
using System.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;

public class GetData : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {

        try
        {
            context.Response.ContentType = "text/html";
            int matchId = 0;
            long frefNo = 0;
            long trefNo = 0;
            string betTypes = context.Request.Form["bettype"];
            string typename = context.Request.Form["bettypename"];

            System.Text.StringBuilder html = new System.Text.StringBuilder();

            int type = 0;
            int.TryParse(context.Request["type"], out type);
            if (!int.TryParse(context.Request.Form["matchid"], out matchId) ||
                !long.TryParse(context.Request.Form["frefno"], out frefNo) ||
                !long.TryParse(context.Request.Form["trefno"], out trefNo))
            {
                html.Append(AlertMessage.GetMsg("Invalid Input Data!", AlertMessage.MsgType.Error));
                context.Response.Write(html.ToString());
                //GotoErroPage(context);
                return;
            }

            int liveindicator = DNAide.Core.CastAide.AsInt32(context.Request["ddlLiveIndicator"], 2);
            string betteam1378 = DNAide.Core.CastAide.AsString(context.Request["ddlBetTeam1378"], "All");
            string betteam515 = DNAide.Core.CastAide.AsString(context.Request["ddlBetTeam515"], "All");
            string betteam4 = context.Request.Form["betteam4"];

            string hdp = context.Request.Form["hdp"];
            hdp = (string.IsNullOrEmpty(hdp)) ? "9999" : hdp;

            System.Collections.Hashtable param = new System.Collections.Hashtable();
            param.Add("matchid", matchId);
            param.Add("fromrefno", frefNo);
            param.Add("torefno", trefNo);
            param.Add("hdp", hdp);
            param.Add("liveindicator", liveindicator);
            param.Add("bettypes", betTypes);
            param.Add("betteam1378", betteam1378);
            param.Add("betteam515", betteam515);
            param.Add("betteam4", betteam4);
            param.Add("username", AppSession.Username);
            param.Add("typename", typename);
            param.Add("bettype", betTypes);

            Sunplus.Accounting.Business.SpecialDelete.SpecialDelete sp = new Sunplus.Accounting.Business.SpecialDelete.SpecialDelete();
            string sData = sp.GetSpecialDelete(param);
            context.Response.Write(sData);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            GotoErroPage(context);
        }
        catch (System.Exception)
        {

            throw;
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
