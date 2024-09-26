namespace Fanex.Tarus.Customer.Business.LicenseeMember
{
    using Fanex.Tarus.Customer.Constants;
    using Fanex.Tarus.Customer.Criteria.LicenseeMember;
    using Fanex.Tarus.Customer.DataAccess.Licensee;
    using Fanex.Tarus.Customer.Requests.LicenseeMember;
    using Fanex.Tarus.Customer.Entities;

    public class LicenseeMemberService : ILicenseeMemberService
    {
        private readonly ILicenseeDataAccess dataAccess;

        public LicenseeMemberService()
        {
            dataAccess = new LicenseeDataAccess();
        }

        public int CheckExistAgent(string agentName, int custDBGroupId)
        {
            var criteria = new CheckExistAccountCriteria(agentName, RoleIdConstants.AGENT, custDBGroupId);

            return dataAccess.CheckExistAccount(criteria).CustId;
        }

        public Result InsertMember(InsertMemberRequest request, int userId, string ip)
        {
            var registerResult = dataAccess.RegisterAccount(new RegisterAccountCriteria(
                request.Username,
                request.Recommend,
                RoleIdConstants.MEMBER,
                request.Site,
                request.CustDBGroupID));

            if (registerResult.Item1.IsSuccess)
            {
                var criteria = new InsertMemberCriteria
                {
                    CustId = registerResult.Item2,
                    Recommend = request.Recommend,
                    Username = request.Username,
                    Creator = userId,
                    IP = ip
                };

                return dataAccess.InsertMember(criteria);
            }

            return registerResult.Item1;
        }
    }
}