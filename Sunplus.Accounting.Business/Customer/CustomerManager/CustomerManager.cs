namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Extensions;
    using Core.Helper;
    using Core.Web;
    using DataAccess;
    using DataAccess.AdoNet;
    using DataAccess.IBCDLL.Customer;
    using DNAide.Core;
    using Entities;
    using Entities.Casino;
    using Entities.Customer;
    using Fanex.Module.NG2.Entities;
    using Fanex.Module.NG2.Services;
    using Fanex.Tarus.Security.Criteria;
    using Fanex.Tarus.Security.Domain;
    using Fanex.Tarus.Security.Services;
    using Security;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using Result = Entities.Result;

    public partial class CustomerManager : ICustomerManager
    {
        private CustomerService customerService;
        private ISmartPunterService smartPunterService;
        private ICustomerDAO _customerDAO;

        public CustomerManager()
            : this(null)
        {
        }

        public CustomerManager(ICustomerDAO customerDAO)
        {
            _customerDAO = customerDAO ?? new CustomerDAO();

            customerService = new CustomerService();
            smartPunterService = new SmartPunterService();
        }

        public CustDetail GetCustomerInfo(string username)
        {
            var dtCustomer = _customerDAO.GetCustomerInfo(username);
            CustDetail custDetail = new CustDetail();
            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                var rowData = dtCustomer.Rows[0];
                custDetail = new CustDetail
                {
                    CustId = CastAide.AsInt32(rowData["custid"]),
                    RoleId = CastAide.AsInt32(rowData["roleid"]),
                    Site = rowData["site"].ToString()
                };
            }
            return custDetail;
        }

        public CustDetail GetCustomerInfoMoveSite(string username)
        {
            var dtCustomer = _customerDAO.GetCustomerInfoMoveSite(username);
            CustDetail custDetail = new CustDetail();
            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                var rowData = dtCustomer.Rows[0];
                custDetail = new CustDetail
                {
                    CustId = CastAide.AsInt32(rowData["custid"]),
                    RoleId = CastAide.AsInt32(rowData["roleid"]),
                    Site = rowData["site"].ToString()
                };
            }
            return custDetail;
        }

        public CustomerDetail GetCustomerDetail(string username)
        {
            try
            {
                return IBCProvider.IBCCustomer.GetCustomerDetail(username);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public CustomerDetail GetCustomerStatus(int custId)
        {
            try
            {
                return IBCProvider.IBCCustomer.GetCustomerStatus(custId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public Hashtable GetHtmlCustInfoContent(string userName, byte checkCustId = 0)
        {
            Hashtable arrcontent = new Hashtable();
            try
            {
                var noDataMessage = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                var htmlcontent = noDataMessage;

                if (!string.IsNullOrEmpty(userName))
                {
                    var ds = customerService.GetCustomerInfo(userName, AppSession.SYSTEM, checkCustId);

                    if (ds != null && ds.ResultSet.Tables.Count.Equals(5))
                    {
                        DataRow customerInfoRow = ds.ResultSet.Tables[0].Rows[0];
                        DataRow lineInfoRow = ds.ResultSet.Tables[1].Rows[0];
                        DataRow cashRow = ds.ResultSet.Tables[2].Rows[0];
                        DataRow downlineStatusInfoRow = ds.ResultSet.Tables[3].Rows[0];
                        DataTable dynamicSettingtable = ds.ResultSet.Tables[4];

                        var customerInfoViewModel = new CustomerInfoViewModel(customerInfoRow, lineInfoRow, cashRow, downlineStatusInfoRow, dynamicSettingtable);
                        AssignCustomerSession(customerInfoViewModel);

                        bool isDenied = smartPunterService.IsDeniedViewSmartPunter(
                            new CheckSmartPunterCriteria
                            {
                                CustId = customerInfoViewModel.CustId,
                                UserName = customerInfoViewModel.UserName,
                                UserId = AppSession.AccId,
                                ActionId = SmartPunterActionId.SearchCustomer,
                                Ip = Utils.GetClientIP(),
                                IsRestricted = !PermissionService.HasPermission("SmartPunter")
                            });

                        if (isDenied)
                        {
                            htmlcontent = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "smartPunterAtuthorized"), AlertMessage.MsgType.Error);
                            arrcontent.Add("strcontent", htmlcontent);
                            return arrcontent;
                        }

                        UpdateCustomer.CustomerUpdUserSearch(
                            AppSession.AccId,
                            customerInfoViewModel.UserName,
                            customerInfoViewModel.Firstname,
                            customerInfoViewModel.Lastname);

                        htmlcontent = WebUtils.LoadTemplateBySystem("CustomerInfo/CustomerInfo.htm", true);
                        htmlcontent = RenderCustomerInfoTabs(htmlcontent, customerInfoViewModel);
                        htmlcontent = RenderExtraCustomerInfoTabs(htmlcontent, customerInfoViewModel);
                        htmlcontent = RenderSettingTabs(htmlcontent, customerInfoRow, dynamicSettingtable, customerInfoViewModel);
                        arrcontent.Add("isba", customerInfoViewModel.IsBA);
                    }
                }

                arrcontent.Add("strcontent", htmlcontent);
                return arrcontent;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return arrcontent;
            }
        }

        public static string GetCustName(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return AppSession.MemberUserName;
                case 2:
                    return AppSession.AgentUserName;
                case 3:
                    return AppSession.MasterUserName;
                case 4:
                    return AppSession.SusperUserName;
                default:
                    return string.Empty;
            }
        }

        public Result UpdateCustomerSite(int custId, string newSite)
        {
            Result result;
            if (!Permissions.GetHostPermission(Enumerations.Functions.Customer.MoveSite))
                return new Result { ErrorCode = 500, ErrorMessage = ResourceExtension.GetResourceValue(Sunplus.Accounting.Core.Extensions.ResourceExtension.Resources.ErrorMsg, "_500") };
            if (string.IsNullOrEmpty(newSite))
            {
                result = new Result { ErrorCode = 1, ErrorMessage = "Site is empty" };
                return result;
            }

            if (custId <= 0)
            {
                result = new Result { ErrorCode = 2, ErrorMessage = "Invalid Username" };
                return result;
            }

            result = _customerDAO.UpdateCustomerSite(custId, newSite, AppSession.AccId, HttpContext.Current.Request.UserHostAddress);

            return result;
        }

        public Result UpdateDangerousCustomer(int custId, int updateDownline, int dangerType, int dangerLevel)
        {
            var result = new Result();
            try
            {
                if (custId <= 0)
                {
                    result = new Result { ErrorCode = 1, ErrorMessage = "Invalid Username" };
                }
                result = _customerDAO.UpdateLevelDangerous(custId, updateDownline, dangerType, dangerLevel, AppSession.AccId,
                                                           HttpContext.Current.Request.UserHostAddress);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public CustomerExRate SelectCustomerExRate(string username)
        {
            return Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.SelectCustomerExRate(username);
        }

        public Result CustomerCrossTransferInsert(
            int fromCustID,
            int toCustID,
            double fromAmount,
            double toAmount,
            double fromExRate,
            double toExRate,
            string forexUsername,
            string remark,
            int custid)
        {
            bool IsHadPermissionOnCrossCurrencyTransfer = Permissions.GetHostPermission("FinanceTransfer");
            if (IsHadPermissionOnCrossCurrencyTransfer == false)
            {
                return new Result { ErrorCode = -1, ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500") };
            }
            return Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.CustomerCrossTransferInsert(fromCustID, toCustID, fromAmount, toAmount, fromExRate, toExRate, forexUsername, remark, custid);
        }

        public List<ForexAccount> SelectAllForexAccount() => Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.SelectAllForexAccount();

        #region Enable Number game for Downline

        public BetSettingNG CheckEnableNGDownline(string username) => _customerDAO.CheckEnableNGDownline(username);

        public Result UpdateBetSettingNGDownline(
            string username,
            int isEnable,
            string minbet,
            string maxbet,
            string maxMatch,
            string MaxBall)
            => _customerDAO.UpdateBetSettingNGDownline(
                            username,
                            isEnable,
                            minbet,
                            maxbet,
                            maxMatch,
                            MaxBall);

        #endregion Enable Number game for Downline

        public Result AutoInsertThridPartyAccount(CSCustomerDetail custInfo, CustDetail uplineInfo, int userId, string userName, string ip)
        {
            // Delay 3 sec to allow system do replication.
            System.Threading.Thread.Sleep(3000);

            var result = new Result();
            if (uplineInfo.IsUplineSyncCasino)
            {
                System.Threading.Thread.Sleep(3000);
                var csService = new PT.Casino.CasinoManager();

                csService.AutoInsertCasinoAccount(custInfo, userId, userName, ip);
            }

            if (uplineInfo.IsUplineSyncNG2)
            {
                ING2StatusService syncService = new NG2StatusService();
                syncService.AutoInsertAccount(custInfo.IBCCustID, custInfo.UserName, (RoleId)custInfo.RoleId, custInfo.UpperCustID, custInfo.IsDisableNG2Upline, SiteId.NAP, custInfo.Currency, AppSession.AccId, AccCommon.SystemId);
            }

            return result;
        }

        public string GetRNGSiteNameByCustId(int custId) => _customerDAO.GetRNGSiteNameByCustId(custId);

        #region private functions
        private static string RenderCustomerInfoTabs(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            htmlcontent = RenderCustomerDetail(htmlcontent, customerInfoViewModel);
            htmlcontent = GenUplineBox(htmlcontent, customerInfoViewModel);

            if (!customerInfoViewModel.ViewFiAcc)
            {
                htmlcontent = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
            }

            htmlcontent = GenTransLinskBox(htmlcontent, customerInfoViewModel);
            return htmlcontent;
        }

        private static string RenderExtraCustomerInfoTabs(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            htmlcontent = RenderBalanceInfo(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderDirectInfoTab(htmlcontent, customerInfoViewModel);
            return htmlcontent;
        }

        private string RenderSettingTabs(string htmlcontent, DataRow customerInfoRow, DataTable dynamicSettingtable, CustomerInfoViewModel customerInfoViewModel)
        {
            RenderLeftPanelParameter renderLeftPanelParameter = RenderSettingMenu(
                ref htmlcontent,
                customerInfoRow,
                dynamicSettingtable,
                customerInfoViewModel);

            htmlcontent = RenderDetailMenu(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);
            htmlcontent = GenLoginDetail(htmlcontent, customerInfoViewModel);

            return htmlcontent;
        }

        private void AssignCustomerSession(CustomerInfoViewModel customerInfoViewModel)
        {
            AppSession.CustRoleId = customerInfoViewModel.RoleId;
            AppSession.CustID = customerInfoViewModel.CustId;
            AppSession.CustUName = customerInfoViewModel.UserName;

            AppSession.MemberID = customerInfoViewModel.MemberId;
            AppSession.MemberUserName = customerInfoViewModel.MemberName;

            AppSession.AgentID = customerInfoViewModel.AgentId;
            AppSession.AgentUserName = customerInfoViewModel.AgentName;

            AppSession.MasterID = customerInfoViewModel.MasterId;
            AppSession.MasterUserName = customerInfoViewModel.MasterName;

            AppSession.SuperID = customerInfoViewModel.SuperId;
            AppSession.SusperUserName = customerInfoViewModel.SuperName;

            AppSession.IsSyncCSCurrentCust = customerInfoViewModel.SyncRnGCasino;
            AppSession.IsInternal = customerInfoViewModel.IsInternalUser;
        }

        private static string GetSettingAbnormal(int roleId, bool isManageCustomerAccess, int currentLevel)
        {
            try
            {
                AbnormalSetting abnormal = new AbnormalSetting();
                return abnormal.GenAbnormal(roleId, isManageCustomerAccess, currentLevel);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        private static string AssignJS(Hashtable param)
        {
            string jsscript = "<script>";

            foreach (string key in param.Keys)
            {
                jsscript += string.Format(Globals.JS_VariableFormat, key, param[key]);
            }
            jsscript += "</script>";
            return jsscript;
        }

        #endregion private functions
    }
}