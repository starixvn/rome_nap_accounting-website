namespace Fanex.Tarus.Customer.Business.LicenseeMember
{
    using Fanex.Tarus.Customer.Requests.LicenseeMember;
    using Fanex.Tarus.Customer.Entities;

    public interface ILicenseeMemberService
    {
        int CheckExistAgent(string agentName, int custDBGroupId);

        Result InsertMember(InsertMemberRequest request, int userId, string ip);
    }
}