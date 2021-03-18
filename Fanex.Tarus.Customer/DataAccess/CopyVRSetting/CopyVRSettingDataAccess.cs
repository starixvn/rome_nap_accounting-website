namespace Fanex.Tarus.Customer.DataAccess.CopyVRSetting
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.CopyVRSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.CopyVRSetting;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICopyVRSettingDataAccess
    {
        List<CopyResultEntity> Copy(CopyVRSettingCriteria criteria);
    }

    public class CopyVRSettingDataAccess : ICopyVRSettingDataAccess
    {
        public List<CopyResultEntity> Copy(CopyVRSettingCriteria criteria)
        {
            List<CopyResultEntity> copyResults = new List<CopyResultEntity>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    copyResults.Add(new CopyResultEntity
                    {
                        UserName = Utils.ToString(reader["Username"]),
                        ErrorCode = Utils.ToInt32(reader["ErrorCode"]),
                        ErrorMsg = Utils.ToString(reader["ErrorMsg"])
                    });
                }
            }

            return copyResults;
        }
    }
}
