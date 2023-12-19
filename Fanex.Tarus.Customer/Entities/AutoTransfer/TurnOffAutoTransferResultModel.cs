using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.AutoTransfer
{
    public class TurnOffAutoTransferResultModel
    {
        public string TargetIds { get; set; }
        public string UplineName { get; set; }
        public string DownlineNames { get; set; }

        public List<AffectedDownlinesInfoModel> AffectedDownlines { get; set; }
    }
}
