namespace Fanex.Tarus.BetList.Controller
{
    using System.Collections.Generic;
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
        { }

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

        public ActionResult PPResult(ThirdPartyResultRequest request)
        {
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };

            return BaseResult(request, resultRequest, "PPResultConfigs");
        }

        public ActionResult HabaneroResult(ThirdPartyResultRequest request) => BaseResult(request, "HabaneroResultConfigs");

        public ActionResult CGResult(ThirdPartyResultRequest request) => BaseResult(request, "CGResultConfigs");

        public ActionResult AdvantPlayResult(ThirdPartyResultRequest request)
        {
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };

            return BaseResult(request, resultRequest, "AdvantPlayResultConfigs");
        }

        public ActionResult SabaKenoResult(ThirdPartyResultRequest request)
        {
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };
            ViewBag.ProductId = request.ProductId;
            return BaseResult(request, resultRequest, "SabaKenoResultConfigs");
        }

        public ActionResult AdvantPlayMiniResult(ThirdPartyResultRequest request) => BaseResult(request, "AdvantPlayMiniResultConfigs");

        public ActionResult JokerResult(ThirdPartyResultRequest request) => BaseResult(request, "JokerResultConfigs");

        public ActionResult PGSoftResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("PGSoftResultConfigs");
            var ipInfo = IBCProvider.Utils.GetIPInformation(HttpContext.Request.UserHostAddress);
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                Location = ipInfo != null ? ipInfo.CountryShort : string.Empty,
                BetTypeId = request.BetTypeId
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

        public ActionResult JiliResult(ThirdPartyResultRequest request)
        {

            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId,
                TransDesc = request.TransDesc
            };

            return BaseResult(request, resultRequest, "JiliResultConfigs");
        }

        public ActionResult WEResult(ThirdPartyResultRequest request)
            => BaseResult(request, "WEResultConfigs");

        public ActionResult SexyGamingResult(ThirdPartyResultRequest request)
        {
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                Site = request.GameId3rd
            };

            return BaseResult(request, resultRequest, "SexyGamingResultConfigs");
        }        

        public ActionResult Live22Result(ThirdPartyResultRequest request)
            => BaseResult(request, "Live22ResultConfigs");

        public ActionResult UUSlotsResult(ThirdPartyResultRequest request)
            => BaseResult(request, "UUSlotsResultConfigs");

        public ActionResult VGamingResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("VGamingConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                BetTypeId = request.BetTypeId
            };
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }

        public ActionResult FastSpinResult(ThirdPartyResultRequest request)
            => BaseResult(request, "FastSpinResultConfigs");

        public ActionResult FACHAIResult(ThirdPartyResultRequest request)
            => BaseResult(request, "FACHAIResultConfigs");

        public ActionResult NextspinResult(ThirdPartyResultRequest request)
            => BaseResult(request, "NextspinResultConfigs");

        public ActionResult ONCasinoResult(ThirdPartyResultRequest request)
            => BaseResult(request, "ONCasinoResultConfigs");

        public ActionResult PlaytechResult(ThirdPartyResultRequest request)
            => BaseResult(request, "PlaytechResultConfigs");
            
        public ActionResult GPIResult(ThirdPartyResultRequest request)
            => BaseResult(request, "GPIRNGResultConfigs");

        public ActionResult PlayStarResult(ThirdPartyResultRequest request)
            => BaseResult(request, "PlayStarResultConfigs");

        public ActionResult GPILiveCasinoResult(ThirdPartyResultRequest request)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult("GPILiveCasinoResultConfigs");
            var resultRequest = new ResultRequest(request.CustId, request.TransId3rd, request.GameId3rd, request.CurrencyId, request.SiteId)
            {
                EndTime = request.EndTime,
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

        private ActionResult BaseResult(ThirdPartyResultRequest request, ResultRequest resultRequest, string configName)
        {
            SetProductResultTitle(request.ProductId);
            var service = new ThirdPartyResult(configName);
            var result = service.GetResultGET(resultRequest);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(ProductResultTitle, result));
        }


        public ActionResult SabaxDResult(ThirdPartyResultRequest request)
            => BaseResult(request, "SabaxDResultConfigs");
        
        public ActionResult ASKMESLOTResult(ThirdPartyResultRequest request)
        {
            ViewBag.ProductId = request.ProductId;

            return BaseResult(request, "ASKMESLOTResultConfigs");
        }

        public ActionResult FunkyGamesResult(ThirdPartyResultRequest request)
            => BaseResult(request, "FunkyGamesResultConfigs");

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

        public string Site { get; set; }
        public string TransDesc { get; set; }
    }
}
