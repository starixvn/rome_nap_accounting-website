namespace Fanex.Tarus.BetList.Business
{
    using System;
    using System.Linq;
    using System.Net;
    using Fanex.BetList.ColossusBets.Builder;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.DataAccess;
    using Fanex.Tarus.BetList.Entities.ColossusBets;
    using Fanex.Tarus.BetList.Resources;
    using Fanex.Tarus.Entities;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Newtonsoft.Json;

    public class ColossusBetsService : IColossusBetsService
    {
        private readonly IColossusBetsRepository _colossusBetsRepository;

        public ColossusBetsService()
            : this(null)
        {
        }

        public ColossusBetsService(IColossusBetsRepository colossusBetsRepository)
        {
            _colossusBetsRepository = colossusBetsRepository ?? new ColossusBetsRepository();
        }

        public string BetDetails(GetColossusBetsBetDetailsCriteria criteria)
        {
            ITicketHelper ticketHelper = new TicketHelper();
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            var detailsTickets = _colossusBetsRepository.BetDetails(criteria).ToList();
            for (var i = 0; i < 100; i++)
            {
                detailsTickets.Add(detailsTickets[0]);
            }

            return builder.BuildColossusBetDetails(detailsTickets, ticketHelper);
        }

        public ViewMemberChoiceResult MemberChoice(GetColossusBetsBetDetailsCriteria criteria)
        {
            var token = GetToken(criteria);
            var siteId = GetSiteId_ColossusBets();
            var iframe = string.Format(
                AppSettings.ColossusBetsAPI_MemberChoice,
                criteria.EventId,
                token != null ? token.Data.Authorization : string.Empty,
                siteId);
            var result = new ViewMemberChoiceResult
            {
                Iframe = iframe
            };

            result.ActionInfo = token != null ?
               new ResultDetail
               {
                   ErrCode = token.ErrorCode,
                   ErrMsg = token.ErrorMsg
               } :
               new ResultDetail
               {
                   ErrCode = 1,
                   ErrMsg = BetListResources.ColossusBetsAPI_FailedToConnect
               };

            return result;
        }

        private ViewTokenResult GetToken(GetColossusBetsBetDetailsCriteria criteria)
        {
            try
            {
                var getTokenAPI = string.Format(
                    AppSettings.ColossusBetsAPI_GetToken,
                    AppSession.AccId,
                    criteria.CustId);
                WebClient client = new WebClient();
                var result = JsonConvert.DeserializeObject<ViewTokenResult>(client.DownloadString(new Uri(getTokenAPI)));
                return result;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }

            return null;
        }

        private int GetSiteId_ColossusBets()
        {
            return 2100100;
        }
    }
}
