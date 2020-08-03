using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class PTEntity
    {
        public PTEntity()
        {
        }        

        public int SportType { get; set; }

        public string SportTypeName => CacheRef.GetSportName(SportType);

        public int BetType { get; set; }

        public string BetTypeName
        {
            get
            {
                var betTypeName = CacheRef.GetBettype(BetType);

                return betTypeName;
            }
        }

        public bool IsLive { get; set; }

        public decimal AgentPT { get; set; }

        public decimal AgentPTPercent
            => 0 <= AgentPT && AgentPT <= 100 ? AgentPT / 100 : AgentPT;
    }
}
