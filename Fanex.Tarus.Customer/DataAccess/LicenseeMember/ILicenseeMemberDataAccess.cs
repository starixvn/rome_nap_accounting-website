namespace Fanex.Tarus.Customer.DataAccess.LicenseeMember
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.LicenseeMember;
    using Fanex.Tarus.Infrastructure.Extensions;

    public interface ILicenseeMemberDataAccess
    {
        SiteDB GetSiteDBConfig(string username);

        int CheckExistAgent(CheckExistAgentCriteria criteria);

        (Result, int) RegisterMember(RegisterMemberCriteria criteria);

        Result InsertMember(InsertMemberCriteria criteria);
    }
}