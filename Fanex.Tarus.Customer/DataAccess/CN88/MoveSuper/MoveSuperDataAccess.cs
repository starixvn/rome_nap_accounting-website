namespace Fanex.Tarus.Customer.DataAccess.CN88.MoveSuper
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.CN88.MoveSuper;
    using Fanex.Tarus.Customer.Entities.CN88.MoveSuper;
    using System.Collections.Generic;

    public class MoveSuperDataAccess : BaseDataAccess, IMoveSuperDataAccess
    {
        public IEnumerable<string> GetHeadlineUserNameSuggestion(GetHeadlineUserNameSuggestionCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    yield return reader["Username"].ToString();
                }
            }
        }

        public SearchSuperModel SearchSuper(SearchSuperCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                var hasData = reader.Read();
                return hasData ? new SearchSuperModel()
                {
                    SuperId = Utils.ToInt32(reader["SuperId"]),
                    CurrentHeadlineId = Utils.ToInt32(reader["Current_HeadlineId"]),
                    CurrentHeadlineUsername = Utils.ToString(reader["Current_HeadlineUsername"]),
                    CurrentExtraPTSetting = Utils.ToString(reader["Current_ExtraPTSetting"]),
                    NextHeadlineId = Utils.ToInt32(reader["Next_HeadlineId"]),
                    NextHeadlineUsername = Utils.ToString(reader["Next_HeadlineUsername"]),
                    NextExtraPTSetting = Utils.ToString(reader["Next_ExtraPTSetting"]),
                    NoOfMember = Utils.ToInt32(reader["NoOfMember"])
                } : null;
            }
        }

        public Result MoveSuper(MoveSuperCriteria criteria) => this.ExecuteNonQuery(criteria);
    }
}
