namespace Fanex.Tarus.Customer.DataAccess.CN88.MoveSuper
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.CN88.MoveSuper;
    using Fanex.Tarus.Customer.Entities.CN88.MoveSuper;
    using System.Collections.Generic;

    public interface IMoveSuperDataAccess
    {
        IEnumerable<string> GetHeadlineUserNameSuggestion(GetHeadlineUserNameSuggestionCriteria criteria);

        SearchSuperModel SearchSuper(SearchSuperCriteria criteria);

        Result MoveSuper(MoveSuperCriteria criteria);
    }
}
