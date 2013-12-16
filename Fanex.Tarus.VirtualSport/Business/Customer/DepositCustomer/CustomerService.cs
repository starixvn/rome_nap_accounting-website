// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.VirtualSport.Business.Customer.DepositCustomer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.VirtualSport.DataAccess.Customer.DepositCustomer;
    using Fanex.Tarus.VirtualSport.Entities;
    using Fanex.Tarus.VirtualSport.Entities.Customer.DepositCustomer;    
    
    public class CustomerService : ICustomerService
    {
        private static ICustomerAccess customerAccess;

        public CustomerService(ICustomerAccess dataAccess)
        {
            customerAccess = dataAccess ?? new CustomerAccess();
        }

        public CustomerService():this(null)
        {
        }

        #region super
        public Result AddNewSuperSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.AddNewSuperSettings(requestParams);                           
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }

        public CustomerSettings GetSuperSetting(int custId)
        {
            var result = new CustomerSettings();
            try
            {
                result.CustId = custId;
                result.RoleId = (int)Enumerations.Role.Super;
                result.Settings = new List<CustomerSettingDetail>();
                var virtualSports = VirtualSportsSection.Instance.Sports;
                foreach (VirtualSportsElement sport in virtualSports)
                {
                    var sportId = CastAide.AsInt32(sport.Id);
                    if (sport.Active)
                    {
                        var sportName = sport.Name;
                        var sportSetting = customerAccess.GetSuperSetting(custId, sportId);
                        if (sportSetting != null && sportSetting.Tables.Count > 0 && sportSetting.Tables[0].Rows.Count > 0)
                        {
                            var settings = sportSetting.Tables[0].Rows;
                            foreach (DataRow setting in settings)
                            {
                                var customerSettingDetail = new CustomerSettingDetail()
                                {
                                    SportId = sportId,
                                    SportName = sportName,
                                    MinBet = CastAide.AsDouble(setting["minbet"]),
                                    MaxBet = CastAide.AsDouble(setting["maxbet"]),
                                    MaxPerMatch = CastAide.AsDouble(setting["maxpermatch"]),
                                    SuperPositionTaking = CastAide.AsDouble(setting["spositiontaking"])
                                };
                                result.Settings.Add(customerSettingDetail);
                            }
                        }
                        else
                        {
                            var customerSettingDetail = new CustomerSettingDetail()
                            {
                                SportId = sportId,
                                SportName = sportName,
                                MinBet = CastAide.AsDouble(sport.Minbet),
                                MaxBet = CastAide.AsDouble(sport.Maxbet),
                                MaxPerMatch = CastAide.AsDouble(sport.Maxpermatch),
                                SuperPositionTaking = 0
                            };
                            result.Settings.Add(customerSettingDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public Result UpdateSuperSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.UpdateSuperSettings(requestParams);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }
        #endregion

        #region master
        public Result AddNewMasterSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.AddNewMasterSettings(requestParams);
                return result;
            }
            catch (Exception ex)
            {                
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }

        public CustomerSettings GetMasterSetting(int custId)
        {
            var result = new CustomerSettings();
            try
            {
                result.CustId = custId;
                result.RoleId = (int)Enumerations.Role.Master;
                result.Settings = new List<CustomerSettingDetail>();
                var virtualSports = VirtualSportsSection.Instance.Sports;
                foreach (VirtualSportsElement sport in virtualSports)
                {
                    var sportId = CastAide.AsInt32(sport.Id);
                    if (sport.Active)
                    {
                        var sportName = sport.Name;
                        var sportSetting = customerAccess.GetMasterSettings(custId, sportId);
                        if (sportSetting != null && sportSetting.Tables.Count > 0 && sportSetting.Tables[0].Rows.Count > 0)
                        {
                            var settings = sportSetting.Tables[0].Rows;
                            foreach (DataRow setting in settings)
                            {
                                var customerSettingDetail = new CustomerSettingDetail()
                                {
                                    SportId = sportId,
                                    SportName = sportName,
                                    MinBet = CastAide.AsDouble(setting["minbet"]),
                                    MinBetMax = CastAide.AsDouble(setting["minbet_max"]),
                                    MaxBet = CastAide.AsDouble(setting["maxbet"]),
                                    MaxBetMin = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxPerMatch = CastAide.AsDouble(setting["maxpermatch"]),
                                    MaxPerMatchMax = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    MasterPositionTaking = CastAide.AsDouble(setting["mpositiontaking"]),
                                    MasterPositionTakingMax = CastAide.AsDouble(setting["mpositiontaking_max"])
                                };
                                result.Settings.Add(customerSettingDetail);
                            }
                        }
                        else
                        {
                            var customerSettingDetail = new CustomerSettingDetail()
                            {
                                SportId = sportId,
                                SportName = sportName,
                                MinBet = CastAide.AsDouble(sport.Minbet),
                                MinBetMax = CastAide.AsDouble(sport.Minbet),
                                MaxBet = CastAide.AsDouble(sport.Maxbet),
                                MaxBetMin = CastAide.AsDouble(sport.Maxbet),
                                MaxPerMatch = CastAide.AsDouble(sport.Maxpermatch),
                                MaxPerMatchMax = CastAide.AsDouble(sport.Maxpermatch),
                                MasterPositionTaking = 0,
                                MasterPositionTakingMax = 0
                            };
                            result.Settings.Add(customerSettingDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public CustomerSettings GetMasterSettingForAddNew(int uplineId)
        {
            var result = new CustomerSettings();
            try
            {
                result.CustId = uplineId;
                result.RoleId = (int)Enumerations.Role.Master;
                result.Settings = new List<CustomerSettingDetail>();
                var virtualSports = VirtualSportsSection.Instance.Sports;
                foreach (VirtualSportsElement sport in virtualSports)
                {
                    var sportId = CastAide.AsInt32(sport.Id);
                    if (sport.Active)
                    {
                        var sportName = sport.Name;
                        var sportSetting = customerAccess.GetMasterSettingsForAddNew(uplineId, sportId);
                        if (sportSetting != null && sportSetting.Tables.Count > 0 && sportSetting.Tables[0].Rows.Count > 0)
                        {
                            var settings = sportSetting.Tables[0].Rows;
                            foreach (DataRow setting in settings)
                            {
                                var customerSettingDetail = new CustomerSettingDetail()
                                {
                                    SportId = sportId,
                                    SportName = sportName,
                                    MinBet = CastAide.AsDouble(setting["minbet_max"]),
                                    MinBetMax = CastAide.AsDouble(setting["minbet_max"]),
                                    MaxBet = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxBetMin = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxPerMatch = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    MaxPerMatchMax = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    MasterPositionTaking = CastAide.AsDouble(setting["mpositiontaking_max"]),
                                    MasterPositionTakingMax = CastAide.AsDouble(setting["mpositiontaking_max"])
                                };
                                result.Settings.Add(customerSettingDetail);
                            }
                        }
                        else
                        {
                            var customerSettingDetail = new CustomerSettingDetail()
                            {
                                SportId = sportId,
                                SportName = sportName,
                                MinBet = CastAide.AsDouble(sport.Minbet),
                                MinBetMax = CastAide.AsDouble(sport.Minbet),
                                MaxBet = CastAide.AsDouble(sport.Maxbet),
                                MaxBetMin = CastAide.AsDouble(sport.Maxbet),
                                MaxPerMatch = CastAide.AsDouble(sport.Maxpermatch),
                                MaxPerMatchMax = CastAide.AsDouble(sport.Maxpermatch),
                                MasterPositionTaking = 0,
                                MasterPositionTakingMax = 0
                            };
                            result.Settings.Add(customerSettingDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public Result UpdateMasterSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.UpdateMasterSettings(requestParams);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }
        #endregion

        #region agent
        public Result AddNewAgentSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.AddNewAgentSettings(requestParams);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }

        public CustomerSettings GetAgentSetting(int custId)
        {
            var result = new CustomerSettings();
            try
            {
                result.CustId = custId;
                result.RoleId = (int)Enumerations.Role.Master;
                result.Settings = new List<CustomerSettingDetail>();
                var virtualSports = VirtualSportsSection.Instance.Sports;
                foreach (VirtualSportsElement sport in virtualSports)
                {
                    var sportId = CastAide.AsInt32(sport.Id);
                    if (sport.Active)
                    {
                        var sportName = sport.Name;
                        var sportSetting = customerAccess.GetAgentSettings(custId, sportId);
                        if (sportSetting != null && sportSetting.Tables.Count > 0 && sportSetting.Tables[0].Rows.Count > 0)
                        {
                            var settings = sportSetting.Tables[0].Rows;
                            foreach (DataRow setting in settings)
                            {
                                var customerSettingDetail = new CustomerSettingDetail()
                                {
                                    SportId = sportId,
                                    SportName = sportName,
                                    MinBet = CastAide.AsDouble(setting["minbet"]),
                                    MinBetMax = CastAide.AsDouble(setting["minbet_max"]),
                                    MaxBet = CastAide.AsDouble(setting["maxbet"]),
                                    MaxBetMin = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxPerMatch = CastAide.AsDouble(setting["maxpermatch"]),
                                    MaxPerMatchMax = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    AgentPositionTaking = CastAide.AsDouble(setting["apositiontaking"]),
                                    AgentPositionTakingMax = CastAide.AsDouble(setting["apositiontaking_max"])
                                };
                                result.Settings.Add(customerSettingDetail);
                            }
                        }
                        else
                        {
                            var customerSettingDetail = new CustomerSettingDetail()
                            {
                                SportId = sportId,
                                SportName = sportName,
                                MinBet = CastAide.AsDouble(sport.Minbet),
                                MinBetMax = CastAide.AsDouble(sport.Minbet),
                                MaxBet = CastAide.AsDouble(sport.Maxbet),
                                MaxBetMin = CastAide.AsDouble(sport.Maxbet),
                                MaxPerMatch = CastAide.AsDouble(sport.Maxpermatch),
                                MaxPerMatchMax = CastAide.AsDouble(sport.Maxpermatch),
                                AgentPositionTaking = 0,
                                AgentPositionTakingMax = 0
                            };
                            result.Settings.Add(customerSettingDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public CustomerSettings GetAgentSettingForAddNew(int uplineId)
        {
            var result = new CustomerSettings();
            try
            {
                result.CustId = uplineId;
                result.RoleId = (int)Enumerations.Role.Master;
                result.Settings = new List<CustomerSettingDetail>();
                var virtualSports = VirtualSportsSection.Instance.Sports;
                foreach (VirtualSportsElement sport in virtualSports)
                {
                    var sportId = CastAide.AsInt32(sport.Id);
                    if (sport.Active)
                    {
                        var sportName = sport.Name;
                        var sportSetting = customerAccess.GetAgentSettingsForAddNew(uplineId, sportId);
                        if (sportSetting != null && sportSetting.Tables.Count > 0 && sportSetting.Tables[0].Rows.Count > 0)
                        {
                            var settings = sportSetting.Tables[0].Rows;
                            foreach (DataRow setting in settings)
                            {
                                var customerSettingDetail = new CustomerSettingDetail()
                                {
                                    SportId = sportId,
                                    SportName = sportName,
                                    MinBet = CastAide.AsDouble(setting["minbet_max"]),
                                    MinBetMax = CastAide.AsDouble(setting["minbet_max"]),
                                    MaxBet = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxBetMin = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxPerMatch = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    MaxPerMatchMax = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    AgentPositionTaking = CastAide.AsDouble(setting["apositiontaking_max"]),
                                    AgentPositionTakingMax = CastAide.AsDouble(setting["apositiontaking_max"])
                                };
                                result.Settings.Add(customerSettingDetail);
                            }
                        }
                        else
                        {
                            var customerSettingDetail = new CustomerSettingDetail()
                            {
                                SportId = sportId,
                                SportName = sportName,
                                MinBet = CastAide.AsDouble(sport.Minbet),
                                MinBetMax = CastAide.AsDouble(sport.Minbet),
                                MaxBet = CastAide.AsDouble(sport.Maxbet),
                                MaxBetMin = CastAide.AsDouble(sport.Maxbet),
                                MaxPerMatch = CastAide.AsDouble(sport.Maxpermatch),
                                MaxPerMatchMax = CastAide.AsDouble(sport.Maxpermatch),
                                AgentPositionTaking = 0,
                                AgentPositionTakingMax = 0
                            };
                            result.Settings.Add(customerSettingDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public Result UpdateAgentSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.UpdateAgentSettings(requestParams);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }
        #endregion

        #region member
        public Result AddNewMemberSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.AddNewMemberSettings(requestParams);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }

        public CustomerSettings GetMemberSetting(int custId)
        {
            var result = new CustomerSettings();
            try
            {
                result.CustId = custId;
                result.RoleId = (int)Enumerations.Role.Master;
                result.Settings = new List<CustomerSettingDetail>();
                var virtualSports = VirtualSportsSection.Instance.Sports;
                foreach (VirtualSportsElement sport in virtualSports)
                {
                    var sportId = CastAide.AsInt32(sport.Id);
                    if (sport.Active)
                    {
                        var sportName = sport.Name;
                        var sportSetting = customerAccess.GetMemberSettings(custId, sportId);
                        if (sportSetting != null && sportSetting.Tables.Count > 0 && sportSetting.Tables[0].Rows.Count > 0)
                        {
                            var settings = sportSetting.Tables[0].Rows;
                            foreach (DataRow setting in settings)
                            {
                                var customerSettingDetail = new CustomerSettingDetail()
                                {
                                    SportId = sportId,
                                    SportName = sportName,
                                    MinBet = CastAide.AsDouble(setting["minbet"]),
                                    MinBetMax = CastAide.AsDouble(setting["minbet_max"]),
                                    MaxBet = CastAide.AsDouble(setting["maxbet"]),
                                    MaxBetMin = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxPerMatch = CastAide.AsDouble(setting["maxpermatch"]),
                                    MaxPerMatchMax = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    AgentPositionTaking = CastAide.AsDouble(setting["apositiontaking"]),
                                    AgentPositionTakingMax = CastAide.AsDouble(setting["apositiontaking_max"])
                                };
                                result.Settings.Add(customerSettingDetail);
                            }
                        }
                        else
                        {
                            var customerSettingDetail = new CustomerSettingDetail()
                            {
                                SportId = sportId,
                                SportName = sportName,
                                MinBet = CastAide.AsDouble(sport.Minbet),
                                MinBetMax = CastAide.AsDouble(sport.Minbet),
                                MaxBet = CastAide.AsDouble(sport.Maxbet),
                                MaxBetMin = CastAide.AsDouble(sport.Maxbet),
                                MaxPerMatch = CastAide.AsDouble(sport.Maxpermatch),
                                MaxPerMatchMax = CastAide.AsDouble(sport.Maxpermatch),
                                AgentPositionTaking = 0,
                                AgentPositionTakingMax = 0
                            };
                            result.Settings.Add(customerSettingDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public CustomerSettings GetMemberSettingForAddNew(int uplineId)
        {
            var result = new CustomerSettings();
            try
            {
                result.CustId = uplineId;
                result.RoleId = (int)Enumerations.Role.Master;
                result.Settings = new List<CustomerSettingDetail>();
                var virtualSports = VirtualSportsSection.Instance.Sports;
                foreach (VirtualSportsElement sport in virtualSports)
                {
                    var sportId = CastAide.AsInt32(sport.Id);
                    if (sport.Active)
                    {
                        var sportName = sport.Name;
                        var sportSetting = customerAccess.GetMemberSettingsForAddNew(uplineId, sportId);
                        if (sportSetting != null && sportSetting.Tables.Count > 0 && sportSetting.Tables[0].Rows.Count > 0)
                        {
                            var settings = sportSetting.Tables[0].Rows;
                            foreach (DataRow setting in settings)
                            {
                                var customerSettingDetail = new CustomerSettingDetail()
                                {
                                    SportId = sportId,
                                    SportName = sportName,
                                    MinBet = CastAide.AsDouble(setting["minbet_max"]),
                                    MinBetMax = CastAide.AsDouble(setting["minbet_max"]),
                                    MaxBet = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxBetMin = CastAide.AsDouble(setting["maxbet_min"]),
                                    MaxPerMatch = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    MaxPerMatchMax = CastAide.AsDouble(setting["maxpermatch_max"]),
                                    AgentPositionTaking = CastAide.AsDouble(setting["apositiontaking_max"]),
                                    AgentPositionTakingMax = CastAide.AsDouble(setting["apositiontaking_max"])
                                };
                                result.Settings.Add(customerSettingDetail);
                            }
                        }
                        else
                        {
                            var customerSettingDetail = new CustomerSettingDetail()
                            {
                                SportId = sportId,
                                SportName = sportName,
                                MinBet = CastAide.AsDouble(sport.Minbet),
                                MinBetMax = CastAide.AsDouble(sport.Minbet),
                                MaxBet = CastAide.AsDouble(sport.Maxbet),
                                MaxBetMin = CastAide.AsDouble(sport.Maxbet),
                                MaxPerMatch = CastAide.AsDouble(sport.Maxpermatch),
                                MaxPerMatchMax = CastAide.AsDouble(sport.Maxpermatch),
                                AgentPositionTaking = 0,
                                AgentPositionTakingMax = 0
                            };
                            result.Settings.Add(customerSettingDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public Result UpdateMemberSettings(List<RequestParam> requestParams)
        {
            try
            {
                var result = customerAccess.UpdateMemberSettings(requestParams);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }
        #endregion
    }
}
