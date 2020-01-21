namespace Fanex.Tarus.Customer.DataAccess.ViewPeriodsCN88
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.ViewPeriodsCN88;
    using Fanex.Tarus.Customer.Entities.ViewPeriodsCN88;
    using System.Collections.Generic;

    public interface IViewPeriodsCN88DataAccess
    {
        IEnumerable<HeadlinePeriods> Search(SearchHeadlineCriteria criteria);

        Result UpdatePeriod(UpdateHeadlinePeriodCriteria criteria);
    }

    public class ViewPeriodsCN88DataAccess : BaseDataAccess, IViewPeriodsCN88DataAccess
    {
        public IEnumerable<HeadlinePeriods> Search(SearchHeadlineCriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var reader = objDb.ExecuteReader(criteria.GetSpParams());
                    while (reader.Read())
                    {
                        yield return new HeadlinePeriods
                        {
                            HeadlineId = Utils.ToInt32(reader["custid"]),
                            HeadlineUsername = Utils.ToString(reader["username"]),
                            FirstName = Utils.ToString(reader["FirstName"]),
                            LastName = Utils.ToString(reader["LastName"]),
                            Currency = Utils.ToString(reader["Currency"]),
                            IsViewAllPeriods = Utils.ToBoolean(reader["IsViewAllPeriod"])
                        };
                    }
                }
            }
        }

        public Result UpdatePeriod(UpdateHeadlinePeriodCriteria criteria) => ExecuteNonQuery(criteria);

        private List<HeadlinePeriods> FakeData()
        {
            var result = new List<HeadlinePeriods>();
            for (int i = 0; i < 100; i++)
            {
                result.Add(new HeadlinePeriods() { HeadlineId = i, Currency = "UUS", FirstName = "FirstName" + i, LastName = "LastName" + i, HeadlineUsername = "Headline" + i, IsViewAllPeriods = true });
            }
            return result;
        }
    }
}
