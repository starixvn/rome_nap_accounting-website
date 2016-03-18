namespace Fanex.Tarus.Balance.Business
{
    using System;
    using System.Threading;
    using DNAide.Utils;
    using Fanex.Tarus.Balance.Criteria;
    using Fanex.Tarus.Balance.DataAccess;
    using Fanex.Tarus.Balance.Entities;
    using Fanex.Tarus.Security.Permission;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;

    public class ResetWinningExtraService : IResetWinningService
    {
        private IResetWinningAccess _resetWinningAccess;

        public ResetWinningExtraService(IResetWinningAccess resetWinningAccess)
        {
            _resetWinningAccess = resetWinningAccess ?? new ResetWinningExtraAccess();
        }

        public ResetWinningExtraService()
            : this(null)
        {
        }

        public bool CheckPermissionToReset()
        {
            var permissionService = new PermissionService();
            var pageAccess = permissionService.GetPageAccess(AppSession.AccId);
            var pathActionName = "ResetWinningExtra/Index";
            bool hasPermission = permissionService.GetHostPermission(pathActionName, pageAccess);

            return hasPermission;
        }

        public ResetWinningDTO GetDataReportResetWinning(string date, int data)
        {
            var result = new ResetWinningDTO();

            try
            {
                result = this._resetWinningAccess.GetDataResetWinning(date, data);
                return result;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }

            return result;
        }

        public ResetWinning_New GetResetWinningDataAfterMoveBetDay(GetResetWinningCriteria criteria)
        {
            criteria.FromDate = Convert.ToDateTime(criteria.FromDate).ToShortDateString();
            criteria.ToDate = Convert.ToDateTime(criteria.ToDate).ToShortDateString();
            criteria.Creator = AppSession.AccId;
            criteria.IP = Utils.GetClientIP();

            var result = new ResetWinning_New();

            try
            {
                var resetWinningData = this._resetWinningAccess.GetResetWinningDataAfterMoveBetDay(criteria);
                result = this._resetWinningAccess.GetResetWinningByDay(resetWinningData);
                return result;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }

            return result;
        }

        public ResetWinningBeforeMoveBetDay GetResetWinningDataBeforeMoveBetDay(GetResetWinningBeforeMoveBetDayCriteria criteria)
        {
            criteria.WinLossDate = Convert.ToDateTime(criteria.WinLossDate).ToShortDateString();
            var result = new ResetWinningBeforeMoveBetDay();

            try
            {
                result = this._resetWinningAccess.GetResetWinningDataBeforeMoveBetDay(criteria);
                return result;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }

            return result;
        }

        public ResetStatus ResetWinningByDay(GetResetWinningCriteria criteria)
        {
            ResetWinningByDay resetWinningByDay = new ResetWinningByDay();
            criteria.ViewOrResetStatus = 1; ////Reset
            criteria.Creator = AppSession.AccId;
            criteria.IP = Utils.GetClientIP();
            try
            {
                resetWinningByDay = this.GetResetWinningByDayAfterReset(criteria);

                if (resetWinningByDay.ResetStatus.ErrorCode == 0)
                {
                    Thread.Sleep(10000);
                    resetWinningByDay.ResetWinning.WinLossDate = criteria.FromDate;
                    resetWinningByDay.ResetWinning.UserReset = AppSession.Username;
                    bool isSendSuccess = Mailing.ResetWinningSuccess(resetWinningByDay.ResetWinning, criteria.IsExtraPT);

                    if (!isSendSuccess)
                    {
                        throw new Exception("Can't send email");
                    }
                }
                else
                {
                    Mailing.ResetWinningEmailNotification(criteria, resetWinningByDay.ResetStatus, criteria.IsExtraPT);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return resetWinningByDay.ResetStatus;
        }

        /// <summary>
        /// Gets the reset winning by day.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Reset winning data.</returns>
        public ResetWinning_New GetResetWinningByDay(GetResetWinningCriteria criteria)
        {
            criteria.Creator = AppSession.AccId;
            criteria.IP = Utils.GetClientIP();
            var result = new ResetWinning_New();
            try
            {
                ResetWinning_New resetWinningData = this._resetWinningAccess.GetResetWinningDataAfterMoveBetDay(criteria);
                result = this.BuidResetWinningData(resetWinningData);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the reset winning by day after reset.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Reset winning data to send email.</returns>
        public ResetWinningByDay GetResetWinningByDayAfterReset(GetResetWinningCriteria criteria)
        {
            var result = new ResetWinningByDay();

            try
            {
                var resetWinningByDay = _resetWinningAccess.ResetWinningByDay(criteria);
                result.ResetStatus = resetWinningByDay.ResetStatus;
                if (resetWinningByDay.ResetStatus.ErrorCode == 0)
                {
                    result.ResetWinning = this.BuidResetWinningData(resetWinningByDay.ResetWinning);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public ResetWinning_New BuidResetWinningData(ResetWinning_New resetWinningData)
        {
            var result = new ResetWinning_New();

            decimal winlossTotal = 0, varianceTotal = 0, resetTotal = 0, resetLatestTotal = 0, varianceRow = 0;
            int numberResetWinningRow = resetWinningData.ReportRows.Count;
            for (int i = 0; i < numberResetWinningRow; i++)
            {
                ResetWinning_New currentResetWinning = resetWinningData.ReportRows[i];
                winlossTotal += currentResetWinning.WinLoss;
                varianceTotal += varianceRow = currentResetWinning.WinLoss + currentResetWinning.Reset;
                resetTotal += currentResetWinning.Reset;
                resetLatestTotal += currentResetWinning.LatestReset;

                result.ReportRows.Add(new ResetWinning_New
                {
                    Product = currentResetWinning.Product,
                    WinLoss = Math.Round(currentResetWinning.WinLoss, 2),
                    Variance = Math.Round(varianceRow, 2),
                    Reset_Str = currentResetWinning.Reset == 0 ? "-" : NumberFormat.DecFormat(currentResetWinning.Reset, 2),
                    LatestReset_Str = currentResetWinning.LatestReset == 0 ? "-" : NumberFormat.DecFormat(currentResetWinning.LatestReset, 2)
                });
            }                       
            
            result.UserReset = AppSession.AccUserName;
            result.Variance = Math.Round(varianceTotal, 2);
            result.Reset_Str = NumberFormat.DecFormat(resetTotal, 2);
            result.WinLoss = winlossTotal;
            result.LatestReset_Str = NumberFormat.DecFormat(resetLatestTotal, 2);

            return result;
        }
    }
}