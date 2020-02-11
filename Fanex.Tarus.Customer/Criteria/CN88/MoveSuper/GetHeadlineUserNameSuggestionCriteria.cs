namespace Fanex.Tarus.Customer.Criteria.CN88.MoveSuper
{
    using Fanex.NAP.Framework.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetHeadlineUserNameSuggestionCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CS_HeadlineUsernameSuggestion";
        }
    }
}
