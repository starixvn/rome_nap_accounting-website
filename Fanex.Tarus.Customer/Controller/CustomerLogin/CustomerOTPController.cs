namespace Fanex.Tarus.Customer.Controller.CustomerLogin
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.CustomerLogin;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using TelegramBot.Client.Models;
    using TelegramBot.Client.Services;
    using TelegramBot.Constants;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CustomerOTPController : BaseController
    {
        private readonly ICustomerOTPService _service;
        private readonly ISettingService _settingService;

        public CustomerOTPController()
            : this(null, null)
        {
        }

        public CustomerOTPController(ICustomerOTPService service, ISettingService settingService)
        {
            _service = service ?? new CustomerOTPService();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _settingService = settingService ?? new SettingService(new HttpClient() { BaseAddress = new System.Uri(ConfigurationManager.AppSettings["TelegramBotApiUri"]) });
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/CustomerLogin/CustomerOTP/Index.cshtml");
        }

        [HttpPost, AuthenticateHost]
        public async Task<JsonResult> RemoveOTP(int custId)
        {
            var result = this._service.RemoveOTP(custId);

            var messenger = await _settingService.GetCustBotMessengerAsync(custId, 0, "en-US");
            if (IsEnabledOTPViaTelegram(messenger)){
                var updateResult = await _settingService.UpdateUserFunction(messenger.UserId, messenger.BotId, new Function { FunctionId = FunctionIds.OTP, Enabled = false }, AppSession.AccId, Request.UserHostAddress);
                return base.Json(new Entities.Result { ErrorCode = updateResult.Code, ErrorMessage = updateResult.Message }, JsonRequestBehavior.AllowGet);
            }

            return base.Json(result, JsonRequestBehavior.AllowGet);
        }

        private bool IsEnabledOTPViaTelegram(CustBotMessenger messenger)
        {
            return messenger != null && messenger.Functions.FirstOrDefault(f => f.FunctionId == FunctionIds.OTP).Enabled;
        }
    }
}
