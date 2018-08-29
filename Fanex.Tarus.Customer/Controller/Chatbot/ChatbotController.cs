using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Chatbot.Client.Models;
using Chatbot.Client.Services;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.Requests;
using Fanex.Tarus.Customer.ViewModels.Chatbot;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller.Chatbot
{
    [SpecialSiteFilter]
    public class ChatbotController : BaseController
    {
        private readonly SettingService settingService;
        private readonly MessageService messageService;

        public ChatbotController()
        {
            settingService = new SettingServiceImpl();
            messageService = new MessageServiceImpl();
        }

        [AuthenticateRead]
        public async Task<ActionResult> Index(int custId, string username)
        {
            var user = await settingService.GetUser(custId, 0, "en-US");

            return View("~/Views/Customer/Chatbot/Index.cshtml", new ChatbotViewModel(username, user));
        }

        [AuthenticateRead]
        public async Task<ActionResult> GetMessageLogs(GetMessageLogsQuery message)
        {
            var messageLogs = await messageService.GetMessageLogs(
                message.UserId,
                message.FromDate,
                message.ToDate,
                message.FunctionId,
                MessengerType.Letstalk);

            return Json(messageLogs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthenticateHost]
        public async Task<ActionResult> UpdateLanguage(int userId, string language)
        {
            var result = await settingService.UpdateLanguage(userId, language);

            return Json(result);
        }

        [HttpPost]
        [AuthenticateHost]
        public async Task<ActionResult> UpdateFunctions(int userId, IEnumerable<int> functionIds)
        {
            var functions = await settingService.GetFunctions(userId, "en-Us");

            var updatefunctions = functions.Select(
                f => new Function
                {
                    Id = f.Id,
                    Command = f.Command,
                    Enabled = functionIds != null && functionIds.Contains(f.Id)
                });

            var result = await settingService.UpdateFunctions(userId, updatefunctions);

            return Json(result);
        }

        [HttpPost]
        [AuthenticateHost]
        public async Task<ActionResult> Unlink(int userId, string letstalkId)
        {
            var result = await settingService.Unsubscribe(userId, MessengerType.Letstalk, letstalkId);

            return Json(result);
        }
    }
}