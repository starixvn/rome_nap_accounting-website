using System.Collections.Generic;
using Fanex.Tarus.SpecialDelete.SendEmail.Entity.LiveCasino;
using Fanex.Tarus.SpecialDelete.SendEmail.Entity;
using Fanex.Tarus.SpecialDelete.SendEmail.Core;

namespace Fanex.Tarus.SpecialDelete.SendEmail.Business.LiveCasino
{
    public interface ILiveCasinoService
    {
        Tickets GetTickets(string matchid, string f_transid, string t_transid);

        ResultModel DeleteBets(string matchid, string f_transid, string t_transid, string IP,
           string transids_n,
           string transids_d,
           string transids_bd,
           string transids_12eu,
           string transids_mhc,
            bool isSendPrivateMSG,
           Message message);

        Tickets GetTicketsForRecoverBet(string transid, out string ErrMsg, out int ErrCode);

        ResultModel RecoverBet(string transid, string userid, string username, string IP);

        string CheckMatchIDwithRefno(string matchid, string f_transid, string t_transid, Fanex.Tarus.SpecialDelete.SendEmail.Core.Sport.SportType sportType);

        /// <summary>
        /// SelNegativeBalanceCustomer
        /// </summary>
        /// <param name="transid"></param>
        /// <param name="actionType"></param>
        /// <param name="excludeRunning">excludeRunning = true when deleting bets of LCS </param>
        /// <returns></returns>
        List<NegativeBalance> SelNegativeBalanceCustomer(string transid, Function.ActionType actionType, bool excludeRunning);

        ResultModel CheckParamInput(string param, bool IsAllowNullOrEmpty);
    }
}