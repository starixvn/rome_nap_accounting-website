using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Constants;

namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    public class RegisterMemberCriteria : CriteriaBase
    {
        public RegisterMemberCriteria(string username, int uplineID, string site, int siteDBGroupID)
        {
            Username = username;
            UplineID = uplineID;
            Site = site;
            SiteDBGroupID = siteDBGroupID;
        }

        public string Username { get; }

        public int UplineID { get; }

        public byte RoleId { get; } = RoleIdConstants.MEMBER;

        public string Site { get; }

        public int SiteDBGroupID { get; }

        public override string GetSettingKey() => "API_CustomerIns_Register";

        public override bool IsValid()
            => UplineID > 0 && !string.IsNullOrWhiteSpace(Username);
    }
}