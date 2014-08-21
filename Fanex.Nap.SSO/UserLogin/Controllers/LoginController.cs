// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.NAP.SSO
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Context;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// Login controller.
    /// </summary>
    public class LoginController : Controller
    {
        #region members

        private ICurrentContext _currentContext;
        private IUserService _userService;

        #endregion members

        #region ctors

        public LoginController()
            : this(null)
        {
        }

        public LoginController(IUserService userService)
        {
            this._userService = userService ?? new UserService();
            this._currentContext = new CurrentContext();
        }

        #endregion ctors

        #region login

        /// <summary>
        /// Login indexes page.
        /// </summary>
        /// <returns>Return site.</returns>
        [ValidateInput(false)]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Index()
        {
            // check-in user
            if (AppSession.AccId != 0)
            {
                var criteria = new UserCheckInUserCriteria
                {
                    UserId = AppSession.OnAccId,
                    SiteId = 100,
                    NetSessionId = Session.SessionID,
                    Ip = _currentContext.IP,
                    LoginDate = DateTime.Now,
                    BrowserName = _currentContext.BrowserName,
                    CountryCode = _currentContext.CountryCode,
                    Domain = _currentContext.DomainName,
                    Platform = _currentContext.Platform
                };

                this._userService.UserCheckIn(ref criteria);

                string time = this._userService.UserSSOGetTime();

                return new RedirectResult(
                    string.Format(
                         NapURL() + "?userId={0}&onlineUserId={1}&time={2}",
                        Cryptography.CfsEnCrypt(AppSession.AccId.ToString()),
                        Cryptography.CfsEnCrypt(criteria.OnUserId.ToString()),
                        Cryptography.CfsEnCrypt(time)));
            }
            else
            {
                return new RedirectResult("Index.aspx");
            }
        }

        private string NapURL()
        {
            Uri uri = HttpContext.Request.Url;
            return string.Format(ConfigurationManager.AppSettings["NapReportLoginUrl"], uri.Scheme + Uri.SchemeDelimiter + HttpContext.Request.Headers["Host"].ToString());
        }

        #endregion login
    }
}