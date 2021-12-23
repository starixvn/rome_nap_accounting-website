namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Result.Core.Habanero;
    using Fanex.Result.Core.LinkedGames;
    using Fanex.Result.Core.MaxGame;
    using Fanex.Result.Core.PPLiveCasino;
    using Fanex.Result.Core.SportsLottery;
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

        public ActionResult SportsLotteryResult(int productId, string custId, string transId3rd)
        {
            SetProductResultTitle(productId);
            var service = new SportsLotteryResult();
            var result = service.GetResultDetail(custId, transId3rd, isDesktop: true);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult VirtualGamesResult(int productId, string gameId)
        {
            SetProductResultTitle(productId);
            var service = new VirtualGamesResult();
            var result = service.GetDetailResult(gameId, "d");

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult LinkedGamesResult(int productId, string custId, string transId3rd)
        {
            SetProductResultTitle(productId);
            var service = new LinkedGamesResult();
            var result = service.GetResult(custId, transId3rd, isDesktop: true);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult MaxGameResult(int productId, string custId, string transId3rd, int betType, string endTime, string siteId)
        {
            SetProductResultTitle(productId);
            var service = new MaxGameResult();
            var result = service.GetResult(custId, transId3rd, betType, endTime, siteId);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult PPLiveCasinoResult(int productId, string custId, string gameId3rd, int betType)
        {
            SetProductResultTitle(productId);
            var service = new PPLiveCasinoResult();
            var result = service.GetResult(custId, gameId3rd, betType);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult HabaneroResult(int productId, string custId, string transId3rd, string gameId3rd, string siteId)
        {
            SetProductResultTitle(productId);
            var service = new HabaneroResult("HabaneroResultConfigs");
            var result = service.GetResult(custId, transId3rd, gameId3rd, siteId);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult CGResult(int productId, string custId, string transId3rd, string gameId3rd, string siteId)
        {
            SetProductResultTitle(productId);
            var service = new HabaneroResult("CGResultConfigs");
            var result = service.GetResult(custId, transId3rd, gameId3rd, siteId);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult AdvantPlayResult(int productId, string custId, string transId3rd, string gameId3rd, string siteId)
        {
            SetProductResultTitle(productId);
            var service = new HabaneroResult("AdvantPlayResultConfigs");
            var result = service.GetResult(custId, transId3rd, gameId3rd, siteId);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult AdvantPlayMiniResult(int productId, string custId, string transId3rd, string gameId3rd, string siteId)
        {
            SetProductResultTitle(productId);
            var service = new HabaneroResult("AdvantPlayMiniResultConfigs");
            var result = service.GetResult(custId, transId3rd, gameId3rd, siteId);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        public ActionResult BBINResult(int productId, int custId, string transId3rd, string gameId3rd, int currency, string siteId)
        {
            SetProductResultTitle(productId);
            var service = new ThirdPartyResult("BBINResultConfigs");
            var request = new ResultRequest(custId, transId3rd, gameId3rd, currency)
            {
                SiteId = siteId
            };
            var result = service.GetResultGET(request);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, new SuccessResult(result)));
        }

        private void SetProductResultTitle(int productId)
            => ProductResultTitle = CacheRef.GetProductName(productId) + " Result";
    }
}
