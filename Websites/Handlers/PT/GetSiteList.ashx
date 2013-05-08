<%@ WebHandler Language="C#" Class="GetCurrencyList" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;

public class GetCurrencyList : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.ContentEncoding = Encoding.UTF8;
        JObject json = new JObject();
        DataRowCollection SiteRowList = SelectCustomer.SiteSelRpt(AppSession.SYSTEM).ResultSet.Tables[0].Rows;
        List<Site> SiteList = new List<Site>();
        foreach (DataRow row in SiteRowList)
        {
            SiteList.Add(new Site(row["site"].ToString()));     
        }
        json["SiteData"] = new JValue(JsonConvert.SerializeObject(SiteList));
        context.Response.Write(JsonConvert.SerializeObject(json));
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

public class Site
{
    string _Value;

    public Site(string value)
    {
        _Value=value;
    }

    public string Value
    {
        get { return _Value; }
        set { _Value = value; }
    }
}