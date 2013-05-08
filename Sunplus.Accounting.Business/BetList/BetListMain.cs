using System;
using System.Data;
using System.Web;

using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using IBCUtils;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Core.LoggingLib;
//using Fanex.Tarus.LiveCasino.Entities.Betlist;
using System.Collections.Generic;
using Fanex.Tarus.LiveCasino.Entities.Betlist;

/// <summary>
/// Summary description for BetListMain
/// </summary>
namespace Sunplus.Accounting.Business.BetList
{
    public class BetListMain : BetlistBase
    {
        public override string RenderHTML(System.Collections.Hashtable input)
        {
            return "";
        }
        public override string CreateHeader(System.Collections.Hashtable input)
        {
            string fdate = HttpContext.Current.Request["fdate"];
            string tdate = HttpContext.Current.Request["tdate"];
            DateTime fdt, tdt;
            if (!Validator.ValidDateMMDDYYYYNotNull(fdate, out fdt).IsSuccess())
                if (HttpContext.Current.Session["fdate"] != null)
                    fdt = (DateTime)HttpContext.Current.Session["fdate"];
                else
                    fdt = DateTime.Now.AddDays(-1);
            if (!Validator.ValidDateMMDDYYYYNotNull(tdate, out tdt).IsSuccess())
                if (HttpContext.Current.Session["tdate"] != null)
                    tdt = (DateTime)HttpContext.Current.Session["tdate"];
                else
                    tdt = DateTime.Now;

            fdt = DNAide.Core.CastAide.AsDateTime(fdt.ToShortDateString());

            HttpContext.Current.Session["fdate"] = fdt;
            HttpContext.Current.Session["tdate"] = tdt;

            StringBuilder betlistHtml = new StringBuilder();
            string bettypeOptions = GenAllBettype(fdt, tdt);

            string betListFilter = WebUtils.LoadTemplate("Betlist/BetListMainFilter.htm");
            TemplateHandler.Assign(ref betListFilter, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdate_trigger", fdt.ToShortDateString(), false, false, 0, 0, "ChangeDateTime()"));
            TemplateHandler.Assign(ref betListFilter, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../", "tdate_trigger", tdt.ToShortDateString(), false, false, 0, 0, "ChangeDateTime()"));
            TemplateHandler.Assign(ref betListFilter, "{BettypeOptions}", CastAide.AsString(bettypeOptions));

            if (CastAide.AsString(bettypeOptions).Trim() != "")
                TemplateHandler.Assign(ref betListFilter, "{btDisplay}", "");
            else
                TemplateHandler.Assign(ref betListFilter, "{btDisplay}", "style='display:none'");

            betlistHtml.Append(betListFilter);

            betlistHtml.Append("<input type='hidden' name='bltype' value='1' />");
            betlistHtml.Append("<script>ViewMainReport()</script>");

            //ViewMainReportByBettype

            return betlistHtml.ToString();
        }

        public string GenAllBettype(DateTime fromdate, DateTime todate)
        {
            int custid = CastAide.AsInt32(HttpContext.Current.Session["CustId"]);
            StringBuilder html = new StringBuilder();

            fromdate = DNAide.Core.CastAide.AsDateTime(fromdate.ToShortDateString()); 
            var dbr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.Acc_BetList_SelBettype(fromdate, todate, custid);
            DataTable dataTable = null;
            if (dbr != null && dbr.Tables.Count > 0)
                dataTable = dbr.Tables[0];

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] Rows = dataTable.Select();
                foreach (DataRow row in Rows)
                {
                    var typeId = CastAide.AsInt32(row["typeid"]);
                    string option = "<option value='{optionvalue}'>{optiondisplay}</option>";
                    TemplateHandler.Assign(ref option, "{optionvalue}", typeId.ToString());

                    var typename = CastAide.AsString(row["typenamee"]);
                    if(1201<= typeId && typeId <= 1299) {
                        typename += " (Virtual Sports)";
                    }
                    TemplateHandler.Assign(ref option, "{optiondisplay}", string.Format("{0} {1}", typename,
                        CastAide.AsInt32(row["typeid"]) == 85 || CastAide.AsInt32(row["typeid"]) == 86 ? "(" +
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNumberGame") + ")" : ""));
                    html.AppendLine(option);
                }
                return html.ToString();
            }
            else
            {
                return "";
            }

        }

        public string GenMainReport(DateTime fdate, DateTime tdate, int inputBettype)
        {
            DataSet rs = null;
            int custId = 0;
            DataSet ds = new DataSet();

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtEvent = new DataTable();
            DataTable dtEvent1 = new DataTable();
            // DataSet dsEvent = new DataSet();
            try
            {
                if (HttpContext.Current.Session["CustId"] != null)
                {
                    custId = CastAide.AsInt32(HttpContext.Current.Session["CustId"]);

                    if ((tdate) >= Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.BalanceUpDay)
                    {
                        rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.BetsSelBetList(fdate, tdate, custId, inputBettype);
                        if (rs.Tables.Count > 0)
                        {
                            dt1 = rs.Tables[0];
                            dtEvent1 = rs.Tables[1];
                        }
                    }

                    if ((fdate) < Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.BalanceUpDay)
                    {
                        rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.BetsSelBetList_Old(fdate, tdate, custId, inputBettype);
                        if (rs.Tables.Count > 0)
                        {
                            dt = rs.Tables[0];
                            dtEvent = rs.Tables[1];
                        }
                    }

                    if (dt.Rows.Count == 0)
                        dt = dt1;
                    else
                    {
                        foreach (DataRow dr in dt1.Rows)
                            dt.ImportRow(dr);
                    }

                    if (dtEvent.Rows.Count == 0)
                        dtEvent = dtEvent1;
                    else
                    {
                        foreach (DataRow drEvent in dtEvent1.Rows)
                            dtEvent.ImportRow(drEvent);
                    }


                    ds.Tables.Add(dt.Copy());
                }
                return GenMainReport(dt, dtEvent);
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }
        }
        public string GenMainReport(string transID)
        {
            DataSet ds = new DataSet();
            int custId;
            try
            {
                if (HttpContext.Current.Session["CustId"] != null)
                    {
                    custId = CastAide.AsInt32(HttpContext.Current.Session["CustId"]);
                    ds = DataAccess.IBCDLL.Accounting.Betlist.BetsSelBetList_ByTransID(custId, transID);
                    return GenMainReport(ds.Tables[0], ds.Tables[1]);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }
            return "";
        }


        public string GenMainReport(string transID, int custId)
        {
            DataSet ds = new DataSet();
            try
            {

                ds = DataAccess.IBCDLL.Accounting.Betlist.BetsSelBetList_ByTransID(custId, transID);
                return GenMainReport(ds.Tables[0], ds.Tables[1], custId);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }

        }

        public string GenMainReport(DataTable ds, DataTable dtEvent, int custId)
        {
            int  index = 0;
            int pageNum = 0;
            int page = 1;
            //DbResult ds = null;
            string rowTpl, eventTpl, eventTplLC;
            string paging = string.Empty;
            StringBuilder html = new StringBuilder();

            rowTpl = WebUtils.LoadTemplate("BetList\\BetListMainRow.htm");
           
            eventTpl = WebUtils.LoadTemplate("BetList\\Event1.htm");
            eventTplLC = WebUtils.LoadTemplate("BetList\\EventLC.htm");

            if (AppSession.SYSTEM.ToLower() != "accounting")
            {
                TemplateHandler.RemoveBlock(ref rowTpl, "delete");
            }
            TemplateHandler.RemoveBlock(ref rowTpl, "HeaderDelete");
            TemplateHandler.RemoveBlock(ref rowTpl, "deleterow");
            TemplateHandler.RemoveBlock(ref rowTpl, "icondelete");
            string rows = TemplateHandler.CopyBlock(rowTpl, "row");
            TemplateHandler.RemoveBlock(ref rowTpl, "row");
            try
            {
               

                if (ds != null && ds.Rows.Count > 0)
                {
                    int startIndex = 0;
                    int endIndex = 0;
                    int pageSize = 30;
                    int.TryParse(HttpContext.Current.Request["page"], out page);
                    pageNum = ds.Rows.Count / pageSize;
                    int rowLastPage = ds.Rows.Count % pageSize;
                    if (rowLastPage > 0)
                        pageNum++;
                    if (page < 1 || page > pageNum)
                    {
                        page = pageNum;
                    }
                    startIndex = (page - 1) * pageSize;
                    if (pageNum != 0)
                        endIndex = startIndex + ((rowLastPage > 0 && page == pageNum) ? rowLastPage : pageSize);
                    decimal total = 0;
                    if (ds.Rows.Count > 0 && ds.Rows.Count > 0)
                    {
                        var ipVNList = IpManagement.IPVNList;
                        _odd_even = "o";
                        for (int i = startIndex; i < endIndex; ++i)
                        {

                            index++;
                            DataRow row = ds.Rows[i];

                            _bettype = Convert.IsDBNull(row["bettype"]) ? 0 : CastAide.AsInt32(row["bettype"]);
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

                            //_bettypeName = CacheRef.GetBettype(_bettype);

                            #region will be removed later
                            if (_bettype == 81 && string.IsNullOrEmpty(_bettypeName))
                                _bettypeName = "1st ball OU";
                            #endregion

                            _sportypeName = CacheRef.GetSportName(row["sporttype"]);
                            _leagueName = CacheRef.GetLeagueName(row["leagueid"]);
                            _OddsTypeName = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(_bettype, CastAide.AsInt32(row["oddstype"])));

                            _transId = Convert.IsDBNull(row["transid"]) ? "0" : row["transid"].ToString();
                            _refno = Convert.IsDBNull(row["refno"]) ? "0" : row["refno"].ToString();
                            _matchCode = row["matchcode"] + "";
                            _hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : CastAide.AsDecimal(row["hdp1"]);
                            _hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : CastAide.AsDecimal(row["hdp2"]);
                            //_odds = CastAide.AsDecimal(row["odds"]);
                            _odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(row["odds"]), _bettype, CastAide.AsInt32(row["oddstype"]));
                            _modds = CastAide.AsDecimal(row["modds"]);
                            if (IsHRBettype(_bettype))
                            {
                                _mOdds = Convert.IsDBNull(row["modds"]) ? 0 : CastAide.AsDecimal(row["modds"]);
                                _finalodds = Convert.IsDBNull(row["finalodds"]) ? 0 : CastAide.AsDecimal(row["finalodds"]);
                                _finalodds2 = Convert.IsDBNull(row["finalodds2"]) ? 0 : CastAide.AsDecimal(row["finalodds2"]);
                            }
                            _com = Convert.IsDBNull(row["commission"]) ? 0 : CastAide.AsDecimal(row["commission"]);
                            _totalcom += _com;
                            _status = row["status"] + "";
                            _home = CacheRef.GetTeamName(row["homeid"]) + (CastAide.AsBoolean(row["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
                            _away = CacheRef.GetTeamName(row["awayid"]);

                            _stake = Convert.IsDBNull(row["stake"]) ? 0 : CastAide.AsDecimal(row["stake"]);
                            _actual_stake = Convert.IsDBNull(row["stake"]) ? 0 : CastAide.AsDecimal(row["actual_stake"]);
                            _transdate = Convert.ToDateTime(row["transdate"]);
                            //_betteam = row["betteam"] + "";

                            _betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(row["betteam"].ToString(), _bettype, "e").Trim();
                            _liveStyle = Convert.IsDBNull(row["liveindicator"]) ? "" : (Convert.ToBoolean(row["liveindicator"]) ? "event-live" : "");
                            _liveindicator = Convert.ToInt16(row["liveindicator"]);
                            _finalHomeScore = row["finalhomescore"] + "";
                            _finalAwayScore = row["finalawayscore"] + "";
                            _liveHomeScore = row["livehomescore"] + "";
                            _liveAwayScore = row["liveawayscore"] + "";
                            _betId = CastAide.AsString(row["betid"]);
                            if (_bettype >= 1301 && _bettype <= 1334)
                            {
                                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype, _betId.Substring(1));
                            }
                            else
                            {
                                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype);
                            }
                           
                            _showtime = CastAide.AsString(row["showtime"]);
                            var _listBetlistEvt = new List<BetlistEvent>();
                            //check refno have not exist in event datatable
                            if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103)
                            {
                                if (dtEvent != null && dtEvent.Rows.Count > 0)
                                {
                                    var query = from blEvent in dtEvent.AsEnumerable()
                                                where blEvent.Field<string>("refno") == _refno
                                                select blEvent;
                                    
                                    DataTable dtb3 = query.CopyToDataTable();

                                    foreach (DataRow rowEvt in query)
                                    {
                                        _listBetlistEvt.Add(new BetlistEvent()
                                        {
                                            GameType = CacheRef.GetLiveCasinoResult(CastAide.AsInt32(Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(rowEvt["betteam"].ToString(), _bettype, "e"))),
                                            Stake = NumberFormat.DecFormat(CastAide.AsDouble(rowEvt["stake"]), 2),
                                            Odds = NumberFormat.DecFormat(CastAide.AsDouble(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(row["odds"]), _bettype, CastAide.AsInt32(row["oddstype"]))), 2)
                                        });

                                    }


                                }
                            }
                            string winlostStr = "&nbsp;";

                            decimal winlost = IBCUtils.FormatHandler.GetDecimal(row["winlost"]);
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

                                if (!(_status.Equals("running") || string.IsNullOrEmpty(_status)))
                                {
                                    total += winlost;
                                    winlostStr = IBCUtils.FormatHandler.FormatNumber20(winlost);
                                }
                            }
                            string rowHtml = (string)rows.Clone();

                            _spt = (Convert.IsDBNull(row["spositiontaking"]) ? 0 : CastAide.AsDecimal(row["spositiontaking"])) * 100;
                            _mpt = (Convert.IsDBNull(row["mpositiontaking"]) ? 0 : CastAide.AsDecimal(row["mpositiontaking"])) * 100;
                            _apt = (Convert.IsDBNull(row["apositiontaking"]) ? 0 : CastAide.AsDecimal(row["apositiontaking"])) * 100;

                            _swinlost = Convert.IsDBNull(row["swinlost"]) ? 0 : Convert.ToDecimal(row["swinlost"]);
                            _mwinlost = Convert.IsDBNull(row["mwinlost"]) ? 0 : Convert.ToDecimal(row["mwinlost"]);
                            _awinlost = Convert.IsDBNull(row["awinlost"]) ? 0 : Convert.ToDecimal(row["awinlost"]);

                            _scommRate = CastAide.AsDouble(row["sdiscount"], 0) * 100;
                            _mcommRate = CastAide.AsDouble(row["adiscount"], 0) * 100;
                            _acommRate = CastAide.AsDouble(row["discount"], 0) * 100;

                            _scomm = Convert.IsDBNull(row["scomm"]) ? 0 : Convert.ToDecimal(row["scomm"]);
                            _mcomm = Convert.IsDBNull(row["acomm"]) ? 0 : Convert.ToDecimal(row["acomm"]);
                            _acomm = Convert.IsDBNull(row["comm"]) ? 0 : Convert.ToDecimal(row["comm"]);

                            //No
                            TemplateHandler.Assign(ref rowHtml, "{no}", (i + 1).ToString());

                            //TransTime
                            if (_bettype == 71 || _bettype == 72 || _bettype == 73 || _bettype == 113 || _bettype == 114 || _bettype == 112)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "<b>Ref No:" + _matchCode + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }
                            else if (_bettype == 1000)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "<b>Ref No:" + _transId.ToString() + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "<b>Ref No:" + _transId.ToString() + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }

                            //Event
                            #region event
                            //phuong.nguyen, add case for HR (31, 32, 33)
                            if (IsHRBettype(_bettype))
                            {
                                _home = row["matchcode"] + " - " + CacheRef.GetHorseTeamName(row["awayid"]);
                                _race = "Race " + row["race"].ToString();
                            }

                            string eventItem = (string)eventTpl.Clone();
                            if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103)
                            {
                                eventItem = (string)eventTplLC.Clone();
                                GenEventLiveCasino(ref eventItem, _listBetlistEvt, 1);
                                TemplateHandler.Assign(ref rowHtml, "{Event}", eventItem);
                            }
                            else
                                GenEvent(ref rowHtml, row, eventItem, 1);
                            #endregion

                            //Odds
                            #region Odds
                            // _odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(_odds, _bettype, CastAide.AsInt32(row["oddstype"]));
                            if (_bettype == 9)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber40(_odds) + "<div class='b bl-leaguename'>" + _OddsTypeName + "</div>");
                                TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "style='width:80px'");
                            }
                            else if (_bettype == 71 || _bettype == 72 || _bettype == 73 || _bettype == 113 || _bettype == 114 || _bettype == 112)
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
                            else if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103 || _bettype == 1000)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Odds}", "-");
                            }
                            else
                            {
                                #region HR bettype
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

                            //OpenTote Exchange Rate
                            var isShowOtExRate = CastAide.AsBoolean(row["isShowOTExRate"], false);
                            var virtualRate = CastAide.AsDouble(row["virtualrate"], 0.0);

                            if (!isShowOtExRate)
                            {
                                TemplateHandler.RemoveBlock(ref rowHtml, "Exchangerate");
                            }
                            else
                                TemplateHandler.Assign(ref rowHtml, "{ExchangeRate}", isShowOtExRate ? virtualRate.ToString() : string.Empty);

                            #region  winlost
                            if (_status.Equals("running") || string.IsNullOrEmpty(_status) || _status.Equals("rejected") || _status.Equals("waiting"))
                            {
                                winlostStr = string.Empty;
                            }
                            winlostStr = CastAide.AsDecimal(winlostStr) < 0 ? "<font color='red'>" + winlostStr + "</font>" : winlostStr;
                            TemplateHandler.Assign(ref rowHtml, "{WinLost}", (!string.IsNullOrEmpty(winlostStr)) ? winlostStr + "<br>" + IBCUtils.FormatHandler.FormatNumber20(_com) : string.Empty);
                            #endregion

                            //status
                            #region status
                            string resultLink = string.Empty;
                            var statusFormat = (_status.ToLower() == "reject" || _status.ToLower() == "refund" || _status.ToLower() == "void") ? "<span style=\"color:red;\">" + _status.ToUpper() + "</span>" : _status;
                            #region HR
                            if (IsHRBettype(_bettype))
                            {
                                decimal spt2 = (Convert.IsDBNull(row["spositiontaking2"]) ? 0 : CastAide.AsDecimal(row["spositiontaking2"])) * 100;
                                decimal mpt2 = (Convert.IsDBNull(row["mpositiontaking2"]) ? 0 : CastAide.AsDecimal(row["mpositiontaking2"])) * 100;
                                decimal apt2 = (Convert.IsDBNull(row["apositiontaking2"]) ? 0 : CastAide.AsDecimal(row["apositiontaking2"])) * 100;

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

                                if (IsHRWinPlaceBettype(_bettype))
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperPT}", "Win: " + IBCUtils.FormatHandler.FormatNumber3(_spt) + "%<br><span style='color:#606060'>Place: " + IBCUtils.FormatHandler.FormatNumber3(spt2) + "%</span>");
                                    TemplateHandler.Assign(ref rowHtml, "{MasterPT}", "Win: " + IBCUtils.FormatHandler.FormatNumber3(_mpt) + "%<br><span style='color:#606060'>Place: " + IBCUtils.FormatHandler.FormatNumber3(mpt2) + "%</span>");
                                    TemplateHandler.Assign(ref rowHtml, "{AgentPT}", "Win: " + IBCUtils.FormatHandler.FormatNumber3(_apt) + "%<br><span style='color:#606060'>Place: " + IBCUtils.FormatHandler.FormatNumber3(apt2) + "%</span>");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperPT}", IBCUtils.FormatHandler.FormatNumber3(_spt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{MasterPT}", IBCUtils.FormatHandler.FormatNumber3(_mpt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{AgentPT}", IBCUtils.FormatHandler.FormatNumber3(_apt) + "%");
                                }



                                TemplateHandler.Assign(ref rowHtml, "{SuperCommRate}", IBCUtils.FormatHandler.FormatNumber3(_scommRate));
                                TemplateHandler.Assign(ref rowHtml, "{MasterCommRate}", IBCUtils.FormatHandler.FormatNumber3(_mcommRate));
                                TemplateHandler.Assign(ref rowHtml, "{AgentCommRate}", IBCUtils.FormatHandler.FormatNumber3(_acommRate));

                                if (_status.Equals(Status.RUNNING))
                                {
                                    TemplateHandler.RemoveBlock(ref rowHtml, "swinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "mwinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "awinlost");

                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentComm");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{swinlost}", IBCUtils.FormatHandler.FormatNumber20(_swinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{mwinlost}", IBCUtils.FormatHandler.FormatNumber20(_mwinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{awinlost}", IBCUtils.FormatHandler.FormatNumber20(_awinlost));

                                    TemplateHandler.Assign(ref rowHtml, "{SuperComm}", IBCUtils.FormatHandler.FormatNumber20(_scomm));
                                    TemplateHandler.Assign(ref rowHtml, "{MasterComm}", IBCUtils.FormatHandler.FormatNumber20(_mcomm));
                                    TemplateHandler.Assign(ref rowHtml, "{AgentComm}", IBCUtils.FormatHandler.FormatNumber20(_acomm));
                                }


                                //show Result link for HR
                                if ((_status.Equals(Status.WIN) || _status.Equals(Status.LOSE)))
                                {
                                    resultLink = "<a style='cursor:pointer' onclick=\"openHRresult('{raceresultpage}', '{wdtitle}', {wdwidth},{wdheight}); return false\">{lblResult}</a>";
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
                                    //TemplateHandler.Assign(ref resultLink, "{wdleft}", "300");
                                    //TemplateHandler.Assign(ref resultLink, "{wdtop}", "100");
                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                }

                            }
                            #endregion
                            else
                            {
                                _status = statusFormat;
                                TemplateHandler.Assign(ref rowHtml, "{Status}", _status);

                                if (_status.Equals(Status.RUNNING))
                                {
                                    TemplateHandler.RemoveBlock(ref rowHtml, "swinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "mwinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "awinlost");

                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentComm");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperComm}", IBCUtils.FormatHandler.FormatNumber20(_scomm));
                                    TemplateHandler.Assign(ref rowHtml, "{MasterComm}", IBCUtils.FormatHandler.FormatNumber20(_mcomm));
                                    TemplateHandler.Assign(ref rowHtml, "{AgentComm}", IBCUtils.FormatHandler.FormatNumber20(_acomm));

                                    TemplateHandler.Assign(ref rowHtml, "{swinlost}", IBCUtils.FormatHandler.FormatNumber20(_swinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{mwinlost}", IBCUtils.FormatHandler.FormatNumber20(_mwinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{awinlost}", IBCUtils.FormatHandler.FormatNumber20(_awinlost));
                                }

                                if (CastAide.AsInt32(row["bettype"]) == 71 || CastAide.AsInt32(row["bettype"]) == 72 || CastAide.AsInt32(row["bettype"]) == 73 || CastAide.AsInt32(row["bettype"]) == 1000)
                                {
                                    TemplateHandler.RemoveBlock(ref rowHtml, "br");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperPT");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterPT");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentPT");

                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperCommRate");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterCommRate");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentCommRate");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperPT}", IBCUtils.FormatHandler.FormatNumber3(_spt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{MasterPT}", IBCUtils.FormatHandler.FormatNumber3(_mpt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{AgentPT}", IBCUtils.FormatHandler.FormatNumber3(_apt) + "%");

                                    TemplateHandler.Assign(ref rowHtml, "{SuperCommRate}", IBCUtils.FormatHandler.FormatNumber3(_scommRate));
                                    TemplateHandler.Assign(ref rowHtml, "{MasterCommRate}", IBCUtils.FormatHandler.FormatNumber3(_mcommRate));
                                    TemplateHandler.Assign(ref rowHtml, "{AgentCommRate}", IBCUtils.FormatHandler.FormatNumber3(_acommRate));
                                }

                                if (CastAide.AsInt32(row["bettype"]) == 1000)
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{swinlost}", "0");
                                    TemplateHandler.Assign(ref rowHtml, "{mwinlost}", "0");
                                    TemplateHandler.Assign(ref rowHtml, "{awinlost}", "0");
                                }

                                //Show Result link for other bettype

                                switch (_status.Trim().ToLower())
                                {
                                    case "running":
                                    case "waiting":
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                        break;
                                    case "void":
                                    case "reject":
                                    case "refund":
                                    case "draw":
                                    case "lose":
                                    case "won":
                                        int sportTypeId = CastAide.AsInt32(row["sporttype"]);
                                        if (sportTypeId == 201) //finance
                                        {
                                            //do nothing if void/reject
                                            if (_status.Trim().ToLower() == "void" || _status.Trim().ToLower() == "reject")
                                                TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                            else
                                            {
                                                resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"viewFinanceResult('" + CastAide.AsString(row["transid"]) + "')\">{lblResult}</div> ";
                                                TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                            }
                                        }
                                        else if (sportTypeId == 13)//politics
                                        {
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                        }
                                        else if (sportTypeId == 161)//number game
                                        {
                                            resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewResultNumberGame('BetListNumberGameResult.aspx?matchId=" + CastAide.AsString(row["matchid"]) + "')\">{lblResult}</div> ";
                                            TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                        }
                                        else if (sportTypeId == 162)//Live Casino
                                        {
                                            resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewResultLiveCasino('../LiveCasino/Betlist/BetListLiveCasinoResult.aspx?matchId=" + CastAide.AsString(row["matchid"]) + "&bettype=" + _bettype + "&league=" + _leagueName + "')\">{lblResult}</div> ";
                                            TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                        }
                                       
                                        else
                                        {
                                            switch (_bettype)
                                            {
                                                case 29://Combination MP
                                                    resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewCombinationMixParlayResult('" + CastAide.AsString(row["refno"]) + "', '" + CastAide.AsString(row["matchid"]) + "', " + _bettype + ", '" + _betId + "')\">{lblResult}</div> ";
                                                    //string resultLink = "<a onclick=\"ViewMixParlayResult('" + CastAide.AsString(row["refno"])  + "')\">{lblResult}</a>";
                                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                                    break;
                                                case 9: //Mix Parlay
                                                    resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewMixParlayResult('" + CastAide.AsString(row["refno"]) + "', '" + CastAide.AsString(row["matchid"]) + "', " + _bettype + ", '" + _betId + "' )\">{lblResult}</div> ";
                                                    //string resultLink = "<a onclick=\"ViewMixParlayResult('" + CastAide.AsString(row["refno"])  + "')\">{lblResult}</a>";
                                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                                    break;
                                                case 10: //Outright
                                                case 71: //Casino
                                                case 72: //P2P
                                                case 73: //Mini game
                                                case 1000:
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                                    //int sportTypeId = CastAide.AsInt32(dr["sporttype"]);
                                                    break;
                                                default: //Sport Book
                                                    resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewResult('" + CastAide.AsString(row["matchid"]) + "', " + _bettype + ", '" + _betId + "')\">{lblResult}</div> ";
                                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                                    break;
                                            }
                                        }

                                        break;
                                    default:
                                        //do nothing
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                        break;
                                }

                            }
                            #endregion

                            //Show IP
                            //TemplateHandler.Assign(ref rowHtml, "{ip}", IPUtils.ReplaceVNIP(row["ip"].ToString()));
                            /*if (!AppSession.ViewIPVN && IpManagement.IsVNIP(CastAide.AsString(row["ip"]), ipVNList))
                            {
                                if (WebSettings.IPVNReplace == "1")
                                    TemplateHandler.Assign(ref rowHtml, "{ip}", CastAide.AsString(row["ip"]));
                                else
                                    TemplateHandler.ReplaceBlock(ref rowHtml, "DisplayIP", WebSettings.IPVNReplace);
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{ip}", CastAide.AsString(row["ip"]));
                            }*/


                            TemplateHandler.Assign(ref rowHtml, "{ip}", IpManagement.ReplaceVNIP(CastAide.AsString(row["ip"])));

                            //Show Delete button
                            #region show delete button
                            //if (CastAide.AsBoolean(row["allowdelete"], true) == false)
                            //    TemplateHandler.RemoveBlock(ref rowHtml, "delete");
                            //if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103 || _bettype == 1000)
                            //    TemplateHandler.RemoveBlock(ref rowHtml, "delete");
                            //TemplateHandler.RemoveBlock(ref rowHtml, "delete");
                           
                      
                            TemplateHandler.Assign(ref rowHtml, "{BetId}", _transId);
                            TemplateHandler.Assign(ref rowHtml, "{betfrom}", ((ds.Columns.Contains("betfrom")) ? CastAide.AsString(row["betfrom"]).ToLower() : string.Empty));
                            TemplateHandler.Assign(ref rowHtml, "{matchid}", ((ds.Columns.Contains("matchid")) ? CastAide.AsString(row["matchid"]).ToLower() : string.Empty));
                            TemplateHandler.Assign(ref rowHtml, "{bettype}", ((ds.Columns.Contains("bettype")) ? CastAide.AsString(row["bettype"]).ToLower() : string.Empty));

                            if (string.IsNullOrEmpty(_liveStyle)) { _odd_even = (_odd_even == "o") ? "e" : "o"; TemplateHandler.Assign(ref rowHtml, "{odd_even}", _odd_even); }
                            else TemplateHandler.Assign(ref rowHtml, "{live}", _liveStyle);

                            TemplateHandler.Assign(ref rowHtml, "{fdate}", _transdate.ToShortDateString());
                            TemplateHandler.Assign(ref rowHtml, "{ThemePath}", WebSettings.ThemePath);


                            html.Append(rowHtml);
                            #endregion
                        }
                        if (index == 0 && WebSettings.disabledHR)
                        {
                            html.Append(AccCommon.GenNoInformation("11"));
                            html.Append("</table>");
                        }
                        else if (index == 0 && WebSettings.disabledFI)
                        {
                            html.Append(AccCommon.GenNoInformation("11"));
                            html.Append("</table>");
                        }
                        else
                        {
                            html.Append("</table>");
                            //html.Append(AccCommon.GenPager_Betlist(pageNum, page, 11) + "</table>");

                            //closed by Kevin
                            //html.Append(Pager.InitPager(pageNum, page, "GotoPage"));
                            //paging = Pager.InitPager(pageNum, page, "GotoPage");

                        }
                    }
                    else
                    {
                        html.Append(AccCommon.GenNoInformation("11"));
                        html.Append("</table>");
                    }
                }
                else
                {
                    html.Append(AccCommon.GenNoInformation("11"));
                    html.Append("</table>");
                }

                if (custId == 0)
                {
                    html.Append(AlertMessage.GetMsg(IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error));
                }

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            TemplateHandler.Assign(ref rowTpl, "{content}", html.ToString());
            //rowTpl += paging;
            return rowTpl;
        }

        //public string GenMainReport(DateTime fdate, DateTime tdate,int inputBettype)
        public string GenMainReport(DataTable ds, DataTable dtEvent)
        {
            int custId = 0, index = 0;
            int pageNum = 0;
            int page = 1;
            //DbResult ds = null;
            string rowTpl, eventTpl, eventTplLC;
            string paging = string.Empty;
            StringBuilder html = new StringBuilder();

            rowTpl = WebUtils.LoadTemplate("BetList\\BetListMainRow.htm");
            eventTpl = WebUtils.LoadTemplate("BetList\\Event1.htm");
            eventTplLC = WebUtils.LoadTemplate("BetList\\EventLC.htm");

            if (AppSession.SYSTEM.ToLower() != "accounting")
            {
                TemplateHandler.RemoveBlock(ref rowTpl, "delete");
            }

            string rows = TemplateHandler.CopyBlock(rowTpl, "row");
            TemplateHandler.RemoveBlock(ref rowTpl, "row");
            try
            {
                custId = CastAide.AsInt32(HttpContext.Current.Session["CustId"]);

                if (ds != null && ds.Rows.Count > 0)
                {
                    int startIndex = 0;
                    int endIndex = 0;
                    int pageSize = 30;
                    int.TryParse(HttpContext.Current.Request["page"], out page);
                    pageNum = ds.Rows.Count / pageSize;
                    int rowLastPage = ds.Rows.Count % pageSize;
                    if (rowLastPage > 0)
                        pageNum++;
                    if (page < 1 || page > pageNum)
                    {
                        page = pageNum;
                    }
                    startIndex = (page - 1) * pageSize;
                    if (pageNum != 0)
                        endIndex = startIndex + ((rowLastPage > 0 && page == pageNum) ? rowLastPage : pageSize);
                    decimal total = 0;
                    if (ds.Rows.Count > 0 && ds.Rows.Count > 0)
                    {
                        var ipVNList = IpManagement.IPVNList;
                        _odd_even = "o";
                        for (int i = startIndex; i < endIndex; ++i)
                        {

                            index++;
                            DataRow row = ds.Rows[i];

                            _bettype = Convert.IsDBNull(row["bettype"]) ? 0 : CastAide.AsInt32(row["bettype"]);
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

                           
                            #region will be removed later

                            if (_bettype == 81 && string.IsNullOrEmpty(_bettypeName))
                                _bettypeName = "1st ball OU";

                            #endregion

                            _sportypeName = CacheRef.GetSportName(row["sporttype"]);
                            _leagueName = CacheRef.GetLeagueName(row["leagueid"]);
                            _OddsTypeName = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(_bettype, CastAide.AsInt32(row["oddstype"])));

                            _transId = Convert.IsDBNull(row["transid"]) ? "0" : row["transid"].ToString();
                            _refno = Convert.IsDBNull(row["refno"]) ? "0" : row["refno"].ToString();
                            _matchCode = row["matchcode"] + "";
                            _hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : CastAide.AsDecimal(row["hdp1"]);
                            _hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : CastAide.AsDecimal(row["hdp2"]);
                            //_odds = CastAide.AsDecimal(row["odds"]);
                            _odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(row["odds"]), _bettype, CastAide.AsInt32(row["oddstype"]));
                            _modds = CastAide.AsDecimal(row["modds"]);
                            if (IsHRBettype(_bettype))
                            {
                                _mOdds = Convert.IsDBNull(row["modds"]) ? 0 : CastAide.AsDecimal(row["modds"]);
                                _finalodds = Convert.IsDBNull(row["finalodds"]) ? 0 : CastAide.AsDecimal(row["finalodds"]);
                                _finalodds2 = Convert.IsDBNull(row["finalodds2"]) ? 0 : CastAide.AsDecimal(row["finalodds2"]);
                            }
                            _com = Convert.IsDBNull(row["commission"]) ? 0 : CastAide.AsDecimal(row["commission"]);
                            _totalcom += _com;
                            _status = row["status"] + "";
                            _home = CacheRef.GetTeamName(row["homeid"]) + (CastAide.AsBoolean(row["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
                            _away = CacheRef.GetTeamName(row["awayid"]);

                            _stake = Convert.IsDBNull(row["stake"]) ? 0 : CastAide.AsDecimal(row["stake"]);
                            _actual_stake = Convert.IsDBNull(row["stake"]) ? 0 : CastAide.AsDecimal(row["actual_stake"]);
                            _transdate = Convert.ToDateTime(row["transdate"]);
                            //_betteam = row["betteam"] + "";

                          
                            var commstatus = 0;
                            if (WebSettings.OfferDoubleComm)
                            {
                                commstatus = CastAide.AsInt32(row["comstatus"]);
                            }
                            _betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(row["betteam"].ToString(), _bettype, "e").Trim();
                            _liveStyle = Convert.IsDBNull(row["liveindicator"]) ? "" : (Convert.ToBoolean(row["liveindicator"]) ? "event-live" : "");
                            _liveindicator = Convert.ToInt16(row["liveindicator"]);
                            _finalHomeScore = row["finalhomescore"] + "";
                            _finalAwayScore = row["finalawayscore"] + "";
                            _liveHomeScore = row["livehomescore"] + "";
                            _liveAwayScore = row["liveawayscore"] + "";
                            _betId = CastAide.AsString(row["betid"]);
                            _showtime = CastAide.AsString(row["showtime"]);
                            if (_bettype >= 1301 && _bettype <= 1334)
                            {
                                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype, _betId.Substring(1));
                            }
                            else
                            {
                                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype);
                            }
                          
                            var _listBetlistEvt = new List<BetlistEvent>();
                            //check refno have not exist in event datatable
                            if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103)
                            {
                                if (dtEvent != null && dtEvent.Rows.Count > 0)
                                {
                                   
                                    var query = from blEvent in dtEvent.AsEnumerable()
                                                where blEvent.Field<string>("refno") == _refno
                                                select blEvent;
                                    //var query = dtEvent.AsEnumerable().Where(
                                    //    s => CastAide.AsString(s["refno"]) == _refno).Select(s => s);

                                    DataTable dtb3 = query.CopyToDataTable();

                                    foreach (DataRow rowEvt in query)
                                    {
                                        if (_bettype == 1101)
                                        {
                                            _listBetlistEvt.Add(new BetlistEvent(){
                                                 GameType = CacheRef.GetLiveCasinoResult(CastAide.AsInt32(rowEvt["betteam"])),
                                                 Stake = IBCUtils.FormatHandler.FormatNumber2(rowEvt["stake"]),
                                                 Odds = IBCUtils.FormatHandler.FormatNumber2(rowEvt["odds"])
                                            });
                                            
                                        }
                                        else if (_bettype == 1102)
                                        {
                                            _listBetlistEvt.Add(new BetlistEvent()
                                            {
                                                GameType = CacheRef.GetRouletteBetItem(CastAide.AsInt32(rowEvt["betteam"])),
                                                Stake = IBCUtils.FormatHandler.FormatNumber2(rowEvt["stake"]),
                                                Odds = IBCUtils.FormatHandler.FormatNumber2(rowEvt["odds"])
                                            });
                                        }

                                    }


                                }
                            }
                            string winlostStr = "&nbsp;";

                            decimal winlost = IBCUtils.FormatHandler.GetDecimal(row["winlost"]);
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

                                if (!(_status.Equals("running") || string.IsNullOrEmpty(_status)))
                                {
                                    total += winlost;
                                    winlostStr = IBCUtils.FormatHandler.FormatNumber20(winlost);
                                }
                            }
                            string rowHtml = (string)rows.Clone();

                            _spt = (Convert.IsDBNull(row["spositiontaking"]) ? 0 : CastAide.AsDecimal(row["spositiontaking"])) * 100;
                            _mpt = (Convert.IsDBNull(row["mpositiontaking"]) ? 0 : CastAide.AsDecimal(row["mpositiontaking"])) * 100;
                            _apt = (Convert.IsDBNull(row["apositiontaking"]) ? 0 : CastAide.AsDecimal(row["apositiontaking"])) * 100;

                            _swinlost = Convert.IsDBNull(row["swinlost"]) ? 0 : Convert.ToDecimal(row["swinlost"]);
                            _mwinlost = Convert.IsDBNull(row["mwinlost"]) ? 0 : Convert.ToDecimal(row["mwinlost"]);
                            _awinlost = Convert.IsDBNull(row["awinlost"]) ? 0 : Convert.ToDecimal(row["awinlost"]);

                            _scommRate = CastAide.AsDouble(row["sdiscount"], 0) * 100;
                            _mcommRate = CastAide.AsDouble(row["adiscount"], 0) * 100;
                            _acommRate = CastAide.AsDouble(row["discount"], 0) * 100;

                            _scomm = Convert.IsDBNull(row["scomm"]) ? 0 : Convert.ToDecimal(row["scomm"]);
                            _mcomm = Convert.IsDBNull(row["acomm"]) ? 0 : Convert.ToDecimal(row["acomm"]);
                            _acomm = Convert.IsDBNull(row["comm"]) ? 0 : Convert.ToDecimal(row["comm"]);

                            //No
                            TemplateHandler.Assign(ref rowHtml, "{no}", (i + 1).ToString());

                            //TransTime
                            if (_bettype == 71 || _bettype == 72 || _bettype == 73 || _bettype == 113 || _bettype == 114 || _bettype == 112)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "<b>Ref No:" + _matchCode + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }
                            else if (_bettype == 1000)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "<b>Ref No:" + _transId.ToString() + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{TransTime}", "<b>Ref No:" + _transId.ToString() + "</b><br>" + _transdate.ToShortDateString() + "&nbsp;" + _transdate.ToLongTimeString());
                            }

                            //Event
                            #region event
                            //phuong.nguyen, add case for HR (31, 32, 33)
                            if (IsHRBettype(_bettype))
                            {
                                _home = row["matchcode"] + " - " + CacheRef.GetHorseTeamName(row["awayid"]);
                                _race = "Race " + row["race"].ToString();
                            }

                            string eventItem = (string)eventTpl.Clone();
                            if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103)
                            {
                                eventItem = (string)eventTplLC.Clone();
                                GenEventLiveCasino(ref eventItem, _listBetlistEvt, 1);
                                TemplateHandler.Assign(ref rowHtml, "{Event}", eventItem);
                            }
                            else
                                GenEvent(ref rowHtml, row, eventItem, 1);
                            #endregion

                            //Odds
                            #region Odds
                            // _odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(_odds, _bettype, CastAide.AsInt32(row["oddstype"]));
                            if (_bettype == 9)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Odds}", IBCUtils.FormatHandler.FormatNumber40(_odds) + "<div class='b bl-leaguename'>" + _OddsTypeName + "</div>");
                                TemplateHandler.Assign(ref rowHtml, "{OddsWidth}", "style='width:80px'");
                            }
                            else if (_bettype == 71 || _bettype == 72 || _bettype == 73 || _bettype == 113 || _bettype == 114 || _bettype == 112)
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
                            else if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103 || _bettype == 1000)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Odds}", "-");
                            }
                            else
                            {
                                #region HR bettype
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

                            //OpenTote Exchange Rate
                            var isShowOtExRate = CastAide.AsBoolean(row["isShowOTExRate"], false);
                            var virtualRate = CastAide.AsDouble(row["virtualrate"], 0.0);

                            if (!isShowOtExRate)
                            {
                                TemplateHandler.RemoveBlock(ref rowHtml, "Exchangerate");
                            }
                            else
                                TemplateHandler.Assign(ref rowHtml, "{ExchangeRate}", isShowOtExRate ? virtualRate.ToString() : string.Empty);

                            #region  winlost
                            if (_status.Equals("running") || string.IsNullOrEmpty(_status) || _status.Equals("rejected") || _status.Equals("waiting"))
                            {
                                winlostStr = string.Empty;
                            }
                            winlostStr = CastAide.AsDecimal(winlostStr) < 0 ? "<font color='red'>" + winlostStr + "</font>" : winlostStr;
                            TemplateHandler.Assign(ref rowHtml, "{WinLost}", (!string.IsNullOrEmpty(winlostStr)) ? winlostStr + "<br>" + IBCUtils.FormatHandler.FormatNumber20(_com) : string.Empty);
                            #endregion

                            //status
                            #region status
                            string resultLink = string.Empty;
                            var statusFormat = (_status.ToLower() == "reject" || _status.ToLower() == "refund" || _status.ToLower() == "void") ? "<span style=\"color:red;\">" + _status.ToUpper() + "</span>" : _status;
                            #region HR
                            if (IsHRBettype(_bettype))
                            {
                                decimal spt2 = (Convert.IsDBNull(row["spositiontaking2"]) ? 0 : CastAide.AsDecimal(row["spositiontaking2"])) * 100;
                                decimal mpt2 = (Convert.IsDBNull(row["mpositiontaking2"]) ? 0 : CastAide.AsDecimal(row["mpositiontaking2"])) * 100;
                                decimal apt2 = (Convert.IsDBNull(row["apositiontaking2"]) ? 0 : CastAide.AsDecimal(row["apositiontaking2"])) * 100;

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

                                if (IsHRWinPlaceBettype(_bettype))
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperPT}", "Win: " + IBCUtils.FormatHandler.FormatNumber3(_spt) + "%<br><span style='color:#606060'>Place: " + IBCUtils.FormatHandler.FormatNumber3(spt2) + "%</span>");
                                    TemplateHandler.Assign(ref rowHtml, "{MasterPT}", "Win: " + IBCUtils.FormatHandler.FormatNumber3(_mpt) + "%<br><span style='color:#606060'>Place: " + IBCUtils.FormatHandler.FormatNumber3(mpt2) + "%</span>");
                                    TemplateHandler.Assign(ref rowHtml, "{AgentPT}", "Win: " + IBCUtils.FormatHandler.FormatNumber3(_apt) + "%<br><span style='color:#606060'>Place: " + IBCUtils.FormatHandler.FormatNumber3(apt2) + "%</span>");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperPT}", IBCUtils.FormatHandler.FormatNumber3(_spt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{MasterPT}", IBCUtils.FormatHandler.FormatNumber3(_mpt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{AgentPT}", IBCUtils.FormatHandler.FormatNumber3(_apt) + "%");
                                }

                               
                                    TemplateHandler.Assign(ref rowHtml, "{SuperCommRate}", IBCUtils.FormatHandler.FormatNumber3(_scommRate));
                                    TemplateHandler.Assign(ref rowHtml, "{MasterCommRate}", IBCUtils.FormatHandler.FormatNumber3(_mcommRate));
                                    TemplateHandler.Assign(ref rowHtml, "{AgentCommRate}", IBCUtils.FormatHandler.FormatNumber3(_acommRate));
                               

                                if (_status.Equals(Status.RUNNING))
                                {
                                    TemplateHandler.RemoveBlock(ref rowHtml, "swinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "mwinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "awinlost");

                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentComm");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{swinlost}", IBCUtils.FormatHandler.FormatNumber20(_swinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{mwinlost}", IBCUtils.FormatHandler.FormatNumber20(_mwinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{awinlost}", IBCUtils.FormatHandler.FormatNumber20(_awinlost));

                                    TemplateHandler.Assign(ref rowHtml, "{SuperComm}", IBCUtils.FormatHandler.FormatNumber20(_scomm));
                                    TemplateHandler.Assign(ref rowHtml, "{MasterComm}", IBCUtils.FormatHandler.FormatNumber20(_mcomm));
                                    TemplateHandler.Assign(ref rowHtml, "{AgentComm}", IBCUtils.FormatHandler.FormatNumber20(_acomm));
                                }


                                //show Result link for HR
                                if ((_status.Equals(Status.WIN) || _status.Equals(Status.LOSE)))
                                {
                                    resultLink = "<a style='cursor:pointer' onclick=\"openHRresult('{raceresultpage}', '{wdtitle}', {wdwidth},{wdheight}); return false\">{lblResult}</a>";
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
                                    //TemplateHandler.Assign(ref resultLink, "{wdleft}", "300");
                                    //TemplateHandler.Assign(ref resultLink, "{wdtop}", "100");
                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                }

                            }
                            #endregion
                            else
                            {
                                _status = statusFormat;
                                TemplateHandler.Assign(ref rowHtml, "{Status}", _status);

                                if (_status.Equals(Status.RUNNING))
                                {
                                    TemplateHandler.RemoveBlock(ref rowHtml, "swinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "mwinlost");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "awinlost");

                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterComm");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentComm");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperComm}", IBCUtils.FormatHandler.FormatNumber20(_scomm));
                                    TemplateHandler.Assign(ref rowHtml, "{MasterComm}", IBCUtils.FormatHandler.FormatNumber20(_mcomm));
                                    TemplateHandler.Assign(ref rowHtml, "{AgentComm}", IBCUtils.FormatHandler.FormatNumber20(_acomm));

                                    TemplateHandler.Assign(ref rowHtml, "{swinlost}", IBCUtils.FormatHandler.FormatNumber20(_swinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{mwinlost}", IBCUtils.FormatHandler.FormatNumber20(_mwinlost));
                                    TemplateHandler.Assign(ref rowHtml, "{awinlost}", IBCUtils.FormatHandler.FormatNumber20(_awinlost));
                                }

                                if (CastAide.AsInt32(row["bettype"]) == 71 || CastAide.AsInt32(row["bettype"]) == 72 || CastAide.AsInt32(row["bettype"]) == 73 || CastAide.AsInt32(row["bettype"]) == 1000)
                                {
                                    TemplateHandler.RemoveBlock(ref rowHtml, "br");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperPT");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterPT");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentPT");

                                    TemplateHandler.RemoveBlock(ref rowHtml, "SuperCommRate");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "MasterCommRate");
                                    TemplateHandler.RemoveBlock(ref rowHtml, "AgentCommRate");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{SuperPT}", IBCUtils.FormatHandler.FormatNumber3(_spt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{MasterPT}", IBCUtils.FormatHandler.FormatNumber3(_mpt) + "%");
                                    TemplateHandler.Assign(ref rowHtml, "{AgentPT}", IBCUtils.FormatHandler.FormatNumber3(_apt) + "%");

                                    if (commstatus == 1)
                                    {
                                        TemplateHandler.Assign(ref rowHtml, "{SuperCommRate}", "2x " + IBCUtils.FormatHandler.FormatNumber3(_scommRate));
                                        TemplateHandler.Assign(ref rowHtml, "{MasterCommRate}", "2x " + IBCUtils.FormatHandler.FormatNumber3(_mcommRate));
                                        TemplateHandler.Assign(ref rowHtml, "{AgentCommRate}", "2x " + IBCUtils.FormatHandler.FormatNumber3(_acommRate));
                                    }
                                    else
                                    {
                                        TemplateHandler.Assign(ref rowHtml, "{SuperCommRate}", IBCUtils.FormatHandler.FormatNumber3(_scommRate));
                                        TemplateHandler.Assign(ref rowHtml, "{MasterCommRate}", IBCUtils.FormatHandler.FormatNumber3(_mcommRate));
                                        TemplateHandler.Assign(ref rowHtml, "{AgentCommRate}", IBCUtils.FormatHandler.FormatNumber3(_acommRate));
                                    }
                                }

                                if (CastAide.AsInt32(row["bettype"]) == 1000)
                                {
                                    TemplateHandler.Assign(ref rowHtml, "{swinlost}", "0");
                                    TemplateHandler.Assign(ref rowHtml, "{mwinlost}", "0");
                                    TemplateHandler.Assign(ref rowHtml, "{awinlost}", "0");
                                }

                                //Show Result link for other bettype

                                switch (_status.Trim().ToLower())
                                {
                                    case "running":
                                    case "waiting":
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                        break;
                                    case "void":
                                    case "reject":
                                    case "refund":
                                    case "draw":
                                    case "lose":
                                    case "won":
                                        int sportTypeId = CastAide.AsInt32(row["sporttype"]);
                                        if (sportTypeId == 201) //finance
                                        {
                                            //do nothing if void/reject
                                            if (_status.Trim().ToLower() == "void" || _status.Trim().ToLower() == "reject")
                                                TemplateHandler.Assign(ref rowHtml, "{Result}", "");

                                            else
                                            {
                                                resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"viewFinanceResult('" + CastAide.AsString(row["transid"]) + "')\">{lblResult}</div> ";
                                                TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                            }
                                        }
                                        else if (sportTypeId == 13)//politics
                                        {
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                        }
                                        else if (sportTypeId == 161)//number game
                                        {
                                            resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewResultNumberGame('BetListNumberGameResult.aspx?matchId=" + CastAide.AsString(row["matchid"]) + "')\">{lblResult}</div> ";
                                            TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                        }
                                        else if (sportTypeId == 162)//Live Casino
                                        {
                                            resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewResultLiveCasino('../LiveCasino/Betlist/BetListLiveCasinoResult.aspx?matchId=" + CastAide.AsString(row["matchid"]) + "&bettype=" + _bettype + "&league=" + _leagueName + "')\">{lblResult}</div> ";
                                            TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                            TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                        }
                                        else
                                        {
                                            switch (_bettype)
                                            {
                                                case 29://Combination MP
                                                    resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewCombinationMixParlayResult('" + CastAide.AsString(row["refno"]) + "', '" + CastAide.AsString(row["matchid"]) + "', " + _bettype + ", '" + _betId + "')\">{lblResult}</div> ";
                                                    //string resultLink = "<a onclick=\"ViewMixParlayResult('" + CastAide.AsString(row["refno"])  + "')\">{lblResult}</a>";
                                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                                    break;
                                                case 9: //Mix Parlay
                                                    resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewMixParlayResult('" + CastAide.AsString(row["refno"]) + "', '" + CastAide.AsString(row["matchid"]) + "', " + _bettype + ", '" + _betId + "')\">{lblResult}</div> ";
                                                    //string resultLink = "<a onclick=\"ViewMixParlayResult('" + CastAide.AsString(row["refno"])  + "')\">{lblResult}</a>";
                                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                                    break;
                                                case 10: //Outright
                                                case 71: //Casino
                                                case 72: //P2P
                                                case 73: //Mini game
                                                case 1000:
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                                    //int sportTypeId = CastAide.AsInt32(dr["sporttype"]);
                                                    break;
                                                default: //Sport Book
                                                    resultLink = "<div class='hand' style='color: #1BA4ED;' onclick=\"ViewResult('" + CastAide.AsString(row["matchid"]) + "', " + _bettype + ", '" + _betId + "')\">{lblResult}</div> ";
                                                    TemplateHandler.Assign(ref resultLink, "{lblResult}", "Result");
                                                    TemplateHandler.Assign(ref rowHtml, "{Result}", resultLink);
                                                    break;
                                            }
                                        }

                                        break;
                                    default:
                                        //do nothing
                                        TemplateHandler.Assign(ref rowHtml, "{Result}", "");
                                        break;
                                }

                            }
                            #endregion

                            //Show IP
                            //TemplateHandler.Assign(ref rowHtml, "{ip}", IPUtils.ReplaceVNIP(row["ip"].ToString()));
                            /*if (!AppSession.ViewIPVN && IpManagement.IsVNIP(CastAide.AsString(row["ip"]), ipVNList))
                            {
                                if (WebSettings.IPVNReplace == "1")
                                    TemplateHandler.Assign(ref rowHtml, "{ip}", CastAide.AsString(row["ip"]));
                                else
                                    TemplateHandler.ReplaceBlock(ref rowHtml, "DisplayIP", WebSettings.IPVNReplace);
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{ip}", CastAide.AsString(row["ip"]));
                            }*/


                            TemplateHandler.Assign(ref rowHtml, "{ip}", IpManagement.ReplaceVNIP(CastAide.AsString(row["ip"])));

                            //Show Delete button
                            #region show delete button
                            if (CastAide.AsBoolean(row["allowdelete"], true) == false)
                                TemplateHandler.RemoveBlock(ref rowHtml, "delete");
                            if (_bettype == 1101 || _bettype == 1102 || _bettype == 1103 || _bettype == 1000)
                                TemplateHandler.RemoveBlock(ref rowHtml, "delete");
                            TemplateHandler.Assign(ref rowHtml, "{BetId}", _transId);
                            TemplateHandler.Assign(ref rowHtml, "{betfrom}", ((ds.Columns.Contains("betfrom")) ? CastAide.AsString(row["betfrom"]).ToLower() : string.Empty));
                            TemplateHandler.Assign(ref rowHtml, "{matchid}", ((ds.Columns.Contains("matchid")) ? CastAide.AsString(row["matchid"]).ToLower() : string.Empty));
                            TemplateHandler.Assign(ref rowHtml, "{bettype}", ((ds.Columns.Contains("bettype")) ? CastAide.AsString(row["bettype"]).ToLower() : string.Empty));

                            if (string.IsNullOrEmpty(_liveStyle)) { _odd_even = (_odd_even == "o") ? "e" : "o"; TemplateHandler.Assign(ref rowHtml, "{odd_even}", _odd_even); }
                            else TemplateHandler.Assign(ref rowHtml, "{live}", _liveStyle);

                            TemplateHandler.Assign(ref rowHtml, "{fdate}", _transdate.ToShortDateString());
                            TemplateHandler.Assign(ref rowHtml, "{ThemePath}", WebSettings.ThemePath);


                            html.Append(rowHtml);
                            #endregion
                        }
                        if (index == 0 && WebSettings.disabledHR)
                        {
                            html.Append(AccCommon.GenNoInformation("11"));
                            html.Append("</table>");
                        }
                        else if (index == 0 && WebSettings.disabledFI)
                        {
                            html.Append(AccCommon.GenNoInformation("11"));
                            html.Append("</table>");
                        }
                        else
                        {
                            html.Append("</table>");
                            //html.Append(AccCommon.GenPager_Betlist(pageNum, page, 11) + "</table>");

                            //closed by Kevin
                            //html.Append(Pager.InitPager(pageNum, page, "GotoPage"));
                            paging = Pager.InitPager(pageNum, page, "GotoPage");

                        }
                    }
                    else
                    {
                        html.Append(AccCommon.GenNoInformation("11"));
                        html.Append("</table>");
                    }
                }
                else
                {
                    html.Append(AccCommon.GenNoInformation("11"));
                    html.Append("</table>");
                }

                if (custId == 0)
                {
                    html.Append(AlertMessage.GetMsg(IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error));
                }

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            TemplateHandler.Assign(ref rowTpl, "{content}", html.ToString());
            rowTpl += paging;
            return rowTpl;
        }


    }
}
