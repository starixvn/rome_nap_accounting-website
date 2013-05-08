



using System.Data;
using Fanex.Data;
using Fanex.Tarus.VirtualSport.Entities.SpecialDelete;

namespace Fanex.Tarus.VirtualSport.DataAccess.SpecialDelete
{
    public class MessengerAccess : IMessengerAccess
    {
        public DataSet SetMessage(Message msg, int matchId)
        {
            DataSet result = null;
            using (ObjectDb objDB = new ObjectDb("Dot_Msg_SetMessage"))
            {
                //objDB.AddInParameter("ispublic", DbType.Int64, 0);
                objDB.AddInParameter("msgtype", DbType.Int64, 1);
                objDB.AddInParameter("issticky", DbType.Int64, 0);
                objDB.AddInParameter("isbatch", DbType.Int64, 1);
                objDB.AddInParameter("istest", DbType.Int64, 0);
                objDB.AddInParameter("creator", DbType.Int64, msg.CreatorId);
                objDB.AddInParameter("creatorname", DbType.String, msg.CreatorName);
                objDB.AddInParameter("content_en", DbType.String, msg.Content_en);
                objDB.AddInParameter("content_ch", DbType.String, msg.Content_ch);
                objDB.AddInParameter("content_cs", DbType.String, msg.Content_cs);
                objDB.AddInParameter("content_it", DbType.String, msg.Content_it);
                objDB.AddInParameter("content_jp", DbType.String, msg.Content_jp);
                objDB.AddInParameter("content_th", DbType.String, msg.Content_th);
                objDB.AddInParameter("content_ko", DbType.String, msg.Content_ko);
                objDB.AddInParameter("content_es", DbType.String, msg.Content_es);
                result = objDB.ExecuteDataSet();
            }
            return result;
        }

        public void AddTargt2Message(int messageId, int targetId, string targetName, string transId, int creatorId)
        {
            using (ObjectDb objDB = new ObjectDb("Dot_Msg_SetUsrMessage"))
            {
                objDB.AddInParameter("messageid", DbType.Int64, messageId);
                objDB.AddInParameter("targetid", DbType.Int64, targetId);
                objDB.AddInParameter("targetname", DbType.String, targetName);
                objDB.AddInParameter("isread", DbType.Int64, 0);
                objDB.AddInParameter("bettransid", DbType.String, transId);
                //objDB.AddInParameter("messagetype", DbType.Int64, 1);
                objDB.AddInParameter("creator", DbType.Int64, creatorId);
                objDB.ExecuteDataSet();
            }
        }
    }
}
