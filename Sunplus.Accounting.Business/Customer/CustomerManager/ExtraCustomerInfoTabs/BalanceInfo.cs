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
            TemplateHandler.Assign(ref balanceinfo, "{colorTotalCash}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.Cash >= 0) ? " b" : " hilight"));

            TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayCash}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.YesterdayCash >= 0) ? " b" : " hilight"));
            if (string.IsNullOrEmpty(customerInfoViewModel.Cash.ToString())) customerInfoViewModel.Cash = 0;
            if (string.IsNullOrEmpty(customerInfoViewModel.YesterdayCash.ToString())) customerInfoViewModel.YesterdayCash = 0;
            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBal}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.Cash));

            TemplateHandler.Assign(ref balanceinfo, "{TotalCashBalCur}", string.Format("{0:n}", customerInfoViewModel.Cash));
            TemplateHandler.Assign(ref balanceinfo, "{CurTotalCashBalCur}", customerInfoViewModel.Currency + " ");

            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBal}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.YesterdayCash));
            TemplateHandler.Assign(ref balanceinfo, "{YesterdayCashBalCur}", string.Format("{0:n}", customerInfoViewModel.YesterdayCash));
            TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayCashBalCur}", customerInfoViewModel.Currency + " ");

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
            if (customerInfoViewModel.IsAllowUpdateGivenCredit)
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
                "return OpenEditVirtualExRate(" + customerInfoViewModel.CustId + ",'" +
                    customerInfoViewModel.UserName + "','" +
                    AccCommon.GetRoleName(customerInfoViewModel.RoleId) + "');";
            var virtualRateTitle = "Click to Edit";
            TemplateHandler.Assign(ref balanceinfo, "{virtualRateClass}", virtualRateClass);
            TemplateHandler.Assign(ref balanceinfo, "{virtualRateOnclick}", virtualRateOnclick);
            TemplateHandler.Assign(ref balanceinfo, "{VirtualRate}", virtualRateText);
            TemplateHandler.Assign(ref balanceinfo, "{virtualRateTitle}", virtualRateTitle);

            TemplateHandler.Assign(ref balanceinfo, "{GivenCreditCurrency}", customerInfoViewModel.Currency + " ");
            TemplateHandler.Assign(ref balanceinfo, "{GivenCredit}", string.Format("{0:n}", customerInfoViewModel.Credit));
            if (customerInfoViewModel.RoleId > 1)
            {
                TemplateHandler.RemoveBlock(ref balanceinfo, "betcredit");

                TemplateHandler.Assign(ref balanceinfo, "{colorTotalBal}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.TotalBal >= 0) ? " b" : " hilight"));
                TemplateHandler.Assign(ref balanceinfo, "{TotalBal}", string.Format("{0:####.##}", customerInfoViewModel.TotalBal));
                TemplateHandler.Assign(ref balanceinfo, "{TotalBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.TotalBal)));
                TemplateHandler.Assign(ref balanceinfo, "{CurTotalBalCur}", customerInfoViewModel.Currency + " ");

                TemplateHandler.Assign(ref balanceinfo, "{colorYesterdayBal}", ((hasTransferPermission && Functions.Customer.IsApplyCssBalanceInfo) ? " canchange" : string.Empty) + ((customerInfoViewModel.YesterdayBal >= 0) ? " b" : " hilight"));
                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBal}", string.Format("{0:####.##}", customerInfoViewModel.YesterdayBal));
                TemplateHandler.Assign(ref balanceinfo, "{YesterdayBalCur}", string.Format("{0:n}", IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.YesterdayBal)));
                TemplateHandler.Assign(ref balanceinfo, "{CurYesterdayBalCur}", customerInfoViewModel.Currency + " ");

                TemplateHandler.Assign(ref balanceinfo, "{OutstandTxn}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.OutStanding)));
                TemplateHandler.Assign(ref balanceinfo, "{UsedCredit}", string.Format("{0:n}", customerInfoViewModel.Currency + " " + IBCUtils.FormatHandler.FormatNumber20(customerInfoViewModel.UsedCredit)));
                TemplateHandler.Assign(ref balanceinfo, "{AgentType}", customerInfoViewModel.DownlineName);
                TemplateHandler.Assign(ref balanceinfo, "{hiddenbalance}", "");
                TemplateHandler.Assign(ref balanceinfo, "{ishiddenmember}", "hidden");

                GetDownlineCountingInfo(ref balanceinfo, ds, customerInfoViewModel.RoleId);
            }
            else
            {
                TemplateHandler.Assign(ref balanceinfo, "{BetCredit}", customerInfoViewModel.Currency + " " + Core.Helper.Utils.GetDecimalRoundDown(customerInfoViewModel.BetCredit));

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
