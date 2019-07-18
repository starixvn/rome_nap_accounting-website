namespace Fanex.Tarus.Customer.Criteria.CN88.Headline
{
    using Fanex.NAP.Framework.Data;

    public class AddNewHeadlineCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        [SpParam(SqlParamName = "userpwd")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public int Currency { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        [SpParam(Ignored = true)]
        public decimal SportbooksPossess { get; set; }

        [SpParam(Ignored = true)]
        public decimal NonSportsPossess { get; set; }

        public string ExtraPTSetting
        {
            get
            {
                return $"<ROOT>" +
                            $"<ExtraPT ProductId=\"1\" ExtraPT=\"{SportbooksPossess}\" />" +
                            $"<ExtraPT ProductId=\"3\" ExtraPT=\"{NonSportsPossess}\" />" +
                        $"</ROOT>";
            }
        }

        public override string GetSettingKey()
        {
            return "CS_CustomerIns_Headline";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(UserName)
                && !string.IsNullOrWhiteSpace(Password)
                && Currency > 0;
        }
    }
}
