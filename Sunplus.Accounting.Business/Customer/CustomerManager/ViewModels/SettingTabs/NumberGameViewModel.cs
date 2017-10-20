namespace Sunplus.Accounting.Business.Customer
{
    using DNAide.Core;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledNumberGame => CastAide.AsBoolean(row1["DisableNumber"], false);

        public bool DisabledDownlinesNumberGame => CastAide.AsBoolean(row1["DisableDownlinesNumber"], false);
    }
}
