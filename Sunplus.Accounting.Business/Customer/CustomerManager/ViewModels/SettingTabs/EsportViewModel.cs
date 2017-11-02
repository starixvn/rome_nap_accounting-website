namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Configurations;
    using Fanex.Common;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool DisabledEsportsParent => !ParentId.Equals(0) && CustParent.IsDisableEsports;

        public bool DisabledEsports => Converter.ToBool(customerInfoRow["DisableEsports"]);

        public bool DisabledEsportsDownline => Converter.ToBool(customerInfoRow["disabledownlinesesports"]);

        public bool RemoveRnGCasinoLink
           => !Permissions.IsAllowCasino() && (WebSettings.disableCS || ((RoleId < (int)Globals.AccountType.Super) && !AllowEditRnGCasino));
    }
}
