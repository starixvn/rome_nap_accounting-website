namespace Sunplus.Accounting.Entities.Criteria.ViewPT
{
    using System.Collections;

    public class ViewGoldDeluxePTCriteria
    {
        public string Username { get; set; }

        public int SportId { get; set; }

        public int BetType { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public Hashtable GetSpParam()
        {
            var storeProcedureParam = new Hashtable();
            storeProcedureParam.Add("username", Username);
            storeProcedureParam.Add("bettype", BetType);

            return storeProcedureParam;
        }

        public string GetSettingKey()
        {
            return "ViewPT_GoldDeluxe";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
