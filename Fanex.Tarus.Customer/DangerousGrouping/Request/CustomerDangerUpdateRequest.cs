using Fanex.Tarus.Customer.DangerousGrouping.Enums;

namespace Fanex.Tarus.Customer.DangerousGrouping.Request
{
    public class CustomerDangerUpdateRequest
    {

        public string Customer { get; set; }

        public bool UpdateDownline { get; set; }

        public DangerType DangerType { get; set; }

        public int DangerLevel { get; set; }
    }
}