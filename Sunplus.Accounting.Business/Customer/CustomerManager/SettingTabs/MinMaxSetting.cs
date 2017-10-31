namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Configurations;
    using DNAide.Web;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
    using RemotingRef;

    public partial class CustomerManager
    {
        private static string GenMenuMinMaxPT(
            string htmlcontent, 
            CustomerInfoViewModel customerInfoViewModel,
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            switch (customerInfoViewModel.RoleId)
            {
                case 1: // member
                    htmlcontent = AssignMemberSetting(htmlcontent, customerInfoViewModel, renderLeftPanelParameter);
                    break;

                case 2: // agent
                    htmlcontent = AssignAgentMinMaxSetting(
                        htmlcontent,
                        customerInfoViewModel.CustId,
                        customerInfoViewModel.UserName,
                        customerInfoViewModel.IsDeposit, 
                        customerInfoViewModel.MasterId);

                    break;

                case 3: // master
                    htmlcontent = AssignMasterMinMaxSetting(
                        htmlcontent,
                        customerInfoViewModel.CustId, 
                        customerInfoViewModel.UserName, 
                        customerInfoViewModel.IsDeposit, 
                        customerInfoViewModel.SuperId);
                    break;

                case 4: // super
                    htmlcontent = AssignSuperMinSetting(
                        htmlcontent, 
                        customerInfoViewModel.CustId, 
                        customerInfoViewModel.UserName, 
                        customerInfoViewModel.IsDeposit);
                    break;

                default:
                    break;
            }

            return htmlcontent;
        }

        private static string AssignMemberSetting(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel,
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            int custId = customerInfoViewModel.CustId;
            int roleId = customerInfoViewModel.RoleId;
            string userName = customerInfoViewModel.UserName;
            bool isDeposit = customerInfoViewModel.IsDeposit;
            string siteName = customerInfoViewModel.CustomerSite;

            bool isHideSportsbook2 = customerInfoViewModel.IsHideSportsbook2;
            bool isHideCockFighting = customerInfoViewModel.IsHideCockFighting;
            bool isHideCricket = customerInfoViewModel.IsHideCricketSettingLink;
            bool isHideVirtualSports = customerInfoViewModel.IsHideVirtualSports;

            bool isShowAllBet = renderLeftPanelParameter.IsShowAllBetForCredit;
            bool isShowVoidbridge = renderLeftPanelParameter.IsShowVoidbridge;
            bool isShowNG2 = renderLeftPanelParameter.IsShowNG2;
            bool isShowFishingWorld = renderLeftPanelParameter.IsShowFishingWorld;
            bool isShowKenoLottery = renderLeftPanelParameter.IsShowKenoLottery;
            bool isShowLucky3 = renderLeftPanelParameter.IsShowLucky3;
            bool isShowKenoSetting = KenoSection.Instance.ShowSetting;

            var agentId = customerInfoViewModel.AgentId;
            var superId = customerInfoViewModel.SuperId;
            var masterId = customerInfoViewModel.MasterId;

            string superSettingTab = string.Empty;
            string masterSettingTab = string.Empty;
            string superOnMemberSettingUrl = string.Empty;
            string masterOnMemberSettingUrl = string.Empty;

            TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "inline");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? string.Empty : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Super PT</span></a></li>");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", isDeposit ? string.Empty : "<a href=\"javascript:void(0);\"><span>Master PT</span></a>");

            superOnMemberSettingUrl = string.Format("EditSuperOnMember/EditSuperOnMemberPage?agentId={0}&custId={1}&uname={2}&siteName={3}", agentId, custId, userName, siteName);
            masterOnMemberSettingUrl = string.Format("EditMasterOnMember/EditMasterOnMemberPage?agentId={0}&custId={1}&uname={2}&siteName={3}", agentId, custId, userName, siteName);

            superSettingTab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + superOnMemberSettingUrl + "\");' target='centerFr'>Sport book</a>";

            if (!WebSettings.disabledHR)
            {
                superSettingTab += string.Format("<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"SuperEditMemberPTRacing/Index?custid={0}&superid={1}\");' target='centerFr'>Racing</a>", custId, superId);
            }

            if (WebSettings.EnableLiveCasino)
            {
                if (AppSession.IsSyncLCSCurrentCust)
                {
                    var urlLiveCasino = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&parentId={1}&uplineLevel=4&actorId={2}&actorUsername={3}&customerId={4}&customerLevel=1", superId, agentId, AppSession.AccId, AppSession.AccUserName, custId);
                    superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + urlLiveCasino + "\");' target='centerFr'>Live Casino</a>";
                }
            }
            if (!isHideVirtualSports && !isDeposit)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Virtual Sports</a>";
            }

            if (isShowKenoSetting)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customersetting/KenoSetting/kenosetting/SuperEditMember?custId=" + custId.ToString() + "&userId=" + AppSession.AccId + "\");' target='centerFr'>Keno</a>";
            }

            if (WebSettings.IsShowGoldDeluxeSettingForCredit)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId=" + superId + "&upLineLevel=4&formType=1&actorId=" + AppSession.AccId + "&custId=" + custId.ToString() + "&level=1\");' target='centerFr'>Gold Deluxe</a>";
            }

            if (!isHideSportsbook2 && !isDeposit)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=16&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Sportsbook 2</a>";
            }

            if (!isHideCockFighting && !isDeposit)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=19&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Cockfighting</a>";
            }

            if (!isHideCricket)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=20&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Cricket</a>";
            }

            if (isShowAllBet)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=21&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Allbet</a>";
            }

            if (isShowVoidbridge)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=22&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Macau Games</a>";
            }

            if (isShowNG2)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=23&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>" + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + "</a>";
            }

            if (isShowFishingWorld)
            {
                superSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?language=en-US&productId=24&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=4\");' target='centerFr'>Fishing World</a>";
            }

            if (isShowKenoLottery)
            {
                superSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery,
                    custId,
                    4, // Super Level.
                    "Lottery");
            }

            if (isShowLucky3)
            {
                superSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    26,
                    custId,
                    4, // Super Level.
                    "Lucky 3");
            }

            masterSettingTab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + masterOnMemberSettingUrl + "\");' target='centerFr'>Sport book</a>";

            if (!WebSettings.disabledHR)
            {
                masterSettingTab += string.Format("<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"MasterEditMemberPTRacing/Index?custid={0}&masterid={1}\");' target='centerFr'>Racing</a>", custId, masterId);
            }

            if (WebSettings.EnableLiveCasino)
            {
                if (AppSession.IsSyncLCSCurrentCust)
                {
                    var urlLiveCasino = string.Format("LiveCasinoSetting/Setting/EditSetting?uplineId={0}&parentId={1}&uplineLevel=3&actorId={2}&actorUsername={3}&customerId={4}&customerLevel=1", masterId, agentId, AppSession.AccId, AppSession.AccUserName, custId);
                    masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + urlLiveCasino + "\");' target='centerFr'>Live Casino</a>";
                }
            }
            if (!isHideVirtualSports && !isDeposit)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=5&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Virtual Sports</a>";
            }

            if (isShowKenoSetting)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customersetting/KenoSetting/kenosetting/MasterEditMember?custId=" + custId.ToString() + "&userId=" + AppSession.AccId + "\");' target='centerFr'>Keno</a>";
            }

            if (WebSettings.IsShowGoldDeluxeSettingForCredit)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId=" + masterId + "&upLineLevel=3&formType=1&actorId=" + AppSession.AccId + "&custId=" + custId.ToString() + "&level=1\");' target='centerFr'>Gold Deluxe</a>";

            }
            if (!isHideSportsbook2 && !isDeposit)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=16&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Sportsbook 2</a>";
            }

            if (!isHideCockFighting && !isDeposit)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=19&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Cockfighting</a>";
            }

            if (!isHideCricket)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=20&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Cricket</a>";
            }

            if (isShowAllBet)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=21&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Allbet</a>";
            }

            if (isShowVoidbridge)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=22&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Macau Games</a>";
            }

            if (isShowNG2)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?productId=23&language=en-US&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>" + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + "</a>";
            }

            if (isShowFishingWorld)
            {
                masterSettingTab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "DCS/ViewCustomerSetting?language=en-US&productId=24&customerLevel=1&customerId=" + custId.ToString() + "&uplinelevel=3\");' target='centerFr'>Fishing World</a>";
            }

            if (isShowKenoLottery)
            {
                masterSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery,
                    custId,
                    3, // Master Level.
                    "Lottery");
            }

            if (isShowLucky3)
            {
                masterSettingTab += string.Format(
                    "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"DCS/ViewCustomerSetting?language=en-US&productId={0}&customerLevel=1&customerId={1}&uplinelevel={2}\");' target='centerFr'>{3}</a>",
                    26,
                    custId,
                    3, // Master Level.
                    "Lucky 3");
            }

            TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", superSettingTab);
            TemplateHandler.Assign(ref htmlcontent, "{contentminmax1}", masterSettingTab);
            return htmlcontent;
        }

        private static string AssignAgentMinMaxSetting(string htmlcontent, int custId, string userName, bool isDeposit, int masterId)
        {
            TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "none");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? string.Empty  : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Min/Max PT</span></a></li>");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", string.Empty);

            var minMaxPtUrl = "EditAgentMinMaxPt/EditAgentMinMaxPtPage?&custId=" + custId + "&uplineId=" + masterId + "&uname=" + userName;
            var minMaxPTtab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";

            if (!WebSettings.disabledHR)
            {
                minMaxPTtab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/pt/racing/AgentMinMaxUpdate.aspx?custId=" + custId.ToString() + "\");' target='centerFr'>Racing</a>";
            }

            TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", minMaxPTtab);
            return htmlcontent;
        }

        private static string AssignMasterMinMaxSetting(string htmlcontent, int custId, string userName, bool isDeposit, int superId)
        {
            TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "none");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? "" : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Min/Max PT</span></a></li>");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", string.Empty);

            var minMaxPtUrl = "EditMasterMinMaxPt/EditMasterMinMaxPtPage?&custId=" + custId + "&uplineId=" + superId + "&uname=" + userName;
            var minMaxPTtab = "<a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";

            if (!WebSettings.disabledHR)
            {
                minMaxPTtab += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/pt/racing/MasterMinMaxUpdate.aspx?custId=" + custId.ToString() + "\");' target='centerFr'>Racing</a>";
            }

            TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", minMaxPTtab);
            return htmlcontent;
        }

        private static string AssignSuperMinSetting(string htmlcontent, int custId, string userName, bool isDeposit)
        {
            TemplateHandler.Assign(ref htmlcontent, "{isHiddenMember}", "none");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax}", isDeposit ? string.Empty : "<li id=\"detail_tab_1\" onclick=\"detail_tabnew_Click(this)\"><a href=\"javascript:void(0);\"><span>Min PT</span></a></li>");
            TemplateHandler.Assign(ref htmlcontent, "{lblMinMax1}", string.Empty);

            var  minMaxPtUrl = "EditSuperMinPt/EditSuperMinPtPage?&custId=" + custId + "&uname=" + userName;
            var submenu3 = "<a class='button_left02'  href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + minMaxPtUrl + "\");' target='centerFr'>Sport book</a>";

            if (!WebSettings.disabledHR)
            {
                submenu3 += "<br><a class='button_left02' href='javascript:;' onclick='parent.gotoURL(\"centerFr\",\"" + "customer/pt/racing/SuperMinUpdate.aspx?custId=" + custId.ToString() + "\");' target='centerFr'>Racing</a>";
            }

            TemplateHandler.Assign(ref htmlcontent, "{contentminmax}", submenu3);
            return htmlcontent;
        }
    }
}
