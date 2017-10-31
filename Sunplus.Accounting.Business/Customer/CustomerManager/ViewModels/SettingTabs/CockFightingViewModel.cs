namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerInfoViewModel
    {
        public bool IsHideCockFighting { get; private set; } = true;

        public bool IsShowCockFighting => !IsHideCockFighting && !IsDeposit;
    }
}
