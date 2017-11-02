namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.Common;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledNumberGame => Converter.ToBool(customerInfoRow["DisableNumber"]);

        public bool DisabledDownlinesNumberGame => Converter.ToBool(customerInfoRow["DisableDownlinesNumber"]);
    }
}
