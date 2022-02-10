namespace Sunplus.Accounting.Business.Customer
{
    using ConfigureDirectAccount;
    using Core.Common;
    using Core.Extensions;
    using Core.Helper;
    using Core.Web;
    using DataAccess;
    using DataAccess.AdoNet;
    using DataAccess.IBCDLL.Customer;
    using DNAide.Core;
    using DNAide.Web;
    using Entities;
    using Entities.Customer;
    using Fanex.Module.NG2.Entities;
    using Fanex.Module.NG2.Services;
    using Fanex.RemotingRef;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Security.Criteria;
    using Fanex.Tarus.Security.Domain;
    using Fanex.Tarus.Security.Services;
    using Security;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;
    using ConverterCommon = Fanex.Common.Converter;
    using Result = Entities.Result;
    using WebSettings = Sunplus.Accounting.Core.Configurations.WebSettings;

    public partial class CustomerManager : ICustomerManager
    {
        private CustomerService customerService;
        private ISmartPunterService smartPunterService;
        private ICustomerDAO _customerDAO;
        private IHelper helper;
        private readonly IRemotingRefService _remotingRefService;
        private readonly RenderLeftPanelManager _renderLeftPanelService;
        private readonly ConfigureDirectAccountService _configureDirectAccountService;

        public CustomerManager()
            : this(null, null)
        {
        }

        public CustomerManager(ICustomerDAO customerDAO, IHelper helper)
        {
            _customerDAO = customerDAO ?? new CustomerDAO();
            this.helper = helper ?? new Helper();

            customerService = new CustomerService();
            smartPunterService = new SmartPunterService();
            _remotingRefService = new RemotingRefService();
            _renderLeftPanelService = new RenderLeftPanelManager();
            _configureDirectAccountService = new ConfigureDirectAccountService();
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
                return IBCProvider.IBCCustomer.GetCustomerStatus(custId, WebSettings.SettingStatus);
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
                    int tableCount = ds != null ? ds.ResultSet.Tables.Count : 0;
                    if (ds.Outputs.Count > 0 && CastAide.AsInt32(ds.Outputs["errcode"]) > 0)
                    {
                        arrcontent.Add("strcontent", AlertMessage.GetMsg(ds.Outputs["errmsg"].ToString(), AlertMessage.MsgType.Error));
                        return arrcontent;
                    }

                    if (ds != null && tableCount >= 5)
                    {
                        DataRow customerInfoRow = ds.ResultSet.Tables[0].Rows[0];
                        DataRow lineInfoRow = ds.ResultSet.Tables[1].Rows[0];
                        DataRow cashRow = ds.ResultSet.Tables[2].Rows[0];
                        DataRow downlineStatusInfoRow = ds.ResultSet.Tables[3].Rows[0];
                        DataTable dynamicSettingtable = ds.ResultSet.Tables[4];
                        DataRow memberCN88CreditRow = tableCount == 7 ? ds.ResultSet.Tables[5].Rows[0] : null;
                        DataRow memberCN88BetStakeRow = tableCount == 7 ? ds.ResultSet.Tables[6].Rows[0] : null;

                        var roleId = ConverterCommon.ToInt(customerInfoRow["roleid"]);
                        decimal memberOutstandingMBC = 0;
                        decimal memberBalanceMBC = 0;
                        decimal memberNetPositionMBC = 0;
                        decimal member3rdPendingTransfer = 0;
                        var IsInternalUser = customerInfoRow.Table.Columns.Contains("IsInternal") && 
                            Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(customerInfoRow["IsInternal"]);

                        if (roleId == (int)Role.Member && 
                            !AppSettings.IsCN88SiteName(
                                ConverterCommon.ToString2(customerInfoRow["membersite"])) &&
                                !IsInternalUser)
                        {
                            var searchAccountId = ConverterCommon.ToInt(customerInfoRow["custid"]);
                            memberOutstandingMBC = _customerDAO.GetMemberOustandingMBC(searchAccountId);

                            var memberMBCBalanceModel = _customerDAO.GetMemberMBCBalanceModel(searchAccountId);
                            memberBalanceMBC = memberMBCBalanceModel.Balance;
                            memberNetPositionMBC = memberMBCBalanceModel.NetPosition;
                            member3rdPendingTransfer = memberMBCBalanceModel.PendingTransfer3rd;
                        }

                        var customerInfoViewModel = new CustomerInfoViewModel(
                            customerInfoRow, 
                            lineInfoRow, 
                            cashRow, 
                            downlineStatusInfoRow, 
                            dynamicSettingtable,
                            memberCN88CreditRow,
                            memberCN88BetStakeRow,
                            memberOutstandingMBC,
                            memberBalanceMBC,
                            memberNetPositionMBC,
                            member3rdPendingTransfer);

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
                        RenderCN88Customer(ref htmlcontent, customerInfoViewModel);
                        RenderCommonInfo(ref htmlcontent, customerInfoViewModel);
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

        public string GetCustName(int roleId)
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

        #region private functions

        private string RenderCustomerInfoTabs(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            htmlcontent = RenderCustomerDetail(htmlcontent, customerInfoViewModel);
            htmlcontent = GenUplineBox(htmlcontent, customerInfoViewModel);

            if (!customerInfoViewModel.IsCN88 && !customerInfoViewModel.ViewFiAcc)
            {
                htmlcontent = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
            }

            htmlcontent = GenTransLinskBox(htmlcontent, customerInfoViewModel);
            TemplateHandler.Assign(ref htmlcontent, "{jsparams}", AssignJS(customerInfoViewModel.JsParam));
            return htmlcontent;
        }

        private string RenderExtraCustomerInfoTabs(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            htmlcontent = RenderBalanceInfo(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderDirectInfoTab(htmlcontent, customerInfoViewModel);
            return htmlcontent;
        }

        private string RenderSettingTabs(string htmlcontent, DataRow customerInfoRow, DataTable dynamicSettingtable, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.Closed)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "CN88CustomerUnlock");
            }

            if (customerInfoViewModel.SearchAccountRoleId == (int)Role.Headline)
            {
                TemplateHandler.Assign(ref htmlcontent, "{linkSettingInfo}", "Customer/EditCustomerInfo.aspx?custId=" + customerInfoViewModel.CustId + "&type=Headline");
                htmlcontent = GenLoginDetail(htmlcontent, customerInfoViewModel);
                return htmlcontent;
            }

            RenderLeftPanelParameter renderLeftPanelParameter = RenderSettingMenu(
            ref htmlcontent,
            customerInfoRow,
            dynamicSettingtable,
            customerInfoViewModel);

            htmlcontent = RenderDetailMenu(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);
            htmlcontent = GenLoginDetail(htmlcontent, customerInfoViewModel);

            return htmlcontent;
        }

        private void RenderCN88Customer(ref string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var isCN88 = AppSettings.IsCN88SiteName(customerInfoViewModel.CustomerSite);

            ShowHideBlock(ref htmlcontent, customerInfoViewModel.SearchAccountRoleId == (int)Role.Headline, "CN88HeadlineVisibleItem", "CN88HeadlineHiddenItem");
            ShowHideBlock(ref htmlcontent, isCN88, "CN88VisibleItem", "CN88HiddenItem");
            ShowHideBlock(ref htmlcontent, customerInfoViewModel.ManageCustomerAccess, "CN88CustomerStatus", "CN88CustomerStatusDisabled");
            ShowHideBlock(ref htmlcontent, customerInfoViewModel.SearchAccountRoleId == (int)Role.Super, "CN88HeadlinePossessOnSuper", string.Empty);

            if (!isCN88 || customerInfoViewModel.SearchAccountRoleId == (int)Role.Headline)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "CN88ViewPT");
            }
        }

        private void ShowHideBlock(ref string htmlContent, bool isShow, string visibleBlockName, string hiddenBlockName)
        {
            if (isShow)
            {
                TemplateHandler.RemoveBlock(ref htmlContent, hiddenBlockName);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlContent, visibleBlockName);
            }
        }

        private void RenderCommonInfo(ref string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref htmlcontent, "{ParentId}", customerInfoViewModel.ParentId.ToString());
            TemplateHandler.Assign(ref htmlcontent, "{RoleId}", customerInfoViewModel.SearchAccountRoleId.ToString());
            TemplateHandler.Assign(ref htmlcontent, "{CustId}", customerInfoViewModel.CustId);
            TemplateHandler.Assign(ref htmlcontent, "{Username}", customerInfoViewModel.UserName);
        }

        private void AssignCustomerSession(CustomerInfoViewModel customerInfoViewModel)
        {
            AppSession.CustRoleId = customerInfoViewModel.SearchAccountRoleId;
            AppSession.CustID = customerInfoViewModel.CustId;
            AppSession.CustUName = customerInfoViewModel.UserName;
            if (customerInfoViewModel.SearchAccountRoleId != (int)Role.Headline)
            {
                AppSession.MemberID = customerInfoViewModel.MemberId;
                AppSession.MemberUserName = customerInfoViewModel.MemberName;

                AppSession.AgentID = customerInfoViewModel.AgentId;
                AppSession.AgentUserName = customerInfoViewModel.AgentName;

                AppSession.MasterID = customerInfoViewModel.MasterId;
                AppSession.MasterUserName = customerInfoViewModel.MasterName;

                AppSession.SuperID = customerInfoViewModel.SuperId;
                AppSession.SusperUserName = customerInfoViewModel.SuperName;

                AppSession.IsInternal = customerInfoViewModel.IsInternalUser;
            }
        }

        private string GetSettingAbnormal(int roleId, bool isManageCustomerAccess, int currentLevel)
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

        private string AssignJS(Hashtable param)
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
