namespace Fanex.Tarus.Customer.Business.ManageSuperSettings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Fanex.Customer.Client;
    using Fanex.Customer.Client.Models;
    using Fanex.Customer.Client.Params;
    using Fanex.Customer.Client.Services;
    using Fanex.Tarus.CopySuperWinlossSetting;
    using Fanex.Tarus.CopySuperWinlossSetting.Model;
    using Fanex.Tarus.Customer.Business.CustomerLogin;
    using Fanex.Tarus.Customer.Constants;
    using Fanex.Tarus.Customer.Criteria.CustomerLogin;
    using Fanex.Tarus.Customer.Criteria.ManageSuperSettings;
    using Fanex.Tarus.Customer.DataAccess.ManageSuperSettings;
    using Fanex.Tarus.Customer.Entities.ManageSuperSettings;
    using Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Customer.Requests.ManageSuperSettings;
    using Fanex.Tarus.Customer.Resources;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
    using Sunplus.Accounting.DataAccess.IBCDLL.Customer;

    public interface IManageSuperSettingsService
    {
        CheckSuperListResult CheckSuperList(CheckSuperListRequest request);

        IEnumerable<Entities.Result> EditSuperInfo(EditSuperInfoRequest request, int userId, string ip);

        WinLossLimitModel GetWinLossLimit(GetWinLossLimitRequest request);

        Task<CopyWinLostLimitResult> CopyWinLossLimit(CopyWinLossLimitRequest request, int userId, string ip);
    }

    public class ManageSuperSettingsService : IManageSuperSettingsService
    {
        private const string Language = "en-US";
        private readonly IManageSuperSettingsDataAccess dataAccess;
        private readonly CopySuperWinlossSettingService copySuperWinlossSettingService;
        private readonly ICustomerService customerServiceClient;
        private readonly ICustomerSecurityCodeService securityCodeService;

        public ManageSuperSettingsService()
        {
            dataAccess = new ManageSuperSettingsDataAccess();
            copySuperWinlossSettingService = new CopySuperWinlossSettingService();
            customerServiceClient = new CustomerService();
            securityCodeService = new CustomerSecurityCodeService();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public CheckSuperListResult CheckSuperList(CheckSuperListRequest request)
        {
            var superList = (request.SuperList ?? string.Empty)
                .Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            var duplications = new List<string>();
            foreach (var cust in superList)
            {
                if (!duplications.Any(d => string.Equals(d, cust, System.StringComparison.OrdinalIgnoreCase))
                    && superList.Count(s => string.Equals(s, cust, System.StringComparison.OrdinalIgnoreCase)) > 1)
                {
                    duplications.Add(cust);
                }
            }

            if (duplications.Any())
            {
                return new CheckSuperListResult
                {
                    Result = new Entities.Result(1, "Duplicate super account(s): " + string.Join(", ", duplications))
                };
            }

            var criteria = new CheckSuperListCriteria
            {
                SuperList = string.Join(",", superList),
                Username = request.SourceSuper,
                IsCopy = request.IsCopy,
                Mode = request.Mode,
                DeniedList = AccountCredit.Acc_DeniedSiteCurrency_Sel(request.UserId)
            };

            var result = dataAccess.CheckSuperList(criteria);

            return result;
        }

        public WinLossLimitModel GetWinLossLimit(GetWinLossLimitRequest request)
        {
            var productList = request.ProductList != null ? string.Join(",", request.ProductList.Split(',').Distinct()) : string.Empty;
            var criteria = new GetWinLossLimitCriteria
            {
                SuperList = request.SuperList,
                Username = request.SourceSuper,
                ProductList = productList
            };

            return dataAccess.GetWinLossLimit(criteria);
        }

        public async Task<CopyWinLostLimitResult> CopyWinLossLimit(CopyWinLossLimitRequest request, int userId, string ip)
        {
            var copyWinLostLimitResult = new CopyWinLostLimitResult();
            var superIds = request.SuperIds.Split(',').Select(superId => int.Parse(superId)).ToList();
            var errorResults = new List<Entities.Result>();
            var processedCount = 0;

            if (request.SportTypes.Any())
            {
                processedCount += superIds.Count;
                var products = request.SportTypes.Where(sp => sp.GroupId != 3 && sp.GroupId != 2) // Exclude GroupId = 3 for LC & RNG Casino, will update using new method
                    .GroupBy(sport => sport.ProductId)
                    .Select(grp => grp.ToList())
                    .Select(sports => new Product { ProductId = sports[0].ProductId, Sporttypes = sports.Select(sport => sport.SportTypeId).ToList() })
                    .ToList();

                if (products.Any())
                {
                    var copySuperWinlossSettingRequest = new CopySuperWinlossSettingRequest
                    {
                        IP = ip,
                        SourceCustId = request.CustId,
                        TargetCustIds = request.SuperIds,
                        UserId = userId,
                        Products = products,
                        SourceUsername = request.SourceUsername
                    };

                    var otherResults = copySuperWinlossSettingService.Copy(copySuperWinlossSettingRequest);
                    var mappingResults = MappingCopyResults(otherResults.ToList(), request.SuperList.ToList());

                    errorResults.AddRange(mappingResults.Where(result => result.ErrorCode != 0).Select(result => new Entities.Result(result.ErrorCode, result.ErrorMessage)));
                }

                if (request.SportTypes.Any(sp => sp.GroupId == 3 || sp.GroupId == 2))
                {
                    var response = await customerServiceClient.CopySupersWinLossLimit(new CopySupersWinLossLimitParam(
                        request.CustId,
                        request.SourceUsername,
                        request.SuperIds,
                        string.Join(",", request.SuperList.Select(s => s.UserName)),
                        userId,
                        (byte)CustomerClientManager.ClientConfigs.SystemId,
                        ip,
                        ObjectDbExtensions.GetSiteDBConfigByCustId(request.CustId)?.CustDBGroupID ?? 0,
                        request.SportTypes.Where(sp => sp.GroupId == 3 || sp.GroupId == 2).Select(sp => sp.SportTypeId)));
                    var mappingResultsNewAPI = MappingCopyResults(response, request.SuperList.ToList());
                    errorResults.AddRange(mappingResultsNewAPI.Where(result => result.ErrorCode != 0).Select(result => new Entities.Result(result.ErrorCode, result.ErrorMessage)));
                }
            }

            if (errorResults.Count > 0)
            {
                return new CopyWinLostLimitResult
                {
                    ErrorCode = errorResults.Count == processedCount ? -1 : 0,
                    CopyErrors = errorResults
                };
            }

            return copyWinLostLimitResult;
        }

        public IEnumerable<Entities.Result> EditSuperInfo(EditSuperInfoRequest request, int userId, string ip)
        {
            var result = new List<Entities.Result>();
            if (request.EditPasswordRequest != null
                && !ValidatePassword(request.EditPasswordRequest, result))
            {
                return result;
            }

            if (request.EditSecurityCodeRequest != null
                && !ValidateSecurityCode(request.EditSecurityCodeRequest, result))
            {
                return result;
            }

            foreach (var super in request.Supers)
            {
                if (request.EditGivenCreditRequest != null)
                {
                    result.Add(EditGivenCredit(super, request.EditGivenCreditRequest, userId));
                }

                if (request.EditPasswordRequest != null)
                {
                    result.Add(EditPassword(super, request.EditPasswordRequest, userId));
                }

                if (request.EditSecurityCodeRequest != null)
                {
                    result.Add(EditSecurityCode(super, request.EditSecurityCodeRequest, userId, ip));
                }
            }

            return result;
        }

        public Entities.Result EditGivenCredit(SuperStatusEntity super, EditGivenCreditRequest request, int userId)
        {
            try
            {
                Credit.UpdateSMACredit(userId, super.CustId, request.Amount);
                return new Entities.Result(0, super.UserName + ": " + Customer_Labels.SuperGivenCreditUpdatedSuccessfully);
            }
            catch (AccException ex)
            {
                return new Entities.Result(1, super.UserName + ": " + ex.Message.ToString());
            }
        }

        public Entities.Result EditPassword(SuperStatusEntity super, EditPasswordRequest request, int userId)
        {
            var password = Fanex.Tarus.Security.Services.Encryptor.Decrypt(request.Password, Fanex.Tarus.Security.Services.Encryptor.Code);
            var updateResult = UpdateCustomer.Acc_CustomerPwdUpd(
                super.CustId,
                IBCUtils.Encryption.CfsEnCode(password),
                userId,
                "E",
                request.ForceChangePassword);
            if (updateResult.ErrorCode == 0)
            {
                if (AccCommon.IsEnabledRecheckin)
                {
                    UpdateCustomer.KickoutCustomer(super.CustId);
                }
            }

            var message = updateResult.ErrorCode == 0 ? Customer_Labels.PasswordChangedSuccessfully : updateResult.ErrorMsg;
            return new Entities.Result(0, super.UserName + ": " + message);
        }

        public Entities.Result EditSecurityCode(SuperStatusEntity super, EditSecurityCodeRequest request, int userId, string ip)
        {
            var criteria = new ResetSecurityCodeCriteria
            {
                Securitycode = Fanex.Tarus.Security.Services.Encryptor.Decrypt(request.SecurityCode, Fanex.Tarus.Security.Services.Encryptor.Code),
                Custid = super.CustId,
                Userid = userId,
                IsForcedChangeSecurityCode = request.ForceChangeSecurityCode,
                RoleId = RoleIdConstants.SUPER,
                IP = ip
            };

            var updateResult = securityCodeService.ResetSecurityCode(criteria);
            var message = updateResult.ErrorCode == 0 ? Customer_Labels.SecurityCodeChangedSuccessfully : updateResult.ErrorMessage;
            return new Entities.Result(0, super.UserName + ": " + message);
        }

        private IEnumerable<Entities.Result> MappingCopyResults(List<CustomerSetting.Module.Core.Models.Result> copyResults, List<SuperStatusEntity> superList)
        {
            var results = new List<Entities.Result>();
            for (int i = 0; i < copyResults.Count; i++)
            {
                results.Add(new Entities.Result(copyResults[i].ErrorCode, string.Format("{0}: {1}", superList[i].UserName, copyResults[i].ErrorMessage)));
            }

            return results;
        }

        private IEnumerable<Entities.Result> MappingCopyResults(List<UpdateCustomerResult> copyResults, List<SuperStatusEntity> superList)
        {
            var results = new List<Entities.Result>();
            for (int i = 0; i < copyResults.Count; i++)
            {
                if (copyResults[i].SubResults == null)
                {
                    results.Add(new Entities.Result());
                }
                else
                {
                    results.Add(new Entities.Result(copyResults[i].SubResults.FirstOrDefault().Code, string.Format("{0}: {1}", superList[i].UserName, copyResults[i].SubResults.FirstOrDefault().Message)));
                }
            }

            return results;
        }

        private bool ValidatePassword(EditPasswordRequest request, List<Entities.Result> updateResult)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                updateResult.Add(new Entities.Result(1, "Sorry, password cannot be empty."));
                return false;
            }

            if (request.Password.Length < 4)
            {
                updateResult.Add(new Entities.Result(1, "Password must have at least 4 characters."));
                return false;
            }

            if (request.Password != request.ConfirmedPassword)
            {
                updateResult.Add(new Entities.Result(1, "Password was not correctly confirmed."));
                return false;
            }

            return true;
        }

        private bool ValidateSecurityCode(EditSecurityCodeRequest request, List<Entities.Result> updateResult)
        {
            if (string.IsNullOrWhiteSpace(request.SecurityCode))
            {
                updateResult.Add(new Entities.Result(1, "Sorry, security code can't be empty."));
                return false;
            }

            if (request.SecurityCode != request.ConfirmedSecurityCode)
            {
                updateResult.Add(new Entities.Result(1, "Sorry, confirm security code doesn't match with security code."));
                return false;
            }

            if (!IsValidSecurityCode(request.SecurityCode))
            {
                updateResult.Add(new Entities.Result(1, "Sorry, security code is incorrect format."));
                return false;
            }

            return true;
        }

        private bool IsValidSecurityCode(string encryptedSecCode)
        {
            var secCode = Fanex.Tarus.Security.Services.Encryptor.Decrypt(encryptedSecCode, Fanex.Tarus.Security.Services.Encryptor.Code);
            var secCodeLength = secCode.Length;
            if (secCodeLength != 6)
            {
                return false;
            }

            var regex = new Regex("/^([0-9])(?!\\1+$)[0-9]+$/");
            var result = regex.Match(secCode).Length == 0 ? true : false;
            if (result)
            {
                var isForwardRun = true;
                var isBackwardRun = true;
                var forward = int.Parse(secCode[0].ToString());
                var backward = int.Parse(secCode[0].ToString());
                for (var i = 1; i < secCodeLength; i++)
                {
                    var current = int.Parse(secCode[i].ToString());
                    if (++forward != current)
                    {
                        isForwardRun = false;
                    }

                    if (--backward != current)
                    {
                        isBackwardRun = false;
                    }
                }

                result = !(isForwardRun || isBackwardRun);
            }

            return result;
        }
    }
}