namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Configuration;
    using Core.Common;
    using Core.Extensions;
    using Core.Web;
    using CustSetting;
    using DNAide.Web;

    public partial class CustomerManager
    {
        private void GenLoginDetail(
            ref string htmlCustInfo, 
            int custId, 
            int roleId, 
            string username,
            CustomerInfoViewModel customerInfoViewModel)
        {
            try
            {
                string tplLoginDetail = WebUtils.LoadTemplate("CustomerInfo/LoginDetail.htm");
                string lastIp = customerInfoViewModel.lastIp;

                var site = new CustSettingMoveSite();
                AppSession.SiteName = customerInfoViewModel.customerSite;
                var siteName = site.GetSiteNameByCustId(custId, roleId, customerInfoViewModel.customerSite, username);

                TemplateHandler.Assign(ref tplLoginDetail, "{createdate}", customerInfoViewModel.createdate + " " + customerInfoViewModel.createtime);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastLogin}", customerInfoViewModel.lastLoginDate + customerInfoViewModel.lastLoginTime);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastTxn}", customerInfoViewModel.lastTxnDate + customerInfoViewModel.lastTxnTime + "&nbsp;");
                TemplateHandler.Assign(ref tplLoginDetail, "{PasswordExpiry}", customerInfoViewModel.pwdExpiryDate + customerInfoViewModel.pwdExpiryTime);
                TemplateHandler.Assign(ref tplLoginDetail, "{customersite}", siteName);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastLoginIP}", "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", IpManagement.ReplaceVNIP(lastIp)) + "\" target=\"_blank\" style='font-weight:bold'>" + IpManagement.ReplaceVNIP(lastIp) + "</a>");
                TemplateHandler.Assign(ref tplLoginDetail, "{custid}", custId);
                TemplateHandler.Assign(ref tplLoginDetail, "{last_change_security_code_date}", customerInfoViewModel.lastChangeSecurityCodeDate);
                TemplateHandler.Assign(ref htmlCustInfo, "{logindetail}", tplLoginDetail);
                TemplateHandler.Assign(ref htmlCustInfo, "{last_change_OTP_date}", customerInfoViewModel.lastChangeOTPCodeDate);

                if (roleId > 1)
                {
                    TemplateHandler.Assign(ref htmlCustInfo, "{loginoptions}", customerInfoViewModel.NicknameLoginOnly ? "Nickname Only" : "Username/Nickname");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref htmlCustInfo, "OTP");
                    TemplateHandler.RemoveBlock(ref htmlCustInfo, "loginoption");
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}
