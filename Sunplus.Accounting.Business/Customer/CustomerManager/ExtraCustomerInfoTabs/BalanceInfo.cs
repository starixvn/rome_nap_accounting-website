namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Web;
    using DNAide.Web;
    using Security;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;

    public partial class CustomerManager
    {
        private static readonly string _statusCountTooltipMBC = "Active/Closed/Suspended";
        private static readonly string _statusCountTitleMBC = "A/C/S";

        private static string RenderBalanceInfo(
            string htmlcontent,
            CustomerInfoViewModel customerInfoViewModel)
        {
            if (!Permissions.HasPermission(Enumerations.Functions.Customer.CustomerBalance))
            {
                TemplateHandler.Assign(ref htmlcontent, "{BalanceInfo}", string.Empty);
                return htmlcontent;
            }

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

            TemplateHandler.Assign(ref balanceinfo, "{RewardBalance}", string.Format("{0:n}", customerInfoViewModel.RewardCurrency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.RewardBalance)));
            if (customerInfoViewModel.SearchAccountRoleId == 1)
            {
                TemplateHandler.Assign(ref balanceinfo, "{NetPosition}",
                    customerInfoViewModel.Currency + " " +
                    IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.NetPosition));
                TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "hidden");
                TemplateHandler.RemoveBlock(ref balanceinfo, "HideOnMember");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "NetPosition");
                TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "");
                TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayBal}", (hasTransferPermission ? " canchange" : string.Empty) + ((customerInfoViewModel.YesterdayBal >= 0) ? " b" : " hilight"));
                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBal}", string.Format("{0:####.##}", customerInfoViewModel.YesterdayBal));
                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.YesterdayBal)));
                TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayBalCur}", customerInfoViewModel.Currency + " ");
                TemplateHandler.Assign(ref balanceinfo, "{OutstandTxn}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.OutStanding)));
                TemplateHandler.Assign(ref balanceinfo, "{RewardPending}", string.Format("{0:n}", customerInfoViewModel.RewardCurrency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.RewardPending)));
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

            if (customerInfoViewModel.SearchAccountRoleId == (int)Role.Headline)
            {
                RenderDownlineCountingInfo(ref balanceinfo, customerInfoViewModel);
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
                        AccCommon.GetRoleName(customerInfoViewModel.SearchAccountRoleId) + "');";
                var virtualRateTitle = "Click to Edit";

                TemplateHandler.Assign(ref balanceinfo, "{superhidden}", "hidden");
                TemplateHandler.Assign(ref balanceinfo, "{virtualRateClass}", virtualRateClass);
                TemplateHandler.Assign(ref balanceinfo, "{virtualRateOnclick}", virtualRateOnclick);
                TemplateHandler.Assign(ref balanceinfo, "{VirtualRate}", virtualRateText);
                TemplateHandler.Assign(ref balanceinfo, "{virtualRateTitle}", virtualRateTitle);

                TemplateHandler.Assign(ref balanceinfo, "{GivenCreditCurrency}", customerInfoViewModel.Currency + " ");
                TemplateHandler.Assign(ref balanceinfo, "{GivenCredit}", string.Format("{0:n}", customerInfoViewModel.Credit));
                if (customerInfoViewModel.SearchAccountRoleId > 1)
                {
                    TemplateHandler.RemoveBlock(ref balanceinfo, "betcredit");

                    TemplateHandler.Assign(ref balanceinfo, "{colorTotalBal}", (hasTransferPermission ? " canchange" : string.Empty) + ((customerInfoViewModel.TotalBal >= 0) ? " b" : " hilight"));
                    TemplateHandler.Assign(ref balanceinfo, "{TotalBal}", string.Format("{0:####.##}", customerInfoViewModel.TotalBal));
                    TemplateHandler.Assign(ref balanceinfo, "{TotalBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.TotalBal)));
                    TemplateHandler.Assign(ref balanceinfo, "{CurTotalBalCur}", customerInfoViewModel.Currency + " ");

                    TemplateHandler.Assign(ref balanceinfo, "{OutstandTxn}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.OutStanding)));
                    TemplateHandler.Assign(ref balanceinfo, "{UsedCredit}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.UsedCredit)));
                    TemplateHandler.Assign(ref balanceinfo, "{AgentType}", customerInfoViewModel.DirectDownlineRoleName);
                    TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "hidden");

                    RenderDownlineCountingInfo(ref balanceinfo, customerInfoViewModel);
                }
                else
                {
                    TemplateHandler.Assign(ref balanceinfo, "{BetCredit}", customerInfoViewModel.Currency + " " + Core.Helper.Utils.GetDecimalRoundDown(customerInfoViewModel.BetCredit));
                    TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "");
                    TemplateHandler.Assign(ref balanceinfo, "{masterhidden}", "hidden");
                    TemplateHandler.Assign(ref balanceinfo, "{agenthidden}", "hidden");
                    TemplateHandler.Assign(ref balanceinfo, "{memberhidden}", "hidden");
                }
            }

            if (customerInfoViewModel.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "3rdPendingTransfer");
            }
            else
            {
                TemplateHandler.Assign(ref balanceinfo, "{3rdPendingTransfer}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.PendingTransfer3rd)));
            }

            string isDirectParams = "";

            if (customerInfoViewModel.CheckedDirectStatus ||
                customerInfoViewModel.SearchAccountRoleId == (int)Role.Super)
            {
                isDirectParams = "&RewardMaxTransId=" + customerInfoViewModel.RewardMaxTransId + "&RewardAmount=" + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.RewardBalance) + "&RewardCurrency=" + customerInfoViewModel.RewardCurrency + "&TotalBalReward=" + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.RewardPending + customerInfoViewModel.YesterdayBal);
            }

            TemplateHandler.Assign(ref balanceinfo, "{DirectRewardParams}", isDirectParams);

            TemplateHandler.Assign(ref htmlcontent, "{BalanceInfo}", balanceinfo);
            return htmlcontent;
        }

        private static void RenderDownlineCountingInfo(ref string balanceInfo, CustomerInfoViewModel customerInfoViewModel)
        {
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

            TemplateHandler.Assign(ref balanceInfo, "{directmemberactive}",
                string.Format("<span style='color:blue'>{0}</span>/<span style='color:red'>{1}</span>/{2}",
                    customerInfoViewModel.DirectMemberActive, customerInfoViewModel.DirectMemberClosed, customerInfoViewModel.DirectMemberSuspended));

            TemplateHandler.Assign(ref balanceInfo, "{statuscounttooltip}", _statusCountTooltipMBC);
            TemplateHandler.Assign(ref balanceInfo, "{statuscounttitle}", _statusCountTitleMBC);
            TemplateHandler.Assign(ref balanceInfo, "{masterhidden}", customerInfoViewModel.SearchAccountRoleId > 3 ? string.Empty : "hidden");
            TemplateHandler.Assign(ref balanceInfo, "{agenthidden}", customerInfoViewModel.SearchAccountRoleId > 2 ? string.Empty : "hidden");
            TemplateHandler.Assign(ref balanceInfo, "{memberhidden}", customerInfoViewModel.SearchAccountRoleId > 1 ? string.Empty : "hidden");
            TemplateHandler.Assign(ref balanceInfo, "{agentmemberhidden}", customerInfoViewModel.SearchAccountRoleId > 2 ? string.Empty : "hidden");
        }
    }
}