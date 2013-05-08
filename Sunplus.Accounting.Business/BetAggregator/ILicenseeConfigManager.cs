using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.BetAggregator;
using DNAide.Data;

namespace Sunplus.Accounting.Business.BetAggregator
{
    public interface ILicenseeConfigManager
    {
        List<LicenseeConfig> SelAllLicensee();
        List<LicenseeConfig> SelBaLicenseeList();
        List<LicenseeLog> SelBaLicenseeLogList(int licenseeId);
        Result InsertConfig(int licenseeId, double hdpComm, double oneXtwoComm, double otherComm, double ngGame, bool enableBa);

        Result UpdateConfig(int licenseeId, double groupa, double groupb, double groupc, double groupd, double oneXtwoComm, double otherComm,
                            bool enableBa);

        Result DeleteConfig(int licenseeId);

        int CheckDownlineBA(int licenseeId);

        List<Sunplus.Accounting.Entities.BetAggregator.AmtLimitLicensee> GetAmtLimitLicensees();

        Result BALimitWarning(int licenseeID, string licenseeName, double limitAmt);

        BACurrenyGroupDTO SelBACurrencyGroupBy(int licenseeId);

        Result BAUpdateGroupSetting(int licenseeId, string setting);
    }
}