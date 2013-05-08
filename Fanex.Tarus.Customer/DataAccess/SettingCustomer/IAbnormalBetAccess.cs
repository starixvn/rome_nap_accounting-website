// -----------------------------------------------------------------------
// <copyright file="IAbormalBetAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer
{
    using Fanex.Tarus.Customer.Entities.SettingCustomer;
    using Fanex.Tarus.Customer.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAbnormalBetAccess
    {
        Result UpdateLevelAbnormal(int custId, int updateDownLine, int abnormalLevel, int userId, string ip);
    }
}
