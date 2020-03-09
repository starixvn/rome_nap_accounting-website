namespace Sunplus.Accounting.Business.Customer
{
    using ConfigureDirectAccount;
    using Core.Common;
    using Core.Configurations;
    using Core.Extensions;
    using Core.Web;
    using Criteria;
    using DataAccess;
    using DNAide.Web;
    using Entities;
    using GlobalResources;
    using Security;
    using System;
    using System.Collections.Generic;
    using System.Data;

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
                IsShowAutoSuspend = customerInfoViewModel.HasReadPermission_AutoSuspend,
                IsDeposit = customerInfoViewModel.IsDeposit,
                IsShowColossusBetsForDeposit = WebSettings.IsShowColossusBets && customerInfoViewModel.IsDeposit,
                IsShowLiveCasinoForCredit = !customerInfoViewModel.HideEnabledCheckBoxLiveCasino,
                IsShowVoidbridge = !customerInfoViewModel.HideEnabledCheckBoxVoidbridge,
                IsShowFishingWorld = !customerInfoViewModel.HideEnableAndEditFishingWorld,
                IsShowVirtualSport = !customerInfoViewModel.HideCheckBoxVirtualSports,
                IsShowVirtualSport2 = !customerInfoViewModel.HideVirtualSports2,
                ParentId = customerInfoViewModel.ParentId,
                RoleId = customerInfoViewModel.SearchAccountRoleId,
                CustId = customerInfoViewModel.CustId,
                Username = customerInfoViewModel.UserName,
                AccId = AppSession.AccId,
                IsShowNG2 = !customerInfoViewModel.HideEnabledCheckBoxNG2Credit,
                IsShowKenoLottery = !customerInfoViewModel.HideKenoLottery,
                IsShowVirtualGames = !customerInfoViewModel.HideVirtualGames,
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

        private string GenSettingControl(
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
                                        customerInfoViewModel.SearchAccountRoleId > 1,
                                        customerInfoViewModel.CustParent.IsClosed,
                                        customerInfoViewModel.AllowUpdateGivenCredit);

            TemplateHandler.Assign(ref htmlcontent, "{closedCustomer}", closedStatusBlock);

            return htmlcontent;
        }

        private string AssignTemplateValue(string htmlcontent)
        {
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusBetTrade}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{betTradeId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdBetTrade);

            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusCashOut}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{cashOutId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdCashOut);

            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusMacau}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{macauId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdMacau);

            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusSportsbook2}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{Sportsbook2Id}", "16");
            TemplateHandler.Assign(ref htmlcontent, "{settingType}", "4");

            TemplateHandler.Assign(ref htmlcontent, "{livecasino}", "41");
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusLiveCasino}", "1");
            return htmlcontent;
        }

        private string RemoveEnableLinks(string htmlcontent, CustomerInfoViewModel customerInfoViewModel, RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (customerInfoViewModel.HideSportsbook2)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Sportsbook2");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{Sportbook2OnClickFunction}", _renderLeftPanelService.GetSportbook2StatusJsFunction(
                    customerInfoViewModel.CustId,
                    customerInfoViewModel.UserName,
                    customerInfoViewModel.SearchAccountRoleId,
                    "Update Sportbook2 Status"));
            }

            if (!customerInfoViewModel.HasReadPermission_AutoSuspend)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "autosuspendstatus");
            }

            if (!WebSettings.EnableAGCasino || customerInfoViewModel.SearchAccountRoleId != 4 || !customerInfoViewModel.IsDeposit)
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

            if (!renderLeftPanelParameter.IsShowMacau)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Macau");
            }

            if (customerInfoViewModel.HideEnabledCheckBoxLiveCasino)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableLiveCasino");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{liveCasinoOnClickFunction}", _renderLeftPanelService.GetProductStatusJsFunction(
                    customerInfoViewModel.IsDeposit,
                    customerInfoViewModel.CustId,
                    customerInfoViewModel.UserName,
                    customerInfoViewModel.SearchAccountRoleId,
                    41,
                    "Update Live Casino Status"));
            }

            return htmlcontent;
        }

        private string RenderKenoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.ShowEnableCheckBoxKeno)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableKeno");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{KenoOnClickFunction}", _renderLeftPanelService.GetProductStatusJsFunction(
                    customerInfoViewModel.IsDeposit,
                    customerInfoViewModel.CustId,
                    customerInfoViewModel.UserName,
                    customerInfoViewModel.SearchAccountRoleId,
                    8,
                    "Update RNG Keno Status"));
            }

            return htmlcontent;
        }

        private string RenderAGCasinoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
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
                if (customerInfoViewModel.IsDeposit && customerInfoViewModel.SearchAccountRoleId > 2)
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "edited-dangerous");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "dangerous");

                    var dangerousTypes = new List<DangerousType>()
                    {
                        new DangerousType(customerInfoViewModel.Danger1, CustomerInfoResources.Danger1),
                        new DangerousType(customerInfoViewModel.Danger2, CustomerInfoResources.Danger2),
                        new DangerousType(customerInfoViewModel.Danger3, CustomerInfoResources.Danger3)
                    };

                    DangerousSetting danger = new DangerousSetting();
                    TemplateHandler.Assign(ref htmlcontent, "{ThemePath}", WebSettings.ThemePath);

                    TemplateHandler.Assign(ref htmlcontent, "{dangerousCustomer}",
                        danger.GenDangerous(
                            customerInfoViewModel.SearchAccountRoleId,
                            customerInfoViewModel.ManageCustomerAccess,
                            dangerousTypes,
                            customerInfoViewModel.IsBA,
                            customerInfoViewModel.SearchAccountRoleName));
                }
            }

            return htmlcontent;
        }

        private string RenderGoldDeluxeForDepositControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var isShowGoldDeluxeForDeposit = WebSettings.EnableGoldDeluxe && customerInfoViewModel.SearchAccountRoleId == 4 && customerInfoViewModel.IsDeposit;
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
                                customerInfoViewModel.SearchAccountRoleId > 1));
            }

            return htmlcontent;
        }

        private string RenderLimitedPerIP(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.SearchAccountRoleId < 2) //Member
            {
                TemplateHandler.Assign(ref htmlcontent, "{LimitPerIP}", "");
            }
            else
            {
                int currentLimitPerIP = customerInfoViewModel.LimitPerIP;
                int parentLimiPerIP = 0;

                if (customerInfoViewModel.SearchAccountRoleId > 1 && customerInfoViewModel.SearchAccountRoleId < 4)
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
                    if (customerInfoViewModel.SearchAccountRoleId < 3)
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

        private string RenderDCSControl(
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
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
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
                        case "22":
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
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
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "NG2",
                                               renderLeftPanelParameter.IsShowNG2,
                                               renderLeftPanelParameter,
                                               "Update " + _remotingRefService.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + " Status",
                                               23);
                            break;
                        case "24":
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
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
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
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
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
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
                        case "38":
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "VirtualGames",
                                               renderLeftPanelParameter.IsShowVirtualGames,
                                               renderLeftPanelParameter,
                                               "Update Virtual Games Status",
                                               Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdVirtualGames);
                            break;
                    }
                }
            }

            return htmlcontent;
        }

        private string RenderColossusBetControl(
            string htmlcontent,
            DataRow row1,
            CustomerDetail CustParent,
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            SettingGroup sg,
            bool hasDownline)
        {
            _renderLeftPanelService.RenderEnableProductBox2(
               row1,
               sg,
               ref htmlcontent,
               isManageCustomerAccess,
               CustParent.IsDisableColossusBets,
               hasDownline,
               "ColossusBet",
               renderLeftPanelParameter.IsShowColossusBetsForDeposit,
               renderLeftPanelParameter,
               "Update Colossus Bets Status",
               15);
            return htmlcontent;
        }

        private string RenderSuperLiveControl(
            string htmlcontent,
            DataRow row1,
            CustomerDetail CustParent,
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            SettingGroup sg,
            bool hasDownline)
        {
            _renderLeftPanelService.RenderEnableProductBox(
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

        private string RenderGoldDeluxeForCreditControl(
            string htmlcontent,
            DataRow row1,
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            SettingGroup sg,
            bool hasDownline)
        {
            _renderLeftPanelService.RenderOldProductEnableBoxLikeDCS(
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

        private string RenderAddMarkDirectAccountControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel, SettingGroup sg)
        {
            htmlcontent = _configureDirectAccountService.AddMarkDirectAccountSettingControl(new AddMarkDirectAccountSettingControlCriteria
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

                RoleId = customerInfoViewModel.SearchAccountRoleId,
                Htmlcontent = htmlcontent,
                IsManageCustomerAccess = customerInfoViewModel.ManageCustomerAccess
            }, htmlcontent);

            return htmlcontent;
        }

        private string RenderInternalControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!WebSettings.DisableInternal && Functions.Settings.IsVisibleInternal)
            {
                var internalControlSettingGroup = new SettingGroup();

                internalControlSettingGroup.Add(
                    GetSettingControlInternalUser(
                        customerInfoViewModel.SearchAccountRoleId,
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

        private string RenderAllDayBalTransferControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.SearchAccountRoleId == 4 || 
                (customerInfoViewModel.IsShowAllDayBalTransfer && customerInfoViewModel.SearchAccountRoleId == 3))
            {
                var allDayBalTransferControlSettingGroup = new SettingGroup();

                allDayBalTransferControlSettingGroup.Add(
                    GetSettingControlAllDayBalTransfer(
                        customerInfoViewModel.SearchAccountRoleId,
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

        private string RenderSuspendedControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var suspendedControlSettingGroup = new SettingGroup();

            suspendedControlSettingGroup.Add(
                GetSettingControlSuspended(
                    customerInfoViewModel.ManageCustomerAccess,
                    "suspended",
                    customerInfoViewModel.Suspend,
                    customerInfoViewModel.SearchAccountRoleId > 1,
                    customerInfoViewModel.CustParent.IsSuspended));

            TemplateHandler.Assign(ref htmlcontent, "{suspendedControl}", suspendedControlSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private string RenderEnableCasinoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref htmlcontent, "{chkCasino}", customerInfoViewModel.DisabledRnGCasino ? 1 : 0);

            var casinoSettingGroup = new SettingGroup();
            casinoSettingGroup.Add(GetSettingControlCasino(
                                    customerInfoViewModel.SearchAccountRoleId,
                                    customerInfoViewModel.ManageCustomerAccess,
                                    "disabledCS",
                                    !customerInfoViewModel.DisabledRnGCasino,
                                    customerInfoViewModel.SyncRnGCasino,
                                    customerInfoViewModel.AllowEditRnGCasino,
                                    customerInfoViewModel.Enabled,
                                    customerInfoViewModel.DisabledRnGCasinoParent));

            TemplateHandler.Assign(ref htmlcontent, "{casinoControl}", casinoSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private string RenderNumberGameControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var numberGameSettingGroup = new SettingGroup();

            numberGameSettingGroup.Add(
                GetSettingControlNumberGame(
                    customerInfoViewModel.SearchAccountRoleId,
                    customerInfoViewModel.CustParent.IsDisabledNumberGame,
                    customerInfoViewModel.ManageCustomerAccess,
                    "disabledNumberGame",
                    !customerInfoViewModel.DisabledNumberGame,
                    !customerInfoViewModel.DisabledDownlinesNumberGame,
                    customerInfoViewModel.Enabled,
                    customerInfoViewModel.IsDeposit));

            TemplateHandler.Assign(ref htmlcontent, "{chkNumberGame}", customerInfoViewModel.DisabledNumberGame ? 1 : 0);
            TemplateHandler.Assign(ref htmlcontent, "{numberGameControl}", numberGameSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private string RenderOutrightControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var outrightControlSettingGroup = new SettingGroup();

            outrightControlSettingGroup.Add(
                GetSettingControlOutright(
                    customerInfoViewModel.SearchAccountRoleId,
                    customerInfoViewModel.CustParent.IsDisableOutright,
                    customerInfoViewModel.ManageCustomerAccess,
                    "disabledOutright",
                    !customerInfoViewModel.DisableOutright,
                    customerInfoViewModel.SearchAccountRoleId > 1,
                    customerInfoViewModel.Enabled));

            TemplateHandler.Assign(ref htmlcontent, "{outrightControl}", outrightControlSettingGroup.GetHTMLSettingControl());
            return htmlcontent;
        }

        private string RenderDoubleCommControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (WebSettings.OfferDoubleComm)
            {
                var doubleCommControlSettingGroup = new SettingGroup();

                doubleCommControlSettingGroup.Add(
                    GetSettingControlDoubleComm(
                        customerInfoViewModel.SearchAccountRoleId,
                        customerInfoViewModel.CustParent.IsEnableDoubleCommLosing,
                        customerInfoViewModel.ManageCustomerAccess, "doubleCommLosing",
                        customerInfoViewModel.Doublecommlosing,
                        customerInfoViewModel.SearchAccountRoleId > 1,
                        customerInfoViewModel.Enabled));

                TemplateHandler.Assign(ref htmlcontent, "{doubleCommControl}", doubleCommControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "doubleCommControl");
            }

            return htmlcontent;
        }

        private string RenderAutoMasterControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.SearchAccountRoleId == 3)
            {
                var allowAutoPTControlSettingGroup = new SettingGroup();

                allowAutoPTControlSettingGroup.Add(
                    GetSettingControlEnableAutoMaster(
                        customerInfoViewModel.SearchAccountRoleId,
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

        private string RenderSelectedBettingControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.SearchAccountRoleId > 1 && Functions.Settings.IsVisibleSelectedBetting)
            {
                var selectedBettingControlSettingGroup = new SettingGroup();

                selectedBettingControlSettingGroup.Add(
                    GetSettingControlSelectedBetting(
                        customerInfoViewModel.SearchAccountRoleId,
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

        private string AssignCustomerInfo(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref htmlcontent, "{CustId}", customerInfoViewModel.CustId);
            TemplateHandler.Assign(ref htmlcontent, "{RoleId}", customerInfoViewModel.SearchAccountRoleId);
            TemplateHandler.Assign(ref htmlcontent, "{userId}", AppSession.AccId);
            TemplateHandler.Assign(ref htmlcontent, "{HasDownline}", customerInfoViewModel.HasDownline ? 1 : 0);

            return htmlcontent;
        }

        private string RenderAbnormalCustomer(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (WebSettings.EnableAbnormal)
            {
                if (customerInfoViewModel.SearchAccountRoleId > 2)
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Abnormal");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "Abnormal");
                    TemplateHandler.Assign(ref htmlcontent, "{AbnormalCustomer}",
                        GetSettingAbnormal(
                            customerInfoViewModel.SearchAccountRoleId,
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
