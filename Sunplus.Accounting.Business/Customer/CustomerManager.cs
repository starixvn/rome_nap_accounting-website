using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Fanex.Tarus.LiveCasino.Distributor;
using Fanex.Tarus.LiveCasino.Entities;
using IBCUtils.Objects;
using Sunplus.Accounting.Business.CustSetting;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Bingo.Core;
using Result = Sunplus.Accounting.Entities.Result;
using System.Xml;
using Sunplus.Accounting.DataAccess.AdoNet;

/// <summary>
/// Summary description for CustomerManager
/// </summary>
namespace Sunplus.Accounting.Business.Customer
{
    public class CustomerManager : ICustomerManager
    {
        public ICustomerDAO _customerDAO;


        public CustomerManager()
            : this(null)
        {

        }

        public CustomerManager(ICustomerDAO customerDAO)
        {
            _customerDAO = customerDAO ?? new CustomerDAO();
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

        public Hashtable GenDownlineList(int custId, int roleId, int pageIndex)
        {
            Hashtable returndata = new Hashtable();
            string content = "";
            string tplDownlineList = WebUtils.LoadTemplate("CustomerInfo/DownlineList.htm");
            string tplRowData = TemplateHandler.CopyBlock(tplDownlineList, "rowdata");
            if (roleId <= IBCUtils.Objects.UserInfo.ROLE_MEMBER || roleId > IBCUtils.Objects.UserInfo.ROLE_SUPER)
            {
                content = "<div style='width:250px;text-align:center;'>No downline list</div>";
                returndata.Add("Content", content);
                returndata.Add("Totalpages", 1);
                return returndata;
            }
            DbResult lsDownline = SiteProvider.Customer().GetDownLineList(custId, 100, pageIndex);
            DataRowCollection rows = lsDownline.ResultSet.Tables[0].Rows;
            int totalDownline = lsDownline.ResultSet.Tables[0].Rows.Count;
            int TotalPages = (int)lsDownline.Outputs["totalpages"];

            if (totalDownline == 0)
            {
                content = "<div style='width:250px;text-align:center;'>No downline list</div>";
                returndata.Add("Content", content);
                returndata.Add("Totalpages", 1);
                return returndata;
            }
            content = "";
            for (int i = 0; i < totalDownline; i = i + 2)
            {
                DataRow row = rows[i];
                string rowHtml = tplRowData;
                TemplateHandler.Assign(ref rowHtml, "{user1}", HttpUtility.HtmlDecode(row["username"] + ""));
                if (i + 1 < totalDownline)
                {
                    row = rows[i + 1];
                    TemplateHandler.Assign(ref rowHtml, "{user2}", HttpUtility.HtmlDecode(row["username"] + ""));
                }
                else
                {
                    TemplateHandler.Assign(ref rowHtml, "{user2}", "&nbsp;");
                }

                content += rowHtml;
            }
            TemplateHandler.ReplaceBlock(ref tplDownlineList, "rowdata", content);
            string strpager = "";
            if (TotalPages > 1)
            {
                strpager = Pager.InitPager(TotalPages, pageIndex, "pageritemclick");
            }

            returndata.Add("Content", tplDownlineList);
            returndata.Add("Totalpages", TotalPages);
            returndata.Add("Pager", strpager);
            return returndata;
        }

        public string GenOnlineList(int custId)
        {

            string tplOnlineList = WebUtils.LoadTemplate("CustomerInfo/OnlineList.htm");
            string tplRowData = TemplateHandler.CopyBlock(tplOnlineList, "rowdata");
            string data = "";
            DataRowCollection rows = DataAccess.IBCDLL.UserController.CustomerSelOnline(custId);
            if (rows != null && rows.Count > 0)
            {
                for (int i = 0; i < rows.Count; ++i)
                {
                    DataRow row = rows[i];
                    string rowhtml = tplRowData;
                    TemplateHandler.Assign(ref rowhtml, "{no}", (i + 1) + "");
                    TemplateHandler.Assign(ref rowhtml, "{ip}", "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", IpManagement.ReplaceVNIP(row["IP"].ToString())) + "\" target=\"_blank\">" + IpManagement.ReplaceVNIP(row["IP"].ToString()) + "</a>");
                    TemplateHandler.Assign(ref rowhtml, "{lastcheckin}", IBCUtils.FormatHandler.FormatDateTimeEn12(row["reload_time"]));
                    data += rowhtml;
                }
            }
            else
            {
                data += "<tr class='calign'><td colspan='3'>This user is not online</td></tr>";
            }
            TemplateHandler.ReplaceBlock(ref tplOnlineList, "rowdata", data);
            return tplOnlineList;


        }

        public CustomerDetail GetCustomerDetail(string username)
        {
            try
            {
                return IBCProvider.IBCCustomer.GetCustomerDetail(username);
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
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

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        public Hashtable GetHtmlCustInfoContent(string userName)
        {
            Hashtable arrcontent = new Hashtable();
            string htmlcontent = "";
            if (string.IsNullOrEmpty(userName))
            {
                htmlcontent = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
            }
            else
            {

                var customerService = new CustomerService();
                var ds = customerService.GetCustomerInfo(userName, AppSession.SYSTEM);

                if (ds == null || ds.ResultSet.Tables.Count == 0)
                {
                    //return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                    htmlcontent = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                }
                else
                {
                    //Add CustSearch


                    #region assign data
                    if (ds.ResultSet.Tables[0].Rows.Count == 0)
                    {
                        //return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                        htmlcontent = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                    }
                    else
                    {
                        DataRow row1 = ds.ResultSet.Tables[0].Rows[0];

                        DataRow row2 = ds.ResultSet.Tables[1].Rows[0];
                        DataRow row3 = null;
                        if (ds.ResultSet.Tables.Count > 2)
                        {
                            row3 = ds.ResultSet.Tables[2].Rows[0];
                        }
                        userName = row1["username"].ToString();
                        AppSession.CustUName = userName;
                        AppSession.MemberID = CastAide.AsInt32(row2["MemberID"]);
                        AppSession.AgentID = CastAide.AsInt32(row2["AgentID"]);
                        AppSession.MasterID = CastAide.AsInt32(row2["MasterID"]);
                        AppSession.SuperID = CastAide.AsInt32(row2["SuperID"]);
                        AppSession.MemberUserName = CastAide.AsString(row2["MemberName"]);
                        AppSession.AgentUserName = CastAide.AsString(row2["AgentName"]);
                        AppSession.MasterUserName = CastAide.AsString(row2["MasterName"]);
                        AppSession.SusperUserName = CastAide.AsString(row2["SuperName"]);

                        string downlinename = "";
                        int custId = Convert.ToInt32(row1["custid"]);
                        int roleId = Convert.ToInt32(row1["roleid"]);

                        bool isDeposit = WebSettings.IsApplySplitingDepositeCustomer ? DNAide.Core.CastAide.AsBoolean(row1["IsDeposit"]) : false;

                        Hashtable jsparam = new Hashtable();
                        jsparam.Add("custid", custId);
                        jsparam.Add("roleid", roleId);

                        AppSession.CustID = custId;
                        AppSession.CustRoleId = roleId;
                        AppSession.IsSyncCSCurrentCust = CastAide.AsBoolean(row1["synccasino"], false);
                        AppSession.IsSyncBingoCurrentCust = CastAide.AsBoolean(row1["syncbingo"], false);
                        AppSession.IsSyncLCSCurrentCust = WebSettings.EnableLiveCasino ? CastAide.AsBoolean(row1["synclivecasino"], false) : false;
                        AppSession.IsHideVirtualSportsCurrentCust = WebSettings.EnableVirtualSports ? CastAide.AsBoolean(row1["hidevirtualsports"], false) : false;

                        string firstname = Convert.IsDBNull(row1["firstname"]) ? "" : row1["firstname"].ToString();
                        string lastname = Convert.IsDBNull(row1["lastname"]) ? "" : row1["lastname"].ToString();
                        DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdUserSearch(AppSession.AccId, userName, firstname, lastname);

                        string lastLoginDate = Convert.IsDBNull(row1["lastlogged"]) ? "" : Convert.ToDateTime(row1["lastlogged"]).ToShortDateString();
                        string lastLoginTime = Convert.IsDBNull(row1["lastlogged"]) ? "" : " " + Convert.ToDateTime(row1["lastlogged"]).ToShortTimeString();
                        string lastIp = Convert.IsDBNull(row1["lastloginip"]) ? "" : row1["lastloginip"].ToString();
                        string customerSite = (roleId == (int)Globals.AccountType.Member) ? CastAide.AsString(row1["membersite"], "") : CastAide.AsString(row1["site"], "");
                        string createdate = Convert.IsDBNull(row1["createdate"]) ? "" : Convert.ToDateTime(row1["createdate"]).ToShortDateString();
                        string createtime = Convert.IsDBNull(row1["createdate"]) ? "" : Convert.ToDateTime(row1["createdate"]).ToShortTimeString();

                        string pwdExpiryDate = Convert.IsDBNull(row1["pwdexpiry"]) ? "" : Convert.ToDateTime(row1["pwdexpiry"]).ToShortDateString();
                        string pwdExpiryTime = Convert.IsDBNull(row1["pwdexpiry"]) ? "" : " " + Convert.ToDateTime(row1["pwdexpiry"]).ToShortTimeString();
                        decimal credit = Convert.ToDecimal(row1["credit"]);
                        decimal virtualRate = Convert.ToDecimal(row1["virtualmultiplier"]);
                        decimal actualRate = Convert.ToDecimal(row1["actualrate"]);
                        string currency = row1["currency"].ToString();
                        bool important = Convert.IsDBNull(row1["important"]) ? false : Convert.ToBoolean(row1["important"]);
                        bool suspend = Convert.IsDBNull(row1["suspended"]) ? false : Convert.ToBoolean(row1["suspended"]);
                        bool autosuspend = Convert.IsDBNull(row1["autosuspend"]) ? false : Convert.ToBoolean(row1["autosuspend"]);
                        bool UplineAutoSuspend = CastAide.AsBoolean(row2["uplineautosuspend"]);

                        bool closed = Convert.IsDBNull(row1["closed"]) ? false : Convert.ToBoolean(row1["closed"]);
                        bool isBA = CastAide.AsInt32(row1["isba"]) == 1 ? true : false;
                        arrcontent.Add("isba", isBA);
                        CustomerDetail _custparent = new CustomerDetail();

                        int parentId = 0;
                        string lastTxnDate = string.Empty;
                        string lastTxnTime = string.Empty;
                        string phone = row1["phone"] + "";
                        string mobile = row1["mobilephone"] + "";
                        string fax = row1["fax"] + "";
                        string nickname = CastAide.AsString(row1["username2"], string.Empty);

                        virtualRate = virtualRate * 100;
                        actualRate = (virtualRate + 100) * actualRate / 100;
                        string vRate = ((virtualRate > 0) ? "+" : string.Empty) + IBCUtils.FormatHandler.FormatNumber4(virtualRate) + " (" + IBCUtils.FormatHandler.FormatNumber4(actualRate) + ")";
                        if (row3 != null && roleId <= 4)
                        {
                            lastTxnDate = Convert.IsDBNull(row3["lastTxn"]) ? "" : Convert.ToDateTime(row3["lastTxn"]).ToShortDateString();
                            lastTxnTime = Convert.IsDBNull(row3["lastTxn"]) ? "" : " " + Convert.ToDateTime(row3["lastTxn"]).ToShortTimeString();
                        }

                        //=============Balance, cash, oustanding
                        decimal outStanding = 0, accOutstanding = 0;
                        decimal cash = 0, totalBal = 0;
                        decimal yesterdayCash = 0, yesterdayBal = 0;
                        decimal betCredit = 0, usedCredit = 0;

                        switch (roleId)
                        {
                            case 1: // member
                                parentId = Convert.ToInt32(row2["agentId"]);
                                accOutstanding = Convert.IsDBNull(row3["outstanding"]) ? 0 : Convert.ToDecimal(row3["outstanding"]);
                                cash = Convert.IsDBNull(row3["cash"]) ? 0 : Convert.ToDecimal(row3["cash"]);
                                betCredit = credit + cash - accOutstanding;
                                yesterdayCash = Convert.IsDBNull(row3["Yesterday_Cash"]) ? 0 : Convert.ToDecimal(row3["Yesterday_Cash"]);
                                downlinename = "";
                                break;
                            case 2: // agent
                                parentId = Convert.ToInt32(row2["masterid"]);
                                accOutstanding = Convert.IsDBNull(row3["agentPositionTaking"]) ? 0 : Convert.ToDecimal(row3["agentPositionTaking"]);
                                outStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                                cash = Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]);
                                totalBal = cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"]));
                                yesterdayCash = Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]);
                                yesterdayBal = yesterdayCash + (Convert.IsDBNull(row3["Yesterday_CustomerCash"]) ? 0 : Convert.ToDecimal(row3["Yesterday_CustomerCash"]));
                                usedCredit = IBCUtils.FormatHandler.GetDecimal(row3["agentUsedCredit"]);
                                downlinename = "Member";
                                break;
                            case 3: // master
                                parentId = Convert.ToInt32(row2["superId"]);
                                accOutstanding = Convert.IsDBNull(row3["masterPositionTaking"]) ? 0 : Convert.ToDecimal(row3["masterPositionTaking"]);
                                outStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                                cash = Convert.IsDBNull(row3["masterCash"]) ? 0 : Convert.ToDecimal(row3["masterCash"]);
                                totalBal = cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"])) +
                                    (Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]));
                                yesterdayCash = Convert.IsDBNull(row3["yesterday_masterCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_masterCash"]);
                                yesterdayBal = yesterdayCash + (Convert.IsDBNull(row3["yesterday_customerCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_customerCash"])) +
                                    (Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]));
                                usedCredit = IBCUtils.FormatHandler.GetDecimal(row3["masterUsedCredit"]);
                                downlinename = "Agent";
                                break;
                            case 4: // super
                                parentId = 0;
                                accOutstanding = Convert.IsDBNull(row3["superPositionTaking"]) ? 0 : Convert.ToDecimal(row3["superPositionTaking"]);
                                outStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                                cash = Convert.IsDBNull(row3["superCash"]) ? 0 : Convert.ToDecimal(row3["superCash"]);
                                totalBal = cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"]))
                                    + (Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]))
                                    + (Convert.IsDBNull(row3["masterCash"]) ? 0 : Convert.ToDecimal(row3["masterCash"]));
                                yesterdayCash = Convert.IsDBNull(row3["yesterday_superCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_superCash"]);
                                yesterdayBal = yesterdayCash + (Convert.IsDBNull(row3["yesterday_customerCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_customerCash"]))
                                    + (Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]))
                                    + (Convert.IsDBNull(row3["yesterday_masterCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_masterCash"]));
                                usedCredit = IBCUtils.FormatHandler.GetDecimal(row3["superUsedCredit"]);
                                downlinename = "Master";
                                break;
                            default:
                                break;
                        }

                    #endregion

                        if (parentId > 0)
                        {
                            _custparent = IBCProvider.IBCCustomer.GetCustomerStatus(parentId);
                        }


                        bool isAllowEditCS = true;
                        bool disabledCSParent = false;
                        if (parentId != 0)
                        {
                            isAllowEditCS = Permissions.IsAllowEditCasino(parentId, _custparent.IsCasinoSync);
                            disabledCSParent = _custparent.IsDisableCasino;
                        }

                        bool isAllowEditBI = true;
                        bool disabledBIParent = false;
                        if (parentId != 0)
                        {
                            isAllowEditBI = Permissions.IsAllowEditBingo(parentId, _custparent.IsBingoSync);
                            disabledBIParent = _custparent.IsDisabledBingoGame;
                        }

                        bool isAllowEditLCS = true;
                        bool isAllowEditVS = true;
                        bool disabledLCSParent = false;
                        if (parentId != 0)
                        {
                            isAllowEditLCS = Permissions.IsAllowEditLiveCasino(parentId, _custparent.IsLiveCasinoSync);
                            isAllowEditVS = Permissions.IsAllowEditVirtualSports(parentId, _custparent.IsHideVirtualSports);
                            disabledLCSParent = _custparent.IsDisableLiveCasino;
                        }

                        var disalbedVSParent = false;
                        if (parentId != 0)
                        {
                            disalbedVSParent = _custparent.IsDisableVirtualSports;
                        }

                        string statusClass = " ";
                        string statusTip = "";
                        bool isOpenStatus = true;
                        if (!CastAide.AsBoolean(row1["Enabled"]))
                        {
                            statusClass = "memberDisabled ";
                            statusTip = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount");
                            isOpenStatus = false;
                        }
                        else if (CastAide.AsBoolean(row1["closed"]))
                        {
                            statusClass = "memberClosed ";
                            statusTip = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblClosedAccount");
                            isOpenStatus = false;
                        }
                        else if (CastAide.AsBoolean(row1["suspended"]))
                        {
                            statusClass = "memberSuspended ";
                            statusTip = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuspendedAccount");
                            isOpenStatus = false;
                        }
                        else
                        {
                            statusClass = " ";
                        }

                        if (roleId <= UserInfo.ROLE_SUPER)
                        {
                            string name = "";
                            string fullname = firstname + " - " + lastname;
                            //name=fullname.Length < 26 ? fullname : fullname.Substring(0,26) + "...";
                            name = fullname;
                            var tplURL = "CustomerInfo/CustomerInfo.htm";
                            if (AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL) && roleId == 4)
                            {
                                tplURL = "CustomerInfo/CustomerInfoSuper.htm";
                            }
                            htmlcontent = WebUtils.LoadTemplateBySystem(tplURL, true);
                            //Customer info
                            string shortUserName = userName.Length < 20 ? userName : userName.Substring(0, 20) + "...";

                            if (roleId == 1)
                            {
                                TemplateHandler.Assign(ref htmlcontent, "{memberClass}", "memberClass");
                                TemplateHandler.Assign(ref htmlcontent, "{onclickUName}", "onclick=\"parent.submitSearch('" + userName + "');\" ");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref htmlcontent, "{cursorhand}", "");
                                TemplateHandler.Assign(ref htmlcontent, "{onclickUName}", " ");
                            }

                            TemplateHandler.Assign(ref htmlcontent, "{statusClass}", statusClass);
                            TemplateHandler.Assign(ref htmlcontent, "{UserName}", shortUserName);
                            TemplateHandler.Assign(ref htmlcontent, "{FullUserName}", userName);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerName}", name);
                            TemplateHandler.Assign(ref htmlcontent, "{FullCustomerName}", fullname);
                            TemplateHandler.Assign(ref htmlcontent, "{NickName}", nickname);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerFax}", fax);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerPhone}", phone);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerMobile}", mobile);
                            //info for internal agent login
                            string rndStr = Encryption.RandomString(6);
                            string accKey = FormsAuthentication.HashPasswordForStoringInConfigFile(AppSession.AccPwd + rndStr, "MD5");
                            TemplateHandler.Assign(ref htmlcontent, "{ThemePath}", WebSettings.ThemePath);
                            TemplateHandler.Assign(ref htmlcontent, "{RndStr}", rndStr);
                            TemplateHandler.Assign(ref htmlcontent, "{AccKey}", accKey);
                            TemplateHandler.Assign(ref htmlcontent, "{AccId}", AppSession.AccId.ToString());
                            TemplateHandler.Assign(ref htmlcontent, "{InternalAgent}", WebSettings.InternalAgentUrl);

                            //Render Enabled 
                            if (isOpenStatus)
                            {
                                TemplateHandler.Assign(ref htmlcontent, "{enabledbg}", string.Empty);
                                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipover}", string.Empty);
                                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipout}", string.Empty);
                            }
                            else
                            {
                                //TemplateHandler.Assign(ref htmlcontent, "{enabledbg}", "background-color:yellow;");
                                TemplateHandler.Assign(ref htmlcontent, "{enabledbg}", "");
                                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipover}", "Tip('" + statusTip + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)");
                                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipout}", "UnTip()");
                            }

                            GenUplineBox(ref htmlcontent, roleId, row2);
                            bool isInternalUser = false;
                            if (DataUtils.IsExistKey(row1, "IsInternal"))
                                isInternalUser = Convert.IsDBNull(row1["IsInternal"]) ? false : Convert.ToBoolean(row1["IsInternal"]);
                            AppSession.IsInternal = isInternalUser;
                            if (!Permissions.FinancePermission.IsViewFiAcc(isInternalUser))
                            {
                                //return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                                htmlcontent = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                            }
                            if (WebSettings.OfferDoubleComm)
                            {
                                var doubleComm = CastAide.AsBoolean(row1["doublecommlosing"]);
                                GenTransLinskBox(ref htmlcontent, custId, roleId, !WebSettings.disabledHR, isInternalUser, userName, isDeposit, doubleComm);
                            }
                            else
                            {
                                GenTransLinskBox(ref htmlcontent, custId, roleId, !WebSettings.disabledHR, isInternalUser, userName, isDeposit, false);
                            }

                            GenLoginDetail(ref htmlcontent, custId, roleId, row1, row3, userName);

                            try
                            {
                                if (roleId > 1)
                                {
                                    bool NicknameLoginOnly = Convert.IsDBNull(row1["NicknameLoginOnly"]) ? false : Convert.ToBoolean(row1["NicknameLoginOnly"]);
                                    TemplateHandler.Assign(ref htmlcontent, "{loginoptions}", NicknameLoginOnly ? "Nickname Only" : "Username/Nickname");
                                }
                                else
                                {
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "loginoption");
                                }
                            }
                            catch (Exception ex) { }
                            #region BalanceInfo
                            string balanceinfo = WebUtils.LoadTemplateBySystem("CustomerInfo/BalanceInfo.htm");

                            bool isManageCustomerAccess = Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust); // 4: id of [Customer] function               
                            bool hasTransferPermission = Permissions.GetHostPermission(Enumerations.Functions.Transfer.OtherTrans); // 2: Transfer
                            bool isManageVirtualRateAccess = Permissions.GetHostPermission(Enumerations.Functions.ExRates.VirtualRate); // [VirtualRate]
                            TemplateHandler.Assign(ref balanceinfo, "{colorTotalCash}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((cash >= 0) ? " b" : " hilight"));

                            TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayCash}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((yesterdayCash >= 0) ? " b" : " hilight"));
                            if (string.IsNullOrEmpty(cash.ToString())) cash = 0;
                            if (string.IsNullOrEmpty(yesterdayCash.ToString())) yesterdayCash = 0;
                            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBal}", IBCUtils.FormatHandler.FormatNumber20(cash));

                            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBalCur}", string.Format("{0:n}", cash));
                            TemplateHandler.Assign(ref balanceinfo, "{CurTotalCashBalCur}", currency + " ");

                            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBal}", IBCUtils.FormatHandler.FormatNumber20(yesterdayCash));
                            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBalCur}", string.Format("{0:n}", yesterdayCash));
                            TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayCashBalCur}", currency + " ");

                            TemplateHandler.Assign(ref balanceinfo, "{AccOutstandTxn}", string.Format("{0:n}", currency + " " + IBCUtils.FormatHandler.FormatNumber20(accOutstanding)));
                            //Check Given Credit Permission
                            bool isallowupdategivencredit = Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);
                            if (isallowupdategivencredit == true)
                            {
                                TemplateHandler.Assign(ref balanceinfo, "{givencreditOnclick}", "return ChangeGivenCredit(true);");
                                TemplateHandler.Assign(ref balanceinfo, "{givencreditClass}", "canchange");
                                TemplateHandler.RemoveBlock(ref balanceinfo, "NotAllowGiven");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref balanceinfo, "{virtualRateClass}", string.Empty);
                                TemplateHandler.RemoveBlock(ref balanceinfo, "AllowGiven");
                            }

                            if (isManageVirtualRateAccess)
                            {
                                TemplateHandler.Assign(ref balanceinfo, "{virtualRateOnclick}", "return OpenEditVirtualExRate(" + custId + ",'" + userName + "','" + AccCommon.GetRoleName(roleId) + "');");
                                TemplateHandler.Assign(ref balanceinfo, "{virtualRateClass}", "canchange");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref balanceinfo, "{virtualRateOnclick}", string.Empty);
                                TemplateHandler.Assign(ref balanceinfo, "{virtualRateClass}", string.Empty);
                            }
                            TemplateHandler.Assign(ref balanceinfo, "{GivenCreditCurrency}", currency + " ");
                            TemplateHandler.Assign(ref balanceinfo, "{GivenCredit}", string.Format("{0:n}", credit));



                            TemplateHandler.Assign(ref balanceinfo, "{VirtualRate}", vRate);


                            if (roleId > 1)
                            {
                                TemplateHandler.RemoveBlock(ref balanceinfo, "betcredit");

                                //TemplateHandler.Assign(ref balanceinfo, "{IsHiddenAgentInfo}", "");
                                TemplateHandler.Assign(ref balanceinfo, "{colorTotalBal}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((totalBal >= 0) ? " b" : " hilight"));
                                TemplateHandler.Assign(ref balanceinfo, "{TotalBal}", string.Format("{0:####.##}", totalBal));
                                TemplateHandler.Assign(ref balanceinfo, "{TotalBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(totalBal)));
                                TemplateHandler.Assign(ref balanceinfo, "{CurTotalBalCur}", currency + " ");

                                TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayBal}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((yesterdayBal >= 0) ? " b" : " hilight"));
                                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBal}", string.Format("{0:####.##}", yesterdayBal));
                                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(yesterdayBal)));
                                TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayBalCur}", currency + " ");

                                TemplateHandler.Assign(ref balanceinfo, "{OutstandTxn}", string.Format("{0:n}", currency + " " + IBCUtils.FormatHandler.FormatNumber20(outStanding)));
                                TemplateHandler.Assign(ref balanceinfo, "{UsedCredit}", string.Format("{0:n}", currency + " " + IBCUtils.FormatHandler.FormatNumber20(usedCredit)));
                                TemplateHandler.Assign(ref balanceinfo, "{AgentType}", downlinename);
                                TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "");
                                TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "hidden");

                                GetDownlineCountingInfo(ref balanceinfo, ds, roleId);
                            }
                            else
                            {
                                TemplateHandler.Assign(ref balanceinfo, "{BetCredit}", currency + " " + Sunplus.Accounting.Core.Helper.Utils.GetDecimalRoundDown(betCredit));

                                TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "hidden");
                                TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "");

                                TemplateHandler.Assign(ref balanceinfo, "{masterhidden}", "hidden");
                                TemplateHandler.Assign(ref balanceinfo, "{agenthidden}", "hidden");
                                TemplateHandler.Assign(ref balanceinfo, "{memberhidden}", "hidden");
                            }

                            TemplateHandler.Assign(ref htmlcontent, "{BalanceInfo}", balanceinfo);
                            #endregion
                            //Setting       
                            #region Setting
                            var strCloseStatus = "";
                            if (AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL) && roleId == 4 && closed)
                            {
                                strCloseStatus = "<span class='chkchecked'></span>";
                            }
                            TemplateHandler.Assign(ref htmlcontent, "{closedCustomer}", strCloseStatus != "" ? strCloseStatus : GetClosedStatus(isManageCustomerAccess, "closed", closed, (roleId > 1), _custparent.IsClosed, isallowupdategivencredit));

                            //Limit per IP
                            if (roleId < 2) //Member
                            {
                                TemplateHandler.Assign(ref htmlcontent, "{LimitPerIP}", "");
                                //TemplateHandler.RemoveBlock(ref strLimitPerIP, "IsUpdateDL");
                                //TemplateHandler.Assign(ref strLimitPerIP, "{disabledDL}", currentLimitPerIP > 0 ? " disabled='true' " : "");
                                //TemplateHandler.Assign(ref strLimitPerIP, "{jsonclick}", "");
                                //TemplateHandler.Assign(ref strLimitPerIP, "{classname}", "");
                            }
                            else
                            {
                                int currentLimitPerIP = CastAide.AsInt16(row1["LimitPerIP"]); ;
                                int parentLimiPerIP = 0;

                                if (roleId > 1 && roleId < 4)
                                {
                                    parentLimiPerIP = _custparent.LimitPerIP;
                                }
                                int maxLimitPerIP = Sunplus.Accounting.Core.Common.Constants.MaxLimitPerIP;

                                string strLimitPerIP = WebUtils.LoadTemplate("LimitedPerIP.htm");
                                string optionLimitPerIP = "";
                                if (parentLimiPerIP == 0)
                                {
                                    for (int i = 0; i <= maxLimitPerIP; i++)
                                    {
                                        if (i == currentLimitPerIP)
                                        {
                                            optionLimitPerIP += "<option value='" + i.ToString() + "' selected='true'>" + i.ToString() + "</option>";
                                        }
                                        else
                                        {
                                            optionLimitPerIP += "<option value='" + i.ToString() + "' >" + i.ToString() + "</option>";
                                        }
                                    }
                                    TemplateHandler.Assign(ref strLimitPerIP, "{disabledLimit}", "");
                                    TemplateHandler.Assign(ref strLimitPerIP, "{LimitPerIPOption}", optionLimitPerIP);

                                    TemplateHandler.Assign(ref strLimitPerIP, "{lblUpdateDownline}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblApplyDownline"));
                                    TemplateHandler.Assign(ref strLimitPerIP, "{disabledDL}", currentLimitPerIP > 0 ? " disabled='true' " : "");
                                    if (Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust))
                                    {
                                        TemplateHandler.Assign(ref strLimitPerIP, "{jsonclick}", "showhideLimitPerIP()");
                                        TemplateHandler.Assign(ref strLimitPerIP, "{classname}", "canchange");
                                    }
                                    else
                                    {
                                        TemplateHandler.Assign(ref strLimitPerIP, "{jsonclick}", "");
                                        TemplateHandler.Assign(ref strLimitPerIP, "{classname}", "");
                                    }
                                    if (roleId < 3)
                                    {
                                        TemplateHandler.Assign(ref strLimitPerIP, "{ishideapplydownline}", "style='display:none;'");
                                    }

                                }
                                else
                                {
                                    TemplateHandler.RemoveBlock(ref strLimitPerIP, "IsUpdateDL");
                                    TemplateHandler.Assign(ref strLimitPerIP, "{jsonclick}", "");
                                    TemplateHandler.Assign(ref strLimitPerIP, "{classname}", "");
                                }
                                TemplateHandler.Assign(ref strLimitPerIP, "{currValue}", currentLimitPerIP);
                                TemplateHandler.Assign(ref htmlcontent, "{LimitPerIP}", strLimitPerIP);
                            }
                            //End limit per IP

                            //Enable P2P
                            var p2pStatus = PokerSection.Instance[PokerSection.PokerCollections.CustomerSetting]["visibleP2PStatus"];
                            bool visibleP2PStatus = CastAide.AsBoolean(p2pStatus.Value);
                            if (!visibleP2PStatus)
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "p2pstatus");
                            }

                            if (Functions.Settings.IsVisibleSpecial)
                            {
                                TemplateHandler.Assign(ref htmlcontent, "{specialCustomer}", StatusSpan(isManageCustomerAccess, "special", important, (roleId > 1)));
                            }

                            //HuyNguyen Dangerous
                            if (Functions.Settings.IsVisibleDangerous)
                            {
                                if (roleId > 2)
                                {
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "edited-dangerous");
                                }
                                else
                                {
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "dangerous");
                                    string[] dangeruostype = new string[] { "Original", "ABT", "Other Sports" };

                                    int dangerLevel = Convert.IsDBNull(row1["danger"]) ? 0 : Convert.ToInt32(row1["danger"]);
                                    int danger2Level = Convert.IsDBNull(row1["danger2"]) ? 0 : Convert.ToInt32(row1["danger2"]);
                                    int danger3Level = Convert.IsDBNull(row1["danger3"]) ? 0 : Convert.ToInt32(row1["danger3"]);


                                    int[] currentlevels = new int[] { dangerLevel, danger2Level, danger3Level };

                                    TemplateHandler.Assign(ref htmlcontent, "{ThemePath}", WebSettings.ThemePath);
                                    TemplateHandler.Assign(ref htmlcontent, "{dangerousCustomer}", GetSettingControlDangerous(roleId, isManageCustomerAccess, dangeruostype, currentlevels, isBA));
                                }
                            }

                            #region Abnormal
                            if (WebSettings.EnableAbnormal)
                            {
                                if (roleId > 2)
                                {
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Abnormal");
                                }
                                else
                                {
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "Abnormal");
                                    int currentLevel = Convert.IsDBNull(row1["abnormal"]) ? 0 : Convert.ToInt32(row1["abnormal"]);
                                    TemplateHandler.Assign(ref htmlcontent, "{AbnormalCustomer}", GetSettingAbnormal(roleId, isManageCustomerAccess, currentLevel));
                                }
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "AbnormalSession");
                            }
                            #endregion
                            GenSettingControl(ref htmlcontent, row1, row3, roleId, parentId, _custparent,
                                              isManageCustomerAccess, suspend, autosuspend, UplineAutoSuspend, isAllowEditCS,
                                              disabledCSParent, isAllowEditBI, disabledBIParent, isAllowEditLCS,
                                              disabledLCSParent, disalbedVSParent);

                            #endregion
                            //Menu detail
                            #region MenuDetail
                            GenMenuMinMaxPT(ref htmlcontent, row2, custId, roleId, currency, userName, (isAllowEditBI && AppSession.IsSyncBingoCurrentCust), isDeposit);

                            string infoUrl = "";
                            string detailUrl = "";
                            string downlineListURL = "";
                            string addDownlineURL = "";
                            string addDownlineHtml = "');\"> Add New (New)</a><br /></span><span class=\"{IsHidden1}\"><a class=\"button_left02\" href=\"javascript:parent.gotoURL('centerFr','";
                            string addDownlineURL_New = "";
                            string detailUrlNew = "";
                            string bingoLinkSetting = "";
                            string pokerLinkSetting = "";
                            var liveCasinoSetting = "";
                            string RacingUpdateURL = "";
                            var VirtualSportsSetting = "";
                            string bettingLimitURL = "";
                            switch (roleId)
                            {
                                case 1: // member
                                    parentId = Convert.ToInt32(row2["agentId"]);
                                    bingoLinkSetting = "Customer/PT/Bingo/MemberUpdate.aspx?custId=" + custId;
                                    pokerLinkSetting = "Customer/PT/Poker/MemberUpdate.aspx?custId=" + custId;
                                    detailUrl = isDeposit ? "DepositCustomer/EditSettingAgent" : "Customer/v2/EditCustomerNew.aspx?set=1&custId=" + custId + "&agentId=" + row2["agentId"] + "&uname=" + userName + "&m=0";
                                    detailUrlNew = isDeposit ? "DepositCustomer/EditSettingAgent" : "EditMember/EditMemberPage?custId=" + custId + "&agentId=" + row2["agentId"] + "&uname=" + userName;
                                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                                    liveCasinoSetting = string.Format("LiveCasino/PTSetting/EditMember.aspx?custId={0}&uplineId={1}", custId, parentId);
                                    VirtualSportsSetting = string.Format("VirtualSportMemberPT/EditMember?custId={0}&uplineId={1}", custId, parentId);
                                    TemplateHandler.Assign(ref htmlcontent, "{IsHidden1}", "hidden");
                                    TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "");
                                    RacingUpdateURL = string.Format("EditMemberPTRacing/Index?custid={0}&uplineid={1}", custId, parentId);

                                    break;

                                case 2: // agent
                                    int _masterID = CastAide.AsInt32(row2["masterid"], -1);
                                    RacingUpdateURL = string.Format("EditAgentPTRacing/Index?custid={0}&uplineid={1}", custId, _masterID);
                                    bingoLinkSetting = "Customer/PT/Bingo/AgentUpdate.aspx?custId=" + custId;
                                    pokerLinkSetting = "Customer/PT/Poker/AgentUpdate.aspx?custId=" + custId;
                                    downlineListURL = "Customer/MemberList.aspx?agentId=" + custId;
                                    addDownlineURL_New = isDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMember/AddNewMemberPage?agentId=" + custId + "&username=" + userName;
                                    addDownlineURL = isDeposit ? "DepositCustomer/AddNewAgent" : "Customer/v2/AddNewMember.aspx?set=1&agentId=" + custId + "&username=" + userName;
                                    detailUrl = isDeposit ? "DepositCustomer/EditSettingAgent" : "Customer/v2/EditAgent.aspx?set=1&custId=" + custId + "&masterId=" + _masterID.ToString() + "&uname=" + userName;
                                    detailUrlNew = isDeposit ? "DepositCustomer/EditSettingAgent" : "EditAgent/EditAgentPage?" + "custId=" + custId + "&masterId=" + _masterID.ToString() + "&uname=" + userName;
                                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Agent";
                                    liveCasinoSetting = string.Format("LiveCasino/PTSetting/EditAgent.aspx?custId={0}&uplineId={1}", custId, _masterID);
                                    VirtualSportsSetting = string.Format("VirtualSportAgentPT/EditAgent?custId={0}&uplineId={1}", custId, _masterID);
                                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Agent");
                                    if (!CustomerUtils.IsTransferDaily(Convert.ToString(_masterID))) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
                                    break;
                                case 3: // master
                                    int _superID = CastAide.AsInt32(row2["superId"], -1);
                                    RacingUpdateURL = string.Format("EditMasterPTRacing/Index?custid={0}&uplineid={1}", custId, _superID);
                                    bingoLinkSetting = "Customer/PT/Bingo/MasterUpdate.aspx?custId=" + custId;
                                    pokerLinkSetting = "Customer/PT/Poker/MasterUpdate.aspx?custId=" + custId;
                                    detailUrl = isDeposit ? "DepositCustomer/EditSettingAgent" : "Customer/v2/EditMaster.aspx?set=1&custId=" + custId + "&superId=" + _superID.ToString() + "&uname=" + userName;
                                    detailUrlNew = isDeposit ? "DepositCustomer/EditSettingAgent" : "EditMaster/EditMasterPage?custId=" + custId + "&superId=" + _superID.ToString() + "&uname=" + userName;
                                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Master";
                                    addDownlineURL_New = isDeposit ? "DepositCustomer/AddNewAgent" : "AddNewAgent/AddNewAgentPage?masterId=" + custId + "&username=" + userName;
                                    addDownlineURL = isDeposit ? "DepositCustomer/AddNewAgent" : "Customer/v2/AddNewAgent.aspx?set=1&masterId=" + custId + "&username=" + userName;
                                    downlineListURL = "Customer/AgentList.aspx?masterId=" + custId;
                                    liveCasinoSetting = string.Format("LiveCasino/PTSetting/EditMaster.aspx?custId={0}&uplineId={1}", custId, _superID);
                                    VirtualSportsSetting = string.Format("VirtualSportMasterPT/EditMaster?custId={0}&uplineId={1}", custId, _superID);
                                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Master");
                                    break;
                                case 4: // super                                    
                                    parentId = 0;
                                    RacingUpdateURL = string.Format("EditSuperPTRacing/Index?custid={0}", custId);
                                    bingoLinkSetting = "Customer/PT/Bingo/SuperUpdate.aspx?custId=" + custId;
                                    pokerLinkSetting = "Customer/PT/Poker/SuperUpdate.aspx?custId=" + custId;
                                    addDownlineURL_New = isDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMaster/AddNewMasterPage?superId=" + custId + "&username=" + userName;
                                    addDownlineURL = isDeposit ? "DepositCustomer/AddNewAgent" : "Customer/V2/AddNewMaster.aspx?set=1&superId=" + custId + "&username=" + userName;
                                    downlineListURL = "Customer/MasterAgentList.aspx?superId=" + custId;
                                    detailUrl = isDeposit ? "DepositCustomer/EditSettingSuper" : "Customer/v2/AddNewSuper.aspx?set=1&custId=" + custId + "&uname=" + userName;
                                    detailUrlNew = isDeposit ? "DepositCustomer/EditSettingSuper" : "EditSuper/EditSuperPage?custId=" + custId + "&uname=" + userName;
                                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                                    liveCasinoSetting = string.Format("LiveCasino/PTSetting/EditSuper.aspx?custId={0}&exchangeId={1}", custId, CastAide.AsInt32(row1["exchangeid"]));
                                    VirtualSportsSetting = string.Format("VirtualSportSuperPT/EditSuper?custId={0}&uname={1}", custId, userName);
                                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Super");
                                    if (custId < 100) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime"); // for dummy super
                                    break;
                                default:
                                    break;
                            }

                            switch (WebSettings.IsShowCustomerSettingRenovation)
                            {
                                case 1:
                                    //run add new old and add new new
                                    addDownlineURL = addDownlineURL_New + addDownlineHtml + addDownlineURL;
                                    break;
                                case 2:
                                    //run only add new new
                                    addDownlineURL = addDownlineURL_New;
                                    break;
                                case 3:
                                    //run add new new and edit new:
                                    addDownlineURL = addDownlineURL_New;
                                    detailUrl = detailUrlNew;
                                    break;
                                default:
                                    //run only add new old
                                    break;
                            }

                            if (isDeposit)
                            {
                                //TemplateHandler.RemoveBlock(ref htmlcontent, "edit-casino");
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-finance");
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-bingo");
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-p2p");
                                /*TemplateHandler.RemoveBlock(ref htmlcontent, "edit-LiveCasino");*/
                                TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
                                TemplateHandler.RemoveBlock(ref htmlcontent, "maxexposure");
                                TemplateHandler.RemoveBlock(ref htmlcontent, "horseracing-editlink");
                                TemplateHandler.RemoveBlock(ref htmlcontent, "detail_edit_menu");
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-VirtualSport");
                            }
                            TemplateHandler.Assign(ref htmlcontent, "{RacingUpdateURL}", RacingUpdateURL);
                            TemplateHandler.Assign(ref htmlcontent, "{linkSettingInfo}", infoUrl);
                            TemplateHandler.Assign(ref htmlcontent, "{bingoLinkSetting}", bingoLinkSetting);
                            TemplateHandler.Assign(ref htmlcontent, "{pokerLinkSetting}", pokerLinkSetting + "&roleId=" + roleId.ToString());
                            TemplateHandler.Assign(ref htmlcontent, "{livecasinoLinkSetting}", liveCasinoSetting + "&roleId=" + roleId.ToString());
                            TemplateHandler.Assign(ref htmlcontent, "{virtualSportLinkSetting}", VirtualSportsSetting + "&roleId=" + roleId.ToString());
                           
                            string bettingSetting = WebSettings.ExternalBettingLimitConfig;
                            //AppSession.GroupID
                            if (!String.IsNullOrEmpty(bettingSetting) && bettingSetting.Contains("|"))
                            {
                                int groupId = CastAide.AsInt32(bettingSetting.Split(new char[] { '|' })[0], 0);
                                if (groupId != AppSession.GroupID && roleId==4 )
                                {
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "edit-BetSetting");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref htmlcontent, "{bettingLimitLinkSetting}", string.Format("customersetting/betsetting/sportsbook/index/{0}/{1}/{2}/{3}", custId, custId, AppSession.AccId.ToString(), AppSession.SYSTEM));
                                }
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-BetSetting");
                            }
                            

                            //---- for menu details -----------
                            AssignEditFinance(ref htmlcontent, roleId);
                            TemplateHandler.Assign(ref htmlcontent, "{ParentId}", parentId.ToString());
                            TemplateHandler.Assign(ref htmlcontent, "{RoleId}", roleId.ToString());
                            TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "hidden");
                            TemplateHandler.Assign(ref htmlcontent, "{linkSettingDetail}", detailUrl);
                            TemplateHandler.Assign(ref htmlcontent, "{linkDownlineList}", downlineListURL);
                            TemplateHandler.Assign(ref htmlcontent, "{linkAddnewDownline}", addDownlineURL);
                            string _username = row1["username"].ToString();
                            TemplateHandler.Assign(ref htmlcontent, "{CustId}", row1["custid"].ToString());
                            TemplateHandler.Assign(ref htmlcontent, "{Username}", _username);
                            //Edit Casino
                            //Qui.Nguyen  -- Casino setting


                            if (!Permissions.IsAllowCasino())
                            {
                                if (WebSettings.disableCS)
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "edit-casino");
                                if ((roleId < (int)Globals.AccountType.Super) && !isAllowEditCS)
                                    TemplateHandler.RemoveBlock(ref htmlcontent, "edit-casino");
                            }

                            //Removed check allow Current for HR
                            if (WebSettings.disabledHR) //|| !AccCommon.IsAllowCurrencyHR(currency))
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "horseracing-editlink");
                            }
                            //---------------------------------
                            if (!BingoSettings.BingoGame.Enabled || !isAllowEditBI)
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-bingo");
                            }
                            if (!WebSettings.EnableLiveCasino || !isAllowEditLCS)
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-LiveCasino");
                            }
                            if (!WebSettings.EnableVirtualSports || !isAllowEditVS)
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-VirtualSport");
                            }

                            #endregion

                            TemplateHandler.Assign(ref htmlcontent, "{jsparams}", AssignJS(jsparam));

                            var p2pSetting = PokerSection.Instance[PokerSection.PokerCollections.CustomerSetting]["visibleP2PSetting"];
                            bool visibleP2PSetting = CastAide.AsBoolean(p2pSetting.Value);
                            if (!visibleP2PSetting)
                            {
                                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-p2p");
                            }
                        }
                        else
                        {
                            htmlcontent = WebUtils.LoadTemplate("CustomerInfo/CustomerNoInfo.htm");
                            //Customer info
                            TemplateHandler.Assign(ref htmlcontent, "{UserName}", userName);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerName}", firstname + " - " + lastname);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerFax}", fax);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerPhone}", phone);
                            TemplateHandler.Assign(ref htmlcontent, "{CustomerMobile}", mobile);

                            TemplateHandler.Assign(ref htmlcontent, "{jsparams}", AssignJS(jsparam));

                            //info for internal agent login
                            //GenOnlineList(ref htmlcontent, custId);
                        }
                        if (roleId == 1)
                        {
                            TemplateHandler.RemoveBlock(ref htmlcontent, "hideonmember");
                        }
                    }
                }
            }

            arrcontent.Add("strcontent", htmlcontent);

            return arrcontent;
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

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        #region private functions

        #region GetSettingControl
        private void GenSettingControl(ref string htmlcontent, DataRow row1, DataRow row3, int roleId, int parentID,
            CustomerDetail CustParent, bool isManageCustomerAccess, bool isSuspended, bool autosuspend, bool UplineAutoSuspend,
            bool isAllowEdit, bool disabledCSParent, bool isAllowEditBI, bool disabledBIParent, bool isAllowEditLCS,
            bool disableLiveCasinoParent, bool disabledVSParent)
        {
            //Convert value disable --> enable
            //Render Setting Control
            //HuyNguyen
            SettingGroup sg = new SettingGroup();

            //Get setting controll Suspended
            sg.Add(GetSettingControlSuspended(isManageCustomerAccess, "suspended", isSuspended, (roleId > 1), CustParent.IsSuspended));

            #region Selected Betting
            if (roleId > 1 && Functions.Settings.IsVisibleSelectedBetting)
            {
                bool SelectedBetting = Convert.IsDBNull(row1["SelectedBetting"]) ? false : Convert.ToBoolean(row1["SelectedBetting"]);
                //bool SelectedBettingDL = Convert.IsDBNull(row1["DownlinesSelectedBetting"]) ? false : Convert.ToBoolean(row1["DownlinesSelectedBetting"]);
                sg.Add(GetSettingControlSelectedBetting(roleId, CustParent.SelectedBetting, isManageCustomerAccess, "SelectedBetting", SelectedBetting, (roleId > 1)));
            }
            #endregion

            #region Auto Suspend
            if (Functions.Settings.IsVisibleAutoSuspend)
                sg.Add(GetSettingControlAutoSuspend(roleId, isManageCustomerAccess, "AutoSuspend", !autosuspend, (roleId > 1), UplineAutoSuspend, CastAide.AsBoolean(row1["Enabled"])));
            #endregion

            #region is Internal user
            if (!WebSettings.DisableInternal && Functions.Settings.IsVisibleInternal)
            {
                bool isInternalUser = Convert.IsDBNull(row1["IsInternal"]) ? false : Convert.ToBoolean(row1["IsInternal"]);
                sg.Add(GetSettingControlInternalUser(roleId, CustParent.IsInternalUser, isManageCustomerAccess, "internalUser", isInternalUser, CastAide.AsBoolean(row1["Enabled"])));
            }
            #endregion

            #region Auto Master
            if (roleId == 3)
            {
                try
                {
                    bool enableAutoMPT = Convert.IsDBNull(row3["DisableAutoMPT"]) ? false : !Convert.ToBoolean(row3["DisableAutoMPT"]);
                    sg.Add(GetSettingControlEnableAutoMaster(roleId, isManageCustomerAccess, "Allow Auto PT", !enableAutoMPT, CastAide.AsBoolean(row1["Enabled"])));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            #endregion

            #region Outright
            bool disabledOutright = Convert.IsDBNull(row1["DisableOutright"]) ? false : Convert.ToBoolean(row1["DisableOutright"]);
            sg.Add(GetSettingControlOutright(roleId, CustParent.IsDisableOutright, isManageCustomerAccess, "disabledOutright", !disabledOutright, (roleId > 1), CastAide.AsBoolean(row1["Enabled"])));
            #endregion

            #region HR
            ////////HR
            //if (Permissions.AllowSaTote(row1["currency"].ToString()))
            //{
            //    bool disabledHR = Convert.IsDBNull(row1["DisableHorseracing"]) ? false : Convert.ToBoolean(row1["DisableHorseracing"]);
            //    bool disabledHRDL = Convert.IsDBNull(row1["DisableDownlinesHR"]) ? false : Convert.ToBoolean(row1["DisableDownlinesHR"]);
            //    sg.Add(GetSettingControlHR(roleId, CustParent.IsDisabledHR, isManageCustomerAccess, "disabledHR", row1["currency"].ToString(), !disabledHR, (roleId > 1), !disabledHRDL, CastAide.AsBoolean(row1["Enabled"])));

            //    TemplateHandler.Assign(ref htmlcontent, "{chkSATote}", disabledHR ? 1 : 0);
            //}
            //else
            //{
            //    TemplateHandler.Assign(ref htmlcontent, "{chkSATote}", 1);
            //}

            //////HR OpenTote (UKTote)
            //bool disabledHROpenTote = Convert.IsDBNull(row1["DisableOpenTote"]) ? false : Convert.ToBoolean(row1["DisableOpenTote"]);
            //bool disabledHROpenToteDL = Convert.IsDBNull(row1["DisableDownlinesOpenTote"]) ? false : Convert.ToBoolean(row1["DisableDownlinesOpenTote"]);
            //sg.Add(GetSettingControlHROpenTote(roleId, CustParent.IsDisabledOpenTote, isManageCustomerAccess, "disabledHROpenTote", row1["currency"].ToString(), !disabledHROpenTote, (roleId > 1), !disabledHROpenToteDL, CastAide.AsBoolean(row1["Enabled"])));
            //TemplateHandler.Assign(ref htmlcontent, "{chkUKTote}", disabledHROpenTote ? 1 : 0);
            #endregion

            #region FI
            if (!AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
            {
                bool disabledFinancial = Convert.IsDBNull(row1["DisableFinancial"]) ? false : Convert.ToBoolean(row1["DisableFinancial"]);
                bool disabledFIDL = Convert.IsDBNull(row1["DisableDownlinesFI"]) ? false : Convert.ToBoolean(row1["DisableDownlinesFI"]);
                sg.Add(GetSettingControlFI(roleId, CustParent.IsDisabledFI, isManageCustomerAccess, "disabledFinancial", !disabledFinancial, (roleId > 1), !disabledFIDL, CastAide.AsBoolean(row1["Enabled"])));
                TemplateHandler.Assign(ref htmlcontent, "{chkFinance}", disabledFinancial ? 1 : 0);
            }
            #endregion

            #region Enable Casino
            bool disabledCasino = Convert.IsDBNull(row1["disablecasino"]) ? false : Convert.ToBoolean(row1["disablecasino"]);
            bool syncCasino = Convert.IsDBNull(row1["synccasino"]) ? false : Convert.ToBoolean(row1["synccasino"]);
            bool disabledCSDL = Convert.IsDBNull(row1["DisableDownlinesCS"]) ? false : Convert.ToBoolean(row1["DisableDownlinesCS"]);
            sg.Add(GetSettingControlCasino(roleId, parentID, isManageCustomerAccess, "disabledCS", !disabledCasino, (roleId > 1), syncCasino, isAllowEdit, CastAide.AsBoolean(row1["Enabled"].ToString()), disabledCSParent, !disabledCSDL));
            TemplateHandler.Assign(ref htmlcontent, "{chkCasino}", disabledCasino ? 1 : 0);
            #endregion

            #region Bingo Game
            bool disabledBingo = Convert.IsDBNull(row1["DisableBingo"]) ? false : Convert.ToBoolean(row1["DisableBingo"]);
            bool syncBingo = Convert.IsDBNull(row1["SyncBingo"]) ? false : Convert.ToBoolean(row1["SyncBingo"]);
            bool disabledBIDL = Convert.IsDBNull(row1["DisableDownlinesBI"]) ? false : Convert.ToBoolean(row1["DisableDownlinesBI"]);
            sg.Add(GetSettingControlBingoGame(roleId, parentID, isManageCustomerAccess, "disabledBingoGame",
                                              !disabledBingo, (roleId > 1), syncBingo, isAllowEditBI,
                                              CastAide.AsBoolean(row1["Enabled"].ToString()), disabledBIParent,
                                              !disabledBIDL));
            TemplateHandler.Assign(ref htmlcontent, "{chkBingo}", disabledBingo ? 1 : 0);
            #endregion

            #region Number Game
            bool disabledNumberGame = Convert.IsDBNull(row1["DisableNumber"]) ? false : Convert.ToBoolean(row1["DisableNumber"]);
            bool disabledNumberGameDL = Convert.IsDBNull(row1["DisableDownlinesNumber"]) ? false : Convert.ToBoolean(row1["DisableDownlinesNumber"]);
            sg.Add(GetSettingControlNumberGame(roleId, CustParent.IsDisabledNumberGame, isManageCustomerAccess,
                                               "disabledNumberGame", !disabledNumberGame, (roleId > 1),
                                               !disabledNumberGameDL, CastAide.AsBoolean(row1["Enabled"])));
            TemplateHandler.Assign(ref htmlcontent, "{chkNumberGame}", disabledNumberGame ? 1 : 0);
            #endregion

            #region live casino
            bool disabledLiveCasino = WebSettings.EnableLiveCasino ? Convert.IsDBNull(row1["disableLiveCasino"]) ? false : Convert.ToBoolean(row1["disableLiveCasino"]) : true;
            bool syncLiveCasino = WebSettings.EnableLiveCasino ? (Convert.IsDBNull(row1["synclivecasino"]) ? false : Convert.ToBoolean(row1["synclivecasino"])) : false;
            bool disabledLCSDL = WebSettings.EnableLiveCasino
                                     ? Convert.IsDBNull(row1["DisableDownlinesLiveCS"])
                                           ? false
                                           : Convert.ToBoolean(row1["DisableDownlinesLiveCS"])
                                     : true;
            sg.Add(GetSettingControlLiveCasino(roleId, isManageCustomerAccess, "disabledLCS", !disabledLiveCasino,
                                               (roleId > 1), syncLiveCasino, isAllowEditLCS,
                                               CastAide.AsBoolean(row1["Enabled"].ToString()), disableLiveCasinoParent,
                                               !disabledLCSDL));
            TemplateHandler.Assign(ref htmlcontent, "{chkLiveCasino}", disabledLiveCasino ? 1 : 0);
            #endregion

            #region Virtual Sports
            bool disabledVirtualSports = WebSettings.EnableVirtualSports ? Convert.IsDBNull(row1["disablevirtualsports"]) ? false : Convert.ToBoolean(row1["disablevirtualsports"]) : true;
            bool disabledVSDownline = WebSettings.EnableVirtualSports
                                     ? Convert.IsDBNull(row1["disabledownlinesvirtualsports"])
                                           ? false
                                           : Convert.ToBoolean(row1["disabledownlinesvirtualsports"])
                                     : true;

            bool hideVirtualSports = WebSettings.EnableVirtualSports ? Convert.ToBoolean(row1["hidevirtualsports"]) : false;
            if (!hideVirtualSports)
            {
                sg.Add(GetSettingControlVirtualSports(roleId, isManageCustomerAccess, "disabledVS", !disabledVirtualSports,
                                                   (roleId > 1), disabledVirtualSports,
                                                   CastAide.AsBoolean(row1["Enabled"].ToString()), disabledVSParent,
                                                   !disabledVSDownline));
            }
            TemplateHandler.Assign(ref htmlcontent, "{chkVirtualSport}", disabledVirtualSports ? 1 : 0);
            #endregion

            #region DoubleComm
            if (WebSettings.OfferDoubleComm)
            {
                bool IsEnableDoubleCommLosing = Convert.IsDBNull(row1["DoubleCommLosing"]) ? false : Convert.ToBoolean(row1["DoubleCommLosing"]);
                sg.Add(GetSettingControlDoubleComm(roleId, CustParent.IsEnableDoubleCommLosing, isManageCustomerAccess, "doubleCommLosing", IsEnableDoubleCommLosing, (roleId > 1), CastAide.AsBoolean(row1["Enabled"])));
            }
            #endregion

            TemplateHandler.Assign(ref htmlcontent, "{settingcontrol}", sg.GetHTMLSettingControl());
        }

        private void GenMenuMinMaxPT(ref string htmlcontent, DataRow row2, int custId, int roleId, string currency, string userName, bool isAllowEditBI, bool isDeposit)
        {
            int parentId;
            string submenu1 = "";
            string submenu2 = "";
            string submenu3 = "";
            string minMaxPtUrl = "";
            var minMaxPtRenovationUrl = string.Empty;
            string masterPtUrl = "";
            switch (roleId)
            {
                case 1: // member
                    parentId = CastAide.AsInt32(row2["agentId"]);
                    var superId = CastAide.AsInt32(row2["superId"]);
                    var masterId = CastAide.AsInt32(row2["masterId"]);
                    TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "inline");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? "" : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Super PT</span></a></li>");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", isDeposit ? "" : "<a href=\"javascript:void(0);\"><span>Master PT</span></a>");


                    minMaxPtUrl = "Customer/v2/EditSuperOnMember.aspx?custId=" + custId + "&agentId=" + row2["agentId"] + "&uname=" + userName;
                    masterPtUrl = "Customer/v2/EditMasterOnMember.aspx?custId=" + custId + "&agentId=" + row2["agentId"] + "&uname=" + userName;
                    if (WebSettings.IsShowCustomerSettingRenovation == 3)
                    {
                        minMaxPtUrl = string.Format("EditSuperOnMember/EditSuperOnMemberPage?agentId={0}&custId={1}&uname={2}", parentId, custId, userName);
                        masterPtUrl = string.Format("EditMasterOnMember/EditMasterOnMemberPage?agentId={0}&custId={1}&uname={2}", parentId, custId, userName);
                    }
                    //HuyNguyen Menu Member
                    submenu1 = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";
                    if (!WebSettings.disabledFI && !AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/v2/EditSuperOnMemberFI.aspx?uplineid=" + parentId + "&custid=" + custId.ToString() + "&roleid=" + roleId.ToString() + "\");' target='centerFr'>Finance</a>";
                    }
                    if (!WebSettings.disabledHR)
                    {
                        submenu1 += string.Format("<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"SuperEditMemberPTRacing/Index?custid={0}&superid={1}\");' target='centerFr'>Racing</a>", custId, superId);
                    }
                    if (BingoSettings.BingoGame.Enabled && isAllowEditBI)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/PT/Bingo/SuperOnMemberUpdate.aspx?custId=" + custId.ToString() + "&roleid=" + roleId.ToString() + "\");' target='centerFr'>Bingo</a>";
                    }
                    if (WebSettings.EnableLiveCasino)
                    {                        
                        if (AppSession.IsSyncLCSCurrentCust)
                        {
                            submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "LiveCasino/PTSetting/SuperEditOnMember.aspx?custId=" + custId.ToString() + "&superId=" + superId + "\");' target='centerFr'>Live Casino</a>";
                        }
                    }
                    if (WebSettings.EnableVirtualSports)
                    {
                        if (!AppSession.IsHideVirtualSportsCurrentCust)
                        {
                            submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "VirtualSportMemberPT/SuperEditMember?custId=" + custId.ToString() + "&superId=" + superId + "\");' target='centerFr'>Virtual Sports</a>";
                        }
                    }

                    submenu2 = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + masterPtUrl + "\");' target='centerFr'>Sport book</a>";
                    if (!WebSettings.disabledFI)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/v2/EditMasterOnMemberFI.aspx?uplineid=" + parentId + "&custid=" + custId.ToString() + "&roleid=" + roleId.ToString() + "\");' target='centerFr'>Finance</a>";
                    }
                    if (!WebSettings.disabledHR)
                    {
                        submenu2 += string.Format("<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"MasterEditMemberPTRacing/Index?custid={0}&masterid={1}\");' target='centerFr'>Racing</a>", custId, masterId);
                    }

                    if (BingoSettings.BingoGame.Enabled && isAllowEditBI)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/PT/Bingo/MasterOnMemberUpdate.aspx?custId=" + custId.ToString() + "&roleid=" + roleId.ToString() + "\");' target='centerFr'>Bingo</a>";
                    }
                    if (WebSettings.EnableLiveCasino)
                    {
                        if (AppSession.IsSyncLCSCurrentCust)
                        {
                            submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "LiveCasino/PTSetting/MasterEditOnMember.aspx?custId=" + custId.ToString() + "&masterId=" + masterId + "\");' target='centerFr'>Live Casino</a>";
                        }
                    }
                    if (WebSettings.EnableVirtualSports)
                    {
                        if (!AppSession.IsHideVirtualSportsCurrentCust)
                        {
                            submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "VirtualSportMemberPT/MasterEditMember?custId=" + custId.ToString() + "&masterId=" + masterId + "\");' target='centerFr'>Virtual Sports</a>";
                        }
                    }

                    TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", submenu1);
                    TemplateHandler.Assign(ref htmlcontent, "{contentminmax1}", submenu2);


                    break;
                case 2: // agent
                    parentId = Convert.ToInt32(row2["masterid"]);
                    TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "none");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? "" : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Min/Max PT</span></a></li>");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", "");
                    minMaxPtRenovationUrl = "EditAgentMinMaxPt/EditAgentMinMaxPtPage?&custId=" + custId + "&uplineId=" + parentId + "&uname=" + userName;
                    minMaxPtUrl = WebSettings.IsShowCustomerSettingRenovation == 3 ? minMaxPtRenovationUrl : "Customer/v2/EditMinMaxPT.aspx?set=1&custId=" + custId + "&type=Agent";
                    submenu3 = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";
                    if (!WebSettings.disabledFI && !AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
                    {
                        submenu3 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/v2/MinMaxFinancePT.aspx?uplineid=" + parentId + "&custid=" + custId.ToString() + "&roleid=" + roleId.ToString() + "\");' target='centerFr'>Finance</a>";
                    }
                    if (!WebSettings.disabledHR)
                    {
                        submenu3 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/pt/racing/AgentMinMaxUpdate.aspx?custId=" + custId.ToString() + "\");' target='centerFr'>Racing</a>";
                    }
                    TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", submenu3);
                    break;
                case 3: // master
                    parentId = Convert.ToInt32(row2["superId"]);
                    TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "none");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? "" : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Min/Max PT</span></a></li>");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", "");

                    minMaxPtRenovationUrl = "EditMasterMinMaxPt/EditMasterMinMaxPtPage?&custId=" + custId + "&uplineId=" + parentId + "&uname=" + userName;
                    minMaxPtUrl = WebSettings.IsShowCustomerSettingRenovation == 3 ? minMaxPtRenovationUrl : "Customer/v2/EditMinMaxPT.aspx?set=1&custId=" + custId + "&type=Master";

                    submenu3 = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";
                    if (!WebSettings.disabledFI && !AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
                    {
                        submenu3 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/v2/MinMaxFinancePT.aspx?uplineid=" + parentId + "&custid=" + custId.ToString() + "&roleid=" + roleId.ToString() + "\");' target='centerFr'>Finance</a>";
                    }
                    if (!WebSettings.disabledHR)
                    {
                        submenu3 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/pt/racing/MasterMinMaxUpdate.aspx?custId=" + custId.ToString() + "\");' target='centerFr'>Racing</a>";
                    }
                    TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", submenu3);
                    break;
                case 4: // super
                    parentId = 0;
                    TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "none");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? "" : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Min PT</span></a></li>");
                    TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", "");
                    minMaxPtRenovationUrl = "EditSuperMinPt/EditSuperMinPtPage?&custId=" + custId + "&uname=" + userName;
                    minMaxPtUrl = WebSettings.IsShowCustomerSettingRenovation == 3 ? minMaxPtRenovationUrl : "Customer/v2/EditMinMaxPT.aspx?set=1&custId=" + custId + "&type=Super";
                    submenu3 = "<a class='button_left02'  href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";
                    if (!WebSettings.disabledFI && !AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
                    {
                        submenu3 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/v2/MinMaxFinancePT.aspx?uplineid=" + parentId + "&custid=" + custId.ToString() + "&roleid=" + roleId.ToString() + "\");' target='centerFr'>Finance</a>";
                    }
                    if (!WebSettings.disabledHR)
                    {
                        submenu3 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/pt/racing/SuperMinUpdate.aspx?custId=" + custId.ToString() + "\");' target='centerFr'>Racing</a>";
                    }
                    TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", submenu3);
                    break;
                default:
                    break;
            }
        }

        private string StatusSpan(bool isManageCustomerAccess, string name, bool currStatus, bool hasDownLine)
        {
            try
            {
                string s = WebUtils.LoadTemplate("settingitem.htm");
                TemplateHandler.Assign(ref s, "{name}", name);
                TemplateHandler.Assign(ref s, "{onmouseout}", string.Empty);
                TemplateHandler.Assign(ref s, "{onmouseover}", string.Empty);
                //TemplateHandler.Assign(ref s, "{value}", (currStatus) ? "Yes" : "No");
                if (isManageCustomerAccess)
                {
                    TemplateHandler.Assign(ref s, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                    TemplateHandler.Assign(ref s, "{onclick}", "return changeStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");");
                }
                else
                {
                    TemplateHandler.Assign(ref s, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                    TemplateHandler.Assign(ref s, "{onclick}", string.Empty);
                    TemplateHandler.Assign(ref s, "{onmouseout}", "");
                    TemplateHandler.Assign(ref s, "{onmouseover}", "");
                }
                return s;
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }
        private string GetClosedStatus(bool isManageCustomerAccess, string name, bool currStatus, bool hasDownLine, bool uplineclosed, bool hasClearCreditPermission)
        {
            try
            {


                string s = WebUtils.LoadTemplate("SettingClosed.htm");
                TemplateHandler.Assign(ref s, "{name}", name);

                if (isManageCustomerAccess)
                {
                    if (!uplineclosed)
                    {
                        TemplateHandler.Assign(ref s, "{onmouseout}", string.Empty);
                        TemplateHandler.Assign(ref s, "{onmouseover}", string.Empty);
                        TemplateHandler.Assign(ref s, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                        TemplateHandler.Assign(ref s, "{onclick}", "return changeStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");");
                        //Check Clear Credit
                        if (!hasClearCreditPermission)
                        {
                            TemplateHandler.RemoveBlock(ref s, "ClearCredit");
                            TemplateHandler.Assign(ref s, "{isallowupdatecredit}", "0");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref s, "{btonclick}", "return updateCloseStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");");
                            TemplateHandler.Assign(ref s, "{UserName}", GetCustName(AppSession.CustRoleId));
                            TemplateHandler.Assign(ref s, "{isallowupdatecredit}", "1");
                        }

                    }
                    else
                    {
                        TemplateHandler.Assign(ref s, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                        TemplateHandler.Assign(ref s, "{onclick}", string.Empty);
                        TemplateHandler.Assign(ref s, "{onmouseout}", "UnTip()");
                        TemplateHandler.Assign(ref s, "{onmouseover}", "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUplineClosed") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)");
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref s, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                    TemplateHandler.Assign(ref s, "{onclick}", string.Empty);
                    TemplateHandler.Assign(ref s, "{onmouseout}", "");
                    TemplateHandler.Assign(ref s, "{onmouseover}", "");
                }
                return s;
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        private SettingControl GetSettingControlSelectedBetting(int roleID, bool parentstatus, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine)
        {

            string classname = "";
            string jsonclick = "";
            string jsonmouseover = "";
            string jsonmouseout = "";

            if (isManageCustomerAccess)
            {

                if (!parentstatus)
                {
                    classname = "canchange";
                    //jsonclick = "return changeStatus('" + statusName + "'," + ((hasDownLine) ? "true" : "false") + ");";
                    jsonclick = "return ShowDisablePop(this);";
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSelectedBettingUL") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }

            }

            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSelectedBetting"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, statusName);

        }

        //HuyNguyen


        private string GetSettingControlDangerous(int roleId, bool isManageCustomerAccess, string[] dangeroustype, int[] levels, bool isBA)
        {
            try
            {
                DangerousSetting danger = new DangerousSetting();
                return danger.GenDangerous(roleId, isManageCustomerAccess, dangeroustype, levels, isBA);
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }
        private SettingControl GetSettingControlSuspended(bool isManageCustomerAccess, string name, bool currStatus, bool hasDownLine, bool UpLineStatus)
        {
            string classname = "";
            string jsonclick = "";
            string jsonmouseover = "";
            string jsonmouseout = "";

            if (isManageCustomerAccess)
            {
                if (!UpLineStatus)
                {
                    classname = (currStatus) ? "chkchecked" : "chkuncheck";
                    jsonclick = "return changeStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");";

                }
                else
                {
                    classname = (currStatus) ? "chkchecked" : "chkuncheck";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuspendedUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }
            else
            {
                classname = (currStatus) ? "chkchecked" : "chkuncheck";
            }

            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuspended"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, name, 0, false, "");
        }

        private SettingControl GetSettingControlHR(int roleID, bool parentstatus, bool isManageCustomerAccess, string statusName, string currencyName, bool currStatus, bool hasDownLine, bool isdisableHRDL, bool isenabled)
        {
            if (WebSettings.disabledHR)
            {
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledHR"), false, true, "", "", "", "", statusName, roleID, isdisableHRDL, "HR");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleID || !parentstatus)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePop(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledHRUplineTT") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledHR"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, isdisableHRDL, "HR");
            }
        }

        private SettingControl GetSettingControlHROpenTote(int roleID, bool parentstatus, bool isManageCustomerAccess, string statusName, string currencyName, bool currStatus, bool hasDownLine, bool isdisableHRDL, bool isenabled)
        {
            if (WebSettings.disabledHR)
            {
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledHROpentote"), false, true, "", "", "", "", statusName, roleID, isdisableHRDL, "OpenTote");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleID || !parentstatus)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePop(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledHRUplineTTUK") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledHROpentote"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, isdisableHRDL, "OpenTote");
            }
        }

        private SettingControl GetSettingControlNumberGame(int roleID, bool parentstatus, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine, bool isdisableNumberGameDL, bool isenabled)
        {
            if (!BingoSettings.NumberGame.Enabled)
            {
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledNumberGame"), false, true, "", "", "", "", statusName, roleID, isdisableNumberGameDL, "NumberGame");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleID || !parentstatus)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePop(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledNumberGameUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledNumberGame"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, isdisableNumberGameDL, "NumberGame");
            }
        }

        private SettingControl GetSettingControlFI(int roleID, bool parentstatus, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine, bool isdisabledFIDL, bool isenabled)
        {
            if (WebSettings.disabledFI)
            {
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledFI"), false, true, "", "", "", "", statusName, roleID, isdisabledFIDL, "FI");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleID || !parentstatus)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePop(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledFIUplineTT") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }

                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledFI"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, isdisabledFIDL, "FI");
            }

        }

        private SettingControl GetSettingControlAutoSuspend(int roleID, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine, bool UplineAutoSuspend, bool isenabled)
        {

            string classname = "";
            string jsonclick = "";
            string jsonmouseover = "";
            string jsonmouseout = "";

            if (isManageCustomerAccess)
            {
                if (isenabled)
                {
                    if (4 == roleID || !UplineAutoSuspend)
                    {
                        classname = "canchange";
                        jsonclick = "return changeStatus('" + statusName + "'," + ((hasDownLine) ? "true" : "false") + ");";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('Upline has been auto-suspended.', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }

            return new SettingControl(statusName, !currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "AS");
        }

        private SettingControl GetSettingControlCasino(int roleID, int parentID, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine, bool syncCasino, bool isAllowEdit, bool isenabled, bool disabledCSParent, bool disableDownLine)
        {

            if (WebSettings.disableCS || !isAllowEdit || !syncCasino)
            {
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledCasino"), false, true, "", "", "", "", statusName, roleID, false, "CS");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleID || !disabledCSParent)
                        {
                            classname = "canchange";
                            //jsonclick = "return changeStatus('" + statusName + "'," + ((hasDownLine) ? "true" : "false") + ");";
                            jsonclick = "return ShowDisablePopCS(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledCSUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }

                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledCasino"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, disableDownLine, "CS");
            }
        }

        private SettingControl GetSettingControlBingoGame(int roleID, int parentID, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine, bool syncBingo, bool isAllowEdit, bool isenabled, bool disabledBIParent, bool disableDownLine)
        {
            if (!BingoSettings.BingoGame.Enabled || !isAllowEdit || !syncBingo)
            {
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledBingoGame"), false, true, "", "", "", "", statusName, roleID, false, "BI");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleID || !disabledBIParent)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePopCS(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledBingoGameUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }

                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledBingoGame"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, disableDownLine, "BI");
            }
        }

        private SettingControl GetSettingControlOutright(int roleID, bool parentstatus, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine, bool isenalbed)
        {

            string classname = "";
            string jsonclick = "";
            string jsonmouseover = "";
            string jsonmouseout = "";
            if (isManageCustomerAccess)
            {
                if (isenalbed)
                {
                    if (4 == roleID || !parentstatus)
                    {
                        classname = "canchange";
                        jsonclick = "return changeStatus('" + "Outright" + "'," + ((hasDownLine) ? "true" : "false") + ");";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledORUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }
            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAllowOutright"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "OR");

        }

        private SettingControl GetSettingControlDoubleComm(int roleID, bool parentstatus, bool isManageCustomerAccess, string statusName, bool currStatus, bool hasDownLine, bool isenalbed)
        {

            string classname = "";
            string jsonclick = "";
            string jsonmouseover = "";
            string jsonmouseout = "";
            if (isManageCustomerAccess)
            {
                if (isenalbed)
                {
                    if (4 == roleID || parentstatus)
                    {
                        classname = "canchange";
                        jsonclick = "return updateDoubleCommLosing(" + ((hasDownLine) ? "true" : "false") + ");";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledDoubleCommUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }
            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAllowDoubleComm"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "DoubleComm");

        }

        private SettingControl GetSettingControlEnableAutoMaster(int roleID, bool isManageCustomerAccess, string statusName, bool currStatus, bool isenabled)
        {
            if (!Permissions.IsEnabledAutoMaster(true) || roleID != 3)
            {
                return new SettingControl(statusName, false, true, "", "", "", "", statusName, roleID, false, "AM");
            }
            else
            {

                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        classname = "canchange";
                        jsonclick = "return changeStatus('autompt',false)";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }

                return new SettingControl(statusName, !currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "AM");
            }
        }

        private SettingControl GetSettingControlInternalUser(int roleID, bool parentIsInternal, bool isManageCustomerAccess, string statusName, bool isInternalUser, bool isenabled)
        {
            if (WebSettings.DisableInternal)
            {
                return new SettingControl("Internal", false, true, "", "", "", "", statusName, roleID, false, "INTERNALUSER");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";
                if (isManageCustomerAccess)
                {

                    if (roleID == 1)
                    {
                        classname = "cannotchangeinternal";
                        jsonmouseout = "";//"UnTip()";
                        jsonmouseover = "";//"Tip('" + Resources.Language.lblCannotChangeMemberInternal + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                    else
                    {
                        if (4 == roleID || !parentIsInternal)
                        {
                            classname = "canchange";
                            jsonclick = "return changeStatus('" + "Internal" + "', true);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUplineIsInternalUser") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }

                }
                //string userType = isInternalUser ? Resources.Language.userInternal : Resources.Language.userBetting;
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "userInternal"), isInternalUser, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "INTERNALUSER");
            }
        }

        private SettingControl GetSettingControlLiveCasino(int roleId, bool isManageCustomerAccess, string statusName, bool currStatus,
            bool hasDownLine, bool syncLiveCasino, bool isAllowEdit, bool isenabled, bool disabledUpline, bool disableDownLine)
        {

            if (!WebSettings.EnableLiveCasino || !isAllowEdit || !syncLiveCasino)
            {
                return
                    new SettingControl(
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "disableLiveCasino"),
                        false, true, "", "", "", "", statusName, roleId, disableDownLine, "LCS");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleId || !disabledUpline)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePop(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledLiveCasinoUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                return
                    new SettingControl(
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "enableLiveCasino"),
                        currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleId,
                        disableDownLine, "LCS");

            }
        }

        private SettingControl GetSettingControlVirtualSports(int roleId, bool isManageCustomerAccess, string statusName, bool currStatus,
            bool hasDownLine, bool hasDownline, bool isenabled, bool disabledUpline, bool disableDownLine)
        {

            if (!WebSettings.EnableVirtualSports)
            {
                return
                    new SettingControl(
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "disableVirtualSports"),
                        false, true, "", "", "", "", statusName, roleId, disableDownLine, "VS");
            }
            else
            {
                string classname = "";
                string jsonclick = "";
                string jsonmouseover = "";
                string jsonmouseout = "";

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleId || !disabledUpline)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePop(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledVSUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                return
                    new SettingControl(
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "virtualsports"),
                        currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleId,
                        disableDownLine, "VS");

            }
        }
        #endregion

        private void GenUplineBox(ref string htmlcontent, int roleId, DataRow row)
        {
            try
            {
                string uplineboxhtml = "";
                uplineboxhtml = WebUtils.LoadTemplate("CustomerInfo/UplineBox.htm");
                if (roleId <= UserInfo.ROLE_SUPER)
                {
                    TemplateHandler.Assign(ref uplineboxhtml, "{SuperId}", row["superID"].ToString());
                    TemplateHandler.Assign(ref uplineboxhtml, "{SuperName}", row["superName"].ToString());
                    TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden1}", "");
                    if (CastAide.AsInt16(row["SuperEnabled"]) == 0)
                    {
                        TemplateHandler.RemoveBlock(ref uplineboxhtml, "SuperLogin");
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden1}", "hidden");
                }
                if (roleId <= UserInfo.ROLE_MASTER)
                {
                    TemplateHandler.Assign(ref uplineboxhtml, "{MasterId}", row["masterID"].ToString());
                    TemplateHandler.Assign(ref uplineboxhtml, "{MasterName}", row["masterName"].ToString());
                    TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden2}", "");
                    if (CastAide.AsInt16(row["MasterEnabled"]) == 0)
                    {
                        TemplateHandler.RemoveBlock(ref uplineboxhtml, "MasterLogin");
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden2}", "hidden");
                }
                if (roleId <= UserInfo.ROLE_AGENT)
                {
                    TemplateHandler.Assign(ref uplineboxhtml, "{AgentId}", row["agentID"].ToString());
                    TemplateHandler.Assign(ref uplineboxhtml, "{AgentName}", row["agentName"].ToString());
                    TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden3}", "");
                    if (CastAide.AsInt16(row["AgentEnabled"]) == 0)
                    {
                        TemplateHandler.RemoveBlock(ref uplineboxhtml, "AgentLogin");
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden3}", "hidden");
                }

                TemplateHandler.Assign(ref htmlcontent, "{UplineBox}", uplineboxhtml);
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

        }

        //Login Detail
        private void GenLoginDetail(ref string htmlCustInfo, int custId, int roleId, DataRow loginData, DataRow transData, string username)
        {
            try
            {
                string tplLoginDetail = WebUtils.LoadTemplate("CustomerInfo/LoginDetail.htm");
                string lastLoginDate = Convert.IsDBNull(loginData["lastlogged"]) ? "" : Convert.ToDateTime(loginData["lastlogged"]).ToShortDateString();
                string lastLoginTime = Convert.IsDBNull(loginData["lastlogged"]) ? "" : " " + Convert.ToDateTime(loginData["lastlogged"]).ToShortTimeString();
                string lastIp = Convert.IsDBNull(loginData["lastloginip"]) ? "" : loginData["lastloginip"].ToString();
                string customerSite = (roleId == (int)Globals.AccountType.Member) ? CastAide.AsString(loginData["membersite"], "") : CastAide.AsString(loginData["site"], "");
                string lastChangeSecurityCodeDate =
                    Convert.IsDBNull(loginData["LastChangedSecurityCodeDate"]) ?
                    string.Empty :
                    Convert.ToString(loginData["LastChangedSecurityCodeDate"]);

                var site = new CustSettingMoveSite();
                customerSite = site.GetSiteNameByCustId(custId, roleId, customerSite, username);

                string createdate = Convert.IsDBNull(loginData["createdate"]) ? "" : Convert.ToDateTime(loginData["createdate"]).ToShortDateString();
                string createtime = Convert.IsDBNull(loginData["createdate"]) ? "" : Convert.ToDateTime(loginData["createdate"]).ToShortTimeString();
                //--------
                string pwdExpiryDate = Convert.IsDBNull(loginData["pwdexpiry"]) ? "" : Convert.ToDateTime(loginData["pwdexpiry"]).ToShortDateString();
                string pwdExpiryTime = Convert.IsDBNull(loginData["pwdexpiry"]) ? "" : " " + Convert.ToDateTime(loginData["pwdexpiry"]).ToShortTimeString();
                string lastTxnDate = string.Empty;
                string lastTxnTime = string.Empty;
                if (transData != null && roleId <= 4)
                {
                    lastTxnDate = Convert.IsDBNull(transData["lastTxn"]) ? "" : Convert.ToDateTime(transData["lastTxn"]).ToShortDateString();
                    lastTxnTime = Convert.IsDBNull(transData["lastTxn"]) ? "" : " " + Convert.ToDateTime(transData["lastTxn"]).ToShortTimeString();
                }

                TemplateHandler.Assign(ref tplLoginDetail, "{createdate}", createdate + " " + createtime);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastLogin}", lastLoginDate + lastLoginTime);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastTxn}", lastTxnDate + lastTxnTime + "&nbsp;");
                TemplateHandler.Assign(ref tplLoginDetail, "{PasswordExpiry}", pwdExpiryDate + pwdExpiryTime);
                TemplateHandler.Assign(ref tplLoginDetail, "{customersite}", customerSite);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastLoginIP}", "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", IpManagement.ReplaceVNIP(lastIp)) + "\" target=\"_blank\" style='font-weight:bold'>" + IpManagement.ReplaceVNIP(lastIp) + "</a>");
                TemplateHandler.Assign(ref tplLoginDetail, "{custid}", custId);
                TemplateHandler.Assign(ref tplLoginDetail, "{last_change_security_code_date}", lastChangeSecurityCodeDate);
                TemplateHandler.Assign(ref htmlCustInfo, "{logindetail}", tplLoginDetail);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        private void GenTransLinskBox(ref string htmlCustInfo, int custId, int roleid, bool allowHR, bool isInternal, string custName, bool isDeposit, bool doubleComm)
        {
            try
            {
                string tplTransLink = WebUtils.LoadTemplateBySystem("CustomerInfo/TransLinksBox.htm");
                string downlineListURL = string.Empty;
                string lblDownlineList = string.Empty;
                Globals.AccountType accLevel = (Globals.AccountType)roleid;

                // Edit by David 31-07-2012
                string statementLink = string.Empty;
                if (!isInternal)
                {
                    statementLink = "Statement/CustomerStatement.aspx?custname={custname}";
                }
                else
                {
                    statementLink = "CustomerStatement/Index?CustID=" + custId + "&CustName={custname}";
                }
                TemplateHandler.Assign(ref tplTransLink, "statementLink", statementLink);

                bool enableDeposit = WebSettings.IsApplySplitingDepositeCustomer;
                if (enableDeposit && isDeposit && accLevel != Globals.AccountType.Member)
                {
                    TemplateHandler.RemoveBlock(ref tplTransLink, "downlinelistbtn");
                }

                switch (accLevel)
                {
                    case Globals.AccountType.Super:
                        if (!isInternal)
                        {

                            if (WebSettings.OfferDoubleComm)
                            {
                                downlineListURL = "CustomerList/MasterAgentList?superId=" + custId + "&doubleCommSuper=" + doubleComm.ToString().ToLower();
                            }
                            else
                            {
                                downlineListURL = "Customer/MasterAgentList.aspx?superId=" + custId;
                            }
                        }
                        else
                        {
                            downlineListURL = "FinanceCustomerList/Index?upline=" + custId + "&roleid=4";
                        }
                        lblDownlineList = "Master List";
                        TemplateHandler.Assign(ref tplTransLink, "{AgentShow}", "hidden");
                        break;
                    case Globals.AccountType.Master:
                        if (!isInternal)
                        {

                            if (WebSettings.OfferDoubleComm)
                            {
                                downlineListURL = "CustomerList/AgentList?masterId=" + custId + "&doubleCommMaster=" + doubleComm.ToString().ToLower();
                            }
                            else
                            {
                                downlineListURL = "Customer/AgentList.aspx?masterId=" + custId;
                            }
                        }
                        else
                        {
                            downlineListURL = "FinanceCustomerList/Index?upline=" + custId + "&roleid=3";
                        }
                        lblDownlineList = "Agent List";
                        TemplateHandler.Assign(ref tplTransLink, "{AgentShow}", "hidden");
                        break;
                    case Globals.AccountType.Agent:
                        if (!isInternal)
                        {
                            if (WebSettings.OfferDoubleComm)
                            {
                                downlineListURL = "CustomerList/MemberList?agentId=" + custId + "&hr=" + allowHR.ToString().ToLower() + "&internal=" + isInternal.ToString().ToLower() + "&doubleCommAgent=" + doubleComm.ToString().ToLower();
                            }
                            else
                            {
                                downlineListURL = "Customer/MemberList.aspx?agentId=" + custId + "&hr=" + allowHR.ToString().ToLower() + "&internal=" + isInternal.ToString().ToLower();
                            }
                        }
                        else
                        {
                            downlineListURL = "FinanceCustomerList/Index?upline=" + custId + "&roleid=2";
                        }
                        lblDownlineList = "Member List";
                        TemplateHandler.Assign(ref tplTransLink, "{AgentShow}", "");
                        break;
                    case Globals.AccountType.Member:
                        downlineListURL = "TarusBetList/BetListByCustInfo?CustID=" + custId;
                        lblDownlineList = "Bet List";
                        TemplateHandler.Assign(ref tplTransLink, "{AgentShow}", "hidden");
                        break;
                }


                TemplateHandler.Assign(ref tplTransLink, "{IsInternal}", isInternal.ToString().ToLower());

                custName = System.Web.HttpUtility.UrlEncode(custName);

                TemplateHandler.Assign(ref tplTransLink, "{custname}", custName);
                TemplateHandler.Assign(ref tplTransLink, "{custid}", custId.ToString());
                TemplateHandler.Assign(ref tplTransLink, "downlineLink", downlineListURL);
                TemplateHandler.Assign(ref tplTransLink, "lblDownline", lblDownlineList);

                TemplateHandler.Assign(ref htmlCustInfo, "{TransLinksBox}", tplTransLink);

                TemplateHandler.Assign(ref htmlCustInfo, "{fromCustName}", AppSession.CustUName);
                TemplateHandler.Assign(ref htmlCustInfo, "{fromCustID}", AppSession.CustID);
                TemplateHandler.Assign(ref htmlCustInfo, "{CustName}", AppSession.CustUName);
                TemplateHandler.Assign(ref htmlCustInfo, "{CustID}", AppSession.CustID);
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }



        private void AssignEditFinance(ref string tpl, int roleId)
        {
            try
            {
                switch (roleId)
                {
                    case 4:
                        if (WebSettings.disabledFI)
                            TemplateHandler.RemoveBlock(ref tpl, "edit-finance");
                        break;
                    case 3:
                    case 2:
                    case 1:
                        if (WebSettings.disabledFI)
                            TemplateHandler.RemoveBlock(ref tpl, "edit-finance");
                        break;
                }
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
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
                    return "";
            }
        }
        #endregion


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

        private static void GetDownlineCountingInfo(ref string balanceInfo, DbResult ds, int roleId)
        {
            int mAgentActive = 0, mAgentClosed = 0, mAgentSuspended = 0;
            int agentActive = 0, agentClosed = 0, agentSuspend = 0;
            int memberActive = 0, memberClosed = 0, memberSuspend = 0;

            if (ds.ResultSet.Tables[3] != null && ds.ResultSet.Tables[3].Rows != null)
            {
                var dtDownlineActiveInfo = ds.ResultSet.Tables[3];

                mAgentActive = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["MAgentActive"]);
                mAgentClosed = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["MAgentClosed"]);
                mAgentSuspended = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["MAgentSuspended"]);

                agentActive = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["AgentActive"]);
                agentClosed = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["AgentClosed"]);
                agentSuspend = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["AgentSuspended"]);

                memberActive = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["MemberActive"]);
                memberClosed = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["MemberClosed"]);
                memberSuspend = CastAide.AsInt32(dtDownlineActiveInfo.Rows[0]["MemberSuspended"]);
            }

            #region assign template

            TemplateHandler.Assign(ref balanceInfo, "{masteractive}",
                                    string.Format("<span style='color:blue'>{0}</span>/<span style='color:red'>{1}</span>/{2}",
                                                    mAgentActive, mAgentClosed, mAgentSuspended));

            TemplateHandler.Assign(ref balanceInfo, "{agentavtive}",
                                    string.Format("<span style='color:blue'>{0}</span>/<span style='color:red'>{1}</span>/{2}",
                                                    agentActive, agentClosed, agentSuspend));

            TemplateHandler.Assign(ref balanceInfo, "{memberactive}",
                                    string.Format("<span style='color:blue'>{0}</span>/<span style='color:red'>{1}</span>/{2}",
                                                    memberActive, memberClosed, memberSuspend));

            TemplateHandler.Assign(ref balanceInfo, "{masterhidden}", roleId > 3 ? "" : "hidden");
            TemplateHandler.Assign(ref balanceInfo, "{agenthidden}", roleId > 2 ? "" : "hidden");
            TemplateHandler.Assign(ref balanceInfo, "{memberhidden}", roleId > 1 ? "" : "hidden");
            #endregion
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

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }

            return result;

        }

        public Sunplus.Accounting.Entities.Customer.CustomerExRate SelectCustomerExRate(string username)
        {
            return Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.SelectCustomerExRate(username);
        }

        public Result CustomerCrossTransferInsert(int fromCustID, int toCustID, double fromAmount, double toAmount, double fromExRate, double toExRate, string forexUsername, string remark, int custid)
        {
            bool IsHadPermissionOnCrossCurrencyTransfer = Sunplus.Accounting.Business.Security.Permissions.GetHostPermission("FinanceTransfer");
            if (IsHadPermissionOnCrossCurrencyTransfer == false)
            {
                return new Result { ErrorCode = -1, ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500") };
            }
            return Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.CustomerCrossTransferInsert(fromCustID, toCustID, fromAmount, toAmount, fromExRate, toExRate, forexUsername, remark, custid);
        }

        public List<Sunplus.Accounting.Entities.Customer.ForexAccount> SelectAllForexAccount()
        {
            return Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.SelectAllForexAccount();
        }

        #region Enable Number game for Downline


        public BetSettingNG CheckEnableNGDownline(string username)
        {
            var betsetting = new BetSettingNG();
            betsetting = _customerDAO.CheckEnableNGDownline(username);
            return betsetting;

        }

        public Result UpdateBetSettingNGDownline(string username, int isEnable, string minbet, string maxbet, string maxMatch, string MaxBall)
        {
            var result = new Result();
            result = _customerDAO.UpdateBetSettingNGDownline(username, isEnable, minbet, maxbet, maxMatch, MaxBall);
            return result;
        }

        #endregion


        public Result AutoInsertThridPartyAccount(CSCustomerDetail custInfo, CustDetail uplineInfo, int userId, string userName, string ip)
        {
            var result = new Result();
            if (uplineInfo.IsUplineSyncCasino)
            {
                //System.Threading.Thread.Sleep(2000);
                var csService = new PT.Casino.CasinoManager();
                //custInfo.SiteName = new Sunplus.Accounting.DataAccess.AdoNet.CustomerDAO().GetRNGSiteNameByCustId(custInfo.IBCCustID);
                csService.AutoInsertCasinoAccount(custInfo, userId, userName, ip);
            }
            if (uplineInfo.IsUplineSyncBingo)
            {
                var biService = new PT.Bingo.BingoManager();
                biService.AutoInsertBingoAccount(custInfo, userId, userName, ip, WebSettings.IsAllowMoveToBingo);
            }
            if (uplineInfo.IsUplineSyncLiveCasino && WebSettings.EnableLiveCasino)
            {
                var liveCsService = new LiveCasinoDistributor();
                var thirdPartyCustInfo = new CustomerInfoDTO()
                                             {
                                                 AgentId = custInfo.AIBCCustID,
                                                 CustId = custInfo.IBCCustID,
                                                 CustName = custInfo.UserName,
                                                 DisableLiveCs = uplineInfo.IsUplineDisableLiveCasino,
                                                 ExchangeId = custInfo.ExchangeID,
                                                 Ip = ip,
                                                 IsEnableDownLines = !uplineInfo.IsUplineDisableLiveCasino,
                                                 Language = "E",
                                                 MasterId = custInfo.MIBCCustID,
                                                 RoleId = custInfo.RoleId,
                                                 SubAccountId = 0,
                                                 SuperId = custInfo.SIBCCustID
                                             };
                liveCsService.AutoInsertPtSetting(thirdPartyCustInfo, userId, userName, 3);
            }
            return result;
        }


        public string GetRNGSiteNameByCustId(int custId)
        {
            try
            {
                return _customerDAO.GetRNGSiteNameByCustId(custId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }
    }

}
