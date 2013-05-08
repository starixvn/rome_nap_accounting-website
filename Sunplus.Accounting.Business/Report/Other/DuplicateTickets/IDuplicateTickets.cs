using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Accounting.Entities.Report.Others;

namespace Sunplus.Accounting.Business.Report.Other.DuplicateTickets
{
    public interface IDuplicateTickets
    {
        DuplicateTicketsDTO SelDuplicateTickets(string wldate);
    }
}
