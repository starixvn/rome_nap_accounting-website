namespace Fanex.Tarus.BetList.Business.Results
{
    using System;

    public interface IVS2RacingResultService
    {
        string GetResult(int matchId, int sportType, DateTime eventDate);
    }
}
