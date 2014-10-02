namespace Fanex.Tarus.Customer.Controller.CustomerList
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.AutoTransferAll;
    using Fanex.Tarus.Customer.Criteria.AutoTransferAll;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.AutoTransferAll;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// Auto transfer controller.
    /// </summary>
    public class AutoTransferAllController : BaseController
    {
        private IAutoTransferAllService _autoTransferAllService = null;

        public AutoTransferAllController()
            : this(null)
        {
        }

        public AutoTransferAllController(AutoTransferAllService autoTransferAllService)
        {
            _autoTransferAllService = autoTransferAllService ?? new AutoTransferAllService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/AutoTransferAll/AutomateTransferList/Index.cshtml");
        }

        [AuthenticateRead]
        public ActionResult AddAgent()
        {
            return View("~/Views/Customer/AutoTransferAll/AddAgent/AddAgent.cshtml");
        }

        [AuthenticateRead]
        [HttpPost]
        public ActionResult GetAutoTransfers(SelectTransferAgentCriteria criteria)
        {
            var model = _autoTransferAllService.GetAutoTranfers(criteria);
            return PartialView("~/Views/Customer/AutoTransferAll/AutomateTransferList/_AutomateTransferList.cshtml", model);
        }

        [AuthenticateHost]
        [HttpPost]
        public JsonResult UpdatedTransfer(UpdatedTransferCriteria criteria)
        {
            var result = _autoTransferAllService.UpdatedTransfer(criteria);
            return Json(result);
        }

        /// <summary>
        /// Clear balance controller.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="transferType">Transfer type.</param>
        /// <returns>Valid page.</returns>
        [AuthenticateHost]
        [HttpPost]
        public JsonResult ClearBalance(string username, int transferType)
        {
            Result result = new Result();

            if (transferType == 1)
            {
                result = _autoTransferAllService.ClearBalanceMembers(new ClearBalanceMembersCriteria
                {
                    AgentName = username
                });
            }
            else
            {
                result = _autoTransferAllService.ClearBalanceDownlines(new ClearBalanceDownlineCriteria
                {
                    AgentName = username
                });
            }

            return Json(result);
        }

        [AuthenticateHost]
        [HttpPost]
        public JsonResult DeletedTransfer(DeletedTransferCriteria criteria)
        {
            var result = _autoTransferAllService.DeletedTransfer(criteria);
            return Json(result);
        }

        [AuthenticateHost]
        [HttpPost]
        public JsonResult InsertAgentsFromPopup(InsertAgentsFromPopupCriteria criteria)
        {
            var result = _autoTransferAllService.InsertAgentsFromPopup(criteria);
            return Json(result);
        }

        [AuthenticateHost]
        public ActionResult ExportTemplateExcelFile()
        {
            var templateFile = _autoTransferAllService.ExportExcelFile();

            return File(templateFile.ToArray(), "application/vnd.ms-excel", "Template.xls");
        }

        /// <summary>
        /// Check valid agent from file controller.
        /// </summary>
        /// <param name="file">File upload.</param>
        /// <returns>Valid page.</returns>
        [HttpPost]
        public ActionResult CheckValidAgentsFromFile(HttpPostedFileBase file)
        {
            ValidAgents validUsernames = new ValidAgents();
            string validFile = CheckValidExcelFile(file);
            ViewBag.ValidFile = validFile;

            if (string.IsNullOrEmpty(validFile))
            {
                validUsernames = _autoTransferAllService.CheckValidAgentsFromFile(file);
                ViewBag.Valid = "Valid agent(s).";
                ViewBag.InValid = "Invalid agent(s), please check!";
            }

            return PartialView("~/Views/Customer/AutoTransferAll/AddAgent/_ValidAgent.cshtml", validUsernames);
        }

        /// <summary>
        /// Insert agents from file.
        /// </summary>
        /// <param name="file">File upload.</param>
        /// <returns>Valid page.</returns>
        [AuthenticateHost]
        [HttpPost]
        public ActionResult InsertAgentsFromFile(HttpPostedFileBase file)
        {
            ViewBag.Valid = "Add agent(s) successfully!";
            ValidAgents successUsernames = _autoTransferAllService.InsertAgentsFromFile(file);
            return PartialView("~/Views/Customer/AutoTransferAll/AddAgent/_ValidAgent.cshtml", successUsernames);
        }

        /// <summary>
        /// Check is excel file.
        /// </summary>
        /// <param name="file">Upload file.</param>
        /// <returns>String object.</returns>
        private string CheckValidExcelFile(HttpPostedFileBase file)
        {
            if (!IsExcelFile(file))
            {
                return "The uploading file is not a excel file, please try again!";
            }

            if (!IsValidFileSize(file))
            {
                return "The excel’s file size must be between 0 and 1MB.";
            }

            return string.Empty;
        }

        /// <summary>
        /// Check is excel file.
        /// </summary>
        /// <param name="file">Upload file.</param>
        /// <returns>Is excel file.</returns>
        private bool IsExcelFile(HttpPostedFileBase file)
        {
            string[] formats = new string[] { ".xls", ".xlsx" };

            bool validExtentionImage = false;

            foreach (var item in formats)
            {
                if (file.FileName.ToLower().Contains(item))
                {
                    validExtentionImage = true;
                }
            }

            if (validExtentionImage)
            {
                return true;
            }

            return false;
        }

        private bool IsValidFileSize(HttpPostedFileBase file)
        {
            return file.ContentLength < 1048576 ? true : false;
        }
    }
}
