using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.PositionTaking
{
    public interface IPokerDAO
    {
        DbResult GetCustomerStatus(int custId);

        DbResult GetCustomerSetting(int custId, int roleId);

        Result UpdateP2P(Hashtable Params, int type);

        Result UpdateP2PStaus(Hashtable Params);

        DbResult GetDefaultSetting();

        DbResult SelectEnableCustomer(int custId);

        Result UpdateDefaultSettingP2P(Hashtable Params);
    }
}