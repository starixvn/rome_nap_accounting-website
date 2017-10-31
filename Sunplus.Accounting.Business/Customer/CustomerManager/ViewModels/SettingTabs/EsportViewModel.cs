namespace Sunplus.Accounting.Business.Customer
{
    using System;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledEsportsParent => !ParentId.Equals(0) && CustParent.IsDisableEsports;

        public bool DisabledEsports => !Convert.IsDBNull(customerInfoRow["DisableEsports"]) && Convert.ToBoolean(customerInfoRow["DisableEsports"]);

        public bool DisabledEsportsDownline => !Convert.IsDBNull(customerInfoRow["disabledownlinesesports"]) && Convert.ToBoolean(customerInfoRow["disabledownlinesesports"]);
    }
}
