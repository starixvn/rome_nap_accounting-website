using System.Collections.Generic;

namespace Fanex.Tarus.Entities.Customer
{
    public class EditSportbookCommissionViewModel
    {
        public List<SportbookCommissionSportTypeBettype> ListSporttypeBettype { get; set; }

        public EditSportbookCommissionViewModel()
        {
            ListSporttypeBettype = new List<SportbookCommissionSportTypeBettype>();
        }
    }

    public class SportbookCommissionSportTypeBettype
    {
        public int SportType { get; set; }

        public int Bettype { get; set; }

        public string BettypeName { get; set; }
    }
}
