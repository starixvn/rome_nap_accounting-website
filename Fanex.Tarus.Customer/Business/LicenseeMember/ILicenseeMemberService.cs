namespace Fanex.Tarus.Customer.Business.LicenseeMember
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Requests.LicenseeMember;
    using Fanex.Tarus.Infrastructure.Extensions;

    public interface ILicenseeMemberService
    {
        SiteDB GetSiteDBConfig(string username);

        int CheckExistAgent(string agentName);

        Result InsertMember(InsertMemberRequest request, int userId, string ip);
    }
}