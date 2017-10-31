namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerInfoViewModel
    {
        public bool IsHideSportsbook2 { get; private set; } = true;

        public bool IsShowSportsbook2 => !IsHideSportsbook2 && !IsDeposit;
    }
}
