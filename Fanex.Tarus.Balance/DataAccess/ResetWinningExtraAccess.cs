namespace Fanex.Tarus.Balance.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DNAide.Core;
    using DNAide.Utils;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Balance.Criteria;
    using Fanex.Tarus.Balance.Entities;

    public class ResetWinningExtraAccess : IResetWinningAccess
    {
        public Dictionary<string, DateTime> SelBalanceUpMovebetDays()
        {
            var datatable = new DataTable();
            var balmoveday = new Dictionary<string, DateTime>();
            using (ObjectDb objDB = new ObjectDb("Core_ReportService_GetBalanceUpDay"))
            {
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
                balmoveday.Add("BalanceUpDay", CastAide.AsDateTime(datatable.Rows[0]["BalanceUpDay"]));
                balmoveday.Add("MovebetsDay", CastAide.AsDateTime(datatable.Rows[0]["MovebetsDay"]));
            }

            return balmoveday;
        }
        
        public ResetWinning_New GetResetWinningDataAfterMoveBetDay(GetResetWinningCriteria criteria)
        {
            var result = new ResetWinning_New();
            Hashtable outParams = new Hashtable();
            DataTable resetWinningTable = new DataTable();
            decimal winlossTotal = 0, resetTotal = 0, winLoss = 0, reset = 0, variance = 0, varianceTotal = 0;

            Hashtable _params = new Hashtable();
            _params.Add("fromdate", criteria.FromDate);
            _params.Add("todate", criteria.ToDate);
            _params.Add("mode", criteria.ViewOrResetStatus);
            _params.Add("creator", criteria.Creator);
            _params.Add("ip", criteria.IP);

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKeys()))
            {
                var resetWinningDataSet = objDb.ExecuteDataSet(_params);
                resetWinningTable = resetWinningDataSet.Tables[0];
            }

            foreach (DataRow row in resetWinningTable.Rows)
            {
                winlossTotal += winLoss = CastAide.AsDecimal(row["winlost"]);
                resetTotal += reset = CastAide.AsDecimal(row["reset"]);
                varianceTotal += variance = winLoss + reset;

                var resetWinning = new ResetWinning_New
                {
                    WinLossDate = Convert.ToDateTime(row["winlostdate"]).ToShortDateString(),
                    Product = CastAide.AsString(row["groupname"]),
                    WinLoss = winLoss,
                    Reset = reset,
                    Variance = variance
                };

                result.ReportRows.Add(resetWinning);
            }

            result.Reset = resetTotal;
            result.Reset_Str = NumberFormat.DecFormat(resetTotal, 2);
            result.WinLoss = winlossTotal;
            result.Variance = varianceTotal;

            return result;
        }

        public ResetWinning_New GetResetWinningByDay(ResetWinning_New resetWinning)
        {
            var result = new ResetWinning_New();

            var resetWinningByDayGroup = resetWinning.ReportRows.GroupBy(x => x.WinLossDate).ToList();

            foreach (var group in resetWinningByDayGroup)
            {
                var resetWinningByDay = new ResetWinning_New();
                var numberReset = group.Count();
                var listResetWinningByDayItems = group.ToList();
                decimal winlossTotal = 0, varianceTotal = 0, resetTotal = 0;

                for (int i = 0; i < numberReset; i++)
                {
                    var currentResetWinningDay = listResetWinningByDayItems[i];
                    winlossTotal += currentResetWinningDay.WinLoss;
                    varianceTotal += currentResetWinningDay.WinLoss + currentResetWinningDay.Reset;
                    resetTotal += currentResetWinningDay.Reset;
                    var resetWinningByProduct = new ResetWinning_New
                    {
                        Product = currentResetWinningDay.Product,
                        WinLoss = currentResetWinningDay.WinLoss,
                        Reset = currentResetWinningDay.Reset,
                        Reset_Str = currentResetWinningDay.Reset == 0 ? "-" : NumberFormat.DecFormat(currentResetWinningDay.Reset, 2),
                        Variance = currentResetWinningDay.Variance
                    };

                    resetWinningByDay.ReportRows.Add(resetWinningByProduct);
                }

                resetWinningByDay.Variance = varianceTotal;
                resetWinningByDay.WinLossDate = Convert.ToDateTime(group.Key).ToShortDateString();
                resetWinningByDay.Reset_Str = NumberFormat.DecFormat(resetTotal, 2);
                resetWinningByDay.WinLoss = winlossTotal;

                result.ReportRows.Add(resetWinningByDay);
            }

            return result;
        }

        public ResetWinningBeforeMoveBetDay GetResetWinningDataBeforeMoveBetDay(GetResetWinningBeforeMoveBetDayCriteria criteria)
        {
            var result = new ResetWinningBeforeMoveBetDay();
            DataTable resetWinningTable = new DataTable();
            Hashtable outParams = new Hashtable();
            decimal winloss = 0, reset = 0, variance = 0, winlossTotal = 0, resetTotal = 0;

            Hashtable _params = new Hashtable();
            _params.Add("winlostdate", criteria.WinLossDate);

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKeys()))
            {
                var resetWinningDataSet = objDb.ExecuteDataSet(_params);
                resetWinningTable = resetWinningDataSet.Tables[0];
                outParams = objDb.GetOuputParams();
            }

            foreach (DataRow row in resetWinningTable.Rows)
            {
                string product = CastAide.AsString(row["groupname"]);
                winlossTotal += winloss = CastAide.AsDecimal(row["winlost"]);
                resetTotal += reset = CastAide.AsDecimal(row["reset"]);                
                variance = winloss + reset;

               var resetWinning = new ResetWinningBeforeMoveBetDay
                {
                    Product = product,
                    WinLoss = NumberFormat.DecFormat(winloss, 2),
                    Reset = reset == 0 ? "-" : NumberFormat.DecFormat(reset, 2),
                    Variance = NumberFormat.DecFormat(variance, 2)
                };

                result.ReportRows.Add(resetWinning);
            }
            
            result.Variance = NumberFormat.DecFormat(variance, 2);
            result.WinLoss = NumberFormat.DecFormat(winlossTotal, 2);
            result.Reset = NumberFormat.DecFormat(resetTotal, 2);
                        
            return result;
        }

        public ResetWinningByDay ResetWinningByDay(GetResetWinningCriteria criteria)
        {
            ResetWinningByDay resetWinningByDay = new ResetWinningByDay();

            DataSet resetWinningDataSet = new DataSet();
            DataTable resetWinningTable = new DataTable();
            Hashtable outputParams = new Hashtable();
            Hashtable _params = new Hashtable();
            _params.Add("fromdate", criteria.FromDate);
            _params.Add("todate", criteria.ToDate);
            _params.Add("mode", criteria.ViewOrResetStatus);
            _params.Add("creator", criteria.Creator);
            _params.Add("ip", criteria.IP);

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKeys()))
            {
                resetWinningDataSet = objDb.ExecuteDataSet(_params);
                outputParams = objDb.GetOuputParams();
            }

            int errorCode = Converter.ToInt(outputParams["errcode"]);
            resetWinningByDay.ResetStatus.ErrorCode = errorCode;
            resetWinningByDay.ResetStatus.ErrorMessage = Converter.ToString2(outputParams["errmsg"]);

            if (errorCode == 0)
            {
                resetWinningTable = resetWinningDataSet.Tables[0];
                resetWinningByDay.ResetWinning = this.GetResetWiningDataAfterReset(resetWinningTable);
            }

            return resetWinningByDay;
        }

        private ResetWinning_New GetResetWiningDataAfterReset(DataTable resetWinningTable)
        {
            var result = new ResetWinning_New();
            decimal resetTotal = 0, reset = 0, winlossTotal = 0, latestReset = 0, latestResetTotal = 0, variance = 0, varianceTotal = 0, winLoss = 0;

            foreach (DataRow row in resetWinningTable.Rows)
            {
                winlossTotal += winLoss = CastAide.AsDecimal(row["winlost"]);
                resetTotal += reset = CastAide.AsDecimal(row["totalreset"]);
                latestResetTotal += latestReset = CastAide.AsDecimal(row["latestreset"]);
                varianceTotal += variance = winLoss + reset;

                var resetWinning = new ResetWinning_New
                {
                    WinLossDate = Convert.ToDateTime(row["winlostdate"]).ToShortDateString(),
                    Product = CastAide.AsString(row["groupname"]),
                    WinLoss = winLoss,
                    Reset = reset,
                    LatestReset = latestReset,
                    Variance = variance
                };

                result.ReportRows.Add(resetWinning);
            }

            result.Reset_Str = NumberFormat.DecFormat(resetTotal, 2);
            result.LatestReset_Str = NumberFormat.DecFormat(latestResetTotal, 2);
            result.Variance = varianceTotal;
            result.WinLoss = winlossTotal;

            return result;
        }

        public ResetWinningDTO GetDataResetWinning(string date, int data)
        {
            throw new NotImplementedException();
        }
    }
}
