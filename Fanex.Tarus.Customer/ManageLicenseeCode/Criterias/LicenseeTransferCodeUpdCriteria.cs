namespace Fanex.Tarus.Customer.ManageLicenseeCode.Criterias
{
    public class LicenseeTransferCodeUpdCriteria : LicenseeTransferCodeInsCriteria
    {
        public string FromDateOld { get; set; }

        public string ToDateOld { get; set; }

        public string LicenseeCodeOld { get; set; }

        public override string GetSettingKey()
            => "Acc_LicenseeTransferCode_Upd";
    }
}
