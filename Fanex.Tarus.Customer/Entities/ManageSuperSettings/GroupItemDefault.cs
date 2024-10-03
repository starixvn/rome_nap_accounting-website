namespace Fanex.Tarus.Customer.Entities.ManageSuperSettings
{
    public class GroupItemDefault
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int SportTypeId { get; set; }

        public string SportTypeName { get; set; }

        public int BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public string DisplayName { get; set; }

        public int? SubGroupId { get; set; }

        public string SubGroupName { get; set; }

        public bool IsLive { get; set; }

        public virtual bool ShouldSerializeIsLive() => true;

        public virtual void MapUpline() { }
    }
}
