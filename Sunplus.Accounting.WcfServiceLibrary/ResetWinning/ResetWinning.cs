// -----------------------------------------------------------------------
// <copyright file="ResetWinning.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.WcfServiceLibrary.ResetWinning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sunplus.Accounting.Entities.Report.ResetWinning;
    using Sunplus.Accounting.Business.BootStrappers;
    using System.ServiceModel.Activation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ResetWinning: IResetWinning
    {

        #region IResetWinning Members

        public ResetWinningDTO GetDataResetWinning(string date, int data)
        {
            var result = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.ResetWinning.IResetWinningManager>();
            return result.getDataReportResetWinning(date, data);
        }

        #endregion
    }
}
