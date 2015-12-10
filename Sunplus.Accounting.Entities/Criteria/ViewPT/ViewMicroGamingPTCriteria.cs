namespace Sunplus.Accounting.Entities.Criteria.ViewPT
{
    using System.Collections;

    public class ViewMicroGamingPTCriteria
    {
        public string Username { get; set; }

        public int SportId { get; set; }

        public int BetType { get; set; }

        public bool IsLive { get; set; }

        public bool HasCommission { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsMicroGaming { get; set; }

        public bool IsAGCasino { get; set; }

        public Hashtable GetSpParam()
        {
            var storeProcedureParam = new Hashtable();
            storeProcedureParam.Add("username", Username);
            storeProcedureParam.Add("sporttype", SportId);
            storeProcedureParam.Add("bettype", BetType);
            storeProcedureParam.Add("islive", IsLive);
            storeProcedureParam.Add("hascomm", HasCommission);
            storeProcedureParam.Add("errcode", 0);
            storeProcedureParam.Add("errmsg", string.Empty);
            return storeProcedureParam;
        }

        public string GetSettingKey()
        {
            return "Customer_ViewPT_DepositPT";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
