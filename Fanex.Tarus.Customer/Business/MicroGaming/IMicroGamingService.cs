namespace Fanex.Tarus.Customer.Business.MicroGaming
{
    using Fanex.Tarus.Customer.Criteria.MicroGaming;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.MicroGaming;

    public interface IMicroGamingService
    {
        MicroGamingCustomerInfo CustomerSetting(GetSettingCriteria criteria);

        Result EditSetting(EditSettingCriteria criteria);
    }
}
