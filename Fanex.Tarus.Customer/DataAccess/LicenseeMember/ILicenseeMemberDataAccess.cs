namespace Fanex.Tarus.Customer.DataAccess.LicenseeMember
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.LicenseeMember;

    public interface ILicenseeMemberDataAccess
    {
        int CheckExistAgent(CheckExistAgentCriteria criteria);

        (Result, int) RegisterMember(RegisterMemberCriteria criteria);

        Result InsertMember(InsertMemberCriteria criteria);
    }
}