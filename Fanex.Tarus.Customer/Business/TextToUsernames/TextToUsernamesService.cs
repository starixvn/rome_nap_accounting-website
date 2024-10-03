namespace Fanex.Tarus.Customer.Business.ManageSuperSettings
{
    using Fanex.Tarus.Customer.Criteria.TextToUsernames;
    using Fanex.Tarus.Customer.DataAccess.ManageSuperSettings;
    using Fanex.Tarus.Customer.Requests.TextToUsernames;
    using Fanex.Tarus.Customer.ViewModels.TextToUsernames;
    using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

    public interface ITextToUsernamesService
    {
        GetUsernamesViewModel GetUsernames(GetUsernamesRequest request);
    }

    public class TextToUsernamesService : ITextToUsernamesService
    {
        private readonly ITextToUsernamesDataAccess dataAccess;

        public TextToUsernamesService()
        {
            dataAccess = new TextToUsernamesDataAccess();
        }

        public GetUsernamesViewModel GetUsernames(GetUsernamesRequest request)
        {
            var criteria = new GetUsernamesCriteria(request.Text, request.UserId, AccountCredit.Acc_DeniedSiteCurrency_Sel(request.UserId));

            return dataAccess.GetUsernames(criteria);
        }
    }
}