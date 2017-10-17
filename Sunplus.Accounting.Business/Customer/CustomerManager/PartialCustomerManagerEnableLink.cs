namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using DNAide.Core;
    using DNAide.Web;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
    using RemotingRef;
    using Criteria;
    using ConfigureDirectAccount;
    using Security;
    using Core.Common;
    using Core.Configurations;
    using Core.Extensions;
    using Core.Web;
    using DataAccess;
    using Entities;
    using Bingo.Core;

    public partial class CustomerManager
    {
        private RenderLeftPanelParameter RenderSettingMenu(
            string userName, 
            ref string htmlcontent,
            DataRow row1,
            DataTable dynamicSettingtable, 
            CustomerInfoViewModel customerInfoViewModel, 
            CustomerDetail _custparent, 
            int parentId, 
            int roleId, 
            int custId, 
            bool isManageCustomerAccess, 
            bool isallowupdategivencredit)
        {
            var strCloseStatus = "";


            if (AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL)
                && roleId == 4 &&
                !WebSettings.ExternalAdminUsersEditSuperStatus.Contains(AppSession.AccId.ToString())
                && customerInfoViewModel.closed)
            {
                strCloseStatus = "<span class='chkchecked'></span>";
            }

            TemplateHandler.Assign(ref htmlcontent, "{closedCustomer}", strCloseStatus != "" ? strCloseStatus : GetClosedStatus(isManageCustomerAccess, "closed", customerInfoViewModel.closed, (roleId > 1), _custparent.IsClosed, isallowupdategivencredit));

            //Limit per IP
            if (roleId < 2) //Member
            {
                TemplateHandler.Assign(ref htmlcontent, "{LimitPerIP}", "");
            }
            else
            {
                int currentLimitPerIP = customerInfoViewModel.LimitPerIP;
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

            // Louis: Auto suspend status
            var hasReadPermission_AutoSuspend = Permissions.HasPermission(Enumerations.Functions.Customer.AutoSuspend);
            if (!hasReadPermission_AutoSuspend)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "autosuspendstatus");
            }

            if (!WebSettings.EnableAGCasino || roleId != 4 || !customerInfoViewModel.isDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "AGCasinoStatus");
            }

            var isShowGoldDeluxe = WebSettings.EnableGoldDeluxe && roleId == 4 && customerInfoViewModel.isDeposit;
            if (!isShowGoldDeluxe)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "GoldDeluxe");
            }

            if (Functions.Settings.IsVisibleSpecial)
            {
                TemplateHandler.Assign(ref htmlcontent, "{specialCustomer}", StatusSpan(isManageCustomerAccess, "special", customerInfoViewModel.important, (roleId > 1)));
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

                    int[] currentlevels = new int[] { customerInfoViewModel.danger, customerInfoViewModel.danger2, customerInfoViewModel.danger3 };

                    TemplateHandler.Assign(ref htmlcontent, "{ThemePath}", WebSettings.ThemePath);
                    TemplateHandler.Assign(ref htmlcontent, "{dangerousCustomer}", GetSettingControlDangerous(roleId, isManageCustomerAccess, dangeruostype, currentlevels, customerInfoViewModel.isBA));
                }
            }

            #region Progressive Bonus

            bool isAllowProgressiveBonus = isManageCustomerAccess && IsAllowEditProgressiveBonus(parentId, custId);
            if (!isAllowProgressiveBonus)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "ProgressiveBonus");
            }

            var canEditAGCasinoStatus = WebSettings.EnableAGCasino && customerInfoViewModel.isLicensee;
            if (!canEditAGCasinoStatus)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "AGCasinoStatus");
            }

            bool isShowKenoStatus = KenoSection.Instance.Enable;
            if (!isShowKenoStatus)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableKeno");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{CustId}", custId);
                TemplateHandler.Assign(ref htmlcontent, "{RoleId}", roleId);
                TemplateHandler.Assign(ref htmlcontent, "{userId}", AppSession.AccId);
            }

            #endregion Progressive Bonus
            var customerDetail = new CustomerDetail
            {
                CustID = custId,
                RoleId = roleId,
                AccId = AppSession.AccId,
                HasDownline = customerInfoViewModel.HasDownline
            };

            var isShowEnableBetTrade = WebSettings.EnableBetTrade && roleId == 4 && customerInfoViewModel.isDeposit;
            var isShowEnableCashOut = WebSettings.EnableCashOut && roleId == 4 && customerInfoViewModel.isDeposit;
            var isShowEnableMicrogaming = WebSettings.EnableMicrogaming && customerInfoViewModel.isDeposit;
            var isShowMacau = WebSettings.EnableMacau && roleId == 4 && customerInfoViewModel.isDeposit;
            var isShowColossusBetsForCredit = !customerInfoViewModel.isHideColossusBets;
            var isShowSportsbook2 = !customerInfoViewModel.isHideSportsbook2 && !customerInfoViewModel.isDeposit;
            var isShowCockFighting = !customerInfoViewModel.isHideCockFighting && !customerInfoViewModel.isDeposit;
            if (!isShowSportsbook2)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Sportsbook2");
            }

            if (!isShowCockFighting)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCockFighting");
            }

            if (customerInfoViewModel.isHideAllbetUpline || !customerInfoViewModel.isSyncAllbetUpline)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableAllbet");
            }

            if (customerInfoViewModel.isHideCricketSettingLink)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCricket");

            }

            var hideCheckBoxAllbet = !WebSettings.IsShowAllBet || (roleId < 4 && customerInfoViewModel.isHideAllbetUpline) || !customerInfoViewModel.isSyncAllbetUpline;
            // !this.IsShowDCSStatusLink(isHideAllbetUpline, isSyncAllbet, isSyncAllbetUpline, isSyncAllbetDownline);
            var hideCheckBoxVoidbridge = !WebSettings.IsShowVoidbridge || (roleId < 4 && customerInfoViewModel.isHideVoidbridgeUpline) || !customerInfoViewModel.isSyncVoidbridgeUpline;
            // !this.IsShowDCSStatusLink(isHideVoidbridgeUpline, isSyncVoidbridge, isSyncVoidbridgeUpline, isSyncVoidbridgeDownline);

            var hideCheckBoxNG2 = !WebSettings.IsShowNG2 || customerInfoViewModel.isHideNG2Upline || !customerInfoViewModel.isSyncNG2Upline || !customerInfoViewModel.isSyncNG2 || customerInfoViewModel.isHideNG2;
            var hideCheckBoxLucky3 = !WebSettings.IsShowLucky3 || customerInfoViewModel.isHideLucky3Upline || customerInfoViewModel.isHideLucky3;

            var renderLeftPanelParameter = new RenderLeftPanelParameter
            {
                IsShowKeno = isShowKenoStatus,
                IsShowGoldDeluxe =
                    WebSettings.EnableGoldDeluxe &&
                    customerInfoViewModel.IsSyncUplineGoldDeluxe
                    && !customerInfoViewModel.isDeposit
                    && !string.Equals(customerInfoViewModel.currency, "Won", StringComparison.OrdinalIgnoreCase),
                IsShowMacau = isShowMacau,
                IsShowEnableBetTrade = isShowEnableBetTrade,
                IsShowEnableCashOut = isShowEnableCashOut,
                IsShowEnableMicrogaming = isShowEnableMicrogaming,
                IsShowAutoSuspend = hasReadPermission_AutoSuspend,
                IsShowColossusBetsForCredit = isShowColossusBetsForCredit,
                IsShowAllBetForCredit = !hideCheckBoxAllbet,
                IsShowVoidbridge = !hideCheckBoxVoidbridge,
                IsShowFishingWorld = !customerInfoViewModel.hideEnableAndEditFishingWorld,
                IsShowSportsbook2 = isShowSportsbook2,
                IsShowCockFighting = isShowCockFighting,
                IsHideCricket = customerInfoViewModel.isHideCricketSettingLink,
                IsShowVirtualSport = !customerInfoViewModel.isHideVirtualSports,
                IsShowVirtualSport2 = !customerInfoViewModel.isHideVirtualSports2,
                ParentId = parentId,
                RoleId = roleId,
                CustId = custId,
                IsDeposit = customerInfoViewModel.isDeposit,
                Username = userName,
                AccId = AppSession.AccId,
                IsShowNG2 = !hideCheckBoxNG2,
                IsShowKenoLottery = !customerInfoViewModel.hideKenoLottery,
                IsShowLucky3 = !hideCheckBoxLucky3
            };

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

            RenderLeftPanelManager.RenderEnableLink(ref htmlcontent, renderLeftPanelParameter);

            RenderLeftPanelManager.AssignCustomerInfo(ref htmlcontent, customerDetail);

            #region Abnormal

            RenderAbnormalCustomer(roleId, ref htmlcontent, customerInfoViewModel.abnormal, isManageCustomerAccess);

            #endregion Abnormal

            GenSettingControl(
                ref htmlcontent,
                row1,
                customerInfoViewModel,
                custId,
                roleId,
                parentId,
                _custparent,
                isManageCustomerAccess,
                customerInfoViewModel.suspend,
                customerInfoViewModel.autosuspend,
                customerInfoViewModel.UplineAutoSuspend,
                renderLeftPanelParameter,
                dynamicSettingtable);
            return renderLeftPanelParameter;
        }

        private void GenSettingControl(
            ref string htmlcontent,
            DataRow row1,
            CustomerInfoViewModel customerInfoViewModel,
            int custId,
            int roleId,
            int parentID,
            CustomerDetail CustParent,
            bool isManageCustomerAccess,
            bool isSuspended,
            bool autosuspend,
            bool UplineAutoSuspend,
            RenderLeftPanelParameter renderLeftPanelParameter,
            DataTable dcsTable = null)
        {
            //Convert value disable --> enable
            //Render Setting Control
            //HuyNguyen
            SettingGroup sg = new SettingGroup();
            string userName = customerInfoViewModel.username;

            var hasDownline = customerInfoViewModel.HasDownline;

            #region is Internal user

            if (!WebSettings.DisableInternal && Functions.Settings.IsVisibleInternal)
            {
                var internalControlSettingGroup = new SettingGroup();
                internalControlSettingGroup.Add(GetSettingControlInternalUser(roleId, CustParent.IsInternalUser, isManageCustomerAccess, "internalUser", customerInfoViewModel.isInternalUser, customerInfoViewModel.Enabled));
                TemplateHandler.Assign(ref htmlcontent, "{internalControl}", internalControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "internalControl");
            }

            ConfigureDirectAccountService.AddMarkDirectAccountSettingControl(new AddMarkDirectAccountSettingControlCriteria
            {
                SettingGroup = sg,
                UserId = customerInfoViewModel.custId.ToString(),
                Username = userName,
                DirectAccountStatusInfo = new DirectAccountStatusInfo
                {
                    IsShowDirectStatus = !customerInfoViewModel.IsHideDirectStatus,
                    IsCheckedDirectStatus = customerInfoViewModel.IsCheckedDirectStatus,
                    CanEditDirectStatus = !customerInfoViewModel.IsDisabledDirectStatus && Permissions.GetHostPermission(Enumerations.Functions.Customer.ConfigureDirectAccount)
                },

                RoleId = roleId,
                Htmlcontent = htmlcontent,
                IsManageCustomerAccess = isManageCustomerAccess
            });

            #endregion is Internal user

            //Get setting controll Suspended
            var suspendedControlSettingGroup = new SettingGroup();
            suspendedControlSettingGroup.Add(GetSettingControlSuspended(isManageCustomerAccess, "suspended", isSuspended, (roleId > 1), CustParent.IsSuspended));
            TemplateHandler.Assign(ref htmlcontent, "{suspendedControl}", suspendedControlSettingGroup.GetHTMLSettingControl());

            if (roleId == 4)
            {
                var allDayBalTransferControlSettingGroup = new SettingGroup();
                allDayBalTransferControlSettingGroup.Add(GetSettingControlAllDayBalTransfer(roleId, isManageCustomerAccess, "AllDayBalTransfer", customerInfoViewModel.isAllDayBalTransfer));
                TemplateHandler.Assign(ref htmlcontent, "{allDayBalTransferControl}", allDayBalTransferControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "allDayBalTransferControl");
            }

            #region Selected Betting
            if (roleId > 1 && Functions.Settings.IsVisibleSelectedBetting)
            {
                var selectedBettingControlSettingGroup = new SettingGroup();
                selectedBettingControlSettingGroup.Add(GetSettingControlSelectedBetting(roleId, CustParent.SelectedBetting, isManageCustomerAccess, "SelectedBetting",customerInfoViewModel.SelectedBetting, (roleId > 1)));
                TemplateHandler.Assign(ref htmlcontent, "{selectedBettingControl}", selectedBettingControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "selectedBettingControl");
            }

            #endregion Selected Betting

            #region Auto Master

            if (roleId == 3)
            {
                try
                {
                    var allowAutoPTControlSettingGroup = new SettingGroup();
                    allowAutoPTControlSettingGroup.Add(GetSettingControlEnableAutoMaster(roleId, isManageCustomerAccess, "Allow Auto PT", !customerInfoViewModel.DisableAutoMPT, customerInfoViewModel.Enabled));
                    TemplateHandler.Assign(ref htmlcontent, "{allowAutoPTControl}", allowAutoPTControlSettingGroup.GetHTMLSettingControl());
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                }
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "allowAutoPTControl");
            }

            #endregion Auto Master

            #region DoubleComm

            if (WebSettings.OfferDoubleComm)
            {
                var doubleCommControlSettingGroup = new SettingGroup();
                doubleCommControlSettingGroup.Add(GetSettingControlDoubleComm(roleId, CustParent.IsEnableDoubleCommLosing, isManageCustomerAccess, "doubleCommLosing", customerInfoViewModel.doublecommlosing, (roleId > 1), customerInfoViewModel.Enabled));
                TemplateHandler.Assign(ref htmlcontent, "{doubleCommControl}", doubleCommControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "doubleCommControl");
            }
            #endregion DoubleComm

            #region Outright

            var outrightControlSettingGroup = new SettingGroup();
            outrightControlSettingGroup.Add(GetSettingControlOutright(roleId, CustParent.IsDisableOutright, isManageCustomerAccess, "disabledOutright", !customerInfoViewModel.DisableOutright, (roleId > 1), customerInfoViewModel.Enabled));
            TemplateHandler.Assign(ref htmlcontent, "{outrightControl}", outrightControlSettingGroup.GetHTMLSettingControl());

            #endregion Outright

            #region HRFixedOdds

            if (WebSettings.OfferHorseRacingFixedOdds)
            {
                //bool isEnableHorseRacingFixedOdds = 
                //bool disableDownlinesHorseRacingFixedOdds = 

                var hRFixedOddsSettingGroup = new SettingGroup();
                hRFixedOddsSettingGroup.Add(GetSettingControlEnableHorseRacingFixedOdds(
                    roleId,
                    CustParent.IsEnableHorseRacingFixedOdds,
                    isManageCustomerAccess,
                    "enableHorseRacingFixedOdds",
                    customerInfoViewModel.isEnableHorseRacingFixedOdds,
                    (roleId > 1),
                    !customerInfoViewModel.disableDownlinesHorseRacingFixedOdds,
                    CastAide.AsBoolean(customerInfoViewModel.Enabled)));
                TemplateHandler.Assign(ref htmlcontent, "{chkEnableHorseRacingFixedOdds}", customerInfoViewModel.isEnableHorseRacingFixedOdds ? 1 : 0);

                TemplateHandler.Assign(ref htmlcontent, "{hRFixedOddsControl}", hRFixedOddsSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "hRFixedOddsControl");
            }
            #endregion HRFixedOdds

            #region Esports

            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["HideESports"]) && (Permissions.HasPermission(Enumerations.Functions.Customer.EnableEsports)))
            {
                TemplateHandler.Assign(ref htmlcontent, "{chkEsports}", customerInfoViewModel.disabledEsports ? 1 : 0);

                var eSportsSettingGroup = new SettingGroup();
                eSportsSettingGroup.Add(GetSettingControlEsports(roleId, isManageCustomerAccess, "disabledEsports", !customerInfoViewModel.disabledEsports,
                                                       (roleId > 1), customerInfoViewModel.disabledEsports,
                                                       CastAide.AsBoolean(customerInfoViewModel.Enabled.ToString()), customerInfoViewModel.disabledEsportsParent,
                                                       !customerInfoViewModel.disabledEsportsDownline));

                TemplateHandler.Assign(ref htmlcontent, "{eSportsControl}", eSportsSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "{chkEsports}");
                TemplateHandler.RemoveBlock(ref htmlcontent, "eSportsControl");
            }

            #endregion Esports

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

            #region Number Game

            var numberGameSettingGroup = new SettingGroup();
            numberGameSettingGroup.Add(GetSettingControlNumberGame(roleId, CustParent.IsDisabledNumberGame, isManageCustomerAccess,
                                               "disabledNumberGame", !customerInfoViewModel.disabledNumberGame, (roleId > 1),
                                               !customerInfoViewModel.disabledNumberGameDL, customerInfoViewModel.Enabled));
            TemplateHandler.Assign(ref htmlcontent, "{chkNumberGame}", customerInfoViewModel.disabledNumberGame ? 1 : 0);

            TemplateHandler.Assign(ref htmlcontent, "{numberGameControl}", numberGameSettingGroup.GetHTMLSettingControl());

            #endregion Number Game

            #region Enable Casino

            TemplateHandler.Assign(ref htmlcontent, "{chkCasino}", customerInfoViewModel.disabledCasino ? 1 : 0);

            var casinoSettingGroup = new SettingGroup();
            casinoSettingGroup.Add(GetSettingControlCasino(
                roleId, 
                parentID, 
                isManageCustomerAccess, 
                "disabledCS", 
                !customerInfoViewModel.disabledCasino, 
                (roleId > 1), 
                customerInfoViewModel.syncCasino,
                customerInfoViewModel.isAllowEditCS, 
                customerInfoViewModel.Enabled, 
                customerInfoViewModel.disabledCSParent, 
                !customerInfoViewModel.disabledCSDL));

            TemplateHandler.Assign(ref htmlcontent, "{casinoControl}", casinoSettingGroup.GetHTMLSettingControl());
            #endregion Enable Casino


            #region live casino
            TemplateHandler.Assign(ref htmlcontent, "{chkLiveCasino}", customerInfoViewModel.disabledLiveCasino ? 1 : 0);

            var liveCasinoSettingGroup = new SettingGroup();
            casinoSettingGroup.Add(GetSettingControlLiveCasino(roleId, isManageCustomerAccess, "disabledLCS", !customerInfoViewModel.disabledLiveCasino,
                                               (roleId > 1), customerInfoViewModel.syncLiveCasino, customerInfoViewModel.isAllowEditLCS,
                                               customerInfoViewModel.Enabled, customerInfoViewModel.disabledLCSParent,
                                               !customerInfoViewModel.disabledLCSDL));
            TemplateHandler.Assign(ref htmlcontent, "{liveCasinoControl}", casinoSettingGroup.GetHTMLSettingControl());

            #endregion live casino

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

            TemplateHandler.Assign(ref htmlcontent, "{settingcontrol}", sg.GetHTMLSettingControl());
        }

        private string StatusSpan(bool isManageCustomerAccess, string name, bool currStatus, bool hasDownLine)
        {
            try
            {
                string s = WebUtils.LoadTemplate("specialstatus.htm");
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
                            jsonclick = "return leftPanelManager.updateThirdPartyProductStatus({CustId}, '{Username}', {RoleId}, 6, 'Update Casino Status');";
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

        private SettingControl GetSettingControlEnableHorseRacingFixedOdds(
            int roleId,
            bool parentStatus,
            bool isManageCustomerAccess,
            string statusName,
            bool currentStatus,
            bool hasDownline,
            bool disableDownLine,
            bool isEnabled)
        {
            string classname = "";
            string jsonclick = "";
            string jsonmouseover = "";
            string jsonmouseout = "";
            if (isManageCustomerAccess)
            {
                if (isEnabled)
                {
                    if (4 == roleId || parentStatus)
                    {
                        classname = "canchange";
                        jsonclick = "return ShowDisablePop(this);";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" +
                            ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledHorseRacingFixedOddsUpline") +
                            "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" +
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") +
                        "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }

            return new SettingControl(
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnableHorseRacingFixedOdds"),
                currentStatus,
                false,
                jsonclick,
                jsonmouseout,
                jsonmouseover,
                classname,
                statusName,
                roleId,
                disableDownLine,
                "EnableHorseRacingFixedOdds");
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

        private SettingControl GetSettingControlLiveCasino(int roleId,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool hasDownLine,
            bool syncLiveCasino,
            bool isAllowEdit,
            bool isenabled,
            bool disabledUpline,
            bool disableDownLine)
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

        private SettingControl GetSettingControlAllDayBalTransfer(int roleId, bool isManageCustomerAccess, string statusName, bool currentStatus)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;

            if (isManageCustomerAccess)
            {
                if (roleId == 4)
                {
                    classname = "canchange";
                    jsonclick = "return ChangeAllDayBalTransfer(this);";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('Check to allow All-Day Balance Transfer mode', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
                else
                {
                    classname = "cannotchange";
                }
            }

            return new SettingControl("All-Day Bal Transfer", currentStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleId, false, statusName);
        }

        private SettingControl GetSettingControlEsports(int roleId, bool isManageCustomerAccess, string statusName, bool currStatus,
           bool hasDownLine, bool hasDownline, bool isenabled, bool disabledUpline, bool disableDownLine)
        {
            if (!WebSettings.EnableVirtualSports)
            {
                return
                    new SettingControl(
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "disableEsports"),
                        false, true, "", "", "", "", statusName, roleId, disableDownLine, "ES");
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
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledESUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
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
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEsports"),
                        currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleId,
                        disableDownLine, "ES");
            }
        }
    }
}
