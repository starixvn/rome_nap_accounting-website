// -----------------------------------------------------------------------
// <copyright file="INumberGameDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.SendEmail.DataAccess.NumberGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INumberGameDataAccess
    {
        DataSet GetTickets(string MatchID, string FromRefNo, string ToRefNo, string live, string dead);

        DataSet GetMatch(string MatchId);
       
        DataSet DeleteBets(string matchid, string f_transid, string t_transid, string dead, string live,

            string transids_n,
            string transids_d,
            string transids_bd,
            string transids_12eu,
            string transids_mhc,
                        
            string username, string password, string userid, string ip, string reason,
            out string errcode, out string errmsg

           );

        DataSet GetTicketsForRecoverBet(string TransID, out string ErrMsg, out int ErrCode);

        void RemoveMsgInBoDBMessage(string TransID, string UserID);

        DataSet RecoverBetAtIBCdb(string TransId, string UserId, string IP, out string ErrMsg, out int ErrCode, out bool IsNegativeBalance);
    }
}
