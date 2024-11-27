using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus;

namespace Fanex.Tarus.Customer.Requests.ManageSuperSettings
{
    public class EditSuperInfoRequest
    {
        public List<SuperStatusEntity> Supers { get; set; }

        public EditGivenCreditRequest EditGivenCreditRequest { get; set; }
        public EditPasswordRequest EditPasswordRequest { get; set; }
        public EditSecurityCodeRequest EditSecurityCodeRequest { get; set; }
    }
}
