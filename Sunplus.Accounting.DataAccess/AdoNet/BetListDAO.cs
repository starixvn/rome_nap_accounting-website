using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using Fanex.Data;
using Sunplus.Accounting.Core.Cache.Extension;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper.IP;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.Utils;
using Fanex.Tarus.Infrastructure.Extensions;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    public class BetListDAO : DataAccess, IBetListDAO
    {
        protected StoreProcedureElement GetBetListStoreProcedure(string alias)
        {
            var spElement = GetStoreProcedure(Core.Configurations.StoreProcedureGroup.BetListSection.StoreCollection.BetListStores, alias);
            return spElement;
        }


        public List<BetListDetail> GetLatestBetList(int totalRow)
        {
            throw new NotImplementedException();
        }

        public List<BetMonitor> GetLatestBABetList(int totalRow, int amount, Int64 lastTransId)
        {
            var betList = new List<BetMonitor>();
            var spElement = GetBetListStoreProcedure("BALatestBetlist");
            var db = GetDatabase(spElement.DbName);


            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "totalrow", DbType.Int32, totalRow);
                db.AddInParameter(dbCommand, "amount", DbType.Int32, amount);
                db.AddInParameter(dbCommand, "lasttransid", DbType.Int64, lastTransId);

                var data = db.ExecuteReader(dbCommand);

                if (data != null)
                {
                    while (data.Read())
                    {
                        var bettran = new Bettran
                                          {
                                              Transid = CastAide.AsString(data["transid"]),
                                              HomeId = CastAide.AsString(data["homeid"]),
                                              AwayId = CastAide.AsString(data["awayid"]),
                                              Bettype = CastAide.AsInt32(data["bettype"]),
                                              SportId = CastAide.AsString(data["sporttype"]),
                                              Refno = CastAide.AsString(data["refno"]),
                                              Matchid = CastAide.AsInt32(data["matchid"]),
                                              Betid = CastAide.AsString(data["betid"]),
                                              Hdp1 = CastAide.AsString(data["hdp1"]),
                                              Hdp2 = CastAide.AsString(data["hdp2"]),
                                              Odds = CastAide.AsString(data["odds"]),
                                              OddsType = CastAide.AsInt32(data["OddsType"]),
                                              Status = CastAide.AsString(data["status"]),
                                              Stake = CastAide.AsString(data["stake"]),
                                              Transdate = CastAide.AsString(data["transdate"]),
                                              Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(data["betteam"].ToString(), CastAide.AsInt32(data["bettype"]), "e"),
                                              Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                                              Modds = CastAide.AsString(data["modds"]),
                                              Custid = CastAide.AsInt32(data["custid"]),
                                              Livehomescore = CastAide.AsString(data["livehomescore"]),
                                              Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                                              LeagueId = CastAide.AsString(data["leagueid"]),
                                              IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                                              CurrencyName = CastAide.AsString(data["CurrencyName"]),
                                              UserName = CastAide.AsString(data["UserName"]),
                                              MatchCode = CastAide.AsString(data["matchcode"])


                                          };
                        var betMonitor = new BetMonitor
                                             {
                                                 Transid = bettran.Transid,
                                                 BettranDetail = bettran

                                                 //Name = reader.GetString(reader["Name"]
                                             };
                        betList.Add(betMonitor);
                    }
                    data.Close();
                }

                return betList;

            }


        }

        public BABetList GetBABetList(int pagesize, int pageindex, string fromdate, string todate, string username, List<IPVNBean> IPVNList, int currency, int bettype, int fromStake, int toStake, int excludeTest)
        {
            var betList = new List<BetListDetail>();
            var spElement = GetBetListStoreProcedure("BABetlist");
            var db = GetDatabase(spElement.DbName);


            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "FromDate", DbType.String, fromdate);
                db.AddInParameter(dbCommand, "ToDate", DbType.String, todate);
                db.AddInParameter(dbCommand, "MoveBetsDay", DbType.String, ReportUtils.BalanceUp.MoveBetsDay.ToString());
                db.AddInParameter(dbCommand, "BalanceUpDay", DbType.String, ReportUtils.BalanceUp.BalanceUpDay.ToString());
                db.AddInParameter(dbCommand, "Username", DbType.String, username);
                db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pagesize);
                db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageindex);
                db.AddInParameter(dbCommand, "currency", DbType.Int32, currency);
                db.AddInParameter(dbCommand, "Bettype", DbType.Int32, bettype);
                db.AddInParameter(dbCommand, "FromStake", DbType.Int32, fromStake);
                db.AddInParameter(dbCommand, "ToStake", DbType.Int32, toStake);
                db.AddInParameter(dbCommand, "excludeTest", DbType.Int32, excludeTest);

                db.AddOutParameter(dbCommand, "TotalRows", DbType.Int32, 10);
                dbCommand.CommandTimeout = 6000;
                var data = db.ExecuteReader(dbCommand);
                int resultCount = 0;
                if (data != null)
                {
                    var IsAdminCanViewVNIP = (WebSettings.IPVNReplace == "1") ? true : AppSession.ViewIPVN;

                    while (data.Read())
                    {
                        var bettran = new Bettran()
                        {
                            Transid = CastAide.AsString(data["transid"]),
                            HomeId = CastAide.AsString(data["homeid"]),
                            AwayId = CastAide.AsString(data["awayid"]),
                            Bettype = CastAide.AsInt32(data["bettype"]),
                            SportId = CastAide.AsString(data["sporttype"]),
                            Refno = CastAide.AsString(data["refno"]),
                            Matchid = CastAide.AsInt32(data["matchid"]),
                            Betid = CastAide.AsString(data["betid"]),
                            Hdp1 = CastAide.AsString(data["hdp1"]),
                            Hdp2 = CastAide.AsString(data["hdp2"]),
                            Odds = CastAide.AsString(data["odds"]),
                            OddsType = CastAide.AsInt32(data["OddsType"]),
                            Status = CastAide.AsString(data["status"]),
                            Stake = CastAide.AsString(data["stake"]),
                            actualStake = CastAide.AsString(data["Actual_Stake"]),
                            Transdate = CastAide.AsString(data["transdate"]),
                            Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(data["betteam"].ToString(), CastAide.AsInt32(data["bettype"]), "e"),
                            Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                            Modds = CastAide.AsString(data["modds"]),
                            Custid = CastAide.AsInt32(data["custid"]),
                            Livehomescore = CastAide.AsString(data["livehomescore"]),
                            Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                            LeagueId = CastAide.AsString(data["leagueid"]),
                            IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                            CurrencyName = CastAide.AsString(data["CurrencyName"]),
                            UserName = CastAide.AsString(data["UserName"]),
                            MatchCode = CastAide.AsString(data["matchcode"]),
                            Ip = CastAide.AsString(data["ip"]),
                            //CompanyWinloss = CastAide.AsString(data["companywinlost"]),
                            Winlost = CastAide.AsString(data["winlost"]),
                            Virtual = CastAide.AsString(data["AmtDueToBAVirtual"]),
                            Virtualrate = CastAide.AsString(data["virtualrate"]),
                            Real = CastAide.AsString(data["AmtDuetoBAReal"]),
                            Acomm = CastAide.AsString(data["comm"]),
                            Adiscount = CastAide.AsString(data["discount"])
                        };
                        var baBetlist = new BetListDetail()
                        {
                            Transid = bettran.Transid,
                            BettranDetail = bettran

                            //Name = reader.GetString(reader["Name"]
                        };
                        if (!IsAdminCanViewVNIP)
                        {
                            var ipmanager = new IpManager(IPVNList);
                            baBetlist.Ip = ipmanager.ReplaceVnIp(baBetlist.Ip, WebSettings.IPVNReplace);
                        }
                        betList.Add(baBetlist);
                    }
                    data.Close();
                    resultCount = Convert.ToInt32(db.GetParameterValue(dbCommand, "TotalRows"));
                }
                return new BABetList() { Items = betList, totalItem = resultCount };
            }
        }

        private string getSessionName(bool isMain, int Type, string Licensees, string bettypes)
        {
            var sessionName = "";
            sessionName = Type == 0 ? "L_" + Licensees : "B_" + bettypes;
            if (!isMain)
            {
                sessionName += Type == 0 ? "B_" + bettypes : "L_" + Licensees;
            }
            return sessionName;
        }

        public TicketsListingBetlistDTO GetTicketsListingBetlist(bool fromCache, bool isMain, int Type, int matchID, string Licensees, string bettypes, string bettypesLive, int groupBy, int Language, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            var ticketsListing = new TicketsListingBetlistDTO();
            var sessionName = getSessionName(isMain, groupBy, Licensees, bettypes);

            //var _ticketsListing = SessionExtension.GetSession(HttpContext.Current.Session, sessionName);
            var _ticketsListing = CacheExtension.Get(sessionName);

            if (!fromCache || _ticketsListing == null)
            {

                #region getDataFromDB
                //SessionExtension.AddSession(HttpContext.Current.Session, sessionName, null);
                CacheExtension.Delete(sessionName);
                ticketsListing.Items = new List<TicketsListingBetlist>();
                using (ObjectDb objDb = new ObjectDb("Acc_Rpt_TicketsListingDetail", 6000))
                {
                    var p = new Hashtable();
                    p.Add("matchid", matchID);
                    p.Add("licenseenames", Licensees);
                    p.Add("bettypes", bettypes);
                    p.Add("bettypeslive", bettypesLive);
                    p.Add("type", Type);
                    p.Add("livetype", DNAide.Core.CastAide.AsString(LiveType, "0"));
                    p.Add("safilter", SAFilter);
                    if (Type == 0)//sport book
                    {
                        if (HomeScore.Length > 0 && AwayScore.Length > 0)
                        {                               
                            p.Add("livehomescore", CastAide.AsInt32(HomeScore));
                            p.Add("liveawayscore", CastAide.AsInt32(AwayScore));

                        }
                        if (FromTransDate.Length > 0 && ToTransDate.Length > 0)
                        {                               
                            p.Add("fromtransdate", CastAide.AsDateTime(FromTransDate).AddHours(-12));
                            p.Add("totransdate", CastAide.AsDateTime(ToTransDate).AddHours(-12));
                        }

                    }

                    objDb.AssignParamValues(p);
                    var ds = objDb.ExecuteDataSet();
                    if (ds != null)
                    {
                        var dtBetTrans = ds.Tables[0];
                        var dtMBetTrans = ds.Tables[1];

                        #region none MP bettype
                        foreach (DataRow data in dtBetTrans.Rows)
                        {
                            var bettran = new Bettran
                            {
                                Transid = CastAide.AsString(data["transid"]),
                                Transdate = CastAide.AsString(data["transdate"]),
                                UserName = CastAide.AsString(data["UserName"]),
                                Odds = CastAide.AsString(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(data["odds"]), CastAide.AsInt32(data["bettype"]), CastAide.AsInt32(data["oddstype"]))),
                                Stake = CastAide.AsString(data["stake"]),
                                Winlost = CastAide.AsString(data["winlost"]),
                                CurrencyName = CastAide.AsString(data["CurrencyName"]),
                                Bettype = CastAide.AsInt32(data["bettype"]),
                                Status = CastAide.AsString(data["status"]),

                                HomeId = CastAide.AsString(data["homeid"]),
                                AwayId = CastAide.AsString(data["awayid"]),

                                Hdp1 = CastAide.AsString(data["hdp1"]),
                                Hdp2 = CastAide.AsString(data["hdp2"]),
                                Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(data["betteam"].ToString(), CastAide.AsInt32(data["bettype"]), "e"),
                                Betid = CastAide.AsString(data["betid"]),

                                Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                                Livehomescore = CastAide.AsString(data["livehomescore"]),
                                Liveawayscore = CastAide.AsString(data["liveawayscore"]),


                                IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                                SATicket = CastAide.AsInt32(data["secacceptflag"])

                            };

                            if (bettran.Bettype < 1201 || 1299 < bettran.Bettype)
                            {
                                bettran.OddsType = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(data["bettype"]), CastAide.AsInt32(data["oddstype"]));
                            }

                            var tkBetlist = new TicketsListingBetlist()
                            {
                                Transid = bettran.Transid,
                                BettranDetail = bettran,
                                MBettranDetail = new Bettran()

                            };
                            ticketsListing.Items.Add(tkBetlist);
                        }
                        #endregion

                        #region MP bettype
                        foreach (DataRow mdata in dtMBetTrans.Rows)
                        {
                            var bettran = new Bettran()
                            {
                                SystemParlaySelection_Bettran = CastAide.AsString(mdata["selection"]),
                                Transid = CastAide.AsString(mdata["transid"]),
                                Transdate = CastAide.AsString(mdata["transdate"]),
                                UserName = CastAide.AsString(mdata["UserName"]),
                                Odds = CastAide.AsString(mdata["odds"]),
                                Stake = CastAide.AsString(mdata["stake"]),
                                Winlost = CastAide.AsString(mdata["winlost"]),
                                CurrencyName = CastAide.AsString(mdata["CurrencyName"]),
                                Bettype = CastAide.AsInt32(mdata["bettype"]),
                                Status = CastAide.AsString(mdata["status"]),

                                HomeId = CastAide.AsString(mdata["homeid"]),
                                AwayId = CastAide.AsString(mdata["awayid"]),

                                Hdp1 = CastAide.AsString(mdata["hdp1"]),
                                Hdp2 = CastAide.AsString(mdata["hdp2"]),
                                Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(mdata["betteam"].ToString(), CastAide.AsInt32(mdata["bettype"]), "e"),

                                Liveindicator = CastAide.AsBoolean(mdata["liveindicator"]),
                                Livehomescore = CastAide.AsString(mdata["livehomescore"]),
                                Liveawayscore = CastAide.AsString(mdata["liveawayscore"]),
                                IsNeutral = CastAide.AsBoolean(mdata["isNeutral"]),
                                SATicket = CastAide.AsInt32(mdata["secacceptflag"])

                            };

                            if (bettran.Bettype < 1201 || 1299 < bettran.Bettype)
                            {
                                bettran.OddsType = CastAide.AsInt32(mdata["OddsType"]);
                            }

                            var mbetTrans = new Bettran
                            {
                                // SystemParlaySelection_Bettran = CastAide.AsString(mdata["selection"]),
                                Transid = CastAide.AsString(mdata["transid"]),
                                Transdate = CastAide.AsString(mdata["transdate"]),
                                Odds = CastAide.AsString(mdata["odds"]),

                                Bettype = CastAide.AsInt32(mdata["bettypem"]),

                                HomeId = CastAide.AsString(mdata["homeid"]),
                                AwayId = CastAide.AsString(mdata["awayid"]),
                                Hdp1 = CastAide.AsString(mdata["hdp1"]),
                                Hdp2 = CastAide.AsString(mdata["hdp2"]),
                                Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(mdata["betteam"].ToString(), CastAide.AsInt32(mdata["bettype"]), "e"),
                                Liveindicator = CastAide.AsBoolean(mdata["liveindicator"]),
                                Livehomescore = CastAide.AsString(mdata["livehomescore"]),
                                Liveawayscore = CastAide.AsString(mdata["liveawayscore"]),
                                Modds = CastAide.AsString(mdata["oddsm"]),
                                IsNeutral = CastAide.AsBoolean(mdata["isNeutral"])

                            };

                            if (bettran.Bettype < 1201 || 1299 < bettran.Bettype)
                            {
                                bettran.OddsType = CastAide.AsInt32(mdata["OddsType"]);
                            }

                            var tkBetlist = new TicketsListingBetlist()
                            {
                                Transid = bettran.Transid,
                                BettranDetail = bettran,
                                MBettranDetail = mbetTrans

                            };
                            ticketsListing.Items.Add(tkBetlist);
                        }
                        #endregion
                    }
                    ticketsListing.totalItem = ticketsListing.Items.Count;
                }
                #endregion
                //SessionExtension.AddSession(HttpContext.Current.Session, sessionName, ticketsListing);
                CacheExtension.Set(sessionName, ticketsListing, DateTime.Now.AddMinutes(30));
            }
            else
            {
                ticketsListing = (TicketsListingBetlistDTO)_ticketsListing;
            }
            return ticketsListing;
        }

        public BABetList GetBABetListByTransId(string refno, List<IPVNBean> IPVNList)
        {
            var betList = new List<BetListDetail>();
            using (ObjectDb db = new ObjectDb("BABetlistByTransID"))
            {
                Hashtable param = new Hashtable();
                param.Add("transid", refno);

                db.AssignParamValues(param);

                var data = db.ExecuteReader();
                
                int resultCount = 0;
                if (data != null)
                {
                    var IsAdminCanViewVNIP = (WebSettings.IPVNReplace == "1") ? true : AppSession.ViewIPVN;

                    while (data.Read())
                    {
                        var bettran = new Bettran()
                        {
                            Transid = CastAide.AsString(data["transid"]),
                            HomeId = CastAide.AsString(data["homeid"]),
                            AwayId = CastAide.AsString(data["awayid"]),
                            Bettype = CastAide.AsInt32(data["bettype"]),
                            SportId = CastAide.AsString(data["sporttype"]),
                            Refno = CastAide.AsString(data["refno"]),
                            Matchid = CastAide.AsInt32(data["matchid"]),
                            Betid = CastAide.AsString(data["betid"]),
                            Hdp1 = CastAide.AsString(data["hdp1"]),
                            Hdp2 = CastAide.AsString(data["hdp2"]),
                            Odds = CastAide.AsString(data["odds"]),
                            OddsType = CastAide.AsInt32(data["OddsType"]),
                            Status = CastAide.AsString(data["status"]),
                            Stake = CastAide.AsString(data["stake"]),
                            Transdate = CastAide.AsString(data["transdate"]),
                            Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(data["betteam"].ToString(), CastAide.AsInt32(data["bettype"]), "e"),
                            Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                            Modds = CastAide.AsString(data["modds"]),
                            Custid = CastAide.AsInt32(data["custid"]),
                            Livehomescore = CastAide.AsString(data["livehomescore"]),
                            Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                            LeagueId = CastAide.AsString(data["leagueid"]),
                            IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                            CurrencyName = CastAide.AsString(data["CurrencyName"]),
                            UserName = CastAide.AsString(data["UserName"]),
                            MatchCode = CastAide.AsString(data["matchcode"]),
                            Ip = CastAide.AsString(data["ip"]),
                            //CompanyWinloss = CastAide.AsString(data["companywinlost"]),
                            Winlost = CastAide.AsString(data["winlost"]),
                            Virtual = CastAide.AsString(data["AmtDueToBAVirtual"]),
                            Virtualrate = CastAide.AsString(data["virtualrate"]),
                            Real = CastAide.AsString(data["AmtDuetoBAReal"]),
                            Acomm = CastAide.AsString(data["comm"]),
                            Adiscount = CastAide.AsString(data["discount"])
                        };
                        var baBetlist = new BetListDetail()
                        {
                            Transid = bettran.Transid,
                            BettranDetail = bettran

                            //Name = reader.GetString(reader["Name"]
                        };
                        if (!IsAdminCanViewVNIP)
                        {
                            var ipmanager = new IpManager(IPVNList);
                            baBetlist.Ip = ipmanager.ReplaceVnIp(baBetlist.Ip, WebSettings.IPVNReplace);
                        }
                        betList.Add(baBetlist);
                    }
                    data.Close();
                    //resultCount = (int)db.GetParameterValue(dbCommand, "TotalRows");
                    resultCount = betList.Count;
                }
                return new BABetList() { Items = betList, totalItem = resultCount };
            }
        }

        public List<BetListDetail> GetMixParlayTickets(string refNo)
        {
            var betList = new List<BetListDetail>();           
            using (ObjectDb objDb = new ObjectDb("Acc_BetsSelBetCheckM_All"))
            {
                objDb.AssignParamValues(new Hashtable() { { "refno", refNo } });
                
                var listMpTicket = new List<BetListDetail>();
                var data = objDb.ExecuteReader();
                if (data != null)
                {
                    while (data.Read())
                    {
                        var bettran = new Bettran()
                        {
                            Transid = CastAide.AsString(data["transid"]),
                            HomeId = CastAide.AsString(data["homeid"]),
                            AwayId = CastAide.AsString(data["awayid"]),
                            Bettype = CastAide.AsInt32(data["bettype"]),
                            SportId = CastAide.AsString(data["sporttype"]),
                            MatchCode = CastAide.AsString(data["matchcode"]),
                            Matchid = CastAide.AsInt32(data["matchid"]),
                            Hdp1 = CastAide.AsString(data["hdp1"]),
                            Hdp2 = CastAide.AsString(data["hdp2"]),
                            Odds = CastAide.AsString(data["odds"]),
                            Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(data["betteam"].ToString(), CastAide.AsInt32(data["bettype"]), "e"),
                            Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                            Livehomescore = CastAide.AsString(data["livehomescore"]),
                            Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                            LeagueId = CastAide.AsString(data["leagueid"]),
                            StatusId = CastAide.AsInt32(data["statusId"]),

                            IsNeutral = CastAide.AsBoolean(data["isNeutral"])
                        };

                        var betListDetail = new BetListDetail()
                        {
                            Transid = bettran.Transid,
                            BettranDetail = bettran

                            //Name = reader.GetString(reader["Name"]
                        };
                        listMpTicket.Add(betListDetail);
                    }
                    data.Close();
                }
                return listMpTicket;
            }
        }

        #region IBetListDAO Members

        public DataTable GetBettype()
        {
            var dataTable = new DataTable();
            try
            {
                
                using (ObjectDb db = new ObjectDb("GetBettype", 600))
                {
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dataTable = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return dataTable;
        }

        public DataTable GetCurrency()
        {
            var dataTable = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("GetCurrency"))
                {
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dataTable = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dataTable;
        }

        #endregion

        public List<CombinationDetalDTO> GenDetailMP(string serial, DataTable dataTable)
        {
            var tableDetailMP = new List<CombinationDetalDTO>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (CastAide.AsString(row["serial"]).Trim() == serial)
                {
                    var betlistDto = new CombinationDetalDTO();
                    betlistDto.AwayName = SportBookUtils.GetTeamName(CastAide.AsString(row["awayid"]));
                    betlistDto.HomeName = SportBookUtils.GetTeamName(CastAide.AsString(row["homeid"]));
                    tableDetailMP.Add(betlistDto);
                }

            }
            return tableDetailMP;
        }

        public CombinationDetailMP GetCombinationDetailMP(string refNo)
        {            
            using (ObjectDb db = new ObjectDb("Acc_BetlistCombinationParlay_Details"))
            {
                var combinationDetailMP = new CombinationDetailMP();
                double totalWLComm = 0.00;
                var dataTable = new DataTable();
                var dataTable1 = new DataTable();
                var combinationMP = new List<BetListDetail>();

                var param = new Hashtable { { "refno", refNo }, { "outstanding", 0 } };
                db.AssignParamValues(param);
                var dbResult = db.ExecuteDataSet();

                if (dbResult != null && dbResult.Tables.Count > 0)
                {
                    dataTable = dbResult.Tables[0];
                    dataTable1 = dbResult.Tables[1];
                    var outstanding = CastAide.AsDecimal(db.GetParameterValue("outstanding"));
                    combinationDetailMP.outStanding = FormatHandler.FormatNumber2(outstanding, "en-EN");
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    var betlistDetail = new BetListDetail();
                    List<CombinationDetalDTO> combinationdetail = GenDetailMP(CastAide.AsString(row["serial"]), dataTable1);
                    var bettran = new Bettran
                    {
                        Transdate = CastAide.AsString(row["winlostdate"]),
                        Status = CastAide.AsString(row["status"]),
                        Stake = CastAide.AsString(row["stake"]),
                        Winlost = CastAide.AsString(row["winlost"]),
                        Odds = CastAide.AsString(row["modds"])
                    };
                    betlistDetail.BettranDetail = bettran;
                    betlistDetail.CombinationDetails = combinationdetail;
                    combinationMP.Add(betlistDetail);
                    totalWLComm += CastAide.AsDouble(row["winlost"]);

                }
                combinationDetailMP.TotalWLComm = FormatHandler.FormatNumber2(totalWLComm, "en-EN");
                combinationDetailMP.items = combinationMP;
                return combinationDetailMP;
            }
        }

        public DataSet GetBetListSystemParlayByTransId(string transId, int custId, DateTime moveBetDay, DateTime balaceUpDay)
        {
            DataSet result = null;           

            using (ObjectDb db = new ObjectDb("Acc_BetlistCombinationParlay_ByTransID", 180))
            {
                var param = new Hashtable { { "transid", transId }, { "custid", custId }, { "movebetday", moveBetDay }, { "balanceupday", balaceUpDay } };
                db.AssignParamValues(param);
                result = db.ExecuteDataSet();
            }

            return result;

        }
    }
}
