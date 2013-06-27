// -----------------------------------------------------------------------
// <copyright file="IMessageService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.SendEmail.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.SpecialDelete.SendEmail.Entity;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMessageService
    {
        int SetMessage(Message Msg, int[] TargetId, string[] TargetName, int MatchId, string[] TransId);

        void AddTargt2Message(int MessageId, int[] TargetId, string[] TargetName, string[] TransID);
    }
}
