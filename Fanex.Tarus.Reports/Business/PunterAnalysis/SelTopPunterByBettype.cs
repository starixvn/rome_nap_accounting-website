// -----------------------------------------------------------------------
// <copyright file="SelTopPunterByBettype.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.PunterAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using DNAide.Core;
    using Fanex.Tarus.Infrastructure.Extensions;
    using DNAide.Utils;
    using System.Globalization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SelTopPunterByBettypeProcessData
    {
       
        
        public List<DataTable> SelTopPunterByBettype_ProcessDataDateRange(DataTable dtData, int liveindicator)
        {
            List<DataTable> listData = new List<DataTable>();

            try
            {
                dtData.Columns.Add("HasData", typeof(int));

                List<DataRow> filterRows = new List<DataRow>();
                if (liveindicator == 2)
                {
                    filterRows = dtData.AsEnumerable().Select(s => s).ToList();
                }
                else
                {
                    filterRows = dtData.AsEnumerable().Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).Select(s => s).ToList();
                }


                DataTable dtSum = dtData.Clone();
                List<int> LeagueIds = filterRows.Select(s => CastAide.AsInt32(s["leagueid"])).Distinct().ToList();
                
                #region Assin Data For Sum                
                foreach (int leagueId in LeagueIds)
                {
                    var rowByLeagueId = filterRows.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).Select(s => s).ToList();
                    if (rowByLeagueId.Count == 0)
                        continue;

                    DataRow drNewRow = dtSum.NewRow();
                    drNewRow["leagueid"] = leagueId;
                    drNewRow["winloss"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                    drNewRow["turnover"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                    drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                        ? "0"
                        : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                    drNewRow["betcount"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                    drNewRow["HasData"] = 1;
                    drNewRow["rptYear"] = 1;

                    dtSum.Rows.Add(drNewRow);
                }
                #endregion

                // get data yearly
                List<int> years = filterRows.Select(s => CastAide.AsInt32(s["rptyear"])).Distinct().ToList();
                var YearsOrder = years.OrderBy(s => s);

                foreach (int year in YearsOrder)
                {
                    var rowsByYear = filterRows.Where(s => CastAide.AsInt32(s["rptyear"]) == year).ToList();

                    #region Assign Data For Year  
                    DataTable dtYear = dtData.Clone();
                    foreach (DataRow dr in dtSum.Select())
                    {
                        DataRow drNewRow = dtYear.NewRow();
                        int leagueId = CastAide.AsInt32(dr["leagueId"]);
                        var rowsByLeagueId = rowsByYear.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).ToList();

                        drNewRow["leagueid"] = leagueId;
                        drNewRow["rptyear"] = year;                        

                        if (rowsByLeagueId.Count == 0)
                        {
                            drNewRow["HasData"] = 0;
                        }
                        else
                        {
                            drNewRow["winloss"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                            drNewRow["turnover"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                            drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                                ? "0"
                                : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                            drNewRow["betcount"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                            drNewRow["HasData"] = 1;
                        }

                        dtYear.Rows.Add(drNewRow);
                    }
                    listData.Add(dtYear); 
                    #endregion

                    List<int> months = rowsByYear.Select(s => CastAide.AsInt32(s["rptMonth"])).Distinct().ToList();
                    var MonthsOrder = months.OrderBy(s => s);

                    foreach (int month in MonthsOrder)
                    {
                        DataTable dtMonth = dtData.Clone();
                        var rowsByMonth = rowsByYear.Where(s => CastAide.AsInt32(s["rptMonth"]) == month).ToList();

                        #region Assign Data For Month                        
                        foreach (DataRow dr in dtSum.Select())
                        {
                            DataRow drNewRow = dtMonth.NewRow();
                            int leagueId = CastAide.AsInt32(dr["leagueId"]);
                            var rowsByLeagueId = rowsByMonth.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).ToList(); 

                            drNewRow["leagueid"] = leagueId;
                            drNewRow["rptyear"] = year;
                            drNewRow["rptmonth"] = month; 

                            if (rowsByLeagueId.Count == 0)
                            {
                                drNewRow["HasData"] = 0;
                            }
                            else
                            {
                                drNewRow["winloss"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                                drNewRow["turnover"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                                drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                                drNewRow["betcount"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                                drNewRow["HasData"] = 1;
                            }

                            dtMonth.Rows.Add(drNewRow);
                        }
                        #endregion

                        listData.Add(dtMonth);
                    }
                    
                }

                // grand total
                listData.Add(dtSum);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listData;
        }

        public List<DataTable> SelTopPunterByBettype_ProcessDataWeekRange(DataTable dtData, int liveindicator)
        {
            List<DataTable> listData = new List<DataTable>();

            try
            { 
                dtData.Columns.Add("HasData", typeof(int));

                List<DataRow> filterRows = new List<DataRow>();
                if (liveindicator == 2)
                {
                    filterRows = dtData.AsEnumerable().Select(s => s).ToList();
                }
                else
                {
                    filterRows = dtData.AsEnumerable().Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).Select(s => s).ToList();
                }


                DataTable dtSum = dtData.Clone();
                List<int> LeagueIds = filterRows.Select(s => CastAide.AsInt32(s["leagueid"])).Distinct().ToList();

                #region Assin Data For Sum
                foreach (int leagueId in LeagueIds)
                {
                    var rowByLeagueId = filterRows.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).Select(s => s).ToList();
                    if (rowByLeagueId.Count == 0)
                        continue;

                    DataRow drNewRow = dtSum.NewRow();
                    drNewRow["leagueid"] = leagueId;
                    drNewRow["winloss"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                    drNewRow["turnover"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                    drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                        ? "0"
                        : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                    drNewRow["betcount"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                    drNewRow["HasData"] = 1;
                    drNewRow["rptYear"] = DateTime.Today.Year;

                    dtSum.Rows.Add(drNewRow);
                }
                #endregion

                // get data yearly
                List<int> years = filterRows.Select(s => CastAide.AsInt32(s["rptyear"])).Distinct().ToList();
                var YearsOrder = years.OrderBy(s => s);

                foreach (int year in YearsOrder)
                {
                    var rowsByYear = filterRows.Where(s => CastAide.AsInt32(s["rptyear"]) == year).ToList();

                    
                    List<int> weeks = rowsByYear.Select(s => CastAide.AsInt32(s["week"])).Distinct().ToList();
                    var WeeksOrder = weeks.OrderBy(s=>s);

                    #region get data by week
                    foreach (int week in WeeksOrder)
                    {
                        var rowsByWeek = rowsByYear.Where(s => CastAide.AsInt32(s["week"]) == week).ToList();
                        DataTable dtWeek = dtData.Clone();
                        foreach (DataRow dr in dtSum.Select())
                        {
                            DataRow drNewRow = dtWeek.NewRow();
                            int leagueId = CastAide.AsInt32(dr["leagueId"]);
                            var rowsByLeagueId = rowsByWeek.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).ToList(); 

                            drNewRow["leagueid"] = leagueId;
                            drNewRow["rptyear"] = year;
                            drNewRow["rptmonth"] = dr["rptmonth"];
                            drNewRow["week"] = week;

                            if (rowsByLeagueId.Count == 0)
                            {
                                drNewRow["HasData"] = 0;
                            }
                            else
                            {
                                drNewRow["winloss"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                                drNewRow["turnover"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                                drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                                drNewRow["betcount"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                                drNewRow["HasData"] = 1;
                            }

                            dtWeek.Rows.Add(drNewRow); 
                        }
                        listData.Add(dtWeek); 
                    }
                    #endregion
                }

                // grand total
                listData.Add(dtSum);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listData;
        }

        public List<DataTable> SelTopPunterByBettype_ProcessDataAnnual(DataTable dtData, int liveindicator)
        {
            List<DataTable> listData = new List<DataTable>();

            try
            {
                dtData.Columns.Add("HasData", typeof(int));

                List<DataRow> filterRows = new List<DataRow>();
                if (liveindicator == 2)
                {
                    filterRows = dtData.AsEnumerable().Select(s => s).ToList();
                }
                else
                {
                    filterRows = dtData.AsEnumerable().Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).Select(s => s).ToList();
                }

                //get Data Annual
                DataTable dtSum = dtData.Clone();
                List<int> LeagueIds = filterRows.Select(s => CastAide.AsInt32(s["leagueid"])).Distinct().ToList();
                int year = 0;
                foreach (int leagueId in LeagueIds)
                {
                    var rowByLeagueId = filterRows.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).Select(s => s).ToList();
                    if (rowByLeagueId.Count == 0)
                        continue;

                    DataRow drNewRow = dtSum.NewRow();
                    drNewRow["leagueid"] = leagueId;
                    drNewRow["winloss"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                    drNewRow["turnover"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                    drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                        ? "0"
                        : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                    drNewRow["betcount"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                    drNewRow["HasData"] = 1;
                    drNewRow["rptYear"] = rowByLeagueId[0]["rptYear"];
                    year = CastAide.AsInt32(rowByLeagueId[0]["rptYear"]);

                    dtSum.Rows.Add(drNewRow);
                }
                listData.Add(dtSum);

                // get data monthly
                List<int> months = filterRows.Select(s => CastAide.AsInt32(s["rptmonth"])).Distinct().ToList();
                var MonthsOrder = months.OrderBy(s => s);

                foreach (int month in MonthsOrder)
                {
                    var rowByMonth = filterRows.Where(s => CastAide.AsInt32(s["rptMonth"]) == month).ToList();
                    DataTable dtMonth = dtData.Clone();
                    foreach (DataRow dr in dtSum.Select())
                    {
                        DataRow drNewRow = dtMonth.NewRow();
                        int leagueId = CastAide.AsInt32(dr["leagueId"]);
                        var rowByLeagueId = rowByMonth.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).ToList();

                        drNewRow["leagueid"] = leagueId;
                        drNewRow["rptmonth"] = month;
                        drNewRow["rptyear"] = year;

                        if (rowByLeagueId.Count == 0)
                        {
                            drNewRow["HasData"] = 0;
                        }
                        else
                        {
                            drNewRow["winloss"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                            drNewRow["turnover"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                            drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                                ? "0"
                                : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                            drNewRow["betcount"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                            drNewRow["HasData"] = 1;
                            drNewRow["rptYear"] = rowByLeagueId[0]["rptYear"];
                        }

                        dtMonth.Rows.Add(drNewRow);
                    }

                    listData.Add(dtMonth);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listData;
        }

        public List<DataTable> SelTopPunterByBettype_ProcessDataMonthly(DataTable dtData, int liveindicator)
        {
            List<DataTable> listData = new List<DataTable>();

            try
            {
                dtData.Columns.Add("HasData", typeof(int));

                List<DataRow> filterRows = new List<DataRow>();
                if (liveindicator == 2)
                {
                    filterRows = dtData.AsEnumerable().Select(s => s).ToList();
                }
                else
                {
                    filterRows = dtData.AsEnumerable().Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).Select(s => s).ToList();
                }

                //get Data monthly
                DataTable dtSum = dtData.Clone();
                List<int> LeagueIds = filterRows.Select(s => CastAide.AsInt32(s["leagueid"])).Distinct().ToList();
                foreach (int leagueId in LeagueIds)
                {
                    var rowByLeagueId = filterRows.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).Select(s => s).ToList();
                    if (rowByLeagueId.Count == 0)
                        continue;

                    DataRow drNewRow = dtSum.NewRow();
                    drNewRow["leagueid"] = leagueId;
                    drNewRow["winloss"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                    drNewRow["turnover"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                    drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                        ? "0"
                        : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                    drNewRow["betcount"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                    drNewRow["HasData"] = 1;
                    drNewRow["rptMonth"] = rowByLeagueId[0]["rptMonth"];
                    drNewRow["rptYear"] = rowByLeagueId[0]["rptYear"];

                    dtSum.Rows.Add(drNewRow);
                }
                listData.Add(dtSum);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listData;
        }

        public List<DataTable> SelTopPunterByBettype_ProcessDataLongRange(DataTable dtData, int liveindicator)
        {
            List<DataTable> listData = new List<DataTable>();

            try
            {
                dtData.Columns.Add("HasData", typeof(int));

                List<DataRow> filterRows = new List<DataRow>();
                if (liveindicator == 2)
                {
                    filterRows = dtData.AsEnumerable().Select(s => s).ToList();
                }
                else
                {
                    filterRows = dtData.AsEnumerable().Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).Select(s => s).ToList();
                }


                DataTable dtSum = dtData.Clone();
                List<int> LeagueIds = filterRows.Select(s => CastAide.AsInt32(s["leagueid"])).Distinct().ToList();
                foreach (int leagueId in LeagueIds)
                {
                    var rowByLeagueId = filterRows.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).Select(s => s).ToList();
                    if (rowByLeagueId.Count == 0)
                        continue;

                    DataRow drNewRow = dtSum.NewRow();
                    drNewRow["leagueid"] = leagueId;
                    drNewRow["winloss"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                    drNewRow["turnover"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                    drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                        ? "0"
                        : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                    drNewRow["betcount"] = rowByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                    drNewRow["HasData"] = 1;
                    drNewRow["rptYear"] = 1;

                    dtSum.Rows.Add(drNewRow);
                }


                // get data yearly
                List<int> years = filterRows.Select(s => CastAide.AsInt32(s["rptyear"])).Distinct().ToList();
                var YearsOrder = years.OrderBy(s => s);

                foreach (int year in YearsOrder)
                {
                    var rowsByYear = filterRows.Where(s => CastAide.AsInt32(s["rptyear"]) == year).ToList();
                    DataTable dtYear = dtData.Clone();
                    foreach (DataRow dr in dtSum.Select())
                    {
                        DataRow drNewRow = dtYear.NewRow();
                        int leagueId = CastAide.AsInt32(dr["leagueId"]);
                        var rowsByLeagueId = rowsByYear.Where(s => CastAide.AsInt32(s["leagueId"]) == leagueId).ToList();

                        drNewRow["leagueid"] = leagueId;
                        drNewRow["rptyear"] = year;

                        if (rowsByLeagueId.Count == 0)
                        {
                            drNewRow["HasData"] = 0;
                        }
                        else
                        {
                            drNewRow["winloss"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["winloss"]));
                            drNewRow["turnover"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["turnover"]));
                            drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                                ? "0"
                                : NumberFormat.DecFormat(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]) * 100, 2);
                            drNewRow["betcount"] = rowsByLeagueId.Sum(s => CastAide.AsDecimal(s["betcount"]));
                            drNewRow["HasData"] = 1;
                        }

                        dtYear.Rows.Add(drNewRow);
                    }

                    listData.Add(dtYear);
                }

                // grand total
                listData.Add(dtSum);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listData;
        }
       
        public string FirstDateOfWeek(int year, int weekNum, CalendarWeekRule rule)
        {
            DateTime result = new DateTime();
            string format = "MMM dd, yyyy";
            try
            {
                DateTime jan1 = new DateTime(year, 1, 1);

                int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
                DateTime firstMonday = jan1.AddDays(daysOffset);
                var cal = CultureInfo.CurrentCulture.Calendar;
                int firstWeek = cal.GetWeekOfYear(firstMonday, rule, DayOfWeek.Monday);

                //if (firstWeek <= 1)
                //{
                //    weekNum -= 1;
                //}
                weekNum--;
                result = firstMonday.AddDays(weekNum * 7);
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result.ToString(format);
        }
    }
}
