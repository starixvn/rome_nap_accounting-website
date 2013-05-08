// -----------------------------------------------------------------------
// <copyright file="ResetWinning.cs" company="">
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
    using DNAide.Core;
    using Sunplus.Accounting.DataAccess.Report.ResetWinning;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResetWinningManager:IResetWinningManager
    {
        #region IResetWinning Members
        private static IResetWinningDAO _resetWinningDAO;
        public ResetWinningManager(IResetWinningDAO resetWinningDAO)
        {
            _resetWinningDAO = resetWinningDAO;
        }
        public ResetWinningDTO getDataReportResetWinning(string date, int data)
        {
            var list = new List<ResetWinning>();
            var result = new ResetWinningDTO();
            result = _resetWinningDAO.GetDataResetWinning(date, data);
            return result;
            
        }

        #endregion
    }
}
