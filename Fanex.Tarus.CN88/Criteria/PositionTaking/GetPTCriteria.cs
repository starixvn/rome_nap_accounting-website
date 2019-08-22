namespace Fanex.Tarus.CN88.Criteria.PositionTaking
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    public class GetPTCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey()
            => "CS_PTMatrix_Sel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);

        public override Hashtable GetSpParams()
        {
            var storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("username", Username);
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            return storeProcedureParams;
        }
    }
}
