namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class StatusEntity
    {
        public StatusEntity()
        {
        }

        public StatusEntity(
            int sportType,
            int betType,
            byte bitMapOrder,
            bool isDisabled,
            bool isDisabledDownline,
            bool hasDownline,
            bool isDisabledUpline)
        {
            SportType = sportType;
            BetType = betType;
            BitmapOrder = bitMapOrder;
            IsDisabled = isDisabled;
            IsDisabledDownline = isDisabledDownline;
            HasDownline = hasDownline;
            IsDisabledUpline = isDisabledUpline;
        }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public byte BitmapOrder { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsDisabledDownline { get; set; }

        public bool HasDownline { get; set; }

        public bool IsDisabledUpline { get; set; }
    }
}
