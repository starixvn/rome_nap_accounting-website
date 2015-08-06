namespace Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;

    /// <summary>
    /// Update Import Rate Criteria.
    /// </summary>
    public class UpdateImportRateCriteria : CriteriaBase
    {
        public string FileName { get; set; }

        public int FileSize { get; set; }

        [SpParam(Ignored = true)]
        public int TotalSuccessRow { get; set; }

        [SpParam(Ignored = true)]
        public List<ImportVirtualRateResult> SuccessRecords { get; set; }

        public int Index { get; set; }

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

        public override string GetSettingKey()
        {
            return "Customer_VirtualRate_UpdateMultiple";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
