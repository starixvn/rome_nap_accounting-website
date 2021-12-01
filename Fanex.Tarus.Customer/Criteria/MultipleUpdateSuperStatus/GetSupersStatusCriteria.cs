namespace Fanex.Tarus.Customer.Criteria.MultipleUpdateSuperStatus
{
    using Fanex.NAP.Framework.Data;

    public class GetSupersStatusCriteria : CriteriaBase
    {
        public string SuperList { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_CopySetting_CustomerSelStatus";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(SuperList);
    }
}
