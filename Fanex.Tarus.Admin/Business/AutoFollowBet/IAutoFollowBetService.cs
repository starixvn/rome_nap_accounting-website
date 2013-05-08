using Fanex.Tarus.Admin.Entities;
using System.Collections.Generic;
namespace Fanex.Tarus.Admin.Business.AutoFollowBet
{
    public interface IAutoFollowBetService
    {
        List<FollowedCustomer> SelFollowedCustomer(int status);
        Result DelFollowedCustomer(string username);
        Result InsFollowedCustomer(string username, double rate);
        Result UpdFollowedCustomer(string username, double rate);
    }
}
