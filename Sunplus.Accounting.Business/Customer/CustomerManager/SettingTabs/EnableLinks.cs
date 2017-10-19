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
                IsShowKeno = customerInfoViewModel.IsShowKenoStatus,
                IsShowGoldDeluxe =
                    WebSettings.EnableGoldDeluxe &&
                    customerInfoViewModel.IsSyncUplineGoldDeluxe
                    && !customerInfoViewModel.IsDeposit
                    && !string.Equals(customerInfoViewModel.Currency, "Won", StringComparison.OrdinalIgnoreCase),
                IsShowMacau = customerInfoViewModel.IsShowMacau,
                IsShowEnableBetTrade = customerInfoViewModel.IsShowEnableBetTrade,
                IsShowEnableCashOut = customerInfoViewModel.IsShowEnableCashOut,
                IsShowEnableMicrogaming = customerInfoViewModel.IsShowEnableMicrogaming,
                IsShowAutoSuspend = customerInfoViewModel.HasReadPermission_AutoSuspend,
                IsShowColossusBetsForCredit = customerInfoViewModel.IsShowColossusBetsForCredit,
                IsShowAllBetForCredit = !customerInfoViewModel.HideCheckBoxAllbet,
                IsShowVoidbridge = !customerInfoViewModel.HideCheckBoxVoidbridge,
                IsShowFishingWorld = !customerInfoViewModel.HideEnableAndEditFishingWorld,
                IsShowSportsbook2 = customerInfoViewModel.IsShowSportsbook2,
                IsShowCockFighting = customerInfoViewModel.IsShowCockFighting,
                IsHideCricket = customerInfoViewModel.IsHideCricketSettingLink,
                IsShowVirtualSport = !customerInfoViewModel.IsHideVirtualSports,
                IsShowVirtualSport2 = !customerInfoViewModel.IsHideVirtualSports2,
                ParentId = customerInfoViewModel.ParentId,
                RoleId = customerInfoViewModel.RoleId,
                CustId = customerInfoViewModel.CustId,
                IsDeposit = customerInfoViewModel.IsDeposit,
                Username = customerInfoViewModel.UserName,
                AccId = AppSession.AccId,
                IsShowNG2 = !customerInfoViewModel.HideCheckBoxNG2,
                IsShowKenoLottery = !customerInfoViewModel.HideKenoLottery,
                IsShowLucky3 = !customerInfoViewModel.HideCheckBoxLucky3
            };

            htmlcontent = RenderClosedStatus(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderLimitedPerIP(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderAutoSuspendControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderGoldDeluxeForDepositControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderDangerousControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderAGCasinoControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderKenoControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RemoveProductHtmlBlock(htmlcontent, customerInfoViewModel);

            htmlcontent = AssignTemplateValue(htmlcontent);

            htmlcontent = RemoveEnableLinks(htmlcontent, renderLeftPanelParameter);

            htmlcontent = AssignCustomerInfo(htmlcontent, customerInfoViewModel);

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
            RenderAddMarkDirectAccountControl(htmlcontent, customerInfoViewModel, sg);

            #endregion is Internal user

            htmlcontent = RenderSuspendedControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderAllDayBalTransferControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderSelectedBettingControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderAutoMasterControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderDoubleCommControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderOutrightControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderHRFixedOddsControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderEsportsControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderGoldDeluxeForCreditControl(
                htmlcontent,
                row1,
                customerInfoViewModel.IsManageCustomerAccess,
                renderLeftPanelParameter,
                sg,
                customerInfoViewModel.HasDownline);

            htmlcontent = RenderNumberGameControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderEnableCasinoControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderLiveCasinoControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderSuperLiveControl(
                htmlcontent,
                row1,
                customerInfoViewModel.CustParent,
                customerInfoViewModel.IsManageCustomerAccess,
                renderLeftPanelParameter,
                sg,
                customerInfoViewModel.HasDownline);

            htmlcontent = RenderColossusBetControl(
                htmlcontent,
                row1,
                customerInfoViewModel.CustParent,
                customerInfoViewModel.IsManageCustomerAccess,
                renderLeftPanelParameter,
                sg, customerInfoViewModel.HasDownline);

            htmlcontent = RenderDCSControl(
                htmlcontent,
                row1,
                customerInfoViewModel.CustParent,
                customerInfoViewModel.IsManageCustomerAccess,
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
                                        customerInfoViewModel.IsManageCustomerAccess,
                                        "closed", 
                                        customerInfoViewModel.Closed,
                                        customerInfoViewModel.RoleId > 1,
                                        customerInfoViewModel.CustParent.IsClosed,
                                        customerInfoViewModel.IsAllowUpdateGivenCredit);

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

            TemplateHandler.Assign(ref htmlcontent, "{CockFightingId}", "19");
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusCockFighting}", "1");

            TemplateHandler.Assign(ref htmlcontent, "{CricketId}", "20");
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusCricket}", "1");

            TemplateHandler.Assign(ref htmlcontent, "{AllbetId}", "21");
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusAllbet}", "1");
            return htmlcontent;
        }

        private static string RemoveProductHtmlBlock(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.IsShowSportsbook2)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Sportsbook2");
            }

            if (!customerInfoViewModel.IsShowCockFighting)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCockFighting");
            }

            if (customerInfoViewModel.IsHideAllbetUpline || !customerInfoViewModel.IsSyncAllbetUpline)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableAllbet");
            }

            if (customerInfoViewModel.IsHideCricketSettingLink)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCricket");

            }

            return htmlcontent;
        }

        private static string RenderKenoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.IsShowKenoStatus)
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
            var canEditAGCasinoStatus = WebSettings.EnableAGCasino && customerInfoViewModel.IsLicensee;
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
                            customerInfoViewModel.IsManageCustomerAccess, 
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

            if (Functions.Settings.IsVisibleSpecial)
            {
                TemplateHandler.Assign(ref htmlcontent, "{specialCustomer}", 
                    StatusSpan(customerInfoViewModel.IsManageCustomerAccess, 
                                "special", 
                                customerInfoViewModel.Important, 
                                customerInfoViewModel.RoleId > 1));
            }

            return htmlcontent;
        }

        private static string RenderAutoSuspendControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.HasReadPermission_AutoSuspend)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "autosuspendstatus");
            }

            if (!WebSettings.EnableAGCasino || customerInfoViewModel.RoleId != 4 || !customerInfoViewModel.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "AGCasinoStatus");
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
                renderLeftPanelParameter.IsShowGoldDeluxe,
                renderLeftPanelParameter,
                "Update Gold Deluxe Status",
                12);
            return htmlcontent;
        }

        private static void RenderAddMarkDirectAccountControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel, SettingGroup sg)
        {
            ConfigureDirectAccountService.AddMarkDirectAccountSettingControl(new AddMarkDirectAccountSettingControlCriteria
            {
                SettingGroup = sg,
                UserId = customerInfoViewModel.CustId.ToString(),
                Username = customerInfoViewModel.UserName,
                DirectAccountStatusInfo = new DirectAccountStatusInfo
                {
                    IsShowDirectStatus = !customerInfoViewModel.IsHideDirectStatus,
                    IsCheckedDirectStatus = customerInfoViewModel.IsCheckedDirectStatus,
                    CanEditDirectStatus = !customerInfoViewModel.IsDisabledDirectStatus && Permissions.GetHostPermission(Enumerations.Functions.Customer.ConfigureDirectAccount)
                },

                RoleId = customerInfoViewModel.RoleId,
                Htmlcontent = htmlcontent,
                IsManageCustomerAccess = customerInfoViewModel.IsManageCustomerAccess
            });
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
                        customerInfoViewModel.IsManageCustomerAccess,
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
                        customerInfoViewModel.IsManageCustomerAccess,
                        "AllDayBalTransfer",
                        customerInfoViewModel.IsAllDayBalTransfer));

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
                    customerInfoViewModel.IsManageCustomerAccess,
                    "suspended",
                    customerInfoViewModel.Suspend,
                    customerInfoViewModel.RoleId > 1,
                    customerInfoViewModel.CustParent.IsSuspended));

            TemplateHandler.Assign(ref htmlcontent, "{suspendedControl}", suspendedControlSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private static string RenderLiveCasinoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref htmlcontent, "{chkLiveCasino}", customerInfoViewModel.DisabledLiveCasino ? 1 : 0);

            var liveCasinoSettingGroup = new SettingGroup();

            liveCasinoSettingGroup.Add(
                GetSettingControlLiveCasino(
                    customerInfoViewModel.RoleId,
                    customerInfoViewModel.IsManageCustomerAccess,
                    "disabledLCS",
                    !customerInfoViewModel.DisabledLiveCasino,
                    customerInfoViewModel.SyncLiveCasino,
                    customerInfoViewModel.IsAllowEditLCS,
                    customerInfoViewModel.Enabled,
                    customerInfoViewModel.DisabledLCSParent,
                    !customerInfoViewModel.DisabledLCSDL));

            TemplateHandler.Assign(ref htmlcontent, "{liveCasinoControl}", liveCasinoSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private static string RenderEnableCasinoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref htmlcontent, "{chkCasino}", customerInfoViewModel.DisabledCasino ? 1 : 0);

            var casinoSettingGroup = new SettingGroup();
            casinoSettingGroup.Add(GetSettingControlCasino(
                                    customerInfoViewModel.RoleId,
                                    customerInfoViewModel.IsManageCustomerAccess,
                                    "disabledCS",
                                    !customerInfoViewModel.DisabledCasino,
                                    customerInfoViewModel.SyncCasino,
                                    customerInfoViewModel.IsAllowEditCS,
                                    customerInfoViewModel.Enabled,
                                    customerInfoViewModel.DisabledCSParent,
                                    !customerInfoViewModel.DisabledCSDL));

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
                    customerInfoViewModel.IsManageCustomerAccess,
                    "disabledNumberGame",
                    !customerInfoViewModel.DisabledNumberGame,
                    !customerInfoViewModel.DisabledNumberGameDL,
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
                        customerInfoViewModel.IsManageCustomerAccess,
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

        private static string RenderHRFixedOddsControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (WebSettings.OfferHorseRacingFixedOdds)
            {
                var hRFixedOddsSettingGroup = new SettingGroup();

                hRFixedOddsSettingGroup.Add(GetSettingControlEnableHorseRacingFixedOdds(
                    customerInfoViewModel.RoleId,
                    customerInfoViewModel.CustParent.IsEnableHorseRacingFixedOdds,
                    customerInfoViewModel.IsManageCustomerAccess,
                    "enableHorseRacingFixedOdds",
                    customerInfoViewModel.IsEnableHorseRacingFixedOdds,
                    !customerInfoViewModel.DisableDownlinesHorseRacingFixedOdds,
                    CastAide.AsBoolean(customerInfoViewModel.Enabled)));

                TemplateHandler.Assign(ref htmlcontent, "{chkEnableHorseRacingFixedOdds}", customerInfoViewModel.IsEnableHorseRacingFixedOdds ? 1 : 0);
                TemplateHandler.Assign(ref htmlcontent, "{hRFixedOddsControl}", hRFixedOddsSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "hRFixedOddsControl");
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
                    customerInfoViewModel.IsManageCustomerAccess, 
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
                        customerInfoViewModel.IsManageCustomerAccess, "doubleCommLosing",
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
                        customerInfoViewModel.IsManageCustomerAccess,
                        "Allow Auto PT",
                        !customerInfoViewModel.DisableAutoMPT,
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
                        customerInfoViewModel.IsManageCustomerAccess,
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

        public static string RemoveEnableLinks(string htmlcontent, RenderLeftPanelParameter renderLeftPanelParameter)
        {
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

            if (renderLeftPanelParameter.IsHideCricket)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCricket");
            }

            if (renderLeftPanelParameter.IsShowAllBetForCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableAllbet");
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
                            customerInfoViewModel.IsManageCustomerAccess, 
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
