namespace Fanex.Tarus.Customer.Criteria.MicroGaming
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities.MicroGaming;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Constants;
    using Fanex.Tarus.Infrastructure.Helper;

    public class EditSettingCriteria : CriteriaBase
    {
        public int CustomerId { get; set; }

        public int RoleId { get; set; }

        public List<GameTypeModel> GameTypeList { get; set; }

        public int SportId
        {
            get
            {
                return Sports.MicroGamingLiveCasino;
            }
        }

        public int UserId
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public string IP
        {
            get
            {
                return Utils.GetClientIP();
            }
        }

        public string Language
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        public override string GetSettingKey()
        {
            return "Customer_ThirdParty_UpdateCustomerSetting";
        }

        public override bool IsValid()
        {
            return true;
        }

        public override Hashtable GetSpParams()
        {
            Hashtable storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("custid", CustomerId);
            storeProcedureParams.Add("userid", UserId);
            storeProcedureParams.Add("sporttype", SportId);

            var gameTypeLength = GameTypeList != null ? GameTypeList.Count : 0;
            for (var i = 0; i < gameTypeLength; i++)
            {
                var gameType = GameTypeList[i];
                storeProcedureParams.Add("v" + SportId + "_" + gameType.Id + "pt", gameType.PositionTaking);
            }

            storeProcedureParams.Add("ip", IP);
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            storeProcedureParams.Add("langkey", Language);
            return storeProcedureParams;
        }
    }
}
