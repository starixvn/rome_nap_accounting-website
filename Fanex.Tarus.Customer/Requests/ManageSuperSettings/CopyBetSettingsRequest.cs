using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fanex.Tarus.Customer.Requests.ManageSuperSettings
{
    public class CopyBetSettingsRequest : CheckSuperListRequest
    {
        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public byte RoleId { get; set; }

        public byte UplineRoleId { get; set; }

        public int ProductGroupId { get; set; }

        public int CustId { get; set; }

        internal string ToJson() => JsonConvert.SerializeObject(new
        {
            CustId,
            CustIds,
            Usernames,
            RoleId,
            UplineRoleId,
            ProductGroupId,
            IsCopyBetSetting = IsCopy,
        }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}
