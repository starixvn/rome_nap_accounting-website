using Fanex.Tarus.Customer.DangerousGrouping.Enums;

namespace Fanex.Tarus.Customer.DangerousGrouping.Request
{
    public class MultipleCustomersDangerUpdateRequest
    {
        public string CustomerNames { get; set; }

        public bool UpdateDownline { get; set; }

        public DangerType DangerType { get; set; }

        public int DangerLevel { get; set; }
    }
}