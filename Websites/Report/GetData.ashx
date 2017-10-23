<%@ WebHandler Language="C#" Class="GetData" %>

using System;
using System.Web;
using DNAide.Utils;
using DNAide.Core; 
using DNAide.Web;
using DNAide.Data;
using System.Collections;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.Report;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Presentation;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.DAL ;
using System.Web.SessionState;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using IBCUtils;
using System.Linq;
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
        string strData = "", strErrMsg = "";
        bool result = false;
        
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
            case "7":// Tote Odds HR 12BET Report                
                o.Add(new JProperty("sData", ReportHTML.TOddsHR12BETWLReport(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "8":// Tote Odds HR WL Report
                o.Add(new JProperty("sData", ReportHTML.ToteHRWLReport(context)));
                context.Response.Write(JsonConvert.SerializeObject(o)); 
                break;
            case "9":// Credit Term
                string groupMaxLoss = "N/A";
                o.Add(new JProperty("CreditTerm", CreditTermHTML.GetContentCreditTerm(context, ref groupMaxLoss)));
                o.Add(new JProperty("CreditTermFilter", CreditTermHTML.GetContentCreditTermFilter(context, groupMaxLoss))); // fix bug 1791
                context.Response.Write(JsonConvert.SerializeObject(o)); 
                break;
            case "10":// GROUP HORSE RACING REPORT
                if(context.Request["type"] == "tote")
                    strData = ReportHTML.HRGroupDaily_ToteOnly(context);
                else
                    strData = ReportHTML.HRGroupDaily(context);
                
                if (context.Request["IsSendMail"] == "1")
                {
                    try { result = SendMailService.SendMail2(MailSection.MailServer, MailSection.ToMailHRGroup, MailSection.FromMail, MailSection.CCMailHRGroup, MailSection.BCCMailHRGroup, "Group Racing report from " + fdate.ToShortDateString() + " to " + tdate.ToShortDateString(), strData, MailSection.MailPwd); }
                    catch (Exception ex)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
                        strErrMsg = "<span style='font-weight:bold; color:red'>" + ex.Message + "</span>"; Logs.Write(ex.Message);
                    }
                }
                if (result){strErrMsg = "<span style='font-weight:bold; color:blue'>Sent email successfully. </span>";}
                TemplateHandler.RemoveBlock(ref strData, "footer-send-email");
                o.Add(new JProperty("HRReport", strData));
                o.Add(new JProperty("HRError", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));                
                break;
            case "11"://TOdds HR Mansion Report                
                o.Add(new JProperty("sData", ReportHTML.ToteHRMansionWLReport(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "12":// Fixed Odds HR WL Report
                o.Add(new JProperty("sData", ReportHTML.FOddsHRWLReport(context)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "13":// Fixed Odds HR 12BET Report                
                o.Add(new JProperty("sData", ReportHTML.FOddsHR12BETWLReport(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "14"://FOdds HR Mansion Report                
                o.Add(new JProperty("sData", ReportHTML.FOddsHRMansionWLReport(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "15"://TOdds HR AB88 Report                
                o.Add(new JProperty("sData", ReportHTML.ToteHRAB88WLReport(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;                       
            case "18": //Super Balance Movement
                int balanceType = CastAide.AsInt32(context.Request["balancetype"], 0);
                if (balanceType < 0 || balanceType > 1)
                    balanceType = 0;
                strData = ReportHTML.GetSuperMovement(balanceType);
                context.Response.Write(strData);
                break;
            case "19"://FOdds HR AB88 Report                
                o.Add(new JProperty("sData", ReportHTML.FOddsHRAB88WLReport(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;      
            case "20": //Trader Performance
                int isSoccer = ((context.Request["IsSoccer"] == null) ? 1 : CastAide.AsInt32(context.Request["IsSoccer"], 1));
                if (isSoccer > 1 || isSoccer < 0) isSoccer = 1;
                strData = ReportHTML.GetTraderPerformance(context.Request["fdate"], context.Request["tdate"], CastAide.AsString(context.Request["sortCol"]), CastAide.AsString(context.Request["order"]), isSoccer);    
                context.Response.Write(strData);
                break;
            case "21": //Trader Performance Detail
                strData = ReportHTML.GetTraderPerformanceDetail(CastAide.AsString( context.Request["fdate"]),CastAide.AsString(context.Request["tdate"]), CastAide.AsString(context.Request["sortCol"]), CastAide.AsString(context.Request["order"]), CastAide.AsInt32(context.Request["IsSoccer"], 1));
                context.Response.Write(strData);
                break;           
            case "22": //M88 Winloss
                BLL.Report.ReportBase ReportM88Winloss = new BLL.Report.ReportM88WinLoss();
                Hashtable M88WLInput = new Hashtable();
                M88WLInput.Add("date",CastAide.AsString(context.Request["date"]));
                M88WLInput.Add("sport",CastAide.AsString(context.Request["sport"]));
                M88WLInput.Add("fdate",CastAide.AsString(context.Request["fdate"]));
                M88WLInput.Add("tdate",CastAide.AsString(context.Request["tdate"]));
                M88WLInput.Add("currency",CastAide.AsString(context.Request["currency"]));
                M88WLInput.Add("reportby",CastAide.AsString(context.Request["reportby"]));
                M88WLInput.Add("reportdetail", CastAide.AsString(context.Request["reportdetail"]));
                M88WLInput.Add("currencyname", CastAide.AsString(context.Request["currencyname"]));
                M88WLInput.Add("isback", CastAide.AsString(context.Request["isback"]));
                
                strData = ReportM88Winloss.RenderHTML(M88WLInput);
                context.Response.Write(strData);
                break;
            case "23": //Get FromDate, ToDate
                AccCommon.GetDatetimeOption(CastAide.AsString(context.Request["date"]), ref fdate, ref tdate);
                context.Response.Write(fdate.ToString("MM/dd/yyyy") + "|" + tdate.ToString("MM/dd/yyyy"));
                break;
            case "24": //Fun88 Winloss
                var ReportFun88Winloss = new BLL.Report.ReportWinlossFun88();
                Hashtable Fun88WLInput = new Hashtable();
                Fun88WLInput.Add("date", CastAide.AsString(context.Request["date"]));
                Fun88WLInput.Add("sport", CastAide.AsString(context.Request["sport"]));
                Fun88WLInput.Add("fdate", CastAide.AsString(context.Request["fdate"]));
                Fun88WLInput.Add("tdate", CastAide.AsString(context.Request["tdate"]));
                Fun88WLInput.Add("currency", CastAide.AsString(context.Request["currency"]));
                Fun88WLInput.Add("reportby", CastAide.AsString(context.Request["reportby"]));
                Fun88WLInput.Add("reportdetail", CastAide.AsString(context.Request["reportdetail"]));
                Fun88WLInput.Add("currencyname", CastAide.AsString(context.Request["currencyname"]));
                Fun88WLInput.Add("isback", CastAide.AsString(context.Request["isback"]));

                strData = ReportFun88Winloss.RenderHTML(Fun88WLInput);
                context.Response.Write(strData);
                break;
            case "25": //12Bet Winloss
                BLL.Report.ReportBase Report12BETWinloss = new BLL.Report.ReportWinloss12Bet();
                Hashtable WLInput_12BET = new Hashtable();
                WLInput_12BET.Add("date", CastAide.AsString(context.Request["date"]));
                WLInput_12BET.Add("sport", CastAide.AsString(context.Request["sport"]));
                WLInput_12BET.Add("fdate", CastAide.AsString(context.Request["fdate"]));
                WLInput_12BET.Add("tdate", CastAide.AsString(context.Request["tdate"]));
                WLInput_12BET.Add("currency", CastAide.AsString(context.Request["currency"]));
                WLInput_12BET.Add("reportby", CastAide.AsString(context.Request["reportby"]));
                WLInput_12BET.Add("currencyname", CastAide.AsString(context.Request["currencyname"]));
                WLInput_12BET.Add("reporttype", CastAide.AsString(context.Request["reporttype"]));
                WLInput_12BET.Add("isback", CastAide.AsString(context.Request["isback"]));
                WLInput_12BET.Add("recommend", CastAide.AsString(context.Request["recommend"]));



                strData = Report12BETWinloss.RenderHTML(WLInput_12BET);
                context.Response.Write(strData);
                break;

            case "26": //ALOG Winloss
                BLL.Report.ReportBase ReportALogWinloss = new BLL.Report.ReportALogWinLoss();
                Hashtable ALogWLInput = new Hashtable();
                ALogWLInput.Add("date", CastAide.AsString(context.Request["date"]));
                ALogWLInput.Add("sport", CastAide.AsString(context.Request["sport"]));
                ALogWLInput.Add("fdate", CastAide.AsString(context.Request["fdate"]));
                ALogWLInput.Add("tdate", CastAide.AsString(context.Request["tdate"]));
                ALogWLInput.Add("currency", CastAide.AsString(context.Request["currency"]));
                ALogWLInput.Add("reportby", CastAide.AsString(context.Request["reportby"]));
                ALogWLInput.Add("reportdetail", CastAide.AsString(context.Request["reportdetail"]));
                ALogWLInput.Add("currencyname", CastAide.AsString(context.Request["currencyname"]));
                ALogWLInput.Add("isback", CastAide.AsString(context.Request["isback"]));
                strData = ReportALogWinloss.RenderHTML(ALogWLInput);
                context.Response.Write(strData);
                break;
            case "27": //TLC Winloss
                BLL.Report.ReportBase ReportWinLossTLC = new BLL.Report.WinLossTLC();
                Hashtable WLTLCInput = new Hashtable();
                WLTLCInput.Add("date", CastAide.AsString(context.Request["date"]));
                WLTLCInput.Add("sport", CastAide.AsString(context.Request["sport"]));
                WLTLCInput.Add("fdate", CastAide.AsString(context.Request["fdate"]));
                WLTLCInput.Add("tdate", CastAide.AsString(context.Request["tdate"]));
                WLTLCInput.Add("currency", CastAide.AsString(context.Request["currency"]));
                WLTLCInput.Add("reportby", CastAide.AsString(context.Request["reportby"]));
                WLTLCInput.Add("reportdetail", CastAide.AsString(context.Request["reportdetail"]));
                WLTLCInput.Add("currencyname", CastAide.AsString(context.Request["currencyname"]));
                WLTLCInput.Add("isback", CastAide.AsString(context.Request["isback"]));

                strData = ReportWinLossTLC.RenderHTML(WLTLCInput);
                context.Response.Write(strData);
                break;
            case "28":
                BLL.Report.ReportBase ReportWinloss = new BLL.Report.OtherWinLossReport();
                Hashtable inputValues = new Hashtable();
                inputValues.Add("date", CastAide.AsString(context.Request["date"]));
                inputValues.Add("sport", CastAide.AsString(context.Request["sport"]));
                inputValues.Add("fdate", CastAide.AsString(context.Request["fdate"]));
                inputValues.Add("tdate", CastAide.AsString(context.Request["tdate"]));
                inputValues.Add("currency", CastAide.AsString(context.Request["currency"]));
                inputValues.Add("reportby", CastAide.AsString(context.Request["reportby"]));
                inputValues.Add("reportdetail", CastAide.AsString(context.Request["reportdetail"]));
                inputValues.Add("currencyname", CastAide.AsString(context.Request["currencyname"]));
                inputValues.Add("isback", CastAide.AsString(context.Request["isback"]));
                inputValues.Add("site", CastAide.AsString(context.Request["site"]));

                strData = ReportWinloss.RenderHTML(inputValues);
                context.Response.Write(strData);
                break;
            case "29":// Tote Odds HR 12BET Report - Old
                o.Add(new JProperty("sData", ReportHTML.TOddsHR12BETWLReport_Old(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "30"://TOdds HR ALog Report                
                o.Add(new JProperty("sData", ReportHTML.ToteHRALogWLReport(context, ref strErrMsg)));
                o.Add(new JProperty("sErrMsg", strErrMsg));
                context.Response.Write(JsonConvert.SerializeObject(o));
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
