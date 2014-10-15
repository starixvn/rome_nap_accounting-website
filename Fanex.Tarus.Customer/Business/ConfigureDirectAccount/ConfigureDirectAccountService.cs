// --------------------------------------------------------------------------
// <copyright file="ConfigureDirectAccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.ConfigureDirectAccount
{
    using System;
    using System.Web;
    using Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.DataAccess.AGCasino;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Sunplus.Accounting.Core.Constant;

    /// <summary>
    /// Configure Direct Account Service.
    /// </summary>
    public partial class ConfigureDirectAccountService : IConfigureDirectAccountService
    {
        private readonly IConfigureDirectAccountRepository _configureDirectAccountRepository;

        public ConfigureDirectAccountService()
            : this(null)
        {
        }

        public ConfigureDirectAccountService(IConfigureDirectAccountRepository asianGamingCasinoRepository)
        {
            _configureDirectAccountRepository = asianGamingCasinoRepository ?? new ConfigureDirectAccountRepository();
        }

        /// <summary>
        /// Updates the direct account status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateDirectAccountStatus(UpdateDirectAccountStatusCriteria criteria)
        {
            try
            {
                Result updateResult = new Result();
                updateResult = _configureDirectAccountRepository.UpdateDirectAccountStatus(criteria);
                if (updateResult.ErrorCode == 0)
                {
                    updateResult.ErrorMessage = Customer_Labels.ConfigureDirectAccount_ChangeStatus_UpdateSuccess;
                }

                return updateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the direct account status update effect.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update effect.</returns>
        public UpdateDirectAccountStatusEffectInfo UpdateDirectAccountStatusEffect(UpdateDirectStatusEffectCriteria criteria)
        {
            try
            {
                return _configureDirectAccountRepository.UpdateDirectAccountStatusEffect(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Checks the import direct account.
        /// </summary>
        /// <param name="fileUploaded">The file uploaded.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// Import Direct Account Result.
        /// </returns>
        public ImportDirectAccountResult CheckImportDirectAccount(HttpPostedFileBase fileUploaded, int userId)
        {
            try
            {
                ImportDirectAccountResult importResult = new ImportDirectAccountResult();
                var importDirectAccountCriteria = new CheckImportDirectAccountFileCriteria
                {
                    UserId = userId,
                    UsernameList = ReadImportDirectAccountFile(fileUploaded, importResult)
                };

                if (importResult.ImportResultCode != ErrorCode.NoneError)
                {
                    return importResult;
                }

                if (importDirectAccountCriteria.UsernameList.Count == 0)
                {
                    importResult.ImportResultCode = ErrorCode.InvalidFormatFile;
                    importResult.ImportResultMessage = Customer_Labels.ConfigureDirectAccount_Import_InvalidFormat;
                    return importResult;
                }

                importResult = _configureDirectAccountRepository.CheckImportDirectAccount(importDirectAccountCriteria);
                return importResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the import direct account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateImportDirectAccount(UpdateImportDirectAccountCriteria criteria)
        {
            try
            {
                Result updateResult = _configureDirectAccountRepository.UpdateImportDirectAccount(criteria);
                if (updateResult.ErrorCode == 0)
                {
                    updateResult.ErrorMessage = Customer_Labels.ConfigureDirectAccount_ImportDirectAccount_UpdateSuccess;
                }

                return updateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}