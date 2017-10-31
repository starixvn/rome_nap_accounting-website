namespace Sunplus.Accounting.Business.Customer
{
    using System.Web.Security;
    using Core.Common;
    using Core.Configurations;
    using Core.Extensions;
    using DNAide.Utils;
    using DNAide.Web;

    public partial class CustomerManager
    {
        private static string RenderCustomerDetail(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            string statusClass = string.Empty;
            string statusTip = string.Empty;
            bool isOpenStatus = true;

            if (!customerInfoViewModel.Enabled)
            {
                statusClass = "memberDisabled ";
                statusTip = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount");
                isOpenStatus = false;
            }
            else if (customerInfoViewModel.Closed)
            {
                var customerClosedReason = new CustomerClosedReason(customerInfoViewModel.ClosedReason);

                statusClass = customerClosedReason.ClassName + " ";
                statusTip = customerClosedReason.ClosedReason;
                isOpenStatus = false;
            }
            else if (customerInfoViewModel.Suspend)
            {
                statusClass = "memberSuspended ";
                statusTip = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuspendedAccount");
                isOpenStatus = false;
            }
            else
            {
                statusClass = " ";
            }

            string shortUserName = customerInfoViewModel.UserName.Length < 20 ? customerInfoViewModel.UserName : customerInfoViewModel.UserName.Substring(0, 20) + "...";

            if (customerInfoViewModel.RoleId == 1)
            {
                TemplateHandler.Assign(ref htmlcontent, "{memberClass}", "memberClass");
                TemplateHandler.Assign(ref htmlcontent, "{onclickUName}", "onclick=\"parent.submitSearch('" + customerInfoViewModel.UserName + "');\" ");
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{cursorhand}", "");
                TemplateHandler.Assign(ref htmlcontent, "{onclickUName}", " ");
            }

            TemplateHandler.Assign(ref htmlcontent, "{statusClass}", statusClass);
            TemplateHandler.Assign(ref htmlcontent, "{UserName}", shortUserName);
            TemplateHandler.Assign(ref htmlcontent, "{FullUserName}", customerInfoViewModel.UserName);
            TemplateHandler.Assign(ref htmlcontent, "{CustomerName}", customerInfoViewModel.Fullname);
            TemplateHandler.Assign(ref htmlcontent, "{FullCustomerName}", customerInfoViewModel.Fullname);
            TemplateHandler.Assign(ref htmlcontent, "{NickName}", customerInfoViewModel.Nickname);
            TemplateHandler.Assign(ref htmlcontent, "{CustomerFax}", customerInfoViewModel.Fax);
            TemplateHandler.Assign(ref htmlcontent, "{CustomerPhone}", customerInfoViewModel.Phone);
            TemplateHandler.Assign(ref htmlcontent, "{CustomerMobile}", customerInfoViewModel.Mobile);
            //info for internal agent login
            string rndStr = Encryption.RandomString(6);
            string accKey = FormsAuthentication.HashPasswordForStoringInConfigFile(AppSession.AccPwd + rndStr, "MD5");
            TemplateHandler.Assign(ref htmlcontent, "{ThemePath}", WebSettings.ThemePath);
            TemplateHandler.Assign(ref htmlcontent, "{RndStr}", rndStr);
            TemplateHandler.Assign(ref htmlcontent, "{AccKey}", accKey);
            TemplateHandler.Assign(ref htmlcontent, "{AccId}", AppSession.AccId.ToString());
            TemplateHandler.Assign(ref htmlcontent, "{InternalAgent}", WebSettings.InternalAgentUrl);

            //Render Enabled
            if (isOpenStatus)
            {
                TemplateHandler.Assign(ref htmlcontent, "{enabledbg}", string.Empty);
                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipover}", string.Empty);
                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipout}", string.Empty);
            }
            else
            {
                TemplateHandler.Assign(ref htmlcontent, "{enabledbg}", "");
                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipover}", "Tip('" + statusTip + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)");
                TemplateHandler.Assign(ref htmlcontent, "{enabledtooltipout}", "UnTip()");
            }

            if (customerInfoViewModel.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "headline");
            }
            else
            {
                if (customerInfoViewModel.Groupheadlineid.Equals(0))
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "haveHeadline");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref htmlcontent, "noHeadline");
                    TemplateHandler.Assign(ref htmlcontent, "{linkHeadline}", string.Format("CreditTerm/Creditterm_Group_Feed.aspx?groupid={0}", customerInfoViewModel.Groupheadlineid));
                    TemplateHandler.Assign(ref htmlcontent, "{Headline}", customerInfoViewModel.Headline);
                }
            }

            return htmlcontent;
        }
    }
}
