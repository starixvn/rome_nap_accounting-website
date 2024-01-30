using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class CreateCustomerRequest
    {
        public int UplineId { get; set; } = 0;

        public byte UplineRoleId { get; set; } = 0;

        public string UplineUsername { get; set; } = string.Empty;

        public string Site { get; set; } = string.Empty;

        public string SiteLabel { get; set; } = string.Empty;

        public bool NoPermissionEditCredit { get; set; } = false;
        
        internal string ToJson()
            => JsonConvert.SerializeObject(new { UplineId, UplineRoleId, Site, SiteLabel, NoPermissionEditCredit }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}