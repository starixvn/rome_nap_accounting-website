namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Data;
    using ConfigureDirectAccount;
    using Core.Common;
    using Core.Configurations;
    using Core.Extensions;
    using Core.Web;
    using Criteria;
    using DataAccess;
    using DNAide.Web;
    using Entities;
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
                IsShowMacau = customerInfoViewModel.ShowMacauDeposit,
                IsShowEnableBetTrade = customerInfoViewModel.ShowEnableBetTradeDeposit,
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
                IsShowRNGLottery = !customerInfoViewModel.HideRNGLottery,
                IsShowVirtualGames = !customerInfoViewModel.HideVirtualGames,
                IsShowSabaclub = !customerInfoViewModel.HideSabaclub,
                IsShowVGaming = !customerInfoViewModel.HideVGaming,
                IsShowSabaVirtualSports = !customerInfoViewModel.HideSabaVirtualSports,
                IsShowBitcoin = !customerInfoViewModel.HideBitcoin,
                IsShowCashout = !customerInfoViewModel.HideCashout,
                IsShowSabaCasino = !customerInfoViewModel.HideSabaCasino
            };

            htmlcontent = RenderClosedStatus(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderLockedAccess(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderProtectedStatus(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderLimitedPerIP(htmlcontent, customerInfoViewModel);

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
            htmlcontent = RenderAllDayBalTransferOnMasterControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderSelectedBettingControl(htmlcontent, customerInfoViewModel);

            htmlcontent = RenderAutoMasterControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderDoubleCommControl(htmlcontent, customerInfoViewModel);
            htmlcontent = RenderOutrightControl(htmlcontent, customerInfoViewModel);
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
                customerInfoViewModel.ManageCustomerAccess,
                renderLeftPanelParameter,
                dcsTable,
                sg);

            TemplateHandler.Assign(ref htmlcontent, "{settingcontrol}", sg.GetHTMLSettingControl());

            htmlcontent = RenderVirtualGamesControl(htmlcontent, renderLeftPanelParameter);

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
                                        customerInfoViewModel.IsDirectClosed,
                                        customerInfoViewModel.AllowUpdateGivenCredit);

            TemplateHandler.Assign(ref htmlcontent, "{closedCustomer}", closedStatusBlock);

            TemplateHandler.Assign(ref htmlcontent, "{ClosedLabel}", RenderClosedLabel(customerInfoViewModel.LockUnsuspend));
            return htmlcontent;
        }

        private string RenderClosedLabel(bool lockUnsuspend)
        {
            return lockUnsuspend ? "<span style=\"color:red;\">Closed (locked)</span>" : "Closed";
        }

        private string RenderLockedAccess(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "LockedAccessBlock");
            }
            else
            {
                if (customerInfoViewModel.LockedAccess.IsLocked)
                {
                    TemplateHandler.Assign(ref htmlcontent, "{LockedAccessEnabledOnClick}", customerInfoViewModel.ManageCustomerAccess
                        ? $"onclick=\"return unlockAccess('{customerInfoViewModel.LockedAccess.UnlockConfirmMessage}');\""
                        : string.Empty);
                    TemplateHandler.RemoveBlock(ref htmlcontent, "LockedAccessDisabled");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "LockedAccessEnabled");
                }
            }

            return htmlcontent;
        }

        private string RenderProtectedStatus(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.IsDeposit
                || customerInfoViewModel.SearchAccountRoleId < 2
                || CustomerAppSettings.IsNotOfferProtectedMode(customerInfoViewModel.SiteLabel))
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "ProtectedStatusBlock");
            }
            else
            {
                TemplateHandler.Assign(
                    ref htmlcontent,
                    "{ProtectedStatusClassName}",
                    customerInfoViewModel.IsProtectedMode ? "chkchecked" : "chkuncheck");
                TemplateHandler.RemoveBlock(
                    ref htmlcontent,
                    customerInfoViewModel.ManageCustomerAccess ? "ProtectedStatusDisabled" : "ProtectedStatusEnabled");
            }

            return htmlcontent;
        }

        private string AssignTemplateValue(string htmlcontent)
        {
            TemplateHandler.Assign(ref htmlcontent, "{uplineStatusBetTrade}", "1");
            TemplateHandler.Assign(ref htmlcontent, "{betTradeId}", Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdBetTrade);

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

            if (customerInfoViewModel.HideSabaclub)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableSabaclub");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{SabaclubOnClickFunction}", _renderLeftPanelService.GetProductStatusJsFunction(
                    customerInfoViewModel.IsDeposit,
                    customerInfoViewModel.CustId,
                    customerInfoViewModel.UserName,
                    customerInfoViewModel.SearchAccountRoleId,
                    42,
                    "Update Saba.games Status"));
            }

            if (customerInfoViewModel.HideVGaming)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableVGaming");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{VGamingOnClickFunction}", _renderLeftPanelService.GetProductStatusJsFunction(
                    customerInfoViewModel.IsDeposit,
                    customerInfoViewModel.CustId,
                    customerInfoViewModel.UserName,
                    customerInfoViewModel.SearchAccountRoleId,
                    66,
                    "Update Vgaming Status"));
            }

            if (customerInfoViewModel.HideRNGLottery)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableRNGLottery");
            }
            else
            {
                TemplateHandler.Assign(
                    ref htmlcontent,
                    "{RNGLotteryOnClickFunction}",
                    _renderLeftPanelService.GetProductStatusJsFunction(
                    customerInfoViewModel.IsDeposit,
                    customerInfoViewModel.CustId,
                    customerInfoViewModel.UserName,
                    customerInfoViewModel.SearchAccountRoleId,
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery,
                    "Update RNG Lottery Status"));
            }

            if (customerInfoViewModel.HideEnabledCheckBoxVoidbridge)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableMacauGames");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{macauGamesOnClickFunction}", _renderLeftPanelService.GetProductStatusJsFunction(
                    customerInfoViewModel.IsDeposit,
                    customerInfoViewModel.CustId,
                    customerInfoViewModel.UserName,
                    customerInfoViewModel.SearchAccountRoleId,
                    18,
                    "Update RNG Slot Status"));
            }

            return htmlcontent;
        }

        private string RenderKenoControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.ShowEnableCheckBoxKeno || customerInfoViewModel.IsDeposit)
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
                }
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
            bool isManageCustomerAccess,
            RenderLeftPanelParameter renderLeftPanelParameter,
            DataTable dcsTable,
            SettingGroup sg)
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

                        case "6":
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "SabaCasino",
                                               renderLeftPanelParameter.IsShowSabaCasino,
                                               renderLeftPanelParameter,
                                               "Update Saba Casino Status",
                                               6);
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

                        case "25":
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "Cashout",
                                               renderLeftPanelParameter.IsShowCashout,
                                               renderLeftPanelParameter,
                                               "Update Cash Out Status",
                                               25);
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

                        case "54":
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "SabaVirtualSports",
                                               renderLeftPanelParameter.IsShowSabaVirtualSports,
                                               renderLeftPanelParameter,
                                               "Update Saba Virtual Sports Status",
                                               Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdSabaVirtualSports);
                            break;
                        case "69":
                            _renderLeftPanelService.RenderEnableProductBoxDCS2(
                                               row1,
                                               productRow,
                                               sg,
                                               ref htmlcontent,
                                               isManageCustomerAccess,
                                               "Bitcoin",
                                               renderLeftPanelParameter.IsShowBitcoin,
                                               renderLeftPanelParameter,
                                               "Update Bitcoin Status",
                                               69);
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

        private string RenderAllDayBalTransferOnMasterControl(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.SearchAccountRoleId == 4)
            {
                var allDayBalTransferControlSettingGroup = new SettingGroup();

                allDayBalTransferControlSettingGroup.Add(
                    GetSettingControlAllDayBalTransferOnMaster(
                        customerInfoViewModel.SearchAccountRoleId,
                        customerInfoViewModel.ManageCustomerAccess,
                        "AllDayBalTransferOnMaster",
                        customerInfoViewModel.AllDayBalTransferOnMaster,
                        customerInfoViewModel.AllDayBalTransfer));

                TemplateHandler.Assign(ref htmlcontent, "{allDayBalTransferControlOnMaster}", allDayBalTransferControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "allDayBalTransferControlOnMaster");
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
                    customerInfoViewModel.CustParent.IsSuspended,
                    customerInfoViewModel.LockUnsuspend)
                );

            TemplateHandler.Assign(ref htmlcontent, "{suspendedControl}", suspendedControlSettingGroup.GetHTMLSettingControl());

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

        private string RenderVirtualGamesControl(string htmlcontent, RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (!renderLeftPanelParameter.IsShowVirtualGames)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableVirtualGames");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{VirtualGamesOnClickFunction}", _renderLeftPanelService.GetProductStatusJsFunction(
                    renderLeftPanelParameter.IsDeposit,
                    renderLeftPanelParameter.CustId,
                    renderLeftPanelParameter.Username,
                    renderLeftPanelParameter.RoleId,
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdVirtualGames,
                    "Update Virtual Games Status"));
            }

            return htmlcontent;
        }
    }
}