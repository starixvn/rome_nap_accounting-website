using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.AutoTransfer;

namespace Fanex.Tarus.Customer.Entities
{
    public class UpdateTransferOptionResult
    {
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public List<AffectedDownlinesInfoModel> AffectedDownlinesInfo { get; set; }
    }
}
