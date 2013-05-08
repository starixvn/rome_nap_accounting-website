// -----------------------------------------------------------------------
// <copyright file="IOANDAExRate.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.ExchangeRate.Business
{
    using System.Collections.Generic;

    using ExcelUtil.Spreadsheet;

    using Fanex.Tarus.ExchangeRate.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IExchangeRateService
    {
        List<ExchangeDailyDTO> GetOandaExchangeDaily(string fromDate, string toDate);
        ExcelFile ExportExcelExDailyOanda(string fdate, string tdate);
    }

}
