// -----------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.SendEmail.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.Tarus.SpecialDelete.SendEmail.DataAccess;
    using Fanex.Tarus.SpecialDelete.SendEmail.Entity;
    using Fanex.Tarus.Infrastructure.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageService : IMessageService
    {
        IMessageDataAccess _IMessageDataAccess;

        public MessageService(): this(null){ }

        public MessageService(IMessageDataAccess iMessageDataAccess)
        {
            _IMessageDataAccess = iMessageDataAccess ?? new MessageDataAccess();
        }

        string CorrectSpace(string scontent)
        {
            if (scontent == null)
                return string.Empty;
            if (scontent.Length == 0)
                return scontent;
            int index = scontent.IndexOf("@@");

            if (index == -1) return scontent;

            if (index == 0)
            {
                if (scontent.Substring(0, 1).Trim().Length > 0)
                    scontent = scontent.Replace("@@", "@@ ");
            }
            else
            {
                if (scontent.Substring(index - 1, 1).Trim().Length > 0)
                    scontent = scontent.Replace("@@", " @@");


            }
            return scontent;
        }
       
        public int SetMessage(Message Msg, int[] TargetId, string[] TargetName, int MatchId, string[] TransId)
        {

            try
            {
                // for insert check
                if ((Msg.MessageId == 0) && (TargetId.Length < 1))
                    return 0;

                Msg.Content_en = CorrectSpace(Msg.Content_en);
                Msg.Content_ch = CorrectSpace(Msg.Content_ch);
                Msg.Content_cs = CorrectSpace(Msg.Content_cs);
                Msg.Content_it = CorrectSpace(Msg.Content_it);
                Msg.Content_jp = CorrectSpace(Msg.Content_jp);
                Msg.Content_th = CorrectSpace(Msg.Content_th);
                Msg.Content_ko = CorrectSpace(Msg.Content_ko);
                Msg.Content_es = CorrectSpace(Msg.Content_es);

                Msg.MessageId = _IMessageDataAccess.SetMessage(Msg, TargetId, TargetName, MatchId, TransId);

                if (Msg.MessageId > 0)
                {
                    _IMessageDataAccess.AddTargt2Message(Msg.MessageId, TargetId, TargetName, TransId);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return Msg.MessageId;
        }


        public void AddTargt2Message(int MessageId, int[] TargetId, string[] TargetName, string[] TransId) 
        {
            _IMessageDataAccess.AddTargt2Message(MessageId, TargetId, TargetName, TransId);
        }
    }
}
