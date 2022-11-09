namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using Core.Common;
    using DNAide.Web;

    public partial class CustomerManager
    {
        private string RenderDetailMenu(
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

        private string AssignSettingBlock(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel,
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            string userName = customerInfoViewModel.UserName;
            int parentId = customerInfoViewModel.ParentId;
            int roleId = customerInfoViewModel.SearchAccountRoleId;
            int custId = customerInfoViewModel.CustId;
            var agentId = customerInfoViewModel.AgentId;
            var superId = customerInfoViewModel.SuperId;
            var masterId = customerInfoViewModel.MasterId;

            string infoUrl = string.Empty;
            string detailUrl = string.Empty;
            string downlineListURL = string.Empty;
            string addDownlineURL = string.Empty;

            var kenoSetting = string.Empty;

            var agCasinoLinkSetting = string.Empty;

            var creditDCSLink = string.Empty;
            var cn88EditSettingLink = string.Empty;
            var cn88AddnewDownlineLink = string.Empty;

            switch (roleId)
            {
                case 0: // CN88 Headline
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Headline";
                    break;
                case 1: // member
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditMember/EditMemberPage?custId=" + custId + "&agentId=" + agentId + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, parentId, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/AgentEditMember?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{IsHidden1}", "hidden");
                    TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "");
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=1&uplinelevel=2&customerId=" + custId;
                    cn88EditSettingLink = $"CN88CustomerSetting/ViewCN88CustomerSetting/Edit?language=en-US&productId=1&customerLevel=1&uplineId={parentId}&customerId={custId}";
                    break;

                case 2: // agent
                    downlineListURL = "Customer/MemberList.aspx?agentId=" + custId;
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMember/AddNewMemberPage?agentId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditAgent/EditAgentPage?" + "custId=" + custId + "&masterId=" + masterId.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Agent";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, masterId, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditAgent?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Agent");
                    if (!CustomerUtils.IsTransferDaily(Convert.ToString(masterId))) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=2&customerId=" + custId;
                    cn88EditSettingLink = $"CN88CustomerSetting/ViewCN88CustomerSetting/Edit?language=en-US&productId=1&customerLevel=2&uplineId={parentId}&customerId={custId}";
                    cn88AddnewDownlineLink = $"CN88CustomerSetting/ViewCN88CustomerSetting/Create?language=en-US&productId=1&customerLevel=1&uplineId={custId}";
                    break;

                case 3: // master
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditMaster/EditMasterPage?custId=" + custId + "&superId=" + superId.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Master";
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "AddNewAgent/AddNewAgentPage?masterId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    downlineListURL = "Customer/AgentList.aspx?masterId=" + custId;
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, superId, roleId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditMaster?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Master");
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=3&customerId=" + custId;
                    cn88EditSettingLink = $"CN88CustomerSetting/ViewCN88CustomerSetting/Edit?language=en-US&productId=1&customerLevel=3&uplineId={parentId}&customerId={custId}";
                    cn88AddnewDownlineLink = $"CN88CustomerSetting/ViewCN88CustomerSetting/Create?language=en-US&productId=1&customerLevel=2&uplineId={custId}";
                    break;

                case 4: // super
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "AddNewMaster/AddNewMasterPage?superId=" + custId + "&username=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    downlineListURL = "Customer/MasterAgentList.aspx?superId=" + custId;
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingSuper" : "EditSuper/EditSuperPage?custId=" + custId + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}&exchangeId={3}", custId, 0, roleId, customerInfoViewModel.ExchangeId);
                    kenoSetting = string.Format("customersetting/KenoSetting/kenosetting/EditSuper?custId={0}&userId={1}", custId, AppSession.AccId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Super");
                    if (custId < 100) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime"); // for dummy super
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=4&customerId=" + custId;
                    cn88EditSettingLink = $"CN88CustomerSetting/ViewCN88CustomerSetting/Edit?language=en-US&productId=1&customerLevel=4&customerId={custId}";
                    cn88AddnewDownlineLink = $"CN88CustomerSetting/ViewCN88CustomerSetting/Create?language=en-US&productId=1&customerLevel=3&uplineId={custId}";
                    break;

                default:
                    break;
            }

            TemplateHandler.Assign(ref htmlcontent, "{linkSettingInfo}", infoUrl);
            TemplateHandler.Assign(ref htmlcontent, "{virtualSportLinkSetting}", string.Format(creditDCSLink, 5));
            TemplateHandler.Assign(ref htmlcontent, "{kenoLinkSetting}", kenoSetting);
            TemplateHandler.Assign(ref htmlcontent, "{sportsbook2LinkSetting}", string.Format(creditDCSLink, 16));
            TemplateHandler.Assign(ref htmlcontent, "{liveCasinoLinkSetting}", string.Format(creditDCSLink, 41));
            TemplateHandler.Assign(ref htmlcontent, "{voidbridgeLinkSetting}", string.Format(creditDCSLink, 18));
            TemplateHandler.Assign(ref htmlcontent, "{ng2LinkSetting}", string.Format(creditDCSLink, 23));
            TemplateHandler.Assign(ref htmlcontent, "{fishingWorldLinkSetting}", string.Format(creditDCSLink, 24));
            TemplateHandler.Assign(ref htmlcontent, "{Edit-NG2}", _remotingRefService.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2"));
            TemplateHandler.Assign(ref htmlcontent, "{rngLotteryLinkSetting}", string.Format(creditDCSLink, Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery));
            TemplateHandler.Assign(ref htmlcontent, "{virtualGamesLinkSetting}", string.Format(creditDCSLink, Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdVirtualGames));
            TemplateHandler.Assign(ref htmlcontent, "{BitcoinLinkSetting}", string.Format(creditDCSLink, 69));
            TemplateHandler.Assign(ref htmlcontent, "{sabaclubLinkSetting}", string.Format(creditDCSLink, 42));
            TemplateHandler.Assign(ref htmlcontent, "{agcasinoLinkSetting}", agCasinoLinkSetting);
            TemplateHandler.Assign(ref htmlcontent, "{vgamingLinkSetting}", string.Format(creditDCSLink, 66));
            TemplateHandler.Assign(ref htmlcontent, "{linkEditSettingCN88}", cn88EditSettingLink);
            TemplateHandler.Assign(ref htmlcontent, "{linkAddnewDownlineCN88}", cn88AddnewDownlineLink);
            TemplateHandler.Assign(ref htmlcontent, "{CustomerStatus}", customerInfoViewModel.CN88CustStatusName);
            TemplateHandler.Assign(ref htmlcontent, "{CustomerStatusId}", customerInfoViewModel.CN88CustStatus);                 
            TemplateHandler.Assign(ref htmlcontent, "{SabaCasinoLinkSetting}", string.Format(creditDCSLink, 6));

            //---- for menu details -----------
            TemplateHandler.Assign(ref htmlcontent, "{linkSettingDetail}", detailUrl);
            TemplateHandler.Assign(ref htmlcontent, "{linkDownlineList}", downlineListURL);
            TemplateHandler.Assign(ref htmlcontent, "{linkAddnewDownline}", addDownlineURL);
            TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "hidden");

            //Chatbot
            var chatbotLink = $"Chatbot/Index?custid={custId}&username={userName}";
            TemplateHandler.Assign(ref htmlcontent, "{chatbotLink}", chatbotLink);

            return htmlcontent;
        }

        private string RemoveSettingBlock(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "addNew-Deposite");
            }

            if (customerInfoViewModel.HideSettingLinkVirtualSports)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-VirtualSport");
            }

            if (!customerInfoViewModel.ShowSettingLinkKenoCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Keno");
            }       

            if (!customerInfoViewModel.ShowSettingLinkAGCasinoDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-agcasino");
            }

            if (customerInfoViewModel.HideSportsbook2)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-sportsbook2");
            }           

            if (customerInfoViewModel.HideSettingLinkLiveCasino)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-LiveCasino");
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

            if (customerInfoViewModel.HideRNGLottery)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-RNGLottery");
            }

            if (customerInfoViewModel.HideVirtualGames)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-VirtualGames");
            }

            if (customerInfoViewModel.HideBitcoin)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Bitcoin");
            }

            if (customerInfoViewModel.HideSabaclub)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-Sabaclub");
            }

            if (customerInfoViewModel.HideVGaming)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-VGaming");
            }

            // Chatbot
            if (customerInfoViewModel.IsDeposit || customerInfoViewModel.SearchAccountRoleId == 1)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "chatbot");
            }

            if (customerInfoViewModel.HideSabaCasino)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-sabacasino");
            }

            return htmlcontent;
        }

        private string RemoveUnsupportedSettingBlockDeposit(string htmlcontent)
        {
            TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
            TemplateHandler.RemoveBlock(ref htmlcontent, "maxexposure");
            TemplateHandler.RemoveBlock(ref htmlcontent, "detail_edit_menu");
            TemplateHandler.RemoveBlock(ref htmlcontent, "parlayMaxPayout");
            
            return htmlcontent;
        }
    }
}