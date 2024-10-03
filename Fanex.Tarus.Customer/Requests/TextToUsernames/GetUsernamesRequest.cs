using Fanex.CustomerSetting.Module.Core.Web;

namespace Fanex.Tarus.Customer.Requests.TextToUsernames
{
    public class GetUsernamesRequest
    {
        public string Text { get; set; }
        public int UserId => AppSession.AccId;
    }
}
