using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class EditCustomerRequest
    {
        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public byte UplineRoleId { get; set; }

        public string UName { get; set; }

        public bool NoPermissionEditCredit { get; set; }

        internal string ToJson()
            => JsonConvert.SerializeObject(new { CustId, RoleId, UplineRoleId, NoPermissionEditCredit }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}