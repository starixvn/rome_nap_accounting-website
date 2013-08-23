using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.DataAccess.Report.MatchWL;
using Sunplus.Accounting.Entities.Report.MatchWL;

namespace Sunplus.Accounting.Business.Report.MatchWL
{
    public class MatchWL2aService : IMatchWL2aService
    {
        private readonly IMatchWL2aDAO _matchWl2aDao;
        private string _keyMatchWL2a = "";
        private string _keyWL8RM = "";
        private string _keyMatchWL3rd = "";
        private string _keyWLbyBettype = "";
        private int _pageSize = 188;
        private string _keyMatchWL = "";
        private string _keyListWLbyBettype = "";
        private string _keyListWLbyBettypeTotal = "";
        private string _keyListMatchWL3rd = "";
        private string _keyListGrandTotal = "";

        public MatchWL2aService(IMatchWL2aDAO imaMatchWl2aDao)
        {
            _matchWl2aDao = imaMatchWl2aDao;
            _keyMatchWL2a = "ReportMatchWL2a." + AppSession.Username;
            _keyWL8RM = "ReportWL8RM." + AppSession.Username;
            _keyWLbyBettype = "ReportWLbyBettype." + AppSession.Username;
            _keyMatchWL3rd = "ReportMatchWL3rd." + AppSession.Username;
            _keyMatchWL = "ReportMacthWL." + AppSession.Username;
            _keyListWLbyBettype = "ReportListWLbyBettype." + AppSession.Username;
            _keyListWLbyBettypeTotal = "ReportListWLbyBettypeTotal." + AppSession.Username;
            _keyListMatchWL3rd = "ReportListMatchWL3rd." + AppSession.Username;
            _keyListGrandTotal = "ReportListGrandTotal." + AppSession.Username;
        }

        private DataTable dataTable1 = new DataTable();
        private DataTable dataTable2 = new DataTable();
        private DataTable dtWLbyBettype = new DataTable();

        private string wl8rm = "";

        private DbResult rs = null, rsWLbyBettype = null;
        private List<IEnumerable<MatchWL2aBO>> listMatchWL = new List<IEnumerable<MatchWL2aBO>>();

        #region ParallelCalc

        private void ParallelCalc(DateTime wldate, int currpage)
        {
            bool IsGetFromSession = false;
            IsGetFromSession =
                (AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyMatchWL2a) != null
                 && AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWL8RM) != null
                 && AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyMatchWL3rd) != null
                 && AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWLbyBettype) != null
                 );

            if (currpage == 1)
                IsGetFromSession = false;

            if (IsGetFromSession)
            {
                dataTable1 = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyMatchWL2a) as DataTable;
                wl8rm = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWL8RM) as string;
                dataTable2 = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyMatchWL3rd) as DataTable;

                dtWLbyBettype = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWLbyBettype) as DataTable;
            }
            else
            {
                System.Threading.Tasks.Parallel.Invoke(
                    delegate()
                    {
                        if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                            rs = _matchWl2aDao.MatchWL(wldate);
                        else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                            rs = _matchWl2aDao.MatchWL14(wldate);
                        else
                            rs = _matchWl2aDao.MatchWL_WH(wldate);
                    },
                    delegate()
                    {
                        if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                            rsWLbyBettype = _matchWl2aDao.WinLossByBettype(wldate);
                        else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                            rsWLbyBettype = _matchWl2aDao.WinLossByBettype14(wldate);
                        else
                            rsWLbyBettype = _matchWl2aDao.WinLossByBettype_WH(wldate);
                    }
                    );

                dataTable1 = rs.ResultSet.Tables[0];
                dataTable2 = rs.ResultSet.Tables[1];
                wl8rm = NumberFormat.DecFormat(rs.Outputs["winlost_8rm"], 2);

                dtWLbyBettype = rsWLbyBettype.ResultSet.Tables[0];

                dtWLbyBettype.Columns.Add("index", typeof(float));

                foreach (DataRow row in dtWLbyBettype.Rows)
                {
                    row["index"] = row["bettype"];
                    if (CastAide.AsInt32(row["bettype"]) == 29)
                        row["index"] = 9.5;
                }

                DataView dv = dtWLbyBettype.DefaultView;
                dv.Sort = "index asc";
                dtWLbyBettype = dv.ToTable();

                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyMatchWL2a, dataTable1);
                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyWL8RM, wl8rm);
                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyMatchWL3rd, dataTable2);

                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyWLbyBettype, dtWLbyBettype);
            }
        }

        #endregion ParallelCalc

        #region MatchWL

        private MatchWL2aBO CalcTotal(string expression, int typeTotal, DataTable dataTable1)
        {
            MatchWL2aBO data = new MatchWL2aBO();
            data.typeTotal = typeTotal;

            data.comm = NumberFormat.DecFormat(dataTable1.Compute("sum(comm)", expression), 2);
            data.VirtualComm = NumberFormat.DecFormat(dataTable1.Compute("sum(VirtualComm)", expression), 2);
            data.LineTotal = NumberFormat.DecFormat(dataTable1.Compute("sum(LineTotal)", expression), 2);
            data.VirtualLineTotal = NumberFormat.DecFormat(dataTable1.Compute("sum(VirtualLineTotal)", expression), 2);
            data.variance = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable1.Compute("sum(LineTotal)", expression)) - CastAide.AsDecimal(dataTable1.Compute("sum(VirtualLineTotal)", expression)), 2);
            return data;
        }

        private List<string> profiler = new List<string>();

        public MatchWL2aDTO SelMatchWL2a(DateTime wldate, int currpage)
        {
            MatchWL2aDTO matchWl2aDto = new MatchWL2aDTO();
            try
            {
                matchWl2aDto.currpage = currpage + 1;
                matchWl2aDto.IsFinish = false;

                //business here ...

                ParallelCalc(wldate, currpage);

                #region MatchWL

                List<MatchWL2aBO> Rows = new List<MatchWL2aBO>();
                List<MatchWL2aBO> Rows3rd = new List<MatchWL2aBO>();

                int beginrow = (currpage - 1) * _pageSize;
                int endrow = currpage * _pageSize - 1;
                if (endrow >= dataTable1.Rows.Count)
                {
                    endrow = dataTable1.Rows.Count - 1;
                    matchWl2aDto.IsFinish = true;
                }

                int index = 0;
                int oldbettype = 0;
                int oldlive = 0;
                string oldleague = "";
                string expression = "";

                MatchWL2aBO data = new MatchWL2aBO();
                int k = 0;
                for (int i = beginrow; i <= endrow; i++)
                {
                    DataRow dataRow = dataTable1.Rows[i];

                    #region bettype or live

                    if (CastAide.AsInt32(dataRow["bettype"]) != oldbettype
                        || (CastAide.AsInt32(dataRow["liveindicator"], 0) != oldlive)
                        || (CastAide.AsString(dataRow["leaguenametc"]) != oldleague)
                        )
                    {
                        if (index != 0)
                        {
                            data = new MatchWL2aBO();
                            expression = "bettype = " + oldbettype + " and liveindicator=" + oldlive + " and leaguenametc= '" + oldleague + "' ";
                            data = CalcTotal(expression, 1, dataTable1);
                            Rows.Add(data);

                            if (CastAide.AsString(dataRow["leaguenametc"]) != oldleague)
                            {
                                data = new MatchWL2aBO();
                                expression = "leaguenametc= '" + oldleague + "' ";
                                data = CalcTotal(expression, 2, dataTable1);
                                data.leaguenametc = oldleague;
                                Rows.Add(data);
                            }
                        }
                        oldbettype = CastAide.AsInt32(dataRow["bettype"]);
                        oldlive = CastAide.AsInt32(dataRow["liveindicator"], 0);
                        oldleague = CastAide.AsString(dataRow["leaguenametc"]);
                    }

                    #endregion bettype or live

                    data = new MatchWL2aBO();
                    index++;

                    data.matchcode = CastAide.AsString(dataRow["matchcode"]);
                    data.eventdate = CastAide.AsDateTime(dataRow["eventdate"]).ToShortDateString();
                    data.kickofftime = CastAide.AsDateTime(dataRow["kickofftime"]).ToShortTimeString();
                    data.liveindicator = CastAide.AsBoolean(dataRow["liveindicator"]);
                    data.bettype = CacheRef.GetBettype(dataRow["bettype"]);
                    data.homename = CacheRef.GetTeamName(dataRow["homeid"]);
                    data.awayname = CacheRef.GetTeamName(dataRow["awayid"]);
                    data.winlost = NumberFormat.DecFormat(dataRow["winlost"], 2);
                    data.pos = NumberFormat.DecFormat(dataRow["pos"], 2);
                    data.comm = NumberFormat.DecFormat(dataRow["comm"], 2);
                    data.VirtualComm = NumberFormat.DecFormat(dataRow["virtualcomm"], 2);
                    data.LineTotal = NumberFormat.DecFormat(dataRow["linetotal"], 2);
                    data.VirtualLineTotal = NumberFormat.DecFormat(dataRow["VirtualLineTotal"], 2);
                    data.leaguenametc = CastAide.AsString(dataRow["leaguenametc"]);
                    data.IsNeutral = CastAide.AsBoolean(dataRow["IsNeutral"]);
                    data.variance = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["linetotal"]) - CastAide.AsDecimal(dataRow["VirtualLineTotal"]), 2);

                    Rows.Add(data);
                    index++;
                }

                StringBuilder builder = new StringBuilder();

                foreach (string msg in profiler)
                {
                    builder.Append(msg + "<br/>");
                }

                if (matchWl2aDto.IsFinish)
                {
                    #region bettype or live or sport

                    data = new MatchWL2aBO();
                    expression = "bettype = " + oldbettype + " and liveindicator=" + oldlive + " and leaguenametc= '" + oldleague + "' ";
                    data = CalcTotal(expression, 1, dataTable1);
                    Rows.Add(data);

                    data = new MatchWL2aBO();
                    expression = "leaguenametc= '" + oldleague + "' ";
                    data = CalcTotal(expression, 2, dataTable1);
                    data.leaguenametc = oldleague;
                    Rows.Add(data);

                    #endregion bettype or live or sport

                    #region calc grand total

                    data = new MatchWL2aBO();
                    expression = "";
                    data = CalcTotal(expression, 3, dataTable1);
                    data.leaguenametc = "GRAND ";
                    Rows.Add(data);

                    #endregion calc grand total
                }

                #endregion MatchWL

                if (currpage == 1)
                {
                    #region WL3rd

                    foreach (DataRow dataRow in dataTable2.Rows)
                    {
                        data = new MatchWL2aBO();
                        data.eventdate = CastAide.AsDateTime(dataRow["winlostdate"]).ToShortDateString();
                        data.bettype = CastAide.AsString(dataRow["bettype"]);
                        data.winlost = NumberFormat.DecFormat(dataRow["winlost"], 2);
                        data.pos = NumberFormat.DecFormat(dataRow["pos"], 2);
                        data.comm = NumberFormat.DecFormat(dataRow["comm"], 2);
                        data.VirtualComm = NumberFormat.DecFormat(dataRow["VirtualComm"], 2);
                        data.LineTotal = NumberFormat.DecFormat(dataRow["LineTotal"], 2);
                        data.VirtualLineTotal = NumberFormat.DecFormat(dataRow["VirtualLineTotal"], 2);
                        data.variance = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["linetotal"]) - CastAide.AsDecimal(dataRow["VirtualLineTotal"]), 2);
                        data.type3rd = 1;

                        if (data.bettype.ToLower().Equals("casino") || data.bettype.ToLower().Equals("p2p") || data.bettype.ToLower().Equals("racing"))
                        {
                            data.type3rd = 2;//display link
                            data.variance = "N/A";
                        }

                        if (wldate < ReportUtils.BalanceUp.BalanceUpDay)
                        {
                            if (data.bettype.ToLower().Equals("racing"))
                            {
                                data.type3rd = 1;// don't display link, just colspan column home-vs-away
                            }
                        }

                        Rows3rd.Add(data);
                    }

                    AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyListMatchWL3rd, Rows3rd);

                    #endregion WL3rd

                    #region Grand Total

                    expression = "";
                    var grandTotalMatchWL = new MatchWL2aBO();
                    grandTotalMatchWL = CalcTotal(expression, 3, dataTable1);
                    grandTotalMatchWL.leaguenametc = "GRAND ";

                    var grandTotalMatchWL3rd = new MatchWL2aBO();
                    grandTotalMatchWL3rd = CalcTotal(expression, 3, dataTable2);
                    grandTotalMatchWL3rd.leaguenametc = "GRAND ";

                    grandTotalMatchWL.comm = NumberFormat.DecFormat(CastAide.AsDecimal(grandTotalMatchWL.comm) + CastAide.AsDecimal(grandTotalMatchWL3rd.comm), 2);
                    grandTotalMatchWL.VirtualComm = NumberFormat.DecFormat(CastAide.AsDecimal(grandTotalMatchWL.VirtualComm) + CastAide.AsDecimal(grandTotalMatchWL3rd.VirtualComm), 2);
                    grandTotalMatchWL.LineTotal = NumberFormat.DecFormat(CastAide.AsDecimal(grandTotalMatchWL.LineTotal) + CastAide.AsDecimal(grandTotalMatchWL3rd.LineTotal) + CastAide.AsDecimal(wl8rm), 2);
                    grandTotalMatchWL.VirtualLineTotal = NumberFormat.DecFormat(CastAide.AsDecimal(grandTotalMatchWL.VirtualLineTotal) + CastAide.AsDecimal(grandTotalMatchWL3rd.VirtualLineTotal) + CastAide.AsDecimal(wl8rm), 2);
                    grandTotalMatchWL.variance = NumberFormat.DecFormat(CastAide.AsDecimal(grandTotalMatchWL.variance) + Rows3rd.Sum(s => CastAide.AsDecimal(s.variance)), 2);

                    grandTotalMatchWL.typeTotal = 3;

                    List<MatchWL2aBO> grandTotal = new List<MatchWL2aBO>();
                    grandTotal.Add(grandTotalMatchWL);
                    matchWl2aDto.rowsMatchWLTotal = grandTotal;

                    AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyListGrandTotal, grandTotal);

                    #endregion Grand Total
                }

                matchWl2aDto.wl8rm = wl8rm;
                matchWl2aDto.rows = Rows;
                matchWl2aDto.rows3rd = Rows3rd;

                if (currpage == 1)
                {
                    List<WLbyBettype> listWLbyBettypes = new List<WLbyBettype>();
                    List<WLbyBettype> listWLbyBettypesTotal = new List<WLbyBettype>();

                    DataTable dtLastestOrder = dtWLbyBettype.Clone();
                    getWLbyBettype(ref listWLbyBettypes, ref dtLastestOrder, ref listWLbyBettypesTotal, false);
                    dtWLbyBettype = dtLastestOrder;
                    getWLbyBettype(ref listWLbyBettypes, ref dtLastestOrder, ref listWLbyBettypesTotal, true);

                    matchWl2aDto.rowsWLbyBettype = listWLbyBettypes;
                    matchWl2aDto.rowsWLbyBettypeTotal = listWLbyBettypesTotal;

                    AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyListWLbyBettype, listWLbyBettypes);
                    AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyListWLbyBettypeTotal, listWLbyBettypesTotal);

                    listMatchWL.Add(matchWl2aDto.rows);
                    AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyMatchWL, listMatchWL);
                }
                else
                {
                    listMatchWL = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyMatchWL) as List<IEnumerable<MatchWL2aBO>>;
                    listMatchWL.Add(matchWl2aDto.rows);
                    AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyMatchWL, listMatchWL);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return matchWl2aDto;
        }

        #endregion MatchWL

        #region WLbyBettype

        private void getWLbyBettype(ref List<WLbyBettype> wLbyBettypes, ref DataTable dtLastestOrder, ref List<WLbyBettype> listWLbyBettypesTotal, bool IsLastest)
        {
            try
            {

                WLbyBettype data = new WLbyBettype();

                foreach (DataRow dataRow in dtWLbyBettype.Rows)
                {
                    if (!IsLastest)
                    {
                        switch (CastAide.AsInt32(dataRow["bettype"]))
                        {
                            case 9: //mp
                            case 29: //mp - combination
                            case 10: //or
                            case 31: //win Tote Odds
                            case 32: //place Tote Odds
                            case 33: //winplace Tote Odds
                            case 41: //win Fixed Odds
                            case 42: //place Fixed Odds
                            case 43: //winplace Fixed Odds
                            case 400: //8rm
                            case 150: //3rdparty
                            case 200: //12bet
                            case 71: //CASINO- 71
                            case 72: //P2P-72
                                DataRow dr2 = dtLastestOrder.NewRow();
                                for (int i = 0; i < dataRow.ItemArray.Length; i++)
                                {
                                    dr2[i] = dataRow[i];
                                }
                                dtLastestOrder.Rows.Add(dr2);
                                continue;
                        }
                    }

                    data = new WLbyBettype();

                    data.LineTotal_soccer_live = NumberFormat.DecFormat(dataRow["LineTotal_soccer_live"], 2);
                    data.LineTotal_soccer = NumberFormat.DecFormat(dataRow["LineTotal_soccer"], 2);
                    data.LineTotal_others_live = NumberFormat.DecFormat(dataRow["LineTotal_others_live"], 2);
                    data.LineTotal_others = NumberFormat.DecFormat(dataRow["LineTotal_others"], 2);

                    switch (CastAide.AsInt32(dataRow["bettype"]))
                    {
                        case 51: //5 min
                        case 52: //15 min O/U
                        case 53: //30 min O/U
                        case 54: //45 min O/U
                        case 55: //60 min O/U
                        case 56: //End of day O/U

                        case 61: //5 min OE
                        case 62: //15 min OE
                        case 63: //30 min OE
                        case 64: //45 min OE
                        case 65: //60 min OE
                        case 66: //End of day OE

                        case 31: //win Tote Odds
                        case 32: //place Tote Odds
                        case 33: //win-place Tote Odds
                        case 41: //win Fixed Odds
                        case 42: //place Fixed Odds
                        case 43: //win-place Fixed Odds
                        case 150: //3rdparty
                        case 71: //casino
                        case 72: //p2p
                            data.VirtualLineTotal_soccer_live = "N/A";
                            data.VirtualLineTotal_soccer = "N/A";
                            data.VirtualLineTotal_others_live = "N/A";
                            data.VirtualLineTotal_others = "N/A";

                            data.LineTotalReal =
                                NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["LineTotal_soccer_live"])
                                                       + CastAide.AsDecimal(dataRow["LineTotal_soccer"])
                                                       + CastAide.AsDecimal(dataRow["LineTotal_others_live"])
                                                       + CastAide.AsDecimal(dataRow["LineTotal_others"]), 2);
                            data.LineTotalVirtual = "N/A";
                            data.Variance = "N/A";
                            break;

                        default:
                            data.VirtualLineTotal_soccer_live =
                                NumberFormat.DecFormat(dataRow["VirtualLineTotal_soccer_live"], 2);
                            data.VirtualLineTotal_soccer = NumberFormat.DecFormat(dataRow["VirtualLineTotal_soccer"], 2);
                            data.VirtualLineTotal_others_live =
                                NumberFormat.DecFormat(dataRow["VirtualLineTotal_others_live"], 2);
                            data.VirtualLineTotal_others = NumberFormat.DecFormat(dataRow["VirtualLineTotal_others"], 2);

                            data.LineTotalReal =
                                NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["LineTotal_soccer_live"])
                                                       + CastAide.AsDecimal(dataRow["LineTotal_soccer"])
                                                       + CastAide.AsDecimal(dataRow["LineTotal_others_live"])
                                                       + CastAide.AsDecimal(dataRow["LineTotal_others"]), 2);

                            data.LineTotalVirtual =
                                NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["VirtualLineTotal_soccer_live"])
                                                       + CastAide.AsDecimal(dataRow["VirtualLineTotal_soccer"])
                                                       + CastAide.AsDecimal(dataRow["VirtualLineTotal_others_live"])
                                                       + CastAide.AsDecimal(dataRow["VirtualLineTotal_others"]), 2);
                            data.Variance =
                                NumberFormat.DecFormat(
                                    CastAide.AsDecimal(data.LineTotalReal) - CastAide.AsDecimal(data.LineTotalVirtual),
                                    2);
                            break;
                    }

                    string typename = "";
                    switch (CastAide.AsInt32(dataRow["bettype"]))
                    {
                        case 150:
                            typename = "FI 3rd-Party";
                            break;

                        case 400:
                            typename = "8RM";
                            break;

                        case 71:
                            typename = "CASINO";
                            break; //CASINO- 71
                        case 1001:
                            typename = "Kick Off";
                            break;

                        case 29:
                            typename = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "systemParlay");
                            break;

                        default:
                            if (CastAide.AsInt32(dataRow["bettype"]) == 85 || CastAide.AsInt32(dataRow["bettype"]) == 86)
                                typename = CacheRef.GetBettype(dataRow["bettype"]) + " (NumberGame)";
                            else
                                typename = CacheRef.GetBettype(dataRow["bettype"]);
                            break;
                    }
                    typename += " " + dataRow["Hedge"];
                    data.bettype = typename;

                    wLbyBettypes.Add(data);
                }

                #region Total

                if (IsLastest)
                {
                    dtWLbyBettype =
                        AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWLbyBettype) as DataTable;
                    if (dtWLbyBettype != null)
                    {
                        data = new WLbyBettype();
                        data.bettype = "Grand Total";

                        data.LineTotal_soccer_live =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(LineTotal_soccer_live)", ""), 2);
                        data.LineTotal_soccer =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(LineTotal_soccer)", ""), 2);
                        data.LineTotal_others_live =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(LineTotal_others_live)", ""), 2);
                        data.LineTotal_others =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(LineTotal_others)", ""), 2);

                        data.VirtualLineTotal_soccer_live =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(VirtualLineTotal_soccer_live)", ""), 2);
                        data.VirtualLineTotal_soccer =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(VirtualLineTotal_soccer)", ""), 2);
                        data.VirtualLineTotal_others_live =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(VirtualLineTotal_others_live)", ""), 2);
                        data.VirtualLineTotal_others =
                            NumberFormat.DecFormat(dtWLbyBettype.Compute("sum(VirtualLineTotal_others)", ""), 2);

                        data.LineTotalReal = NumberFormat.DecFormat(
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(LineTotal_soccer_live)", "")) +
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(LineTotal_soccer)", "")) +
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(LineTotal_others_live)", "")) +
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(LineTotal_others)", ""))
                            , 2);
                        data.LineTotalVirtual = NumberFormat.DecFormat(
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(VirtualLineTotal_soccer_live)", "")) +
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(VirtualLineTotal_soccer)", "")) +
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(VirtualLineTotal_others_live)", "")) +
                            CastAide.AsDecimal(dtWLbyBettype.Compute("sum(VirtualLineTotal_others)", ""))
                            , 2);
                        data.Variance =
                            NumberFormat.DecFormat(
                                wLbyBettypes.Where(s => s.LineTotalVirtual != "N/A").Sum(
                                    s => CastAide.AsDecimal(s.LineTotalReal))
                                -
                                wLbyBettypes.Where(s => s.LineTotalVirtual != "N/A").Sum(
                                    s => CastAide.AsDecimal(s.LineTotalVirtual)), 2);

                        data.IsTotal = true;
                        listWLbyBettypesTotal.Add(data);
                    }
                }

                #endregion Total
            }
            catch (Exception ex)
            {
            }
        }

        #endregion WLbyBettype

        #region ExportMatchWL

        private void AssignWLbyBettypeExport(ref ExcelWorksheet ws, ref int indexRow, List<WLbyBettype> list, bool IsTotal, int r, int g, int b)
        {
            var gembox = new ExcelGemboxUtils();
            foreach (WLbyBettype data in list)
            {
                ws.Cells[indexRow, 0].Value = data.bettype;
                ws.Cells[indexRow, 0].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center, IsTotal);

                ws.Cells[indexRow, 1].Value = CastAide.AsDecimal(data.LineTotal_soccer_live);
                ws.Cells[indexRow, 1].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                if (data.VirtualLineTotal_soccer_live == "N/A")
                {
                    ws.Cells[indexRow, 2].Value = "N/A";
                    ws.Cells[indexRow, 2].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, IsTotal);
                }
                else
                {
                    ws.Cells[indexRow, 2].Value = CastAide.AsDecimal(data.VirtualLineTotal_soccer_live);
                    ws.Cells[indexRow, 2].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                }

                ws.Cells[indexRow, 3].Value = CastAide.AsDecimal(data.LineTotal_soccer);
                ws.Cells[indexRow, 3].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                if (data.VirtualLineTotal_soccer == "N/A")
                {
                    ws.Cells[indexRow, 4].Value = "N/A";
                    ws.Cells[indexRow, 4].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, IsTotal);
                }
                else
                {
                    ws.Cells[indexRow, 4].Value = CastAide.AsDecimal(data.VirtualLineTotal_soccer);
                    ws.Cells[indexRow, 4].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                }

                ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(data.LineTotal_others_live);
                ws.Cells[indexRow, 5].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                if (data.VirtualLineTotal_others_live == "N/A")
                {
                    ws.Cells[indexRow, 6].Value = "N/A";
                    ws.Cells[indexRow, 6].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, IsTotal);
                }
                else
                {
                    ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(data.VirtualLineTotal_others_live);
                    ws.Cells[indexRow, 6].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                }

                ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(data.LineTotal_others);
                ws.Cells[indexRow, 7].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                if (data.VirtualLineTotal_others == "N/A")
                {
                    ws.Cells[indexRow, 8].Value = "N/A";
                    ws.Cells[indexRow, 8].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, IsTotal);
                }
                else
                {
                    ws.Cells[indexRow, 8].Value = CastAide.AsDecimal(data.VirtualLineTotal_others);
                    ws.Cells[indexRow, 8].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                }

                ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(data.LineTotalReal);
                ws.Cells[indexRow, 9].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                if (data.LineTotalVirtual == "N/A")
                {
                    ws.Cells[indexRow, 10].Value = "N/A";
                    ws.Cells[indexRow, 10].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, IsTotal);
                }
                else
                {
                    ws.Cells[indexRow, 10].Value = CastAide.AsDecimal(data.LineTotalVirtual);
                    ws.Cells[indexRow, 10].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                }

                if (data.Variance == "N/A")
                {
                    ws.Cells[indexRow, 11].Value = "N/A";
                    ws.Cells[indexRow, 11].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, IsTotal);
                }
                else
                {
                    ws.Cells[indexRow, 11].Value = CastAide.AsDecimal(data.Variance);
                    ws.Cells[indexRow, 11].Style = gembox.GetStyleNumber(0, "", r, g, b, IsTotal);
                }

                indexRow++;
            }
        }

        public ExcelFile ExportMatchWL(ref bool hasData)
        {
            hasData = true;
            ExcelFile excelFile = new ExcelFile();
            var dtWLbyBettypeExport = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWLbyBettype) as DataTable;
            if (dtWLbyBettypeExport == null)
                hasData = false;

            var gembox = new ExcelGemboxUtils();

            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            ExcelWorksheet ws = null;
            string tag = "Match Winlost 2a";
            ws = worksheets.Add(tag);
            int indexRow = 0;

            #region WLbyBettype

            #region header

            ws.Cells.GetSubrangeAbsolute(0, 0, 1, 0).Value = "Bet Type";
            ws.Cells.GetSubrangeAbsolute(0, 0, 1, 0).Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(0, 0, 1, 0).Merged = true;

            ws.Cells.GetSubrangeAbsolute(0, 1, 0, 2).Value = "Soccer Live";
            ws.Cells.GetSubrangeAbsolute(0, 1, 0, 2).Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(0, 1, 0, 2).Merged = true;
            ws.Cells[1, 1].Value = "Real";
            ws.Cells[1, 1].Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells[1, 2].Value = "Virtual";
            ws.Cells[1, 2].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells.GetSubrangeAbsolute(0, 3, 0, 4).Value = "Soccer Non-Live";
            ws.Cells.GetSubrangeAbsolute(0, 3, 0, 4).Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(0, 3, 0, 4).Merged = true;
            ws.Cells[1, 3].Value = "Real";
            ws.Cells[1, 3].Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells[1, 4].Value = "Virtual";
            ws.Cells[1, 4].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells.GetSubrangeAbsolute(0, 5, 0, 6).Value = "Other Live";
            ws.Cells.GetSubrangeAbsolute(0, 5, 0, 6).Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(0, 5, 0, 6).Merged = true;
            ws.Cells[1, 5].Value = "Real";
            ws.Cells[1, 5].Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells[1, 6].Value = "Virtual";
            ws.Cells[1, 6].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells.GetSubrangeAbsolute(0, 7, 0, 8).Value = "Other Non-Live";
            ws.Cells.GetSubrangeAbsolute(0, 7, 0, 8).Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(0, 7, 0, 8).Merged = true;
            ws.Cells[1, 7].Value = "Real";
            ws.Cells[1, 7].Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells[1, 8].Value = "Virtual";
            ws.Cells[1, 8].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells.GetSubrangeAbsolute(0, 9, 0, 10).Value = "Line Total";
            ws.Cells.GetSubrangeAbsolute(0, 9, 0, 10).Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(0, 9, 0, 10).Merged = true;
            ws.Cells[1, 9].Value = "Real";
            ws.Cells[1, 9].Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells[1, 10].Value = "Virtual";
            ws.Cells[1, 10].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells.GetSubrangeAbsolute(0, 11, 1, 11).Value = "Variance";
            ws.Cells.GetSubrangeAbsolute(0, 11, 1, 11).Style = gembox.GetStyleTitle(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(0, 11, 1, 11).Merged = true;

            #endregion header

            #region content

            indexRow = 2;
            List<WLbyBettype> listWLbyBettype = new List<WLbyBettype>();
            listWLbyBettype = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyListWLbyBettype) as List<WLbyBettype>;
            int r = 230, g = 233, b = 240;
            AssignWLbyBettypeExport(ref ws, ref indexRow, listWLbyBettype, false, r, g, b);

            List<WLbyBettype> listWLbyBettypeTotal = new List<WLbyBettype>();
            listWLbyBettypeTotal = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyListWLbyBettypeTotal) as List<WLbyBettype>;
            r = 136; g = 170; b = 255;
            AssignWLbyBettypeExport(ref ws, ref indexRow, listWLbyBettypeTotal, true, r, g, b);

            #endregion content

            #endregion WLbyBettype

            indexRow = indexRow + 2;

            #region MatchWL

            #region header

            ws.Cells[indexRow, 0].Value = "Bet Type";
            ws.Cells[indexRow, 0].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells[indexRow, 1].Value = "Date";
            ws.Cells[indexRow, 1].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Value = "Match";
            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Style = gembox.GetStyleTextTotal(181, 196, 208);
            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Merged = true;

            ws.Cells[indexRow, 5].Value = "Winloss";
            ws.Cells[indexRow, 5].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells[indexRow, 6].Value = "Position";
            ws.Cells[indexRow, 6].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells[indexRow, 7].Value = "Comm";
            ws.Cells[indexRow, 7].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells[indexRow, 8].Value = "Virtual Comm";
            ws.Cells[indexRow, 8].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells[indexRow, 9].Value = "Line Total";
            ws.Cells[indexRow, 9].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells[indexRow, 10].Value = "Virtual Line Total";
            ws.Cells[indexRow, 10].Style = gembox.GetStyleTitle(181, 196, 208);

            ws.Cells[indexRow, 11].Value = "Variance";
            ws.Cells[indexRow, 11].Style = gembox.GetStyleTitle(181, 196, 208);
            indexRow++;

            #endregion header

            #region content

            listMatchWL = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyMatchWL) as List<IEnumerable<MatchWL2aBO>>;
            r = 230; g = 233; b = 240;
            foreach (IEnumerable<MatchWL2aBO> matchWl2aBos in listMatchWL)
            {
                foreach (MatchWL2aBO matchWl2aBo in matchWl2aBos)
                {
                    r = 230; g = 233; b = 240;
                    if (matchWl2aBo.typeTotal == 0)
                    {
                        if (matchWl2aBo.type3rd == 0)
                        {
                            if (matchWl2aBo.liveindicator)
                            {
                                ws.Cells[indexRow, 0].Value = "!Live -";
                            }
                            ws.Cells[indexRow, 0].Value += matchWl2aBo.bettype + " " + matchWl2aBo.matchcode;
                        }
                        else
                        {
                            ws.Cells[indexRow, 0].Value = matchWl2aBo.bettype;
                        }
                        ws.Cells[indexRow, 0].Style = gembox.GetStyleText(r, g, b);

                        ws.Cells[indexRow, 1].Value = matchWl2aBo.eventdate + " " + matchWl2aBo.kickofftime;
                        ws.Cells[indexRow, 1].Style = gembox.GetStyleText(r, g, b);

                        if (matchWl2aBo.type3rd == 0)
                        {
                            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Value = matchWl2aBo.homename + "-vs-" + matchWl2aBo.awayname;
                            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Merged = true;
                        }
                        else
                        {
                            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                            ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Merged = true;
                        }

                        ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(matchWl2aBo.winlost);
                        ws.Cells[indexRow, 5].Style = gembox.GetStyleNumber(0, "", r, g, b);

                        ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(matchWl2aBo.pos);
                        ws.Cells[indexRow, 6].Style = gembox.GetStyleNumber(0, "", r, g, b);

                        ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(matchWl2aBo.comm);
                        ws.Cells[indexRow, 7].Style = gembox.GetStyleNumber(0, "", r, g, b);

                        ws.Cells[indexRow, 8].Value = CastAide.AsDecimal(matchWl2aBo.VirtualComm);
                        ws.Cells[indexRow, 8].Style = gembox.GetStyleNumber(0, "", r, g, b);

                        ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(matchWl2aBo.LineTotal);
                        ws.Cells[indexRow, 9].Style = gembox.GetStyleNumber(0, "", r, g, b);

                        ws.Cells[indexRow, 10].Value = CastAide.AsDecimal(matchWl2aBo.VirtualLineTotal);
                        ws.Cells[indexRow, 10].Style = gembox.GetStyleNumber(0, "", r, g, b);

                        ws.Cells[indexRow, 11].Value = CastAide.AsDecimal(matchWl2aBo.variance);
                        ws.Cells[indexRow, 11].Style = gembox.GetStyleNumber(0, "", r, g, b);
                    }
                    else
                    {
                        if (matchWl2aBo.typeTotal == 3)
                        {
                            r = 136; g = 170; b = 255;
                            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Value = "GRAND TOTAL:";
                        }
                        else if (matchWl2aBo.typeTotal == 2)
                        {
                            r = 211; g = 255; b = 200;
                            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Value = matchWl2aBo.leaguenametc + " TOTAL:";
                        }
                        else
                        {
                            r = 255; g = 238; b = 211;
                            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Value = "TOTAL:";
                        }

                        ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, true);
                        ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Merged = true;

                        ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(matchWl2aBo.comm);
                        ws.Cells[indexRow, 7].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                        ws.Cells[indexRow, 8].Value = CastAide.AsDecimal(matchWl2aBo.VirtualComm);
                        ws.Cells[indexRow, 8].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                        ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(matchWl2aBo.LineTotal);
                        ws.Cells[indexRow, 9].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                        ws.Cells[indexRow, 10].Value = CastAide.AsDecimal(matchWl2aBo.VirtualLineTotal);
                        ws.Cells[indexRow, 10].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                        ws.Cells[indexRow, 11].Value = CastAide.AsDecimal(matchWl2aBo.variance);
                        ws.Cells[indexRow, 11].Style = gembox.GetStyleNumber(0, "", r, g, b, true);
                    }

                    indexRow++;
                }
            }

            #endregion content

            #region 8RM

            r = 124; g = 152; b = 164;
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 8).Value = "Total 8RM:";
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 8).Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, true);
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 8).Merged = true;

            ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWL8RM));
            ws.Cells[indexRow, 9].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

            ws.Cells[indexRow, 10].Value = CastAide.AsDecimal(AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyWL8RM));
            ws.Cells[indexRow, 10].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

            ws.Cells[indexRow, 11].Value = 0;
            ws.Cells[indexRow, 11].Style = gembox.GetStyleNumber(0, "", r, g, b, false);

            #endregion 8RM

            #region 3rd

            List<MatchWL2aBO> Row3rd = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyListMatchWL3rd) as List<MatchWL2aBO>;
            indexRow++;
            foreach (MatchWL2aBO matchWl2aBo in Row3rd)
            {
                r = 230; g = 233; b = 240;

                ws.Cells[indexRow, 0].Value = matchWl2aBo.bettype;
                ws.Cells[indexRow, 0].Style = gembox.GetStyleText(r, g, b);

                ws.Cells[indexRow, 1].Value = matchWl2aBo.eventdate;
                ws.Cells[indexRow, 1].Style = gembox.GetStyleText(r, g, b);

                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Value = "";
                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Style = gembox.GetStyleText(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow, 4).Merged = true;

                ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(matchWl2aBo.winlost);
                ws.Cells[indexRow, 5].Style = gembox.GetStyleNumber(0, "", r, g, b);

                ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(matchWl2aBo.pos);
                ws.Cells[indexRow, 6].Style = gembox.GetStyleNumber(0, "", r, g, b);

                ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(matchWl2aBo.comm);
                ws.Cells[indexRow, 7].Style = gembox.GetStyleNumber(0, "", r, g, b);

                if (matchWl2aBo.bettype.ToLower().Equals("casino") || matchWl2aBo.bettype.ToLower().Equals("p2p") || matchWl2aBo.bettype.ToLower().Equals("tote hr"))
                {
                    ws.Cells[indexRow, 8].Value = "N/A";
                    ws.Cells[indexRow, 8].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right);

                    ws.Cells[indexRow, 10].Value = "N/A";
                    ws.Cells[indexRow, 10].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right);

                    ws.Cells[indexRow, 11].Value = "N/A";
                    ws.Cells[indexRow, 11].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right);
                }
                else
                {
                    ws.Cells[indexRow, 8].Value = CastAide.AsDecimal(matchWl2aBo.VirtualComm);
                    ws.Cells[indexRow, 8].Style = gembox.GetStyleNumber(0, "", r, g, b);

                    ws.Cells[indexRow, 10].Value = CastAide.AsDecimal(matchWl2aBo.VirtualLineTotal);
                    ws.Cells[indexRow, 10].Style = gembox.GetStyleNumber(0, "", r, g, b);

                    ws.Cells[indexRow, 11].Value = CastAide.AsDecimal(matchWl2aBo.variance);
                    ws.Cells[indexRow, 11].Style = gembox.GetStyleNumber(0, "", r, g, b);
                }

                ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(matchWl2aBo.LineTotal);
                ws.Cells[indexRow, 9].Style = gembox.GetStyleNumber(0, "", r, g, b);

                indexRow++;
            }

            #endregion 3rd

            #region Grand Total

            List<MatchWL2aBO> GrandTotal = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyListGrandTotal) as List<MatchWL2aBO>;
            foreach (MatchWL2aBO matchWl2aBo in GrandTotal)
            {
                r = 136; g = 170; b = 255;

                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Value = "GRAND TOTAL:";
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Right, true);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 6).Merged = true;

                ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(matchWl2aBo.comm);
                ws.Cells[indexRow, 7].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                ws.Cells[indexRow, 8].Value = CastAide.AsDecimal(matchWl2aBo.VirtualComm);
                ws.Cells[indexRow, 8].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(matchWl2aBo.LineTotal);
                ws.Cells[indexRow, 9].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                ws.Cells[indexRow, 10].Value = CastAide.AsDecimal(matchWl2aBo.VirtualLineTotal);
                ws.Cells[indexRow, 10].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                ws.Cells[indexRow, 11].Value = CastAide.AsDecimal(matchWl2aBo.variance);
                ws.Cells[indexRow, 11].Style = gembox.GetStyleNumber(0, "", r, g, b, true);

                indexRow++;
            }

            #endregion Grand Total

            #endregion MatchWL

            for (int i = 0; i <= 11; i++)
            {
                if (i == 0)
                    ws.Columns[i].Width = 120 * 50;
                else
                    ws.Columns[i].Width = 120 * 30;
            }
            return excelFile;
        }

        #endregion ExportMatchWL
    }
}