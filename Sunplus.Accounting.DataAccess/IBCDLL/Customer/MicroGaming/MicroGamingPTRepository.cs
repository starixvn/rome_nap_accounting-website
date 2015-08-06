namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer.MicroGaming
{
    using DNAide.Data;
    using Fanex.Data;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;

    public class MicroGamingPTRepository : IMicroGamingPTRepository
    {
        public DbResult GetPT(ViewMicroGamingPTCriteria criteria)
        {
            DbResult rawData = null;
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey(), 600))
            {
                objectDb.AssignParamValues(criteria.GetSpParam());
                var dataSet = objectDb.ExecuteDataSet();
                var output = objectDb.GetOuputParams();
                rawData = new DbResult(output, dataSet);
            }

            return rawData;
        }
    }
}
