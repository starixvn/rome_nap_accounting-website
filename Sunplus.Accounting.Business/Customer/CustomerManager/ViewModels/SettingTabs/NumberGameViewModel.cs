namespace Sunplus.Accounting.Business.Customer
{
    using DNAide.Core;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledNumberGame => CastAide.AsBoolean(customerInfoRow["DisableNumber"], false);

        public bool DisabledDownlinesNumberGame => CastAide.AsBoolean(customerInfoRow["DisableDownlinesNumber"], false);
    }
}
