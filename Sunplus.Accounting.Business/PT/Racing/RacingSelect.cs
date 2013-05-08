using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNAide.Core;

using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;

using Sunplus.Accounting.Entities.PositionTaking.Racing;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Business.CacheManager;

namespace Sunplus.Accounting.Business.PT.Racing
{
    public partial class RacingManager
    {
        #region Get Cust Info
        public Setting GetStatusRacebook(int custId)
        {
            var setting = new Setting();
            DataTable data = null;
            try
            {
                _roleId = 0;
                _custId = custId;
                data = _racingDao.GetRacingStatus(custId);
                if (data != null)
                {
                    setting = GetStatusRaceBook(data, custId);
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return setting;
        }
        public Setting GetCustomerSetting(int custId, int roleId)
        {
            var setting = new Setting();
            try
            {
                // check permission
                var hostPermission = Permissions.HasPermission(Enumerations.Functions.Customer.Customer);
                if (!hostPermission)
                {
                    return setting;
                }

                var data = _racingDao.GetRacingSetting(custId, roleId);
                if (data != null)
                {
                    _roleId = roleId;
                    _custId = custId;
                    switch (_roleId)
                    {
                        case (int)CustInfo.Roles.Super:
                            setting = GetSuperSetting(data);
                            break;

                        case (int)CustInfo.Roles.Master:
                            setting = GetMasterSetting(data);
                            break;

                        case (int)CustInfo.Roles.Agent:
                            setting = GetAgentSetting(data);
                            break;

                        case (int)CustInfo.Roles.Member:
                            setting = GetMemberSetting(data);
                            break;
                        case (int)CustInfo.Roles.SuperOnMember: //*******Super on Member params*****
                            setting = SuperOnMemberSetting(data);
                            break;
                        case (int)CustInfo.Roles.MasterOnMember: //*******Master on Member params*****
                            setting = MasterOnMemberSetting(data);
                            break;
                        case (int)CustInfo.Roles.SuperMin: //*******Super Min params*****
                            setting = GetSuperMinPt(data);
                            break;
                        case (int)CustInfo.Roles.MasterMinMax: //*******Master Min/Max params*****
                            setting = GetMasterMinMaxPt(data);
                            break;
                        case (int)CustInfo.Roles.AgentMinMax: //*******Agent Min/Max params*****
                            setting = GetAgentMinMaxPt(data);
                            break;
                        default:
                            break;
                    }
                    setting.IsHideCopyBetSetting = RacingSection.Instance.HideCopyBetSetting;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return setting;
        }

        private static Setting GetStatusRaceBook(System.Data.DataTable settingData, int custId)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];
            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);
            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection)
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    Parallel.ForEach(racing.Totes.Cast<ToteElement>(), tote =>
                    {
                        var toteItem = new Tote
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active,
                            Enable = !ColumnValue<bool>(rowData, string.Format(_fieldDisable, racing.Id, tote.Id), false),
                            EnableUpline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableUplines, racing.Id, tote.Id), false),
                            EnableDownline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableDownlines, racing.Id, tote.Id), false),
                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);
                        //add tote item
                        totes.Add(toteItem);
                    });
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
            // set racing sports
            setting.RacingSportType = sports;
            return setting;
        }

        private static Setting GetSuperSetting(System.Data.DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection)
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes){
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }

                        var toteItem = new Tote
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active,
                            Enable = !ColumnValue<bool>(rowData, string.Format(_fieldDisable, racing.Id, tote.Id), false),
                            EnableDownline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableDownlines, racing.Id, tote.Id), false),
                            MinBet = ColumnValue<decimal>(rowData, string.Format(_fieldMinBet, racing.Id, tote.Id), 0),
                            MaxBet = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBet, racing.Id, tote.Id), 0),
                            MaxPerMatch = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatch, racing.Id, tote.Id), 0),
                            Comm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscount, racing.Id, tote.Id), 0),
                            MinComm = 0,
                            MaxComm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscountMax, racing.Id, tote.Id), 0),
                            HousePt = ColumnValue<decimal>(rowData, string.Format(_fieldHousept, racing.Id, tote.Id), 0),
                            MaxHousePt = ColumnValue<decimal>(rowData, string.Format(_fieldHousetMax, racing.Id, tote.Id), 0),
                            MinHousePt = 0
                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (string betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    Pt = ColumnValue<decimal>(rowData, string.Format(_fieldSuperpt, racing.Id, betId), 0),
                                    MaxPt = ColumnValue<decimal>(rowData, string.Format(_fieldSuperptMax, racing.Id, betId), 0),
                                    MinPt = ColumnValue<decimal>(rowData, string.Format(_fieldSuperptMin, racing.Id, betId), 0),
                                    MasterPt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterpt, racing.Id, betId), 0),
                                    AgentPt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentpt, racing.Id, betId), 0)
                                };

                                betType.PositionTaking = dataBettype;
                                // add bettype
                                betTypes.Add(betType);
                            }
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                  
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
        
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static Setting GetMasterSetting(System.Data.DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection)
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes)
                    {
                         const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote()
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active,
                            Enable = !ColumnValue<bool>(rowData, string.Format(_fieldDisable, racing.Id, tote.Id), false),
                            EnableDownline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableDownlines, racing.Id, tote.Id), false),
                            EnableUpline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableUplines, racing.Id, tote.Id), false),

                            MaxBetMax = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBetMax, racing.Id, tote.Id), 0),
                            MaxPerMatchMax = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatchMax, racing.Id, tote.Id), 0),
                            MinBet = ColumnValue<decimal>(rowData, string.Format(_fieldMinBet, racing.Id, tote.Id), 0),
                            MaxBet = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBet, racing.Id, tote.Id), 0),
                            MaxPerMatch = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatch, racing.Id, tote.Id), 0),

                            Comm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscount, racing.Id, tote.Id), 0),
                            MinComm = 0,
                            MaxComm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscountMax, racing.Id, tote.Id), 0),
                            MinBetMin = ColumnValue<decimal>(rowData, string.Format(_fieldMinBetMin, racing.Id, tote.Id), 0)
                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (string betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    Pt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterpt, racing.Id, betId), 0),
                                    MaxPt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterptMax, racing.Id, betId), 0),
                                    MinPt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterptMin, racing.Id, betId), 0),
                                    MasterPt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterpt, racing.Id, betId), 0),
                                    AgentPt = 0,
                                    IsAutoPt = ColumnValue<bool>(rowData, string.Format(_fieldIsAutoSpt, racing.Id, betId), false),
                                    AutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoSpt, racing.Id, betId), 0),
                                    MaxAutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoSptMax, racing.Id, betId), 0),
                                    MinAutoPt = 0,

                                    UplinePt = ColumnValue<decimal>(rowData, string.Format(_fieldSuperpt, racing.Id, betId), 0),
                                    MaxUplinePt = ColumnValue<decimal>(rowData, string.Format(_fieldSuperptMax, racing.Id, betId), 0),
                                    MinUplinePt = 0
                                };
                                betType.PositionTaking = dataBettype;

                                // add bettype
                                betTypes.Add(betType);
                            }
                           
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
          
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static Setting GetAgentSetting(System.Data.DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection)
            {
                 // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes)
                    {
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote()
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active,
                            Enable = !ColumnValue<bool>(rowData, string.Format(_fieldDisable, racing.Id, tote.Id), false),
                            EnableDownline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableDownlines, racing.Id, tote.Id), false),
                            EnableUpline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableUplines, racing.Id, tote.Id), false),

                            MinBet = ColumnValue<decimal>(rowData, string.Format(_fieldMinBet, racing.Id, tote.Id), 0),
                            MinBetMin = ColumnValue<decimal>(rowData, string.Format(_fieldMinBetMin, racing.Id, tote.Id), 0),
                            MaxBet = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBet, racing.Id, tote.Id), 0),
                            MaxBetMax = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBetMax, racing.Id, tote.Id), 0),
                            MaxPerMatch = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatch, racing.Id, tote.Id), 0),
                            MaxPerMatchMax = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatchMax, racing.Id, tote.Id), 0),
                            Comm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscount, racing.Id, tote.Id), 0),
                            MinComm = 0,
                            MaxComm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscountMax, racing.Id, tote.Id), 0),
                            EnableAutoMPT = !ColumnValue<bool>(rowData, _fieldDisableAutoMpt, false),
                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (string betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    AgentPt = valDefault,
                                    Pt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentpt, racing.Id, betId), 0),
                                    MaxPt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentMaxpt, racing.Id, betId), 0),
                                    MinPt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentMinpt, racing.Id, betId), 0),

                                    IsAutoPt = ColumnValue<bool>(rowData, string.Format(_fieldIsAutoMpt, racing.Id, betId), false),
                                    AutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoMpt, racing.Id, betId), 0),
                                    MaxAutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoMptMax, racing.Id, betId), 0),
                                    MinAutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoMptMax, racing.Id, betId), 0),

                                    MasterPt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterpt, racing.Id, betId), 0),
                                    UplinePt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterpt, racing.Id, betId), 0),
                                    MaxUplinePt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterAptMax, racing.Id, betId), 0),
                                    MinUplinePt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterAptMin, racing.Id, betId), 0),
                                };
                                betType.PositionTaking = dataBettype;

                                // add bettype
                                betTypes.Add(betType);
                            }
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
       
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static Setting GetMemberSetting(System.Data.DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection)
            {
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes)
                    {
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote()
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active,
                            Enable = !ColumnValue<bool>(rowData, string.Format(_fieldDisable, racing.Id, tote.Id), false),
                            EnableUpline = !ColumnValue<bool>(rowData, string.Format(_fieldDisableUplines, racing.Id, tote.Id), false),

                            MinBet = ColumnValue<decimal>(rowData, string.Format(_fieldMinBet, racing.Id, tote.Id), 0),
                            MaxBet = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBet, racing.Id, tote.Id), 0),
                            MaxPerMatch = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatch, racing.Id, tote.Id), 0),

                            MinBetMin = ColumnValue<decimal>(rowData, string.Format(_fieldMinBetMin, racing.Id, tote.Id), 0),
                            MaxBetMax = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBetMax, racing.Id, tote.Id), 0),
                            MaxPerMatchMax = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatchMax, racing.Id, tote.Id), 0),

                            // member com
                            Comm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscount, racing.Id, tote.Id), 0),
                            MinComm = 0,
                            MaxComm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscountMax, racing.Id, tote.Id), 0),

                            // master com
                            MasterComm = ColumnValue<decimal>(rowData, string.Format(_fieldMasterCom, racing.Id, tote.Id), 0)

                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (string betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    Pt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentpt, racing.Id, betId), 0),
                                    MaxPt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentptMax, racing.Id, betId), 0),
                                    MinPt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentptMin, racing.Id, betId), 0),
                                };
                                betType.PositionTaking = dataBettype;

                                // add bettype
                                betTypes.Add(betType);

                            }
                         
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                   
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
            
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static CustInfo GetCustInfo(DataRow rowData)
        {
            var custInfo = new CustInfo()
            {
                CustId = _custId,
                CustLevel = _roleId == 0 ? ColumnValue<int>(rowData, "roleid", 0) : _roleId,
                Fax = ColumnValue<string>(rowData, "fax", string.Empty),
                FirstName = ColumnValue<string>(rowData, "firstname", string.Empty),
                UserName = ColumnValue<string>(rowData, "username", string.Empty),
                LastName = ColumnValue<string>(rowData, "lastname", string.Empty),
                Phone = ColumnValue<string>(rowData, "phone", string.Empty),
                MobilePhone = ColumnValue<string>(rowData, "mobilephone", string.Empty),
                Currency = ColumnValue<int>(rowData, "currency", 0)
            };
            return custInfo;
        }

        private static T ColumnValue<T>(DataRow rowData, string fieldName, T valDefault)
        {
            var dataType = typeof(T);
            
            if (rowData.Table.Columns.Contains(fieldName))
            {
                object objData = null;

                switch (dataType.Name.ToLower())
                {
                    case "decimal":
                        objData = CastAide.AsDecimal(rowData[fieldName], 0);
                        break;
                    case "string":
                        objData = CastAide.AsString(rowData[fieldName], string.Empty);
                        break;
                    case "int32":
                        objData = CastAide.AsInt32(rowData[fieldName], 0);
                        break;
                    case "boolean":
                        objData = CastAide.AsBoolean(rowData[fieldName], false);
                        break;
                    default: // default string
                        objData = CastAide.AsString(rowData[fieldName], string.Empty);
                        break;
                }
                return (T)objData;
            }
            
           
            return valDefault;
        }

        private static Setting GetSuperMinPt(System.Data.DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection.Cast<RacingElement>())
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes.Cast<ToteElement>())
                    {
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active
                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (var betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    Pt = CastAide.AsDecimal(rowData[string.Format(_fieldSuperptMin, racing.Id, betId)]),
                                    MaxPt = CastAide.AsDecimal(rowData[string.Format(_fieldSuperptMax, racing.Id, betId)]),
                                    MinPt = 0
                                };

                                betType.PositionTaking = dataBettype;
                                // add bettype
                                betTypes.Add(betType);
                            }
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static Setting GetMasterMinMaxPt(System.Data.DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection.Cast<RacingElement>())
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes.Cast<ToteElement>())
                    {
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active
                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (var betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    //Max PT
                                    UplinePt = CastAide.AsDecimal(rowData[string.Format(_fieldMasterptMax, racing.Id, betId)]),
                                    MaxUplinePt = RacingSection.Instance.MaxPT,
                                    MinUplinePt = CastAide.AsDecimal(rowData[string.Format(_fieldMasterptMin, racing.Id, betId)]),

                                    //Min PT
                                    Pt = CastAide.AsDecimal(rowData[string.Format(_fieldMasterptMin, racing.Id, betId)]),
                                    MaxPt = CastAide.AsDecimal(rowData[string.Format(_fieldMasterpt, racing.Id, betId)]),
                                    MinPt = 0
                                };

                                betType.PositionTaking = dataBettype;
                                // add bettype
                                betTypes.Add(betType);
                            }
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static Setting GetAgentMinMaxPt(System.Data.DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection.Cast<RacingElement>())
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes.Cast<ToteElement>())
                    {
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active
                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (var betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    //Max PT
                                    UplinePt = CastAide.AsDecimal(rowData[string.Format(_fieldAgentMaxpt, racing.Id, betId)]),
                                    MaxUplinePt = RacingSection.Instance.MaxPT,
                                    MinUplinePt = CastAide.AsDecimal(rowData[string.Format(_fieldAgentMinpt, racing.Id, betId)]),

                                    //Min PT
                                    Pt = CastAide.AsDecimal(rowData[string.Format(_fieldAgentMinpt, racing.Id, betId)]),
                                    MaxPt = CastAide.AsDecimal(rowData[string.Format(_fieldAgentpt, racing.Id, betId)]),
                                    MinPt = 0
                                };

                                betType.PositionTaking = dataBettype;
                                // add bettype
                                betTypes.Add(betType);
                            }
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static Setting SuperOnMemberSetting(DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection)
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes)
                    {
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote()
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active,

                            MinBet = ColumnValue<decimal>(rowData, string.Format(_fieldMinBet, racing.Id, tote.Id), 0),
                            MaxBet = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBet, racing.Id, tote.Id), 0),
                            MaxPerMatch = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatch, racing.Id, tote.Id), 0),

                            // cust com
                            Comm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscount, racing.Id, tote.Id), 0),
                            MasterComm = ColumnValue<decimal>(rowData, string.Format(_fieldMasterAgCom, racing.Id, tote.Id), 0),
                            AgentComm = ColumnValue<decimal>(rowData, string.Format(_fieldAgentCom, racing.Id, tote.Id), 0),
                            MemberComm = 0

                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            Parallel.ForEach(betStringType.Cast<string>(), betId =>
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    // member PT
                                    Pt = ColumnValue<decimal>(rowData, string.Format(_fieldSuperpt, racing.Id, betId), 0),
                                    MaxPt = ColumnValue<decimal>(rowData, string.Format(_fieldPtMax, racing.Id, betId), 0),
                                    MinPt = ColumnValue<decimal>(rowData, string.Format(_fieldPtMin, racing.Id, betId), 0),
                                    MasterPt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterpt, racing.Id, betId), 0),
                                    AgentPt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentpt, racing.Id, betId), 0),

                                    // Member auto
                                    IsAutoPt = ColumnValue<bool>(rowData, string.Format(_fieldIsAutoSOnMem, racing.Id, betId), false),
                                    AutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoSOnMemPt, racing.Id, betId), 0),
                                    MaxAutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoSOnMemMax, racing.Id, betId), 0),
                                    IsUplineAutoPt = ColumnValue<bool>(rowData, string.Format(_fieldIsAutoSpt, racing.Id, betId), false)
                                };
                                betType.PositionTaking = dataBettype;

                                // add bettype
                                betTypes.Add(betType);
                            });
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                   
                   
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
          
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static Setting MasterOnMemberSetting(DataTable settingData)
        {
            var setting = new Setting();
            var rowData = settingData.Rows[0];

            // get custinfo
            setting.CustInfo = GetCustInfo(rowData);

            // get racing setting
            var racingCollection = RacingSection.Instance.Racings;

            // get sport type
            var sports = new List<Sport>();
            foreach (RacingElement racing in racingCollection)
            {
                // set sport information
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(racing.Id),
                    Name = racing.Name,
                    Visible = racing.Active
                };
                // set data tote information
                if (racing.Totes.Count > 0)
                {
                    var totes = new List<Tote>();
                    foreach (ToteElement tote in racing.Totes)
                    {
                        const decimal valDefault = 0;
                        // set defaultRacingSportType
                        if (racing.Active && tote.IsTop)
                        {
                            // set racing default
                            setting.DefaultRacingSportType = sport.Name + " " + tote.Name;
                            setting.DefaultRacingSportTypeId = string.Concat(racing.Id.ToString(), "_", tote.Id.ToString());
                        }
                        var toteItem = new Tote()
                        {
                            Id = racing.Id + "_" + tote.Id,
                            Name = tote.Name,
                            IsTop = tote.IsTop,
                            Visible = tote.Active,
                            Enable = !ColumnValue<bool>(rowData, string.Format(_fieldDisable, racing.Id, tote.Id), false),

                            MinBet = ColumnValue<decimal>(rowData, string.Format(_fieldMinBet, racing.Id, tote.Id), 0),
                            MaxBet = ColumnValue<decimal>(rowData, string.Format(_fieldMaxBet, racing.Id, tote.Id), 0),
                            MaxPerMatch = ColumnValue<decimal>(rowData, string.Format(_fieldMaxPerMatch, racing.Id, tote.Id), 0),

                            EnableAutoMPT = !ColumnValue<bool>(rowData, string.Format(_fieldDisableAutoMpt, racing.Id, tote.Id), false),

                            // member com
                            Comm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscount, racing.Id, tote.Id), 0),
                            MinComm = 0,
                            MaxComm = ColumnValue<decimal>(rowData, string.Format(_fieldDiscountMax, racing.Id, tote.Id), 0),

                            // master com
                            AgentComm = ColumnValue<decimal>(rowData, string.Format(_fieldAgentCom, racing.Id, tote.Id), 0)

                        };
                        // check allow currency
                        toteItem.Visible = CheckAllowCurrency(setting.CustInfo.Currency, tote.Currency, toteItem.Visible);

                        var betStringType = tote.BetType.Split(',');
                        var betTypes = new List<Bettype>();
                        if (betStringType.Length > 0)
                        {
                            foreach (string betId in betStringType)
                            {
                                var betType = new Bettype()
                                {
                                    Id = CastAide.AsInt32(betId),
                                    Name = GetBettypeName(CastAide.AsInt32(betId)),
                                    PositionTaking = null
                                };
                                var dataBettype = new Sunplus.Accounting.Entities.PositionTaking.Racing.Data()
                                {
                                    Pt = ColumnValue<decimal>(rowData, string.Format(_fieldMasterpt, racing.Id, betId), 0),
                                    MaxPt = ColumnValue<decimal>(rowData, string.Format(_fieldPtMax, racing.Id, betId), 0),
                                    MinPt = ColumnValue<decimal>(rowData, string.Format(_fieldPtMin, racing.Id, betId), 0),

                                    AgentPt = ColumnValue<decimal>(rowData, string.Format(_fieldAgentpt, racing.Id, betId), 0),

                                    // isauto
                                    IsAutoPt = ColumnValue<bool>(rowData, string.Format(_fieldIsAutoMOnMem, racing.Id, betId), false),
                                    AutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoMOnMemPt, racing.Id, betId), 0),
                                    MaxAutoPt = ColumnValue<decimal>(rowData, string.Format(_fieldAutoMOnMemMax, racing.Id, betId), 0),
                                    IsUplineAutoPt = ColumnValue<bool>(rowData, string.Format(_fieldIsAutoMpt, racing.Id, betId), false)

                                };
                                betType.PositionTaking = dataBettype;

                                // add bettype
                                betTypes.Add(betType);
                            }
                          
                            // set tote bettypes
                            toteItem.Bettypes = betTypes;
                        }
                        //add tote item
                        totes.Add(toteItem);
                    }
                  
                    // set sport totes
                    sport.Totes = totes;
                }
                // add sport type
                sports.Add(sport);
            }
          
            // set racing sports
            setting.RacingSportType = sports;

            return setting;
        }

        private static string GetBettypeName(int bettypeId)
        {
            return (bettypeId % 2 == 1) ? "Win" : "Place";
        }

        public static bool CheckAllowCurrency(int currencyId, string allowCurrrency, bool isVisible)
        {
            if (!isVisible) { return false; } // current off - not check currency
            if (string.IsNullOrEmpty(allowCurrrency) || allowCurrrency == "*" || currencyId == 0)
            {
                return true;
            }
            var lsExchange = CacheUtils.Exchange;
            var currencyName = CacheUtils.GetExchangeName(lsExchange, currencyId);
            return allowCurrrency.Split(',').Contains(currencyName);
        }
        #endregion Get Cust Info
    }
}
