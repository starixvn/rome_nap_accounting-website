namespace Fanex.Tarus.BetList.Criteria
{
    using NAP.Framework.Data;

    public class GetSiteGroupIdByRefNoCriteria : CriteriaBase
    {
        public GetSiteGroupIdByRefNoCriteria(string refno, bool isRefNo = false)
        {
            RefNo = refno;
            IsRefNo = isRefNo;
        }
        public string RefNo { get; set; }

        public bool IsRefNo { get; set; }

        public string SiteDBGroupId { get; set; }

        public override string GetSettingKey()
            => "Acc_BetList_SelSiteDBByRefNo";

        public override bool IsValid()
            => !string.IsNullOrEmpty(RefNo);
    }
}