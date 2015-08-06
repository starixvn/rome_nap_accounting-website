namespace Fanex.Tarus.Customer.Criteria.VirtualRate
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// View History Log Criteria.
    /// </summary>
    public class ViewHistoryLogCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_VirtualRate_GetLog";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
