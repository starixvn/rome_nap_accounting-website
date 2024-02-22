namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Result.Core.ThirdParty;
    using Fanex.Result.Core.ThirdParty.Models;
    using Fanex.Tarus.BetList.ViewModel;
    using Infrastructure.Caching;
    using Sunplus.Accounting.DataAccess;

    public class ThirdPartyController : BaseController
    {
        private string ProductResultTitle = string.Empty;

        public ThirdPartyController()
        {
        }

        public ActionResult SportsLotteryResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("SportsLotteryResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId,
                EndTime = request.EndTime,
                Platform = "D"
            };

            var result = service.GetResultGET(resultRequest);
            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));

        }

        public ActionResult VirtualGamesResult(ThirdPartyResultRequest request)
            => BaseResult(request, "VirtualGamesResultConfigs");

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
            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
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
            return Json(new ThirdPartyResultViewModel(ProductResultTitle, result), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PPResult(ThirdPartyResultRequest request) => BaseResult(request, "PPResultConfigs");

        public ActionResult HabaneroResult(ThirdPartyResultRequest request) => BaseResult(request, "HabaneroResultConfigs");

        public ActionResult CGResult(ThirdPartyResultRequest request) => BaseResult(request, "CGResultConfigs");

        public ActionResult AdvantPlayResult(ThirdPartyResultRequest request) => BaseResult(request, "AdvantPlayResultConfigs");

        public ActionResult AdvantPlayMiniResult(ThirdPartyResultRequest request) => BaseResult(request, "AdvantPlayMiniResultConfigs");

        public ActionResult JokerResult(ThirdPartyResultRequest request) => BaseResult(request, "JokerResultConfigs");

        public ActionResult PGSoftResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("PGSoftResultConfigs");
            var ipInfo = IBCProvider.Utils.GetIPInformation(HttpContext.Request.UserHostAddress);
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                Location = ipInfo != null ? ipInfo.CountryShort : string.Empty
            };

            var result = service.GetResultGET(resultRequest);
            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult IONResult(ThirdPartyResultRequest request) => BaseResult(request, "IONCasinoResultConfigs");

        public ActionResult RNGWarResult(ThirdPartyResultRequest request) => BaseResult(request, "RNGWarResultConfigs");

        public ActionResult SpadegamingResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("SpadegamingResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult CardClubResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("CardClubResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult BGResult(ThirdPartyResultRequest request) => BaseResult(request, "BGResultConfigs");

        public ActionResult RNGCasinoResult(ThirdPartyResultRequest request) => BaseResult(request, "RNGCasinoResultConfigs");

        public ActionResult VoidbridgeResult(ThirdPartyResultRequest request)
        => BaseResult(request, "VoidbridgeResultConfigs");

        public ActionResult BBINResult(ThirdPartyResultRequest request) => BaseResult(request, "BBINResultConfigs");

        public ActionResult MGRNGCasinoResult(ThirdPartyResultRequest request) //=> BaseResult(request, "MGResultConfigs");
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("MGResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult FGGResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("FGGResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                EndTime = request.EndTime
            };

            var result = service.GetResultGET(resultRequest);
            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult SEALResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("SEALResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }        

        private ActionResult BaseResult(ThirdPartyResultRequest request, string configName)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult(configName);
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId);
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult JiliResult(ThirdPartyResultRequest request)
            => BaseResult(request, "JiliResultConfigs");

        public ActionResult WEResult(ThirdPartyResultRequest request)
            => BaseResult(request, "WEResultConfigs");

        public ActionResult Live22Result(ThirdPartyResultRequest request)
            => BaseResult(request, "Live22ResultConfigs");

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
