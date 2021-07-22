namespace Fanex.Tarus.Customer.Criteria.OpenCustomer
{
    using NAP.Framework.Data;

    public class GetSingleOpenDownlinesInfoCriteria : CriteriaBase
    {
        public GetSingleOpenDownlinesInfoCriteria(
            int custId,
            bool isAffected,
            int downlineRoleId,
            int pageSize = 50,
            int pageIndex = 0)
        {
            CustId = custId;
            IsAffected = isAffected;
            DownlineRoleId = downlineRoleId;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int CustId { get; }

        public bool IsAffected { get; }

        public int DownlineRoleId { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public override string GetSettingKey() => "Acc_Customer_CloseInfoDetailSel";

        public override bool IsValid() => CustId > 0;
    }
}