namespace Fanex.Tarus.VirtualSport.Business.SpecialDelete
{
    using Fanex.Tarus.VirtualSport.Entities;
    using Fanex.Tarus.VirtualSport.Entities.SpecialDelete;

    public interface IRecoverBetsService
    {
        SpecialDeleteBetInfoList SelRecoverBets(string transid);
        Result RecoverBets(string transid);
    }
}
