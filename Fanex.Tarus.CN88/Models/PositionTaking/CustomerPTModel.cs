using Fanex.Tarus.CN88.Helpers;

namespace Fanex.Tarus.CN88.Models.PositionTaking
{
    public class CustomerPTModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string SportsbookPT { get; set; }

        public string NonSportPT { get; set; }

        public string OldSportsbookPT { get; set; }

        public string OldNonSportPT { get; set; }

        public bool IsSportsbookChanged => PositionTakingHelper.IsUpdatePTTime() && OldSportsbookPT != SportsbookPT;

        public bool IsNonSportChanged => PositionTakingHelper.IsUpdatePTTime() && OldNonSportPT != NonSportPT;
    }
}
