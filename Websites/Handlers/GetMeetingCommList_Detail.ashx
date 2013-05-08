<%@ WebHandler Language="C#" Class="GetMeetingCommList_Detail" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Business.Others;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Core;
using Resources;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;



public class GetMeetingCommList_Detail : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context)
    {
        try
        {
            context.Response.ContentType = "text/html";
            context.Response.ContentEncoding = Encoding.UTF8;
            string partnerType = CastAide.AsString(context.Request["partnerType"], "");

            JObject json = new JObject();
            List<MeetingDetail> meetingDetails = new List<MeetingDetail>();
            DataTable dtMeetingList = CompanyReport.GetHRMeetingPartnerList_Details(partnerType);
            if (dtMeetingList != null)
            {

                int totalrows = dtMeetingList.Rows.Count;
                if (totalrows > 0)
                {
                    var lsExchange = CacheUtils.Exchange;
                    for (int i = 0; i < totalrows; i++)
                    {
                        DataRow row = dtMeetingList.Rows[i];
                        string Meeting = row["MeetingID"].ToString();
                        string ProgramID = row["ProgramID"].ToString();
                        string PoolCurrency = CacheUtils.GetExchangeName(lsExchange, CastAide.AsInt32(row["PoolCurrency"]));
                        meetingDetails.Add(new MeetingDetail(i + 1,
                                                             CastAide.AsString(RemotingRef.RefData.GetLeagueName(
                                                                 Meeting,
                                                                 "EN")),
                                                             ProgramID, PoolCurrency));
                    }
                    json["MeetingData"] = new JValue(JsonConvert.SerializeObject(meetingDetails));
                }
                else
                {
                    meetingDetails.Add(new MeetingDetail(0, Resources.Language.thereisnodata, "", ""));
                    json["MeetingData"] = new JValue(JsonConvert.SerializeObject(meetingDetails));

                }
                context.Response.Write(JsonConvert.SerializeObject(json));

            }
            else
            {
                context.Response.Write(Resources.Language.thereisnodata);
            }
            
        }
        catch(Exception ex)
        {
            GotoErroPage(context);
            context.Response.End();
        }
        

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}

