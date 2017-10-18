namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Data;
    using System.Linq;
    using Bingo.Core;
    using Core.Common;
    using Core.Configurations;
    using DNAide.Core;
    using DNAide.Web;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
    using RemotingRef;
    using Security;

    public partial class CustomerManager
    {
        private static string RenderDetailMenu(
            string htmlcontent, 
            DataRow row1, 
            DataRow row2, 
            CustomerInfoViewModel customerInfoViewModel, 
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            string userName = customerInfoViewModel.userName;
            int parentId = customerInfoViewModel.parentId;
            int roleId = customerInfoViewModel.roleId;
            int custId = customerInfoViewModel.custId;

            htmlcontent = GenMenuMinMaxPT(
                        htmlcontent,
                        row2,
                        customerInfoViewModel,
                        renderLeftPanelParameter);

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
            var kenoSetting = string.Empty;
            var agCasinoLinkSetting = string.Empty;
            var casinoLinkSetting = string.Empty;
            var goldDeluxeLinkSetting = string.Empty;
            var colossusBetsLinkSetting = string.Empty;
            var creditDCSLink = string.Empty;
            string RacingUpdateURL = "";
            switch (roleId)
            {
                case 1: // member
                    parentId = Convert.ToInt32(row2["agentId"]);
                    bingoLinkSetting = "Customer/PT/Bingo/MemberUpdate.aspx?custId=" + custId;
                    pokerLinkSetting = "Customer/PT/Poker/MemberUpdate.aspx?custId=" + custId;
                    detailUrl = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingAgent" : "Customer/v2/EditCustomerNew.aspx?set=1&custId=" + custId + "&agentId=" + row2["agentId"] + "&uname=" + userName + "&m=0";
                    detailUrlNew = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingAgent" : "EditMember/EditMemberPage?custId=" + custId + "&agentId=" + row2["agentId"] + "&uname=" + userName + "&siteName=" + customerInfoViewModel.customerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    liveCasinoSetting = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&parentId={1}&uplineLevel=2&actorId={2}&actorUsername={3}&customerId={4}&customerLevel=1", parentId, parentId, AppSession.AccId, AppSession.AccUserName, custId);
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, parentId, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/AgentEditMember?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{IsHidden1}", "hidden");
                    TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "");
                    RacingUpdateURL = string.Format("EditMemberPTRacing/Index?custid={0}&uplineid={1}", custId, parentId);
                    casinoLinkSetting = "CustomerSetting/Casino/Member/EditMember/" + custId + "/" + parentId;
                    goldDeluxeLinkSetting = string.Format("GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId={0}&uplineLevel=2&actorId={1}&custId={2}&level=1&formType=1", parentId, AppSession.AccId, custId);
                    colossusBetsLinkSetting = string.Format("customersetting/ColossusBetSetting/ColossusBetSetting/EditSetting?custid={0}&recommend={1}&roleid=1&UserId={2}", custId, parentId, AppSession.AccId);
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=1&uplinelevel=2&customerId=" + custId;
                    break;

                case 2: // agent
                    int _masterID = CastAide.AsInt32(row2["masterid"], -1);
                    RacingUpdateURL = string.Format("EditAgentPTRacing/Index?custid={0}&uplineid={1}", custId, _masterID);
                    bingoLinkSetting = "Customer/PT/Bingo/AgentUpdate.aspx?custId=" + custId;
                    pokerLinkSetting = "Customer/PT/Poker/AgentUpdate.aspx?custId=" + custId;
                    downlineListURL = "Customer/MemberList.aspx?agentId=" + custId;
                    addDownlineURL_New = customerInfoViewModel.isDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMember/AddNewMemberPage?agentId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.customerSite;
                    addDownlineURL = customerInfoViewModel.isDeposit ? "DepositCustomer/AddNewAgent" : "Customer/v2/AddNewMember.aspx?set=1&agentId=" + custId + "&username=" + userName;
                    detailUrl = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingAgent" : "Customer/v2/EditAgent.aspx?set=1&custId=" + custId + "&masterId=" + _masterID.ToString() + "&uname=" + userName;
                    detailUrlNew = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingAgent" : "EditAgent/EditAgentPage?" + "custId=" + custId + "&masterId=" + _masterID.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.customerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Agent";
                    liveCasinoSetting = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&parentId={1}&uplineLevel=3&actorId={2}&actorUsername={3}&customerId={4}&customerLevel=2", _masterID, _masterID, AppSession.AccId, AppSession.AccUserName, custId);
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, _masterID, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditAgent?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Agent");
                    if (!CustomerUtils.IsTransferDaily(Convert.ToString(_masterID))) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
                    casinoLinkSetting = "CustomerSetting/Casino/Agent/EditAgent/" + custId + "/" + _masterID;
                    goldDeluxeLinkSetting = string.Format("GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId={0}&uplineLevel=3&actorId={1}&custId={2}&level=2&formType=1", _masterID, AppSession.AccId, custId);
                    colossusBetsLinkSetting = string.Format("customersetting/ColossusBetSetting/ColossusBetSetting/EditSetting?custid={0}&mrecommend={1}&roleid=2&UserId={2}", custId, _masterID, AppSession.AccId);
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=2&customerId=" + custId;
                    break;

                case 3: // master
                    int _superID = CastAide.AsInt32(row2["superId"], -1);
                    RacingUpdateURL = string.Format("EditMasterPTRacing/Index?custid={0}&uplineid={1}", custId, _superID);
                    bingoLinkSetting = "Customer/PT/Bingo/MasterUpdate.aspx?custId=" + custId;
                    pokerLinkSetting = "Customer/PT/Poker/MasterUpdate.aspx?custId=" + custId;
                    detailUrl = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingAgent" : "Customer/v2/EditMaster.aspx?set=1&custId=" + custId + "&superId=" + _superID.ToString() + "&uname=" + userName;
                    detailUrlNew = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingAgent" : "EditMaster/EditMasterPage?custId=" + custId + "&superId=" + _superID.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.customerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Master";
                    addDownlineURL_New = customerInfoViewModel.isDeposit ? "DepositCustomer/AddNewAgent" : "AddNewAgent/AddNewAgentPage?masterId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.customerSite;
                    addDownlineURL = customerInfoViewModel.isDeposit ? "DepositCustomer/AddNewAgent" : "Customer/v2/AddNewAgent.aspx?set=1&masterId=" + custId + "&username=" + userName;
                    downlineListURL = "Customer/AgentList.aspx?masterId=" + custId;
                    liveCasinoSetting = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&uplineLevel=4&actorId={2}&customerId={4}&customerLevel=3", _superID, _superID, AppSession.AccId, AppSession.AccUserName, custId);
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, _superID, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditMaster?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Master");
                    casinoLinkSetting = "CustomerSetting/Casino/Master/EditMaster/" + custId + "/" + _superID;
                    goldDeluxeLinkSetting = string.Format("GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId={0}&upLineLevel=4&actorId={1}&custId={2}&level=3&formType=1", _superID, AppSession.AccId, custId);
                    colossusBetsLinkSetting = string.Format("customersetting/ColossusBetSetting/ColossusBetSetting/EditSetting?custid={0}&srecommend={1}&roleid=3&UserId={2}", custId, _superID, AppSession.AccId);
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=3&customerId=" + custId;
                    break;

                case 4: // super
                    RacingUpdateURL = string.Format("EditSuperPTRacing/Index?custid={0}", custId);
                    bingoLinkSetting = "Customer/PT/Bingo/SuperUpdate.aspx?custId=" + custId;
                    pokerLinkSetting = "Customer/PT/Poker/SuperUpdate.aspx?custId=" + custId;
                    addDownlineURL_New = customerInfoViewModel.isDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMaster/AddNewMasterPage?superId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.customerSite;
                    addDownlineURL = customerInfoViewModel.isDeposit ? "DepositCustomer/AddNewAgent" : "Customer/V2/AddNewMaster.aspx?set=1&superId=" + custId + "&username=" + userName;
                    downlineListURL = "Customer/MasterAgentList.aspx?superId=" + custId;
                    detailUrl = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingSuper" : "Customer/v2/AddNewSuper.aspx?set=1&custId=" + custId + "&uname=" + userName;
                    detailUrlNew = customerInfoViewModel.isDeposit ? "DepositCustomer/EditSettingSuper" : "EditSuper/EditSuperPage?custId=" + custId + "&uname=" + userName + "&siteName=" + customerInfoViewModel.customerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    liveCasinoSetting = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId=0&uplineLevel=0&actorId={0}&actorUsername={1}&customerId={2}&customerLevel=4", AppSession.AccId, AppSession.AccUserName, custId);
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}&exchangeId={3}", custId, 0, roleId, CastAide.AsInt32(row1["exchangeid"]));
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditSuper?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Super");
                    if (custId < 100) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime"); // for dummy super
                    casinoLinkSetting = "CustomerSetting/Casino/Super/EditSuper/" + custId + "/0";
                    goldDeluxeLinkSetting = string.Format("GoldDeluxeSetting/GoldDeluxe/EditSetting?upLineId=0&upLineLevel=0&actorId={0}&custId={1}&level=4&formType=1", AppSession.AccId, custId);
                    colossusBetsLinkSetting = string.Format("customersetting/ColossusBetSetting/ColossusBetSetting/EditSetting?custId={0}&roleid=4&UserId={1}", custId, AppSession.AccId);
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=4&customerId=" + custId;
                    break;

                default:
                    break;
            }

            RenderLeftPanelManager.RenderEditMicroGamingLink(ref htmlcontent, renderLeftPanelParameter);
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

            if (customerInfoViewModel.isDeposit)
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
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Keno");
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-KenoLottery");
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Lucky3");
            }

            TemplateHandler.Assign(ref htmlcontent, "{RacingUpdateURL}", RacingUpdateURL);
            TemplateHandler.Assign(ref htmlcontent, "{linkSettingInfo}", infoUrl);
            TemplateHandler.Assign(ref htmlcontent, "{bingoLinkSetting}", bingoLinkSetting);
            TemplateHandler.Assign(ref htmlcontent, "{pokerLinkSetting}", pokerLinkSetting + "&roleId=" + roleId.ToString());
            TemplateHandler.Assign(ref htmlcontent, "{livecasinoLinkSetting}", liveCasinoSetting);
            TemplateHandler.Assign(ref htmlcontent, "{virtualSportLinkSetting}", string.Format(creditDCSLink, 5));
            TemplateHandler.Assign(ref htmlcontent, "{kenoLinkSetting}", kenoSetting);
            TemplateHandler.Assign(ref htmlcontent, "{casinoLinkSetting}", casinoLinkSetting);
            TemplateHandler.Assign(ref htmlcontent, "{goldDeluxeLinkSetting}", goldDeluxeLinkSetting);
            TemplateHandler.Assign(ref htmlcontent, "{colossusBetLinkSetting}", colossusBetsLinkSetting);
            TemplateHandler.Assign(ref htmlcontent, "{sportsbook2LinkSetting}", string.Format(creditDCSLink, 16));
            TemplateHandler.Assign(ref htmlcontent, "{cockFightingSettingLink}", string.Format(creditDCSLink, 19));
            TemplateHandler.Assign(ref htmlcontent, "{cricketSettingLink}", string.Format(creditDCSLink, 20));
            TemplateHandler.Assign(ref htmlcontent, "{allbetLinkSetting}", string.Format(creditDCSLink, 21));
            TemplateHandler.Assign(ref htmlcontent, "{voidbridgeLinkSetting}", string.Format(creditDCSLink, 22));
            TemplateHandler.Assign(ref htmlcontent, "{ng2LinkSetting}", string.Format(creditDCSLink, 23));
            TemplateHandler.Assign(ref htmlcontent, "{fishingWorldLinkSetting}", string.Format(creditDCSLink, 24));
            TemplateHandler.Assign(ref htmlcontent, "{Edit-NG2}", "Edit " + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2"));
            TemplateHandler.Assign(ref htmlcontent, "{kenoLotteryLinkSetting}", string.Format(creditDCSLink, Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery));
            TemplateHandler.Assign(ref htmlcontent, "{lucky3LinkSetting}", string.Format(creditDCSLink, 26));

            if (!WebSettings.EnableAGCasino /*|| roleId != 4*/ || !customerInfoViewModel.isLicensee)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-agcasino");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{agcasinoLinkSetting}", agCasinoLinkSetting);
            }

            string bettingSetting = WebSettings.ExternalBettingLimitConfig;
            //AppSession.GroupID
            try
            {
                if (!String.IsNullOrEmpty(bettingSetting) && bettingSetting.Contains("|"))
                {
                    var groupId = bettingSetting.Split(new char[] { '|' })[0].Split(new char[] { ',' });
                    if (!groupId.Contains(AppSession.GroupID.ToString()) && roleId == 4)
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
            }
            catch
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-BetSetting");
            }



            //---- for menu details -----------
            htmlcontent = AssignEditFinance(htmlcontent, roleId);
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
                if ((roleId < (int)Globals.AccountType.Super) && !customerInfoViewModel.isAllowEditCS)
                    TemplateHandler.RemoveBlock(ref htmlcontent, "edit-casino");
            }

            //Removed check allow Current for HR
            if (WebSettings.disabledHR) //|| !AccCommon.IsAllowCurrencyHR(currency))
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "horseracing-editlink");
            }
            //---------------------------------
            if (!BingoSettings.BingoGame.Enabled || !customerInfoViewModel.isAllowEditBI)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-bingo");
            }
            if (!WebSettings.EnableLiveCasino || !customerInfoViewModel.isAllowEditLCS)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-LiveCasino");
            }
            if (customerInfoViewModel.isHideVirtualSports)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-VirtualSport");
            }

            bool isShowSetting = KenoSection.Instance.ShowSetting;
            if (!isShowSetting)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Keno");
            }


            if (!WebSettings.IsShowGoldDeluxeSettingForCredit || customerInfoViewModel.isDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-GoldDeluxe");
            }

            if (customerInfoViewModel.isHideColossusBets || customerInfoViewModel.isDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-ColossusBet");
            }

            if (customerInfoViewModel.isHideSportsbook2 || customerInfoViewModel.isDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-sportsbook2");
            }

            if (customerInfoViewModel.isHideCockFighting)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-CockFighting");
            }

            if (customerInfoViewModel.isHideCricketSettingLink)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Cricket");
            }

            if (customerInfoViewModel.isHideAllbetUpline || !customerInfoViewModel.isSyncAllbetUpline)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Allbet");
            }

            if (customerInfoViewModel.isHideVoidbridgeUpline || !customerInfoViewModel.isSyncVoidbridgeUpline)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Voidbridge");
            }

            if (customerInfoViewModel.isHideNG2Upline || customerInfoViewModel.isHideNG2 || !customerInfoViewModel.isSyncNG2Upline)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-NG2");
            }

            if (customerInfoViewModel.hideEnableAndEditFishingWorld)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-FishingWorld");
            }

            if (customerInfoViewModel.hideKenoLottery)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-KenoLottery");
            }

            if (customerInfoViewModel.isHideLucky3Upline || customerInfoViewModel.isHideLucky3)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Lucky3");
            }

            TemplateHandler.Assign(ref htmlcontent, "{jsparams}", AssignJS(customerInfoViewModel.jsparam));

            var p2pSetting = PokerSection.Instance[PokerSection.PokerCollections.CustomerSetting]["visibleP2PSetting"];
            bool visibleP2PSetting = CastAide.AsBoolean(p2pSetting.Value);
            if (!visibleP2PSetting)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-p2p");
            }

            return htmlcontent;
        }
    }
}
