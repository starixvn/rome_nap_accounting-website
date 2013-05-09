namespace Fanex.Tarus.BetList.Business.NumberGame
{
    using System.Collections.Generic;

    using Fanex.Tarus.BetList.Entities.NumberGame;
    using Fanex.Tarus.Entities;

    public interface INumberWheelService
    {

        List<NumberWheelChoice> GetChoiceList();

        ResultDetail InsertNewChoice(NumberWheelChoice newNumberWheelChoice);

        ResultDetail UpdateChoice(int hdp1, int betTeam);

        string GetChoiceName(int hdp1, int betTeam);

    }
}
