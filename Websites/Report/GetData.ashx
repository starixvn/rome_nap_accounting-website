<%@ WebHandler Language="C#" Class="GetData" %>

using System;
using System.Web;
using DNAide.Core; 
using System.Collections;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Presentation;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GetData : IHttpHandler, IRequiresSessionState  {

    JObject o = new JObject();
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html"; 

        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        
        string mode = context.Request["mode"];
        mode = (string.IsNullOrEmpty(mode)) ? "0" : mode;

        string db = (CastAide.AsString(context.Request["IsRep"],"0").Equals("1")) ? "rep" : "main";
        bool IsYesterday = CastAide.AsString(context.Request["IsYesterDay"], "0").Equals("1");
        string username = context.Request["username"];
        string firstletter = context.Request["letter"];
        string currency = context.Request["currency"];
        int roleid = (string.IsNullOrEmpty(context.Request["roleid"])) ? 3 : CastAide.AsInt32(context.Request["roleid"]);
        int custid = (string.IsNullOrEmpty(context.Request["custid"])) ? 0 : CastAide.AsInt32(context.Request["custid"]);   
        
        DateTime fdate = CastAide.AsDateTime(context.Request["fdate"]);
        DateTime tdate = CastAide.AsDateTime(context.Request["tdate"]);        
        
        switch (mode)
        {            
            case "3": // update remark of CustAccount bi custid
                Hashtable hash = new Hashtable();
                hash.Add("accountremark", HttpUtility.HtmlEncode(context.Request["remark"].ToString()));
                hash.Add("custid", custid);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("accountremarkdate", DateTime.Now);
                hash.Add("userid", CastAide.AsInt32(context.Session["AccId"],0));
                var rs = AccountReport.Acc_CreditTermUpdRemark(hash);
                if (rs.ErrorCode > 0)
                    context.Response.Write("no|" + CastAide.AsString(rs.ErrorMessage));
                else
                {
                    string accountremarkdate = CastAide.AsString(rs.ErrorHandler[0]);
                    accountremarkdate = Reports.datevalue(accountremarkdate);
                    context.Response.Write("ok|" + accountremarkdate);
                }
                break;
            case "4"://get credit term                
                context.Response.Write(Reports.GetCreditTerm_1());
                break;                
         
            case "9":// Credit Term
                string groupMaxLoss = "N/A";
                o.Add(new JProperty("CreditTerm", CreditTermHTML.GetContentCreditTerm(context, ref groupMaxLoss)));
                o.Add(new JProperty("CreditTermFilter", CreditTermHTML.GetContentCreditTermFilter(context, groupMaxLoss))); // fix bug 1791
                context.Response.Write(JsonConvert.SerializeObject(o)); 
                break; 

            case "23": //Get FromDate, ToDate
                AccCommon.GetDatetimeOption(CastAide.AsString(context.Request["date"]), ref fdate, ref tdate);
                context.Response.Write(fdate.ToString("MM/dd/yyyy") + "|" + tdate.ToString("MM/dd/yyyy"));
                break;  
           
            default:
                o.Add(new JProperty("Error", "Error"));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;                
        }
    }
  
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
