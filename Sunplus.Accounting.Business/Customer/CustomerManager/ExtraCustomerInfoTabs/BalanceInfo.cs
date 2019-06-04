namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Web;
    using DNAide.Web;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Security;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;

    public partial class CustomerManager
    {
        private static readonly string _statusCountTooltipCN88 = "Active/View Only/Suspended/Inactive";

        private static readonly string _statusCountTooltipMBC = "Active/Closed/Suspended";

        private static readonly string _statusCountTitleCN88 = "A/V/S/I";

        private static readonly string _statusCountTitleMBC = "A/C/S";

        private static string RenderBalanceInfo(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel)
        {
            bool hasTransferPermission = Permissions.GetHostPermission(Enumerations.Functions.Transfer.OtherTrans); // 2: Transfer

            string balanceinfo = WebUtils.LoadTemplateBySystem("CustomerInfo/BalanceInfo.htm");
            TemplateHandler.Assign(ref balanceinfo, "{colorTotalCash}", (hasTransferPermission ? " canchange" : string.Empty) + ((customerInfoViewModel.Cash >= 0) ? " b" : " hilight"));

            TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayCash}", (hasTransferPermission ? " canchange" : string.Empty) + ((customerInfoViewModel.YesterdayCash >= 0) ? " b" : " hilight"));
            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBal}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.Cash));

            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBalCur}", string.Format("{0:n}", customerInfoViewModel.Cash));
            TemplateHandler.Assign(ref balanceinfo, "{CurTotalCashBalCur}", customerInfoViewModel.Currency + " ");

            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBal}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.YesterdayCash));
            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBalCur}", string.Format("{0:n}", customerInfoViewModel.YesterdayCash));
            TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayCashBalCur}", customerInfoViewModel.Currency + " ");
            TemplateHandler.Assign(ref balanceinfo, "{CustName}", customerInfoViewModel.UserName);

            TemplateHandler.Assign(ref balanceinfo, "{AccOutstandTxn}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.AccOutstanding)));
            if (customerInfoViewModel.RoleId == 1)
            {
                TemplateHandler.Assign(ref balanceinfo, "{NetPosition}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.NetPosition));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "NetPosition");
            }

            //Check Given Credit Permission
            if (customerInfoViewModel.AllowUpdateGivenCredit)
            {
                TemplateHandler.Assign(ref balanceinfo, "{givencreditOnclick}", "return ChangeGivenCredit(true);");
                TemplateHandler.Assign(ref balanceinfo, "{givencreditClass}", "canchange");
                TemplateHandler.RemoveBlock(ref balanceinfo, "NotAllowGiven");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "AllowGiven");
            }

            if (customerInfoViewModel.RoleId == (int)Role.Headline)
            {
                RenderDownlineCountingInfo(ref balanceinfo, customerInfoViewModel);
                TemplateHandler.Assign(ref balanceinfo, "{CN88HeadlineCurrency}", customerInfoViewModel.Currency);
                TemplateHandler.Assign(ref balanceinfo, "{superhidden}", string.Empty);
                TemplateHandler.Assign(ref balanceinfo, "{masterhidden}", string.Empty);
                TemplateHandler.Assign(ref balanceinfo, "{agenthidden}", string.Empty);
                TemplateHandler.Assign(ref balanceinfo, "{memberhidden}", string.Empty);
            }
            else
            {
                var virtualRateClass = "canchange";
                var virtualRateText = customerInfoViewModel.EnableVirtualRate ? "Edit" : "Disabled";
                var virtualRateOnclick =
                    "return OpenEditVirtualExRate(" + customerInfoViewModel.CustId + ",'" +
                        customerInfoViewModel.UserName + "','" +
                        AccCommon.GetRoleName(customerInfoViewModel.RoleId) + "');";
                var virtualRateTitle = "Click to Edit";

                TemplateHandler.Assign(ref balanceinfo, "{superhidden}", "hidden");
                TemplateHandler.Assign(ref balanceinfo, "{virtualRateClass}", virtualRateClass);
                TemplateHandler.Assign(ref balanceinfo, "{virtualRateOnclick}", virtualRateOnclick);
                TemplateHandler.Assign(ref balanceinfo, "{VirtualRate}", virtualRateText);
                TemplateHandler.Assign(ref balanceinfo, "{virtualRateTitle}", virtualRateTitle);

                TemplateHandler.Assign(ref balanceinfo, "{GivenCreditCurrency}", customerInfoViewModel.Currency + " ");
                TemplateHandler.Assign(ref balanceinfo, "{GivenCredit}", string.Format("{0:n}", customerInfoViewModel.Credit));
                if (customerInfoViewModel.RoleId > 1)
                {
                    TemplateHandler.RemoveBlock(ref balanceinfo, "betcredit");

                    TemplateHandler.Assign(ref balanceinfo, "{colorTotalBal}", (hasTransferPermission ? " canchange" : string.Empty) + ((customerInfoViewModel.TotalBal >= 0) ? " b" : " hilight"));
                    TemplateHandler.Assign(ref balanceinfo, "{TotalBal}", string.Format("{0:####.##}", customerInfoViewModel.TotalBal));
                    TemplateHandler.Assign(ref balanceinfo, "{TotalBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.TotalBal)));
                    TemplateHandler.Assign(ref balanceinfo, "{CurTotalBalCur}", customerInfoViewModel.Currency + " ");

                    TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayBal}", (hasTransferPermission ? " canchange" : string.Empty) + ((customerInfoViewModel.YesterdayBal >= 0) ? " b" : " hilight"));
                    TemplateHandler.Assign(ref balanceinfo, "{YesterdayBal}", string.Format("{0:####.##}", customerInfoViewModel.YesterdayBal));
                    TemplateHandler.Assign(ref balanceinfo, "{YesterdayBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.YesterdayBal)));
                    TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayBalCur}", customerInfoViewModel.Currency + " ");

                    TemplateHandler.Assign(ref balanceinfo, "{OutstandTxn}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.OutStanding)));
                    TemplateHandler.Assign(ref balanceinfo, "{UsedCredit}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.UsedCredit)));
                    TemplateHandler.Assign(ref balanceinfo, "{AgentType}", customerInfoViewModel.DownlineName);
                    TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "");
                    TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "hidden");

                    RenderDownlineCountingInfo(ref balanceinfo, customerInfoViewModel);
                }
                else
                {
                    TemplateHandler.Assign(ref balanceinfo, "{BetCredit}", customerInfoViewModel.Currency + " " + Core.Helper.Utils.GetDecimalRoundDown(customerInfoViewModel.BetCredit));
                    if (AppSettings.IsCN88SiteName(customerInfoViewModel.CustomerSite))
                    {
                        TemplateHandler.Assign(ref balanceinfo, "{YesterdayBetCredit}", customerInfoViewModel.YesterdayBetCredit == -1 ? "N/A" : Core.Helper.Utils.GetDecimalRoundDown(customerInfoViewModel.YesterdayBetCredit));
                    }

                    TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "hidden");
                    TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "");

                    TemplateHandler.Assign(ref balanceinfo, "{masterhidden}", "hidden");
                    TemplateHandler.Assign(ref balanceinfo, "{agenthidden}", "hidden");
                    TemplateHandler.Assign(ref balanceinfo, "{memberhidden}", "hidden");
                }
            }

            TemplateHandler.Assign(ref htmlcontent, "{BalanceInfo}", balanceinfo);
            return htmlcontent;
        }

        private static void RenderDownlineCountingInfo(ref string balanceInfo, CustomerInfoViewModel customerInfoViewModel)
        {
            if (AppSettings.IsCN88SiteName(customerInfoViewModel.CustomerSite))
            {
                RenderDownlineCountingInfoCN88(ref balanceInfo, customerInfoViewModel);
                return;
            }

            RenderDownlineCountingInfoMBC(ref balanceInfo, customerInfoViewModel);
        }

        private static void RenderDownlineCountingInfoMBC(ref string balanceInfo, CustomerInfoViewModel customerInfoViewModel)
        {
            TemplateHandler.Assign(ref balanceInfo, "{masteractive}",
                                    string.Format("<span style='color:blue'>{0}</span>/<span style='color:red'>{1}</span>/{2}",
                                                    customerInfoViewModel.MAgentActive, customerInfoViewModel.MAgentClosed, customerInfoViewModel.MAgentSuspended));

            TemplateHandler.Assign(ref balanceInfo, "{agentavtive}",
                                    string.Format("<span style='color:blue'>{0}</span>/<span style='color:red'>{1}</span>/{2}",
                                                    customerInfoViewModel.AgentActive, customerInfoViewModel.AgentClosed, customerInfoViewModel.AgentSuspended));

            TemplateHandler.Assign(ref balanceInfo, "{memberactive}",
                                    string.Format("<span style='color:blue'>{0}</span>/<span style='color:red'>{1}</span>/{2}",
                                                    customerInfoViewModel.MemberActive, customerInfoViewModel.MemberClosed, customerInfoViewModel.MemberSuspended));

            TemplateHandler.Assign(ref balanceInfo, "{statuscounttooltip}", _statusCountTooltipMBC);
            TemplateHandler.Assign(ref balanceInfo, "{statuscounttitle}", _statusCountTitleMBC);
            TemplateHandler.Assign(ref balanceInfo, "{masterhidden}", customerInfoViewModel.RoleId > 3 ? string.Empty : "hidden");
            TemplateHandler.Assign(ref balanceInfo, "{agenthidden}", customerInfoViewModel.RoleId > 2 ? string.Empty : "hidden");
            TemplateHandler.Assign(ref balanceInfo, "{memberhidden}", customerInfoViewModel.RoleId > 1 ? string.Empty : "hidden");
        }

        private static void RenderDownlineCountingInfoCN88(ref string balanceInfo, CustomerInfoViewModel customerInfoViewModel)
        {
            if (customerInfoViewModel.RoleId == (int)Role.Headline)
            {
                TemplateHandler.Assign(ref balanceInfo, "{superactive}",
                                    string.Format("<span class='status-count-active'>{0}</span>/<span class='status-count-view-only'>{1}</span>/<span class='status-count-suspended'>{2}</span>/<span class='status-count-inactive'>{3}</span>",
                                                    customerInfoViewModel.SAgentActive, customerInfoViewModel.SAgentViewOnly, customerInfoViewModel.SAgentSuspended, customerInfoViewModel.SAgentInactive));
            }

            TemplateHandler.Assign(ref balanceInfo, "{masteractive}",
                                    string.Format("<span class='status-count-active'>{0}</span>/<span class='status-count-view-only'>{1}</span>/<span class='status-count-suspended'>{2}</span>/<span class='status-count-inactive'>{3}</span>",
                                                    customerInfoViewModel.MAgentActive, customerInfoViewModel.MAgentViewOnly, customerInfoViewModel.MAgentSuspended, customerInfoViewModel.MAgentInactive));

            TemplateHandler.Assign(ref balanceInfo, "{agentavtive}",
                                    string.Format("<span class='status-count-active'>{0}</span>/<span class='status-count-view-only'>{1}</span>/<span class='status-count-suspended'>{2}</span>/<span class='status-count-inactive'>{3}</span>",
                                                    customerInfoViewModel.AgentActive, customerInfoViewModel.AgentViewOnly, customerInfoViewModel.AgentSuspended, customerInfoViewModel.AgentInactive));

            TemplateHandler.Assign(ref balanceInfo, "{memberactive}",
                                    string.Format("<span class='status-count-active'>{0}</span>/<span class='status-count-view-only'>{1}</span>/<span class='status-count-suspended'>{2}</span>/<span class='status-count-inactive'>{3}</span>",
                                                    customerInfoViewModel.MemberActive, customerInfoViewModel.MemberViewOnly, customerInfoViewModel.MemberSuspended, customerInfoViewModel.MemberInactive));

            TemplateHandler.Assign(ref balanceInfo, "{statuscounttooltip}", _statusCountTooltipCN88);
            TemplateHandler.Assign(ref balanceInfo, "{statuscounttitle}", _statusCountTitleCN88);
            if (customerInfoViewModel.RoleId != (int)Role.Headline)
            {
                TemplateHandler.Assign(ref balanceInfo, "{masterhidden}", customerInfoViewModel.RoleId > 3 ? string.Empty : "hidden");
                TemplateHandler.Assign(ref balanceInfo, "{agenthidden}", customerInfoViewModel.RoleId > 2 ? string.Empty : "hidden");
                TemplateHandler.Assign(ref balanceInfo, "{memberhidden}", customerInfoViewModel.RoleId > 1 ? string.Empty : "hidden");
            }
        }
    }
}
