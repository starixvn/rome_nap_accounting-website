using System.Collections.Generic;
using System.ServiceModel.Activation;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.ExChangeRate;
using Sunplus.Accounting.Entities;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.WcfServiceLibrary.ExRate
{
    /// <summary>
    /// Summary description for DailyTransManager
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExRateService : IExRateService
    {
        public ResultBO ReadExRateFromExcel()
        {
            var exRateMng = new ExchangeRateManager();
            return exRateMng.ReadExRateFromExcel();
        }

        public List<ExchangeRate> LoadExchangeRateOpenTote()
        {
            var exRateMng = new ExchangeRateManager();
            return exRateMng.LoadExchangeRateOpenTote();
        }

        public ResultBO UpdateExRateOpenTote(List<ExchangeRate> ExRateList)
        {
            var exRateMng = new ExchangeRateManager();
            return exRateMng.UpdateExRateOpenTote(ExRateList);
        }

        public string GenHeader()
        {
            var exRateMng = new ExchangeRateManager();
            return exRateMng.GenHeader();
        }

        public string GenNewRate()
        {
            var exRateMng = new ExchangeRateManager();
            return exRateMng.GenNewRate();
        }

        public ExcelFile ExportExRate()
        {
            var exRateMng = new ExchangeRateManager();
            return exRateMng.ExportExRate();
        }
    }
}