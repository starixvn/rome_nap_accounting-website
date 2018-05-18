namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using CustomerSetting.Module.Core.Web.Controllers;
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
            var productName = CacheRef.GetProductName(message.ProductId);
            var productService = serviceFactory.CreateProductResultService(message.ProductId);
            var result = productService.GetDetailLink(message);

            return View("~/Views/BetList/ThirdParty/Result.cshtml", new ThirdPartyResultViewModel(productName, result));
        }
    }
}
