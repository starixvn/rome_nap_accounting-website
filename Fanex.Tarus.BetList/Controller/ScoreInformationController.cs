// --------------------------------------------------------------------------
// <copyright file="ScoreInformationController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Controller
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Tarus.BetList.Business.ScoreInformation;
    using Fanex.Tarus.BetList.Criteria.ScoreInformation;
    

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ScoreInformationController : Controller
    {
        private readonly IScoreInformationService _service;

        public ScoreInformationController()
            : this(null)
        {
        }

        public ScoreInformationController(IScoreInformationService service)
        {
            this._service = service ?? new ScoreInformationService();
        }

        [HttpPost]
        public JsonResult GetScoreInformation(GetScoreInformationCriteria criteria)
        {
            var result = this._service.GetScoreInfo(criteria);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
