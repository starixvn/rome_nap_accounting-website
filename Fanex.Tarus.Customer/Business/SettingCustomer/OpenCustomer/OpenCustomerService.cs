namespace Fanex.Tarus.Customer.Business.SettingCustomer.OpenCustomer
{
    using CustomerSetting.Module.Core.Models;
    using Requests.CustomerSetting.OpenCustomer;
    using ViewModels.CustomerSetting.OpenCustomer;

    public interface OpenCustomerService
    {
        SingleOpenCustomerViewModel GetSingleOpenCustomerInfo(GetSingleOpenCustomerInfoQuery query);

        SingleOpenDownlinesViewModel GetSingleOpenDownlinesInfo(GetSingleOpenDownlinesInfoQuery query);

        Result UpdateSingleCustomerClosedStatus(UpdateClosedStatusQuery query);

        ClosedMatrixViewModel GetClosedMatrixInfo(string username);
    }
}