using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;

using RemotingRef;



public partial class Report_HRBL : PageBase
{

    public StringBuilder html = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        GenReport();
    }

    /// <summary>
    /// Generate report body
    /// </summary>    
    void GenReport()
    {
        DbResult ds = null;

        try
        {
            int leagueid = CastAide.AsInt32(Request["leagueid"]);
            string raceNo = Request["raceNo"];
            int currency = CastAide.AsInt32(Request["currencyID"]);
            DateTime wldate = CastAide.AsDateTime(Request["wldate"], DateTime.Today);
            string db = ((string.IsNullOrEmpty(Request["db"])) ? "rep" : Request["db"]);
            string WL = ((string.IsNullOrEmpty(Request["WL"])) ? "WL" : Request["WL"]);

            DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
            if (wldate <= movebetsday)
                ds = MatchWinLostReport.WinlostHRMansion_BetList_Details_14(leagueid, raceNo, currency, wldate);
            else
                ds = MatchWinLostReport.WinlostHRMansion_BetList_Details(leagueid, raceNo, currency, wldate, db);

            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("BetList\\BetListToteHRMansionDetails.htm");
            string tplContent = TemplateHandler.CopyBlock(tpl, "content");

            TemplateHandler.Assign(ref tpl, "RaceNo", raceNo);
            TemplateHandler.Assign(ref tpl, "LeagueName", CacheRef.GetLeagueName(leagueid));
            TemplateHandler.Assign(ref tpl, "Currency", Request["currencyName"]);
            TemplateHandler.Assign(ref tpl, "WLDate", Request["wldate"]);

            StringBuilder rows = new StringBuilder();
            if (ds != null)
            {
                DataTable dt = ds.ResultSet.Tables[0];
                int index = 0;
                if (dt.Rows.Count > 0)
                {
                    string odd_even = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        index++;
                        string row = tplContent;
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        TemplateHandler.Assign(ref row, "odd_even", odd_even);

                        TemplateHandler.Assign(ref row, "No", index);
                        TemplateHandler.Assign(ref row, "Member", dr["username"]);
                        TemplateHandler.Assign(ref row, "UserName2", dr["username2"]);
                        TemplateHandler.Assign(ref row, "TransID", dr["transid"]);
                        TemplateHandler.Assign(ref row, "TransTime", dr["transdate"]);
                        TemplateHandler.Assign(ref row, "Stake", NumberFormat.DecFormat(dr["stake"], 2));
                        TemplateHandler.Assign(ref row, "WinLoss", NumberFormat.DecFormat(dr["winlost"], 2));
                        TemplateHandler.Assign(ref row, "PlayerComm", NumberFormat.DecFormat(dr["PlayerComm"], 2));
                        TemplateHandler.Assign(ref row, "WLPTAgency", "");
                        TemplateHandler.Assign(ref row, "CommIncomeAgency", NumberFormat.DecFormat(dr["CommIncomeAgency"], 2));
                        TemplateHandler.Assign(ref row, "Dividend", NumberFormat.DecFormat(dr["dividend"], 2));

                        //OpenTote Exchange Rate
                        var isShowOtExRate = CastAide.AsBoolean(dr["isShowOTExRate"], false);
                        var virtualRate = CastAide.AsDouble(dr["virtualrate"], 0.0);

                        if (!isShowOtExRate)
                        {
                            TemplateHandler.RemoveBlock(ref row, "Exchangerate");
                        }
                        else
                            TemplateHandler.Assign(ref row, "{ExchangeRate}", isShowOtExRate ? virtualRate.ToString() : string.Empty);

                        int bettype = CastAide.AsInt32(dr["bettype"]);

                        if (bettype == 31 || bettype == 32)
                        {
                            TemplateHandler.RemoveBlock(ref row, "winplace-status");
                            TemplateHandler.RemoveBlock(ref row, "winplace-odds");
                            TemplateHandler.Assign(ref row, "Status", dr["status"]);

                            if (dr["status"].Equals(Status.WIN) || dr["status"].Equals(Status.Refund))
                                TemplateHandler.Assign(ref row, "{Odds}", NumberFormat.DecFormat(dr["Odds"], 2));
                            else
                                TemplateHandler.Assign(ref row, "{Odds}", "-");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "WinStatus", dr["winStatus"]);
                            TemplateHandler.Assign(ref row, "PlaceStatus", dr["placeStatus"]);
                            TemplateHandler.Assign(ref row, "Status", "");

                            TemplateHandler.Assign(ref row, "{WinOdds}", (dr["winStatus"].Equals(Status.WIN)) ? NumberFormat.DecFormat(dr["WinOdds"], 2) : "0.00");
                            TemplateHandler.Assign(ref row, "{PlaceOdds}", (dr["placeStatus"].Equals(Status.LOSE) || dr["placeStatus"].Equals(Status.Refund)) ? "0.00" : NumberFormat.DecFormat(dr["PlaceOdds"], 2));
                            TemplateHandler.Assign(ref row, "{Odds}", string.Empty);
                        }

                        TemplateHandler.Assign(ref row, "choice", dr["matchcode"] + " - " + CacheRef.GetHorseTeamName(dr["teamid"]));
                        TemplateHandler.Assign(ref row, "bettype", CacheRef.GetBettype(dr["bettype"]));

                        rows.Append(row);
                    }
                    TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());

                    //total
                    TemplateHandler.Assign(ref tpl, "TotalStake", NumberFormat.DecFormat(dt.Compute("sum(stake)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalWL", NumberFormat.DecFormat(dt.Compute("sum(winlost)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalAgWL", "");
                    TemplateHandler.Assign(ref tpl, "TotalCom", NumberFormat.DecFormat(dt.Compute("sum(PlayerComm)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalAgCom", NumberFormat.DecFormat(dt.Compute("sum(CommIncomeAgency)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalDividend", NumberFormat.DecFormat(dt.Compute("sum(dividend)", ""), 2));
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref tpl, "content", AccCommon.GenNoInformation("11"));
                    TemplateHandler.RemoveBlock(ref tpl, "total");
                }
            }
            html.Append(tpl);

        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }


    }
}
