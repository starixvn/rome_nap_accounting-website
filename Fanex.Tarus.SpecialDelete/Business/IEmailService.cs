// -----------------------------------------------------------------------
// <copyright file="IEmailService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.DNAide;
    using Fanex.Tarus.SpecialDelete.Entity;
using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IEmailService
    {
        string SendEmailWhenDeleteBetFromBetListToCSD(int errcode, string errmsg, DataSet rs, bool IsNumberGame, string reason);

        string SendEmailWhenDeleteBetFromBetListToInternalGroup(int errcode, string errmsg, DataSet rs, bool IsNumberGame, bool IsLiveCasino, bool IsRealCurrency, string reason);
            
        bool SendMailWhenDeleteBetToDBA(string ErrMsg, string ErrCode, string MatchID, string FromRefNo, string ToRefNo);

        void SendEmailWhenDeleteBetToInternalGroup(ResultModel result, DataTable dtMatch, DataTable dtUsers, DataTable dtChoice, bool IsNumberGame, bool IsLiveCasino, bool IsRealCurrency, string reason);

        void SendEmailWhenDeleteBetToCSD(ResultModel result, DataTable dtMatch, DataTable dtUsers, DataTable dtChoice, bool IsNumberGame, bool IsLiveCasino, string reason);

        bool SendMailWhenRecoverBetToDBA(string ErrMsg, string ErrCode, string TransID);

        void SendEmailWhenRecoverBetToInternalGroup(ResultModel result, DbResult rs, bool IsNumberGame, bool IsLiveCasino, bool IsRealCurrency, string transid, bool IsNegativeBalance);

        void SendEmailWhenRecoverBetToCSD(ResultModel result, DbResult rs, bool IsNumberGame, bool IsLiveCasino, string transid, bool IsNegativeBalance);

        string SendEmailWhenDeleteMixParlayBetFromBetListToCSD(int errcode, string errmsg, DataSet rs, string reason);

        string SendEmailWhenDeleteMixParlayBetFromBetListToInternalGroup(int errcode, string errmsg, DataSet rs, bool IsRealCurrency, string reason);

        string SendEmailToCSDEarlyKO(DataSet rs, string transid, string reason);

        string SendEmailWhenDeleteBetToInternalGroupEarlyKO(DataSet rs, string transid, bool IsRealCurrency, string reason);

        string SendEmailWhenDeleteVirtualSportToCSD(int errcode, string errmsg, DataSet rs);

        string SendEmailWhenDeleteVirtualSportToInternalGroup(int errcode, string errmsg, DataSet rs, bool isRealCurrency);
    }
}
