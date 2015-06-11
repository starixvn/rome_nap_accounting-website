namespace Fanex.Tarus.Customer.DataAccess.MicroGaming
{
    using Fanex.Tarus.Customer.Criteria.MicroGaming;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.MicroGaming;

    public interface IMicroGamingRepository
    {
        MicroGamingCustomerInfo CustomerSetting(GetSettingCriteria criteria);

        Result EditSetting(EditSettingCriteria criteria);
    }
}
}
