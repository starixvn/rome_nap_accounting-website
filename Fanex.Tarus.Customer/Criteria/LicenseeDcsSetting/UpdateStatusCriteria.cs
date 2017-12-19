using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class UpdateStatusCriteria : NonQueryCriteria
    {
        public UpdateStatusCriteria(
            int productId,
            int superId,
            StatusEntity status,
            int userId,
            string ip)
        {
            ProductId = productId;
            SuperId = superId;
            IsDisabled = status.IsDisabled;
            IsDisabledDownline = status.IsDisabledDownline;
            SportType = status.SportType;
            BetType = status.BetType;
            BitmapOrder = status.BitmapOrder;
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
            return base.IsValid() && ProductId > 0;
        }
    }
}
