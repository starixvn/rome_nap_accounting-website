namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.Common;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledEsportsParent => !ParentId.Equals(0) && CustParent.IsDisableEsports;

        public bool DisabledEsports => Converter.ToBool(customerInfoRow["DisableEsports"]);

        public bool DisabledEsportsDownline => Converter.ToBool(customerInfoRow["disabledownlinesesports"]);
    }
}
