namespace Fanex.Tarus.Customer.Business.ViewPeriodsCN88
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.ViewPeriodsCN88;
    using Fanex.Tarus.Customer.DataAccess.ViewPeriodsCN88;
    using Fanex.Tarus.Customer.Entities.ViewPeriodsCN88;
    using System.Collections.Generic;

    public interface IViewPeriodsCN88Service
    {
        IEnumerable<HeadlinePeriods> Search(SearchHeadlineCriteria criteria);

        Result UpdatePeriod(UpdateHeadlinePeriodCriteria criteria);
    }

    public class ViewPeriodsCN88Service : IViewPeriodsCN88Service
    {
        private readonly IViewPeriodsCN88DataAccess _viewPeriodsCN88DataAccess;

        public ViewPeriodsCN88Service() : this(null) { }

        public ViewPeriodsCN88Service(IViewPeriodsCN88DataAccess viewPeriodsCN88DataAccess)
        {
            _viewPeriodsCN88DataAccess = viewPeriodsCN88DataAccess ?? new ViewPeriodsCN88DataAccess();
        }

        public IEnumerable<HeadlinePeriods> Search(SearchHeadlineCriteria criteria) => _viewPeriodsCN88DataAccess.Search(criteria);

        public Result UpdatePeriod(UpdateHeadlinePeriodCriteria criteria) => _viewPeriodsCN88DataAccess.UpdatePeriod(criteria);
    }
}
