namespace Fanex.Tarus.Customer.Business.LicenseeMember
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Requests.LicenseeMember;

    public interface ILicenseeMemberService
    {
        int CheckExistAgent(string agentName);

        Result InsertMember(InsertMemberRequest request, int userId, string ip);
    }
}