namespace Fanex.Tarus.Customer.Business.LicenseeOddsGroup
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.LicenseeOddsGroup;
    using Fanex.Tarus.Customer.DataAccess.LicenseeOddsGroup;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.ViewModels.LicenseeOddsGroup;
    using System.Collections.Generic;
    using Result = CustomerSetting.Module.Core.Models.Result;

    public interface ILicenseeOddsGroupService
    {
        IEnumerable<BaseModel> GetAgents(int superId);

        IEnumerable<AgentOddsGroupEntity> GetAgentOddsGroups(string custids);

        Result UpdateOddsGroup(ICriteria criteria);
    }

    public class LicenseeOddsGroupService : ILicenseeOddsGroupService
    {
        private readonly ILicenseeOddsGroupDataAccess _oddsGroupDataAccess;

        public LicenseeOddsGroupService() : this(null) { }

        public LicenseeOddsGroupService(ILicenseeOddsGroupDataAccess settingDataAccess)
        {
            _oddsGroupDataAccess = settingDataAccess ?? new LicenseeOddsGroupDataAccess();
        }

        public IEnumerable<BaseModel> GetAgents(int superId) => _oddsGroupDataAccess.GetAgents(new GetAgentListCriteria(superId));

        public IEnumerable<AgentOddsGroupEntity> GetAgentOddsGroups(string custids) => _oddsGroupDataAccess.GetAgentOddsGroups(new GetAgentOddsGroupCriteria(custids));

        public Result UpdateOddsGroup(ICriteria criteria) => _oddsGroupDataAccess.UpdateOddsGroup(criteria);
    }
}
