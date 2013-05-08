using System;
using BLL.Report;
using ExcelUtil.Spreadsheet;

/// <summary>
/// Summary description for ExChangenRateBase
/// </summary>
namespace Sunplus.Accounting.Business.ExChangeRate
{
    public abstract class ExChangeRateBase : ReportBase
    {
        public virtual string GenHeader()
        {
            throw new NotImplementedException();
        }

        public virtual string GenNewRate()
        {
            throw new NotImplementedException();
        }

        public virtual ExcelFile ExportExRate()
        {
            throw new NotImplementedException();
        }

        public virtual string ReadExRateFromExcel()
        {
            throw new NotImplementedException();
        }
    }
}