using System.Collections.Generic;
using System.ServiceModel.Activation;
using Fanex.Tarus.SpecialDelete.Business;
using Fanex.Tarus.SpecialDelete.Entity.LiveCasino;
using Fanex.Tarus.SpecialDelete.Entity;
using Fanex.Tarus.SpecialDelete.Business.LiveCasino;


namespace Fanex.Tarus.SpecialDelete.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SpecialDeleteService : ISpecialDeleteService
    {
        //public Tickets GetTickets(int MatchID, decimal FromRefNo, decimal ToRefNo)
        //{
        //    return (new LiveCasinoService()).GetTickets(MatchID, FromRefNo, ToRefNo);
        //}

        //public string DeleteBets(int MatchID, string TransIds, decimal FromRefNo, decimal ToRefNo, decimal FromTransID, decimal ToTransID, string UserName, string UserPwd, int UserID, Message message)
        //{
        //    return string.Empty;
        //   // return (new LiveCasinoManager()).DeleteBets(MatchID, TransIds, FromRefNo, ToRefNo, FromTransID, ToTransID, UserName, UserPwd, UserID, Sunplus.Accounting.Core.Helper.Utils.GetClientIP(), message);
        //}

        //public Tickets GetTicketsForRecoverBet(decimal TransID)
        //{
        //    return (new LiveCasinoService()).GetTicketsForRecoverBet(TransID);
        //}

        //public string RecoverBet(decimal TransId, int UserId, string UserName)
        //{
        //    //return (new LiveCasinoManager()).RecoverBet(TransId, UserId, UserName, Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
        //    return string.Empty;
        //}
    }
}
