namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Result.Core.LinkedGames;
    using Fanex.Result.Core.MaxGame;
    using Fanex.Result.Core.SportsLottery;
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

        public ThirdPartyController()
        {
            serviceFactory = new ProductResultServiceFactoryImpl(Logger.Log, new RestClient());
        }

        public ActionResult Result(ThirdPartyResultBaseQuery message)
        {
            var productName = CacheRef.GetProductName(message.ProductId) + " Result";
            var productService = serviceFactory.CreateProductResultService(message.ProductId);
            var result = productService.GetDetailLink(message);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(productName, result));
        }

        public ActionResult SportsLotteryResult(int productId, string custId, string transId3rd)
        {
            var productName = CacheRef.GetProductName(productId) + " Result";
            var service = new SportsLotteryResult();
            var result = service.GetResultDetail(custId, transId3rd, isDesktop: true);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(productName, new SuccessResult(result)));
        }

        public ActionResult VirtualGamesResult(int productId, string gameId)
        {
            var productName = CacheRef.GetProductName(productId) + " Result";
            var service = new VirtualGamesResult();
            var result = service.GetDetailResult(gameId, "d");

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(productName, new SuccessResult(result)));
        }

        public ActionResult LinkedGamesResult(int productId, string custId, string transId3rd)
        {
            var productName = CacheRef.GetProductName(productId) + " Result";
            var service = new LinkedGamesResult();
            var result = service.GetResult(custId, transId3rd, isDesktop: true);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(productName, new SuccessResult(result)));
        }

        public ActionResult MaxGameResult(int productId, string custId, string transId3rd, int betType, string endTime, string siteId)
        {
            var productName = CacheRef.GetProductName(productId) + " Result";
            var service = new MaxGameResult();
            var result = service.GetResult(custId, transId3rd, betType, endTime, siteId);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(productName, new SuccessResult(result)));
        }
    }
}
