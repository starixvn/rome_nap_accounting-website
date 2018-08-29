using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Chatbot.Client.Models;

namespace Fanex.Tarus.Customer.ViewModels.Chatbot
{
    public class ChatbotViewModel
    {
        private readonly IDictionary<string, string> languages = new Dictionary<string, string>
        {
            ["en-US"] = "English",
            ["zh-TW"] = "繁體中文",
            ["zh-CN"] = "简体中文",
            ["ja-JP"] = "日本語",
            ["th-TH"] = "ภาษาไทย",
            ["ko-KR"] = "한국어",
        };

        private readonly User user;

        public ChatbotViewModel(string userName, User user)
        {
            UserName = userName;
            this.user = user;
        }

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
            => languages.Select(l => new SelectListItem
            {
                Text = l.Value,
                Value = l.Key,
                Selected = l.Key.Equals(user.Language)
            });

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