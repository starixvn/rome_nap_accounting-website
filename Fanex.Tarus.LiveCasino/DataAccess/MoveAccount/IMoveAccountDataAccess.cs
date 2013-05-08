using Fanex.Tarus.LiveCasino.Entities;
using Fanex.Tarus.LiveCasino.Entities.MoveAccount;

namespace Fanex.Tarus.LiveCasino.DataAccess.MoveAccount
{
    
    public interface IMoveAccountDataAccess
    {
        Result MoveMaster(MoveAccountInfo moveAccountInfo);

        Result MoveAgent(MoveAccountInfo moveAccountInfo);

        Result RollbackMoveMaster(MoveAccountInfo moveAccountInfo);

        Result RollbackMoveAgent(MoveAccountInfo moveAccountInfo);

        void WriteLog(LogMoveAccountInfo logData);

        Result CheckMoveMaster(MoveAccountInfo moveInfo);

        Result CheckMoveAgent(MoveAccountInfo moveInfo);
    }
}
