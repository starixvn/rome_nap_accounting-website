

using System.Web.Mvc;
using Fanex.Tarus.VirtualSport.Business.SpecialDelete;
using Fanex.Tarus.VirtualSport.Entities.SpecialDelete;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.VirtualSport.Controllers.SpecialDelete
{
    public class DeleteBetsController : BaseController
    {
        #region IFields

        //GET: /DeleteBetsService/
        private readonly IDeleteBetsService _deleteBetsService;

        #endregion IFields
        #region IConstructions

        public DeleteBetsController()
            : this(null)
        {
        }

        public DeleteBetsController(IDeleteBetsService deleteBetsService)
        {
            _deleteBetsService = deleteBetsService ?? new DeleteBetsService();
        }

        #endregion IConstructions
        #region IMethods
        #region DeleteBets

        [AuthenticateRead]
        public ActionResult DeleteBets()
        {
            return View("~/Views/VirtualSport/SpecialDelete/DeleteBets.cshtml");
        }

        [HttpPost]
        public JsonResult GetBettypes(int type)
        {
            var result = _deleteBetsService.GetAllBettypes(type);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
     
        [HttpPost]
        public JsonResult BettransSelSpecialDel(SpecialDeleteParam param)
        {
            var result = _deleteBetsService.BettransSelSpecialDel(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DeleteSpecialTransaction(SpecialDeleteParam param)
        {
            var result = _deleteBetsService.DeleteSpecialTransaction(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion DeleteBets
        #endregion IMethods
    }
}
