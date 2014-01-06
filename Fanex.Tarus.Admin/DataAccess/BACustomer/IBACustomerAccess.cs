// -----------------------------------------------------------------------
// <copyright file="IBACustomerAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.DataAccess.BACustomer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.Tarus.Admin.Entities;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBACustomerAccess
    {
        DataTable GetLicensee();

        DataTable GetBACustomerList(int LicenseeId, int Status, string Username);

        Result InsBACustomerList(string strCustId, string Rate, int userId, string IP, string follower);
     
    }

}
