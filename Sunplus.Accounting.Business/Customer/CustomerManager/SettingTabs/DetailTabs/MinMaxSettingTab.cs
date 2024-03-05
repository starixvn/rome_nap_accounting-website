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

            if (!customerInfoViewModel.HideSettingLinkVirtualSports)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Virtual Sports</a>";
            }

            if (!customerInfoViewModel.HideLiveAndRNGCasino)
            {
                superSettingTab += string.Format(
                   "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Super, userName, siteName, ProductGroupIdConstants.LiveAndRNGCasino) + "\");' target='centerFr'>{3}</a>",
                   Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery,
                   custId,
                   4, // Super Level.
                   "Live & RNG Casino");
            }

            if (renderLeftPanelParameter.IsShowRNGLottery)
            {
                superSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery,
                    custId,
                    4, // Super Level.
                    "RNG Lottery");
            }

            if (renderLeftPanelParameter.IsShowBitcoin)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=69&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Bitcoin</a>";
            }

            string masterSettingTab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Master, userName, siteName, ProductGroupIdConstants.SportsBook) + "\");' target='centerFr'>Sportsbook</a>";

            if (!customerInfoViewModel.HideSettingLinkVirtualSports)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Virtual Sports</a>";
            }

            if (!customerInfoViewModel.HideLiveAndRNGCasino)
            {
                masterSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + BuildSettingUrl(custId, RoleIdConstants.Member, RoleIdConstants.Master, userName, siteName, ProductGroupIdConstants.LiveAndRNGCasino) + "\");' target='centerFr'>{3}</a>",
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery,
                    custId,
                    3, // Master Level.
                    "Live & RNG Casino");
            }

            if (renderLeftPanelParameter.IsShowRNGLottery)
            {
                masterSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery,
                    custId,
                    3, // Master Level.
                    "RNG Lottery");
            }

            if (renderLeftPanelParameter.IsShowBitcoin)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=69&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Bitcoin</a>";
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