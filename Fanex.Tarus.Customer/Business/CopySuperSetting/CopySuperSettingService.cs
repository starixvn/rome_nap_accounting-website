namespace Fanex.Tarus.Customer.Business.CopySuperSetting
{
    using System;
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
    using Fanex.Tarus.Infrastructure.Extensions;

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
            try
            {
                var copyWinLostLimitResult = new CopyWinLostLimitResult();

                var casinoSportType = request.SportTypes.FirstOrDefault(sport => sport.ProductId == CasinoProductId);
                var otherSportTypes = request.SportTypes.Where(sport => sport.ProductId != CasinoProductId);
                var superIds = request.SuperIds.Split(',').Select(superId => int.Parse(superId)).ToList();
                var errorResults = new List<Result>();

                if (casinoSportType != null)
                {
                    var casinoResult = memberWinLimitPartialService.MultipleCopySuperMemberWinLimit(request.CustId, superIds, userId);

                    errorResults.AddRange(casinoResult.Where(result => !result.ResultError.IsSuccess).Select(result => new Result(result.ResultError.ErrorCode, result.ResultError.ErrorMessage)));
                }

                if (otherSportTypes != null)
                {
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
                    errorResults.AddRange(otherResults.Where(result => !result.IsSuccess).Select(result => new Result(result.ErrorCode, result.ErrorMessage)));
                }

                if (errorResults.Count > 0)
                {
                    return new CopyWinLostLimitResult
                    {
                        ErrorCode = errorResults.Count == superIds.Count ? -1 : 0,
                        CopyErrors = errorResults
                    };
                }

                return copyWinLostLimitResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                return new CopyWinLostLimitResult
                {
                   ErrorCode = -2,
                   ErrorMessage = "Internal Server Error."
                };
            }
        }
    }
}
