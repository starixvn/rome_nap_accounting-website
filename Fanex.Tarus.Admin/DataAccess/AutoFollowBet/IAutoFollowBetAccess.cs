
using System.Data;
using Fanex.Tarus.Admin.Entities;
namespace Fanex.Tarus.Admin.DataAccess.AutoFollowBet
{
    public interface IAutoFollowBetAccess
    {
        DataTable AutoFollowBetSelCustomer(int status);
        Result AutoFollowBetDelCustomer(string username);
        Result AutoFollowBetInsCustomer(string username, double rate);
        Result AutoFollowBetUpdCustomer(string username, double rate); 
    }
}
