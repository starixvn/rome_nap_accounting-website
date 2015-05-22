namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    using DNAide.Data;
    using Fanex.Data;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;

    /// <summary>
    /// View Keno PT Repository.
    /// </summary>
    public class ViewGoldDeluxePTRepository
    {
        /// <summary>
        /// Gets The Position Taking.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Position Taking.</returns>
        public DbResult GetPT(ViewGoldDeluxePTCriteria criteria)
        {
            DbResult result = null;
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey(), 600))
            {
                db.AssignParamValues(criteria.GetSpParam());
                var ds = db.ExecuteDataSet();
                var output = db.GetOuputParams();
                result = new DbResult(output, ds);
            }

            return result;
        }
    }
}
