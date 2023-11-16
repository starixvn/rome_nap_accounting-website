namespace Sunplus.Accounting.Business.Customer
{
    using System;
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

            var agCasinoLinkSetting = string.Empty;

            var creditDCSLink = string.Empty;

            switch (roleId)
            {
                case 0:
                    break;

                case 1: // member
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditMember/EditMemberPage?custId=" + custId + "&agentId=" + agentId + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, parentId, roleId);
                    TemplateHandler.Assign(ref htmlcontent, "{IsHidden1}", "hidden");
                    TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "");
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=1&uplinelevel=2&customerId=" + custId;
                    break;

                case 2: // agent
                    downlineListURL = "Customer/MemberList.aspx?agentId=" + custId;
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "Customer/CreateCustomer?theme=nap&uplineId=" + custId + "&uplineRoleId=2" + "&uplineUsername=" + userName;
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditAgent/EditAgentPage?" + "custId=" + custId + "&masterId=" + masterId.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Agent";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, masterId, roleId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Agent");
                    if (!CustomerUtils.IsTransferDaily(Convert.ToString(masterId))) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=2&customerId=" + custId;
                    break;

                case 3: // master
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingAgent" : "EditMaster/EditMasterPage?custId=" + custId + "&superId=" + superId.ToString() + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Master";
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "Customer/CreateCustomer?theme=nap&uplineId=" + custId + "&uplineRoleId=3" + "&uplineUsername=" + userName;
                    downlineListURL = "Customer/AgentList.aspx?masterId=" + custId;
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, superId, roleId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Master");
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=3&customerId=" + custId;
                    break;

                case 4: // super
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "Customer/CreateCustomer?theme=nap&uplineId=" + custId + "&uplineRoleId=4" + "&uplineUsername=" + userName;
                    downlineListURL = "Customer/MasterAgentList.aspx?superId=" + custId;
                    detailUrl = customerInfoViewModel.IsDeposit ? "DepositCustomer/EditSettingSuper" : "EditSuper/EditSuperPage?custId=" + custId + "&uname=" + userName + "&siteName=" + customerInfoViewModel.CustomerSite;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}&exchangeId={3}", custId, 0, roleId, customerInfoViewModel.ExchangeId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Super");
                    if (custId < 100) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime"); // for dummy super
                    creditDCSLink = "DCS/ViewCustomerSetting?productId={0}&language=en-US&customerLevel=4&customerId=" + custId;
                    break;

                default:
                    break;
            }

            TemplateHandler.Assign(ref htmlcontent, "{linkSettingInfo}", infoUrl);
            TemplateHandler.Assign(ref htmlcontent, "{virtualSportLinkSetting}", string.Format(creditDCSLink, 5));
            TemplateHandler.Assign(ref htmlcontent, "{sportsbook2LinkSetting}", string.Format(creditDCSLink, 16));
            TemplateHandler.Assign(ref htmlcontent, "{liveCasinoLinkSetting}", string.Format(creditDCSLink, 41));
            TemplateHandler.Assign(ref htmlcontent, "{voidbridgeLinkSetting}", string.Format(creditDCSLink, 18));
            TemplateHandler.Assign(ref htmlcontent, "{rngLotteryLinkSetting}", string.Format(creditDCSLink, Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery));
            TemplateHandler.Assign(ref htmlcontent, "{virtualGamesLinkSetting}", string.Format(creditDCSLink, Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdVirtualGames));
            TemplateHandler.Assign(ref htmlcontent, "{BitcoinLinkSetting}", string.Format(creditDCSLink, 69));
            TemplateHandler.Assign(ref htmlcontent, "{sabaclubLinkSetting}", string.Format(creditDCSLink, 42));
            TemplateHandler.Assign(ref htmlcontent, "{agcasinoLinkSetting}", agCasinoLinkSetting);
            TemplateHandler.Assign(ref htmlcontent, "{vgamingLinkSetting}", string.Format(creditDCSLink, 66));     
            TemplateHandler.Assign(ref htmlcontent, "{SabaCasinoLinkSetting}", string.Format(creditDCSLink, 6));

            //---- for menu details -----------
            TemplateHandler.Assign(ref htmlcontent, "{linkSettingDetail}", detailUrl);
            TemplateHandler.Assign(ref htmlcontent, "{linkDownlineList}", downlineListURL);
            TemplateHandler.Assign(ref htmlcontent, "{linkAddnewDownline}", addDownlineURL); 
            TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "hidden");

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

            return htmlcontent;
        }
    }
}