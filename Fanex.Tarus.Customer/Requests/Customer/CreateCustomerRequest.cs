using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Sunplus.Accounting.Business;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class CreateCustomerRequest
    {
        public int UplineId { get; set; } = 0;

        public byte RoleId { get; set; }

        public byte UplineRoleId { get; set; } = 0;

        public string UplineUsername { get; set; } = string.Empty;

        public string Site { get; set; } = string.Empty;

        public string SiteLabel { get; set; } = string.Empty;

        public bool NoPermissionEditCredit { get; set; } = false;

        internal virtual string ToJson()
            => JsonConvert.SerializeObject(new { UplineId, UplineRoleId, RoleId, Site, SiteLabel, NoPermissionEditCredit, ProductGroupId = ProductGroupIdConstants.SportsBook }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}