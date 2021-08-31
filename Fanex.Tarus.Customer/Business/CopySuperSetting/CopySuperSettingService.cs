namespace Fanex.Tarus.Customer.Business.CopySuperSetting
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Module.Casino.Services.Impl;
    using Fanex.CustomerSetting.Module.Casino.Services.Interface;
    using Fanex.Tarus.CopySuperWinlossSetting;
    using Fanex.Tarus.CopySuperWinlossSetting.Model;
    using Fanex.Tarus.Customer.Criteria.CopySuperSetting;
    using Fanex.Tarus.Customer.DataAccess.CopySuperSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.CopySuperSetting;
    using Fanex.Tarus.Customer.Requests.CopySuperSetting;

    public interface ICopySuperSettingService
    {
        CheckSuperListResult CheckSuperList(CheckSuperListRequest request);
        CopyWinLostLimitResult CopyWinLossLimit(UpdateWinLossLimitRequest request, int userId, string ip);
    }

    public class CopySuperSettingService : ICopySuperSettingService
    {
        private readonly ICopySuperSettingDataAccess dataAccess;
        private readonly MemberWinLimitPartialService memberWinLimitPartialService;
        private readonly CopySuperWinlossSettingService copySuperWinlossSettingService;
        private const int CasinoProductId = 6;

        public CopySuperSettingService()
        {
            dataAccess = new CopySuperSettingDataAccess();
            memberWinLimitPartialService = new MemberWinLimitPartialServiceImpl();
            copySuperWinlossSettingService = new CopySuperWinlossSettingService();
        }

        public CheckSuperListResult CheckSuperList(CheckSuperListRequest request)
        {
            var criteria = new CheckSuperListCriteria
            {
                SuperList = request.SuperList,
                Username = request.SourceSuper
            };

            return dataAccess.CheckSuperList(criteria);
        }

        public CopyWinLostLimitResult CopyWinLossLimit(UpdateWinLossLimitRequest request, int userId, string ip)
        {
            var copyWinLostLimitResult = new CopyWinLostLimitResult();

            var casinoSportType = request.SportTypes.FirstOrDefault(sport => sport.ProductId == CasinoProductId);
            var otherSportTypes = request.SportTypes.Where(sport => sport.ProductId != CasinoProductId);
            var superIds = request.SuperIds.Split(',').Select(superId => int.Parse(superId)).ToList();
            var errorResults = new List<Result>();
            var processedCount = 0;

            if (casinoSportType != null)
            {
                processedCount += superIds.Count;
                var casinoResult = memberWinLimitPartialService.MultipleCopySuperMemberWinLimit(request.CustId, superIds, userId);

                errorResults.AddRange(casinoResult.Where(result => !result.ResultError.IsSuccess).Select(result => MappingCustErrorMessage(result.ResultError.ErrorCode, result.ResultError.ErrorMessage, result.CustId, request.SuperList.ToList())));
            }

            if (otherSportTypes.Any())
            {
                processedCount += superIds.Count;
                var products = otherSportTypes
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
                    Products = products
                };

                var otherResults = copySuperWinlossSettingService.Copy(copySuperWinlossSettingRequest);
                var mappingResults = MappingCopyResults(otherResults.ToList(), request.SuperList.ToList());

                errorResults.AddRange(mappingResults.Where(result => result.ErrorCode != 0).Select(result => new Result(result.ErrorCode, result.ErrorMessage)));
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

        private IEnumerable<Result> MappingCopyResults(List<CustomerSetting.Module.Core.Models.Result> copyResults, List<BaseModel> superList)
        {
            var results = new List<Result>();
            for (int i = 0; i < copyResults.Count; i++)
            {
                results.Add(new Result(copyResults[i].ErrorCode, string.Format("{0}: {1}", superList[i].Name, copyResults[i].ErrorMessage)));
            }

            return results;
        }

        private Result MappingCustErrorMessage(int errorCode, string errorMessage,int custId, List<BaseModel> superList)
        {
            var result = new Result(errorCode, errorMessage);
            var super = superList.FirstOrDefault(sp => sp.Id == custId);
            if (super != null)
            {
                result.ErrorMessage = string.Format("{0}: {1}", super.Name, errorMessage);
            }

            return result;
        }
    }
}
