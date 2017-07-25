namespace Fanex.Tarus.ActivityTracking.Web
{
    using Fanex.ActivityTracking.Web;
    using Models;
    using Services;

    public class NapActivityTrackingHttpModule : ActivityTrackingHttpModule
    {
        public NapActivityTrackingHttpModule()
            : base(new NapUserSession(), new RequestAnalyzer())
        {
        }
    }
}