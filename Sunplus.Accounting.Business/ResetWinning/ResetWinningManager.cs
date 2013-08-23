// -----------------------------------------------------------------------
// <copyright file="ResetWinning.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Business.Report.ResetWinning
{
    using System.Collections.Generic;
    using Sunplus.Accounting.DataAccess.Report.ResetWinning;
    using Sunplus.Accounting.Entities.Report.ResetWinning;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResetWinningManager : IResetWinningManager
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

        #endregion IResetWinning Members
    }
}