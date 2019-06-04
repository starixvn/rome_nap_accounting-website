using Fanex.Tarus.Customer.Entities.CN88;
using static Fanex.Tarus.Infrastructure.Common.Enumerations;

namespace Fanex.Tarus.Customer.ViewModels.CN88
{
    public class CreditRangeViewModel
    {
        public CreditRangeViewModel()
        {
        }

        public CreditRangeViewModel(
            CreditRangeModel creditRange,
            int searchAccountId,
            int roleId)
        {
            GivenCredit = creditRange.GivenCredit;
            UsedCredit = creditRange.UsedCredit;
            MinCredit = creditRange.MinCredit;
            MaxCredit = creditRange.MaxCredit;
            RoleId = roleId;
            SearchAccountId = searchAccountId;
        }

        public int SearchAccountId { get; }

        public int RoleId { get; set; }

        public decimal GivenCredit { get; }

        public decimal UsedCredit { get; }

        public decimal MinCredit { get; }

        public decimal MaxCredit { get; }

        public bool IsMember
        {
            get
            {
                return this.RoleId == (int)Role.Member;
            }
        }

        public bool IsSuper
        {
            get
            {
                return this.RoleId == (int)Role.Super;
            }
        }
    }
}
