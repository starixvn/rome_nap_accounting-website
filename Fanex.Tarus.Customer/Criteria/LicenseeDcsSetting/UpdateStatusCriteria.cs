namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class UpdateStatusCriteria : NonQueryCriteria
    {
        public UpdateStatusCriteria(
            int productId,
            int superId,
            bool isDisabled,
            bool isDisabledDownline,
            int sportType,
            int betType,
            byte bitmapOrder,
            int userId,
            string ip)
        {
            ProductId = productId;
            SuperId = superId;
            IsDisabled = isDisabled;
            IsDisabledDownline = isDisabledDownline;
            SportType = sportType;
            BetType = betType;
            BitmapOrder = bitmapOrder;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public bool IsDisabled { get; }

        public bool IsDisabledDownline { get; }

        public int SportType { get; }

        public int BetType { get; }

        public byte BitmapOrder { get; }

        public override string GetSettingKey()
        {
            return "Acc_Dep_CustSetting_StatusUpd";
        }

        public override bool IsValid()
        {
            return base.IsValid() && SportType > 0 && BetType > 0;
        }
    }
}
