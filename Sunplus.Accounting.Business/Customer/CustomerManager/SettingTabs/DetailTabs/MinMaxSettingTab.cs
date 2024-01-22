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

            var agentId = customerInfoViewModel.AgentId;
            TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "inline");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? string.Empty : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Super PT</span></a></li>");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", isDeposit ? string.Empty : "<a href=\"javascript:void(0);\"><span>Master PT</span></a>");

            string superOnMemberSettingUrl = string.Format("Customer/IndirectEditMember?roleId=1&uplineRoleId=4&agentId={0}&custId={1}&uname={2}&siteName={3}", agentId, custId, userName, siteName);
            string masterOnMemberSettingUrl = string.Format("Customer/IndirectEditMember?roleId=1&uplineRoleId=3&agentId={0}&custId={1}&uname={2}&siteName={3}", agentId, custId, userName, siteName);
            string superSettingTab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + superOnMemberSettingUrl + "\");' target='centerFr'>Sportsbook</a>";

            if (renderLeftPanelParameter.IsShowSabaCasino)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=6&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Saba Casino</a>";
            }

            if (!customerInfoViewModel.HideSettingLinkVirtualSports)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Virtual Sports</a>";
            }

            if (renderLeftPanelParameter.IsShowLiveCasinoForCredit)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=41&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Live Casino</a>";
            }

            if (renderLeftPanelParameter.IsShowVoidbridge)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=18&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>RNG Slot</a>";
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

            if (renderLeftPanelParameter.IsShowVirtualGames)
            {
                superSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdVirtualGames,
                    custId,
                    4, // Super Level.
                    "Virtual Games");
            }

            if (renderLeftPanelParameter.IsShowSabaclub)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=42&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Saba.games</a>";
            }

            if (renderLeftPanelParameter.IsShowVGaming)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=66&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Vgaming</a>";
            }

            if (renderLeftPanelParameter.IsShowBitcoin)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=69&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Bitcoin</a>";
            }

            string masterSettingTab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + masterOnMemberSettingUrl + "\");' target='centerFr'>Sportsbook</a>";

            if (renderLeftPanelParameter.IsShowSabaCasino)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=6&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Saba Casino</a>";
            }

            if (!customerInfoViewModel.HideSettingLinkVirtualSports)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Virtual Sports</a>";
            }

            if (renderLeftPanelParameter.IsShowLiveCasinoForCredit)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=41&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Live Casino</a>";
            }

            if (renderLeftPanelParameter.IsShowVoidbridge)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=18&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>RNG Slot</a>";
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

            if (renderLeftPanelParameter.IsShowVirtualGames)
            {
                masterSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdVirtualGames,
                    custId,
                    3, // Master Level.
                    "Virtual Games");
            }

            if (renderLeftPanelParameter.IsShowSabaclub)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=42&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Saba.games</a>";
            }

            if (renderLeftPanelParameter.IsShowVGaming)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=66&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Vgaming</a>";
            }

            if (renderLeftPanelParameter.IsShowBitcoin)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=69&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Bitcoin</a>";
            }

            TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", superSettingTab);
            TemplateHandler.Assign(ref htmlcontent, "{contentminmax1}", masterSettingTab);
            return htmlcontent;
        }
    }
}