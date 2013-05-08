// -----------------------------------------------------------------------
// <copyright file="IResetWinning.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Business.Report.ResetWinning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Sunplus.Accounting.Entities.Report.ResetWinning;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IResetWinningManager
    {
       ResetWinningDTO getDataReportResetWinning(string date, int data);

    }
}
