using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DNAide.Core;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.PositionTaking.Racing;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.PositionTaking.Racing;

namespace Sunplus.Accounting.Business.PT.Racing
{
    public partial class RacingManager : BaseManager, IRacingManager
    {
        #region Racing DAO

        private static IRacingDAO _racingDao;

        #endregion Racing DAO

        #region sport racing super const

        private static string _fieldDisable = "disable{0}_{1}";
        private static string _fieldDisableDownlines = "disabledownlines{0}_{1}";
        private static string _fieldMinBet = "v{0}minbet_{1}";
        private static string _fieldMaxBet = "v{0}maxbet_{1}";
        private static string _fieldMaxPerMatch = "v{0}maxpermatch_{1}";
        private static string _fieldDiscount = "v{0}discount_{1}";
        private static string _fieldDiscountMax = "v{0}discount_{1}_max";
        private static string _fieldHousept = "v{0}ibcpositiontaking_{1}";
        private static string _fieldHousetMax = "v{0}ibcpositiontaking_{1}_max";
        private static string _fieldHouseptMin = "v{0}ibcpositiontaking_{1}_min";
        private static string _fieldSuperpt = "v{0}spositiontaking_{1}";
        private static string _fieldSuperptMax = "v{0}spositiontaking_{1}_max";
        private static string _fieldSuperptMin = "v{0}spositiontaking_{1}_min";
        private static string _fieldMasterpt = "v{0}mpositiontaking_{1}";
        private static string _fieldAgentpt = "v{0}apositiontaking_{1}";

        private static string _aust51 = "Aust Win";
        private static string _aust52 = "Aust Place";

        #endregion sport racing super const

        #region sport racing master const

        private static string _fieldDisableUplines = "disableuplines{0}_{1}";
        private static string _fieldMinBetMin = "v{0}minbet_{1}_min";
        private static string _fieldMaxBetMax = "v{0}maxbet_{1}_max";
        private static string _fieldMaxPerMatchMax = "v{0}maxpermatch_{1}_max";
        private static string _fieldAutoSpt = "auto_v{0}spositiontaking_{1}";
        private static string _fieldAutoSptMax = "auto_v{0}spositiontaking_{1}_max";
        private static string _fieldIsAutoSpt = "isauto_v{0}spositiontaking_{1}";
        private static string _fieldMasterptMax = "v{0}mpositiontaking_{1}_max";
        private static string _fieldMasterptMin = "v{0}mpositiontaking_{1}_min";

        #endregion sport racing master const

        #region sport racing agent const

        private static string _fieldIsAutoMpt = "isauto_v{0}mpositiontaking_{1}";
        private static string _fieldAutoMpt = "auto_v{0}mpositiontaking_{1}";
        private static string _fieldAutoMptMax = "auto_v{0}mpositiontaking_{1}_max";
        private static string _fieldDisableAutoMpt = "disableautompt";
        private static string _fieldAgentMaxpt = "v{0}apositiontaking_{1}_max";
        private static string _fieldAgentMinpt = "v{0}apositiontaking_{1}_min";
        private static string _fieldMasterAptMax = "v{0}mapositiontaking_{1}_max";
        private static string _fieldMasterAptMin = "v{0}mapositiontaking_{1}_min";
        private static string _fieldIsUdpAllMembers = "updateallmembers";
        private static string _fieldNewSuperPt = "newspositiontaking";
        private static string _fieldNewMasterPt = "newmpositiontaking";

        #endregion sport racing agent const

        #region sport racing member

        private static string _fieldMasterCom = "v{0}discount_{1}_master";
        private static string _fieldAgentptMax = "v{0}pos_{1}_max";
        private static string _fieldAgentptMin = "v{0}pos_{1}_min";

        #endregion sport racing member

        #region update setting params

        private static string _fieldUpdateDisable = "updatedisable{0}_{1}";
        private static string _fieldUpdateMinMaxBet = "updateminmaxbet{0}_{1}";
        private static string _fieldIsCoppyPreset = "iscopypreset{0}_{1}";
        private static string _fieldUdpComm = "updatecommission";
        private static string _fieldUpdPt = "updatepositiontaking";
        private static string _fieldMinPt = "v{0}positiontaking_{1}_min";
        private static string _fieldMaxPt = "v{0}positiontaking_{1}_max";

        #endregion update setting params

        #region sport racing super on member

        private static string _fieldMasterAgCom = "v{0}discount_{1}_mag";
        private static string _fieldAgentCom = "v{0}discount_{1}_ag";
        private static string _fieldPtMax = "v{0}pos_{1}_max";
        private static string _fieldPtMin = "v{0}pos_{1}_min";
        private static string _fieldIsAutoSOnMem = "isauto_v{0}spositiontaking_mem_{1}";
        private static string _fieldIsAutoSOnMaster = "isauto_v{0}smpositiontaking_{1}";
        private static string _fieldAutoSOnMemPt = "auto_v{0}spositiontaking_mem_{1}";
        private static string _fieldAutoSOnMemMax = "auto_v{0}pos_{1}_max";

        #endregion sport racing super on member

        #region sport racing master on member

        private static string _fieldIsAutoMOnMem = "isauto_v{0}mpositiontaking_mem_{1}";
        private static string _fieldAutoMOnMemPt = "auto_v{0}mpositiontaking_mem_{1}";
        private static string _fieldAutoMOnMemMax = "auto_v{0}mpositiontaking_mem_{1}_max";

        #endregion sport racing master on member

        #region update minmax

        private static string _fieldIsUdpMinPt = "updateminpos";
        private static string _fieldIsUdpMaxPt = "updatemaxpos";

        #endregion update minmax

        #region private variable

        private static int _roleId = 0;
        private static int _custId = 0;

        #endregion private variable

        public RacingManager(IRacingDAO racingDao)
        {
            _racingDao = racingDao;
        }

        #region Update cust info

        public Result UpdateCustomerStatus(Setting setting)
        {
            _roleId = CastAide.AsInt32(setting.CustInfo.CustLevel);
            var result = new Result();
            try
            {
                var hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Customer.Customer);
                if (!hostPermission)
                {
                    result.ErrorCode = 100;
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500");
                    return result;
                }
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                // get racing setting
                foreach (Sport racing in setting.RacingSportType)
                {
                    foreach (Tote tote in racing.Totes)
                    {
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // set disable params
                        settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);
                        settingParams.Add(string.Format(_fieldDisableDownlines, racing.Id, toteId), !tote.EnableDownline);
                        // set update disable params
                        settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId), tote.UpdateDisable);
                    }
                }

                //System.Threading.Tasks.Parallel.ForEach(setting.RacingSportType.Cast<Sport>(),
                //    racing => Parallel.ForEach(racing.Totes.Cast<Tote>(),
                //        tote =>
                //        {
                //            // receive tote data
                //            var strTote = tote.Id.Split('_');
                //            var toteId = CastAide.AsInt32(strTote[1]);

                //            // set disable params
                //            settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);
                //            settingParams.Add(string.Format(_fieldDisableDownlines, racing.Id, toteId), !tote.EnableDownline);
                //            // set update disable params
                //            settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId), tote.UpdateDisable);

                //        }
                //    )
                //);
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("username", username);
                settingParams.Add("errcode", "");
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingStatus(settingParams);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public Result UpdateCustomerSetting(Setting setting)
        {
            _roleId = CastAide.AsInt32(setting.CustInfo.CustLevel);
            _custId = CastAide.AsInt32(setting.CustInfo.CustId);
            var result = new Result();
            try
            {
                var hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Customer.Customer);
                if (!hostPermission)
                {
                    result.ErrorCode = 100;
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500");
                    result.ErrorHandler = new Dictionary<int, string> { { 500, result.ErrorMessage } };
                    return result;
                }
                // check valid user & role
                var crypting = GetCyripting();
                if (crypting != setting.Crypting)
                {
                    result.ErrorCode = 100;
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "100");
                    result.ErrorHandler = new Dictionary<int, string> { { 100, result.ErrorMessage } };
                    return result;
                }
                switch (_roleId)
                {
                    case (int)CustInfo.Roles.Super://s
                        result = UpdateSupperSetting(setting);
                        break;

                    case (int)CustInfo.Roles.Master://m
                        result = UpdateMasterSetting(setting);
                        break;

                    case (int)CustInfo.Roles.Agent://a
                        result = UpdateAgentSetting(setting);
                        break;

                    case (int)CustInfo.Roles.Member://m
                        result = UpdateMemberSetting(setting);
                        break;

                    case (int)CustInfo.Roles.SuperOnMember: //*******Super on Member params*****
                        result = UpdateSuperOnMember(setting);
                        break;

                    case (int)CustInfo.Roles.MasterOnMember: //*******Master on Member params*****
                        result = UpdateMasterOnMember(setting);
                        break;

                    case (int)CustInfo.Roles.SuperMin: //*******Super Min params*****
                        result = UpdateSuperMinPt(setting);
                        break;

                    case (int)CustInfo.Roles.MasterMinMax: //*******Master Min/Max params*****
                    case (int)CustInfo.Roles.AgentMinMax: //*******Agent Min/Max params*****
                        result = UpdateMinMaxPt(setting);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            return result;
        }

        private static Result UpdateSupperSetting(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                foreach (Sport racing in setting.RacingSportType)
                {
                    foreach (Tote tote in racing.Totes)
                    {
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // set disable params
                        settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId),
                                          tote.UpdateDisable);
                        settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);
                        settingParams.Add(string.Format(_fieldDisableDownlines, racing.Id, toteId),
                                          !tote.EnableDownline);

                        // check update comm
                        CheckIsUpdate(ref settingParams, _fieldUdpComm, tote.UpdateComm);
                        settingParams.Add(string.Format(_fieldDiscount, racing.Id, toteId), tote.Comm);

                        // set params house PT
                        settingParams.Add(string.Format(_fieldHousept, racing.Id, toteId), tote.HousePt);

                        // check is update minmax
                        var keyUpdateMinMax = string.Format(_fieldUpdateMinMaxBet, racing.Id, toteId);
                        CheckIsUpdate(ref settingParams, keyUpdateMinMax, tote.UpdateMinMax);
                        // set min/max
                        settingParams.Add(string.Format(_fieldMinBet, racing.Id, toteId), tote.MinBet);
                        settingParams.Add(string.Format(_fieldMaxBet, racing.Id, toteId), tote.MaxBet);
                        settingParams.Add(string.Format(_fieldMaxPerMatch, racing.Id, toteId), tote.MaxPerMatch);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);
                        // set params pt
                        foreach (Bettype bettype in tote.Bettypes)
                        {
                            // receive bettype data
                            var betId = CastAide.AsInt32(bettype.Id);
                            var dataPt = bettype.PositionTaking;

                            // customer PT
                            settingParams.Add(string.Format(_fieldSuperpt, racing.Id, betId), dataPt.Pt);
                            settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.MasterPt);
                            settingParams.Add(string.Format(_fieldAgentpt, racing.Id, betId), dataPt.AgentPt);
                        }
                    }
                }

                // get racing setting
                //Parallel.ForEach(setting.RacingSportType.Cast<Sport>(),
                //    racing => Parallel.ForEach(racing.Totes.Cast<Tote>(),
                //        tote =>
                //        {
                //            // receive tote data
                //            var strTote = tote.Id.Split('_');
                //            var toteId = CastAide.AsInt32(strTote[1]);

                //            // set disable params
                //            settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId),
                //                              tote.UpdateDisable);
                //            settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);
                //            settingParams.Add(string.Format(_fieldDisableDownlines, racing.Id, toteId),
                //                              !tote.EnableDownline);

                //            // check update comm
                //            CheckIsUpdate(ref settingParams, _fieldUdpComm, tote.UpdateComm);
                //            settingParams.Add(string.Format(_fieldDiscount, racing.Id, toteId), tote.Comm);

                //            // set params house PT
                //            settingParams.Add(string.Format(_fieldHousept, racing.Id, toteId), tote.HousePt);

                //            // check is update minmax
                //            var keyUpdateMinMax = string.Format(_fieldUpdateMinMaxBet, racing.Id, toteId);
                //            CheckIsUpdate(ref settingParams, keyUpdateMinMax, tote.UpdateMinMax);
                //            // set min/max
                //            settingParams.Add(string.Format(_fieldMinBet, racing.Id, toteId), tote.MinBet);
                //            settingParams.Add(string.Format(_fieldMaxBet, racing.Id, toteId), tote.MaxBet);
                //            settingParams.Add(string.Format(_fieldMaxPerMatch, racing.Id, toteId), tote.MaxPerMatch);

                //            // check is update PT
                //            CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);
                //            // set params pt
                //            Parallel.ForEach(tote.Bettypes.Cast<Bettype>(), bettype =>
                //            {
                //                // receive bettype data
                //                var betId = CastAide.AsInt32(bettype.Id);
                //                var dataPt = bettype.PositionTaking;

                //                // customer PT
                //                settingParams.Add(string.Format(_fieldSuperpt, racing.Id, betId), dataPt.Pt);
                //                settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.MasterPt);
                //                settingParams.Add(string.Format(_fieldAgentpt, racing.Id, betId), dataPt.AgentPt);

                //            });
                //        }
                //    )
                //);

                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("username", username);
                settingParams.Add("errcode", "");
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private static Result UpdateMasterSetting(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                settingParams.Add(_fieldIsUdpAllMembers, setting.IsUpdateAllMember);
                settingParams.Add(_fieldNewSuperPt, setting.NewPt);

                foreach (Sport racing in setting.RacingSportType)
                {
                    foreach (Tote tote in racing.Totes)
                    {
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // check update comm
                        CheckIsUpdate(ref settingParams, _fieldUdpComm, tote.UpdateComm);
                        settingParams.Add(string.Format(_fieldDiscount, racing.Id, toteId), tote.Comm);

                        // set disable params
                        settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);
                        settingParams.Add(string.Format(_fieldDisableDownlines, racing.Id, toteId), !tote.EnableDownline);

                        // set update disable params
                        settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId), tote.UpdateDisable);

                        // set params is copy reset
                        settingParams.Add(string.Format(_fieldIsCoppyPreset, racing.Id, toteId), tote.IsApplyPresetPT);

                        // check is update minmax
                        var keyUpdateMinMax = string.Format(_fieldUpdateMinMaxBet, racing.Id, toteId);
                        CheckIsUpdate(ref settingParams, keyUpdateMinMax, tote.UpdateMinMax);

                        // Min / Max bet
                        settingParams.Add(string.Format(_fieldMinBet, racing.Id, toteId), tote.MinBet);
                        settingParams.Add(string.Format(_fieldMaxBet, racing.Id, toteId), tote.MaxBet);
                        settingParams.Add(string.Format(_fieldMaxPerMatch, racing.Id, toteId), tote.MaxPerMatch);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);
                        // set params bettype
                        foreach (Bettype bettype in tote.Bettypes)
                        {
                            var betId = CastAide.AsInt32(bettype.Id);
                            var dataPt = bettype.PositionTaking;

                            // set auto PT
                            settingParams.Add(string.Format(_fieldIsAutoSpt, racing.Id, betId), dataPt.IsAutoPt);
                            // customer PT
                            if (dataPt.IsAutoPt)
                            {
                                dataPt.UplinePt = 0;
                            }
                            else
                            {
                                dataPt.AutoPt = 0;
                            }
                            settingParams.Add(string.Format(_fieldAutoSpt, racing.Id, betId), dataPt.AutoPt);
                            settingParams.Add(string.Format(_fieldSuperpt, racing.Id, betId), dataPt.UplinePt);
                            settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.Pt);
                        }

                        //Parallel.ForEach(tote.Bettypes.Cast<Bettype>(), bettype =>
                        //{
                        //    // receive bettype data
                        //    var betId = CastAide.AsInt32(bettype.Id);
                        //    var dataPt = bettype.PositionTaking;

                        //    // set auto PT
                        //    settingParams.Add(string.Format(_fieldIsAutoSpt, racing.Id, betId), dataPt.IsAutoPt);
                        //    // customer PT
                        //    if (dataPt.IsAutoPt)
                        //    {
                        //        dataPt.UplinePt = 0;
                        //    }
                        //    else
                        //    {
                        //        dataPt.AutoPt = 0;
                        //    }
                        //    settingParams.Add(string.Format(_fieldAutoSpt, racing.Id, betId), dataPt.AutoPt);
                        //    settingParams.Add(string.Format(_fieldSuperpt, racing.Id, betId), dataPt.UplinePt);
                        //    settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.Pt);
                        //});
                    }
                }

                // get racing setting
                //Parallel.ForEach(setting.RacingSportType.Cast<Sport>(),
                //    racing => Parallel.ForEach(racing.Totes.Cast<Tote>(),
                //        tote =>
                //        {
                //            // receive tote data
                //            var strTote = tote.Id.Split('_');
                //            var toteId = CastAide.AsInt32(strTote[1]);

                //            // check update comm
                //            CheckIsUpdate(ref settingParams, _fieldUdpComm, tote.UpdateComm);
                //            settingParams.Add(string.Format(_fieldDiscount, racing.Id, toteId), tote.Comm);

                //            // set disable params
                //            settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);
                //            settingParams.Add(string.Format(_fieldDisableDownlines, racing.Id, toteId), !tote.EnableDownline);

                //            // set update disable params
                //            settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId), tote.UpdateDisable);

                //            // set params is copy reset
                //            settingParams.Add(string.Format(_fieldIsCoppyPreset, racing.Id, toteId), tote.IsApplyPresetPT);

                //            // check is update minmax
                //            var keyUpdateMinMax = string.Format(_fieldUpdateMinMaxBet, racing.Id, toteId);
                //            CheckIsUpdate(ref settingParams, keyUpdateMinMax, tote.UpdateMinMax);

                //            // Min / Max bet
                //            settingParams.Add(string.Format(_fieldMinBet, racing.Id, toteId), tote.MinBet);
                //            settingParams.Add(string.Format(_fieldMaxBet, racing.Id, toteId), tote.MaxBet);
                //            settingParams.Add(string.Format(_fieldMaxPerMatch, racing.Id, toteId), tote.MaxPerMatch);

                //            // check is update PT
                //            CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);
                //            // set params bettype
                //            Parallel.ForEach(tote.Bettypes.Cast<Bettype>(), bettype =>
                //            {
                //                // receive bettype data
                //                var betId = CastAide.AsInt32(bettype.Id);
                //                var dataPt = bettype.PositionTaking;

                //                // set auto PT
                //                settingParams.Add(string.Format(_fieldIsAutoSpt, racing.Id, betId), dataPt.IsAutoPt);
                //                // customer PT
                //                if (dataPt.IsAutoPt)
                //                {
                //                    dataPt.UplinePt = 0;
                //                }
                //                else
                //                {
                //                    dataPt.AutoPt = 0;
                //                }
                //                settingParams.Add(string.Format(_fieldAutoSpt, racing.Id, betId), dataPt.AutoPt);
                //                settingParams.Add(string.Format(_fieldSuperpt, racing.Id, betId), dataPt.UplinePt);
                //                settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.Pt);
                //            });
                //        }
                //    )
                //);
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("username", username);
                settingParams.Add("errcode", "");
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private static Result UpdateAgentSetting(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);
                settingParams.Add(_fieldIsUdpAllMembers, setting.IsUpdateAllMember);
                settingParams.Add(_fieldNewMasterPt, setting.NewPt);

                // get racing setting
                var racingCollection = RacingSection.Instance.Racings;
                foreach (Sport racing in setting.RacingSportType.Cast<Sport>())
                {
                    foreach (Tote tote in racing.Totes.Cast<Tote>())
                    {
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // check update comm
                        CheckIsUpdate(ref settingParams, _fieldUdpComm, tote.UpdateComm);
                        settingParams.Add(string.Format(_fieldDiscount, racing.Id, toteId), tote.Comm);

                        // set disable params
                        settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);
                        settingParams.Add(string.Format(_fieldDisableDownlines, racing.Id, toteId), !tote.EnableDownline);

                        // set update disable params
                        settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId), tote.UpdateDisable);

                        // check is update minmax
                        var keyUpdateMinMax = string.Format(_fieldUpdateMinMaxBet, racing.Id, toteId);
                        CheckIsUpdate(ref settingParams, keyUpdateMinMax, tote.UpdateMinMax);

                        // Min / Max bet
                        settingParams.Add(string.Format(_fieldMinBet, racing.Id, toteId), tote.MinBet);
                        settingParams.Add(string.Format(_fieldMaxBet, racing.Id, toteId), tote.MaxBet);
                        settingParams.Add(string.Format(_fieldMaxPerMatch, racing.Id, toteId), tote.MaxPerMatch);

                        //IsCopyPreset
                        settingParams.Add(string.Format(_fieldIsCoppyPreset, racing.Id, toteId), tote.IsApplyPresetPT);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);

                        // set params bettype
                        foreach (Bettype bettype in tote.Bettypes.Cast<Bettype>())
                        {
                            // receive bettype data
                            var betId = CastAide.AsInt32(bettype.Id);
                            var dataPt = bettype.PositionTaking;

                            //auto Mpt
                            settingParams.Add(string.Format(_fieldIsAutoMpt, racing.Id, betId), dataPt.IsAutoPt);

                            // customer PT
                            if (dataPt.IsAutoPt)
                            {
                                dataPt.UplinePt = 0;
                            }
                            else
                            {
                                dataPt.AutoPt = 0;
                            }
                            settingParams.Add(string.Format(_fieldAutoMpt, racing.Id, betId), dataPt.AutoPt);
                            settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.UplinePt);
                            settingParams.Add(string.Format(_fieldAgentpt, racing.Id, betId), dataPt.Pt);
                        }
                    }
                }
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("username", username);
                settingParams.Add("errcode", "");
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private static Result UpdateMemberSetting(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                // get racing setting
                var racingCollection = RacingSection.Instance.Racings;

                foreach (Sport racing in setting.RacingSportType.Cast<Sport>())
                {
                    foreach (Tote tote in racing.Totes.Cast<Tote>())
                    {
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // check update comm
                        CheckIsUpdate(ref settingParams, _fieldUdpComm, tote.UpdateComm);
                        settingParams.Add(string.Format(_fieldDiscount, racing.Id, toteId), tote.Comm);

                        // set disable params
                        settingParams.Add(string.Format(_fieldDisable, racing.Id, toteId), !tote.Enable);

                        // set update disable params
                        settingParams.Add(string.Format(_fieldUpdateDisable, racing.Id, toteId), tote.UpdateDisable);

                        // check is update minmax
                        var keyUpdateMinMax = string.Format(_fieldUpdateMinMaxBet, racing.Id, toteId);
                        CheckIsUpdate(ref settingParams, keyUpdateMinMax, tote.UpdateMinMax);
                        // Min / Max bet
                        settingParams.Add(string.Format(_fieldMinBet, racing.Id, toteId), tote.MinBet);
                        settingParams.Add(string.Format(_fieldMaxBet, racing.Id, toteId), tote.MaxBet);
                        settingParams.Add(string.Format(_fieldMaxPerMatch, racing.Id, toteId), tote.MaxPerMatch);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);

                        // set params bettype
                        foreach (Bettype bettype in tote.Bettypes.Cast<Bettype>())
                        {
                            // receive bettype data
                            var betId = CastAide.AsInt32(bettype.Id);
                            var dataPt = bettype.PositionTaking;

                            // customer PT
                            settingParams.Add(string.Format(_fieldAgentpt, racing.Id, betId), dataPt.Pt);
                        }
                    }
                }
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("username", username);
                settingParams.Add("errcode", "");
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private static Result UpdateSuperMinPt(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                // get racing setting
                foreach (Sport racing in setting.RacingSportType.Cast<Sport>())
                {
                    foreach (Tote tote in racing.Totes.Cast<Tote>())
                    {
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldIsUdpMinPt, tote.UpdatePT);
                        // check not max super PT
                        CheckIsUpdate(ref settingParams, _fieldIsUdpMaxPt, false);

                        // set params bettype
                        foreach (Bettype bettype in tote.Bettypes.Cast<Bettype>())
                        {
                            // receive bettype data
                            var betId = CastAide.AsInt32(bettype.Id);
                            var dataPt = bettype.PositionTaking;

                            // customer PT
                            settingParams.Add(string.Format(_fieldMinPt, racing.Id, betId), dataPt.Pt);
                        }
                    }
                }
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("errcode", 0);
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private static Result UpdateMinMaxPt(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                // get racing setting
                foreach (Sport racing in setting.RacingSportType.Cast<Sport>())
                {
                    foreach (Tote tote in racing.Totes.Cast<Tote>())
                    {
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldIsUdpMinPt, tote.UpdatePT);
                        CheckIsUpdate(ref settingParams, _fieldIsUdpMaxPt, tote.UpdatePT);

                        // set params bettype
                        foreach (Bettype bettype in tote.Bettypes.Cast<Bettype>())
                        {
                            // receive bettype data
                            var betId = CastAide.AsInt32(bettype.Id);
                            var dataPt = bettype.PositionTaking;

                            // customer PT
                            settingParams.Add(string.Format(_fieldMinPt, racing.Id, betId), dataPt.Pt);
                            settingParams.Add(string.Format(_fieldMaxPt, racing.Id, betId), dataPt.UplinePt);
                        }
                    }
                }
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("errcode", 0);
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private static Result UpdateSuperOnMember(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                foreach (Sport racing in setting.RacingSportType)
                {
                    foreach (Tote tote in racing.Totes)
                    {
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);

                        foreach (Bettype bettype in tote.Bettypes)
                        {
                            // receive bettype data
                            var betId = CastAide.AsInt32(bettype.Id);
                            var dataPt = bettype.PositionTaking;

                            // reset PT
                            if (dataPt.IsAutoPt)
                            {
                                dataPt.Pt = 0;
                            }
                            else
                            {
                                dataPt.AutoPt = 0;
                            }
                            // customer PT
                            settingParams.Add(string.Format(_fieldSuperpt, racing.Id, betId), dataPt.Pt);

                            // set auto PT
                            settingParams.Add(string.Format(_fieldIsAutoSOnMaster, racing.Id, betId), dataPt.IsUplineAutoPt); // super auto PT on master
                            settingParams.Add(string.Format(_fieldIsAutoSpt, racing.Id, betId), dataPt.IsAutoPt); // super auto PT on member
                            settingParams.Add(string.Format(_fieldAutoSpt, racing.Id, betId), dataPt.AutoPt);
                        }
                    }
                }

                // get racing setting
                //Parallel.ForEach(setting.RacingSportType.Cast<Sport>(),
                //    racing => Parallel.ForEach(racing.Totes.Cast<Tote>(),
                //        tote =>
                //        {
                //            // receive tote data
                //            var strTote = tote.Id.Split('_');
                //            var toteId = CastAide.AsInt32(strTote[1]);

                //            // check is update PT
                //            CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);

                //            // set params bettype
                //            Parallel.ForEach(tote.Bettypes.Cast<Bettype>(), bettype =>
                //            {
                //                // receive bettype data
                //                var betId = CastAide.AsInt32(bettype.Id);
                //                var dataPt = bettype.PositionTaking;

                //                // reset PT
                //                if (dataPt.IsAutoPt)
                //                {
                //                    dataPt.Pt = 0;
                //                }
                //                else
                //                {
                //                    dataPt.AutoPt = 0;
                //                }
                //                // customer PT
                //                settingParams.Add(string.Format(_fieldSuperpt, racing.Id, betId), dataPt.Pt);

                //                // set auto PT
                //                settingParams.Add(string.Format(_fieldIsAutoSOnMaster, racing.Id, betId), dataPt.IsUplineAutoPt); // super auto PT on master
                //                settingParams.Add(string.Format(_fieldIsAutoSpt, racing.Id, betId), dataPt.IsAutoPt); // super auto PT on member
                //                settingParams.Add(string.Format(_fieldAutoSpt, racing.Id, betId), dataPt.AutoPt);

                //            });
                //        }
                //    )
                //);
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("username", username);
                settingParams.Add("errcode", "");
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private static Result UpdateMasterOnMember(Setting setting)
        {
            var result = new Result();
            try
            {
                var custId = CastAide.AsInt32(setting.CustInfo.CustId);
                var userId = AppSession.AccId;

                var settingParams = new Hashtable();
                // set cust param
                settingParams.Add("custid", custId);
                settingParams.Add("userid", userId);

                foreach (Sport racing in setting.RacingSportType)
                {
                    foreach (Tote tote in racing.Totes)
                    {
                        // receive tote data
                        // receive tote data
                        var strTote = tote.Id.Split('_');
                        var toteId = CastAide.AsInt32(strTote[1]);

                        // check is update PT
                        CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);
                        // set params bettype

                        foreach (Bettype bettype in tote.Bettypes)
                        {
                            // receive bettype data
                            var betId = CastAide.AsInt32(bettype.Id);

                            var dataPt = bettype.PositionTaking;

                            // customer PT
                            if (dataPt.IsAutoPt)
                            {
                                dataPt.Pt = 0;
                            }
                            else
                            {
                                dataPt.AutoPt = 0;
                            }
                            settingParams.Add(string.Format(_fieldIsAutoMpt, racing.Id, betId), dataPt.IsUplineAutoPt);
                            settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.Pt);

                            settingParams.Add(string.Format(_fieldIsAutoMOnMem, racing.Id, betId), dataPt.IsAutoPt);
                            settingParams.Add(string.Format(_fieldAutoMOnMemPt, racing.Id, betId), dataPt.AutoPt);
                        }
                    }
                }
                // get racing setting
                //Parallel.ForEach(setting.RacingSportType.Cast<Sport>(),
                //    racing => Parallel.ForEach(racing.Totes.Cast<Tote>(),
                //        tote =>
                //        {
                //            // receive tote data
                //            var strTote = tote.Id.Split('_');
                //            var toteId = CastAide.AsInt32(strTote[1]);

                //            // check is update PT
                //            CheckIsUpdate(ref settingParams, _fieldUpdPt, tote.UpdatePT);
                //            // set params bettype
                //            Parallel.ForEach(tote.Bettypes.Cast<Bettype>(), bettype =>
                //            {
                //                // receive bettype data
                //                var betId = CastAide.AsInt32(bettype.Id);

                //                var dataPt = bettype.PositionTaking;

                //                // customer PT
                //                if (dataPt.IsAutoPt)
                //                {
                //                    dataPt.Pt = 0;
                //                }
                //                else
                //                {
                //                    dataPt.AutoPt = 0;
                //                }
                //                settingParams.Add(string.Format(_fieldIsAutoMpt, racing.Id, betId), dataPt.IsUplineAutoPt);
                //                settingParams.Add(string.Format(_fieldMasterpt, racing.Id, betId), dataPt.Pt);

                //                settingParams.Add(string.Format(_fieldIsAutoMOnMem, racing.Id, betId), dataPt.IsAutoPt);
                //                settingParams.Add(string.Format(_fieldAutoMOnMemPt, racing.Id, betId), dataPt.AutoPt);

                //            });
                //        }
                //    )
                //);
                // static params
                var username = CastAide.AsString(setting.CustInfo.UserName);
                var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
                settingParams.Add("subaccid", 0);
                settingParams.Add("language", "EN");
                settingParams.Add("username", username);
                settingParams.Add("errcode", "");
                settingParams.Add("errmsg", "");
                settingParams.Add("ip", ipClient);

                result = _racingDao.UpdateRacingSetting(settingParams, _roleId);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        #endregion Update cust info

        private static string GetCyripting()
        {
            return IBCCrypt.IBCCrypt.GenMD5Key(_custId.ToString() + _roleId.ToString()) + IBCCrypt.IBCCrypt.GenMD5Key("^") + _custId + _roleId.ToString();
        }

        private static void CheckIsUpdate(ref Hashtable settingParams, string paramName, bool isUpdate)
        {
            if (!settingParams.ContainsKey(paramName))
            {
                settingParams.Add(paramName, false);
            }
            if (isUpdate)
            {
                settingParams[paramName] = isUpdate;
            }
        }
    }
}