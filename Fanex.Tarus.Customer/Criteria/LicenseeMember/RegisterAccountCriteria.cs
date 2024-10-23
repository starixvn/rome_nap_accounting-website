using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    public class RegisterAccountCriteria : CriteriaBase
    {
        public RegisterAccountCriteria(string username, int uplineID, byte roleId, string site, int siteDBGroupID)
        {
            Username = username;
            UplineID = uplineID;
            Site = site;
            SiteDBGroupID = siteDBGroupID;
            RoleId = roleId;
        }

        public string Username { get; }

        public int UplineID { get; }

        public byte RoleId { get; }

        public string Site { get; }

        public int SiteDBGroupID { get; }

        public override string GetSettingKey() => "API_CustomerIns_Register";

        public override bool IsValid()
            => UplineID > 0 && !string.IsNullOrWhiteSpace(Username);
    }
}