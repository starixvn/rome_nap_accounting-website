namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class UpdatePositionTakingCriteria : NonQueryCriteria
    {
        public UpdatePositionTakingCriteria(
            int productId,
            int superId,
            decimal superPT,
            int userId,
            string ip)
        {
            ProductId = productId;
            SuperId = superId;
            PT = superPT;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public decimal PT { get; }

        public override string GetSettingKey()
        {
            return "Acc_Dep_CustSetting_PositionTakingSettingUpd";
        }

        public override bool IsValid()
        {
            return base.IsValid() && ProductId > 0 && 0 <= PT && PT <= 100;
        }
    }
}
