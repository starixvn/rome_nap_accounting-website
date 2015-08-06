namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer.MicroGaming
{
    using DNAide.Data;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;

    public interface IMicroGamingPTRepository
    {
        DbResult GetPT(ViewMicroGamingPTCriteria criteria);
    }
}
