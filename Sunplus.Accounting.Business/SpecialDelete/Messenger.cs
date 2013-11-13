using System;
using System.Collections;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;
using Fanex.Data;

/// <summary>
/// Summary description for Messenger
/// </summary>

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public sealed class Messenger
    {
        /// <summary>
        /// Insert message into table Msg, with IsPublic = 0 (private), MsgType = 1 (message to customer), IsSticky = 0 (default no announcement)
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="TargetId"></param>
        /// <param name="TargetName"></param>
        /// <returns></returns>
        public static int SetMessage(Message Msg, int[] TargetId, string[] TargetName, int MatchId, string[] TransId)
        {
            DbResult rs = null;
            try
            {
                // for insert check
                if ((Msg.MessageId == 0) && (TargetId.Length < 1))
                    return 0;

                var p = new Hashtable();

                Msg.Content_en = CorrectSpace(Msg.Content_en);
                Msg.Content_ch = CorrectSpace(Msg.Content_ch);
                Msg.Content_cs = CorrectSpace(Msg.Content_cs);
                Msg.Content_it = CorrectSpace(Msg.Content_it);
                Msg.Content_jp = CorrectSpace(Msg.Content_jp);
                Msg.Content_th = CorrectSpace(Msg.Content_th);
                Msg.Content_ko = CorrectSpace(Msg.Content_ko);
                Msg.Content_es = CorrectSpace(Msg.Content_es);


                p.Add("ispublic", 0);
                p.Add("msgtype", 1);
                p.Add("issticky", 0);
                p.Add("creator", Msg.CreatorId);
                p.Add("creatorname", Msg.CreatorName);
                p.Add("matchid", MatchId);
                p.Add("isbatch", 1);
                p.Add("istest", 0);

                p.Add("content_en", Msg.Content_en);
                p.Add("content_ch", Msg.Content_ch);
                p.Add("content_cs", Msg.Content_cs);
                p.Add("content_it", Msg.Content_it);
                p.Add("content_jp", Msg.Content_jp);
                p.Add("content_th", Msg.Content_th);
                p.Add("content_ko", Msg.Content_ko);
                p.Add("content_es", Msg.Content_es);

                using (ObjectDb db = new ObjectDb("Dot_Msg_SetMessage"))
                {
                    db.AssignParamValues(p);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    rs = new DbResult(output, ds);
                }

                Msg.MessageId = CastAide.AsInt32(rs.ResultSet.Tables[0].Rows[0]["MessageId"]);
                if (Msg.MessageId > 0)
                {
                    AddTargt2Message(Msg.MessageId, TargetId, TargetName, TransId);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return Msg.MessageId;
        }

        /// <summary>
        /// Insert target information of the message inserted into table MsgTarget with TargetID = customerID (member,agent,master,super), TargetName = customer's username, MessageType = 1
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="TargetId"></param>
        /// <param name="TargetName"></param>
        /// <returns></returns>
        public static void AddTargt2Message(int MessageId, int[] TargetId, string[] TargetName, string[] TransID)
        {
            try
            {
                for (int i = 0; i < TargetId.Length; i++)
                {
                    var p = new Hashtable();
                    p.Add("messageid", MessageId);
                    p.Add("targetid", TargetId[i]);
                    p.Add("targetname", TargetName[i]);
                    p.Add("isread", 0);
                    p.Add("bettransid", TransID[i]);
                    //p.Add("messagetype", 1);
                    // set user or role via SP
                    if (TargetId[i] != 0)
                    {
                        using (ObjectDb db = new ObjectDb("Dot_Msg_SetUsrMessage"))
                        {
                            db.AssignParamValues(p);
                            db.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static string CorrectSpace(string scontent)
        {
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
    }
}