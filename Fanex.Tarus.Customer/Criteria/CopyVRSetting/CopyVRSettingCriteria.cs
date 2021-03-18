namespace Fanex.Tarus.Customer.Criteria.CopyVRSetting
{
    using Fanex.NAP.Framework.Data;

    public class CopyVRSettingCriteria : CriteriaBase
    {
        [SpParam(SqlParamName ="Source_Username")]
        public string SourceMember { get; set; }

        [SpParam(SqlParamName = "Dest_Usernames")]
        public string DestinationMembers { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerCopyVRSetting";
        }


        public override bool IsValid()
        {
            return true;
        }
    }
}
