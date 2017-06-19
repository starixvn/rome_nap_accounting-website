namespace Fanex.Tarus.ActivityTracking.Web
{
    using System.Web;
    using Fanex.ActivityTracking.Configurations;
    using Fanex.ActivityTracking.Web;
    using Models;
    using Services;

    public class NapActivityTrackingHttpModule : ActivityTrackingHttpModule
    {
        public NapActivityTrackingHttpModule()
            : base(InitHttpModule, new NapUserSession(), new RequestAnalyzer())
        {
        }

        public static void InitHttpModule()
        {
            if (AppSettings.ActivityTrackingConfig == null
                && HttpContext.Current != null
                && HttpContext.Current.Server != null)
            {
                AppSettings.Init(HttpContext.Current.Server.MapPath(AppSettings.ActivityTrackingConfigFilePath));
            }
        }
    }
}