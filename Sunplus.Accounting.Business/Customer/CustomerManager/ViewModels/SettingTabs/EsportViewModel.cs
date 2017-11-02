namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.CustomerSetting.Module.Core;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledEsportsParent => !ParentId.Equals(0) && CustParent.IsDisableEsports;

        public bool DisabledEsports => Utils.ToBoolean(customerInfoRow["DisableEsports"]);

        public bool DisabledEsportsDownline => Utils.ToBoolean(customerInfoRow["disabledownlinesesports"]);
    }
}
