using System.ServiceModel.Activation;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.WcfServiceLibrary.BetList
{
    /// <summary>
    /// Summary description for DailyTransManager
    /// </summary>

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BetListNumberGameManager : IBetListNumberGameManager
    {
        #region IBetListNumberGameManager Members
        public NumbergameResult GetBetListNumberGame(int matchId)
        {
            var betlistNumberGame = new Sunplus.Accounting.Business.BetList.BetListNumberGameManager();
            return betlistNumberGame.GetBetListNumberGame(matchId);
        }

        #endregion
    }
}



