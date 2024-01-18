using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class EditCustomerRequest
    {
        public int CustId { get; set; }

        public int RoleId { get; set; }

        public string UName { get; set; }

        public string SiteName { get; set; }

        internal string ToJson()
            => JsonConvert.SerializeObject(new { CustId, RoleId }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}