namespace Fanex.Tarus.Customer.Criteria.MicroGaming
{
    using Fanex.NAP.Framework.Data;

    public class EditSettingCriteria : CriteriaBase
    {
        public int ParentId { get; set; }

        public int CustId { get; set; }

        public int RoleId { get; set; }
    }
}
