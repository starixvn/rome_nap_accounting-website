using Chatbot.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Tarus.Customer.ViewModels.Chatbot
{
    public class ChatbotViewModel
    {
        private IDictionary<string, string> languages => new Dictionary<string, string>
        {
            ["en-US"] = "English",
            ["zh-TW"] = "繁體中文",
            ["zh-CN"] = "简体中文",
            ["ja-JP"] = "日本語",
            ["th-TH"] = "ภาษาไทย",
            ["ko-KR"] = "한국어"
        };

        private IEnumerable<SelectListItem> offerLanguages;

        private User user { get; set; }

        public ChatbotViewModel(string userName, User user)
        {
            UserName = userName;
            this.user = user;
            offerLanguages = languages.Select(l => new SelectListItem
            {
                Text = l.Value,
                Value = l.Key,
                Selected = l.Key.Equals(user.Language)
            });
        }

        public bool AlreadyLinked
            => user != null
            && !user.HasNoMessengerAccount
            && user.MessengerAccounts.Any(a => a.Type == MessengerType.Letstalk && a.Enabled);

        public int UserId => user.Id;

        public int CustId => user.CustId;

        public string UserName { get; }

        public string Language => user.Language;

        public Messenger LetstalkAccount
            => user.MessengerAccounts.FirstOrDefault(m => m.Type == MessengerType.Letstalk && m.Enabled);

        public IEnumerable<SelectListItem> Functions
            => user.Functions.Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.Id.ToString(),
                Selected = f.Enabled
            });

        public IEnumerable<SelectListItem> OfferLanguages
        {
            get { return offerLanguages; }
            set { offerLanguages = value; }
        }

        public IEnumerable<SelectListItem> FeatureFilterItems
        {
            get
            {
                var functions = user.Functions.Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                }).ToList();

                functions.Insert(0, new SelectListItem { Text = "All", Value = "0", Selected = true });
                functions.Insert(functions.Count, new SelectListItem { Text = "Unrecognized commands", Value = "1000000" });

                return functions;
            }
        }
    }
}