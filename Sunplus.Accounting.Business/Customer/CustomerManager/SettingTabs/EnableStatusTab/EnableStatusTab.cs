namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Configuration;
    using System.Data;
    using ConfigureDirectAccount;
    using Core.Common;
    using Core.Configurations;
    using Core.Extensions;
    using Core.Web;
    using Criteria;
    using DataAccess;
    using DNAide.Core;
    using DNAide.Web;
    using Entities;
    using RemotingRef;
    using Security;

    public partial class CustomerManager
    {
        private RenderLeftPanelParameter RenderSettingMenu(
            ref string htmlcontent,
            DataRow row1,
            DataTable dynamicSettingtable,
            CustomerInfoViewModel customerInfoViewModel)
        {
            var renderLeftPanelParameter = new RenderLeftPanelParameter
            {
                IsShowKeno = customerInfoViewModel.ShowEnableCheckBoxKeno,
                IsShowGoldDeluxeForCredit =
                    WebSettings.EnableGoldDeluxe &&
                    customerInfoViewModel.SyncUplineGoldDeluxe
                    && !customerInfoViewModel.IsDeposit
                    && !string.Equals(customerInfoViewModel.Currency, "Won", StringComparison.OrdinalIgnoreCase),
                IsShowMacau = customerInfoViewModel.ShowMacauDeposit,
                IsShowEnableBetTrade = customerInfoViewModel.ShowEnableBetTradeDeposit,
                IsShowEnableCashOut = customerInfoViewModel.ShowEnableCashOutDeposit,
                IsShowEnableMicrogaming = customerInfoViewModel.ShowEnableMicrogamingDeposit,
                IsShowAutoSuspend = customerInfoViewModel.HasReadPermission_AutoSuspend,
                IsShowColossusBetsForCredit = customerInfoViewModel.ShowEnabledCheckboxColossusBets,
                IsShowAllBetForCredit = !customerInfoViewModel.HideEnabledCheckBoxAllbet,
                IsShowVoidbridge = !customerInfoViewModel.HideEnabledCheckBoxVoidbridge,
                IsShowFishingWorld = !customerInfoViewModel.HideEnableAndEditFishingWorld,
                IsHideCricket = customerInfoViewModel.HideCricketCredit,
                IsShowVirtualSport = !customerInfoViewModel.HideCheckBoxVirtualSports,
                IsShowVirtualSport2 = !customerInfoViewModel.HideVirtualSports2,
                ParentId = customerInfoViewModel.ParentId,
                RoleId = customerInfoViewModel.RoleId,
                CustId = customerInfoViewModel.CustId,
                IsDeposit = customerInfoViewModel.IsDeposit,
                Username = customerInfoViewModel.UserName,
                AccId = AppSession.AccId,
                IsShowNG2 = !customerInfoViewModel.HideEnabledCheckBoxNG2Credit,
                IsShowKenoLottery = !customerInfoViewModel.HideKenoLottery,
                IsShowLucky3 = !customerInfoViewModel.HideEnabledCheckBoxLucky3Credit,
                IsShowBullfighting = !customerInfoViewModel.HideSettingBullfighting
            };

            htmlcontent = RenderClosedStatus(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderLimitedPerIP(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderGoldDeluxeForDepositControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderStatus(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderDangerousControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderAGCasinoControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderKenoControl(htmlcontent, customerInfoViewModel);

            htmlcontent = AssignTemplateValue(htmlcontent);
            htmlcontent = AssignCustomerInfo(htmlcontent, customerInfoViewModel);

            htmlcontent = RemoveEnableLinks(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);

            htmlcontent = RenderAbnormalCustomer(htmlcontent, customerInfoViewModel);
            htmlcontent = GenSettingControl(
                            htmlcontent,
                            row1,
                            customerInfoViewModel,
                            renderLeftPanelParameter,
                            dynamicSettingtable);

            return renderLeftPanelParameter;
        }
      
        private static string GenSettingControl(
           string htmlcontent,
           DataRow row1,
           CustomerInfoViewModel customerInfoViewModel,
           RenderLeftPanelParameter renderLeftPanelParameter,
           DataTable dcsTable = null)
        {
            SettingGroup sg = new SettingGroup();

            #region is Internal user

            htmlcontent = RenderInternalControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderAddMarkDirectAccountControl(htmlcontent, customerInfoViewModel, sg);

            #endregion is Internal user

            htmlcontent = RenderSuspendedControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderAllDayBalTransferControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderSelectedBettingControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderAutoMasterControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderDoubleCommControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderOutrightControl(htmlcontent, customerInfoViewModel);
            
            htmlcontent = RenderEsportsControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderGoldDeluxeForCreditControl(
                htmlcontent,
                row1,
                customerInfoViewModel.ManageCustomerAccess,
                renderLeftPanelParameter,
                sg,
                customerInfoViewModel.HasDownline);

            htmlcontent = RenderNumberGameControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderEnableCasinoControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderSuperLiveControl(
                htmlcontent,
                row1,
                customerInfoViewModel.CustParent,
                customerInfoViewModel.ManageCustomerAccess,
                renderLeftPanelParameter,
                sg,
                customerInfoViewModel.HasDownline);

            htmlcontent = RenderColossusBetControl(
                htmlcontent,
                row1,
                customerInfoViewModel.CustParent,
                customerInfoViewModel.ManageCustomerAccess,
                renderLeftPanelParameter,
                sg, 
                customerInfoViewModel.HasDownline);

            htmlcontent = RenderDCSControl(
                htmlcontent,
                row1,
                customerInfoViewModel.CustParent,
                customerInfoViewModel.ManageCustomerAccess,
                renderLeftPanelParameter,
                dcsTable,
                sg,
                customerInfoViewModel.HasDownline);

            TemplateHandler.Assign(ref htmlcontent, "{settingcontrol}", sg.GetHTMLSettingControl());

            return htmlcontent;
        }

        private string RenderClosedStatus(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var closedStatusBlock = GetClosedStatus(
                                        customerInfoViewModel.ManageCustomerAccess,
                                        "closed", 
                                        customerInfoViewModel.Closed,
                                        customerInfoViewModel.RoleId > 1,
                                        customerInfoViewModel.CustParent.IsClosed,
                                        customerInfoViewModel.AllowUpdateGivenCredit);

            TemplateHandler.Assign(ref htmlcontent, "{closedCustomer}", closedStatusBlock);

            return htmlcontent;
        }

        private static string AssignTemplateValue(string htmlcontent)
        {
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusBetTrade}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{betTradeId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdBetTrade);

            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusCashOut}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{cashOutId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdCashOut);

            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusMicrogaming}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{microGamingId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdMicrogaming);

            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusMacau}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{macauId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdMacau);

            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusSportsbook2}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{Sportsbook2Id}", "16");
            TemplateHandler.Assign(ref htmlcontent, "{settingType}", "4");
           

            TemplateHandler.Assign(ref htmlcontent, "{CricketId}", "20");
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusCricket}", "1");

            TemplateHandler.Assign(ref htmlcontent, "{AllbetId}", "21");
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusAllbet}", "1");
            return htmlcontent;
        }

        private static string RemoveEnableLinks(string htmlcontent, CustomerInfoViewModel customerInfoViewModel, RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (customerInfoViewModel.HideSportsbook2)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Sportsbook2");
            }

            if (customerInfoViewModel.HideCricketCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCricket");
            }

            if (!customerInfoViewModel.HasReadPermission_AutoSuspend)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "autosuspendstatus");
            }

            if (!WebSettings.EnableAGCasino || customerInfoViewModel.RoleId != 4 || !customerInfoViewModel.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "AGCasinoStatus");
            }

            if (!renderLeftPanelParameter.IsShowEnableBetTrade)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "BetTrade");
            }

            if (!renderLeftPanelParameter.IsShowEnableCashOut)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCashOut");
            }

            if (!renderLeftPanelParameter.IsShowEnableMicrogaming)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Microgaming");
            }

            if (!renderLeftPanelParameter.IsShowMacau)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Macau");
            }

            if (customerInfoViewModel.HideEnabledCheckBoxAllbet)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableAllbet");
            }

            return htmlcontent;
        }

        private static string RenderKenoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.ShowEnableCheckBoxKeno)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableKeno");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{CustId}", customerInfoViewModel.CustId);
                TemplateHandler.Assign(ref htmlcontent, "{RoleId}", customerInfoViewModel.RoleId);
                TemplateHandler.Assign(ref htmlcontent, "{userId}", AppSession.AccId);
            }

            return htmlcontent;
        }

        private static string RenderAGCasinoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var canEditAGCasinoStatus = WebSettings.EnableAGCasino && customerInfoViewModel.IsDeposit;
            if (!canEditAGCasinoStatus)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "AGCasinoStatus");
            }

            return htmlcontent;
        }

        private string RenderDangerousControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (Functions.Settings.IsVisibleDangerous)
            {
                if (customerInfoViewModel.RoleId > 2)
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "edited-dangerous");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "dangerous");
                    string[] dangeruostype = new string[] { "Original", "ABT", "Other Sports" };

                    int[] currentlevels = new int[] { customerInfoViewModel.Danger, customerInfoViewModel.Danger2, customerInfoViewModel.Danger3 };

                    TemplateHandler.Assign(ref htmlcontent, "{ThemePath}", WebSettings.ThemePath);
                    TemplateHandler.Assign(ref htmlcontent, "{dangerousCustomer}", 
                        GetSettingControlDangerous(
                            customerInfoViewModel.RoleId, 
                            customerInfoViewModel.ManageCustomerAccess, 
                            dangeruostype, 
                            currentlevels, 
                            customerInfoViewModel.IsBA));
                }
            }

            return htmlcontent;
        }

        private string RenderGoldDeluxeForDepositControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var isShowGoldDeluxeForDeposit = WebSettings.EnableGoldDeluxe && customerInfoViewModel.RoleId == 4 && customerInfoViewModel.IsDeposit;
            if (!isShowGoldDeluxeForDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "GoldDeluxe");
            }

            return htmlcontent;
        }

        private string RenderStatus(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (Functions.Settings.IsVisibleSpecial)
            {
                TemplateHandler.Assign(ref htmlcontent, "{specialCustomer}",
                    StatusSpan(customerInfoViewModel.ManageCustomerAccess,
                                "special",
                                customerInfoViewModel.Important,
                                customerInfoViewModel.RoleId > 1));
            }

            return htmlcontent;
        }

        private static string RenderLimitedPerIP(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.RoleId < 2) //Member
            {
                TemplateHandler.Assign(ref htmlcontent, "{LimitPerIP}", "");
            }
            else
            {
                int currentLimitPerIP = customerInfoViewModel.LimitPerIP;
                int parentLimiPerIP = 0;

                if (customerInfoViewModel.RoleId > 1 && customerInfoViewModel.RoleId < 4)
                {
                    parentLimiPerIP = customerInfoViewModel.CustParent.LimitPerIP;
                }
                int maxLimitPerIP = Constants.MaxLimitPerIP;

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
                    if (customerInfoViewModel.RoleId < 3)
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

            return htmlcontent;
        }

        private static string RenderDCSControl(
            string htmlcontent,
            DataRow row1,
            CustomerDetail CustParent,
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            DataTable dcsTable,
            SettingGroup sg,
            bool hasDownline)
        {
            foreach (DataRow productRow in dcsTable.Rows)
            {
                if (productRow["productID"] != null)
                {
                    var productId = productRow["productID"].ToString();
                    switch (productId)
                    {
                        case "5":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "VirtualSports",
                                               renderLeftPanelParameter.IsShowVirtualSport,
                                               renderLeftPanelParameter,
                                               "Update Virtual Sports Status",
                                               5);
                            break;
                        case "20":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               CustParent.IsDisableCricket,
                                               hasDownline,
                                               "Cricket",
                                               !renderLeftPanelParameter.IsHideCricket,
                                               renderLeftPanelParameter,
                                               "Update Cricket Status",
                                               20,
                                               4);
                            break;
                        case "22":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "Voidbridge",
                                               renderLeftPanelParameter.IsShowVoidbridge,
                                               renderLeftPanelParameter,
                                               "Update Macau Games Status",
                                               22);
                            break;
                        case "23":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "NG2",
                                               renderLeftPanelParameter.IsShowNG2,
                                               renderLeftPanelParameter,
                                               "Update " + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + " Status",
                                               23);
                            break;
                        case "24":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "FishingWorld",
                                               renderLeftPanelParameter.IsShowFishingWorld,
                                               renderLeftPanelParameter,
                                               "Update Fishing World Status",
                                               24);
                            break;
                        case "27":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "VirtualSports2",
                                               renderLeftPanelParameter.IsShowVirtualSport2,
                                               renderLeftPanelParameter,
                                               "Update Virtual Sports 2 Status",
                                               27);
                            break;

                        case "28":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "Lottery",
                                               renderLeftPanelParameter.IsShowKenoLottery,
                                               renderLeftPanelParameter,
                                               "Update Lottery Status",
                                               Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery);
                            break;

                        case "26":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "Lucky3",
                                               renderLeftPanelParameter.IsShowLucky3,
                                               renderLeftPanelParameter,
                                               "Update Lucky 3 Status",
                                               26);
                            break;

                        case "30":
                            RenderLeftPanelManager.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "Bullfighting",
                                               renderLeftPanelParameter.IsShowBullfighting,
                                               renderLeftPanelParameter,
                                               "Update Bullfighting Status",
                                               Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdBullfighting);
                            break;
                    }
                }
            }

            return htmlcontent;
        }

        private static string RenderColossusBetControl(
            string htmlcontent,
            DataRow row1,
            CustomerDetail CustParent,
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            SettingGroup sg,
            bool hasDownline)
        {
            RenderLeftPanelManager.RenderEnableProductBox2(
               row1,
               sg,
               ref htmlcontent,
               isManageCustomerAccess,
               CustParent.IsDisableColossusBets,
               hasDownline,
               "ColossusBet",
               renderLeftPanelParameter.IsShowColossusBetsForCredit,
               renderLeftPanelParameter,
               "Update Colossus Bets Status",
               15);
            return htmlcontent;
        }

        private static string RenderSuperLiveControl(
            string htmlcontent,
            DataRow row1,
            CustomerDetail CustParent,
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            SettingGroup sg,
            bool hasDownline)
        {
            RenderLeftPanelManager.RenderEnableProductBox(
                            row1,
                            sg,
                            ref htmlcontent,
                            isManageCustomerAccess,
                            CustParent.IsDisableMetricGaming,
                            hasDownline,
                            "SuperLive",
                            WebSettings.EnableMetricGaming,
                            renderLeftPanelParameter,
                            "Update Super Live Status");
            return htmlcontent;
        }

        private static string RenderGoldDeluxeForCreditControl(
            string htmlcontent,
            DataRow row1,
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            SettingGroup sg,
            bool hasDownline)
        {
            RenderLeftPanelManager.RenderOldProductEnableBoxLikeDCS(
                row1,
                sg,
                ref htmlcontent,
                isManageCustomerAccess,
                false,
                hasDownline,
                "GoldDeluxe",
                renderLeftPanelParameter.IsShowGoldDeluxeForCredit,
                renderLeftPanelParameter,
                "Update Gold Deluxe Status",
                12);
            return htmlcontent;
        }

        private static string RenderAddMarkDirectAccountControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel, SettingGroup sg)
        {
            htmlcontent = ConfigureDirectAccountService.AddMarkDirectAccountSettingControl(new AddMarkDirectAccountSettingControlCriteria
            {
                SettingGroup = sg,
                UserId = customerInfoViewModel.CustId.ToString(),
                Username = customerInfoViewModel.UserName,
                DirectAccountStatusInfo = new DirectAccountStatusInfo
                {
                    IsShowDirectStatus = !customerInfoViewModel.HideDirectStatus,
                    IsCheckedDirectStatus = customerInfoViewModel.CheckedDirectStatus,
                    CanEditDirectStatus = !customerInfoViewModel.DisabledDirectStatus && Permissions.GetHostPermission(Enumerations.Functions.Customer.ConfigureDirectAccount)
                },

                RoleId = customerInfoViewModel.RoleId,
                Htmlcontent = htmlcontent,
                IsManageCustomerAccess = customerInfoViewModel.ManageCustomerAccess
            }, htmlcontent);

            return htmlcontent;
        }

        private static string RenderInternalControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!WebSettings.DisableInternal && Functions.Settings.IsVisibleInternal)
            {
                var internalControlSettingGroup = new SettingGroup();

                internalControlSettingGroup.Add(
                    GetSettingControlInternalUser(
                        customerInfoViewModel.RoleId,
                        customerInfoViewModel.CustParent.IsInternalUser,
                        customerInfoViewModel.ManageCustomerAccess,
                        "internalUser",
                        customerInfoViewModel.IsInternalUser));

                TemplateHandler.Assign(ref htmlcontent, "{internalControl}", internalControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "internalControl");
            }

            return htmlcontent;
        }

        private static string RenderAllDayBalTransferControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.RoleId == 4)
            {
                var allDayBalTransferControlSettingGroup = new SettingGroup();

                allDayBalTransferControlSettingGroup.Add(
                    GetSettingControlAllDayBalTransfer(
                        customerInfoViewModel.RoleId,
                        customerInfoViewModel.ManageCustomerAccess,
                        "AllDayBalTransfer",
                        customerInfoViewModel.AllDayBalTransfer));

                TemplateHandler.Assign(ref htmlcontent, "{allDayBalTransferControl}", allDayBalTransferControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "allDayBalTransferControl");
            }

            return htmlcontent;
        }

        private static string RenderSuspendedControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var suspendedControlSettingGroup = new SettingGroup();

            suspendedControlSettingGroup.Add(
                GetSettingControlSuspended(
                    customerInfoViewModel.ManageCustomerAccess,
                    "suspended",
                    customerInfoViewModel.Suspend,
                    customerInfoViewModel.RoleId > 1,
                    customerInfoViewModel.CustParent.IsSuspended));

            TemplateHandler.Assign(ref htmlcontent, "{suspendedControl}", suspendedControlSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private static string RenderEnableCasinoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref htmlcontent, "{chkCasino}", customerInfoViewModel.DisabledRnGCasino ? 1 : 0);

            var casinoSettingGroup = new SettingGroup();
            casinoSettingGroup.Add(GetSettingControlCasino(
                                    customerInfoViewModel.RoleId,
                                    customerInfoViewModel.ManageCustomerAccess,
                                    "disabledCS",
                                    !customerInfoViewModel.DisabledRnGCasino,
                                    customerInfoViewModel.SyncRnGCasino,
                                    customerInfoViewModel.AllowEditRnGCasino,
                                    customerInfoViewModel.Enabled,
                                    customerInfoViewModel.DisabledRnGCasinoParent,
                                    !customerInfoViewModel.DisabledDownlinesRnGCasino));

            TemplateHandler.Assign(ref htmlcontent, "{casinoControl}", casinoSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private static string RenderNumberGameControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var numberGameSettingGroup = new SettingGroup();

            numberGameSettingGroup.Add(
                GetSettingControlNumberGame(
                    customerInfoViewModel.RoleId,
                    customerInfoViewModel.CustParent.IsDisabledNumberGame,
                    customerInfoViewModel.ManageCustomerAccess,
                    "disabledNumberGame",
                    !customerInfoViewModel.DisabledNumberGame,
                    !customerInfoViewModel.DisabledDownlinesNumberGame,
                    customerInfoViewModel.Enabled));

            TemplateHandler.Assign(ref htmlcontent, "{chkNumberGame}", customerInfoViewModel.DisabledNumberGame ? 1 : 0);
            TemplateHandler.Assign(ref htmlcontent, "{numberGameControl}", numberGameSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private static string RenderEsportsControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["HideESports"]) && (Permissions.HasPermission(Enumerations.Functions.Customer.EnableEsports)))
            {
                TemplateHandler.Assign(ref htmlcontent, "{chkEsports}", customerInfoViewModel.DisabledEsports ? 1 : 0);

                var eSportsSettingGroup = new SettingGroup();

                eSportsSettingGroup.Add(
                    GetSettingControlEsports(
                        customerInfoViewModel.RoleId,
                        customerInfoViewModel.ManageCustomerAccess,
                        "disabledEsports",
                        !customerInfoViewModel.DisabledEsports,
                        CastAide.AsBoolean(customerInfoViewModel.Enabled.ToString()),
                        customerInfoViewModel.DisabledEsportsParent,
                        !customerInfoViewModel.DisabledEsportsDownline));

                TemplateHandler.Assign(ref htmlcontent, "{eSportsControl}", eSportsSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "{chkEsports}");
                TemplateHandler.RemoveBlock(ref htmlcontent, "eSportsControl");
            }

            return htmlcontent;
        }

        private static string RenderOutrightControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var outrightControlSettingGroup = new SettingGroup();

            outrightControlSettingGroup.Add(
                GetSettingControlOutright(
                    customerInfoViewModel.RoleId,
                    customerInfoViewModel.CustParent.IsDisableOutright,
                    customerInfoViewModel.ManageCustomerAccess, 
                    "disabledOutright",
                    !customerInfoViewModel.DisableOutright,
                    customerInfoViewModel.RoleId > 1,
                    customerInfoViewModel.Enabled));

            TemplateHandler.Assign(ref htmlcontent, "{outrightControl}", outrightControlSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private static string RenderDoubleCommControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (WebSettings.OfferDoubleComm)
            {
                var doubleCommControlSettingGroup = new SettingGroup();

                doubleCommControlSettingGroup.Add(
                    GetSettingControlDoubleComm(
                        customerInfoViewModel.RoleId,
                        customerInfoViewModel.CustParent.IsEnableDoubleCommLosing,
                        customerInfoViewModel.ManageCustomerAccess, "doubleCommLosing",
                        customerInfoViewModel.Doublecommlosing,
                        customerInfoViewModel.RoleId > 1,
                        customerInfoViewModel.Enabled));

                TemplateHandler.Assign(ref htmlcontent, "{doubleCommControl}", doubleCommControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "doubleCommControl");
            }

            return htmlcontent;
        }

        private static string RenderAutoMasterControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.RoleId == 3)
            {
                var allowAutoPTControlSettingGroup = new SettingGroup();

                allowAutoPTControlSettingGroup.Add(
                    GetSettingControlEnableAutoMaster(
                        customerInfoViewModel.RoleId,
                        customerInfoViewModel.ManageCustomerAccess,
                        "Allow Auto PT",
                        customerInfoViewModel.DisableAutoMPT,
                        customerInfoViewModel.Enabled));

                TemplateHandler.Assign(ref htmlcontent, "{allowAutoPTControl}", allowAutoPTControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "allowAutoPTControl");
            }

            return htmlcontent;
        }

        private static string RenderSelectedBettingControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.RoleId > 1 && Functions.Settings.IsVisibleSelectedBetting)
            {
                var selectedBettingControlSettingGroup = new SettingGroup();

                selectedBettingControlSettingGroup.Add(
                    GetSettingControlSelectedBetting(
                        customerInfoViewModel.RoleId,
                        customerInfoViewModel.CustParent.SelectedBetting,
                        customerInfoViewModel.ManageCustomerAccess,
                        "SelectedBetting",
                        customerInfoViewModel.SelectedBetting));

                TemplateHandler.Assign(ref htmlcontent, "{selectedBettingControl}", selectedBettingControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "selectedBettingControl");
            }

            return htmlcontent;
        }

        private static string AssignCustomerInfo(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref htmlcontent, "{CustId}", customerInfoViewModel.CustId);
            TemplateHandler.Assign(ref htmlcontent, "{RoleId}", customerInfoViewModel.RoleId);
            TemplateHandler.Assign(ref htmlcontent, "{userId}", AppSession.AccId);
            TemplateHandler.Assign(ref htmlcontent, "{HasDownline}", customerInfoViewModel.HasDownline ? 1 : 0);

            return htmlcontent;
        }

        private static string RenderAbnormalCustomer(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (WebSettings.EnableAbnormal)
            {
                if (customerInfoViewModel.RoleId > 2)
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Abnormal");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "Abnormal");
                    TemplateHandler.Assign(ref htmlcontent, "{AbnormalCustomer}",
                        GetSettingAbnormal(
                            customerInfoViewModel.RoleId, 
                            customerInfoViewModel.ManageCustomerAccess, 
                            customerInfoViewModel.Abnormal));
                }
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "AbnormalSession");
            }

            return htmlcontent;
        }
    }
}
