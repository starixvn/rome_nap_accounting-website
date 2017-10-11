using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Fanex.Common;
using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Configurations;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Criteria;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Repositories.Impl;
using Fanex.LiveCasino.CoreSetting.Services;
using Fanex.Module.NG2.Entities;
using Fanex.Module.NG2.Services;
using Fanex.Tarus.Security.Criteria;
using Fanex.Tarus.Security.Domain;
using Fanex.Tarus.Security.Services;
using IBCUtils.Objects;
using RemotingRef;
using Sunplus.Accounting.Business.Criteria;
using Sunplus.Accounting.Business.Customer.ConfigureDirectAccount;
using Sunplus.Accounting.Business.CustSetting;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.AdoNet;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Bingo.Core;
using Result = Sunplus.Accounting.Entities.Result;

namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerManager
    {
        private void GenSettingControl(
            ref string htmlcontent,
            DataRow row1,
            DataRow row3,
            int custId,
            int roleId,
            int parentID,
            CustomerDetail CustParent,
            bool isManageCustomerAccess,
            bool isSuspended,
            bool autosuspend,
            bool UplineAutoSuspend,
            bool isAllowEdit,
            bool disabledCSParent,
            bool isAllowEditBI,
            bool disabledBIParent,
            bool isAllowEditLCS,
            bool disableLiveCasinoParent,
            bool disabledVSParent,
            bool disabledEsportsParent,
            RenderLeftPanelParameter renderLeftPanelParameter,
            DataTable dcsTable = null)
        {
            //Convert value disable --> enable
            //Render Setting Control
            //HuyNguyen
            SettingGroup sg = new SettingGroup();
            string userName = Converter.ToString2(row1["username"]);

            var hasDownline = Convert.IsDBNull(row1["HasDownline"]) ?
            false :
            Convert.ToBoolean(row1["HasDownline"]);

            #region is Internal user

            if (!WebSettings.DisableInternal && Functions.Settings.IsVisibleInternal)
            {
                bool isInternalUser = Convert.IsDBNull(row1["IsInternal"]) ? false : Convert.ToBoolean(row1["IsInternal"]);
                var internalControlSettingGroup = new SettingGroup();
                internalControlSettingGroup.Add(GetSettingControlInternalUser(roleId, CustParent.IsInternalUser, isManageCustomerAccess, "internalUser", isInternalUser, CastAide.AsBoolean(row1["Enabled"])));
                TemplateHandler.Assign(ref htmlcontent, "{internalControl}", internalControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "internalControl");
            }

            ConfigureDirectAccountService.AddMarkDirectAccountSettingControl(new AddMarkDirectAccountSettingControlCriteria
            {
                SettingGroup = sg,
                UserId = Converter.ToString2(row1["custid"]),
                Username = userName,
                DirectAccountStatusInfo = new DirectAccountStatusInfo
                {
                    IsShowDirectStatus = !Converter.ToBool(row1["IsHideDirectStatus"]),
                    IsCheckedDirectStatus = Converter.ToBool(row1["IsCheckDirectStatus"]),
                    CanEditDirectStatus = !Converter.ToBool(row1["IsDisabledDirectStatus"]) && Permissions.GetHostPermission(Enumerations.Functions.Customer.ConfigureDirectAccount)
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
                bool isAllDayBalTransfer = row1.Table.Columns.Contains("isalldaybaltransfer") && Convert.ToBoolean(row1["isalldaybaltransfer"]);
                var allDayBalTransferControlSettingGroup = new SettingGroup();
                allDayBalTransferControlSettingGroup.Add(GetSettingControlAllDayBalTransfer(roleId, isManageCustomerAccess, "AllDayBalTransfer", isAllDayBalTransfer));
                TemplateHandler.Assign(ref htmlcontent, "{allDayBalTransferControl}", allDayBalTransferControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "allDayBalTransferControl");
            }

            #region Selected Betting
            if (roleId > 1 && Functions.Settings.IsVisibleSelectedBetting)
            {
                bool SelectedBetting = Convert.IsDBNull(row1["SelectedBetting"]) ? false : Convert.ToBoolean(row1["SelectedBetting"]);
                var selectedBettingControlSettingGroup = new SettingGroup();
                selectedBettingControlSettingGroup.Add(GetSettingControlSelectedBetting(roleId, CustParent.SelectedBetting, isManageCustomerAccess, "SelectedBetting", SelectedBetting, (roleId > 1)));
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
                    bool enableAutoMPT = Convert.IsDBNull(row3["DisableAutoMPT"]) ? false : !Convert.ToBoolean(row3["DisableAutoMPT"]);
                    var allowAutoPTControlSettingGroup = new SettingGroup();
                    allowAutoPTControlSettingGroup.Add(GetSettingControlEnableAutoMaster(roleId, isManageCustomerAccess, "Allow Auto PT", !enableAutoMPT, CastAide.AsBoolean(row1["Enabled"])));
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
                bool IsEnableDoubleCommLosing = Convert.IsDBNull(row1["DoubleCommLosing"]) ? false : Convert.ToBoolean(row1["DoubleCommLosing"]);
                var doubleCommControlSettingGroup = new SettingGroup();
                doubleCommControlSettingGroup.Add(GetSettingControlDoubleComm(roleId, CustParent.IsEnableDoubleCommLosing, isManageCustomerAccess, "doubleCommLosing", IsEnableDoubleCommLosing, (roleId > 1), CastAide.AsBoolean(row1["Enabled"])));
                TemplateHandler.Assign(ref htmlcontent, "{doubleCommControl}", doubleCommControlSettingGroup.GetHTMLSettingControl());
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "doubleCommControl");
            }
            #endregion DoubleComm

            #region Outright

            bool disabledOutright = Convert.IsDBNull(row1["DisableOutright"]) ? false : Convert.ToBoolean(row1["DisableOutright"]);
            var outrightControlSettingGroup = new SettingGroup();
            outrightControlSettingGroup.Add(GetSettingControlOutright(roleId, CustParent.IsDisableOutright, isManageCustomerAccess, "disabledOutright", !disabledOutright, (roleId > 1), CastAide.AsBoolean(row1["Enabled"])));
            TemplateHandler.Assign(ref htmlcontent, "{outrightControl}", outrightControlSettingGroup.GetHTMLSettingControl());

            #endregion Outright

            #region HRFixedOdds

            if (WebSettings.OfferHorseRacingFixedOdds)
            {
                bool isEnableHorseRacingFixedOdds = Convert.IsDBNull(row1["DisableHRFixedOdds"]) ?
                    false :
                    !Convert.ToBoolean(row1["DisableHRFixedOdds"]);
                bool disableDownlinesHorseRacingFixedOdds = !Convert.IsDBNull(row1["DisableDownlinesHRFixedOdds"]) ?
                    Convert.ToBoolean(row1["DisableDownlinesHRFixedOdds"]) :
                    false;

                var hRFixedOddsSettingGroup = new SettingGroup();
                hRFixedOddsSettingGroup.Add(GetSettingControlEnableHorseRacingFixedOdds(
                    roleId,
                    CustParent.IsEnableHorseRacingFixedOdds,
                    isManageCustomerAccess,
                    "enableHorseRacingFixedOdds",
                    isEnableHorseRacingFixedOdds,
                    (roleId > 1),
                    !disableDownlinesHorseRacingFixedOdds,
                    CastAide.AsBoolean(row1["Enabled"])));
                TemplateHandler.Assign(ref htmlcontent, "{chkEnableHorseRacingFixedOdds}", isEnableHorseRacingFixedOdds ? 1 : 0);

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
                bool disabledEsports = Convert.IsDBNull(row1["DisableEsports"]) ? false : Convert.ToBoolean(row1["DisableEsports"]);
                bool disabledEsportsDownline = Convert.IsDBNull(row1["disabledownlinesesports"]) ? false : Convert.ToBoolean(row1["disabledownlinesesports"]);
                TemplateHandler.Assign(ref htmlcontent, "{chkEsports}", disabledEsports ? 1 : 0);

                var eSportsSettingGroup = new SettingGroup();
                eSportsSettingGroup.Add(GetSettingControlEsports(roleId, isManageCustomerAccess, "disabledEsports", !disabledEsports,
                                                       (roleId > 1), disabledEsports,
                                                       CastAide.AsBoolean(row1["Enabled"].ToString()), disabledEsportsParent,
                                                       !disabledEsportsDownline));

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

            bool disabledNumberGame = Convert.IsDBNull(row1["DisableNumber"]) ? false : Convert.ToBoolean(row1["DisableNumber"]);
            bool disabledNumberGameDL = Convert.IsDBNull(row1["DisableDownlinesNumber"]) ? false : Convert.ToBoolean(row1["DisableDownlinesNumber"]);
            var numberGameSettingGroup = new SettingGroup();
            numberGameSettingGroup.Add(GetSettingControlNumberGame(roleId, CustParent.IsDisabledNumberGame, isManageCustomerAccess,
                                               "disabledNumberGame", !disabledNumberGame, (roleId > 1),
                                               !disabledNumberGameDL, CastAide.AsBoolean(row1["Enabled"])));
            TemplateHandler.Assign(ref htmlcontent, "{chkNumberGame}", disabledNumberGame ? 1 : 0);

            TemplateHandler.Assign(ref htmlcontent, "{numberGameControl}", numberGameSettingGroup.GetHTMLSettingControl());

            #endregion Number Game

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

            #endregion HR

            #region Enable Casino

            bool disabledCasino = Convert.IsDBNull(row1["disablecasino"]) ? false : Convert.ToBoolean(row1["disablecasino"]);
            bool syncCasino = Convert.IsDBNull(row1["synccasino"]) ? false : Convert.ToBoolean(row1["synccasino"]);
            bool disabledCSDL = Convert.IsDBNull(row1["DisableDownlinesCS"]) ? false : Convert.ToBoolean(row1["DisableDownlinesCS"]);
            TemplateHandler.Assign(ref htmlcontent, "{chkCasino}", disabledCasino ? 1 : 0);

            var casinoSettingGroup = new SettingGroup();
            casinoSettingGroup.Add(GetSettingControlCasino(roleId, parentID, isManageCustomerAccess, "disabledCS", !disabledCasino, (roleId > 1), syncCasino, isAllowEdit, CastAide.AsBoolean(row1["Enabled"].ToString()), disabledCSParent, !disabledCSDL));
            TemplateHandler.Assign(ref htmlcontent, "{casinoControl}", casinoSettingGroup.GetHTMLSettingControl());
            #endregion Enable Casino

            #region Bingo Game

            bool disabledBingo = Convert.IsDBNull(row1["DisableBingo"]) ? false : Convert.ToBoolean(row1["DisableBingo"]);
            bool syncBingo = Convert.IsDBNull(row1["SyncBingo"]) ? false : Convert.ToBoolean(row1["SyncBingo"]);
            bool disabledBIDL = Convert.IsDBNull(row1["DisableDownlinesBI"]) ? false : Convert.ToBoolean(row1["DisableDownlinesBI"]);
            sg.Add(GetSettingControlBingoGame(roleId, parentID, isManageCustomerAccess, "disabledBingoGame",
                                              !disabledBingo, (roleId > 1), syncBingo, isAllowEditBI,
                                              CastAide.AsBoolean(row1["Enabled"].ToString()), disabledBIParent,
                                              !disabledBIDL));
            TemplateHandler.Assign(ref htmlcontent, "{chkBingo}", disabledBingo ? 1 : 0);

            #endregion Bingo Game

            #region live casino

            bool disabledLiveCasino = WebSettings.EnableLiveCasino ? Convert.IsDBNull(row1["disableLiveCasino"]) ? false : Convert.ToBoolean(row1["disableLiveCasino"]) : true;
            bool syncLiveCasino = WebSettings.EnableLiveCasino ? (Convert.IsDBNull(row1["synclivecasino"]) ? false : Convert.ToBoolean(row1["synclivecasino"])) : false;
            bool disabledLCSDL = WebSettings.EnableLiveCasino
                                     ? Convert.IsDBNull(row1["DisableDownlinesLiveCS"])
                                           ? false
                                           : Convert.ToBoolean(row1["DisableDownlinesLiveCS"])
                                     : true;
            TemplateHandler.Assign(ref htmlcontent, "{chkLiveCasino}", disabledLiveCasino ? 1 : 0);

            var liveCasinoSettingGroup = new SettingGroup();
            casinoSettingGroup.Add(GetSettingControlLiveCasino(roleId, isManageCustomerAccess, "disabledLCS", !disabledLiveCasino,
                                               (roleId > 1), syncLiveCasino, isAllowEditLCS,
                                               CastAide.AsBoolean(row1["Enabled"].ToString()), disableLiveCasinoParent,
                                               !disabledLCSDL));
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

        #region GetSettingControl

        private void GenMenuMinMaxPT(ref string htmlcontent, DataRow row2, int custId, int roleId, string currency, string userName, bool isAllowEditBI, bool isDeposit, string siteName, bool isHideSportsbook2, bool isHideCockFighting, bool isHideCricket, bool isShowAllBet, bool isShowVoidbridge, bool isShowNG2, bool isShowFishingWorld, bool isHideVirtualSports, bool isShowKenoLottery, bool isShowLucky3)
        {
            int parentId;
            string submenu1 = "";
            string submenu2 = "";
            string submenu3 = "";
            string minMaxPtUrl = "";
            var minMaxPtRenovationUrl = string.Empty;
            string masterPtUrl = "";
            bool isShowSetting = KenoSection.Instance.ShowSetting;
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
                        minMaxPtUrl = string.Format("EditSuperOnMember/EditSuperOnMemberPage?agentId={0}&custId={1}&uname={2}&siteName={3}", parentId, custId, userName, siteName);
                        masterPtUrl = string.Format("EditMasterOnMember/EditMasterOnMemberPage?agentId={0}&custId={1}&uname={2}&siteName={3}", parentId, custId, userName, siteName);
                    }

                    submenu1 = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";

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
                            var urlLiveCasino = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&parentId={1}&uplineLevel=4&actorId={2}&actorUsername={3}&customerId={4}&customerLevel=1", superId, parentId, AppSession.AccId, AppSession.AccUserName, custId);
                            submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + urlLiveCasino + "\");' target='centerFr'>Live Casino</a>";
                        }
                    }
                    if (!isHideVirtualSports && !isDeposit)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Virtual Sports</a>";
                    }

                    if (isShowSetting)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customersetting/KenoSetting/kenosetting/SuperEditMember?custId=" + custId.ToString() + "&userId=" + AppSession.AccId + "\");' target='centerFr'>Keno</a>";
                    }

                    if (WebSettings.IsShowGoldDeluxeSettingForCredit)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId=" + superId + "&upLineLevel=4&formType=1&actorId=" + AppSession.AccId + "&custId=" + custId.ToString() + "&level=1\");' target='centerFr'>Gold Deluxe</a>";
                    }

                    if (!isHideSportsbook2 && !isDeposit)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=16&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Sportsbook 2</a>";
                    }

                    if (!isHideCockFighting && !isDeposit)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=19&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Cockfighting</a>";
                    }

                    if (!isHideCricket)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=20&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Cricket</a>";
                    }

                    if (isShowAllBet)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=21&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Allbet</a>";
                    }

                    if (isShowVoidbridge)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=22&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Macau Games</a>";
                    }

                    if (isShowNG2)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=23&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>" + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + "</a>";
                    }

                    if (isShowFishingWorld)
                    {
                        submenu1 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?language=en-US&productId=24&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Fishing World</a>";
                    }

                    if (isShowKenoLottery)
                    {
                        submenu1 += string.Format(
                            "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                            Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery,
                            custId,
                            4, // Super Level.
                            "Lottery");
                    }

                    if (isShowLucky3)
                    {
                        submenu1 += string.Format(
                            "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                            26,
                            custId,
                            4, // Super Level.
                            "Lucky 3");
                    }

                    submenu2 = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + masterPtUrl + "\");' target='centerFr'>Sport book</a>";

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
                            var urlLiveCasino = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&parentId={1}&uplineLevel=3&actorId={2}&actorUsername={3}&customerId={4}&customerLevel=1", masterId, parentId, AppSession.AccId, AppSession.AccUserName, custId);
                            submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + urlLiveCasino + "\");' target='centerFr'>Live Casino</a>";
                        }
                    }
                    if (!isHideVirtualSports && !isDeposit)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Virtual Sports</a>";
                    }

                    if (isShowSetting)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customersetting/KenoSetting/kenosetting/MasterEditMember?custId=" + custId.ToString() + "&userId=" + AppSession.AccId + "\");' target='centerFr'>Keno</a>";
                    }

                    if (WebSettings.IsShowGoldDeluxeSettingForCredit)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId=" + masterId + "&upLineLevel=3&formType=1&actorId=" + AppSession.AccId + "&custId=" + custId.ToString() + "&level=1\");' target='centerFr'>Gold Deluxe</a>";

                    }
                    if (!isHideSportsbook2 && !isDeposit)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=16&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Sportsbook 2</a>";
                    }

                    if (!isHideCockFighting && !isDeposit)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=19&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Cockfighting</a>";
                    }

                    if (!isHideCricket)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=20&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Cricket</a>";
                    }

                    if (isShowAllBet)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=21&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Allbet</a>";
                    }

                    if (isShowVoidbridge)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=22&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Macau Games</a>";
                    }

                    if (isShowNG2)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=23&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>" + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + "</a>";
                    }

                    if (isShowFishingWorld)
                    {
                        submenu2 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?language=en-US&productId=24&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Fishing World</a>";
                    }

                    if (isShowKenoLottery)
                    {
                        submenu2 += string.Format(
                            "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                            Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery,
                            custId,
                            3, // Master Level.
                            "Lottery");
                    }

                    if (isShowLucky3)
                    {
                        submenu2 += string.Format(
                            "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                            26,
                            custId,
                            3, // Master Level.
                            "Lucky 3");
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

        #endregion GetSettingControl
    }
}
