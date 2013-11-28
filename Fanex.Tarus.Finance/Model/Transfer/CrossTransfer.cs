namespace Fanex.Tarus.Finance.Model.Transfer
{
    public class CrossTransfer
    {
        public bool IsHadPermissionOnCrossCurrencyTransfer { get; set; }

        public bool IsHadPermissionOnYesterdayTransfer { get; set; }

        public string ForexAccount { get; set; }
    }
}