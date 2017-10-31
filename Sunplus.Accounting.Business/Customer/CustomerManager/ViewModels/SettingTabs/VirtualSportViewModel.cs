namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool DisalbedVSParent => !ParentId.Equals(0) && CustParent.IsDisableVirtualSports;

        public bool IsHideVirtualSports { get; private set; } = true;
    }
}
