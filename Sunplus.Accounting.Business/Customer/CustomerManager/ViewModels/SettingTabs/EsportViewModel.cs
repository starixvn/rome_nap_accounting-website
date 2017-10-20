namespace Sunplus.Accounting.Business.Customer
{
    using System;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledEsportsParent => !ParentId.Equals(0) && CustParent.IsDisableEsports;

        public bool DisabledEsports => !Convert.IsDBNull(row1["DisableEsports"]) && Convert.ToBoolean(row1["DisableEsports"]);

        public bool DisabledEsportsDownline => !Convert.IsDBNull(row1["disabledownlinesesports"]) && Convert.ToBoolean(row1["disabledownlinesesports"]);
    }
}
