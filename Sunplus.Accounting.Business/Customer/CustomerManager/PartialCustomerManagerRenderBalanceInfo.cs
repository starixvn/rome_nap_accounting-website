namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Web;
    using DNAide.Data;
    using DNAide.Web;
    using Security;

    public partial class CustomerManager
    {
        private static string RenderBalanceInfo(
            string htmlcontent, 
            DbResult ds,
            CustomerInfoViewModel customerInfoViewModel)
        {
            bool hasTransferPermission = Permissions.GetHostPermission(Enumerations.Functions.Transfer.OtherTrans); // 2: Transfer

            string balanceinfo = WebUtils.LoadTemplateBySystem("CustomerInfo/BalanceInfo.htm");
            TemplateHandler.Assign(ref balanceinfo, "{colorTotalCash}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.cash >= 0) ? " b" : " hilight"));

            TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayCash}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.yesterdayCash >= 0) ? " b" : " hilight"));
            if (string.IsNullOrEmpty(customerInfoViewModel.cash.ToString())) customerInfoViewModel.cash = 0;
            if (string.IsNullOrEmpty(customerInfoViewModel.yesterdayCash.ToString())) customerInfoViewModel.yesterdayCash = 0;
            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBal}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.cash));

            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBalCur}", string.Format("{0:n}", customerInfoViewModel.cash));
            TemplateHandler.Assign(ref balanceinfo, "{CurTotalCashBalCur}", customerInfoViewModel.currency + " ");

            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBal}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.yesterdayCash));
            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBalCur}", string.Format("{0:n}", customerInfoViewModel.yesterdayCash));
            TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayCashBalCur}", customerInfoViewModel.currency + " ");

            TemplateHandler.Assign(ref balanceinfo, "{AccOutstandTxn}", string.Format("{0:n}", customerInfoViewModel.currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.accOutstanding)));
            if (customerInfoViewModel.roleId == 1)
            {
                TemplateHandler.Assign(ref balanceinfo, "{NetPosition}", customerInfoViewModel.currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.netPosition));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "NetPosition");
            }

            //Check Given Credit Permission
            if (customerInfoViewModel.isallowupdategivencredit)
            {
                TemplateHandler.Assign(ref balanceinfo, "{givencreditOnclick}", "return ChangeGivenCredit(true);");
                TemplateHandler.Assign(ref balanceinfo, "{givencreditClass}", "canchange");
                TemplateHandler.RemoveBlock(ref balanceinfo, "NotAllowGiven");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "AllowGiven");
            }

            var virtualRateClass = "canchange";
            var virtualRateText = customerInfoViewModel.EnableVirtualRate ? "Edit" : "Disabled";
            var virtualRateOnclick =
                "return OpenEditVirtualExRate(" + customerInfoViewModel.custId + ",'" +
                    customerInfoViewModel.userName + "','" +
                    AccCommon.GetRoleName(customerInfoViewModel.roleId) + "');";
            var virtualRateTitle = "Click to Edit";
            TemplateHandler.Assign(ref balanceinfo, "{virtualRateClass}", virtualRateClass);
            TemplateHandler.Assign(ref balanceinfo, "{virtualRateOnclick}", virtualRateOnclick);
            TemplateHandler.Assign(ref balanceinfo, "{VirtualRate}", virtualRateText);
            TemplateHandler.Assign(ref balanceinfo, "{virtualRateTitle}", virtualRateTitle);

            TemplateHandler.Assign(ref balanceinfo, "{GivenCreditCurrency}", customerInfoViewModel.currency + " ");
            TemplateHandler.Assign(ref balanceinfo, "{GivenCredit}", string.Format("{0:n}", customerInfoViewModel.credit));
            if (customerInfoViewModel.roleId > 1)
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "betcredit");

                TemplateHandler.Assign(ref balanceinfo, "{colorTotalBal}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.totalBal >= 0) ? " b" : " hilight"));
                TemplateHandler.Assign(ref balanceinfo, "{TotalBal}", string.Format("{0:####.##}", customerInfoViewModel.totalBal));
                TemplateHandler.Assign(ref balanceinfo, "{TotalBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.totalBal)));
                TemplateHandler.Assign(ref balanceinfo, "{CurTotalBalCur}", customerInfoViewModel.currency + " ");

                TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayBal}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.yesterdayBal >= 0) ? " b" : " hilight"));
                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBal}", string.Format("{0:####.##}", customerInfoViewModel.yesterdayBal));
                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.yesterdayBal)));
                TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayBalCur}", customerInfoViewModel.currency + " ");

                TemplateHandler.Assign(ref balanceinfo, "{OutstandTxn}", string.Format("{0:n}", customerInfoViewModel.currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.outStanding)));
                TemplateHandler.Assign(ref balanceinfo, "{UsedCredit}", string.Format("{0:n}", customerInfoViewModel.currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.usedCredit)));
                TemplateHandler.Assign(ref balanceinfo, "{AgentType}", customerInfoViewModel.downlinename);
                TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "");
                TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "hidden");

                GetDownlineCountingInfo(ref balanceinfo, ds, customerInfoViewModel.roleId);
            }
            else
            {
                TemplateHandler.Assign(ref balanceinfo, "{BetCredit}", customerInfoViewModel.currency + " " + Core.Helper.Utils.GetDecimalRoundDown(customerInfoViewModel.betCredit));

                TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "hidden");
                TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "");

                TemplateHandler.Assign(ref balanceinfo, "{masterhidden}", "hidden");
                TemplateHandler.Assign(ref balanceinfo, "{agenthidden}", "hidden");
                TemplateHandler.Assign(ref balanceinfo, "{memberhidden}", "hidden");
            }

            TemplateHandler.Assign(ref htmlcontent, "{BalanceInfo}", balanceinfo);
            return htmlcontent;
        }

    }
}
