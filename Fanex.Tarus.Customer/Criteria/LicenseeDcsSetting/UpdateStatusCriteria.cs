using Fanex.NAP.Framework.Data;
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
            string ip,
            int custDBGroupId)
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
            CustDBGroupId = custDBGroupId;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public bool IsDisabled { get; }

        public bool IsDisabledDownline { get; }

        public int SportType { get; }

        public int BetType { get; }

        public byte BitmapOrder { get; }

        public bool IsAgentOnly { get; }

        [SpParam(Ignored = true)]
        public int CustDBGroupId { get; }

        public override string GetSettingKey()
        {
            if (CustDBGroupId == 0)
            {
                return "Acc_Dep_CustSetting_StatusUpd";
            }

            return $"SiteDB{CustDBGroupId}_Acc_Dep_CustSetting_StatusUpd";
        }

        public override bool IsValid()
            => base.IsValid() && ProductId > 0;
    }
}