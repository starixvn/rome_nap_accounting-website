using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class SportBookInformationRequest
    {
        public int CustId { get; set; }

        public byte RoleId { get; set; }
        
        public byte UplineRoleId { get; set; }
        
        public int TabIndex { get; set; }
        
    }
}