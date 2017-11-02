namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using Core.Common;
    using DNAide.Web;
    using RemotingRef;

    public partial class CustomerManager
    {
        private static string RenderDetailMenu(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel, 
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            htmlcontent = GenMenuMinMaxPT(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);

            htmlcontent = AssignSettingBlock(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);

            htmlcontent = RemoveSettingBlock(htmlcontent, customerInfoViewModel);

            if (customerInfoViewModel.IsDeposit)
            {
                htmlcontent = RemoveUnsupportedSettingBlockDeposit(htmlcontent);
            }

            return htmlcontent;
        }

        private static string AssignSettingBlock(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel, 
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            string userName = customerInfoViewModel.UserName;
            int parentId = customerInfoViewModel.ParentId;
            int roleId = customerInfoViewModel.RoleId;
            int custId = customerInfoViewModel.CustId;
            var agentId = customerInfoViewModel.AgentId;
            var superId = customerInfoViewModel.SuperId;
            var masterId = customerInfoViewModel.MasterId;

            string infoUrl = string.Empty;
            string detailUrl = string.Empty;
            string downlineListURL = string.Empty;
            string addDownlineURL = string.Empty;

            var liveCasinoSetting = string.Empty;
            var kenoSetting = string.Empty;

            var agCasinoLinkSetting = string.Empty;
            var casinoLinkSetting = string.Empty;

            var goldDeluxeLinkSetting = string.Empty;
            var colossusBetsLinkSetting = string.Empty;
            var creditDCSLink = string.Empty;
            string RacingUpdateURL = string.Empty;

            switch (roleId)
            {
                case 1: // member
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditMember/EditMemberPage?custId=" + custId + "&agentId=" + agentId + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
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
                    RacingUpdateURL = string.Format("EditAgentPTRacing/Index?custid={0}&uplineid={1}", custId, masterId);
                    downlineListURL = "Customer/MemberList.aspx?agentId=" + custId;
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMember/AddNewMemberPage?agentId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditAgent/EditAgentPage?" + "custId=" + custId + "&masterId=" + masterId.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Agent";
                    liveCasinoSetting = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&parentId={1}&uplineLevel=3&actorId={2}&actorUsername={3}&customerId={4}&customerLevel=2", masterId, masterId, AppSession.AccId, AppSession.AccUserName, custId);
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, masterId, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditAgent?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Agent");
                    if (!CustomerUtils.IsTransferDaily(Convert.ToString(masterId))) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
                    casinoLinkSetting = "CustomerSetting/Casino/Agent/EditAgent/" + custId + "/" + masterId;
                    goldDeluxeLinkSetting = string.Format("GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId={0}&uplineLevel=3&actorId={1}&custId={2}&level=2&formType=1", masterId, AppSession.AccId, custId);
                    colossusBetsLinkSetting = string.Format("customersetting/ColossusBetSetting/ColossusBetSetting/EditSetting?custid={0}&mrecommend={1}&roleid=2&UserId={2}", custId, masterId, AppSession.AccId);
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=2&customerId=" + custId;
                    break;

                case 3: // master
                    RacingUpdateURL = string.Format("EditMasterPTRacing/Index?custid={0}&uplineid={1}", custId, superId);
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditMaster/EditMasterPage?custId=" + custId + "&superId=" + superId.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Master";
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "AddNewAgent/AddNewAgentPage?masterId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    downlineListURL = "Customer/AgentList.aspx?masterId=" + custId;
                    liveCasinoSetting = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&uplineLevel=4&actorId={2}&customerId={4}&customerLevel=3", superId, superId, AppSession.AccId, AppSession.AccUserName, custId);
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, superId, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditMaster?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Master");
                    casinoLinkSetting = "CustomerSetting/Casino/Master/EditMaster/" + custId + "/" + superId;
                    goldDeluxeLinkSetting = string.Format("GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId={0}&upLineLevel=4&actorId={1}&custId={2}&level=3&formType=1", superId, AppSession.AccId, custId);
                    colossusBetsLinkSetting = string.Format("customersetting/ColossusBetSetting/ColossusBetSetting/EditSetting?custid={0}&srecommend={1}&roleid=3&UserId={2}", custId, superId, AppSession.AccId);
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=3&customerId=" + custId;
                    break;

                case 4: // super
                    RacingUpdateURL = string.Format("EditSuperPTRacing/Index?custid={0}", custId);
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMaster/AddNewMasterPage?superId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    downlineListURL = "Customer/MasterAgentList.aspx?superId=" + custId;
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingSuper" : "EditSuper/EditSuperPage?custId=" + custId + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    liveCasinoSetting = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId=0&uplineLevel=0&actorId={0}&actorUsername={1}&customerId={2}&customerLevel=4", AppSession.AccId, AppSession.AccUserName, custId);
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}&exchangeId={3}", custId, 0, roleId, customerInfoViewModel.ExchangeId);
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

            TemplateHandler.Assign(ref htmlcontent, "{RacingUpdateURL}", RacingUpdateURL);
            TemplateHandler.Assign(ref htmlcontent, "{linkSettingInfo}", infoUrl);
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
            TemplateHandler.Assign(ref htmlcontent, "{agcasinoLinkSetting}", agCasinoLinkSetting);

            var editSettingLink = $"MicroGaming/EditSettingView?parentId={renderLeftPanelParameter.ParentId}&custId={renderLeftPanelParameter.CustId}&roleId={renderLeftPanelParameter.RoleId}";
            TemplateHandler.Assign(ref htmlcontent, "{microgamingLinkSetting}", editSettingLink);

            //---- for menu details -----------
            TemplateHandler.Assign(ref htmlcontent, "{linkSettingDetail}", detailUrl);
            TemplateHandler.Assign(ref htmlcontent, "{linkDownlineList}", downlineListURL);
            TemplateHandler.Assign(ref htmlcontent, "{linkAddnewDownline}", addDownlineURL);
            TemplateHandler.Assign(ref htmlcontent, "{ParentId}", parentId.ToString());
            TemplateHandler.Assign(ref htmlcontent, "{RoleId}", roleId.ToString());
            TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "hidden");
            TemplateHandler.Assign(ref htmlcontent, "{CustId}", custId);
            TemplateHandler.Assign(ref htmlcontent, "{Username}", customerInfoViewModel.UserName);
            TemplateHandler.Assign(ref htmlcontent, "{jsparams}", AssignJS(customerInfoViewModel.JsParam));

            return htmlcontent;
        }

        private static string RemoveSettingBlock(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.HideSettingLinkRnGCasino)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-casino");
            }

            if (!customerInfoViewModel.ShowHorseRacingLinkCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "horseracing-editlink");
            }

            if (!customerInfoViewModel.ShowMicrogamingSettingLinkDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-MicroGaming");
            }

            if (!customerInfoViewModel.ShowSettingLinkLiveCasino)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-LiveCasino");
            }

            if (customerInfoViewModel.HideVirtualSports)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-VirtualSport");
            }

            if (!customerInfoViewModel.ShowSettingLinkKeno)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Keno");
            }

            if (!customerInfoViewModel.ShowSettingLinkGoldDeluxeCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-GoldDeluxe");
            }

            if (customerInfoViewModel.HideSettingLinkColossusBetsCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-ColossusBet");
            }

            if (!customerInfoViewModel.ShowSettingLinkAGCasinoDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-agcasino");
            }

            if (customerInfoViewModel.HideSportsbook2)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-sportsbook2");
            }

            if (customerInfoViewModel.HideCricketCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Cricket");
            }

            if (customerInfoViewModel.HideSettingLinkAllbet)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Allbet");
            }

            if (customerInfoViewModel.HideSettingLinkVoidbridge)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Voidbridge");
            }

            if (customerInfoViewModel.HideSettingLinkNG2Credit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-NG2");
            }

            if (customerInfoViewModel.HideEnableAndEditFishingWorld)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-FishingWorld");
            }

            if (customerInfoViewModel.HideKenoLottery)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-KenoLottery");
            }

            if (customerInfoViewModel.HideSettingLinkLucky3Credit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Lucky3");
            }

            return htmlcontent;
        }

        private static string RemoveUnsupportedSettingBlockDeposit(string htmlcontent)
        {
            TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
            TemplateHandler.RemoveBlock(ref htmlcontent, "maxexposure");
            TemplateHandler.RemoveBlock(ref htmlcontent, "detail_edit_menu");
            return htmlcontent;
        }
    }
}
