using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI; 
using System.Text;
using DNAide.Utils;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Business.Report.Common;
using Sunplus.Accounting.Business.Report.DailyWL;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using FormatHandler = IBCUtils.FormatHandler;


/// <summary>
/// CITS
/// </summary>
public partial class DailyWL_Sporttype : PageBase
{
    public string today = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    public string yesterday = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-1));
    private static IReportMainDAO _reportMainDao ;

    RMReportUtils utilsBARM = new RMReportUtils(RMReportUtils.ReportName.Report2h, RMReportUtils.RMType.BARM);

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        string fdate = Request["reportDate"];
        if (string.IsNullOrEmpty(fdate))
            fdate = Request["fdate"];
        
        fdate = (fdate);

        string path = "<b>Main</b>";        
        bool IsWL = CastAide.AsDateTime(fdate) > ReportUtils.BalanceUp.MoveBetsDay;

        #region Header
        int displaydays = 0;
        TimeSpan sp = DateTime.Today - new DateTime(2011, 01, 01);
        displaydays = sp.Days;
        string evt = "SetEvtGetdate()";
        string strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingleMaxDays_2h(fdate, "Date", string.Empty, displaydays, 7, "../", path, evt);

        if (IsWL)
            TemplateHandler.Assign(ref strDateTimeSingle, "strDispday", "");            
        else
            TemplateHandler.Assign(ref strDateTimeSingle, "strDispday", "none");

        string db = Request["db"];
        if (db == null) db = "Rep";
        if (db == "Rep")
        {
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "checked=checked");
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "");
        }
        else
        {
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "");
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "checked=checked");
        }
        TemplateHandler.Assign(ref strDateTimeSingle, "db", db);

        headerDatetime.Text = strDateTimeSingle;       
        

        #endregion
        
        content.Text = getcontent(CastAide.AsDateTime(fdate));

        string btnExport = Request["IsExport"];
        if (btnExport == "1")
        {
            ExportToExcel("DailyWL_Sporttype");
        }
    }

    #region getcontent 
    string getcontent(DateTime wldate)
    {
        StringBuilder html = new StringBuilder();
        
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");       

        try
        {
            TemplateHandler.Assign(ref norow, "{tdno}", "6");
            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
            string strfdate;
            strfdate = (Request["reportDate"]);
            if (string.IsNullOrEmpty(strfdate))
                strfdate = Request["fdate"];

            strfdate = (string.IsNullOrEmpty(strfdate)) ? DateTime.Today.ToShortDateString() : strfdate;
            if (!Validator.ValidDateMMDDYYYY(strfdate, out wldate).IsSuccess())
            {
                return html.ToString();  
            }
            
            //header
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostSummaryAllHeader.htm", new string[] { "{msg}", "{col}" }, new string[] { "DAILY WIN/LOSS REPORT", "7" }));
            string WLheader = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryHeader.htm");
            TemplateHandler.RemoveBlock(ref WLheader, "no-link");            
            DbResult rs  = null;

            var dailyWL = CommonBootStrapper.Locator.GetInstance<IReportRacingDAO>();

            rs = dailyWL.DailyWLDirect_Sporttype(wldate.ToShortDateString());

            TemplateHandler.Assign(ref WLheader, "{TypeName}", "SportType"); 
            if (rs == null)
            {
                #region no data
                TemplateHandler.Assign(ref WLheader, "{TypeName}", "SportType");
                html.Append(WLheader);
                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                html.Append(norow);
                return html.ToString();
                #endregion                
            }
            DataTable dt = rs.ResultSet.Tables[0];

            #region declare variable
            string sp = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummarySportType.htm");
            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryAllRow.htm");
            TemplateHandler.RemoveBlock(ref rows, "no-link");

            string colspans = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryColspan.htm"), colspan = "";
            TemplateHandler.Assign(ref colspans, "{col}", "7");            

            string totalrows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryTotalRow.htm");
            totalrows = totalrows.Replace("tdtitle", "");
            TemplateHandler.Assign(ref totalrows, "{col}", "1");
            TemplateHandler.Assign(ref totalrows, "{colorwin}", "");
            TemplateHandler.Assign(ref totalrows, "{colorpos}", "");
            TemplateHandler.Assign(ref totalrows, "{colorcom}", "");
            TemplateHandler.Assign(ref totalrows, "{colorline}", "");
            TemplateHandler.Assign(ref totalrows, "{colorturnover}", "");
            TemplateHandler.Assign(ref totalrows, "{colormargin}", "");

            double winlost_barm = 0;
            double winlost_8rm = 0;
            

            string odd_even = "";
            int index = 0, indexSport = 0;
            string pBetType = "", BetType = "", color = ""; 

            int SportType = 0;
            string sporttype = "";

            double BTypeWin = 0, BTypePos = 0, BTypeCom = 0, BTypeTurnover = 0, BTypeLineTotal = 0;
            double SportWin = 0, SportPos = 0, SportCom = 0, SportTurnover = 0, SportLineTotal = 0;
            double AllWin = 0, AllPos = 0, AllCom = 0, AllTurnover = 0, AllLineTotal = 0;
            double IBC8RMWin = 0, IBC8RMPos = 0, IBC8RMCom = 0, IBC8RMTurnover = 0, IBC8RMLineTotal = 0;
            double IBC8RMSportWin = 0, IBC8RMSportPos = 0, IBC8RMSportCom = 0, IBC8RMSportTurnover = 0, IBC8RMSportTurnOver = 0;
            
            double HRLineTotal = 0, FILineTotal = 0, CasinoLineTotal = 0, BingoLineTotal = 0, LiveCasinoLineTotal = 0, P2PLineTotal = 0;

            double Win = 0, Pos = 0, Com = 0, Turnover = 0, LineTotal;
            bool pLive = false, Live = false, Is8RM = false, IsBARM = false, Is3rdPartyShare = false; ;
            int ibcIndex = 0;
            string totalRowSportType = "";
            string db = ReportUtils.DBType;
            #endregion declare

            if (dt.Rows.Count > 0)
            {
                string sportname = "";
                int indexIBC = 0;
                int indexBARM = 0;
                #region foreach
                foreach (DataRow dr in dt.Rows)
                {
                    SportType = FormatHandler.GetInt(dr["SportType"]);
                    BetType = dr["SportType"].ToString();
                    sporttype = SportType.ToString();
                    // IBC
                    if (BetType != pBetType)
                    {
                        if (index > 1)
                        {
                            color = "#dddddd";
                            string totalrow = totalrows.Clone().ToString();                            
                            totalrow = "<tr style='height:18px;' align='right'><td>Total:</td><td colspan='3'></td><td nowrap>"+ Sunplus.Accounting.Business.Report.Reports.colorvalue(BTypeLineTotal) +"</td><td colspan='2'></td></tr>";
                            html.Append(totalrow);
                            indexIBC++;
                        }
                        BTypeWin = 0; BTypePos = 0; BTypeCom = 0; BTypeTurnover = 0; BTypeLineTotal = 0;
                        pBetType = BetType;
                        pLive = Live;
                        index = 0;
                    }
                    index++;

                    #region sport type 
                    string tname = "";
                    
                    tname = dr["sportname"].ToString();
                    
                    if (!sportname.Equals(tname))
                    {
                        if (indexSport > 1)
                        {
                            totalRowSportType = totalrows.Clone().ToString();
                            color = "#99cccc";
                            Reports.genrow2h(ref totalRowSportType, color, "<b>" + sportname + " Total:</b>", SportWin, SportPos, SportCom, SportLineTotal, SportTurnover, false);
                            SportWin = SportPos = SportCom = SportTurnover = 0;

                            if (indexSport > 0 && SportType == 1) html.Append(totalRowSportType);

                            colspan = colspans.Clone().ToString();
                            TemplateHandler.Assign(ref colspan, "{msg}", "");
                            TemplateHandler.Assign(ref colspan, "{height}", "rpt-height-tr");
                            html.Append(colspan);
                            indexSport = 1;
                        }

                        sportname = Convert.ToString(dr["sportname"]);
                        string h = WLheader.Clone().ToString();
                        if(indexSport == 0) 
                        {
                            html.Append(h);
                            html.Append(colspan);
                        }
                        indexSport++;
                    }
                    #endregion

                    odd_even = (odd_even == "") ? "evenrow" : "";                    
                    string row = rows.Clone().ToString();
                    string btype = "", btypeBARM="";

                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    Turnover = FormatHandler.GetDouble(dr["Turnover"]);
                    LineTotal = FormatHandler.GetDouble(dr["LineTotal"]);

                    int ibc = CastAide.AsInt32(dr["IBC"]);

                    if (ibc == 1) // 8RM
                    {
                        btype = sportname + " 8RM "  ;
                        IBC8RMWin += Win; IBC8RMSportWin += Win;
                        IBC8RMPos += Pos; IBC8RMSportPos += Pos;
                        IBC8RMCom += Com; IBC8RMSportCom += Com;
                        IBC8RMTurnover += Turnover; IBC8RMSportTurnover += Turnover;
                        IBC8RMLineTotal += LineTotal;
                        ibcIndex++;
                        Is8RM = true;

                        winlost_8rm += LineTotal;
                    }
                    else if (ibc == 2 ) // BARM
                    {
                        if (utilsBARM.IsActive)
                        {
                            btype = sportname + " " + utilsBARM.DisplayName;
                            IBC8RMWin += Win; IBC8RMSportWin += Win;
                            IBC8RMPos += Pos; IBC8RMSportPos += Pos;
                            IBC8RMCom += Com; IBC8RMSportCom += Com;
                            IBC8RMTurnover += Turnover; IBC8RMSportTurnover += Turnover;
                            IBC8RMLineTotal += LineTotal;
                            ibcIndex++;

                            IsBARM = true;

                            winlost_barm += LineTotal;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        ibcIndex = 0;
                        Is8RM = false;
                        IsBARM = false;
                        btype = "<b>" + dr["SportName"].ToString() + "</b>";

                        if (SportType == 1 )//Soccer
                        {
                            btype = "<a href='DailyWL_Frames.aspx?sporttype=" + sporttype + "&fdate=" + wldate.ToShortDateString() +  "&path=" + sportname + "&type=soccer&db=" + db + "'><b>" + btype + "</b></a>";
                            Hashtable hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_Soccer.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_soccer"] = hash;
                        }
                        else if (SportType == 151)//HR
                        {
                            btype = "<a href='DailyWL/DailyWL.aspx" + "?fdate=" + wldate.ToShortDateString() + "&isExpExcel=0'><b>" + btype + "</b></a>";
                            
                            Hashtable hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_Soccer.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_hr"] = hash;

                        }
                        else if (SportType == 201)//FI
                        {
                            btype = "<a href='DailyWL_Frames.aspx?sporttype=" + sporttype + "&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&type=fi&db=" + db +  "'><b>" + btype + "</b></a>";
                            Hashtable hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_Soccer.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_fi"] = hash;
                        }
                        else if (SportType == 251)//Casino
                        {
                            btype = "<a href='DailyWL_Frames.aspx?sporttype=" + sporttype + "&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&type=casino&db=" + db + "'><b>" + btype + "</b></a>";
                            Hashtable hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_Soccer.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_casino"] = hash;
                        }
                        else if (SportType == 252)//bingo
                        {
                            btype = "<a href='DailyWL_Frames.aspx?sporttype=" + sporttype + "&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&type=bingo&db=" + db + "'><b>" + btype + "</b></a>";
                            var hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_Soccer.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_bingo"] = hash;
                        }
                        else if (SportType == 253)//p2p
                        {
                            //btype = "<a href='DailyWL_Frames.aspx?sporttype=" + sporttype + "&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&type=p2p&db=" + db + "'><b>" + btype + "</b></a>";
                            var hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_Soccer.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_p2p"] = hash;
                        }
                        else if (SportType == 99 )//MP 
                        {
                            string date = WebSettings.getDateDailyWLMP;
                            if(DateTime.Compare(CastAide.AsDateTime(wldate), CastAide.AsDateTime(date)) < 0)
                                btype = "<a href='DailyWL_MixParlay.aspx?&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&db=" + db + "&bettype=9&ispopup=false'><b>" + btype + "</b></a>";
                            else
                            {
                                btype = "<a href='DailyWL_MixParlayAdvance.aspx?&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&db=" + db + "&bettype=9&ispopup=false'><b>" + btype + "</b></a>";
                            }
                        }
                        else if (SportType == 98)// Combination Mix Parlay
                        {                            
                            btype = "<a href='DailyWL_CombinationMixParlay.aspx?&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&db=" + db + "&bettype=29'><b>" + btype + "</b></a>";
                        }
                        else if (SportType == 2)//Other
                        {
                            btype = "<a href='DailyWL_OtherSport.aspx?sporttype=" + sporttype + "&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&db=" + db + "'><b>" + btype + "</b></a>";
                            Hashtable hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_OtherSport.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_other"] = hash;
                        }
                        else if (SportType == 161)//Number game
                        {
                            btype = "<a href='DailyWL_Frames.aspx?sporttype=" + sporttype + "&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&type=numbergame&db=" + db + "'><b>" + btype + "</b></a>";
                            var hash = new Hashtable();
                            hash.Add("sporttype", sporttype);
                            hash.Add("path", sportname);
                            hash.Add("page", "DailyWL_Soccer.aspx");
                            hash.Add("linkclick", dr["SportName"].ToString());
                            Session["sitemap_numbergame"] = hash;
                        }
                        else if (SportType == 162)//Live Casino
                        {
                            btype = "<a href='../LiveCasino/Report/DailyWL_LiveCasino.aspx?&fdate=" + wldate.ToShortDateString() + "&path=" + sportname + "&db=" + db + "&bettype=9'><b>" + btype + "</b></a>";
                        }

                       
                    }

                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);

                    if(Is8RM)
                        row = "<tr style='height:18px;' align='right'><td align='left'>" + btype + "</td><td colspan='3'></td><td nowrap>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(LineTotal) + "</td><td colspan='2'></td></tr>";
                    else
                        Reports.genrow2h(ref row, color, btype, Win, Pos, Com, LineTotal, Turnover, Is8RM);
                    html.Append(row);

                    if (indexSport == 1 && SportType != 1)
                    {
                        colspan = colspans.Clone().ToString();
                        TemplateHandler.Assign(ref colspan, "{msg}", "");
                        TemplateHandler.Assign(ref colspan, "{height}", "rpt-height-tr");
                        html.Append(colspan);
                    }
                  

                    switch (SportType)
                    { 
                       
                        case 151: // Line Total HR
                            HRLineTotal += LineTotal;
                            break;
                        case 251: // Line Total Casino
                            CasinoLineTotal += LineTotal;
                            break;
                        case 252:
                            BingoLineTotal += LineTotal;
                            break;
                        case 253:
                            P2PLineTotal += LineTotal;
                            break;
                        case 162:
                            LiveCasinoLineTotal += LineTotal;
                            break;
                       
                        default://not include FI and HR, NumberGame when calculate All Total
                            AllWin += Win; BTypeWin += Win; SportWin += Win;
                            AllPos += Pos; BTypePos += Pos; SportPos += Pos;
                            AllCom += Com; BTypeCom += Com; SportCom += Com;
                            AllTurnover += Turnover; BTypeTurnover += Turnover; SportTurnover += Turnover;
                            AllLineTotal += LineTotal; BTypeLineTotal += LineTotal; SportLineTotal += LineTotal;
                            break;
                    }
                }
                #endregion //end foreach

                #region //total

                if (ibcIndex>0)
                {
                     color = "#dddddd";
                     string totalrow = totalrows.Clone().ToString();
                     Reports.genrow2h(ref totalrow, color, sportname + " Total: ", BTypeWin, BTypePos, BTypeCom, BTypeLineTotal, BTypeTurnover, false);
                     string row = "";
                     html.Append(totalrow);
                     BTypeWin = 0; BTypePos = 0; BTypeCom = 0; BTypeTurnover = 0;
                     pBetType = BetType;
                     pLive = Live;
                     index = 0;
                }

                totalRowSportType = totalrows.Clone().ToString();
                color = "#99cccc";
                Reports.genrow2h(ref totalRowSportType, color, "<b>" + sportname + " Total:</b>", SportWin, SportPos, SportCom, SportLineTotal, SportTurnover, false);

                SportWin = SportPos = SportCom = SportTurnover = 0;

                if (indexSport > 0 && SportType == 1) html.Append(totalRowSportType);

                colspan = colspans.Clone().ToString();
                TemplateHandler.Assign(ref colspan, "{msg}", "");                
                html.Append(colspan);

                //All Total
                totalRowSportType = totalrows.Clone().ToString();
                color = " bgcolor='#dddddd'";
                TemplateHandler.Assign(ref totalRowSportType, "{colorline}", color);
                TemplateHandler.Assign(ref totalRowSportType, "{coloradmin}", color);

                //Not include 8RM when calculate All Total
                AllWin -= IBC8RMWin;
                AllPos -= IBC8RMPos;
                AllCom -= IBC8RMCom;
                AllTurnover -= IBC8RMTurnover;

                double line = AllWin + AllPos + AllCom;
                double Margin = (AllTurnover==0) ? 0 :((line / AllTurnover) * 100);
                Sunplus.Accounting.Business.Report.Reports.genrow(ref totalRowSportType, "", "<b>All Total:</b>", Reports.colorvalue(AllWin), FormatHandler.FormatNumber20(AllPos), FormatHandler.FormatNumber20(AllCom), "<b>" + FormatHandler.FormatNumber20(AllTurnover) + "</b>", "<b>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(line) + "</b>", Reports.colorvalue(Margin, "%"));
                html.Append(totalRowSportType);

                //Total IBC
                if (indexIBC > 0)
                {
                    totalRowSportType = totalrows.Clone().ToString();


                    TemplateHandler.Assign(ref totalRowSportType, "{colorwin}", color);                   

                    double IBCTotal = winlost_8rm + (utilsBARM.IsActive ? winlost_barm : 0);
                    totalRowSportType = "<tr style='height:18px;' align='right'><td nowarap><b>IBC(8RM" + (utilsBARM.IsActive ? " + " + utilsBARM.DisplayName : string.Empty) + ") Total:</b></td><td colspan='3'></td><td nowrap><b>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(IBCTotal) + "</b></td><td colspan='2'></td></tr>";
                    html.Append(totalRowSportType);
                }

              

                //colspan row 
                colspan = colspans.Clone().ToString();
                TemplateHandler.Assign(ref colspan, "{msg}", "");

                html.Append(colspan);

                
                totalRowSportType = totalrows.Clone().ToString();
                double grandtotal_virtual = AllWin + AllPos + AllCom + winlost_8rm + (utilsBARM.IsActive ? CastAide.AsDouble(winlost_barm) : 0);
                double linetotal_actual = CastAide.AsDouble(rs.Outputs["linetotal"], 0);//Real
                double winLost_8rm_actual = winlost_8rm;
                double grandtotal_actual = linetotal_actual + winLost_8rm_actual + (utilsBARM.IsActive ? CastAide.AsDouble(winlost_barm) : 0); //Real
                double Variance = grandtotal_actual - grandtotal_virtual - HRLineTotal - FILineTotal -  CasinoLineTotal - BingoLineTotal - LiveCasinoLineTotal;

                string strRow_ActualCurrency = "<tr align='center'><td colspan='3'></td><td><b>Real</b></td><td><b>Virtual</b></td><td><b>Variance</b></td><td></td></tr>";
                html.Append(strRow_ActualCurrency);

                Sunplus.Accounting.Business.Report.Reports.genrow(ref totalRowSportType, "", "<b>GRAND TOTAL:</b>", "", "", Reports.colorvalue(grandtotal_actual), Reports.colorvalue(Variance), Reports.colorvalue(grandtotal_virtual), "");
                html.Append(totalRowSportType);

                colspan = colspans.Clone().ToString();                
                TemplateHandler.Assign(ref colspan, "{msg}", "Grand Total = (All:Line Total) - (IBC:WinLost) <br> IBC:Commission was ignored");
                html.Append(colspan);
                #endregion end total
            }
            else
            {
                #region no data
                string WL = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryHeader.htm");
                TemplateHandler.Assign(ref WL, "{width}", "10");
                html.Append(WL);
                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                html.Append(norow);
                #endregion
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
            TemplateHandler.Assign(ref norow, "{nodata}", (ex.Message));
            html.Append("</table>");
            
        }
        html.Append("</table>");

        #region footer

        StringBuilder htmlFooter = new StringBuilder();
        string footer = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/DailyWLFooter.htm");
        string url_today = "DailyWL_Sporttype.aspx?F=RECAL&fdate=" + wldate.ToShortDateString() + "&Date=" + DateTime.Today.ToShortDateString();  // - today
        string url_yesterday = "DailyWL_Sporttype.aspx?F=RECAL&fdate=" + wldate.ToShortDateString() + "&Date=" + DateTime.Today.AddDays(-1).ToShortDateString();//- yesterday

        TemplateHandler.Assign(ref footer, "{url-today}", url_today);
        TemplateHandler.Assign(ref footer, "{url-yesterday}", url_yesterday);
        TemplateHandler.Assign(ref footer, "{fdate}", wldate.ToShortDateString());
        TemplateHandler.Assign(ref footer, "{tdate}", wldate.ToShortDateString());
        TemplateHandler.Assign(ref footer, "{Today}", DateTime.Today.ToShortDateString());
        TemplateHandler.Assign(ref footer, "{Yesterday}", DateTime.Today.AddDays(-1).ToShortDateString());
        
        TemplateHandler.RemoveBlock(ref footer, "main");
        htmlFooter.Append(footer);
      
        #endregion end footer

        html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("TableBlank.htm"));
        
        return html.ToString();
    }
    #endregion

    #region ExportToExcel
    public void ExportToExcel(string FileName)
    {
        string strStyle = ReportHTML.CSSExcelReport();

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
        Response.Charset = "";

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        content.RenderControl(htmlWrite);

        Response.Write(strStyle);

        Response.Write(stringWrite.ToString());
        Response.End();
    }
    #endregion

    
}
