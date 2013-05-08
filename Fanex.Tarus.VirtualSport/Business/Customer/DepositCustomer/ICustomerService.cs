// -----------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.VirtualSport.Business.Customer.DepositCustomer
{
    using System.Collections.Generic;
    using Fanex.Tarus.VirtualSport.Entities.Customer.DepositCustomer;    
    using Fanex.Tarus.VirtualSport.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerService
    {
        Result AddNewSuperSettings(List<RequestParam> requestParams);
        CustomerSettings GetSuperSetting(int custId);
        Result UpdateSuperSettings(List<RequestParam> requestParams);

        Result AddNewMasterSettings(List<RequestParam> requestParams);
        CustomerSettings GetMasterSetting(int custId);
        CustomerSettings GetMasterSettingForAddNew(int uplineId);
        Result UpdateMasterSettings(List<RequestParam> requestParams);

        Result AddNewAgentSettings(List<RequestParam> requestParams);
        CustomerSettings GetAgentSetting(int custId);
        CustomerSettings GetAgentSettingForAddNew(int uplineId);
        Result UpdateAgentSettings(List<RequestParam> requestParams);

        Result AddNewMemberSettings(List<RequestParam> requestParams);
        CustomerSettings GetMemberSetting(int custId);
        CustomerSettings GetMemberSettingForAddNew(int uplineId);
        Result UpdateMemberSettings(List<RequestParam> requestParams);
    }
}
