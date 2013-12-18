using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Tarus.Entities.Customer.DepositCustomer;

namespace Fanex.Tarus.Web.Controllers.Customer
{
    using Business.Implement.Customer;
    using Business.Interface.Customer;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Fanex.Tarus.Infrastructure.Common;

    public class DepositCustomerController : BaseController
    {
        private readonly IDepositCustomerService _depositCustomerService;

        public DepositCustomerController(IDepositCustomerService depositCustomerService)
        {
            _depositCustomerService = depositCustomerService ?? new DepositCustomerService();
        }

        public DepositCustomerController()
            : this(null)
        {

        }

        [AuthenticateRead]
        public ActionResult NewLicensee()
        {
            return View("~/Views/Customer/DepositCustomer/NewLicensee.cshtml");
        }

        [AuthenticateHost]
        public JsonResult NewLicenseeSubmitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.NewLicenseeAddNewSuper(postParam), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateRead]
        public ActionResult EditSettingSuper()
        {
            ViewData["title"] = "Edit Super PT";
            ViewData["superId"] = AppSession.CustID;
            ViewData["username"] = "Super: " + AppSession.CustUName;

            var customerService = new Sunplus.Accounting.Business.Customer.CustomerService();
            var ds = customerService.GetCustomerInfo(AppSession.CustUName, AppSession.SYSTEM);

            if (ds == null || ds.ResultSet.Tables.Count == 0 || ds.ResultSet.Tables[0].Rows.Count == 0)
            {
                return View("~/Views/Shared/NoInfo.cshtml");
            }
            var row = ds.ResultSet.Tables[0].Rows[0];
            var roleId = DNAide.Core.CastAide.AsInt32(row["roleid"]);
            ViewData["roleId"] = roleId.ToString();

            if (roleId != 4)
            {
                return RedirectToAction("EditSettingAgent");
            }

            var dataJson = _depositCustomerService.GetSuperSetting(AppSession.CustID);
            return View("~/Views/Customer/DepositCustomer/EditSettingSuper.cshtml",
                new Models.Customer.DepositCustomer.SuperSetting { DataJson = string.IsNullOrWhiteSpace(dataJson) ? "null" : dataJson });
        }

        [AuthenticateRead]
        public ActionResult EditSettingAgent()
        {
            var customerService = new Sunplus.Accounting.Business.Customer.CustomerService();
            var ds = customerService.GetCustomerInfo(AppSession.CustUName, AppSession.SYSTEM);

            if (ds == null || ds.ResultSet.Tables.Count == 0 || ds.ResultSet.Tables[0].Rows.Count == 0)
            {
                return View("~/Views/Shared/NoInfo.cshtml");
            }
            var row = ds.ResultSet.Tables[0].Rows[0];
            var roleId = DNAide.Core.CastAide.AsInt32(row["roleid"]);

            if (roleId == 4)
            {
                return RedirectToAction("EditSettingSuper");
            }

            if (roleId == 3)
            {
                ViewData["username"] = "Master: " + AppSession.CustUName;
                ViewData["title"] = "Edit Master PT";
            }
            else if (roleId == 2)
            {
                ViewData["username"] = "Agent: " + AppSession.CustUName;
                ViewData["title"] = "Edit Agent PT";
            }
            else
            {
                ViewData["username"] = "Member: " + AppSession.CustUName;
                ViewData["title"] = "Edit Member PT";
            }

            ViewData["roleId"] = roleId.ToString();

            string dataJson;

            if (roleId == 3)
            {
                dataJson = _depositCustomerService.GetMasterSetting(AppSession.CustID);
            }
            else if (roleId == 2)
            {
                dataJson = _depositCustomerService.GetAgentSetting(AppSession.CustID);
            }
            else
            {
                dataJson = _depositCustomerService.GetMemberSetting(AppSession.CustID);
            }

            return View("~/Views/Customer/DepositCustomer/EditSettingAgent.cshtml",
                new Models.Customer.DepositCustomer.SuperSetting { DataJson = string.IsNullOrWhiteSpace(dataJson) ? "null" : dataJson });
        }

        [AuthenticateRead]
        public ActionResult AddNewAgent()
        {
            var customerService = new Sunplus.Accounting.Business.Customer.CustomerService();
            var ds = customerService.GetCustomerInfo(AppSession.CustUName, AppSession.SYSTEM);

            if (ds == null || ds.ResultSet.Tables.Count == 0 || ds.ResultSet.Tables[0].Rows.Count == 0)
            {
                return View("~/Views/Shared/NoInfo.cshtml");
            }
            var row = ds.ResultSet.Tables[0].Rows[0];
            var roleId = DNAide.Core.CastAide.AsInt32(row["roleid"]) - 1;
            if (roleId == 3)
            {
                ViewData["username"] = "Add New Master to " + AppSession.CustUName;
                ViewData["title"] = "Add New Master";
            }
            else if (roleId == 2)
            {
                ViewData["username"] = "Add New Agent to " + AppSession.CustUName;
                ViewData["title"] = "Add New Agent";
            }
            else
            {
                ViewData["username"] = "Add New Member to " + AppSession.CustUName;
                ViewData["title"] = "Add New Member";
            }

            ViewData["roleId"] = roleId.ToString();
            string dataJson;
            switch (roleId)
            {
                case 3:
                    dataJson = _depositCustomerService.GetSuperSetting(AppSession.CustID);
                    break;
                case 2:
                    dataJson = _depositCustomerService.GetMasterSettingForAddNew(AppSession.CustID, roleId);
                    break;
                default:
                    dataJson = _depositCustomerService.GetAgentSettingForAddNew(AppSession.CustID, roleId);
                    break;
            }

            return View("~/Views/Customer/DepositCustomer/AddNewAgent.cshtml",
                    new Models.Customer.DepositCustomer.SuperSetting { DataJson = string.IsNullOrWhiteSpace(dataJson) ? "null" : dataJson });
        }

        [AuthenticateHost]
        public JsonResult AddNewMasterSumitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.NewLicenseeAddNewMaster(postParam), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult AddNewAgentSumitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.NewLicenseeAddNewAgent(postParam), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult AddNewMemberSumitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.NewLicenseeAddNewMember(postParam), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult UpdateSuperSettingSumitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.UpdateSuperSetting(postParam), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult UpdateMasterSettingSumitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.UpdateMasterSetting(postParam), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult UpdateAgentSettingSumitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.UpdateAgentSetting(postParam), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult UpdateMemberSettingSumitHandler(NewLicenseePostParam postParam)
        {
            return Json(_depositCustomerService.UpdateMemberSetting(postParam), JsonRequestBehavior.AllowGet);
        }
    }
}
