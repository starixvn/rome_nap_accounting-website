
namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    using DNAide.Data;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;

    public interface IViewKenoPTRepository
    {
        DbResult GetPT(ViewKenoPTCriteria criteria);
    }
}
