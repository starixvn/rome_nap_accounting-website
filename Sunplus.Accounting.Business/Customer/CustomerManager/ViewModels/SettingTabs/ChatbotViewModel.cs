using System.Linq;
using Chatbot.Client.Models;

namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerInfoViewModel
    {
        public User ChatbotUser { get; }

        public bool HideChatbotLink
            => RoleId == 1
            || ChatbotUser == null
            || ChatbotUser.HasNoMessengerAccount
            || !ChatbotUser.MessengerAccounts.Any(a => a.Type == MessengerType.Letstalk && a.Enabled);
    }
}