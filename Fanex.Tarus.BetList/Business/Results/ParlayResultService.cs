// --------------------------------------------------------------------------
// <copyright file="ParlayResultService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;
    using ONEbook.NG2.API;
    using Sunplus.Accounting.Core.Common;
    using System.Linq;

    public class ParlayResultService : IParlayResultService
    {
        private IParlayResultDataAccess _parlayResultDataAccess;

        public ParlayResultService(IParlayResultDataAccess parlayResultDataAccess)
        {
            this._parlayResultDataAccess = parlayResultDataAccess ?? new ParlayResultDataAccess();
        }

        public ParlayResultService()
            : this(null)
        {
        }

        public string GetResult(long refNo)
        {
            var parlayData = this._parlayResultDataAccess.GetParlayResult(new GetParlayResultCriteria
            {
                RefNo = refNo
            });

            return ResultEngine.HtmlParser(parlayData.ToList(), this.GetFanexResultSetting()).Replace("original-title", " title ");
        }

        public string GetNG2Paraly(long refNo)
        {
            var service = new Services();
            var req = new ONEbook.NG2.API.DTO.ShowParlayDetail.Request();
            req.Auth.SiteId = 3000;
            req.UserId = AppSession.AccId;
            req.Call.TransactionId = refNo.ToString();
            var res = service.ShowParlayDetail(req);

            if (res.Result.IsSuccess)
            {
                if (string.IsNullOrWhiteSpace(res.Result.Url))
                {
                    return "<div>There is no information available.</div>";
                }

                /*using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(res.Result.Url);

                    return "<link rel=\"stylesheet\" type=\"text/css\" href=\"css/ng2.css\"/>" + s;
                }*/

                return "<script type=\"text/javascript\">location.href='" + res.Result.Url + "';</script>";
            }

            return "<div style=\"color: #ff0000; font-weight: bold\">" + res.Result.ErrorDescription + "</div>";
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <returns>The object of ResultSetting.</returns>
        private ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                IsOutright = false,
                IsParlayPopUp = true,
                IsPopUp = true,
                VisiblePopUpTitle = false,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = false,
                HighlightWinnerTeam = true
            };
        }
    }
}