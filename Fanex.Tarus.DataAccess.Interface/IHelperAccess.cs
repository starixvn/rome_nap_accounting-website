using System.Data;
namespace Fanex.Tarus.DataAccess.Interface
{
    public interface IHelperAccess
    {
        DataTable BalanceDateInfo();

        bool CheckIsInternal(string userName);
    }
}
