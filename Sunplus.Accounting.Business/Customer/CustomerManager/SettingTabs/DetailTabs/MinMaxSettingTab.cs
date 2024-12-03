namespace Sunplus.Accounting.Business.Customer
{
    using DNAide.Web;
    using Fanex.Tarus.Infrastructure.Constants;

    public partial class CustomerManager
    {
        private string GenMenuMinMaxPT(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel,
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (customerInfoViewModel.SearchAccountRoleId == RoleIdConstants.Member)
            {
                htmlcontent = AssignMemberSetting(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "none");
                TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", string.Empty);
                TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", string.Empty);
            }

            return htmlcontent;
        }

        private string AssignMemberSetting(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel,
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            int custId = customerInfoViewModel.CustId;
            string userName = customerInfoViewModel.UserName;
            bool isDeposit = customerInfoViewModel.IsDeposit;
            string siteName = customerInfoViewModel.CustomerSite;

            TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "inline");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? string.Empty : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Super PT</span></a></li>");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", isDeposit ? string.Empty : "<a href=\"javascript:void(0);\"><span>Master PT</span></a>");

            string superSettingTab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Super, userName, siteName, ProductGroupIdConstants.SportsBook) + "\");' target='centerFr'>Sportsbook</a>";

            if (!customerInfoViewModel.HideLiveAndRNGCasino)
            {
                superSettingTab += string.Format(
                   "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Super, userName, siteName, ProductGroupIdConstants.LiveAndRNGCasino) + "\");' target='centerFr'>{0}</a>",
                   "Live & RNG Casino");
            }

            if (!customerInfoViewModel.HideRNGGames)
            {
                superSettingTab += string.Format(
                   "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Super, userName, siteName, ProductGroupIdConstants.RNGGames) + "\");' target='centerFr'>{0}</a>",
                   "RNG Games");
            }

            string masterSettingTab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Master, userName, siteName, ProductGroupIdConstants.SportsBook) + "\");' target='centerFr'>Sportsbook</a>";

            if (!customerInfoViewModel.HideLiveAndRNGCasino)
            {
                masterSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Master, userName, siteName, ProductGroupIdConstants.LiveAndRNGCasino) + "\");' target='centerFr'>{0}</a>",
                    "Live & RNG Casino");
            }

            if (!customerInfoViewModel.HideRNGGames)
            {
                masterSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Master, userName, siteName, ProductGroupIdConstants.RNGGames) + "\");' target='centerFr'>{0}</a>",
                    "RNG Games");
            }

            TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", superSettingTab);
            TemplateHandler.Assign(ref htmlcontent, "{contentminmax1}", masterSettingTab);
            return htmlcontent;
        }

        private string BuildSettingUrl(int custId, int roleId, int uplineRoleId, string userName, string siteName, int productGroupId)
        {
            return $"Customer/EditCustomer?roleId={roleId}&uplineRoleId={uplineRoleId}&custId={custId}&uname={userName}&siteName={siteName}&productGroupId={productGroupId}";
        }
    }
}