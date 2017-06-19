namespace Fanex.Tarus.ActivityTracking.Models
{
    using System;
    using System.Web;
    using Fanex.ActivityTracking.Models;
    using Infrastructure.Common;

    public class NapUserSession : IUserSession
    {
        public int AdminId => AppSession.AccId;

        public string AdminName => AppSession.AccUserName;

        public int CustId => 0;

        public string CustName => string.Empty;

        public string SessionID => HttpContext.Current.Session.SessionID;
    }
}