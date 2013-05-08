using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Balance.Business
{
    using Fanex.Tarus.Balance.Entities;

    public interface IResetWinningService
    {
        ResetWinningDTO GetDataReportResetWinning(string date, int data);
    }
}
