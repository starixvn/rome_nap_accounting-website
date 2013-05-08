<%@ WebHandler Language="C#" Class="DeleteTrans" %>

using System;
using System.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using System.Web.SessionState;
using DNAide.Data;
using DNAide.Web;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Collections;
using System.Text;
using System.Data;
using IBCUtils.DAL;


public class DeleteTrans : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            context.Response.ContentType = "text/xml";
            string matchId = context.Request["matchid"].Trim();
            string bettype = context.Request["bettype"];
            
            Hashtable hash = new Hashtable();
            hash.Add("matchid", matchId);
            hash.Add("bettype", bettype);
            hash.Add("fromrefno", context.Request["frefno"].Trim());
            hash.Add("torefno", context.Request["trefno"].Trim());
            hash.Add("betteam", context.Request["betteam"]);
            string hdp = context.Request["hdp"];
            hdp = (string.IsNullOrEmpty(hdp)) ? "9999" : hdp;
            hash.Add("hdp", hdp);

          

            #region DNAide
            DbResult rs = AccountReport.Acc_BettransSelDelSpecial_MixParlay(hash);
            DataTable dt = rs.ResultSet.Tables[0];
            #endregion

            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportMP.htm");
            string tplContent = TemplateHandler.CopyBlock(tpl, "content");
            StringBuilder rows = new StringBuilder();
            int index =  0;
            string odd_even = "";
            foreach (DataRow dr in dt.Rows)
            {
                
                index ++;
                odd_even = (odd_even == "") ? "evenrow" : "";
                string row = tplContent.Clone().ToString();

                TemplateHandler.Assign(ref row, "odd_even", odd_even);
                TemplateHandler.Assign(ref row, "No", index);
                TemplateHandler.Assign(ref row, "CustId", dr["username"]);
                TemplateHandler.Assign(ref row, "UserName", dr["username"]);
                TemplateHandler.Assign(ref row, "Transdate", dr["transdate"]);
                TemplateHandler.Assign(ref row, "RefNo", dr["transid"]);
                TemplateHandler.Assign(ref row, "Stake", NumberFormat.DecFormat(dr["StakePositiontaking"],2));
                TemplateHandler.Assign(ref row, "Currency", dr["currency"]);
                TemplateHandler.Assign(ref row, "Choice", GenMPRow(dr["refno"].ToString(), matchId, bettype));
                rows.Append(row);
            }

            if (dt.Rows.Count > 0)
            {
                TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());
                TemplateHandler.RemoveBlock(ref tpl, "nodata");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref tpl, "content");
            }
                

            context.Response.Write(tpl);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    decimal odds = 0, hdp1 = 0, hdp2 = 0;
    string bettypeName = "", sportypeName = "", leagueName = "", betteam = "";
    int bettype = 0;
    bool IsSearchResult = false;
    string GenMPRow(string refno, string MatchID, string Bettype)
    {
        string tplEvent = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("BetList/DeleteBetMPEvent.htm");
        TemplateHandler.Assign(ref tplEvent, "MoreBetInfo", "");
        string strData = "";
        try
        {
            DbResult ds = AccountReport.AccBetsSelBetCheckM(refno);
            string eventItems = string.Empty;
            if (ds != null)
            {
                for (int i = 0; i < ds.ResultSet.Tables[0].Rows.Count; ++i)
                {
                    string mpItem = (string)tplEvent.Clone();
                    DataRow row2 = ds.ResultSet.Tables[0].Rows[i];
                    string mpEvent = (string)mpItem.Clone();
                    string mpHome = CacheRef.GetTeamName(row2["homeid"]) + (CastAide.AsBoolean(row2["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty);
                    string mpAway = CacheRef.GetTeamName(row2["awayid"]);
                    betteam = row2["betteam"] + "";
                    sportypeName = CacheRef.GetSportName(row2["Sporttype"]);
                    leagueName = CacheRef.GetLeagueName(row2["leagueid"]);
                    bettype = Convert.IsDBNull(row2["bettype"]) ? 0 : Convert.ToInt32(row2["bettype"]);
                    decimal mpHdp1 = Convert.IsDBNull(row2["hdp1"]) ? 0 : Convert.ToDecimal(row2["hdp1"]);
                    decimal mpHdp2 = Convert.IsDBNull(row2["hdp1"]) ? 0 : Convert.ToDecimal(row2["hdp2"]);
                    decimal mpHdp = (betteam == "h") ? mpHdp2 - mpHdp1 : mpHdp1 - mpHdp2;
                    decimal mpOdds = Convert.IsDBNull(row2["odds"]) ? 0 : Convert.ToDecimal(row2["odds"]);
                    odds = mpOdds;
                    hdp1 = mpHdp1;
                    hdp2 = mpHdp2;
                    
                    //bettypeName = row2["typename"] + "";
                    bettypeName = CacheRef.GetBettype(row2["bettype"]);
                    
                    string moreBetInfo = string.Empty;

                    string mid = row2["matchid"].ToString();
                    string bid = row2["bettype"].ToString();

                    if (mid == MatchID && bid == Bettype)
                    {
                        TemplateHandler.Assign(ref mpItem, "MP", "");
                        IsSearchResult = true;
                    }
                    else
                    {
                        TemplateHandler.Assign(ref mpItem, "MP", "MP");
                        IsSearchResult = false;
                    }
                    switch (bettype)
                    {
                        case 1:
                        case 7:
                        case 20:
                        case 21:
                            GenHdpEvent(ref mpItem, mpHome, mpAway, betteam, true);
                            break;
                        case 2:
                        case 12:
                            GenOEEvent(ref mpItem, mpHome, mpAway, true);
                            break;
                        case 3:
                        case 8:
                            GenOUEvent(ref mpItem, mpHome, mpAway, true);
                            break;
                        case 4:
                        case 14:
                            GenCSEvent(ref mpItem, mpHome, mpAway, true);
                            break;
                        case 5:
                        case 15:

                            Gen1x2Event(ref mpItem, mpHome, mpAway, true);
                            break;
                        case 6:
                        case 16:
                            GenTGEvent(ref mpItem, mpHome, mpAway, true);
                            break;
                        default:
                            mpItem = string.Empty;
                            break;
                    }                    
                    strData += mpItem + ((i == ds.ResultSet.Tables[0].Rows.Count - 1) ? "" :  "<br>");
                }
            }
       
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return strData;
    }

    string oddMP()
    {
        string cls = (IsSearchResult) ? "" : "MP";
        return "<span class='style_12BB"+ cls +"'><b> @ " + NumberFormat.DecFormat(odds,3) + "</b></span>";
    }
    void GenHdpEvent(ref string eventItem, string home, string away, string betteam, bool isMP)
    {
        string liveScore = "";
        string choice = "";
        if (bettype == 20) //Money line
        {
            if (betteam == "h") choice = home;
            else choice = away;
        }
        else
        {
            if (betteam == "h") choice = home + " " + NumberFormat.DecFormat((hdp2 - hdp1),2) + liveScore;
            else choice = away + " " + NumberFormat.DecFormat((hdp1 - hdp2),2) + liveScore;
        }
        
        if (isMP) choice += oddMP();
        TemplateHandler.Assign(ref eventItem, "{choice}", choice);
        TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
        TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
        TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
        TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
    }
    void GenOEEvent(ref string eventItem, string home, string away, bool isMP)
    {
        string choice = "";
        if (betteam == "h") choice = "ODD";
        else choice = "EVEN";
        
        if (isMP) choice += oddMP();
        TemplateHandler.Assign(ref eventItem, "{choice}", choice);
        TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
        TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
        TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
        TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
    }
    void GenOUEvent(ref string eventItem, string home, string away, bool isMP)
    {
        string liveScore = "";
        string choice = "";
        if (betteam == "h") choice = "OVER " + NumberFormat.DecFormat(Math.Abs(hdp1),2) + liveScore;
        else choice = "UNDER " + NumberFormat.DecFormat(Math.Abs(hdp1),2) + liveScore;
        
        if (isMP) choice += oddMP();
        TemplateHandler.Assign(ref eventItem, "{choice}", choice);
        TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
        TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
        TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
        TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
    }
    void GenCSEvent(ref string eventItem, string home, string away, bool isMP)
    {
        string choice = betteam;
        if (isMP) choice += oddMP();
        TemplateHandler.Assign(ref eventItem, "{choice}", choice);
        TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
        TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
        TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
        TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
    }
    void Gen1x2Event(ref string eventItem, string home, string away, bool isMP)
    {
        string choice = "";
        if (bettype == 5) choice = "FT. " + betteam;
        if (bettype == 15) choice = "HT. " + betteam;
        if (isMP) choice += oddMP();
        
        TemplateHandler.Assign(ref eventItem, "{choice}", choice);
        TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
        TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
        TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
        TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
    }
    void GenTGEvent(ref string eventItem, string home, string away, bool isMP)
    {
        string choice = betteam;
        if (isMP) choice += oddMP(); 
        TemplateHandler.Assign(ref eventItem, "{choice}", choice);
        TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
        TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
        TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
        TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
    }

}
