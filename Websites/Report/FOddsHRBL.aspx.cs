using System;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.Report;

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
    private void GenReport()
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
                ds = MatchWinLostReport.WinlostFOddsHR_BetList_Details_14(leagueid, raceNo, currency, wldate);
            else
                ds = MatchWinLostReport.WinlostFOddsHR_BetList_Details(leagueid, raceNo, currency, wldate, db);

            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("BetList\\BetListFOddsHRDetails.html");
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
                        TemplateHandler.Assign(ref row, "TransID", dr["transid"]);
                        TemplateHandler.Assign(ref row, "TransTime", dr["transdate"]);
                        TemplateHandler.Assign(ref row, "Stake", NumberFormat.DecFormat(dr["stake"], 2));
                        TemplateHandler.Assign(ref row, "WinLoss", NumberFormat.DecFormat(dr["winlost"], 2));
                        TemplateHandler.Assign(ref row, "PlayerComm", NumberFormat.DecFormat(dr["PlayerComm"], 2));
                        TemplateHandler.Assign(ref row, "WLPTAgency", NumberFormat.DecFormat(dr["WLPTAgency"], 2));
                        TemplateHandler.Assign(ref row, "CommIncomeAgency", NumberFormat.DecFormat(dr["CommIncomeAgency"], 2));
                        TemplateHandler.Assign(ref row, "WLPTIBC", NumberFormat.DecFormat(dr["WLPTIBC"], 2));
                        TemplateHandler.Assign(ref row, "CommIncomeIBC", NumberFormat.DecFormat(dr["CommIncomeIBC"], 2));
                        TemplateHandler.Assign(ref row, "Dividend", NumberFormat.DecFormat(dr["dividend"], 2));

                        int bettype = CastAide.AsInt32(dr["bettype"]);

                        if (bettype == 41 || bettype == 42)
                        {
                            TemplateHandler.RemoveBlock(ref row, "winplace-status");
                            TemplateHandler.RemoveBlock(ref row, "winplace43");
                            TemplateHandler.Assign(ref row, "lblWinOdds", Resources.Language.betAt);
                            TemplateHandler.Assign(ref row, "lblWinOddsSet", Resources.Language.settle);
                            TemplateHandler.Assign(ref row, "Status", dr["status"]);
                            TemplateHandler.Assign(ref row, "WinOddsSet", NumberFormat.DecFormat(dr["Odds"], 2));
                            if (NumberFormat.DecFormat(dr["mOdds"], 2) != NumberFormat.DecFormat(dr["Odds"], 2) && CastAide.AsString(dr["status"]).ToUpper() == Status.WIN)
                            {
                                TemplateHandler.Assign(ref row, "{Odds}", "");
                                TemplateHandler.Assign(ref row, "WinOdds", NumberFormat.DecFormat(dr["mOdds"], 2));
                            }
                            else
                            {
                                TemplateHandler.Assign(ref row, "{Odds}", (CastAide.AsString(dr["status"]).ToUpper() != Status.WIN) ? "-" : NumberFormat.DecFormat(dr["Odds"], 2));
                                TemplateHandler.RemoveBlock(ref row, "WO43");
                            }
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{Odds}", "");
                            TemplateHandler.Assign(ref row, "lblWinOdds", Resources.Language.wBetAt);
                            TemplateHandler.Assign(ref row, "lblPlaceOdds", Resources.Language.pBetAt);
                            TemplateHandler.Assign(ref row, "WinStatus", dr["winStatus"]);
                            TemplateHandler.Assign(ref row, "PlaceStatus", dr["placeStatus"]);
                            TemplateHandler.Assign(ref row, "Status", "");
                            TemplateHandler.Assign(ref row, "WinOddsSet", (CastAide.AsString(dr["winStatus"]).ToUpper() != Status.WIN) ? "0.00" : NumberFormat.DecFormat(dr["WinOdds"], 2));
                            TemplateHandler.Assign(ref row, "PlaceOddsSet", (CastAide.AsString(dr["placeStatus"]).ToUpper() != Status.WIN) ? "0.00" : NumberFormat.DecFormat(dr["PlaceOdds"], 2));
                            if (NumberFormat.DecFormat(dr["finalodds"], 2) != NumberFormat.DecFormat(dr["WinOdds"], 2) && CastAide.AsString(dr["winStatus"]).ToUpper() == Status.WIN)
                            {
                                TemplateHandler.Assign(ref row, "lblWinOddsSet", Resources.Language.WSettle);
                                TemplateHandler.Assign(ref row, "WinOdds", NumberFormat.DecFormat(dr["finalodds"], 2));
                            }
                            else
                            {
                                TemplateHandler.Assign(ref row, "lblWinOddsSet", Resources.Language.lblWin);
                                TemplateHandler.RemoveBlock(ref row, "winOdds43");
                            }
                            if (NumberFormat.DecFormat(dr["finalodds2"], 2) != NumberFormat.DecFormat(dr["PlaceOdds"], 2) && CastAide.AsString(dr["placeStatus"]).ToUpper() == Status.WIN)
                            {
                                TemplateHandler.Assign(ref row, "lblPlaceOddsSet", Resources.Language.pSettle);
                                TemplateHandler.Assign(ref row, "PlaceOdds", NumberFormat.DecFormat(dr["finalodds2"], 2));
                            }
                            else
                            {
                                TemplateHandler.Assign(ref row, "lblPlaceOddsSet", Resources.Language.lblPlace);
                                TemplateHandler.RemoveBlock(ref row, "PlaceOdds43");
                            }
                        }

                        TemplateHandler.Assign(ref row, "choice", dr["matchcode"] + " - " + CacheRef.GetHorseTeamName(dr["teamid"]));
                        TemplateHandler.Assign(ref row, "bettype", CacheRef.GetBettype(dr["bettype"]));

                        rows.Append(row);
                    }
                    TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());

                    //total
                    TemplateHandler.Assign(ref tpl, "TotalStake", NumberFormat.DecFormat(dt.Compute("sum(stake)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalWL", NumberFormat.DecFormat(dt.Compute("sum(winlost)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalAgWL", NumberFormat.DecFormat(dt.Compute("sum(WLPTAgency)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalIBCWL", NumberFormat.DecFormat(dt.Compute("sum(WLPTIBC)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalCom", NumberFormat.DecFormat(dt.Compute("sum(PlayerComm)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalAgCom", NumberFormat.DecFormat(dt.Compute("sum(CommIncomeAgency)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalIBCCom", NumberFormat.DecFormat(dt.Compute("sum(CommIncomeIBC)", ""), 2));
                    TemplateHandler.Assign(ref tpl, "TotalDividend", NumberFormat.DecFormat(dt.Compute("sum(dividend)", ""), 2));
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref tpl, "content", AccCommon.GenNoInformation("10"));
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