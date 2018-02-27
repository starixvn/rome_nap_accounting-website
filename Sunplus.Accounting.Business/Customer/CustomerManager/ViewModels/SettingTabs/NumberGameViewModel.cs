namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.CustomerSetting.Module.Core;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledNumberGame => Utils.ToBoolean(customerInfoRow["DisableNumber"]);

        public bool DisabledDownlinesNumberGame => false;
    }
}
