// -----------------------------------------------------------------------
// <copyright file="IEnableService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.VirtualSport.Business.Customer.EnableVirtualSports
{
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.Tarus.VirtualSport.Entities.Customer.EnableVirtualSports;
    using System.Collections.Generic;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IEnableService
    {
        CustomerSettings CustomerSelStatusVS(string userName, Result result);

        List<Result> Acc_CustomerUpdStatusVS(Entities.Customer.EnableVirtualSports.CustomerSettings settings);
    }
}
