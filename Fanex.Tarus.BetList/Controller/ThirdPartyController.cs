namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Result.Core.ThirdParty;
    using Fanex.Result.Core.ThirdParty.Models;
    using Fanex.Result.Core.VirtualGames;
    using Fanex.Result.ThirdParty.Models;
    using Infrastructure.Caching;
    using Logging;
    using RestSharp;
    using Result.ThirdParty.Models.Requests;
    using Result.ThirdParty.Models.Views;
    using Result.ThirdParty.Services;
    using Result.ThirdParty.Services.Impl;

    public class ThirdPartyController : BaseController
    {
        private readonly ProductResultServiceFactory serviceFactory;
        private string ProductResultTitle = string.Empty;

        public ThirdPartyController()
        {
            serviceFactory = new ProductResultServiceFactoryImpl(Logger.Log, new RestClient());
        }

        public ActionResult Result(ThirdPartyResultBaseQuery message)
        {
            SetProductResultTitle(message.ProductId);
            var productService = serviceFactory.CreateProductResultService(message.ProductId);
            var result = productService.GetDetailLink(message);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult SportsLotteryResult(ThirdPartyResultRequest request) => BaseResult(request, "SportsLotteryResultConfigs");

        public ActionResult VirtualGamesResult(int productId, string gameId)
        {
            SetProductResultTitle(productId);
            var service = new VirtualGamesResult();
            var result = service.GetDetailResult(gameId, "d");

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult LinkedGamesResult(ThirdPartyResultRequest request) => BaseResult(request, "LinkedGamesResultConfigs");

        public ActionResult MaxGameResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("MaxGameResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId,
                EndTime = request.EndTime
            };

            var result = service.GetResultGET(resultRequest);
            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }
        public ActionResult PPLiveCasinoResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("PPLiveCasinoResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };

            var result = service.GetResultGET(resultRequest);
            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult PPResult(ThirdPartyResultRequest request) => BaseResult(request, "PPResultConfigs");

        public ActionResult HabaneroResult(ThirdPartyResultRequest request) => BaseResult(request, "HabaneroResultConfigs");

        public ActionResult CGResult(ThirdPartyResultRequest request) => BaseResult(request, "CGResultConfigs");

        public ActionResult AdvantPlayResult(ThirdPartyResultRequest request) => BaseResult(request, "AdvantPlayResultConfigs");

        public ActionResult AdvantPlayMiniResult(ThirdPartyResultRequest request) => BaseResult(request, "AdvantPlayMiniResultConfigs");
        public ActionResult JokerResult(ThirdPartyResultRequest request) => BaseResult(request, "JokerResultConfigs");

        public ActionResult PGSoftResult(ThirdPartyResultRequest request) => BaseResult(request, "PGSoftResultConfigs");

        public ActionResult GDGResult(ThirdPartyResultRequest request) => BaseResult(request, "GDGResultConfigs");

        public ActionResult IONResult(ThirdPartyResultRequest request) => BaseResult(request, "IONCasinoResultConfigs");

        public ActionResult RNGWarResult(ThirdPartyResultRequest request) => BaseResult(request, "RNGWarResultConfigs");

        public ActionResult SpadegamingResult(ThirdPartyResultRequest request) => BaseResult(request, "SpadegamingResultConfigs");

        public ActionResult CardClubResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("CardClubResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }
        public ActionResult BGResult(ThirdPartyResultRequest request) => BaseResult(request, "BGResultConfigs");

        public ActionResult RNGCasinoResult(ThirdPartyResultRequest request) => BaseResult(request, "RNGCasinoResultConfigs");

        public ActionResult VoidbridgeResult(ThirdPartyResultRequest request)
        => BaseResult(request, "VoidbridgeResultConfigs");

        public ActionResult BBINResult(ThirdPartyResultRequest request) => BaseResult(request, "BBINResultConfigs");

        private ActionResult BaseResult(ThirdPartyResultRequest request, string configName)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult(configName);
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId);
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        private void SetProductResultTitle(int productId)
            => ProductResultTitle = CacheRef.GetProductName(productId) + " Result";
    }

    public class ThirdPartyResultRequest
    {
        public int ProductId { get; set; }

        public string SiteId { get; set; }

        public int CustId { get; set; }

        public string TransId3rd { get; set; }

        public string GameId3rd { get; set; }

        public string Language { get; set; }

        public int CurrencyId { get; set; }

        public int BetTypeId { get; set; }

        public string EndTime { get; set; }
    }
}
