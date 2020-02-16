

namespace Fanex.Tarus.Customer.DataAccess.LockUnsuspend
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities.LockUnsuspend;
    using System.Collections.Generic;

    public interface ILockUnsuspendService
    {
        List<LockUnsuspendResultEntity> Search(ICriteria criteria);

        Result Update(ICriteria criteria);
    }

    public class LockUnsuspendService : BaseDataAccess, ILockUnsuspendService
    {
        public List<LockUnsuspendResultEntity> Search(ICriteria criteria)
        {
            List<LockUnsuspendResultEntity> result = new List<LockUnsuspendResultEntity>();
            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var reader = objDb.ExecuteReader(criteria.GetSpParams());
                    while (reader.Read())
                    {
                        result.Add(new LockUnsuspendResultEntity()
                        {
                            CustId = Utils.ToInt32(reader["CustId"]),
                            RoleId = Utils.ToInt32(reader["RoleId"]),
                            UserName = Utils.ToString(reader["UserName"]),
                            Status = Utils.ToString(reader["CustStatus"])
                        });
                    }
                }
            }

            return result;
        }

        public Result Update(ICriteria criteria) => ExecuteNonQuery(criteria);
    }
}
;