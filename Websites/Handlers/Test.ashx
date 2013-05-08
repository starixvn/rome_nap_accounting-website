<%@ WebHandler Language="C#" Class="Test" %>

using System;
using System.Collections.Generic;
using System.Web;
using Sunplus.Accounting.Business.PositionTaking.SportBook;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class Test : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.Redirect("http://google.com.vn?q=hello");
        JObject json = new JObject();

        json["info"] = new JValue(JsonConvert.SerializeObject(new MeetingDetail1("sa", "7")));
        //json["test2"] = new JValue(JsonConvert.SerializeObject(new MeetingDetail("ka", "9")));
        
        BettypeDetail bettypeDetail = new BettypeDetail(1, "Hdp", 0.1, 0.1);
        List<BettypeDetail> betTypes = new List<BettypeDetail>();
        betTypes.Add(new BettypeDetail(1, "Hdp1", 0.1, 0.1));
        betTypes.Add(new BettypeDetail(2, "Hdp2", 0.1, 0.1));
        
        JObject jsonBT = new JObject();
        jsonBT["hdp1"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(1, "Hdp1", 0.1, 0.1)));
        jsonBT["hdp2"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(2, "Hdp2", 0.1, 0.1)));
        jsonBT["hdp3"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(3, "Hdp3", 0.1, 0.1)));
        jsonBT["hdp4"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(4, "Hdp4", 0.1, 0.1)));
        jsonBT["hdp5"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(5, "Hdp5", 0.1, 0.1)));
        jsonBT["hdp6"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(6, "Hdp6", 0.1, 0.1)));
        jsonBT["hdp7"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(7, "Hdp7", 0.1, 0.1)));
        jsonBT["hdp8"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(8, "Hdp8", 0.1, 0.1)));
        jsonBT["hdp9"] = new JValue(JsonConvert.SerializeObject(new BettypeDetail(9, "Hdp9", 0.1, 0.1)));
        
        JObject jsonSport = new JObject();
        jsonSport["Soccer"] = new JValue(JsonConvert.SerializeObject(new SportDetail("Soccer", 1, JsonConvert.SerializeObject(jsonBT))));

        json["pt"] = new JValue(JsonConvert.SerializeObject(jsonSport));

        IPtService ptService = new PtManager();
        
        context.Response.Write(ptService.Select(PtManager.Level.Super, PtManager.PtType.AddNewSuper));
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

public class MeetingDetail1
{
    public MeetingDetail1()
    {
        _MeetingName = "hello";
        _Commission = "0.7";
    }
    private string _MeetingName;

    public string MeetingName
    {
        get { return _MeetingName; }
        set { _MeetingName = value; }
    }
    private string _Commission;

    public string Commission
    {
        get { return _Commission; }
        set { _Commission = value; }
    }
        
    
    public MeetingDetail1(string meetingName, string commission)
    {
        _MeetingName = meetingName;
        _Commission = commission;
    }
}

