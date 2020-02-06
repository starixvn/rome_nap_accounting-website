namespace Fanex.Tarus.Customer.Business.CN88.MoveSuper
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Entities.CN88.MoveSuper;
    using Fanex.Tarus.Customer.Requests.CN88.MoveSuper;
    using Fanex.Tarus.Customer.ViewModels.CN88.MoveSuper;

    public interface IMoveSuperService
    {
        HeadlineUsernameSuggestion GetHeadlineUserNameSuggestion(string userName);

        MoveSuperViewModel SearchSuper(string userName);

        Result MoveSuper(MoveSuperRequest request);
    }
}
