namespace Fanex.Tarus.Infrastructure.Models
{
    using System.Collections.Generic;

    public class BetTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GameType { get; set; }

        public List<BetTypeModel> Children { get; set; }
    }
}
