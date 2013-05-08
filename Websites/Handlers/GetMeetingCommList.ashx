<%@ WebHandler Language="C#" Class="GetMeetingCommList" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;
using Sunplus.Accounting.Business.Others;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Core;
using Resources;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;



public class GetMeetingCommList : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.ContentEncoding = Encoding.UTF8;
        try
        {
            JObject json = new JObject();
            List<MeetingPartner> meetingPartner = new List<MeetingPartner>();
            DataTable dtMeetingList = CompanyReport.GetHRMeetingPartnerList();
            if (dtMeetingList != null)
            {
                int totalrows = dtMeetingList.Rows.Count;
                if (totalrows > 0)
                {
                    for (int i = 0; i < totalrows; i++)
                    {
                        DataRow row = dtMeetingList.Rows[i];
                        string Partner = row["PartnerType"].ToString();
                        double commWin = CastAide.AsDouble(row["ToteCommWin"], 0)*100;
                        double commPlace = CastAide.AsDouble(row["ToteCommPlace"], 0)*100;
                        double commWinPlace = (commWin + commPlace)/2;
                        double Fees = CastAide.AsDouble(row["ToteFee"], 0)*100;
                        meetingPartner.Add(new MeetingPartner(Partner, commWin, commPlace, commWinPlace, Fees));
                    }
                    json["MeetingData"] = new JValue(JsonConvert.SerializeObject(meetingPartner));
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
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    

    private DataTable GenSampleData()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add("meetingid", typeof(int));
        tbl.Columns.Add("ProgramID", typeof(string));
        tbl.Columns.Add("commwin", typeof(int));
        tbl.Columns.Add("commplace", typeof(int));
        tbl.Columns.Add("commtype", typeof(string));
        
        for (int i=1;i<20;i++)
        {
            tbl.Rows.Add(i, "ID_" + i, i, i+1, "Type_" + i);
        }
        return tbl;
    }

}

