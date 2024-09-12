namespace Fanex.Tarus.Customer.Business.LicenseeMember
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.LicenseeMember;
    using Fanex.Tarus.Customer.DataAccess.LicenseeMember;
    using Fanex.Tarus.Customer.Requests.LicenseeMember;

    public class LicenseeMemberService : ILicenseeMemberService
    {
        private readonly ILicenseeMemberDataAccess dataAccess;

        public LicenseeMemberService()
        {
            dataAccess = new LicenseeMemberDataAccess();
        }

        public int CheckExistAgent(string agentName)
        {
            var criteria = new CheckExistAgentCriteria(agentName);

            return dataAccess.CheckExistAgent(criteria);
        }

        public Result InsertMember(InsertMemberRequest request, int userId, string ip)
        {
            var registerResult = dataAccess.RegisterMember(new RegisterMemberCriteria(request.Username, request.Recommend, request.Site, request.CustDBGroupID));

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