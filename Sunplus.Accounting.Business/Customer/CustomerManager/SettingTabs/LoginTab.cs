namespace Sunplus.Accounting.Business.Customer
{
    using System.Configuration;
    using Core.Common;
    using Core.Web;
    using CustSetting;
    using DNAide.Web;

    public partial class CustomerManager
    {
        private static string GenLoginDetail(
            string htmlcontent, 
            CustomerInfoViewModel customerInfoViewModel)
        {
            string tplLoginDetail = WebUtils.LoadTemplate("CustomerInfo/LoginDetail.htm");
            string lastIp = customerInfoViewModel.LastIp;

            var site = new CustSettingMoveSite();
            AppSession.SiteName = customerInfoViewModel.CustomerSite;
            var siteName = site.GetSiteNameByCustId(customerInfoViewModel.CustId, customerInfoViewModel.RoleId, customerInfoViewModel.CustomerSite, customerInfoViewModel.UserName);

            TemplateHandler.Assign(ref tplLoginDetail, "{createdate}", customerInfoViewModel.Createdate + " " + customerInfoViewModel.Createtime);
            TemplateHandler.Assign(ref tplLoginDetail, "{LastLogin}", customerInfoViewModel.LastLoginDate + customerInfoViewModel.LastLoginTime);
            TemplateHandler.Assign(ref tplLoginDetail, "{LastTxn}", customerInfoViewModel.LastTxnDate + customerInfoViewModel.LastTxnTime + "&nbsp;");
            TemplateHandler.Assign(ref tplLoginDetail, "{PasswordExpiry}", customerInfoViewModel.PwdExpiryDate + customerInfoViewModel.PwdExpiryTime);
            TemplateHandler.Assign(ref tplLoginDetail, "{customersite}", siteName);
            TemplateHandler.Assign(ref tplLoginDetail, "{LastLoginIP}", "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", IpManagement.ReplaceVNIP(lastIp)) + "\" target=\"_blank\" style='font-weight:bold'>" + IpManagement.ReplaceVNIP(lastIp) + "</a>");
            TemplateHandler.Assign(ref tplLoginDetail, "{custid}", customerInfoViewModel.CustId);
            TemplateHandler.Assign(ref tplLoginDetail, "{last_change_security_code_date}", customerInfoViewModel.LastChangeSecurityCodeDate);
            TemplateHandler.Assign(ref htmlcontent, "{logindetail}", tplLoginDetail);
            TemplateHandler.Assign(ref htmlcontent, "{last_change_OTP_date}", customerInfoViewModel.LastChangeOTPCodeDate);

            if (customerInfoViewModel.RoleId > 1)
            {
                TemplateHandler.Assign(ref htmlcontent, "{loginoptions}", customerInfoViewModel.NicknameLoginOnly ? "Nickname Only" : "Username/Nickname");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "OTP");
                TemplateHandler.RemoveBlock(ref htmlcontent, "loginoption");
            }

            return htmlcontent;
        }
    }
}
