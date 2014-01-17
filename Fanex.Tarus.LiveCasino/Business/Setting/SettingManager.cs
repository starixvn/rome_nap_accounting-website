using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using Fanex.LiveCasinoSetting.Business;
using Fanex.LiveCasinoSetting.Model;
using Fanex.LiveCasinoSetting.Model.Log;
using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.ViewModel;
using Fanex.Tarus.LiveCasino.DataAccess;
using Fanex.Tarus.LiveCasino.Entities;
using FluentValidation.Results;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using CustomerInfo = Fanex.Tarus.LiveCasino.Entities.CustomerInfo;
using GameType = Fanex.Tarus.LiveCasino.Entities.GameType;
using PTSetting = Fanex.Tarus.LiveCasino.Entities.PTSetting;
using RequestContext = Fanex.LiveCasinoSetting.Request.RequestContext;
using Result = Fanex.Tarus.LiveCasino.Entities.Result;
using StakeLevel = Fanex.Tarus.LiveCasino.Entities.StakeLevel;

namespace Fanex.Tarus.LiveCasino.Business.Setting
{
    public class SettingManager : BaseSetting, ISettingManager
    {
        private readonly ISettingService _settingService;
        private readonly IGameSettingService _gameSettingService;
        private readonly ILogWriterService _logService;
        private readonly ILimitSettingManager _limitSettingManager;
        bool IsUpdatedStatus = false;

        public SettingManager()
            : this(null, null, null, null)
        {

        }

        public SettingManager(ISettingService settingService, IGameSettingService gameSettingService, ILogWriterService logService, ILimitSettingManager limitSettingManager)
        {
            _settingService = settingService ?? new SettingService();
            _gameSettingService = gameSettingService ?? new GameSettingService();
            _logService = logService ?? new LogWriterService();
            _limitSettingManager = limitSettingManager ?? new LimitSettingManager();
        }

        public LiveCasinoSettingDTO GetSuperSetting(int custId, int exchangeId)
        {
            var result = new LiveCasinoSettingDTO();
            try
            {
                var superGameSetting = _gameSettingService.GetByCustomerId(custId);
                if (superGameSetting.Count > 0)
                    exchangeId = 0;

                var setting = _settingService.GetSuperSetting(custId, exchangeId);

                #region customer info
                result.CustomerInfo = new CustomerInfo()
                {
                    CustomerId = setting.CustomerInfo.CustomerId,
                    Fax = setting.CustomerInfo.Fax,
                    FirstName = setting.CustomerInfo.FirstName,
                    LastName = setting.CustomerInfo.LastName,
                    Mobile = setting.CustomerInfo.Mobile,
                    NickName = setting.CustomerInfo.NickName,
                    Phone = setting.CustomerInfo.Phone,
                    RoleId = setting.CustomerInfo.RoleId,
                    UserName = setting.CustomerInfo.UserName,
                    ExchangeId = setting.CustomerInfo.ExChangeId,
                    IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino,
                    Site = setting.CustomerInfo.Site,


                };
                #endregion

                #region pt setting
                if (setting != null)
                {
                    result.PTSetting = new PTSetting()
                    {
                        Comm = setting.PTSetting.Comm,
                        MinPT = setting.PTSetting.MinPT,
                        MaxPT = setting.PTSetting.MaxPT,
                        Upline = new PTSetting()
                        {
                            MinPT = setting.PTSetting.Upline != null ? setting.PTSetting.Upline.MinPT : 0,
                            MaxPT = setting.PTSetting.Upline != null ? setting.PTSetting.Upline.MaxPT : 0.9m,
                            Comm = setting.PTSetting.Upline != null ? setting.PTSetting.Upline.Comm : 0
                        }
                    };
                }

                #endregion

                #region game setting
                result.GameSetting = new List<GameType>();
                if (setting.GameSetting != null && setting.GameSetting.Count > 0)
                {
                    foreach (LiveCasinoSetting.Model.GameType gameSetting in setting.GameSetting)
                    {
                        var bettingLimit = new List<StakeLevel>();
                        foreach (LiveCasinoSetting.Model.StakeLevel limit in gameSetting.BettingLimit)
                        {
                            bettingLimit.Add(new StakeLevel()
                            {
                                Id = limit.Id,
                                IsSelected = limit.IsSelected,
                                Text = limit.Text
                            });

                        }
                        result.GameSetting.Add(new GameType()
                        {
                            Id = gameSetting.Id,
                            Name = CacheRef.GetBettype(gameSetting.BetTypeId),
                            BetTypeId = gameSetting.BetTypeId,
                            BettingLimit = bettingLimit
                        });
                    }
                }

                #endregion

                #region Win Limit Setting
                result.WinLimitSetting = new Entities.Setting.LimitSetting();
                result.WinLimitSetting.IsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoReset;
                result.WinLimitSetting.UplineIsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoResetUpline;

                result.WinLimitSetting.Limits = _limitSettingManager.SelWinLimitSetting(setting.CustomerInfo.Limit.MaxWinLimit, setting.CustomerInfo.Limit.MaxWinLimitUpline);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public LiveCasinoSettingDTO GetMasterSetting(int custId, int uplineId)
        {
            var setting = _settingService.GetMasterSetting(custId, uplineId);

            var result = new LiveCasinoSettingDTO();
            try
            {

                #region customer info
                result.CustomerInfo = new CustomerInfo()
                {
                    CustomerId = setting.CustomerInfo.CustomerId,
                    Fax = setting.CustomerInfo.Fax,
                    FirstName = setting.CustomerInfo.FirstName,
                    LastName = setting.CustomerInfo.LastName,
                    Mobile = setting.CustomerInfo.Mobile,
                    NickName = setting.CustomerInfo.NickName,
                    Phone = setting.CustomerInfo.Phone,
                    RoleId = setting.CustomerInfo.RoleId,
                    UserName = setting.CustomerInfo.UserName,
                    IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino,
                    ExchangeId = setting.CustomerInfo.ExChangeId,
                    uplineId = uplineId,

                };
                #endregion

                #region pt setting
                if (setting != null)
                {
                    var masterPtSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting;
                    var uplineSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline;
                    result.PTSetting = new PTSetting()
                    {
                        Comm = masterPtSetting.Comm,
                        MinPT = masterPtSetting.MinPT,
                        MaxPT = masterPtSetting.MaxPT,
                        Upline = new PTSetting()
                        {
                            MinPT = uplineSetting.MinPT,
                            MaxPT = uplineSetting.MaxPT,
                            Comm = uplineSetting.Comm,
                            PresetPT = uplineSetting.PresetPT,
                            AutoPresetPT = uplineSetting.AutoPresetPT,
                            IsAutoPresetPT = uplineSetting.IsAutoPresetPT,

                        }
                    };
                }

                #endregion

                #region game setting

                result.GameSetting = new List<GameType>();
                if (setting.GameSetting != null && setting.GameSetting.Count > 0)
                {
                    foreach (LiveCasinoSetting.Model.GameType gameSetting in setting.GameSetting)
                    {
                        var bettingLimit = new List<StakeLevel>();
                        foreach (LiveCasinoSetting.Model.StakeLevel limit in gameSetting.BettingLimit)
                        {
                            bettingLimit.Add(new StakeLevel()
                            {
                                Id = limit.Id,
                                IsSelected = limit.IsSelected,
                                Text = limit.Text
                            });
                        }
                        result.GameSetting.Add(new GameType()
                        {
                            Id = gameSetting.Id,
                            Name = RemotingRef.RefData.GetBetTypeName(gameSetting.BetTypeId.ToString(), "en"),
                            BetTypeId = gameSetting.BetTypeId,
                            BettingLimit = bettingLimit
                        });
                    }
                }
                #endregion

                #region Win Limit Setting
                result.WinLimitSetting = new Entities.Setting.LimitSetting();
                result.WinLimitSetting.IsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoReset;
                result.WinLimitSetting.UplineIsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoResetUpline;

                result.WinLimitSetting.Limits = _limitSettingManager.SelWinLimitSetting(setting.CustomerInfo.Limit.MaxWinLimit, setting.CustomerInfo.Limit.MaxWinLimitUpline);
                #endregion
            }
            catch
            {
            }
            return result;
        }

        public LiveCasinoSettingDTO GetAgentSetting(int custId, int uplineId)
        {
            var setting = _settingService.GetAgentSetting(custId, uplineId);

            var result = new LiveCasinoSettingDTO();
            try
            {
                #region customer info
                result.CustomerInfo = new CustomerInfo()
                {
                    CustomerId = setting.CustomerInfo.CustomerId,
                    Fax = setting.CustomerInfo.Fax,
                    FirstName = setting.CustomerInfo.FirstName,
                    LastName = setting.CustomerInfo.LastName,
                    Mobile = setting.CustomerInfo.Mobile,
                    NickName = setting.CustomerInfo.NickName,
                    Phone = setting.CustomerInfo.Phone,
                    RoleId = setting.CustomerInfo.RoleId,
                    UserName = setting.CustomerInfo.UserName,
                    IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino,
                    ExchangeId = setting.CustomerInfo.ExChangeId,
                    uplineId = uplineId
                };
                #endregion

                #region pt setting
                if (setting != null)
                {
                    var agentPtSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting;
                    var uplineSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline;

                    result.PTSetting = new PTSetting()
                    {
                        Comm = agentPtSetting.Comm,
                        MinPT = agentPtSetting.MinPT,
                        MaxPT = agentPtSetting.MaxPT,

                        Upline = new PTSetting()
                        {
                            MinPT = uplineSetting != null ? uplineSetting.MinPT : 0,
                            MaxPT = uplineSetting != null ? uplineSetting.MaxPT : 0,
                            Comm = uplineSetting != null ? uplineSetting.Comm : 0,
                            PresetPT = uplineSetting != null ? uplineSetting.PresetPT : 0,
                            AutoPresetPT = uplineSetting != null ? uplineSetting.AutoPresetPT : 0,
                            IsAutoPresetPT = uplineSetting != null ? uplineSetting.IsAutoPresetPT : false,
                            IsDisabledAutoPresetPT = uplineSetting != null ? uplineSetting.IsDisabledAutoPresetPT : false

                        }
                    };


                }

                #endregion

                #region game setting
                result.GameSetting = new List<GameType>();
                if (setting.GameSetting != null && setting.GameSetting.Count > 0)
                {
                    foreach (LiveCasinoSetting.Model.GameType gameSetting in setting.GameSetting)
                    {
                        var bettingLimit = new List<StakeLevel>();
                        foreach (LiveCasinoSetting.Model.StakeLevel limit in gameSetting.BettingLimit)
                        {
                            bettingLimit.Add(new StakeLevel()
                            {
                                Id = limit.Id,
                                IsSelected = limit.IsSelected,
                                Text = limit.Text
                            });
                        }
                        result.GameSetting.Add(new GameType()
                        {
                            Id = gameSetting.Id,
                            Name = RemotingRef.RefData.GetBetTypeName(gameSetting.BetTypeId.ToString(), "en"),
                            BetTypeId = gameSetting.BetTypeId,
                            BettingLimit = bettingLimit
                        });
                    }
                }
                #endregion

                #region Win Limit Setting
                result.WinLimitSetting = new Entities.Setting.LimitSetting();
                result.WinLimitSetting.IsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoReset;
                result.WinLimitSetting.UplineIsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoResetUpline;

                result.WinLimitSetting.Limits = _limitSettingManager.SelWinLimitSetting(setting.CustomerInfo.Limit.MaxWinLimit, setting.CustomerInfo.Limit.MaxWinLimitUpline);
                #endregion
            }
            catch
            {
            }
            return result;
        }

        public LiveCasinoSettingDTO GetMemberSetting(int custId, int uplineId)
        {
            var setting = _settingService.GetMemberSetting(custId, uplineId);
            var result = new LiveCasinoSettingDTO();
            try
            {

                #region customer info
                result.CustomerInfo = new CustomerInfo()
                {
                    CustomerId = setting.CustomerInfo.CustomerId,
                    Fax = setting.CustomerInfo.Fax,
                    FirstName = setting.CustomerInfo.FirstName,
                    LastName = setting.CustomerInfo.LastName,
                    Mobile = setting.CustomerInfo.Mobile,
                    NickName = setting.CustomerInfo.NickName,
                    Phone = setting.CustomerInfo.Phone,
                    RoleId = setting.CustomerInfo.RoleId,
                    UserName = setting.CustomerInfo.UserName,
                    IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino,
                    ExchangeId = setting.CustomerInfo.ExChangeId,
                    uplineId = uplineId,
                    DisableLiveCasino = setting.CustomerInfo.DisableLiveCasino,
                    UplineDisableLiveCasino = setting.CustomerInfo.UplineDisableLiveCasino,

                };
                #endregion

                #region pt setting
                if (setting != null)
                {
                    var agentPtSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting;
                    var uplineSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline;

                    result.PTSetting = new PTSetting()
                    {
                        Comm = agentPtSetting.Comm,
                        Upline = new PTSetting()
                        {
                            MinPT = uplineSetting != null ? uplineSetting.MinPT : 0,
                            MaxPT = uplineSetting != null ? uplineSetting.MaxPT : 0,
                            Comm = uplineSetting != null ? uplineSetting.Comm : 0,
                            PresetPT = uplineSetting != null ? uplineSetting.PresetPT : 0
                        }
                    };


                }

                #endregion

                #region game setting
                result.GameSetting = new List<GameType>();
                if (setting.GameSetting != null && setting.GameSetting.Count > 0)
                {
                    var gameSettingIndex = 0;
                    foreach (LiveCasinoSetting.Model.GameType gameSetting in setting.GameSetting)
                    {
                        var isHasSetting = false;
                        var bettingLimit = new List<StakeLevel>();

                        int cntSelected = gameSetting.BettingLimit.Where(s => s.IsSelected).Count();
                        foreach (LiveCasinoSetting.Model.StakeLevel limit in gameSetting.BettingLimit)
                        {
                            bool isDuplicateText = gameSetting.BettingLimit.Where(s => s.Text == limit.Text).Count() > 1;
                            if (isDuplicateText)
                            {
                                limit.Text = limit.Text + " ";
                            }
                            bettingLimit.Add(new StakeLevel()
                            {
                                Id = limit.Id,
                                IsSelected = (cntSelected >= 2)
                                    ? false
                                    : limit.IsSelected,
                                Text = limit.Text
                            });
                            if (cntSelected == 1)
                                isHasSetting = true;
                        }
                        result.GameSetting.Add(new GameType()
                        {
                            Id = gameSetting.Id,
                            Name = RemotingRef.RefData.GetBetTypeName(gameSetting.BetTypeId.ToString(), "en"),
                            BetTypeId = gameSetting.BetTypeId,
                            BettingLimit = bettingLimit,
                            IsUplineUnCheckAll = cntSelected == 0
                        });
                        if (!isHasSetting && bettingLimit.Count > 0)
                            result.GameSetting[gameSettingIndex].BettingLimit[0].IsSelected = true;



                        gameSettingIndex++;
                    }
                }


                #endregion

                #region Win Limit Setting
                result.WinLimitSetting = new Entities.Setting.LimitSetting();
                result.WinLimitSetting.IsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoReset;
                result.WinLimitSetting.UplineIsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoResetUpline;

                result.WinLimitSetting.Limits = _limitSettingManager.SelWinLimitSetting(setting.CustomerInfo.Limit.MaxWinLimit, setting.CustomerInfo.Limit.MaxWinLimitUpline);
                #endregion
            }
            catch
            {
            }
            return result;
        }

        public LiveCasinoSettingDTO GetSuperOnMemberSetting(int custId, int uplineId)
        {
            var setting = _settingService.SuperGetMemberSetting(custId, uplineId);
            var result = new LiveCasinoSettingDTO();
            try
            {

                #region customer info
                result.CustomerInfo = new CustomerInfo()
                {
                    CustomerId = setting.CustomerInfo.CustomerId,
                    Fax = setting.CustomerInfo.Fax,
                    FirstName = setting.CustomerInfo.FirstName,
                    LastName = setting.CustomerInfo.LastName,
                    Mobile = setting.CustomerInfo.Mobile,
                    NickName = setting.CustomerInfo.NickName,
                    Phone = setting.CustomerInfo.Phone,
                    RoleId = setting.CustomerInfo.RoleId,
                    ExchangeId = setting.CustomerInfo.ExChangeId,
                    UserName = setting.CustomerInfo.UserName
                };
                #endregion

                #region pt setting
                if (setting != null)
                {
                    var agentPtSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline;
                    var masterSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline.Upline;
                    var superSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline.Upline.Upline;
                    result.PTSetting = new PTSetting()
                    {
                        Upline = new PTSetting()
                        {
                            PresetPT = agentPtSetting != null ? agentPtSetting.PresetPT : 0,
                            Upline = new PTSetting()
                            {
                                PresetPT = masterSetting != null ? masterSetting.PresetPT : 0,
                                IsAutoPresetPT = masterSetting.IsAutoPresetPT,
                                Upline = new PTSetting()
                                {
                                    MinPT = superSetting != null ? superSetting.MinPT : 0,
                                    MaxPT = superSetting != null ? superSetting.MaxPT : 0,
                                    PresetPT = superSetting != null ? superSetting.PresetPT : 0,
                                    AutoPresetPT = superSetting != null ? superSetting.AutoPresetPT : 0,
                                    IsAutoPresetPT = superSetting != null ? superSetting.IsAutoPresetPT : false,
                                    MaxAutoPresetPT = superSetting != null ? superSetting.MaxAutoPresetPT : 0

                                }
                            }
                        }
                    };


                }

                #endregion

                #region game setting
                result.GameSetting = new List<GameType>();
                if (setting.GameSetting != null && setting.GameSetting.Count > 0)
                {
                    var gameSettingIndex = 0;
                    foreach (LiveCasinoSetting.Model.GameType gameSetting in setting.GameSetting)
                    {
                        var isHasSetting = false;
                        var bettingLimit = new List<StakeLevel>();
                        foreach (LiveCasinoSetting.Model.StakeLevel limit in gameSetting.BettingLimit)
                        {
                            bool isDuplicateText = gameSetting.BettingLimit.Where(s => s.Text == limit.Text).Count() > 1;
                            if (isDuplicateText)
                            {
                                limit.Text = limit.Text + " ";
                            }
                            bettingLimit.Add(new StakeLevel()
                            {
                                Id = limit.Id,
                                IsSelected = limit.IsSelected,
                                Text = limit.Text
                            });
                            if (limit.IsSelected)
                                isHasSetting = true;
                        }
                        result.GameSetting.Add(new GameType()
                        {
                            Id = gameSetting.Id,
                            Name = RemotingRef.RefData.GetBetTypeName(gameSetting.BetTypeId.ToString(), "en"),
                            BetTypeId = gameSetting.BetTypeId,
                            BettingLimit = bettingLimit,
                            IsUplineUnCheckAll = !isHasSetting
                        });
                        if (!isHasSetting)
                            result.GameSetting[gameSettingIndex].BettingLimit[0].IsSelected = true;
                        gameSettingIndex++;
                    }

                }
                #endregion

                #region Win Limit Setting
                result.WinLimitSetting = new Entities.Setting.LimitSetting();
                result.WinLimitSetting.IsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoReset;
                result.WinLimitSetting.UplineIsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoResetUpline;

                result.WinLimitSetting.Limits = _limitSettingManager.SelWinLimitSetting(setting.CustomerInfo.Limit.MaxWinLimit, setting.CustomerInfo.Limit.MaxWinLimitUpline);
                #endregion
            }
            catch
            {
            }
            return result;
        }

        public LiveCasinoSettingDTO GetMasterOnMemberSetting(int custId, int uplineId)
        {
            var setting = _settingService.MasterGetMemberSetting(custId, uplineId);
            var result = new LiveCasinoSettingDTO();
            try
            {

                #region customer info
                result.CustomerInfo = new CustomerInfo()
                {
                    CustomerId = setting.CustomerInfo.CustomerId,
                    Fax = setting.CustomerInfo.Fax,
                    FirstName = setting.CustomerInfo.FirstName,
                    LastName = setting.CustomerInfo.LastName,
                    Mobile = setting.CustomerInfo.Mobile,
                    NickName = setting.CustomerInfo.NickName,
                    Phone = setting.CustomerInfo.Phone,
                    RoleId = setting.CustomerInfo.RoleId,
                    ExchangeId = setting.CustomerInfo.ExChangeId,
                    UserName = setting.CustomerInfo.UserName
                };
                #endregion

                #region pt setting
                if (setting != null)
                {
                    var agentPtSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline;
                    var masterSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)setting.PTSetting.Upline.Upline;

                    result.PTSetting = new PTSetting()
                    {
                        Upline = new PTSetting()
                        {
                            PresetPT = agentPtSetting != null ? agentPtSetting.PresetPT : 0,
                            IsAutoPresetPT = agentPtSetting.IsAutoPresetPT,
                            Upline = new PTSetting()
                            {
                                MinPT = masterSetting != null ? masterSetting.MinPT : 0,
                                MaxPT = masterSetting != null ? masterSetting.MaxPT : 0,
                                PresetPT = masterSetting != null ? masterSetting.PresetPT : 0,
                                AutoPresetPT = masterSetting != null ? masterSetting.AutoPresetPT : 0,
                                IsAutoPresetPT = masterSetting != null ? masterSetting.IsAutoPresetPT : false,
                                IsDisabledAutoPresetPT = masterSetting != null ? masterSetting.IsDisabledAutoPresetPT : false,
                                MaxAutoPresetPT = masterSetting != null ? masterSetting.MaxAutoPresetPT : 0
                            }
                        }
                    };


                }

                #endregion

                #region game setting
                result.GameSetting = new List<GameType>();
                if (setting.GameSetting != null && setting.GameSetting.Count > 0)
                {
                    var gameSettingIndex = 0;
                    foreach (LiveCasinoSetting.Model.GameType gameSetting in setting.GameSetting)
                    {
                        var isHasSetting = false;
                        var bettingLimit = new List<StakeLevel>();
                        foreach (LiveCasinoSetting.Model.StakeLevel limit in gameSetting.BettingLimit)
                        {
                            bool isDuplicateText = gameSetting.BettingLimit.Where(s => s.Text == limit.Text).Count() > 1;
                            if (isDuplicateText)
                            {
                                limit.Text = limit.Text + " ";
                            }
                            bettingLimit.Add(new StakeLevel()
                            {
                                Id = limit.Id,
                                IsSelected = limit.IsSelected,
                                Text = limit.Text
                            });
                            if (limit.IsSelected)
                                isHasSetting = true;
                        }
                        result.GameSetting.Add(new GameType()
                        {
                            Id = gameSetting.Id,
                            Name = RemotingRef.RefData.GetBetTypeName(gameSetting.BetTypeId.ToString(), "en"),
                            BetTypeId = gameSetting.BetTypeId,
                            BettingLimit = bettingLimit,
                            IsUplineUnCheckAll = !isHasSetting
                        });
                        if (!isHasSetting)
                            result.GameSetting[gameSettingIndex].BettingLimit[0].IsSelected = true;
                        gameSettingIndex++;
                    }

                }
                #endregion

                #region Win Limit Setting
                result.WinLimitSetting = new Entities.Setting.LimitSetting();
                result.WinLimitSetting.IsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoReset;
                result.WinLimitSetting.UplineIsAutoResetLiveCasino = setting.CustomerInfo.Limit.IsDailyAutoResetUpline;

                result.WinLimitSetting.Limits = _limitSettingManager.SelWinLimitSetting(setting.CustomerInfo.Limit.MaxWinLimit, setting.CustomerInfo.Limit.MaxWinLimitUpline);
                #endregion
            }
            catch
            {
            }
            return result;
        }

        ResultDTO GetResultDTO(LiveCasinoSetting.Model.Result result, Fanex.LiveCasinoSetting.Model.SuperPTSetting ptSetting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();
            resultDto.IsSuccess = false;

            string[] keys = result.ErrorCode.Split('\a');
            string[] values = result.ErrorMessage.Split('\a');


            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].Length == 0) continue;
                if (DNAide.Core.CastAide.AsInt32(keys[i]) < 0)
                {
                    resultDto.IsSuccess = true;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = keys[i],
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "_" + DNAide.Core.CastAide.AsInt32(keys[i]) * (-1))
                    });
                }
                else if (keys[i].ToLower() == "cs0000")
                {
                    continue;
                }
                else
                {
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = keys[i],
                        Value = values[i]
                    });
                    resultDto.IsSuccess = false;
                    break;
                }
            }

            if (ptSetting != null && !ptSetting.ValidationResult.IsValid)
            {
                foreach (ValidationFailure error in ptSetting.ValidationResult.Errors)
                {
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = "1000",
                        Value = error.ErrorMessage.Replace("P T", "PT")
                    });
                }
            }



            return resultDto;
        }

        public ResultDTO UpdateSuperPTSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }

                IRequestContext requestContext = new RequestContext();
                SuperPTSetting ptSetting = (SuperPTSetting)_settingService.GetSuperPTSetting(setting.requestContext.CustId);

                ((INotifyPropertyChanged)ptSetting).PropertyChanged
                    += (o, ar) =>
                    {
                        if (ar.PropertyName == "Upline")
                        {
                            ((INotifyPropertyChanged)ptSetting.Upline).PropertyChanged += (upline, arg) =>
                            {
                                (ptSetting.Upline as SuperPTSetting).PropertiesChangedList.Add(arg.PropertyName);
                            };
                        }

                        if (!ptSetting.PropertiesChangedList.Contains(ar.PropertyName))
                        {
                            ptSetting.PropertiesChangedList.Add(ar.PropertyName);
                        }


                    };

                requestContext.SignUpIp = setting.requestContext.Ip;
                requestContext.CustId = setting.requestContext.CustId;
                requestContext.UserId = setting.requestContext.UserId;
                requestContext.Language = "E";
                requestContext.IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino;

                ptSetting.MaxPT = setting.PTSetting.MaxPT;
                ptSetting.MinPT = setting.PTSetting.MinPT;
                ptSetting.Comm = setting.PTSetting.Comm;

                LiveCasinoSetting.Model.Result result = _settingService.UpdateSuperPTSetting(requestContext, ptSetting);

                resultDto = GetResultDTO(result, ptSetting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return resultDto;

        }

        public ResultDTO UpdateGameSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO result = new ResultDTO();
            result.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    result.IsSuccess = false;
                    result.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return result;
                }

                int custid = setting.requestContext.CustId;

                #region Get Setting Info
                var gameTypeList = _gameSettingService.GetByCustomerId(custid);
                List<LiveCasinoSetting.Model.GameType> oldValues = new List<LiveCasinoSetting.Model.GameType>();

                foreach (var gameType in gameTypeList)
                {
                    LiveCasinoSetting.Model.GameType _gameType = new LiveCasinoSetting.Model.GameType()
                    {
                        Id = gameType.Id,
                        Name = gameType.Name,
                        BetTypeId = gameType.BetTypeId,
                    };
                    _gameType.BettingLimit = new List<LiveCasinoSetting.Model.StakeLevel>();

                    foreach (var stakeLevel in gameType.BettingLimit)
                    {
                        _gameType.BettingLimit.Add(new LiveCasinoSetting.Model.StakeLevel()
                        {
                            Id = stakeLevel.Id,
                            IsDirty = stakeLevel.IsDirty,
                            IsSelected = stakeLevel.IsSelected,
                            Text = stakeLevel.Text
                        });
                    }

                    oldValues.Add(_gameType);
                }
                #endregion

                #region tracking values
                gameTypeList
                    .ForEach(g => g.BettingLimit
                        .ForEach(b => ((INotifyPropertyChanged)b).PropertyChanged += (o, arg) =>
                        {
                            if ((arg.PropertyName == "IsSelected"))
                            {
                                (o as LiveCasinoSetting.Model.StakeLevel).IsDirty = (arg.PropertyName == "IsSelected");
                            }
                        }));
                if (gameTypeList.Count > 0)
                {
                    foreach (LiveCasinoSetting.Model.GameType gameType in gameTypeList)
                    {
                        foreach (var stakeLavel in gameType.BettingLimit)
                        {
                            var gameSetting = setting.GameSetting.Where(s => s.Id == gameType.Id).Select(
                                f => f.BettingLimit.Where(r => r.Id == stakeLavel.Id).Select(k => k.IsSelected));
                            if (gameSetting != null && gameSetting.Count() > 0)
                            {
                                var stakeSetting = gameSetting.ToList().FirstOrDefault();
                                if (stakeSetting != null && stakeSetting.Count() > 0)
                                {
                                    stakeLavel.IsSelected = stakeSetting.FirstOrDefault();
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (GameType gameType in setting.GameSetting)
                    {
                        var bettingLimit = new List<LiveCasinoSetting.Model.StakeLevel>();
                        foreach (var stakeLavel in gameType.BettingLimit)
                        {
                            var gameSetting = setting.GameSetting.Where(s => s.Id == gameType.Id).Select(
                                f => f.BettingLimit.Where(r => r.Id == stakeLavel.Id).Select(k => k.IsSelected));
                            if (gameSetting != null && gameSetting.Count() > 0)
                            {
                                var stakeSetting = gameSetting.ToList().FirstOrDefault();
                                if (stakeSetting != null && stakeSetting.Count() > 0)
                                {
                                    stakeLavel.IsSelected = stakeSetting.FirstOrDefault();
                                }
                            }

                            bettingLimit.Add(new LiveCasinoSetting.Model.StakeLevel()
                            {
                                Id = stakeLavel.Id,
                                IsSelected = stakeLavel.IsSelected,
                                Text = stakeLavel.Text,
                                IsDirty = true
                            });
                        }
                        gameTypeList.Add(new LiveCasinoSetting.Model.GameType() { Id = gameType.Id, BetTypeId = gameType.BetTypeId, Name = gameType.Name, BettingLimit = bettingLimit });
                    }
                }

                #endregion

                #region UpdateGameSetting
                var totalErr = 0;

                var liveCsAccess = new BaseDataAccess();
                CustomerStatusInfo _customerStatusInfo =
                    liveCsAccess.SelCustStatusInfo(setting.CustomerInfo.CustomerId, setting.CustomerInfo.UserName,
                                                   setting.requestContext.UserId);

                var request = new InsertAccountRequest()
                {
                    CustId = setting.CustomerInfo.CustomerId,
                    AgentId = setting.requestContext.AgentId,
                    CustUserName = setting.CustomerInfo.UserName,
                    ExchangeId = setting.CustomerInfo.ExchangeId,
                    SignUpIp = setting.requestContext.Ip,
                    IsDisabledLiveCasino = _customerStatusInfo.DisableLiveCs,
                    IsEnableDownline = _customerStatusInfo.IsEnableDownLines,
                    Language = "E",
                    MasterId = setting.requestContext.MasterId,
                    RoleId = setting.CustomerInfo.RoleId,
                    CustSite = setting.CustomerInfo.RoleId == 4 ? setting.CustomerInfo.Site : string.Empty,
                    UserName = AppSession.AccUserName,
                    SignUpForm = 1,
                    SubAccId = 0,
                    SuperId = setting.requestContext.SuperId,
                    UserId = setting.requestContext.UserId,
                    IsAutoInsert = false

                };

                var _result = _gameSettingService.UpdateGameSetting(setting.requestContext.UplineRoleId, gameTypeList, request);

                foreach (KeyValuePair<int, LiveCasinoSetting.Model.Result> keyValuePair in _result)
                {
                    bool NotExistedMsg = result.ErrorHandler.Where(s => s.Key == "-123456").Count() == 0;
                    if (keyValuePair.Value.ErrorCode.Length == 0 && NotExistedMsg)
                    {
                        string msg = string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UpdateLiveCasinoGameSettingSuccess"), CacheRef.GetBettype(keyValuePair.Key));
                        result.ErrorHandler.Add(new Result()
                                                    {
                                                        Key = keyValuePair.Value.ErrorCode,
                                                        Value = msg
                                                    });
                    }
                    else if (keyValuePair.Value.ErrorCode.ToLower() == "gs0000")
                    {
                        continue;
                    }
                    else if (keyValuePair.Value.ErrorMessage.Length > 0)
                    {
                        result.ErrorHandler.Add(new Result()
                                                    {
                                                        Key = "1200",
                                                        Value = keyValuePair.Value.ErrorMessage
                                                    });
                        totalErr++;
                    }
                }

                #region WriteLogUpdateStakeLevel

                var oldSettings = new List<LiveCasinoSetting.Model.GameType>();
                var newSettings = new List<LiveCasinoSetting.Model.GameType>();
                for (var i = 0; i < oldValues.Count && i < gameTypeList.Count; i++)
                {
                    var oldGameTypeValue = oldValues[i];
                    var newGameTypeValue = gameTypeList[i];

                    var isDifferent = false;
                    for (var j = 0; j < oldGameTypeValue.BettingLimit.Count &&
                        j < newGameTypeValue.BettingLimit.Count; j++)
                    {
                        var oldSelectedValue = oldGameTypeValue.BettingLimit[j].IsSelected;
                        var newSelectedValue = newGameTypeValue.BettingLimit[j].IsSelected;
                        if (oldSelectedValue != newSelectedValue)
                        {
                            isDifferent = true;
                            break;
                        }
                    }

                    if (isDifferent)
                    {
                        oldSettings.Add(oldGameTypeValue);
                        newSettings.Add(newGameTypeValue);
                    }
                }

                WriteLogUpdateStakeLevel(oldSettings, newSettings, _result, setting);

                #endregion
                #endregion

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }



            return result;
        }


        public ResultDTO UpdateMasterPTSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }

                IRequestContext requestContext = new RequestContext();

                var ptSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)_settingService.GetMasterPTSeting(setting.requestContext.CustId, setting.requestContext.UplineId);

                ((INotifyPropertyChanged)ptSetting).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!ptSetting.PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           ptSetting.PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };



                ((INotifyPropertyChanged)ptSetting.Upline).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!(ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           (ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };

                requestContext.SignUpIp = setting.requestContext.Ip;
                requestContext.CustId = setting.requestContext.CustId;
                requestContext.UserId = setting.requestContext.UserId;
                requestContext.UplineId = setting.requestContext.UplineId;
                requestContext.Language = "E";
                requestContext.IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino;

                var uplinePT = ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting;

                uplinePT.PresetPT = setting.PTSetting.Upline.PresetPT;
                uplinePT.IsAutoPresetPT = setting.PTSetting.Upline.IsAutoPresetPT;
                uplinePT.AutoPresetPT = setting.PTSetting.Upline.AutoPresetPT;
                uplinePT.IsUpdateAllMember = setting.PTSetting.Upline.IsUpdateAllMember;
                uplinePT.NewMemberPT = setting.PTSetting.Upline.NewMemberPT;
                ptSetting.MaxPT = setting.PTSetting.MaxPT;
                ptSetting.MinPT = setting.PTSetting.MinPT;
                ptSetting.Comm = setting.PTSetting.Comm;

                if (uplinePT.IsAutoPresetPT)
                {
                    uplinePT.PresetPT = 0;
                }
                else
                {
                    uplinePT.AutoPresetPT = 0;
                }

                ptSetting.Upline = uplinePT;

                var result = _settingService.UpdateMasterPTSetting(requestContext, ptSetting);

                resultDto = GetResultDTO(result, ptSetting);

            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }

            return resultDto;
        }



        public ResultDTO UpdateAgentPTSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }

                IRequestContext requestContext = new RequestContext();
                Fanex.LiveCasinoSetting.Model.PTSetting ptSetting = new Fanex.LiveCasinoSetting.Model.PTSetting();

                ptSetting =
                    (Fanex.LiveCasinoSetting.Model.PTSetting)
                    _settingService.GetAgentPTSetting(setting.requestContext.CustId, setting.requestContext.UplineId);

                #region tracking values
                ((INotifyPropertyChanged)ptSetting.Upline).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!(ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           (ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };
                ((INotifyPropertyChanged)ptSetting).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!ptSetting.PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           ptSetting.PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };
                #endregion

                requestContext.SignUpIp = setting.requestContext.Ip;
                requestContext.CustId = setting.requestContext.CustId;
                requestContext.UserId = setting.requestContext.UserId;
                requestContext.UplineId = setting.requestContext.UplineId;
                requestContext.Language = "E";
                requestContext.IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino;

                var uplinePT = ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting;

                uplinePT.PresetPT = setting.PTSetting.Upline.PresetPT;
                uplinePT.IsAutoPresetPT = setting.PTSetting.Upline.IsAutoPresetPT;
                uplinePT.AutoPresetPT = setting.PTSetting.Upline.AutoPresetPT;
                uplinePT.IsUpdateAllMember = setting.PTSetting.Upline.IsUpdateAllMember;
                uplinePT.NewMemberPT = setting.PTSetting.Upline.NewMemberPT;
                ptSetting.MaxPT = setting.PTSetting.MaxPT;
                ptSetting.MinPT = setting.PTSetting.MinPT;
                ptSetting.Comm = setting.PTSetting.Comm;

                if (!uplinePT.IsDisabledAutoPresetPT)
                {
                    if (uplinePT.IsAutoPresetPT)
                    {
                        uplinePT.PresetPT = 0;
                    }
                    else
                    {
                        uplinePT.AutoPresetPT = 0;
                    }
                }

                ptSetting.Upline = uplinePT;

                var result = _settingService.UpdateAgentPTSetting(requestContext, ptSetting);
                resultDto = GetResultDTO(result, ptSetting);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return resultDto;
        }


        public ResultDTO UpdatememberPTSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }

                IRequestContext requestContext = new RequestContext();
                Fanex.LiveCasinoSetting.Model.PTSetting ptSetting = new Fanex.LiveCasinoSetting.Model.PTSetting();

                ptSetting =
                    (Fanex.LiveCasinoSetting.Model.PTSetting)
                    _settingService.GetMemberPTSetting(setting.requestContext.CustId, setting.requestContext.UplineId);
                #region tracking values

                ((INotifyPropertyChanged)ptSetting.Upline).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!(ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           (ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };
                ((INotifyPropertyChanged)ptSetting).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!ptSetting.PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           ptSetting.PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };
                #endregion

                requestContext.SignUpIp = setting.requestContext.Ip;
                requestContext.CustId = setting.requestContext.CustId;
                requestContext.UserId = setting.requestContext.UserId;
                requestContext.UplineId = setting.requestContext.UplineId;
                requestContext.Language = "E";
                requestContext.IsSyncLiveCasino = setting.CustomerInfo.IsSyncLiveCasino;

                var uplinePT = ptSetting.Upline as Fanex.LiveCasinoSetting.Model.PTSetting;
                uplinePT.PresetPT = setting.PTSetting.Upline.PresetPT;

                ptSetting.Upline = uplinePT;
                ptSetting.Comm = setting.PTSetting.Comm;

                var result = _settingService.UpdateMemberPTSetting(requestContext, ptSetting);


                resultDto = GetResultDTO(result, ptSetting);

                #region Update Status
                ////System.Threading.Thread.Sleep(6000);;
                //var custId = setting.requestContext.CustId;
                //var uplineId = setting.requestContext.UplineId;
                //var custInfo = _settingService.GetMemberSetting(custId, uplineId).CustomerInfo;
                //if (custInfo.DisableLiveCasino != !setting.CustomerInfo.DisableLiveCasino)
                //{
                //    if (!IsUpdatedStatus)
                //    {
                //        resultDto.ErrorHandler.AddRange(UpdateStatusLiveCasino(setting).ErrorHandler);
                //        IsUpdatedStatus = true;
                //    }
                //}
                #endregion

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }


            return resultDto;
        }



        public ResultDTO SuperUpdateMemberPTSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }

                IRequestContext requestContext = new RequestContext();
                Fanex.LiveCasinoSetting.Model.PTSetting ptSetting = new Fanex.LiveCasinoSetting.Model.PTSetting();

                ptSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)_settingService.SuperGetMemberPTSetting(setting.requestContext.CustId, setting.requestContext.UplineId);
                #region tracking values
                ((INotifyPropertyChanged)ptSetting.Upline.Upline.Upline).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!(ptSetting.Upline.Upline.Upline as SuperPTSetting).PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           (ptSetting.Upline.Upline.Upline as SuperPTSetting).PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };

                ((INotifyPropertyChanged)ptSetting).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!ptSetting.PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           ptSetting.PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };
                #endregion
                requestContext.SignUpIp = setting.requestContext.Ip;
                requestContext.CustId = setting.requestContext.CustId;
                requestContext.UserId = setting.requestContext.UserId;
                requestContext.UplineId = setting.requestContext.UplineId;
                requestContext.Language = "E";

                var preset = setting.PTSetting.Upline.Upline.Upline.PresetPT;

                var upPtSetting = ptSetting.Upline.Upline.Upline as SuperPTSetting;
                upPtSetting.PresetPT = setting.PTSetting.Upline.Upline.Upline.PresetPT;
                upPtSetting.IsAutoPresetPT = setting.PTSetting.Upline.Upline.Upline.IsAutoPresetPT;
                upPtSetting.AutoPresetPT = setting.PTSetting.Upline.Upline.Upline.AutoPresetPT;

                if (upPtSetting.IsAutoPresetPT)
                {
                    upPtSetting.PresetPT = preset;
                }
                else
                {
                    upPtSetting.AutoPresetPT = 0;
                }

                ptSetting.Upline.Upline.Upline = upPtSetting;

                var result = _settingService.UpdateMemberPTSettingBySuper(requestContext, ptSetting);
                resultDto = GetResultDTO(result, ptSetting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }



            return resultDto;
        }



        public ResultDTO MasterUpdateMemberPTSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }

                IRequestContext requestContext = new RequestContext();
                Fanex.LiveCasinoSetting.Model.PTSetting ptSetting = (Fanex.LiveCasinoSetting.Model.PTSetting)_settingService.MasterGetMemberPTSetting(setting.requestContext.CustId, setting.requestContext.UplineId);
                #region tracking values
                ((INotifyPropertyChanged)ptSetting.Upline.Upline).PropertyChanged
                   += (o, ar) =>
                   {
                       if (!(ptSetting.Upline.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Contains(ar.PropertyName))
                       {
                           (ptSetting.Upline.Upline as Fanex.LiveCasinoSetting.Model.PTSetting).PropertiesChangedList.Add(ar.PropertyName);
                       }
                   };
                #endregion

                requestContext.SignUpIp = setting.requestContext.Ip;
                requestContext.CustId = setting.requestContext.CustId;
                requestContext.UserId = setting.requestContext.UserId;
                requestContext.UplineId = setting.requestContext.UplineId;
                requestContext.Language = "E";

                var upPtSetting = ptSetting.Upline.Upline as Fanex.LiveCasinoSetting.Model.PTSetting;
                upPtSetting.PresetPT = setting.PTSetting.Upline.Upline.PresetPT;
                upPtSetting.IsAutoPresetPT = setting.PTSetting.Upline.Upline.IsAutoPresetPT;
                upPtSetting.AutoPresetPT = setting.PTSetting.Upline.Upline.AutoPresetPT;

                if (upPtSetting.IsDisabledAutoPresetPT)
                {
                    upPtSetting.IsAutoPresetPT = false;
                    upPtSetting.AutoPresetPT = 0;
                }
                else if (upPtSetting.IsAutoPresetPT)
                {
                    upPtSetting.PresetPT = upPtSetting.AutoPresetPT;
                }
                else
                {
                    upPtSetting.AutoPresetPT = 0;
                }

                ptSetting.Upline.Upline = upPtSetting;

                var result = _settingService.UpdateMemberPTSettingByMaster(requestContext, ptSetting);
                resultDto = GetResultDTO(result, ptSetting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return resultDto;
        }


        #region Autot insert pt setting
        public Result AutoInsertPtSetting(CustomerInfoDTO customerInfo, int adminUserId, string adminUserName, int signupFrom)
        {
            var result = new Result();
            AutoInsertThridPartyPtSetting(customerInfo, adminUserId, adminUserName, signupFrom);
            return result;
        }

        #region call auto insert function
        public void AutoInsertThridPartyPtSetting(CustomerInfoDTO custInfo, int adminUserId, string adminUserName, int signupFrom)
        {
            signupFrom = 1;
            try
            {
                #region  auto insert
                var request = new InsertAccountRequest()
                {
                    CustId = custInfo.CustId,
                    AgentId = custInfo.AgentId,
                    CustUserName = custInfo.CustName,
                    ExchangeId = custInfo.ExchangeId,
                    SignUpIp = custInfo.Ip,
                    IsDisabledLiveCasino = custInfo.DisableLiveCs,
                    IsEnableDownline = custInfo.IsEnableDownLines,
                    Language = custInfo.Language,
                    MasterId = custInfo.MasterId,
                    RoleId = custInfo.RoleId,
                    UserName = adminUserName,
                    SignUpForm = signupFrom,
                    SubAccId = 0,
                    SuperId = custInfo.SuperId,
                    UserId = adminUserId,
                    IsAutoInsert = true

                };
                DataTable dtResult = new DataTable();
                var setting = _gameSettingService.InsertAccount(request, ref dtResult);
                string errCode = setting.ErrorCode ?? string.Empty;
                string errMsg = setting.ErrorMessage ?? string.Empty;
                WriteLogAutoInsertAccount(request, dtResult, errCode, errMsg);

                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private Fanex.LiveCasinoSetting.Model.Result ManualInsertThridPartyPtSetting(CustomerInfoDTO custInfo)
        {
            var signupFrom = 1;
            var userId = AppSession.AccId;
            var userName = AppSession.Username;
            Fanex.LiveCasinoSetting.Model.Result result = new Fanex.LiveCasinoSetting.Model.Result();
            custInfo.Language = "E";
            try
            {
                #region  auto insert
                var request = new InsertAccountRequest()
                {
                    CustId = custInfo.CustId,
                    AgentId = custInfo.AgentId,
                    CustUserName = custInfo.CustName,
                    ExchangeId = custInfo.ExchangeId,
                    SignUpIp = custInfo.Ip,
                    IsDisabledLiveCasino = custInfo.DisableLiveCs,
                    IsEnableDownline = custInfo.IsEnableDownLines,
                    Language = custInfo.Language,
                    MasterId = custInfo.MasterId,
                    RoleId = custInfo.RoleId,
                    UserName = userName,
                    SignUpForm = signupFrom,
                    SubAccId = 0,
                    SuperId = custInfo.SuperId,
                    UserId = userId,
                    IsAutoInsert = false

                };

                result = _gameSettingService.InsertAccount(request);

                WriteLogManualInsertAccount(request);

                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        #endregion
        #endregion

        #region write log
        void WriteLogAutoInsertAccount(InsertAccountRequest request, DataTable dtResult, string errCode, string errMsg)
        {
            try
            {
                LogAutoInsertAccount log = new LogAutoInsertAccount();
                log.UserId = request.UserId;
                log.UserName = request.UserName;
                log.SignUpIp = request.SignUpIp;
                log.Language = request.Language;
                log.CustId = request.CustId;
                log.ExchangeId = request.ExchangeId;
                log.CustUserName = request.CustUserName;
                log.SignUpForm = 01;
                log.SignUpIp = request.SignUpIp;
                log.dataResult = dtResult;
                log.CustomerList = new List<KeyValuePair<int, string>>();
                log.CustomerList.Add(new KeyValuePair<int, string>(log.CustId, log.CustUserName));
                log.RoleId = request.RoleId;
                log.SuperId = request.SuperId;
                log.MasterId = request.MasterId;
                log.AgentId = request.AgentId;

                log.ErrorCode = errCode;
                log.ErrorMessage = errMsg;

                switch (request.RoleId)
                {
                    case 4:
                        log.SuperId = request.CustId;
                        break;
                    case 3:
                        log.Action = LogActions.AutoInsert_Master;
                        log.MasterId = request.CustId;
                        break;
                    case 2:
                        log.Action = LogActions.AutoInsert_Agent;
                        log.AgentId = request.CustId;
                        break;
                    case 1:
                        log.Action = LogActions.AutoInsert_Member;
                        break;
                }



                _logService.WriteLog((ILogData)log);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

        }

        void WriteLogManualInsertAccount(InsertAccountRequest request)
        {
            try
            {
                LogManualInsertAccount log = new LogManualInsertAccount();
                log.UserId = request.UserId;
                log.UserName = request.UserName;
                log.SignUpIp = request.SignUpIp;
                log.Language = request.Language;
                log.CustId = request.CustId;
                log.ExchangeId = request.ExchangeId;
                log.CustUserName = request.CustUserName;
                log.SignUpForm = 01;
                log.SignUpIp = request.SignUpIp;
                log.CustomerList = new List<KeyValuePair<int, string>>();
                log.CustomerList.Add(new KeyValuePair<int, string>(log.CustId, log.CustUserName));
                log.RoleId = request.RoleId;
                log.SuperId = request.SuperId;
                log.MasterId = request.MasterId;
                log.AgentId = request.AgentId;

                switch (request.RoleId)
                {
                    case 4:
                        log.SuperId = request.CustId;
                        log.Action = LogActions.ManualInsert_Super;
                        break;
                    case 3:
                        log.Action = LogActions.ManualInsert_Master;
                        log.MasterId = request.CustId;
                        break;
                    case 2:
                        log.Action = LogActions.ManualInsert_Agent;
                        log.AgentId = request.CustId;
                        break;
                    case 1:
                        log.Action = LogActions.ManualInsert_Member;
                        break;
                }



                _logService.WriteLog((ILogData)log);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

        }

        void WriteLogUpdateStakeLevel(
            List<LiveCasinoSetting.Model.GameType> oldGameTypeValues,
            List<LiveCasinoSetting.Model.GameType> newGameTypeValues,
            Dictionary<int, LiveCasinoSetting.Model.Result> resultDictionary,
            LiveCasinoSettingDTO setting)
        {
            try
            {
                LogUpdateStakeLevel log = new LogUpdateStakeLevel(oldGameTypeValues, newGameTypeValues, resultDictionary);
                log.CustId = setting.CustomerInfo.CustomerId;
                log.SignUpIp = setting.requestContext.Ip;
                log.Language = "E";
                log.UserId = setting.requestContext.UserId;

                int roleId = setting.CustomerInfo.RoleId;

                log.RoleId = roleId;

                switch (roleId)
                {
                    case 4:
                        log.Action = LogActions.UpdateStakeLevel_Super;
                        break;
                    case 3:
                        log.Action = LogActions.UpdateStakeLevel_Master;
                        break;
                    case 2:
                        log.Action = LogActions.UpdateStakeLevel_Agent;
                        break;
                    case 1:
                        log.Action = LogActions.UpdateStakeLevel_Member;
                        break;
                }
                _logService.WriteLog((ILogData)log);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }


        }

        #endregion

        public Result UpdateCustomerSyncInfo(LiveCasinoSettingDTO currentSetting)
        {



            var result = new Result();
            try
            {
                var request = new UpdateCustomerSyncInfo()
                                  {
                                      CustId = currentSetting.CustomerInfo.CustomerId,
                                      SyncLiveCasino = true,
                                      UserId = currentSetting.requestContext.UserId,
                                      SubAccId = 0,
                                      SignUpIp = currentSetting.requestContext.Ip,
                                      Language = "E"
                                  };
                var setting = _settingService.UpdateCustomerSyncInfo(request);
                result.Key = setting.ErrorCode;
                result.Value = setting.ErrorMessage;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public bool GetSyncLiveCasinoStatus(int custId, string userName, int userId)
        {
            var liveCsAccess = new BaseDataAccess();
            try
            {
                return liveCsAccess.GetSyncLiveCasinoStatus(custId, userName, userId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }


        public SettingViewModel SelSuperSetting(int custId, int exchangeId)
        {
            SettingViewModel settingView = null;
            try
            {
                settingView = _settingService.GetSuperSetting(custId, exchangeId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return settingView;
        }

        public ResultDTO UpdateStatusLiveCasino(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }
                InsertAccountRequest insertAccount = new InsertAccountRequest();
                UpdateLiveCasinoStatusRequest request = new UpdateLiveCasinoStatusRequest(insertAccount);

                request.CustId = setting.CustomerInfo.CustomerId;
                request.IsDisabledLiveCasino = !setting.CustomerInfo.DisableLiveCasino;
                if (setting.CustomerInfo.RoleId == 1)
                    request.IsEnableLiveCasinoForDownline = false;
                else
                    request.IsEnableLiveCasinoForDownline = !setting.CustomerInfo.DisableLiveCasino;
                request.UserId = setting.requestContext.UserId;
                request.SignUpIp = setting.requestContext.Ip;
                request.Language = "E";

                var rs = _settingService.UpdateStatusLiveCasino(request);

                string key = rs.ErrorCode;
                string value = rs.ErrorMessage;

                if (key.Length == 0)
                {
                    key = "-100";
                    value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UpdateLiveCasinoStatusSuccess");
                }
                else
                {
                    key = "100";
                    value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UpdateLiveCasinoStatusUnSuccess");
                }

                resultDto.ErrorHandler = new List<Result>();
                resultDto.ErrorHandler.Add(new Result()
                                               {
                                                   Key = key,
                                                   Value = value
                                               });


            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return resultDto;
        }


        public ResultDTO UpdateSuperLiveCasino(LiveCasinoSettingDTO setting)
        {
            ResultDTO result = new ResultDTO();
            result.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("customer", AppSession.RoleAccess))
                {
                    result.IsSuccess = false;
                    result.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return result;
                }

                if (setting.isAllowUpdatePtSetting)
                {
                    result.ErrorHandler.AddRange(UpdateSuperPTSetting(setting).ErrorHandler);
                }
                if (setting.isAllowUpdateWinLimit)
                {
                    result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                }
                if (setting.isAllowUpdateGameSetting)
                {
                    result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public ResultDTO UpdateWinlimitSetting(LiveCasinoSettingDTO setting)
        {
            ResultDTO resultDto = new ResultDTO();
            resultDto.ErrorHandler = new List<Result>();
            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return resultDto;
                }
                Result result = _limitSettingManager.UpdateWinLimit(setting.CustomerInfo.CustomerId, setting.WinLimitSetting.MaxWinLiveCasino, setting.WinLimitSetting.IsAutoResetLiveCasino);
                if (result.Key == "0")
                {
                    result.IsSuccess = true;
                    result.Key = "-104";
                    result.Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "_" + DNAide.Core.CastAide.AsInt32(104));
                }

                resultDto.IsSuccess = true;
                resultDto.ErrorHandler.Add(result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return resultDto;

        }

        public ResultDTO UpdateMasterLiveCasino(LiveCasinoSettingDTO setting)
        {
            ResultDTO result = new ResultDTO();
            result.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    result.IsSuccess = false;
                    result.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return result;
                }
                if (!setting.CustomerInfo.IsSyncLiveCasino)
                {
                    var rsPT = UpdateMasterPTSetting(setting);
                    result.ErrorHandler.AddRange(rsPT.ErrorHandler);

                    if (rsPT.IsSuccess)
                    {
                        result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                        result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                    }
                    else
                    {
                        result.ErrorHandler.Add(new Result()
                        {
                            Key = "1000",
                            Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UpdateLiveCasinoGameSettingUnSuccess")
                        });
                    }
                }
                else
                {
                    if (setting.isAllowUpdatePtSetting)
                    {
                        result.ErrorHandler.AddRange(UpdateMasterPTSetting(setting).ErrorHandler);
                    }
                    if (setting.isAllowUpdateWinLimit)
                    {
                        result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                    }
                    if (setting.isAllowUpdateGameSetting)
                    {
                        result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                    }

                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public ResultDTO UpdateAgentLiveCasino(LiveCasinoSettingDTO setting)
        {
            ResultDTO result = new ResultDTO();
            result.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    result.IsSuccess = false;
                    result.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return result;
                }
                if (!setting.CustomerInfo.IsSyncLiveCasino)
                {
                    var rsPT = UpdateAgentPTSetting(setting);
                    result.ErrorHandler.AddRange(rsPT.ErrorHandler);

                    if (rsPT.IsSuccess)
                    {
                        result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                        result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                    }
                    else
                    {
                        result.ErrorHandler.Add(new Result()
                        {
                            Key = "1000",
                            Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UpdateLiveCasinoGameSettingUnSuccess")
                        });
                    }
                }
                else
                {
                    if (setting.isAllowUpdatePtSetting)
                    {
                        result.ErrorHandler.AddRange(UpdateAgentPTSetting(setting).ErrorHandler);
                    }
                    if (setting.isAllowUpdateGameSetting)
                    {
                        result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                    }
                    if (setting.isAllowUpdateWinLimit)
                    {
                        result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                    }

                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public ResultDTO UpdateMemberLiveCasino(LiveCasinoSettingDTO setting)
        {
            ResultDTO result = new ResultDTO();
            result.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    result.IsSuccess = false;
                    result.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return result;
                }
                if (!setting.CustomerInfo.IsSyncLiveCasino)
                {
                    var rsPT = UpdatememberPTSetting(setting);
                    result.ErrorHandler.AddRange(rsPT.ErrorHandler);

                    if (rsPT.IsSuccess)
                    {
                        result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                        result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                    }
                    else
                    {
                        result.ErrorHandler.Add(new Result()
                        {
                            Key = "1000",
                            Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UpdateLiveCasinoGameSettingUnSuccess")
                        });
                    }
                }
                else
                {
                    if (setting.isAllowUpdatePtSetting)
                    {
                        result.ErrorHandler.AddRange(UpdatememberPTSetting(setting).ErrorHandler);
                    }
                    if (setting.isAllowUpdateWinLimit)
                    {
                        result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                    }
                    if (setting.isAllowUpdateGameSetting)
                    {
                        result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                    }
                }

                #region Update Status
                //System.Threading.Thread.Sleep(3000); ;
                var custId = setting.requestContext.CustId;
                var uplineId = setting.requestContext.UplineId;
                var custInfo = _settingService.GetMemberSetting(custId, uplineId).CustomerInfo;
                if (custInfo.DisableLiveCasino != !setting.CustomerInfo.DisableLiveCasino)
                {
                    if (!IsUpdatedStatus)
                    {
                        result.ErrorHandler.AddRange(UpdateStatusLiveCasino(setting).ErrorHandler);
                        IsUpdatedStatus = true;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }



        public ResultDTO UpdateMasterOnMemberLiveCasino(LiveCasinoSettingDTO setting)
        {
            ResultDTO result = new ResultDTO();
            result.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    result.IsSuccess = false;
                    result.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return result;
                }
                if (setting.isAllowUpdatePtSetting)
                {
                    result.ErrorHandler.AddRange(MasterUpdateMemberPTSetting(setting).ErrorHandler);
                }
                if (setting.isAllowUpdateWinLimit)
                {
                    result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                }
                if (setting.isAllowUpdateGameSetting)
                {
                    result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }



        public ResultDTO UpdateSuperOnMemberLiveCasino(LiveCasinoSettingDTO setting)
        {
            ResultDTO result = new ResultDTO();
            result.ErrorHandler = new List<Result>();

            try
            {
                if (!GetHostPermission("Customer", AppSession.RoleAccess))
                {
                    result.IsSuccess = false;
                    result.ErrorHandler.Add(new Result()
                    {
                        Key = Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized.ToString(),
                        Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"),
                        IsSuccess = false
                    });
                    return result;
                }
                if (setting.isAllowUpdatePtSetting)
                {
                    result.ErrorHandler.AddRange(SuperUpdateMemberPTSetting(setting).ErrorHandler);
                }
                if (setting.isAllowUpdateWinLimit)
                {
                    result.ErrorHandler.AddRange(UpdateWinlimitSetting(setting).ErrorHandler);
                }
                if (setting.isAllowUpdateGameSetting)
                {
                    result.ErrorHandler.AddRange(UpdateGameSetting(setting).ErrorHandler);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;


        }


    }
}
