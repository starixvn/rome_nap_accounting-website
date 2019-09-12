namespace Fanex.Tarus.CN88.Models.PositionTaking
{
    using System.Collections.Generic;

    public class SportCommModel
    {
        public SportCommModel()
        {
            HandicapGroup = new Dictionary<string, decimal>();
            InplayHandicapGroup = new Dictionary<string, decimal>();
        }

        public Dictionary<string, decimal> HandicapGroup { get; set; }
        public Dictionary<string, decimal> InplayHandicapGroup { get; set; }

        public decimal _1x2Group { get; set; }
        public decimal MoneylineGroup { get; set; }
        public decimal OtherGroup { get; set; }
        public decimal InplayOtherGroup { get; set; }
        public decimal OutrightGroup { get; set; }
    }
}
