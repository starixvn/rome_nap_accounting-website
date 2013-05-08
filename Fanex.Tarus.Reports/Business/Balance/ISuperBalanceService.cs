using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelUtil.Spreadsheet;

namespace Fanex.Tarus.Reports.Business.Balance
{
    public interface ISuperBalanceService
    {

        string GetSuperBalanceTotal(string db, bool isYesterday, string usernameSearch, string firstletter, string currency);
        ExcelFile ExportExcelSuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter, string currency, string title);
    }
}
