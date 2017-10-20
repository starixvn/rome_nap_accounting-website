namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerInfoViewModel
    {
        public bool IsHideCockFighting { get; set; } = true;

        public bool IsShowCockFighting => !IsHideCockFighting && !IsDeposit;
    }
}
