namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetBBinBetDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public long Refno { get; set; }

        public int TransType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_G_Detail";
        }

        public override bool IsValid()
        {
            return Refno > 0;
        }
    }
}
