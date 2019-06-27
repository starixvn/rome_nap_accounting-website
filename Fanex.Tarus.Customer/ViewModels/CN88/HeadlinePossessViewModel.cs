using System;

namespace Fanex.Tarus.Customer.ViewModels.CN88
{
    public class HeadlinePossessViewModel
    {
        public HeadlinePossessViewModel() : this(false)
        { }

        public HeadlinePossessViewModel(bool isAfterCreateNewSuper)
        {
            CurrentEffectiveDate = DateTime.Today;
            NewEffectiveDate = isAfterCreateNewSuper ? DateTime.Today : DateTime.Today.AddDays(1);
        }

        public int CustId { get; set; }

        public string UserName { get; set; }

        public bool HasMember { get; set; }

        public int CurrentSportbookPossess { get; set; }

        public int CurrentNonSportPossess { get; set; }

        public DateTime CurrentEffectiveDate { get; set; }

        public int NewSportbookPossess { get; set; }

        public int NewNonSportPossess { get; set; }

        public DateTime NewEffectiveDate { get; set; }

        public bool IsAfterCreateNewSuper { get; set; }
    }
}
