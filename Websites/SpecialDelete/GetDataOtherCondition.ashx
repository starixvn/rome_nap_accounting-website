<%@ WebHandler Language="C#" Class="GetDataOtherCondition" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.SpecialDelete;

public class GetDataOtherCondition : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {

        try
        {
            context.Response.ContentType = "text/html";

            SpecialDeleteOtherCondition sp = new SpecialDeleteOtherCondition();
            System.Collections.Hashtable param = new System.Collections.Hashtable();
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