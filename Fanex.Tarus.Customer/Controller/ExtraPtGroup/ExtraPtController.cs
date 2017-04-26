using System.Linq;
using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Business.ExtraPtGroup;
using Fanex.Tarus.Customer.Criteria.ExtraPtGroup;
using Fanex.Tarus.Customer.Entities.ExtraPtGroup;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Security.Permission;
using Fanex.Tarus.Web.Controllers;
using Newtonsoft.Json;
using Sunplus.Accounting.Business.Security;

namespace Fanex.Tarus.Customer.Controller.ExtraPtGroup
{
    public class ExtraPtController : BaseController
    {
        private readonly IExtraPtService _service = new ExtraPtService();

        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Index(byte productId = 0)
        {
            var groupsByProduct = _service.GetGroupsByProduct(new GroupByProductCriteria { ProductId = productId });
            var products = _service.GetProducts(new ProductCriteria { IsExtra = true });
            if (!Permissions.HasPermission(Sunplus.Accounting.Core.Common.Enumerations.Functions.AccessSpecialSites))
            {
                products = (from p in products
                           where p.ProductId != 23
                           select p).ToList();
            }

            var model = new GroupByProductViewModel { Products = products, GroupsByProduct = groupsByProduct, CurrentProductId = productId };

            return View("~/Views/Admin/ExtraPtGroup/Index.cshtml", model);
        }

        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Edit(byte groupId = 0, byte productId = 0)
        {
            var groups = _service.GetGroups(new GroupCriteria());
            var products = _service.GetProducts(new ProductCriteria { IsExtra = false });
            var groupByProduct = _service.GetGroupsByProduct(new GroupByProductCriteria { ProductId = productId })
                .FirstOrDefault(g => g.Group.GroupId == groupId && g.Product.ProductId == productId);
            if (!Permissions.HasPermission(Sunplus.Accounting.Core.Common.Enumerations.Functions.AccessSpecialSites))
            {
                products = (from p in products
                            where p.ProductId != 23
                            select p).ToList();
            }

            var model = new ModifyGroupByProductViewModel
            {
                Products = products,
                Groups = groups,
                SelectedItem = groupByProduct
            };

            return View("~/Views/Admin/ExtraPtGroup/Edit.cshtml", model);
        }

        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Add()
        {
            var groups = _service.GetGroups(new GroupCriteria());
            var products = _service.GetProducts(new ProductCriteria { IsExtra = false });
            if (!Permissions.HasPermission(Sunplus.Accounting.Core.Common.Enumerations.Functions.AccessSpecialSites))
            {
                products = (from p in products
                            where p.ProductId != 23
                            select p).ToList();
            }

            var model = new AddGroupByProductViewModel { Products = products, Groups = groups };

            return View("~/Views/Admin/ExtraPtGroup/Add.cshtml", model);
        }

        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Host)]
        public ActionResult Insert(string data)
        {
            var model = JsonConvert.DeserializeObject<GroupByProduct>(data);
            var result = _service.InsertGroupByProduct(new InsertGroupByProductCriteria
            {
                EffectiveDate = model.EffectiveDate,
                EndDate = model.EndDate,
                ExtraPt = model.ExtraPt / 100,
                GroupId = model.Group.GroupId,
                ProductId = model.Product.ProductId,
                UserId = AppSession.AccId
            });

            return Json(new Result(result.ErrorCode, result.IsSuccess ? "New setting has been successfully created." : result.ErrorMessage));
        }

        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Host)]
        public ActionResult Update(string data)
        {
            var model = JsonConvert.DeserializeObject<UpdateGroupByProduct>(data);
            var result = _service.UpdateGroupByProduct(new UpdateGroupByProductCriteria
            {
                NewExtraPt = model.NewExtraPt / 100,
                OldExtraPt = model.OldExtraPt / 100,
                GroupId = model.Group.GroupId,
                NewEffectiveDate = model.NewEffectiveDate,
                NewEndDate = model.NewEndDate,
                OldEffectiveDate = model.OldEffectiveDate,
                OldEndDate = model.OldEndDate,
                ProductId = model.Product.ProductId,
                UserId = AppSession.AccId
            });

            return Json(new Result(result.ErrorCode, result.IsSuccess ? "New setting has been successfully updated." : result.ErrorMessage));
        }

        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Host)]
        public ActionResult Delete(string data)
        {
            var model = JsonConvert.DeserializeObject<GroupByProduct>(data);
            var result = _service.DeleteGroupByProduct(new DeleteGroupByProductCriteria
            {
                EffectiveDate = model.EffectiveDate,
                EndDate = model.EndDate,
                ExtraPt = model.ExtraPt / 100,
                GroupId = model.Group.GroupId,
                ProductId = model.Product.ProductId,
                UserId = AppSession.AccId
            });

            return Json(new Result(result.ErrorCode, result.IsSuccess ? "The selected Extra PT setting was successfully deleted." : result.ErrorMessage));
        }
    }
}
