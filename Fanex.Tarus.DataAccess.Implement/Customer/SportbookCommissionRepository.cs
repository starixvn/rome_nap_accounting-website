using Fanex.Common;
using Fanex.Data;
using Fanex.Tarus.DataAccess.Interface.Customer;
using Fanex.Tarus.Entities.Criteria.Customer;
using Fanex.Tarus.Entities.Customer;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.DataAccess.Implement.Customer
{
    public class SportbookCommissionRepository : BaseRepository, ISportbookCommissionRepository
    {
        public Result CheckMembers(SportbookCommissionCheckUsersCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public EditSportbookCommissionViewModel GetSporttypeBettype(SportbookCommissionSporttypeBettypeSelectCriteria criteria)
        {
            var result = new EditSportbookCommissionViewModel();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    result.ListSporttypeBettype.Add(new SportbookCommissionSportTypeBettype()
                    {
                        Bettype = Converter.ToInt(reader["bettype"]),
                        SportType = Converter.ToInt(reader["SportType"]),
                        BettypeName = Converter.ToString2(reader["GroupBettype"])
                    });
                }
            }

            return result;
        }

        public Result UpdateCommission(SportbookCommissionUpdateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
