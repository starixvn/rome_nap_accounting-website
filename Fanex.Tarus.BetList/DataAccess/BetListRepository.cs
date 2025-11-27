namespace Fanex.Tarus.BetList.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Constants;
    using Criteria;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.BetList.Entities.BetList;
    using Fanex.Tarus.Infrastructure.Helper;
    using Models;
    using Resources;
    using Sunplus.Accounting.Core.Extensions;

    public class BetListRepository : IBetListRepository
    {
        public IList<ProductEntity> GetProductList(SelectProductCriteria criteria)
        {
            var entities = new List<ProductEntity>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        entities.Add(new ProductEntity
                        {
                            ProductID = DataUtils.ToInt16(reader["ProductID"]),
                            ProductName = DataUtils.ToString(reader["ProductName"]),                            
                            Enabled = DataUtils.ToBoolean(reader["Enabled"]),                            
                            GroupType = DataUtils.ToInt16(reader["GroupType"]),
                            CasinoType = DataUtils.ToInt16(reader["CasinoType"])                            
                        });
                    }
                }

                return entities;
            }
        }

        public IList<BetType> GetBetTypeByDate(BetTypeByDateCriteria criteria)
        {
            var betTypeList = new List<BetType>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria);
                if (dataSet != null &&
                    dataSet.Tables.Count > 0)
                {
                    foreach (DataRow r in dataSet.Tables[0].Rows)
                    {
                        betTypeList.Add(new BetType
                        {
                            BetTypeID = Converter.To<int>(r["typeid"]),
                            BetTypeName = r["typenamee"].ToString()
                        });
                    }
                }
            }

            return betTypeList;
        }

        public IList<BetType> GetBetTypeByDate(BetTypeByDateInSiteDBCriteria criteria, string AGIP)
        {
            var betTypeList = new List<BetType>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSetScaleoutDB(criteria.GetSpParams(), AGIP);
                if (dataSet != null &&
                    dataSet.Tables.Count > 0)
                {
                    foreach (DataRow r in dataSet.Tables[0].Rows)
                    {
                        betTypeList.Add(new BetType
                        {
                            BetTypeID = Converter.To<int>(r["typeid"]),
                            BetTypeName = r["typenamee"].ToString()
                        });
                    }
                }
            }

            return betTypeList;
        }

        /// <summary>
        /// Fetches the member win loss.
        /// </summary>
        /// <typeparam name="S">The parameter type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="mapObjectFunction">The map object function.</param>
        /// <param name="outputList">The output list.</param>
        /// <returns>The member win loss.</returns>
        public IList<S> FetchMemberWinLoss<S>(ICriteria criteria, Func<DataRow, S> mapObjectFunction, ref List<ITicketData> outputList) where S : class
        {
            IList<S> listData = new List<S>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        DataTable secondTable = dataSet.Tables[1];
                        foreach (DataRow row in secondTable.Rows)
                        {
                            outputList.Add(new TicketData
                            {
                                RefNo = Converter.ToString2(row["refno"]),
                                BetTypeId = Converter.To<int>(row["bettype"]),
                                Status = Converter.ToString2(row["status"]),
                                Odds = Converter.To<decimal>(row["odds"]),
                                Stake = Converter.To<decimal>(row["stake"]),
                                BetTeam = Converter.ToString2(row["betteam"]),
                            });
                        }
                    }
                }
            }

            return listData;
        }

        /// <summary>
        /// Fetches the specified data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="baseCurrency">The base currency.</param>
        /// <returns>The specified data.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<TicketDetail> Fetch(dynamic criteria, CurrencyContants baseCurrency)
        {
            string winlostCol = "Winlost";
            string awinlostCol = "LicenseeWL";
            string stakeCol = "Stake";
            string actualstakeCol = "Actual_Stake";
            string commCol = "comm";
            string acommCol = "playercomm";

            switch (baseCurrency)
            {
                case CurrencyContants.EUR:
                    winlostCol = "WinlostEUR";
                    awinlostCol = "LicenseeWLEUR";
                    stakeCol = "StakeEUR";
                    commCol = "commEUR";
                    acommCol = "PlayercommEUR";
                    actualstakeCol = "actual_stakeEUR";
                    break;

                case CurrencyContants.MYR:
                    winlostCol = "WinlostMYR";
                    awinlostCol = "LicenseeWLMYR";
                    stakeCol = "StakeMYR";
                    commCol = "commMYR";
                    acommCol = "PlayerCommMYR";
                    actualstakeCol = "actual_stakeMYR";
                    break;

                case CurrencyContants.USD:
                    winlostCol = "WinlostUSD";
                    awinlostCol = "LicenseeWLUSD";
                    stakeCol = "StakeUSD";
                    commCol = "commUSD";
                    acommCol = "playercommUSD";
                    actualstakeCol = "actual_stakeUSD";
                    break;
            }

            IList<TicketDetail> listData = new List<TicketDetail>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria);
                if (dataSet != null &&
                    dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow r in currentTable.Rows)
                    {
                        listData.Add(new TicketDetail
                        {
                            Ticket = new Ticket
                            {
                                SportTypeId = Converter.ToInt(r["Sporttype"]),
                                TransId = Converter.To<long>(r["TransID"]),
                                BetId = Converter.To<long>(r["BetID"]),
                                BetTypeId = Converter.ToInt(r["Bettype"]),
                                Handicap1 = Converter.ToDecimal(r["hdp1"]),
                                Handicap2 = Converter.ToDecimal(r["hdp2"]),
                                MasterOdds = Converter.ToDecimal(r["Modds"]),
                                OddsType = Converter.ToString2(r["OddsType"]),
                                Odds = Converter.ToDecimal(r["Odds"]),
                                Stake = (int)Converter.ToDecimal(r[stakeCol]),
                                ActualStake = (int)Converter.ToInt(r[actualstakeCol]),
                                BetTeam = Converter.ToString2(r["Betteam"]),
                                Status = Converter.ToString2(r["Status"]),
                                RefNo = Converter.ToString2(r["RefNO"]),
                                TransDate = Converter.ToDateTime(r["Transdate"]),
                                WinlostDate = Converter.ToDateTime(r["Winlostdate"]),
                                Winlost = Converter.ToDecimal(r[winlostCol]),
                                Comm = Converter.ToDecimal(r[commCol]),
                                PlayerComm = Converter.ToDecimal(r[acommCol]),
                                IsLive = Converter.ToBool(r["Liveindicator"]),
                                LiveHomeScore = Converter.ToInt(r["Livehomescore"]),
                                LiveAwayScore = Converter.ToInt(r["Liveawayscore"]),
                                IP = Converter.ToString2(r["IP"]),
                                LeagueId = Converter.ToInt(r["LeagueID"]),
                                MatchId = Converter.ToInt(r["MatchID"]),
                                MatchCode = Converter.ToString2(r["MatchCode"]),
                                Race = Converter.ToString2(r["Race"]),
                                HomeId = Converter.To<long>(r["HomeID"]),
                                AwayId = Converter.To<long>(r["AwayID"]),
                                IsNeutral = Converter.ToBool(r["isNeutral"]),
                                ShowTime = Converter.ToString2(r["showtime"]),
                                AgentWinlost = Converter.ToDecimal(r[awinlostCol])
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty
                        });
                    }
                }
            }

            return listData;
        }

        /// <summary>
        /// Fetches the specified data.
        /// </summary>
        /// <typeparam name="S">The parameter type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="mapObjectFunction">The map object function.</param>
        /// <param name="outputParams">The output parameters.</param>
        /// <returns>The specified data.</returns>
        public IList<S> Fetch<S>(
            dynamic criteria,
            Func<DataRow, S> mapObjectFunction,
            ref Hashtable outputParams) where S : class
        {
            IList<S> listData = new List<S>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria);
                outputParams = db.GetOuputParams();
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }
                }
            }

            return listData;
        }

        /// <summary>
        /// Fetches the specified data.
        /// </summary>
        /// <typeparam name="S">The parameter type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="mapObjectFunction">The map object function.</param>
        /// <returns>The specified data.</returns>
        public IList<S> Fetch<S>(dynamic criteria, Func<DataRow, S> mapObjectFunction) where S : class
        {
            IList<S> listData = new List<S>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria);
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }
                }
            }

            return listData;
        }

        public IList<S> Fetch<S>(dynamic criteria, string AGIP, Func<DataRow, S> mapObjectFunction) where S : class
        {
            IList<S> listData = new List<S>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable hash = criteria.GetSpParams();
                DataSet dataSet = db.ExecuteDataSetScaleoutDB(hash, AGIP);
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }
                }
            }

            return listData;
        }

        /// <summary>
        /// Fetches the specified data.
        /// </summary>
        /// <typeparam name="S">The parameter type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="mapObjectFunction">The map object function.</param>
        /// <param name="outputList">The output list.</param>
        /// <returns>The specified data.</returns>
        public IList<S> Fetch<S>(dynamic criteria, Func<DataRow, S> mapObjectFunction, ref List<ITicketData> outputList) where S : class
        {
            IList<S> listData = new List<S>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria);
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        DataTable secondTable = dataSet.Tables[1];
                        foreach (DataRow row in secondTable.Rows)
                        {
                            outputList.Add(new TicketData
                            {
                                RefNo = Converter.ToString2(row["refno"]),
                                BetTypeId = Converter.To<int>(row["bettype"]),
                                Status = Converter.ToString2(row["status"]),
                                Odds = Converter.To<decimal>(row["odds"]),
                                Stake = Converter.To<decimal>(row["stake"]),
                                BetTeam = Converter.ToString2(row["betteam"]),
                                TransDesc = Converter.ToString2(row["TransDesc"])
                            });
                        }
                    }
                }
            }

            return listData;
        }

        public IList<S> Fetch<S>(dynamic criteria, string AGIP, Func<DataRow, S> mapObjectFunction, ref List<ITicketData> outputList) where S : class
        {
            IList<S> listData = new List<S>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable param = criteria.GetSpParams();
                DataSet dataSet = db.ExecuteDataSetScaleoutDB(param, AGIP);
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        DataTable secondTable = dataSet.Tables[1];
                        foreach (DataRow row in secondTable.Rows)
                        {
                            outputList.Add(new TicketData
                            {
                                RefNo = Converter.ToString2(row["refno"]),
                                BetTypeId = Converter.To<int>(row["bettype"]),
                                Status = Converter.ToString2(row["status"]),
                                Odds = Converter.To<decimal>(row["odds"]),
                                Stake = Converter.To<decimal>(row["stake"]),
                                BetTeam = Converter.ToString2(row["betteam"]),
                                TransDesc = Converter.ToString2(row["TransDesc"])
                            });
                        }
                    }
                }
            }

            return listData;
        }

        public KeyValuePair<dynamic, IList<S>> FetchWithOutputs<S>(dynamic criteria, Func<DataRow, S> mapObjectFunction, ref List<ITicketData> outputList) where S : class
        {
            IList<S> listData = new List<S>();
            int errCode = 0, maxTotalRow = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        DataTable secondTable = dataSet.Tables[1];
                        foreach (DataRow row in secondTable.Rows)
                        {
                            outputList.Add(new TicketData
                            {
                                RefNo = Converter.ToString2(row["refno"]),
                                BetTypeId = Converter.To<int>(row["bettype"]),
                                Status = Converter.ToString2(row["status"]),
                                Odds = Converter.To<decimal>(row["odds"]),
                                Stake = Converter.To<decimal>(row["stake"]),
                                BetTeam = Converter.ToString2(row["betteam"]),
                                TransDesc = Converter.ToString2(row["TransDesc"])
                            });
                        }
                    }
                }

                errCode = (int)db.GetOuputParams()["errcode"];
                maxTotalRow = (int)db.GetOuputParams()["maxtotalrow"];
            }

            return new KeyValuePair<dynamic, IList<S>>(new { errCode, maxTotalRow }, listData);
        }

        public KeyValuePair<dynamic, IList<S>> FetchWithOutputs<S>(dynamic criteria, string AGIP, Func<DataRow, S> mapObjectFunction, ref List<ITicketData> outputList) where S : class
        {
            IList<S> listData = new List<S>();
            int errCode = 0, maxTotalRow = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable param = criteria.GetSpParams();
                DataSet dataSet = db.ExecuteDataSetScaleoutDB(param, AGIP);
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        DataTable secondTable = dataSet.Tables[1];
                        foreach (DataRow row in secondTable.Rows)
                        {
                            outputList.Add(new TicketData
                            {
                                RefNo = Converter.ToString2(row["refno"]),
                                BetTypeId = Converter.To<int>(row["bettype"]),
                                Status = Converter.ToString2(row["status"]),
                                Odds = Converter.To<decimal>(row["odds"]),
                                Stake = Converter.To<decimal>(row["stake"]),
                                BetTeam = Converter.ToString2(row["betteam"]),
                                TransDesc = Converter.ToString2(row["TransDesc"])
                            });
                        }
                    }
                }

                errCode = (int)db.GetOuputParams()["errcode"];
                maxTotalRow = (int)db.GetOuputParams()["maxtotalrow"];
            }

            return new KeyValuePair<dynamic, IList<S>>(new { errCode, maxTotalRow }, listData);
        }

        public SystemParlayModel FetchSystemParlay(BetListSystemParlayByRefnoCriteria criteria)
        {
            var systemParlayModel = new SystemParlayModel();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var dataSet = db.ExecuteDataSetScaleoutDB(criteria.GetSpParams(), criteria.CustomerId);
                if (dataSet != null &&
                    dataSet.Tables.Count > 0)
                {
                    var ticketDetailTable = dataSet.Tables[0];
                    foreach (DataRow r in ticketDetailTable.Rows)
                    {
                        systemParlayModel.TicketsDetailList.Add(new TicketDetail
                        {
                            Ticket = new Ticket
                            {
                                TransId = Converter.To<long>(r["transid"]),
                                TransDate = Converter.ToDateTime(r["transdate"]),
                                RefNo = Converter.ToString2(r["refno"]),
                                WinlostDate = Convert.ToDateTime(r["transdate"]),
                                Odds = Converter.ToDecimal(r["odds"]),
                                OddsSpread = Converter.ToDecimal(r["oddsspread"]),
                                MasterOdds = Converter.ToDecimal(r["modds"]),
                                Stake = Converter.To<decimal>(r["stake"]),
                                ActualStake = Converter.To<decimal>(r["actual_stake"]),
                                Winlost = Converter.ToDecimal(r["winlost"]),
                                PlayerComm = Converter.ToDecimal(r["playercomm"]),
                                Status = Converter.ToString2(r["status"]),

                                CommStatus = Converter.To<string>(r["comstatus"]),
                                BetTeam = Converter.ToString2(r["betteam"]),
                                IP = Converter.ToString2(r["ip"]),
                                SuperPositionTaking = Converter.ToDecimal(r["spositiontaking"]),
                                MasterPositionTaking = Converter.ToDecimal(r["mpositiontaking"]),
                                AgentPositionTaking = Converter.ToDecimal(r["apositiontaking"]),
                                SuperWinlost = Converter.ToDecimal(r["swinlost"]),
                                MasterWinlost = Converter.ToDecimal(r["mwinlost"]),
                                AgentWinlost = Converter.ToDecimal(r["awinlost"]),
                                SuperDiscount = Converter.ToDecimal(r["sdiscount"]),
                                MasterDiscount = Converter.ToDecimal(r["adiscount"]),
                                AgentDiscount = Converter.ToDecimal(r["discount"]),
                                SuperComm = Converter.ToDecimal(r["scomm"]),
                                AgentComm = Converter.ToDecimal(r["acomm"]),
                                Comm = Converter.ToDecimal(r["comm"]),
                                BetTypeId = 29,
                                Handicap1 = Converter.ToDecimal(r["hdp1"]),
                                Handicap2 = Converter.ToDecimal(r["hdp2"]),
                                BetCheck = Converter.ToString2(r["BetCheck"]),
                                ExtraInfo = Converter.ToString2(r["ExtraInfo"]),
                                TransDesc = Converter.ToString2(r["transdesc"])
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty,
                            DeviceType = r.Table.Columns.Contains("DeviceType") && !r.IsNull("DeviceType") ? Converter.To<int>(r["DeviceType"]) : (int?)null
                        });
                    }

                    var choiceTable = dataSet.Tables[1];
                    foreach (DataRow r in choiceTable.Rows)
                    {
                        systemParlayModel.SystemParlayChoice.Add(new TicketDetail
                        {
                            Ticket = new Ticket
                            {
                                TransId = Converter.To<long>(r["transid"]),
                                RefNo = Converter.ToString2(r["refno"]),
                                WinlostDate = Converter.ToDateTime(r["winlostdate"]),
                                TransDate = Converter.ToDateTime(r["transdate"]),
                                BetTypeId = Converter.To<int>(r["bettype"]),
                                HomeId = Converter.To<long>(r["homeid"]),
                                AwayId = Converter.To<long>(r["awayid"]),
                                Handicap1 = Converter.ToDecimal(r["hdp1"]),
                                Handicap2 = Converter.ToDecimal(r["hdp2"]),
                                Odds = Converter.ToDecimal(r["odds"]),
                                BetTeam = Converter.ToString2(r["betteam"]),
                                LiveHomeScore = Converter.To<int>(r["livehomescore"]),
                                LiveAwayScore = Converter.To<int>(r["liveawayscore"]),
                                LeagueId = Converter.To<int>(r["leagueid"]),
                                SportTypeId = Converter.To<int>(r["sporttype"]),
                                IsNeutral = Converter.ToBool(r["isNeutral"]),
                                MatchCode = Converter.ToString2(r["matchcode"]),
                                MatchId = Converter.To<int>(r["matchid"]),
                                BetCheck = Converter.ToString2(r["BetCheck"]),
                                TransDesc = Converter.ToString2(r["transdesc"]),
                                Stake = Converter.ToDecimal(r["Stake"]),
                                Currency = Converter.ToString2(r["Currency"]),
                                IsLive = Converter.ToBool(r["liveindicator"]),
                                EventDate = Converter.ToDateTime(r["eventdate"]),
                                Status = Converter.ToString2(r["status"]),
                                SettlementStatus = r.Table.Columns.Contains("SettlementStatus") ? Convert.ToByte(r["SettlementStatus"]) : (byte)0
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty
                        });
                    }

                    var eventTable = dataSet.Tables[2];

                    var showAlternateLink = eventTable.Rows.Count > SystemParlayDetail.MaxMatchRowsToShow;
                    if (showAlternateLink)
                    {
                        systemParlayModel.SystemParlayDetail.ShowAlternateLink = true;
                    }
                    else
                    {
                        var matchTable = dataSet.Tables[3];
                        foreach (DataRow r in eventTable.Rows)
                        {
                            var serial = Converter.To<int>(r["serial"]);
                            var matchList = new List<Match>();
                            foreach (DataRow row in matchTable.Rows)
                            {
                                if (serial == Converter.To<int>(row["serial"]))
                                {
                                    matchList.Add(new Match
                                    {
                                        HomeID = Converter.To<int>(row["homeid"]),
                                        AwayID = Converter.To<int>(row["awayid"]),
                                        IsNeutral = Converter.To<bool>(row["isneutral"])
                                    });
                                }
                            }

                            systemParlayModel.SystemParlayDetail.EventList.Add(new Event
                            {
                                WinLossDate = Converter.ToDateTime(r["winlostdate"]),
                                Odds = Converter.To<decimal>(r["odds"]),
                                Stake = Converter.To<decimal>(r["stake"]),
                                WinLoss = Converter.To<decimal>(r["winlost"]),
                                Status = r["status"].ToString(),
                                MatchList = matchList,
                                MasterOdds = Converter.To<decimal>(r["modds"]),
                                BoostRate = Converter.To<decimal>(r["extravalue"]),
                            });

                            systemParlayModel.SystemParlayDetail.SubTotal += Converter.To<decimal>(r["winlost"]);
                        }
                    }

                    var outputParams = db.GetOuputParams();
                    systemParlayModel.IsParlayBoost = Converter.ToBool(outputParams["isparlayboost"]);
                }
            }

            return systemParlayModel;
        }

        /// <summary>
        /// Bets the list select detail combination mix parlay.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list select detail combination mix parlay.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public CombinationParlay BetList_SelDetailCombinationMP(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                ITicketHelper ticketHelper = new TicketHelper();
                DataSet dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    DataTable dataTableMaster = dataSet.Tables[0];
                    DataTable dataTableDetail = dataSet.Tables[1];

                    CombinationParlay result = new CombinationParlay
                    {
                        GroupParlayEvents = new List<GroupParlayEvent>(),
                        OutStanding = Converter.ToDecimal(db.GetParameterValue("outstanding")),
                        Total = new ParlayEvent()
                    };

                    var winlostGroups = dataTableMaster.Rows.Cast<DataRow>()
                        .GroupBy(d => d["winlostdate"]);
                    foreach (var winlostGroup in winlostGroups)
                    {
                        GroupParlayEvent groupParlayEvent = new GroupParlayEvent
                        {
                            WinLossDate = Converter.ToDateTime(winlostGroup.Key),
                            ParlayEvents = new List<ParlayEvent>(),
                            SubTotal = new ParlayEvent()
                        };

                        foreach (var item in winlostGroup)
                        {
                            decimal winlost = Converter.ToDecimal(item["winlost"]);
                            decimal playercomm = Converter.ToDecimal(item["playercomm"]);

                            // calculate subtotal and total
                            groupParlayEvent.SubTotal.WinLoss += winlost + playercomm;
                            result.Total.WinLoss += winlost + playercomm;

                            // WIN:  serial != 0
                            // LOSE: serial == 0
                            int serial = Converter.ToInt(item["serial"]);

                            ParlayEvent parlayEvent = new ParlayEvent
                            {
                                Odds = Converter.ToDecimal(item["modds"]),
                                Stake = Converter.ToDecimal(item["stake"]),
                                WinLoss = winlost,
                                Comm = playercomm,
                                Status = Converter.ToString2(item["status"]).ToUpper()
                            };
                            groupParlayEvent.ParlayEvents.Add(parlayEvent);

                            // get Home -vs- Away
                            if (serial == 0)
                            {
                                // LOSE
                                parlayEvent.MatchList.Add(new Match
                                {
                                    Event = BetListResources.Total_of_losing_tickets
                                });
                            }
                            else
                            {
                                DataRow[] drowDetails = dataTableDetail.Select("serial = " + serial);
                                if (drowDetails != null)
                                {
                                    Match match;
                                    foreach (var dr in drowDetails)
                                    {
                                        match = new Match
                                        {
                                            HomeID = Converter.ToInt(dr["homeid"]),
                                            AwayID = Converter.ToInt(dr["awayid"])
                                        };
                                        string homeName = ticketHelper.GetTeamNameById(match.HomeID);
                                        string awayName = ticketHelper.GetTeamNameById(match.AwayID);
                                        match.Event = homeName + " -vs- " + awayName;

                                        parlayEvent.MatchList.Add(match);
                                    }
                                }
                            }
                        }

                        result.GroupParlayEvents.Add(groupParlayEvent);
                    }

                    return result;
                }
            }

            return null;
        }

        public string GetSiteNameByCustId(ICriteria criteria)
        {
            string siteName = string.Empty;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        siteName = reader["sitename"].ToString();
                    }
                }
            }

            return siteName;
        }

        public int MapPlayerIdToCustId(MapPlayerId3rdToCustIdCriteria criteria)
        {
            var custId = 0;

            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputs = db.GetOuputParams();
                custId = Converter.ToInt(outputs["custid"]);
            }

            return custId;
        }

        public IEnumerable<SportBettypeMatrixModel> SelectSportBettypeMatrix()
        {
            var criteria = new GetSportBettypeMatrixCriteria();
            var seen = new HashSet<(int SportType, int BetType)>();
            var result = new List<SportBettypeMatrixModel>();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    int sportType = Converter.To<int>(reader["SportType"]);
                    int betType = Converter.To<int>(reader["BetType"]);
                    var key = (sportType, betType);

                    if (seen.Add(key))
                    {
                        result.Add(new SportBettypeMatrixModel
                        {
                            SportType = sportType,
                            BetType = betType,
                            ProductId = Converter.To<int>(reader["ProductID"])
                        });
                    }
                }

                reader.Close();
            }

            return result;
        }
    }
}