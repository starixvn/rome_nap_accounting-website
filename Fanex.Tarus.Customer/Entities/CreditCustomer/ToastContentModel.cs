using System.Resources;

namespace Fanex.Tarus.Customer.CreditCustomer.Entities
{
    public class ToastContentModel
    {
        public ToastContentModel(ResourceManager resource, string key,string langue)
        {
            Title = resource.GetString(key + "ToastTitle_" + langue);
            Body = resource.GetString(key + "ToastMessage_" + langue);
        }

        public ToastContentModel(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public string Title { get; set; }
        public string Body { get; set; }
    }
}
