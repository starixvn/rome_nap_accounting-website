using System.Globalization;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.CreditCustomer.Entities;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public sealed class InsertToastMessageCriteria : CriteriaBase
    {
        private const int WarningTypeId = 3;
        private const int SiteFunctionId_ADBTChange = 2;

        public InsertToastMessageCriteria(int targetId, int creator)
        {
            TypeId = WarningTypeId;
            SiteFunctionId = SiteFunctionId_ADBTChange;
            TargetId = targetId;
            Creator = creator;

            ContentEN = GetContent("en");
            ContentCH = GetContent("ch");
            ContentCS = GetContent("cs");
            ContentTH = GetContent("th");
        }

        public int TypeId { get; set; }

        public int SiteFunctionId { get; set; }

        public int TargetId { get; set; }

        public int Creator { get; set; }

        [SpParam(SqlParamName = "content_en")]
        public string ContentEN { get; set; }

        [SpParam(SqlParamName = "content_ch")]
        public string ContentCH { get; set; }

        [SpParam(SqlParamName = "content_cs")]
        public string ContentCS { get; set; }

        [SpParam(SqlParamName = "content_jp")]
        public string ContentJP { get; set; }

        [SpParam(SqlParamName = "content_it")]
        public string ContentIT => string.Empty;

        [SpParam(SqlParamName = "content_th")]
        public string ContentTH { get; set; }

        [SpParam(SqlParamName = "content_ko")]
        public string ContentKO { get; set; }

        [SpParam(SqlParamName = "content_es")]
        public string ContentES => string.Empty;

        [SpParam(SqlParamName = "content_vn")]
        public string ContentVN { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int ToastId { get; set; }

        public override string GetSettingKey() => "Age_ToastMessage_Ins";

        public override bool IsValid() => true;

        private string GetContent(string langue)
        {
            var culture = CultureInfo.GetCultureInfo(langue);

            var content = new ToastContentModel
            {
                Title = Resources.Customer_Labels.ResourceManager.GetString("ADBTChangeToastTitle_" + langue),
                Body = Resources.Customer_Labels.ResourceManager.GetString("ADBTChangeToastMessage_" + langue)
            };

            return JsonConvert.SerializeObject(content, Formatting.None);
        }
    }
}
