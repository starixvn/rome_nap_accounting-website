namespace Fanex.Tarus.Customer.Business.MicroGaming
{
    using System;
    using Fanex.Tarus.Customer.Criteria.MicroGaming;
    using Fanex.Tarus.Customer.DataAccess.MicroGaming;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.MicroGaming;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.Core.Common;

    public class MicroGamingService : IMicroGamingService
    {
        private readonly IMicroGamingRepository _microGamingRepository;

        public MicroGamingService(IMicroGamingRepository microGamingRepository = null)
        {
            _microGamingRepository = microGamingRepository ?? new MicroGamingRepository();
        }

        public MicroGamingCustomerInfo CustomerSetting(GetSettingCriteria criteria)
        {
            try
            {
                var customerSetting = _microGamingRepository.CustomerSetting(criteria);
                return customerSetting;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new MicroGamingCustomerInfo
            {
                SelectResult = new SelectResult
                {
                    Code = 1,
                    Message = Customer_Labels.Invalid_Customer
                }
            };
        }

        public Result EditSetting(EditSettingCriteria criteria)
        {
            try
            {
                Result updateResult = new Result();
                if (!Permissions.HasHostPermission(Enumerations.Functions.Customer.ViewInfo))
                {
                    updateResult.ErrorCode = 1;
                    updateResult.ErrorMessage = Customer_Labels.Error_NoPermissionOnFunction;
                    return updateResult;
                }

                updateResult = _microGamingRepository.EditSetting(criteria);
                if (updateResult.ErrorCode == 0)
                {
                    updateResult.ErrorMessage = Customer_Labels.MicroGaming_UpdateSuccess;
                }

                return updateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;                   
        }
    }
}
