namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Data;
    using DNAide.Core;
    using DNAide.Web;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
    using RemotingRef;
    using Core.Common;
    using Core.Configurations;
    using Bingo.Core;


    public partial class CustomerManager
    {
        private static string GenMenuMinMaxPT(
            string htmlcontent, 
            DataRow row2,
            CustomerInfoViewModel customerInfoViewModel,
            RenderLeftPanelParameter renderLeftPanelParameter)
        {
            int custId = customerInfoViewModel.custId;
            int roleId = customerInfoViewModel.roleId;
            string userName = customerInfoViewModel.userName;
            bool isAllowEditBI = customerInfoViewModel.isAllowEditBI && AppSession.IsSyncBingoCurrentCust;
            bool isDeposit = customerInfoViewModel.isDeposit;
            string siteName = customerInfoViewModel.customerSite;
            bool isHideSportsbook2 = customerInfoViewModel.isHideSportsbook2;
            bool isHideCockFighting = customerInfoViewModel.isHideCockFighting;
            bool isHideCricket = customerInfoViewModel.isHideCricketSettingLink;
            bool isHideVirtualSports = customerInfoViewModel.isHideVirtualSports;

            bool isShowAllBet = renderLeftPanelParameter.IsShowAllBetForCredit;
            bool isShowVoidbridge = renderLeftPanelParameter.IsShowVoidbridge;
            bool isShowNG2 = renderLeftPanelParameter.IsShowNG2;
            bool isShowFishingWorld = renderLeftPanelParameter.IsShowFishingWorld;
            bool isShowKenoLottery = renderLeftPanelParameter.IsShowKenoLottery;
            bool isShowLucky3 = renderLeftPanelParameter.IsShowLucky3;


            int parentId;
            string submenu1 = string.Empty;
            string submenu2 = string.Empty;
            string submenu3 = string.Empty;
            string minMaxPtUrl = string.Empty;
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

            return htmlcontent;
        }
    }
}
