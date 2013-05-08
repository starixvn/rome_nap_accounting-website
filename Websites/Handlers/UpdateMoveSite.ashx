<%@ WebHandler Language="C#" Class="UpdateMoveCust" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess;

public class UpdateMoveCust : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string mode = "0";
        mode = context.Request["mode"];
        mode = (mode == null) ? "0" : mode;
        switch (mode)
        { 
            case "0"://update site
                var cust = new Sunplus.Accounting.Business.CustSetting.CustSettingMoveSite();
                int CustId = DNAide.Core.CastAide.AsInt32(context.Request["CustId"]);
                string NewSite = context.Request["NewSite"];
                string Msg = cust.UpdateMoveSite(CustId, NewSite);
                context.Response.Write(Msg);
                break;
            case "1"://get site info from username
                System.Collections.Hashtable param = new System.Collections.Hashtable();
                param.Add("username", context.Request["CustId"]);
                DNAide.Data.DbResult ds = DNAide.Data.DbHelper.Execute("Acc_CustomerSelSiteByUserName", param, DBType.ARCHIVE);
                //Site|RoleId|CustId|CustRoleId
                string str = "";
                if (ds.ResultSet.Tables.Count > 0)
                {
                    if (ds.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        System.Data.DataRow dr = ds.ResultSet.Tables[0].Rows[0];
                        str = CastAide.AsString(dr["site"]) + "|";
                        str += CastAide.AsString(dr["custid"]) + "|";
                        if (CastAide.AsString(dr["custid"]) == AppSession.CustID.ToString() || AppSession.AgentID.ToString() == CastAide.AsString(dr["custid"]) || AppSession.MasterID.ToString() == CastAide.AsString(dr["custid"]) || AppSession.SuperID.ToString() == CastAide.AsString(dr["custid"]))
                        {
                            str += "1";
                        }
                        else
                        {
                            str += "0";
                        }
                            
                    }
                }
                context.Response.Write(str);
                break;
        }
        
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}