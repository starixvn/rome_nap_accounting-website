using System;

namespace Fanex.Tarus.BetList.Models
{
    [Serializable]
    public class SiteModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeposit { get; set; }

        public int SiteDBGroupId { get; set; }
    }
}
