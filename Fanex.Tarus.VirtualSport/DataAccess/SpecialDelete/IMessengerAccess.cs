

using System.Data;
using Fanex.Tarus.VirtualSport.Entities.SpecialDelete;
namespace Fanex.Tarus.VirtualSport.DataAccess.SpecialDelete
{
    public interface IMessengerAccess
    {
        DataSet SetMessage(Message msg, int matchId);
        void AddTargt2Message(int messageId, int targetId, string targetName, string transId, int creatorId);
    }
}
