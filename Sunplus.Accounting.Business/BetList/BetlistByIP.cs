using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Core.LoggingLib;
using System.Collections.Generic;
using Fanex.Tarus.LiveCasino.Entities.Betlist;
using DNAide.Utils;

/// <summary>
/// Summary description for BetlistByIP
/// </summary>
namespace Sunplus.Accounting.Business.BetList
{
    public class BetlistByIP : BetlistBase
    {
        public override string RenderHTML(System.Collections.Hashtable input)
        {
            return "";
        }

        public override string CreateHeader(System.Collections.Hashtable input)
        {
            DateTime fdt = CastAide.AsDateTime(HttpContext.Current.Request["fdate"]);
            string ip = CastAide.AsString(HttpContext.Current.Request["ip"]);
            string linkIP = "";
            if (ip != "")
                linkIP = "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", ip) + "\" target=\"_blank\">View IP Info</a>";

            StringBuilder html = new StringBuilder();
            string betListFilter = WebUtils.LoadTemplate("Betlist/BetListByIPFilter.htm");
            TemplateHandler.Assign(ref betListFilter, "{ip}", ip);
            TemplateHandler.Assign(ref betListFilter, "{Calendar}", Calendar.GenCalendarControl("fdate", "../", "fdate_trigger", fdt.ToShortDateString(), false, false, 0, 0, ""));
            TemplateHandler.Assign(ref betListFilter, "{LinkIP}", linkIP);
            html.Append(betListFilter);

            html.Append("<input type='hidden' name='bltype' value='3' />");

            if (HttpContext.Current.Request["nodate"] == "yes")
                return html.ToString();

            html.Append("<script>ViewBLIPReport()</script>");

            return html.ToString();

        }
        public string GenReportByIP(DateTime fdate, string ip)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                if (!AppSession.ViewIPVN && IpManagement.IsVNIP(ip))
                {
                    html.Append("<br>" + StringUtils.FormatErrorString(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "BLIPLimitVN")));
                    return html.ToString();
                }


                // Header
                string betListHeader = WebUtils.LoadTemplate("BetList\\BetListByIPHeader.htm");
                html.Append(betListHeader);

                // content
                int TotalSoccer = 0, TotalOtherSports = 0, TotalAll = 0;
                DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.GetBetList(fdate, ip);
                if (ds != null)
                {
                    string sRow = WebUtils.LoadTemplate("BetList\\BetListByIPRow.htm");
                    string newRow;

                    DataTable dtb = ds.ResultSet.Tables[0];
                    if (dtb.Rows.Count > 0)
                    {
                        DataRow[] rows = dtb.Select();
                        int i = 0;
                        foreach (DataRow row in rows)
                        {
                            newRow = sRow;
                            if (i % 2 == 0)
                                TemplateHandler.Assign(ref newRow, "{odd_even}", "o");
                            else
                                TemplateHandler.Assign(ref newRow, "{odd_even}", "e");
                            TemplateHandler.Assign(ref newRow, "{no}", CastAide.AsString(i + 1));
                            TemplateHandler.Assign(ref newRow, "{fdate}", fdate.ToShortDateString());
                            TemplateHandler.Assign(ref newRow, "{ip}", ip);
                            TemplateHandler.Assign(ref newRow, "{uname}", row["username"].ToString());
                            TemplateHandler.Assign(ref newRow, "{custid}", row["custid"].ToString());
                            if (row["soccerBet"].ToString() != "0")
                                TemplateHandler.Assign(ref newRow, "{soccerBet}", row["soccerBet"].ToString());
                            else
                                TemplateHandler.ReplaceBlock(ref newRow, "LinkSoccer", "<td>-</td>");

                            if (row["otherBet"].ToString() != "0")
                                TemplateHandler.Assign(ref newRow, "{otherBet}", row["otherBet"].ToString());
                            else
                                TemplateHandler.ReplaceBlock(ref newRow, "LinkOther", "<td>-</td>");

                            TemplateHandler.Assign(ref newRow, "{totalBet}", row["totalBet"].ToString());
                            html.Append(newRow);
                            TotalSoccer += CastAide.AsInt32(row["soccerBet"]);
                            TotalOtherSports += CastAide.AsInt32(row["otherBet"]);
                            TotalAll += CastAide.AsInt32(row["totalBet"]);
                            i++;
                        }

                        // Footer
                        string betListFooter = WebUtils.LoadTemplate("BetList\\BetListByIPFooter.htm");
                        TemplateHandler.Assign(ref betListFooter, "{TotalSoccer}", CastAide.AsString(TotalSoccer));
                        TemplateHandler.Assign(ref betListFooter, "{TotalOtherSports}", CastAide.AsString(TotalOtherSports));
                        TemplateHandler.Assign(ref betListFooter, "{TotalAll}", CastAide.AsString(TotalAll));
                        html.Append(betListFooter);
                    }
                    else
                    {
                        html.Append(AccCommon.GenNoInformation("5") + "</table>");
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return html.ToString();
        }
        public string GenReportByIPDetail(int custId, string ip, DateTime fdate, string requestType)
        {
            #region private variables
            StringBuilder html = new StringBuilder();
            int index = 0;
            DataSet ds = null;
            string rowTpl, eventTpl, eventTplLC;

            //closed by Kevin
            rowTpl = WebUtils.LoadTemplate("BetList\\BetListByIPDetailRow.htm");
            //rowTpl = Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("BetList\\BetListByIPScrollable.htm");
            eventTpl = WebUtils.LoadTemplate("BetList\\Event1.htm");
            eventTplLC = WebUtils.LoadTemplate("BetList\\EventLC.htm");
            var isSoccer = false;

            string rowHandler = TemplateHandler.CopyBlock(rowTpl, "row");
            #endregion
            TemplateHandler.RemoveBlock(ref rowTpl, "row");

            try
            {
                if (requestType.ToLowerInvariant() == "soccer")
                {
                    ds = AccountReport.GetBetListSoccer(fdate, custId, ip);
                    isSoccer = true;
                }
                else // (is "other")
                    ds = AccountReport.GetBetListOtherSports(fdate, custId, ip);


                //string betListHeader = Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("BetList\\BetListByIPDetailHeader.htm");
                //html.Append(betListHeader);

                if (ds != null)
                {
                    decimal total = 0;
                    int i = 0;
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var ipVNList = IpManagement.IPVNList;
                        _odd_even = "odd";
                        DataRow[] rows = ds.Tables[0].Select();
                        foreach (DataRow row in rows)
                        {
                            i++;
                            index++;
                            _bettype = Convert.IsDBNull(row["bettype"]) ? 0 : Convert.ToInt32(row["bettype"]);
                            if ((WebSettings.disabledHR) && IsHRBettype(_bettype))
                            {
                                index--;
                                continue;
                            }
                            else
                            {
                                if ((WebSettings.disabledFI) && (IsFIBettype(_bettype)))
                                {
                                    index--;
                                    continue;
                                }
                            }

                            #region get values
                           _sportypeName = CacheRef.GetSportName(row["sporttype"]);
                            _leagueName = CacheRef.GetLeagueName(row["leagueid"]);
                            _OddsTypeName = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(_bettype, CastAide.AsInt32(row["oddstype"])));

                            _transId = Convert.IsDBNull(row["transid"]) ? "0" : row["transid"].ToString();
                            _refno = Convert.IsDBNull(row["refno"]) ? "0" : row["refno"].ToString();
                            _matchCode = row["matchcode"] + "";
                            _hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : Convert.ToDecimal(row["hdp1"]);
                            _hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : Convert.ToDecimal(row["hdp2"]);
                            //_odds = Convert.ToDecimal(row["odds"]);
                            _odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(row["odds"]), _bettype, CastAide.AsInt32(row["oddstype"]));
                            _modds = CastAide.AsDecimal(row["modds"]);
                            _betId = DataUtils.IsExistKey(row, "betid") ? CastAide.AsString(row["betid"]) : string.Empty;
                            if (_bettype >= 1301 && _bettype <= 1334)
                            {
                                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype, _betId.Substring(1));
                            }
                            else
                            {
                                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype);
                            }
                           
                            if (IsHRBettype(_bettype))
                            {
                                _mOdds = Convert.IsDBNull(row["modds"]) ? 0 : Convert.ToDecimal(row["modds"]);
                                _finalodds = Convert.IsDBNull(row["finalodds"]) ? 0 : Convert.ToDecimal(row["finalodds"]);
                                _finalodds2 = Convert.IsDBNull(row["finalodds2"]) ? 0 : Convert.ToDecimal(row["finalodds2"]);
                            }
                            _com = Convert.IsDBNull(row["commission"]) ? 0 : Convert.ToDecimal(row["commission"]);
                            _totalcom += _com;
                            _status = row["status"] + "";
                            _home = CacheRef.GetTeamName(row["homeid"]) + (CastAide.AsBoolean(row["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
                            _away = CacheRef.GetTeamName(row["awayid"]);

                            _stake = Convert.IsDBNull(row["stake"]) ? 0 : Convert.ToDecimal(row["stake"]);
                            _actual_stake = Convert.IsDBNull(row["stake"]) ? 0 : CastAide.AsDecimal(row["actual_stake"]);
                            _transdate = Convert.ToDateTime(row["transdate"]);
                            //_betteam = row["betteam"] + "";
                            _betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(row["betteam"].ToString(), _bettype, "e").Trim();
                            _liveStyle = Convert.IsDBNull(row["liveindicator"]) ? "" : (Convert.ToBoolean(row["liveindicator"]) ? "event-live" : "");
                            _finalHomeScore = row["finalhomescore"] + "";
                            _finalAwayScore = row["finalawayscore"] + "";
                            _liveHomeScore = row["livehomescore"] + "";
                            _liveAwayScore = row["liveawayscore"] + "";
                            //placing = CastAide.AsInt32(row["placing"], 0);
                            _showtime = CastAide.AsString(row["showtime"]);
                            var _listBetlistEvt = new List<BetlistEvent>();
                            DataTable dtEvent = ds.Tables[1];
                            //check refno have not exist in event datatable
                            if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103 || _bettype == 1104)
                            {
                                if (dtEvent != null && dtEvent.Rows.Count > 0)
                                {
                                    
                                    var query = from blEvent in dtEvent.AsEnumerable()
                                                where blEvent.Field<string>("refno") == _refno
                                                select blEvent;

                                    DataTable dtb3 = query.CopyToDataTable();

                                    foreach (DataRow rowEvt in query)
                                    {
                                        if (_bettype == 1101 || _bettype == 1103)
                                        {
                                            _listBetlistEvt.Add(new BetlistEvent()
                                            {
                                                GameType = CacheRef.GetLiveCasinoResult(CastAide.AsInt32(rowEvt["betteam"])),
                                                Stake = IBCUtils.FormatHandler.FormatNumber2(CastAide.AsDouble(rowEvt["stake"])),
                                                Odds = IBCUtils.FormatHandler.FormatNumber2(CastAide.AsDouble(rowEvt["odds"]))
                                            });
                                        }
                                        else if (_bettype == 1102 || _bettype == 1104)
                                        {
                                            _listBetlistEvt.Add(new BetlistEvent()
                                            {
                                                GameType = CacheRef.GetRouletteBetItem(CastAide.AsInt32(rowEvt["betteam"])),
                                                Stake = IBCUtils.FormatHandler.FormatNumber2(CastAide.AsDouble(rowEvt["stake"])),
                                                Odds = IBCUtils.FormatHandler.FormatNumber2(CastAide.AsDouble(rowEvt["odds"]))
                                            });
                                        }

                                    }


                                }
                            }
                            string winlostStr = string.Empty;
                            decimal winlost = IBCUtils.FormatHandler.GetDecimal(row["winlost"]);
                            #endregion

                            if (string.Compare(_status, "running", true) != 0 && string.Compare(_status, "waiting", true) != 0 && string.Compare(_status, "rejected", true) != 0)
                            {
                                //if (_odds >= 0)
                                //{
                                //    winlost -= _stake;
                                //}
                                //else
                                //{
                                //    winlost -= Math.Abs(_stake * _odds);
                                //}
                                if (!string.IsNullOrEmpty(_status))
                                {
                                    total += winlost;
                                    winlostStr = IBCUtils.FormatHandler.FormatNumber20(winlost);
                                }
                                else if (!(_status.Equals("running") || string.IsNullOrEmpty(_status)))
                                {
                                    total += winlost;
                                    winlostStr = IBCUtils.FormatHandler.FormatNumber20(winlost);
                                }
                            }

                            //closed by Kevin
                            //string rowHtml = (string)rowTpl.Clone();
                            string rowHtml = (string)rowHandler.Clone();

                            //no
                            TemplateHandler.Assign(ref rowHtml, "{no}", i.ToString());
                            //username
                            TemplateHandler.Assign(ref rowHtml, "{uname}", row["username"].ToString());
                            //transtime
                            if (_bettype == 71 || _bettype == 72 || _bettype == 113 || _bettype == 114 || _bettype == 112)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "Ref No: <b>" + _matchCode + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "Ref No: <b>" + _transId.ToString() + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }
                            //event
                            #region event
                            //phuong.nguyen, add case for HR (31, 32, 33)
                            if (IsHRBettype(_bettype))
                            {

                                _home = row["matchcode"] + " - " + CacheRef.GetHorseTeamName(row["awayid"]);
                                _race = "Race " + row["race"].ToString();
                            }
                            string eventItem = (string)eventTpl.Clone();

                          
                            if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103 || _bettype == 1104)
                            {
                                eventItem = (string)eventTplLC.Clone();
                                GenEventLiveCasino(ref eventItem, _listBetlistEvt, 1);
                                TemplateHandler.Assign(ref rowHtml, "{Event}", eventItem);
                            }
                            else
                                GenEvent(ref rowHtml, row, eventItem, 1);

                            #endregion

                            //Odds
                            #region odds
                           // _odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(_odds, _bettype, CastAide.AsInt32(row["OddsType"]));
                            if (_bettype == 71 || _bettype == 72 || _bettype == 113 || _bettype == 114 || _bettype == 112)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Odds}", "");
                                TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "");
                            }
                            else if (_bettype == 29)
                            {
                                if (_status.ToLower().Contains("running"))
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{Odds}", "-");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(_modds) + "<div class='b bl-leaguename'>" + _OddsTypeName + "</div>");
                                }
                            }
                            else if (_bettype == 89)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "style='width:80px'");
                                TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(_odds));
                            }
                            else if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Odds}", "-");
                            }
                            else
                            {
                                #region HR
                                if (IsHRBettype(_bettype))
                                {
                                    if (IsHRWinPlaceBettype(_bettype)) // is 33, 43
                                    {
                                        if (!_status.Equals(Status.WIN) && !_status.Equals(Status.LOSE))
                                        {
                                            if (_bettype == 33)
                                            {
                                                TemplateHandler.Assign(ref rowHtml, "{Odds}", _NoShowOdds);
                                                TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "");
                                            }
                                            else // is 43
                                            {
                                                TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "style='width:95px'");
                                                string HTMLwin = "<div><span class='b'>{lblWinOddsSet}: </span><span>{winOddsSet}</span></div>";
                                                string HTMLplace = "<div><span class='b'>{lblPlaceOddsSet}: </span><span>{placeOddsSet}</span></div>";
                                                TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
                                                TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
                                                if (IsRunningWaiting(_status.ToLower()))
                                                {
                                                    TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", IBCUtils.FormatHandler.FormatNumber20(_finalodds));
                                                    TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", IBCUtils.FormatHandler.FormatNumber20(_finalodds2));
                                                }
                                                else // Reject, Void, Refund..
                                                {
                                                    TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", "0");
                                                    TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", "0");
                                                }
                                                TemplateHandler.Assign(ref rowHtml, "{Odds}", HTMLwin + HTMLplace);
                                            }
                                        }
                                        else // Settled : is WIN || LOSE
                                        {
                                            TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "style='width:95px'");
                                            string windOdds = "";
                                            string placeOdds = "";
                                            switch (_liveHomeScore)
                                            {
                                                case "2":
                                                case "0":
                                                    windOdds = IBCUtils.FormatHandler.FormatNumber20(_hdp1);
                                                    break;
                                                case "4":
                                                    if (_bettype == 33)
                                                        windOdds = "0.00";
                                                    else
                                                        windOdds = IBCUtils.FormatHandler.FormatNumber20(_hdp1);
                                                    break;
                                            }
                                            switch (_liveAwayScore)
                                            {
                                                case "2":
                                                    placeOdds = IBCUtils.FormatHandler.FormatNumber20(_hdp2);
                                                    break;
                                                case "0":
                                                case "4":
                                                    if (_bettype == 33)
                                                        placeOdds = "0.00";
                                                    else
                                                        placeOdds = IBCUtils.FormatHandler.FormatNumber20(_hdp2);
                                                    break;
                                            }
                                            if (_bettype == 33)
                                            {
                                                string HTMLwin = "<div><span class='b'>{lblWinOddsSet}: </span><span>{winOddsSet}</span></div>";
                                                string HTMLplace = "<div><span class='b'>{lblPlaceOddsSet}: </span><span>{placeOddsSet}</span></div>";

                                                TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
                                                TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
                                                TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", windOdds);
                                                TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", placeOdds);
                                                TemplateHandler.Assign(ref rowHtml, "{Odds}", HTMLwin + HTMLplace);
                                            }
                                            else //is 43
                                            {
                                                string HTMLwin = "<!--winOdds43--><div><span class='b'>{lblWinOdds}: </span><span>{winOdds}</span></div><!--/winOdds43--><div><span class='b'>{lblWinOddsSet}: </span><span>{winOddsSet}</span></div>";
                                                string HTMLplace = "<!--PlaceOdds43--><div><span class='b'>{lblPlaceOdds}: </span><span>{placeOdds}</span></div><!--/PlaceOdds43--><div><span class='b'>{lblPlaceOddsSet}: </span><span>{placeOddsSet}</span></div>";

                                                // WIN Odds: liveHomeScore=2: WON
                                                TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", (_liveHomeScore != "2") ? "0.00" : windOdds);
                                                if (_finalodds != CastAide.AsDecimal(windOdds) && _liveHomeScore == "2")
                                                {
                                                    TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WSettle"));
                                                    TemplateHandler.Assign(ref HTMLwin, "{lblWinOdds}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "wBetAt"));
                                                    TemplateHandler.Assign(ref HTMLwin, "{winOdds}", IBCUtils.FormatHandler.FormatNumber20(_finalodds));
                                                }
                                                else
                                                {
                                                    TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
                                                    TemplateHandler.RemoveBlock(ref HTMLwin, "winOdds43");
                                                }

                                                // PLACE Odds: liveAwayScore=2: WON                                     
                                                TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", (_liveAwayScore != "2") ? "0.00" : placeOdds);
                                                if (_finalodds2 != CastAide.AsDecimal(placeOdds) && _liveAwayScore == "2")
                                                {
                                                    TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "pSettle"));
                                                    TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOdds}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "pBetAt"));
                                                    TemplateHandler.Assign(ref HTMLplace, "{placeOdds}", IBCUtils.FormatHandler.FormatNumber20(_finalodds2));
                                                }
                                                else
                                                {
                                                    TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
                                                    TemplateHandler.RemoveBlock(ref HTMLplace, "PlaceOdds43");
                                                }
                                                TemplateHandler.Assign(ref rowHtml, "{Odds}", HTMLwin + HTMLplace);
                                            }
                                        }
                                    }
                                    else // 31, 32, 41, 42
                                    {
                                        if (_bettype < 33 && !_status.Equals(Status.WIN))
                                        {
                                            TemplateHandler.Assign(ref rowHtml, "{Odds}", _NoShowOdds);
                                            TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "");
                                        }
                                        else
                                        {
                                            // 31, 32, 41, 42
                                            if (IsVoidRejectRefundLose(_status.ToUpper()))
                                            {
                                                TemplateHandler.Assign(ref rowHtml, "{Odds}", _NoShowOdds);
                                                TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "");
                                            }
                                            else
                                            {
                                                if (_bettype == 41 || _bettype == 42)
                                                {
                                                    TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "style='width:95px'");
                                                    if (IsRunningWaiting(_status.ToLower()))
                                                        TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber20(_mOdds));
                                                    else // Settled: WON, LOSE
                                                    {
                                                        if (_mOdds != _odds)
                                                        {
                                                            _strOdds = "<b>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "betAt") + ": </b>" + IBCUtils.FormatHandler.FormatNumber20(_mOdds)
                                                                    + "<br/><b>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "settle") + ": </b>" + IBCUtils.FormatHandler.FormatNumber20(_odds);
                                                            TemplateHandler.Assign(ref rowHtml, "{Odds}", _strOdds);
                                                        }
                                                        else
                                                            TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber20(_odds));
                                                    }
                                                }
                                                else // 31, 32
                                                    TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber20(_odds) + "<div class='b bl-leaguename'>" + _OddsTypeName + "</div>");
                                            }
                                        }
                                    }
                                }
                                #endregion
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "style='width:80px'");
                                    TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(_odds) + "<div class='b bl-leaguename'>" + _OddsTypeName + "</div>");
                                }
                            }
                            #endregion

                            #region stake
                            string strStake = IBCUtils.FormatHandler.FormatNumber20(_stake);
                            string strActualStake = IBCUtils.FormatHandler.FormatNumber20(_actual_stake);
                            if (_bettype == 29)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Stake}", strActualStake + (Math.Round(_stake, 0) == 0 ? string.Empty : "<br/>(" + strStake + ")"));
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Stake}", strStake);
                            }
                            #endregion

                            //Exchange Rate
                            if (!isSoccer)
                            {
                                var isShowOtExRate = CastAide.AsBoolean(row["isShowOTExRate"], false);
                                var virtualRate = CastAide.AsDouble(row["virtualrate"], 0.0);

                                if (!isShowOtExRate)
                                {
                                    TemplateHandler.RemoveBlock(ref rowHtml, "Exchangerate");
                                }
                                else
                                    TemplateHandler.Assign(ref rowHtml, "{ExchangeRate}",
                                                           isShowOtExRate ? virtualRate.ToString() : string.Empty);
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref rowHtml, "Exchangerate");
                            }


                            //winlost
                            if (_status.Equals("running") || string.IsNullOrEmpty(_status) || _status.Equals("rejected") || _status.Equals("waiting"))
                            {
                                winlostStr = string.Empty;
                                _placing = 0;
                            }
                            winlostStr = CastAide.AsDecimal(winlostStr) < 0 ? "<font color='red'>" + winlostStr + "</font>" : winlostStr;
                            TemplateHandler.Assign(ref rowHtml, "{WinLost}", (!string.IsNullOrEmpty(winlostStr)) ? winlostStr + "<br>" + IBCUtils.FormatHandler.FormatNumber20(_com) : string.Empty);

                            //status
                            #region status
                            string resultLink = string.Empty;
                            var statusFormat = (_status.ToLower() == "reject" || _status.ToLower() == "refund" || _status.ToLower() == "void") ? "<span style=\"color:red;\">" + _status.ToUpper() + "</span>" : _status;
                            if (CastAide.AsInt32(row["sporttype"]) == 161)
                            {
                                _status = statusFormat;
                                TemplateHandler.Assign(ref rowHtml, "{Status}", _status);
                                TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                            }
                            else
                            {
                                if (IsHRBettype(_bettype) || IsFIBettype(_bettype))
                                {
                                    if (IsHRWinPlaceBettype(_bettype) && (_status.Equals(Status.WIN) || _status.Equals(Status.LOSE)))
                                    {
                                        string htmlWinStatus = "<div><span class='b'>{lblWin}: </span><span>{winStatus}</span></div>";
                                        string htmlPlaceStatus = "<div><span class='b'>{lblPlace}: </span><span>{placeStatus}</span></div>";
                                        #region Hot fix, support to display refund status with bettype 33
                                        string placeStatus = "";
                                        switch (_liveAwayScore)
                                        {
                                            case "2":
                                                placeStatus = Status.WIN;
                                                break;
                                            case "4":
                                                placeStatus = Status.LOSE;
                                                break;
                                            case "0":
                                                placeStatus = Status.Refund;
                                                break;
                                        }

                                        TemplateHandler.Assign(ref htmlWinStatus, "{lblWin}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
                                        TemplateHandler.Assign(ref htmlPlaceStatus, "{lblPlace}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
                                        TemplateHandler.Assign(ref htmlWinStatus, "{winStatus}", _liveHomeScore.Equals("2") ? Status.WIN : Status.LOSE);
                                        TemplateHandler.Assign(ref htmlPlaceStatus, "{placeStatus}", placeStatus);
                                        TemplateHandler.Assign(ref rowHtml, "{Status}", htmlWinStatus + htmlPlaceStatus);
                                        #endregion

                                    }
                                    else
                                    {
                                        _status = statusFormat;
                                        TemplateHandler.Assign(ref rowHtml, "{Status}", _status);
                                    }

                                    if (IsHRBettype(_bettype) && (_status.Equals(Status.WIN) || _status.Equals(Status.LOSE)))
                                    {
                                        resultLink = "<a style='cursor:pointer' onclick=\"hrRaceResultWd=dhtmlwindow.open('ajaxbox', 'ajax', '{raceresultpage}', '{wdtitle}', 'width={wdwidth}px,height={wdheight}px,left={wdleft}px,top={wdtop}px,resize=0,scrolling=1'); return false\">{lblResult}</a>";
                                        string gHRBettype = "0";
                                        if (IsHRFixOdds(_bettype))
                                        {
                                            TemplateHandler.Assign(ref resultLink, "{wdheight}", "250");
                                            gHRBettype = "1";
                                        }
                                        else
                                            TemplateHandler.Assign(ref resultLink, "{wdheight}", "200");
                                        string hrefRaceResult = "../Report/HRRaceResult.aspx?leagueid=" + row["leagueid"] + "&raceno=" + row["race"] + "&eventdate=" + CastAide.AsDateTime(row["eventdate"], DateTime.Now).ToShortDateString() + "&HRBettype=" + gHRBettype;
                                        TemplateHandler.Assign(ref resultLink, "{raceresultpage}", hrefRaceResult);
                                        TemplateHandler.Assign(ref resultLink, "{wdtitle}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRaceResult"));
                                        TemplateHandler.Assign(ref resultLink, "{wdwidth}", "300");
                                        TemplateHandler.Assign(ref resultLink, "{wdleft}", "300");
                                        TemplateHandler.Assign(ref resultLink, "{wdtop}", "100");
                                        TemplateHandler.Assign(ref resultLink, "{lblResult}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblResult"));
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                    }
                                    else
                                    {
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                    }

                                }
                                else if (_bettype == 29)
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{Status}", _status);
                                    TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                }
                                else if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103)
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{Status}", _status + "@" + _finalHomeScore + "-" + _finalAwayScore);
                                    TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                }
                                else
                                {
                                    _status = statusFormat;
                                    if (_bettype == 71 || _bettype == 72 || _bettype == 113 || _bettype == 114 || _bettype == 112 || _bettype == 9)
                                    {
                                        TemplateHandler.Assign(ref rowHtml, "{Status}", _status);
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                    }
                                       

                                    else if (_bettype >= 1301 && _bettype <= 1334)
                                    {
                                        TemplateHandler.Assign(ref rowHtml, "{Status}", _status);
                                        if (_status.Equals("running") || string.IsNullOrEmpty(_status) || _status.Equals("rejected") || _status.Equals("waiting"))
                                        {
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                        }
                                        else {
                                            resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewResult('" + CastAide.AsString(row["matchid"]) + "'," + _bettype + ", '" + _betId + "' )\">{lblResult}</div> ";
                                            TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                        }
                                    }
                                    else
                                    {
                                        if (CastAide.AsInt32(row["sporttype"]) >= 33 && CastAide.AsInt32(row["sporttype"]) <= 42)
                                            TemplateHandler.Assign(ref rowHtml, "{Status}", _status + "@" + _finalHomeScore + "-" + _finalAwayScore);
                                        else
                                            TemplateHandler.Assign(ref rowHtml, "{Status}", _status + "@" + _finalHomeScore + "-" + _finalAwayScore + "<br />" + "[HT:" + row["hthomescore"] + "-" + row["htawayscore"] + "]");
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                    }

                                    
                                }
                            }
                            #endregion

                            //operator
                            TemplateHandler.Assign(ref rowHtml, "{Operator}", CastAide.AsString(row["creator"]));
                            //ip
                            //TemplateHandler.Assign(ref rowHtml, "{ip}", IPUtils.ReplaceVNIP(row["ip"].ToString()));
                            if (AppSession.ViewIPVN)
                                TemplateHandler.Assign(ref rowHtml, "{ip}", CastAide.AsString(row["ip"]));
                            else
                                TemplateHandler.Assign(ref rowHtml, "{ip}", IpManagement.ReplaceVNIP(CastAide.AsString(row["ip"]), ipVNList));


                            if (string.IsNullOrEmpty(_liveStyle)) { _odd_even = (_odd_even == "o") ? "e" : "o"; TemplateHandler.Assign(ref rowHtml, "{odd_even}", _odd_even); }
                            else TemplateHandler.Assign(ref rowHtml, "{live}", _liveStyle);


                            html.Append(rowHtml);
                        }

                        if (index == 0 && WebSettings.disabledHR)
                        {
                            html.Append(AccCommon.GenNoInformation("9"));
                        }
                        else if (index == 0 && WebSettings.disabledFI)
                        {
                            html.Append(AccCommon.GenNoInformation("9"));
                        }
                        else
                        {
                            //closed by Kevin

                            var winlost = (total >= 0) ? "win" : "loss";
                            var tt = total + _totalcom;
                            var clwinlost = (total >= 0) ? "s-datavalue" : "s-datavalue-minus";
                            var cltotal = (tt >= 0) ? "s-total" : "s-total-minus";

                            TemplateHandler.Assign(ref rowTpl, "{win}", winlost);
                            TemplateHandler.Assign(ref rowTpl, "{totalwinlost}", IBCUtils.FormatHandler.FormatNumber20(total));
                            TemplateHandler.Assign(ref rowTpl, "{totalcom}", IBCUtils.FormatHandler.FormatNumber20(_totalcom));
                            TemplateHandler.Assign(ref rowTpl, "{total}", IBCUtils.FormatHandler.FormatNumber20(tt));
                            TemplateHandler.Assign(ref rowTpl, "{colorwinlost}", clwinlost);
                            TemplateHandler.Assign(ref rowTpl, "{colortotal}", cltotal);
                        }
                    }
                    else
                    {
                        html.Append(AccCommon.GenNoInformation("9"));
                        TemplateHandler.Assign(ref rowTpl, "{win}", "");
                        TemplateHandler.Assign(ref rowTpl, "{totalwinlost}", IBCUtils.FormatHandler.FormatNumber20(0));
                        TemplateHandler.Assign(ref rowTpl, "{totalcom}", IBCUtils.FormatHandler.FormatNumber20(0));
                        TemplateHandler.Assign(ref rowTpl, "{total}", IBCUtils.FormatHandler.FormatNumber20(0));
                    }
                }
                html.Append("</table>");

            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            //closed by Kevin
            //return html.ToString();

            TemplateHandler.Assign(ref rowTpl, "{content}", html.ToString());
            return rowTpl;
        }
    }
}
