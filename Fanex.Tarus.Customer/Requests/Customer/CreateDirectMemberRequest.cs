using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class CreateDirectMemberRequest : CreateCustomerRequest
    {
        internal override string ToJson()
            => JsonConvert.SerializeObject(new { UplineId, UplineRoleId, RoleId, Site, SiteLabel, NoPermissionEditCredit, AgentId, productGroupId = 0 }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}