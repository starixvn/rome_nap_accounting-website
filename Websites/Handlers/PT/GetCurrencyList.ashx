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
        DataRowCollection CurrencyRowList = SelectCustomer.ExchangeSelRpt(AppSession.SYSTEM).ResultSet.Tables[0].Rows;
        List<Currency> CurrencyList = new List<Currency>();
        foreach (DataRow row in CurrencyRowList)
        { 
            CurrencyList.Add(new Currency(row["exchangeid"].ToString() + "_" + row["HRAllowed"].ToString(), row["currency"].ToString()));     
        }
        json["CurrencyData"] = new JValue(JsonConvert.SerializeObject(CurrencyList));
        context.Response.Write(JsonConvert.SerializeObject(json));
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

public class Currency
{
    string _Value;
    string _Text;

    public Currency(string value, string text)
    {
        _Value=value;
        _Text=text;
    }

    public string Value
    {
        get { return _Value; }
        set { _Value = value; }
    }
 
    public string Text
    {
        get { return _Text; }
        set { _Text = value; }
    }
}