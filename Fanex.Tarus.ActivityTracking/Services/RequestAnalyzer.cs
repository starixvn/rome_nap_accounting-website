namespace Fanex.Tarus.ActivityTracking.Services
{
    using Fanex.ActivityTracking.Configurations;
    using Fanex.ActivityTracking.Web;

    public class RequestAnalyzer : BaseRequestAnalyzer
    {
        public RequestAnalyzer() : base(new JsonConverter())
        {
        }
    }
}