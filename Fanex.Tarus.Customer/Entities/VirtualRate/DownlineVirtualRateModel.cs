namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    public class DownlineVirtualRateModel
    {
        public int RoleId { get; set; }

        public int Count { get; set; }

        public int LockSC { get; set; }

        public int UnlockSC { get => Count - LockSC; }

        public int LockOS { get; set; }

        public int UnlockOS { get => Count - LockOS; }

        public int LockBB { get; set; }

        public int UnlockBB { get => Count - LockBB; }

        public int EnableAB { get; set; }

        public int DisableAB { get => Count - EnableAB; }
    }
}
