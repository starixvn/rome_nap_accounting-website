namespace Fanex.Tarus.Customer.Business.CopySuperSetting
{
    using Fanex.Customer.Client;
    using Fanex.Customer.Client.Models;
    using Fanex.Customer.Client.Params;
    using Fanex.Customer.Client.Services;
    using Fanex.Tarus.CopySuperWinlossSetting;
    using Fanex.Tarus.CopySuperWinlossSetting.Model;
    using Fanex.Tarus.Customer.Criteria.CopySuperSetting;
    using Fanex.Tarus.Customer.DataAccess.CopySuperSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.CopySuperSetting;
    using Fanex.Tarus.Customer.Requests.CopySuperSetting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICopySuperSettingService
    {
        CheckSuperListResult CheckSuperList(CheckSuperListRequest request);

        Task<CopyWinLostLimitResult> CopyWinLossLimit(UpdateWinLossLimitRequest request, int userId, string ip);
    }

    public class CopySuperSettingService : ICopySuperSettingService
    {
        private readonly ICopySuperSettingDataAccess dataAccess;
        private readonly CopySuperWinlossSettingService copySuperWinlossSettingService;
        private readonly ICustomerService customerServiceClient;
        private const int LiveCSProductGroupId = 3;

        public CopySuperSettingService()
        {
            dataAccess = new CopySuperSettingDataAccess();
            copySuperWinlossSettingService = new CopySuperWinlossSettingService();
            customerServiceClient = new CustomerService();
        }

        public CheckSuperListResult CheckSuperList(CheckSuperListRequest request)
        {
            var productList = request.ProductList != null ? string.Join(",", request.ProductList.Split(',').Distinct()) : string.Empty;
            var criteria = new CheckSuperListCriteria
            {
                SuperList = request.SuperList,
                Username = request.SourceSuper,
                ProductList = productList
            };

            return dataAccess.CheckSuperList(criteria);
        }

        public async Task<CopyWinLostLimitResult> CopyWinLossLimit(UpdateWinLossLimitRequest request, int userId, string ip)
        {
            var copyWinLostLimitResult = new CopyWinLostLimitResult();
            var superIds = request.SuperIds.Split(',').Select(superId => int.Parse(superId)).ToList();
            var errorResults = new List<Entities.Result>();
            var processedCount = 0;

            if (request.SportTypes.Any())
            {
                processedCount += superIds.Count;
                var products = request.SportTypes.Where(sp => sp.GroupId != 3) // Exclude GroupId = 3 for LC & RNG Casino, will update using new method
                    .GroupBy(sport => sport.ProductId)
                    .Select(grp => grp.ToList())
                    .Select(sports => new Product { ProductId = sports[0].ProductId, Sporttypes = sports.Select(sport => sport.SportTypeId).ToList() })
                    .ToList();

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

                var response = await customerServiceClient.CopySupersWinLossLimit(new CopySupersWinLossLimitParam(
                    request.CustId,
                    request.SourceUsername,
                    request.SuperIds,
                    string.Join(",", request.SuperList.Select(s => s.Name)),
                    userId,
                    (byte)CustomerClientManager.ClientConfigs.SystemId,
                    ip,
                    request.SportTypes.Where(sp => sp.GroupId == 3).Select(sp => sp.SportTypeId)));
                var mappingResultsNewAPI = MappingCopyResults(response, request.SuperList.ToList());
                errorResults.AddRange(mappingResultsNewAPI.Where(result => result.ErrorCode != 0).Select(result => new Entities.Result(result.ErrorCode, result.ErrorMessage)));
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

        private IEnumerable<Entities.Result> MappingCopyResults(List<CustomerSetting.Module.Core.Models.Result> copyResults, List<BaseModel> superList)
        {
            var results = new List<Entities.Result>();
            for (int i = 0; i < copyResults.Count; i++)
            {
                results.Add(new Entities.Result(copyResults[i].ErrorCode, string.Format("{0}: {1}", superList[i].Name, copyResults[i].ErrorMessage)));
            }

            return results;
        }

        private IEnumerable<Entities.Result> MappingCopyResults(List<UpdateCustomerResult> copyResults, List<BaseModel> superList)
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
                    results.Add(new Entities.Result(copyResults[i].SubResults.FirstOrDefault().Code, string.Format("{0}: {1}", superList[i].Name, copyResults[i].SubResults.FirstOrDefault().Message)));
                }
            }

            return results;
        }
    }
}