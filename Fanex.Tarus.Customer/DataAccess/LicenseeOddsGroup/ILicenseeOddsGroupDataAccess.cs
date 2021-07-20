namespace Fanex.Tarus.Customer.DataAccess.LicenseeOddsGroup
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.ViewModels.LicenseeOddsGroup;
    using System.Collections.Generic;
    using Result = CustomerSetting.Module.Core.Models.Result;

    public interface ILicenseeOddsGroupDataAccess
    {

        IEnumerable<BaseModel> GetAgents(ICriteria criteria);

        IEnumerable<AgentOddsGroupEntity> GetAgentOddsGroups(ICriteria criteria);

        Result UpdateOddsGroup(ICriteria criteria);
    }
}
