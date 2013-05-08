using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CasinoLibrary.SBAgent;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Fanex.Data;
using Sunplus.Accounting.Core.Extensions;

/// <summary>
/// Summary description for APICasinoProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.APIClient
{
    public class APICasinoProvider : CasinoProvider
    {
        //Call from Casino API to get the game kind list and assign to List<GameKindDetail>
        public override List<GameKindDetail> GetGameKinds(string siteName)
        {
            var lsGameKind = new List<GameKindDetail>();
            try
            {
                var _csBasicInfo = new BasicInfo();
                _csBasicInfo.SiteName = siteName;
                DataTable dtGameKind = _csBasicInfo.GetGameKind(null);                
                foreach (DataRow row in dtGameKind.Rows)
                {
                    var gameKind = new GameKindDetail(CastAide.AsInt32(row["CodeID"]), CastAide.AsString(row["CodeNo"]),
                                                      CastAide.AsString(row[Lang("CodeName")]),
                                                      CastAide.AsString(row["CodeSName"]));
                    lsGameKind.Add(gameKind);
                }
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }
            return lsGameKind;
        }

        /// <summary>
        /// Initialize basics property of an object from posted values of a form. Be used in new customer function.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r)
        {
            var _custSetting = new CSCustomerDetail();


            _custSetting.UserName = CastAide.AsString(r["username"]);
            _custSetting.UserPwd = CastAide.AsString(r["userpwd"]);
            _custSetting.FirstName = CastAide.AsString(r["firstname"]);
            _custSetting.LastName = CastAide.AsString(r["lastname"]);
            _custSetting.Phone = CastAide.AsString(r["phone"]);
            _custSetting.MobilePhone = CastAide.AsString(r["mobilephone"]);
            _custSetting.Fax = CastAide.AsString(r["fax"]);
            _custSetting.Email = CastAide.AsString(r["email"]);
            _custSetting.RoleId = AppSession.RoleID - 1;

            _custSetting.Closed = CastAide.AsBoolean(r["closed"]);
            _custSetting.Suspend = CastAide.AsBoolean(r["suspend"] == null ? r["suspended"] : r["suspend"]);


            _custSetting.SIBCCustID = AppSession.RoleID == 4 ? AppSession.CustID : 0;
            _custSetting.MIBCCustID = AppSession.RoleID == 3 ? AppSession.CustID : 0;
            _custSetting.AIBCCustID = AppSession.RoleID == 2 ? AppSession.CustID : 0;

            _custSetting.CasinoSync = false;
            _custSetting.CasinoDisabled = false;
            _custSetting.IBCCustID = CastAide.AsInt32(custid);

            #region rng site name
            _custSetting.SiteName = new Sunplus.Accounting.DataAccess.AdoNet.CustomerDAO().GetRNGSiteNameByCustId(_custSetting.IBCCustID);            
            #endregion
            return _custSetting;
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, string siteName)
        {
            try
            {                
                using (ObjectDb db = new ObjectDb("Acc_UpdCSStatus", 300))
                {
                    var param = new Hashtable();
                    param.Add("custid", custid);
                    param.Add("disabledcs", bDisabledCS);
                    param.Add("synccs", bSyncCS);
                    param.Add("enabledownline", false);

                    param.Add("userid", AppSession.AccId);
                    param.Add("subaccid", 0);
                    param.Add("loginip", HttpContext.Current.Request.UserHostAddress);

                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    bool status = CastAide.AsBoolean(db.GetParameterValue("status"));
                    if (!status) throw new Exception(db.GetParameterValue("errmsg").ToString());
                }
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                LogExtensions.ExceptionHandler(ex);
                throw ex;
            }

            var cust = new Customer();
            cust.SiteName = siteName;
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, true,
                                               HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                                               AppSession.AccId, AppSession.Username, 3);
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline, string siteName)
        {
            try
            {               
                using (ObjectDb db = new ObjectDb("Acc_UpdCSStatus", 300))
                {
                    var param = new Hashtable();
                    param.Add("custid", custid);
                    param.Add("disabledcs", bDisabledCS);
                    param.Add("synccs", bSyncCS);
                    param.Add("enabledownline", isUpdDownline);

                    param.Add("userid", AppSession.AccId);
                    param.Add("subaccid", 0);
                    param.Add("loginip", HttpContext.Current.Request.UserHostAddress);

                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    bool status = CastAide.AsBoolean(db.GetParameterValue("status"));
                    if (!status) throw new Exception(db.GetParameterValue("errmsg").ToString());                    
                }
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                LogExtensions.ExceptionHandler(ex);
                throw ex;
            }

            var cust = new Customer();
            cust.SiteName = siteName;
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, isUpdDownline,
                                               HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                                               AppSession.AccId, AppSession.Username, 3);
        }

        public override bool UpdateCSStatusAfterSync(int custid, bool bSyncCS, bool bDisabledCS, int userId, string ip)
        {
            
            var status = false;
            try
            {                
                using (ObjectDb db = new ObjectDb("Acc_UpdCSStatus", 300))
                {
                    var param = new Hashtable();
                    param.Add("custid", custid);
                    param.Add("disabledcs", bDisabledCS);
                    param.Add("synccs", bSyncCS);
                    param.Add("enabledownline", false);

                    param.Add("userid", userId);
                    param.Add("subaccid", 0);
                    param.Add("loginip", ip);

                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    status = CastAide.AsBoolean(db.GetParameterValue("status"));                    
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return status;
        }

        public override List<GameKindDetail> GetGameKinds(string casinoNo, string siteName)
        {
            var lsGameKind = new List<GameKindDetail>();
            try
            {
                var _csBasicInfo = new BasicInfo();
                _csBasicInfo.SiteName = siteName;
                DataTable dtGameKind = _csBasicInfo.GetGameKind(casinoNo);
                if (dtGameKind != null)
                {
                    foreach (DataRow row in dtGameKind.Rows)
                    {
                        var gameKind = new GameKindDetail(CastAide.AsInt32(row["CodeID"]),
                                                          CastAide.AsString(row["CodeNo"]),
                                                          CastAide.AsString(row[Lang("CodeName")]),
                                                          CastAide.AsString(row["CodeSName"]));
                        lsGameKind.Add(gameKind);
                    }
                }
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }
            return lsGameKind;
        }

        public override List<GameKindDetail> GetGameKinds(int custId, string siteName)
        {
            var lsGameKind = new List<GameKindDetail>();
            try
            {   
                var _cust = new Customer();
                _cust.SiteName = siteName;
                DataTable dtGameKind = _cust.GetCustGameKind(custId);
                foreach (DataRow row in dtGameKind.Rows)
                {
                    var gameKind = new GameKindDetail();
                    gameKind.CodeID = CastAide.AsInt32(row["GameKind"]);
                    gameKind.CodeName = CastAide.AsString(row[Lang("GameKindName")]);
                    gameKind.AgentRollingRate = CastAide.AsDouble(row["AgentRollingRate"]) > 0 ? CastAide.AsDouble(row["AgentRollingRate"]) : 0;
                    gameKind.APositionTaking = CastAide.AsDouble(row["APositionTaking"]) > 0 ? CastAide.AsDouble(row["APositionTaking"]) : 0;
                    gameKind.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]) > 0 ? CastAide.AsDouble(row["AutoPositionTaking"]) : 0;
                    gameKind.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                    gameKind.MAgentRollingRate = CastAide.AsDouble(row["MAgentRollingRate"]);
                    gameKind.MaxPositionTaking = CastAide.AsDouble(row["MaxPositionTaking"]);
                    gameKind.MemberRollingRate = CastAide.AsDouble(row["MemberRollingRate"]);
                    // gameKind.MinMPositionTaking = CastAide.AsDouble(row["MinMPositionTaking"]);
                    gameKind.MinPositionTaking = CastAide.AsDouble(row["MinPositionTaking"]);
                    gameKind.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                    gameKind.SMAgentRollingRate = CastAide.AsDouble(row["SMAgentRollingRate"]);
                    gameKind.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);

                    gameKind.AgentRollingRateUpline = CastAide.AsDouble(row["UplineAgentRollingRate"]);
                    gameKind.APositionTakingUpline = CastAide.AsDouble(row["UplineAPositionTaking"]);
                    gameKind.AutoPositionTakingUpline = CastAide.AsDouble(row["UplineAutoPositionTaking"]);
                    gameKind.IsAutoPositionTakingUpline = CastAide.AsBoolean(row["UplineIsAutoPositionTaking"]);
                    gameKind.MAgentRollingRateUpline = CastAide.AsDouble(row["UplineMAgentRollingRate"]);
                    gameKind.MaxPositionTakingUpline = CastAide.AsDouble(row["UplineMaxPositionTaking"]);
                    gameKind.MemberRollingRateUpline = CastAide.AsDouble(row["UplineMemberRollingRate"]);
                    // gameKind.MinMPositionTakingUpline = CastAide.AsDouble(row["MinMPositionTaking"]);
                    gameKind.MinPositionTakingUpline = CastAide.AsDouble(row["UplineMinPositionTaking"]);
                    gameKind.MPositionTakingUpline = CastAide.AsDouble(row["UplineMPositionTaking"]);
                    gameKind.SMAgentRollingRateUpline = CastAide.AsDouble(row["UplineSMAgentRollingRate"]);
                    gameKind.SPositionTakingUpline = CastAide.AsDouble(row["UplineSPositionTaking"]);

                    lsGameKind.Add(gameKind);
                }
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }
            return lsGameKind;
        }

        /// <summary>
        /// Get customer PT when super edit member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public override List<GameKindDetail> GetGameKinds_EditMember(int memberId)
        {
            var lsGameKind = new List<GameKindDetail>();

            try
            {
                var _cust = new Customer();
                DataTable dtGameKind = _cust.GetCustGameKind_EditMember(memberId);
                foreach (DataRow row in dtGameKind.Rows)
                {
                    var gameKind = new GameKindDetail();
                    gameKind.CodeID = CastAide.AsInt32(row["GameKind"]);
                    gameKind.CodeName = CastAide.AsString(row[Lang("GameKindName")]);
                    gameKind.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                    gameKind.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                    gameKind.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                    gameKind.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                    gameKind.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                    lsGameKind.Add(gameKind);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lsGameKind;
        }


        //Get customer PT by game type when super edit member
        public override List<GameTypeDetail> GetGameTypes_EditMember(int memberId, int gameKindID, string siteName)
        {
            var lsGameType = new List<GameTypeDetail>();
            try
            {
                var _cust = new Customer();
                _cust.SiteName = siteName;
                DataTable dtGameType = _cust.GetCustGameType_EditMember(memberId, gameKindID);
                foreach (DataRow row in dtGameType.Rows)
                {
                    var gameType = new GameTypeDetail();

                    gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                    gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                    gameType.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                    gameType.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                    gameType.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                    gameType.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                    gameType.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                    lsGameType.Add(gameType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lsGameType;
        }

        public override List<GameKindDetail> GetGameKinds_MasterEditMember(int memberid, string siteName)
        {
            var lsGameKind = new List<GameKindDetail>();
            try
            {
                var _cust = new Customer();
                _cust.SiteName = siteName;
                DataTable dtGameKind = _cust.GetCustGameKind(memberid);
                foreach (DataRow row in dtGameKind.Rows)
                {
                    var gameKind = new GameKindDetail();
                    gameKind.CodeID = CastAide.AsInt32(row["GameKind"]);
                    gameKind.CodeName = CastAide.AsString(row[Lang("GameKindName")]);
                    gameKind.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                    gameKind.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                    gameKind.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                    gameKind.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                    gameKind.MasterMaxPositionTaking = CastAide.AsDouble(row["MasterMaxPositionTalking"]);
                    gameKind.MasterMinPositionTaking = CastAide.AsDouble(row["MasterMinPositionTaking"]);
                    gameKind.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                    lsGameKind.Add(gameKind);
                }

                return lsGameKind;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lsGameKind;
        }

        public override List<GameTypeDetail> GetGameTypes_MasterEditMember(int memberid, int gameKindID, string siteName)
        {
            var lsGameType = new List<GameTypeDetail>();
            try
            {
                var _cust = new Customer();
                _cust.SiteName = siteName;
                DataTable dtGameType = _cust.GetCustGameType(memberid, gameKindID);
                foreach (DataRow row in dtGameType.Rows)
                {
                    var gameType = new GameTypeDetail();

                    gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                    gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                    gameType.AgentRollingRate = CastAide.AsDouble(row["AgentRollingRate"]);
                    gameType.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                    gameType.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                    gameType.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                    gameType.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                    gameType.MasterMaxPositionTaking = CastAide.AsDouble(row["MasterMaxPositionTalking"]);
                    gameType.MasterMinPositionTaking = CastAide.AsDouble(row["MasterMinPositionTaking"]);
                    gameType.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);   
                    lsGameType.Add(gameType);
                }
                return lsGameType;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lsGameType;
        }

        /// <summary>
        /// Get game type list of a certain game kind
        /// </summary>
        /// <param name="gameKindID">ID of game kind</param>
        /// <returns>List GameTypeDetail</returns>
        public override List<GameTypeDetail> GetGameType(int gameKindID, string siteName)
        {
            var lsGameType = new List<GameTypeDetail>();
            try
            {
                var _csBasicInfo = new BasicInfo();
                _csBasicInfo.SiteName = siteName;
                DataTable dtGameType = _csBasicInfo.GetGameType(gameKindID);
                foreach (DataRow row in dtGameType.Rows)
                {
                    var gameType = new GameTypeDetail();
                    //gameType.CasinoID = CastAide.AsInt32(row["CasinoID"]);
                    gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                    gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                    gameType.GameTypeNo = CastAide.AsString(row["GameTypeNo"]);
                    //gameType.GameKind = CastAide.AsInt32(row["GameKind"]);
                    //gameType.GameGroupID = CastAide.AsInt32(row["GameGroupID"]);
                    //gameType.FlashFileName = CastAide.AsString(row["FlashFileName"]);
                    //gameType.AlertMaxIdle = CastAide.AsInt32(row["AlertMaxIdle"]);
                    //gameType.IsTrialTime = CastAide.AsBoolean(row["IsTrialTime"]);
                    //gameType.MaxIdle = CastAide.AsInt32(row["MaxIdle"]);
                    lsGameType.Add(gameType);
                }
                return lsGameType;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lsGameType;
        }

        public override List<GameTypeDetail> GetGameType(int custId, int gameKindID, string siteName)
        {
            var lsGameType = new List<GameTypeDetail>();
            try
            {
                var _cust = new Customer();
                _cust.SiteName = siteName;
                DataTable dtGameType = _cust.GetCustGameType(custId, gameKindID);
                foreach (DataRow row in dtGameType.Rows)
                {
                    var gameType = new GameTypeDetail();

                    gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                    gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                    gameType.AgentRollingRate = CastAide.AsDouble(row["AgentRollingRate"]);
                    gameType.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                    gameType.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                    gameType.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                    gameType.MAgentRollingRate = CastAide.AsDouble(row["MAgentRollingRate"]);
                    gameType.MaxPositionTaking = CastAide.AsDouble(row["MaxPositionTaking"]);
                    gameType.MemberRollingRate = CastAide.AsDouble(row["MemberRollingRate"]);
                    //gameType.MinMPositionTaking = CastAide.AsDouble(row["MinMPositionTaking"]);
                    gameType.MinPositionTaking = CastAide.AsDouble(row["MinPositionTaking"]);
                    gameType.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                    gameType.SMAgentRollingRate = CastAide.AsDouble(row["SMAgentRollingRate"]);
                    gameType.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);

                    gameType.AgentRollingRateUpline = CastAide.AsDouble(row["UplineAgentRollingRate"]);
                    gameType.APositionTakingUpline = CastAide.AsDouble(row["UplineAPositionTaking"]);
                    gameType.AutoPositionTakingUpline = CastAide.AsDouble(row["UplineAutoPositionTaking"]);
                    gameType.IsAutoPositionTakingUpline = CastAide.AsBoolean(row["UplineIsAutoPositionTaking"]);
                    gameType.MAgentRollingRateUpline = CastAide.AsDouble(row["UplineMAgentRollingRate"]);
                    gameType.MaxPositionTakingUpline = CastAide.AsDouble(row["UplineMaxPositionTaking"]);
                    gameType.MemberRollingRateUpline = CastAide.AsDouble(row["UplineMemberRollingRate"]);
                    //gameType.MinMPositionTaking = CastAide.AsDouble(row["MinMPositionTaking"]);
                    gameType.MinPositionTakingUpline = CastAide.AsDouble(row["UplineMinPositionTaking"]);
                    gameType.MPositionTakingUpline = CastAide.AsDouble(row["UplineMPositionTaking"]);
                    gameType.SMAgentRollingRateUpline = CastAide.AsDouble(row["UplineSMAgentRollingRate"]);
                    gameType.SPositionTakingUpline = CastAide.AsDouble(row["UplineSPositionTaking"]);

                    lsGameType.Add(gameType);
                }
                return lsGameType;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lsGameType;
        }

        public override List<StakeLevelDetail> GetCustStakeLevels(int custId, int gameTypeID, string siteName)
        {
            var lsStakeLevel = new List<StakeLevelDetail>();
            try
            {
                var _cust = new Customer();
                _cust.SiteName = siteName;
                DataTable dtStakeList = _cust.GetCustStakeLevel(custId, gameTypeID);
                foreach (DataRow row in dtStakeList.Rows)
                {
                    var stake = new StakeLevelDetail();
                    stake.StakeLevelID = CastAide.AsInt32(row["StakeLevelID"]);
                    stake.StakeLevelName = CastAide.AsString(row["StakeLevelName"]);
                    stake.IsChecked = CastAide.AsBoolean(row["IsChecked"]);
                    lsStakeLevel.Add(stake);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lsStakeLevel;
        }

        public override List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, int currencyId, string siteName)
        {
            var lsStakeLevel = new List<StakeLevelDetail>();
            try
            {
                //int custId = 1267168;

                var bi = new BasicInfo();
                bi.SiteName = siteName;
                DataTable dtStakeList = bi.GetGameTypeStakeLevel(gameTypeID, currencyId);
                foreach (DataRow row in dtStakeList.Rows)
                {
                    var stake = new StakeLevelDetail();
                    stake.StakeLevelID = CastAide.AsInt32(row["StakeLevelID"]);
                    stake.StakeLevelName = CastAide.AsString(row["StakeLevelName"]);
                    stake.IsChecked = true;
                    lsStakeLevel.Add(stake);
                }
                return lsStakeLevel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lsStakeLevel;
        }

        public override List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, string currencyName)
        {
            throw new NotImplementedException();
        }

        public override CSCustomerDetail GetCustSettingCS(int custId, string siteName)
        {
            var _custSetting = new CSCustomerDetail();
            try
            {
                var _cust = new Customer();
                _cust.SiteName = siteName;
                DataTable dtCustomer = _cust.GetCustSetting(custId);
                DataRow row = null;

                if (dtCustomer.Rows.Count > 0)
                {
                    row = dtCustomer.Rows[0];
                    _custSetting.IsEnablePT = CastAide.AsBoolean(row["IsEnablePT"]);
                    _custSetting.TotalWinLoss = CastAide.AsDouble(row["ActualMaxTransfer"]);
                    _custSetting.MaxWin = CastAide.AsDouble(row["MaxWin"]);
                    _custSetting.MaxLose = CastAide.AsDouble(row["MaxLose"]);
                    _custSetting.MaxTransferCredit = CastAide.AsDouble(row["MaxTransferCredit"]);
                    _custSetting.IsMaxWin = CastAide.AsBoolean(row["IsMaxWin"]);
                    _custSetting.IsMaxLose = CastAide.AsBoolean(row["IsMaxLose"]);
                    _custSetting.PTByGameType = CastAide.AsBoolean(row["PTByGameType"]);
                    _custSetting.RoleId = CastAide.AsInt32(row["RoleId"]);
                    _custSetting.UpperCustID = CastAide.AsInt32(row["UpperCustId"]);

                    _custSetting.IsAutoReset = CastAide.AsBoolean(row["IsAutoReset"]);
                    _custSetting.NoCSData = false;
                }
                return _custSetting;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _custSetting;
        }

        //get customer setting from Casino and add to IBC customer info
        public override void GetCustSettingCS(ref CSCustomerDetail _custIBC)
        {
            try
            {
                var _cust = new Customer();
                _cust.SiteName = _custIBC.SiteName;
                DataTable dtCustomer = _cust.GetCustSetting(_custIBC.IBCCustID);
                DataRow row = null;
                if (dtCustomer.Rows.Count > 0)
                {
                    row = dtCustomer.Rows[0];
                    _custIBC.IsEnablePT = CastAide.AsBoolean(row["IsEnablePT"]);
                    _custIBC.TotalWinLoss = CastAide.AsDouble(row["ActualMaxTransfer"]);
                    _custIBC.MaxWin = CastAide.AsDouble(row["MaxWin"]);
                    _custIBC.MaxLose = CastAide.AsDouble(row["MaxLose"]);
                    _custIBC.MaxTransferCredit = CastAide.AsDouble(row["MaxTransferCredit"]);
                    _custIBC.IsMaxWin = CastAide.AsBoolean(row["IsMaxWin"]);
                    _custIBC.IsMaxLose = CastAide.AsBoolean(row["IsMaxLose"]);
                    _custIBC.PTByGameType = CastAide.AsBoolean(row["PTByGameType"]);
                    _custIBC.RoleId = CastAide.AsInt32(row["RoleId"]);
                    _custIBC.UpperCustID = CastAide.AsInt32(row["UpperCustId"]);
                    _custIBC.IsAutoReset = CastAide.AsBoolean(row["IsAutoReset"]);

                    _custIBC.NoCSData = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override CSCustomerDetail GetCustInfoIBC(int _IBCCustId)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateCustClosed(int IBCCustID, bool Closed, string FromIP, string siteName)
        {
            bool isUpdateSuccessfull = false;
            try
            {
                var csCustomer = new Customer();
                csCustomer.SiteName = siteName;
                isUpdateSuccessfull = csCustomer.UpdateCustClosed(IBCCustID, Closed, FromIP, AppSession.AccId,
                                                                  AppSession.Username, 3);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return isUpdateSuccessfull;
        }

        public override bool UpdateCustEnableCasino(int IBCCustID, bool EnabledCasino, bool UpdateDownline,
                                                    string FromIP, string siteName)
        {
            bool isUpdateSuccessfull = false;
            try
            {
                var csCustomer = new Customer();
                csCustomer.SiteName = siteName;
                isUpdateSuccessfull = csCustomer.UpdateCustEnableCasino(IBCCustID, EnabledCasino, UpdateDownline, FromIP,
                                                                        AppSession.AccId, AppSession.Username, 3);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return isUpdateSuccessfull;
        }

        public override bool UpdateCustPwd(int IBCCustID, string UserPwdEncrypted, string FromIP, string siteName)
        {
            bool isUpdatedSuccessfull = false;
            try
            {
                var csCustomer = new Customer();
                csCustomer.SiteName = siteName;
                isUpdatedSuccessfull = csCustomer.UpdateCustPwd(IBCCustID, UserPwdEncrypted, FromIP, AppSession.AccId,
                                                                AppSession.Username, 3);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return isUpdatedSuccessfull;
        }

        public override bool UpdateCustSuspend(int IBCCustID, bool Suspended, string FromIP, string siteName)
        {
            bool isUpdateSuccessfull = false;
            try
            {
                var csCustomer = new Customer();
                csCustomer.SiteName = siteName;
                isUpdateSuccessfull = csCustomer.UpdateCustSuspend(IBCCustID, Suspended, FromIP, AppSession.AccId,
                                                                   AppSession.Username, 3);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return isUpdateSuccessfull;
        }

        /// <summary>
        /// Return a string which matching with current language
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string Lang(string original)
        {
            return original;
        }

        public override bool FullResetMaxTransfer(string listIBCCustID, string FromIP, int ModifyBy,
                                                  string ModifyByUsername, int FromSystem, string siteName)
        {
            bool isResetSuccessfull = false;
            try
            {
                var cust = new Customer();
                cust.SiteName = siteName;
                isResetSuccessfull = cust.FullSettlementResetMaxTransfer(listIBCCustID, FromIP, ModifyBy,
                                                                         ModifyByUsername, FromSystem);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return isResetSuccessfull;
        }

        public override List<ErrorDetail> MoveMaster(string MasterUsername, string SuperUsername, string FromIP,
                                                     int ModifyByUserId, string ModifyByUser, string siteName)
        {
            var errors = new List<ErrorDetail>();
            try
            {
                var cust = new Customer();
                cust.SiteName = siteName;
                DataTable tableResult = cust.MoveMaster(MasterUsername, SuperUsername, FromIP, ModifyByUserId,
                                                        ModifyByUser);
                foreach (DataRow row in tableResult.Rows)
                {
                    errors.Add(new ErrorDetail(row["ErrCode"].ToString(), row["ErrMsg"].ToString()));
                }
                return errors;
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return errors;
        }

        public override List<ErrorDetail> MoveAgent(string agentUsername, string masterUsername, string fromIP,
                                                    int modifyByUserId, string modifyByUser, string siteName)
        {
            var errors = new List<ErrorDetail>();
            try
            {
                var cust = new Customer();
                cust.SiteName = siteName;
                DataTable tableResult = cust.MoveAgent(agentUsername, masterUsername, fromIP, modifyByUserId,
                                                       modifyByUser);
                foreach (DataRow row in tableResult.Rows)
                {
                    errors.Add(new ErrorDetail(row["ErrCode"].ToString(), row["ErrMsg"].ToString()));
                }
                return errors;
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return errors;
        }

        public override bool RollbackMaster(string MasterUsername, bool IsNeedRollback, string siteName)
        {
            bool hasSuccessRollbackMaster = false;
            try
            {
                var cust = new Customer();
                cust.SiteName = siteName;
                hasSuccessRollbackMaster = cust.RollbackMaster(MasterUsername, IsNeedRollback);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return hasSuccessRollbackMaster;
        }

        public override bool RollbackAgent(string AgentUsername, bool IsNeedRollback, string siteName)
        {
            bool hasSuccessRollbackAgent = false;
            try
            {
                var cust = new Customer();
                cust.SiteName = siteName;
                hasSuccessRollbackAgent = cust.RollbackAgent(AgentUsername, IsNeedRollback);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                throw ex;
            }

            return hasSuccessRollbackAgent;
        }

        public override bool UpdateCSStatusWithoutSync(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        { 
            bool status = false;
            try
            {                
                using (ObjectDb db = new ObjectDb("Acc_UpdCSStatus", 300))
                {
                    var param = new Hashtable();
                    param.Add("custid", custid);
                    param.Add("disabledcs", bDisabledCS);
                    param.Add("synccs", bSyncCS);
                    param.Add("enabledownline", isUpdDownline);

                    param.Add("userid", AppSession.AccId);
                    param.Add("subaccid", 0);
                    param.Add("loginip", HttpContext.Current.Request.UserHostAddress);

                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    status = CastAide.AsBoolean(db.GetParameterValue("status"));
                    if (!status) throw new Exception(CastAide.AsString(db.GetParameterValue("errmsg")));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw ex;
            }

            return status;
        }


        private class StoreProcedureConfiguration:DataAccess
        {
            public StoreProcedureElement GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CasinoSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}