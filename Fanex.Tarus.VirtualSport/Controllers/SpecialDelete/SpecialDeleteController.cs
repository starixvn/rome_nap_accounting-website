

using System.Web.Mvc;
using Fanex.Tarus.VirtualSport.Business.SpecialDelete;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.VirtualSport.Controllers.SpecialDelete
{
    public class SpecialDeleteController : BaseController
    {
        #region IFields

        //GET: /RecoverBetsService/
        private readonly ISpecialDeleteService _specialDeleteService;

        #endregion IFields
        #region IConstructions

        public SpecialDeleteController()
            : this(null)
        {
        }

        public SpecialDeleteController(ISpecialDeleteService specialDeleteService)
        {
            _specialDeleteService = specialDeleteService ?? new BaseSpecialDelete();
        }

        #endregion IConstructions
        #region IMethods  
    
        [HttpPost]
        public JsonResult CheckRefNo(string matchid, string fromRefno, string toRefno)
        {
            var result = _specialDeleteService.CheckRefNo(matchid, fromRefno, toRefno);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }


        #endregion IMethods
    }
}
