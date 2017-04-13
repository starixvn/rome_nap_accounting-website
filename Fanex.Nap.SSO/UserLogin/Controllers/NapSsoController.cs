// --------------------------------------------------------------------------
// <copyright file="NapSsoController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.NAP.SSO
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.Tarus.Admin.Core.Configurations;

    /// <summary>
    /// The Nap Single sign on controller.
    /// </summary>
    public class NapSsoController : Controller
    {
        #region members

        private ICurrentContext _currentContext;
        private ICurrentSession _currentSession;
        private IUserService _userService;
        private int _userId = 0;

        #endregion members

        #region ctors

        public NapSsoController()
            : this(null)
        {
        }

        public NapSsoController(IUserService userService)
        {
            this._userService = userService ?? new UserService();
            this._currentContext = new CurrentContext();
        }

        #endregion ctors

        #region login

        /// <summary>
        /// Site Name.
        /// </summary>
        private enum SiteName
        {
            /// <summary>
            /// Nap accounting report.
            /// </summary>
            Report = 3,

            /// <summary>
            /// Customer management system.
            /// </summary>
            CMS = 84,

            /// <summary>
            /// The Invoice management system.
            /// </summary>
            IMS = 73,

            /// <summary>
            /// The special delete.
            /// </summary>
            SpecialDelete = 70,

            /// <summary>
            /// The NAP home.
            /// </summary>
            Home = 0
        }

        /// <summary>
        /// Login Nap Home.
        /// </summary>
        /// <returns>
        /// Return site.
        /// </returns>
        [ValidateInput(false)]
        public ActionResult Index()
        {
            // check-in user
            _userId = GetUserIdBySite(SiteName.Home);
            if (_userId != 0)
            {
                var checkInUser = UserCheckin(Converter.ToInt(ConfigurationManager.AppSettings["NAPHomeSiteId"], 0), _userId);
                return new RedirectResult(
                    string.Format(
                        NapURL(SiteName.Home) + "?userId={0}&onuserId={1}&time={2}",
                        Cryptography.CfsEnCrypt(_userId.ToString(CultureInfo.CurrentCulture)),
                        Cryptography.CfsEnCrypt(checkInUser.OnUserId.ToString(CultureInfo.CurrentCulture)),
                        Cryptography.CfsEnCrypt(GetServerTime())));
            }
            else
            {
                return new RedirectResult("Index.aspx");
            }
        }

        /// <summary>
        /// Login Nap CMS.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Return site.
        /// </returns>
        [ValidateInput(false)]
        public ActionResult Cms(string id)
        {
            // check-in user
            _userId = GetUserIdBySite(SiteName.CMS);
            var urlKey = string.IsNullOrEmpty(id) ? string.Empty : GetNAPReportDirectURL(id);
            if (_userId != 0)
            {
                var checkInUser = UserCheckin(Converter.ToInt(ConfigurationManager.AppSettings["NAPCMSSiteId"], 0), _userId);
                return new RedirectResult(
                    string.Format(
                        "{0}?userId={1}&onuserId={2}&time={2}&redirectUrl={3}&{4}",
                        NapURL(SiteName.CMS),
                        Cryptography.CfsEnCrypt(_userId.ToString(CultureInfo.CurrentCulture)),
                        Cryptography.CfsEnCrypt(checkInUser.OnUserId.ToString(CultureInfo.CurrentCulture)),
                        Cryptography.CfsEnCrypt(GetServerTime()),
                        urlKey,
                        NewTab()));
            }
            else
            {
                return new RedirectResult("Index.aspx");
            }
        }

        /// <summary>
        /// Login Nap Invoice.
        /// </summary>
        /// <param name="redirectURL">The redirect url on IMS .</param>
        /// <returns>
        /// Return site.
        /// </returns>
        [ValidateInput(false)]
        public ActionResult Invoice(Uri redirectURL)
        {
            // check-in user
            _userId = GetUserIdBySite(SiteName.Report);
            if (_userId != 0)
            {
                var checkInUser = UserCheckin(Converter.ToInt(ConfigurationManager.AppSettings["NAPIMSSiteId"], 0), _userId);
                return new RedirectResult(
                    string.Format(
                        NapURL(SiteName.IMS) + "?userId={0}&onuserId={1}&time={2}&redirectUrl={3}&newTab=false",
                        Cryptography.CfsEnCrypt(_userId.ToString(CultureInfo.CurrentCulture)),
                        Cryptography.CfsEnCrypt(checkInUser.OnUserId.ToString(CultureInfo.CurrentCulture)),
                        Cryptography.CfsEnCrypt(GetServerTime()),
                        string.IsNullOrEmpty(redirectURL.ToString()) ? string.Empty : redirectURL.ToString()));
            }
            else
            {
                return new RedirectResult("Index.aspx");
            }
        }

        /// <summary>
        /// Gets the server time.
        /// </summary>
        /// <returns>Return server time.</returns>
        private string GetServerTime()
        {
            string time = this._userService.UserSSOGetTime();
            return time;
        }

        /// <summary>
        /// Users the check in.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Return User Checked In.</returns>
        private UserCheckInUserCriteria UserCheckin(int siteId, int userId)
        {
            var criteria = new UserCheckInUserCriteria
            {
                UserId = userId,
                SiteId = siteId,
                NetSessionId = Session.SessionID,
                Ip = _currentContext.IP,
                LoginDate = DateTime.Now,
                BrowserName = _currentContext.BrowserName,
                CountryCode = _currentContext.CountryCode,
                Domain = _currentContext.DomainName,
                Platform = _currentContext.Platform
            };

            this._userService.UserCheckIn(ref criteria);
            return criteria;
        }

        /// <summary>
        /// Naps the URL.
        /// </summary>
        /// <param name="site">Name of the redirect nap site.</param>
        /// <returns>A Specific Nap URL.</returns>
        private string NapURL(SiteName site)
        {
            Uri uri = HttpContext.Request.Url;
            string loginUrl = string.Empty;
            switch (site)
            {
                case SiteName.Report:
                    loginUrl = ConfigurationManager.AppSettings["NapReportLoginUrl"];
                    break;

                case SiteName.SpecialDelete:
                    loginUrl = ConfigurationManager.AppSettings["NapSpecialDeleteLoginUrl"];
                    break;

                case SiteName.CMS:
                    loginUrl = ConfigurationManager.AppSettings["NapCMSLoginUrl"];
                    break;

                case SiteName.IMS:
                    loginUrl = ConfigurationManager.AppSettings["IMSLoginUrl"];
                    break;

                case SiteName.Home:
                    loginUrl = ConfigurationManager.AppSettings["NapHomeSSOUrl"];
                    break;

                default:
                    loginUrl = ConfigurationManager.AppSettings["NapReportLoginUrl"];
                    break;
            }

            return string.Format(loginUrl, (uri.Host.Contains("oneops.net") ? "https" : uri.Scheme) + Uri.SchemeDelimiter + HttpContext.Request.Headers["Host"].ToString());
        }

        /// <summary>
        /// Gets the user from site identifier.
        /// </summary>
        /// <param name="site">The site name.</param>
        /// <returns>
        /// Return account login id.
        /// </returns>
        private int GetUserIdBySite(SiteName site)
        {
            int userId;
            switch (site)
            {
                case SiteName.Report:
                case SiteName.SpecialDelete:
                case SiteName.CMS:
                case SiteName.IMS:
                    this._currentSession = new Fanex.NAP.Core.Web.Session.CurrentSession();
                    userId = this._currentSession.UserId;
                    break;
                case SiteName.Home:
                    userId = Converter.ToInt(HttpContext.Session["AccId"], 0);
                    break;
                default:
                    userId = Converter.ToInt(HttpContext.Session["AccId"], 0);
                    break;
            }

            return userId;
        }

        /// <summary>
        /// Get new the tab parameter.
        /// </summary>
        /// <returns>New Tab parameter.</returns>
        private string NewTab()
        {
            string newTab = "newTab=false";
            if (Request.Url.ToString().Contains("blank"))
            {
                newTab = "newTab=true";
            }

            return newTab;
        }

        /// <summary>
        /// Gets the nap direct URL.
        /// </summary>
        /// <param name="urlKey">The URL key.</param>
        /// <returns>Nap direct URL.</returns>
        private string GetNAPReportDirectURL(string urlKey)
        {
            var napDirectURL = string.Empty;
            var mappingUrlSection = MappingNapCMSUrlSection.Instance.NapCMSUrl;
            if (mappingUrlSection != null)
            {
                var getMappingConfig = from m in 
                                           mappingUrlSection.Cast<NapCMSUrlElement>().Where(
                                                m => m.AccUrl.ToLower().Equals(urlKey.ToLower()) && m.Active) 
                                       select m;
                if (getMappingConfig != null && getMappingConfig.Count() > 0)
                {
                    napDirectURL = getMappingConfig.First().NapUrl;
                }
            }

            return napDirectURL;
        }

        #endregion login
    }
}