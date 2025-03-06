namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using DNAide.Web;
    using Fanex.Tarus.Infrastructure.Constants;

    public partial class CustomerManager
    {
        private string RenderDetailMenu(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel,
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            htmlcontent = GenMenuMinMaxPT(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);

            htmlcontent = AssignSettingBlock(htmlcontent, customerInfoViewModel);

            htmlcontent = RemoveSettingBlock(htmlcontent, customerInfoViewModel);

            if (customerInfoViewModel.IsDeposit)
            {
                htmlcontent = RemoveUnsupportedSettingBlockDeposit(htmlcontent);
            }

            return htmlcontent;
        }

        private string AssignSettingBlock(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel)
        {
            string userName = customerInfoViewModel.UserName;
            int parentId = customerInfoViewModel.ParentId;
            int roleId = customerInfoViewModel.SearchAccountRoleId;
            int custId = customerInfoViewModel.CustId;
            var superId = customerInfoViewModel.SuperId;
            var masterId = customerInfoViewModel.MasterId;
            var agentId = customerInfoViewModel.AgentId;

            string infoUrl = string.Empty;
            string downlineListURL = string.Empty;
            string addDownlineURL = string.Empty;
            string addNewDirectMemberUrl = string.Empty;

            var agCasinoLinkSetting = string.Empty;

            switch (roleId)
            {
                case 1: // member
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, parentId, roleId);
                    TemplateHandler.Assign(ref htmlcontent, "{IsHidden1}", "hidden");
                    TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "");
                    break;

                case 2: // agent
                    downlineListURL = "Customer/MemberList.aspx?agentId=" + custId;
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "Customer/CreateCustomer?theme=nap&uplineId=" + custId + "&uplineRoleId=2&UplineUsername=" + userName;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Agent";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, masterId, roleId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Agent");
                    if (!CustomerUtils.IsTransferDaily(Convert.ToString(masterId))) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime");
                    break;

                case 3: // master
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Master";
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "Customer/CreateCustomer?theme=nap&uplineId=" + custId + "&uplineRoleId=3&UplineUsername=" + userName;
                    downlineListURL = "Customer/AgentList.aspx?masterId=" + custId;
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}", custId, superId, roleId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Master");
                    addNewDirectMemberUrl = "Customer/CreateDirectMember?theme=nap&uplineId=" + custId + "&uplineRoleId=3&agentId=-3&UplineUsername=" + userName;
                    break;

                case 4: // super
                    addDownlineURL = customerInfoViewModel.IsDeposit ? "DepositCustomer/AddNewAgent" : "Customer/CreateCustomer?theme=nap&uplineId=" + custId + "&uplineRoleId=4&UplineUsername=" + userName;
                    downlineListURL = "Customer/MasterAgentList.aspx?superId=" + custId;
                    infoUrl = "Customer/EditCustomerInfo.aspx?custId=" + custId + "&type=Super";
                    agCasinoLinkSetting = string.Format("AGCasino/EditAGCasinoGameSetting?customerId={0}&uplineId={1}&roleId={2}&exchangeId={3}", custId, 0, roleId, customerInfoViewModel.ExchangeId);
                    TemplateHandler.Assign(ref htmlcontent, "{type}", "Super");
                    if (custId < 100) TemplateHandler.RemoveBlock(ref htmlcontent, "editTransferTime"); // for dummy super
                    addNewDirectMemberUrl = "Customer/CreateDirectMember?theme=nap&uplineId=" + custId + "&uplineRoleId=4&agentId=-4&UplineUsername=" + userName;
                    break;

                default:
                    break;
            }

            TemplateHandler.Assign(ref htmlcontent, "{linkSettingInfo}", infoUrl);
            TemplateHandler.Assign(ref htmlcontent, "{liveAndRNGCasinoLinkSetting}", BuildDetailUrl(customerInfoViewModel.IsDeposit, custId, roleId, userName, customerInfoViewModel.CustomerSite, ProductGroupIdConstants.LiveAndRNGCasino, agentId));
            TemplateHandler.Assign(ref htmlcontent, "{rngGamesLinkSetting}", BuildDetailUrl(customerInfoViewModel.IsDeposit, custId, roleId, userName, customerInfoViewModel.CustomerSite, ProductGroupIdConstants.RNGGames, agentId));
            TemplateHandler.Assign(ref htmlcontent, "{agcasinoLinkSetting}", agCasinoLinkSetting);

            //---- for menu details -----------
            TemplateHandler.Assign(ref htmlcontent, "{linkSettingDetail}", BuildDetailUrl(customerInfoViewModel.IsDeposit, custId, roleId, userName, customerInfoViewModel.CustomerSite, ProductGroupIdConstants.SportsBook, agentId));
            TemplateHandler.Assign(ref htmlcontent, "{linkDownlineList}", downlineListURL);
            TemplateHandler.Assign(ref htmlcontent, "{linkAddnewDownline}", addDownlineURL);
            TemplateHandler.Assign(ref htmlcontent, "{linkAddNewDirectMember}", addNewDirectMemberUrl);
            TemplateHandler.Assign(ref htmlcontent, "{IsShow1}", "hidden");

            return htmlcontent;
        }

        private string BuildDetailUrl(bool isDeposit, int custId, int roleId, string userName, string site, int productGroupId, int agentId)
        {
            var uplineRoleId = roleId + 1;
            if (agentId == -4)
            {
                uplineRoleId = RoleIdConstants.Super;
            }
            else if (agentId == -3)
            {
                uplineRoleId = RoleIdConstants.Master;
            }

            return isDeposit
                ? "DepositCustomer/EditSettingAgent"
                : $"Customer/EditCustomer?theme=nap&uplineRoleId={uplineRoleId}&custId={custId}&roleId={roleId}&uname={userName}&siteName={site}&productGroupId={productGroupId}&agentId={agentId}";
        }

        private string RemoveSettingBlock(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            if (!customerInfoViewModel.IsAllowAddDirectMember)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "addNewDirectMember");
            }

            if (customerInfoViewModel.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "addNew-Deposite");
            }

            if (!customerInfoViewModel.ShowSettingLinkAGCasinoDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-agcasino");
            }

            if (customerInfoViewModel.HideLiveAndRNGCasino)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-LiveAndRNGCasnio");
            }

            if (customerInfoViewModel.HideRNGGames)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-RNGGames");
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