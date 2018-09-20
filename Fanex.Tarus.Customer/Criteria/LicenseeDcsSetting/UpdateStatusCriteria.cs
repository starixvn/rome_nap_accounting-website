using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class UpdateStatusCriteria : NonQueryCriteria
    {
        public UpdateStatusCriteria(
            int productId,
            int custId,
            StatusEntity status,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            ProductId = productId;
            CustId = custId;
            IsDisabled = status.IsDisabled;
            IsDisabledDownline = status.IsDisabledDownline;
            SportType = status.SportType;
            BetType = status.BetType;
            BitmapOrder = status.BitmapOrder;
            IsAgentOnly = isAgentOnly;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public bool IsDisabled { get; }

        public bool IsDisabledDownline { get; }

        public int SportType { get; }

        public int BetType { get; }

        public byte BitmapOrder { get; }

        public bool IsAgentOnly { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_StatusUpd";

        public override bool IsValid()
            => base.IsValid() && ProductId > 0;
    }
}
