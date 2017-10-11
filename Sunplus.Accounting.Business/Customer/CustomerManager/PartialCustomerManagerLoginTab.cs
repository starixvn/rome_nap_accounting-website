using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Fanex.Common;
using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Configurations;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Criteria;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Repositories.Impl;
using Fanex.LiveCasino.CoreSetting.Services;
using Fanex.Module.NG2.Entities;
using Fanex.Module.NG2.Services;
using Fanex.Tarus.Security.Criteria;
using Fanex.Tarus.Security.Domain;
using Fanex.Tarus.Security.Services;
using IBCUtils.Objects;
using RemotingRef;
using Sunplus.Accounting.Business.Criteria;
using Sunplus.Accounting.Business.Customer.ConfigureDirectAccount;
using Sunplus.Accounting.Business.CustSetting;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.AdoNet;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Bingo.Core;
using Result = Sunplus.Accounting.Entities.Result;

namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerManager
    {
        private void GenLoginDetail(ref string htmlCustInfo, int custId, int roleId, DataRow loginData, DataRow transData, string username)
        {
            try
            {
                string tplLoginDetail = WebUtils.LoadTemplate("CustomerInfo/LoginDetail.htm");
                string lastLoginDate = Convert.IsDBNull(loginData["lastlogged"]) ? "" : Convert.ToDateTime(loginData["lastlogged"]).ToShortDateString();
                string lastLoginTime = Convert.IsDBNull(loginData["lastlogged"]) ? "" : " " + Convert.ToDateTime(loginData["lastlogged"]).ToShortTimeString();
                string lastIp = Convert.IsDBNull(loginData["lastloginip"]) ? "" : loginData["lastloginip"].ToString();
                string customerSite = (roleId == (int)Globals.AccountType.Member) ? CastAide.AsString(loginData["membersite"], "") : CastAide.AsString(loginData["site"], "");
                string lastChangeSecurityCodeDate =
                    Convert.IsDBNull(loginData["LastChangedSecurityCodeDate"]) ?
                    string.Empty :
                    Convert.ToString(loginData["LastChangedSecurityCodeDate"]);

                string lastChangeOTPCodeDate =
                  Convert.IsDBNull(loginData["OTPCreateDate"]) ?
                  string.Empty :
                  Convert.ToString(loginData["OTPCreateDate"]);

                var site = new CustSettingMoveSite();
                AppSession.SiteName = customerSite;
                customerSite = site.GetSiteNameByCustId(custId, roleId, customerSite, username);

                string createdate = Convert.IsDBNull(loginData["createdate"]) ? "" : Convert.ToDateTime(loginData["createdate"]).ToShortDateString();
                string createtime = Convert.IsDBNull(loginData["createdate"]) ? "" : Convert.ToDateTime(loginData["createdate"]).ToShortTimeString();
                //--------
                string pwdExpiryDate = Convert.IsDBNull(loginData["pwdexpiry"]) ? "" : Convert.ToDateTime(loginData["pwdexpiry"]).ToShortDateString();
                string pwdExpiryTime = Convert.IsDBNull(loginData["pwdexpiry"]) ? "" : " " + Convert.ToDateTime(loginData["pwdexpiry"]).ToShortTimeString();
                string lastTxnDate = string.Empty;
                string lastTxnTime = string.Empty;
                if (transData != null && roleId <= 4)
                {
                    lastTxnDate = Convert.IsDBNull(transData["lastTxn"]) ? "" : Convert.ToDateTime(transData["lastTxn"]).ToShortDateString();
                    lastTxnTime = Convert.IsDBNull(transData["lastTxn"]) ? "" : " " + Convert.ToDateTime(transData["lastTxn"]).ToShortTimeString();
                }

                TemplateHandler.Assign(ref tplLoginDetail, "{createdate}", createdate + " " + createtime);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastLogin}", lastLoginDate + lastLoginTime);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastTxn}", lastTxnDate + lastTxnTime + "&nbsp;");
                TemplateHandler.Assign(ref tplLoginDetail, "{PasswordExpiry}", pwdExpiryDate + pwdExpiryTime);
                TemplateHandler.Assign(ref tplLoginDetail, "{customersite}", customerSite);
                TemplateHandler.Assign(ref tplLoginDetail, "{LastLoginIP}", "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", IpManagement.ReplaceVNIP(lastIp)) + "\" target=\"_blank\" style='font-weight:bold'>" + IpManagement.ReplaceVNIP(lastIp) + "</a>");
                TemplateHandler.Assign(ref tplLoginDetail, "{custid}", custId);
                TemplateHandler.Assign(ref tplLoginDetail, "{last_change_security_code_date}", lastChangeSecurityCodeDate);
                TemplateHandler.Assign(ref htmlCustInfo, "{logindetail}", tplLoginDetail);
                TemplateHandler.Assign(ref htmlCustInfo, "{last_change_OTP_date}", lastChangeOTPCodeDate);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}
