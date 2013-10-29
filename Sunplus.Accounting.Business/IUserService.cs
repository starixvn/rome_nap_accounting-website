using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Summary description for IUserService
    /// </summary>
    public interface IUserService
    {
        bool InsResetWinning(DateTime dateTime, int userId, string[] arrDesc, string IP, ref string ErrMsg);

        List<ResetWinning> GetResetWinning();

        List<FinanceTransaction> GetResetWinning_ByDate(DateTime transDate, bool isReset);

        DataSet InsDataTmpExcel(int userId, string arrFromCust, string arrToCust, string arrAmount, string arrExRate,
                                 string arrDescription, ref Hashtable error);

        DataSet InsDataTmpExcel(int userId, string xml, ref Hashtable error);

        Result InsDataTmpExcelFinanceCrossTransfer(int userId, string arrFromCust, string arrToCust, string arrFromAmount,
                                        string arrToAmount, string arrDescription);

        bool DelDataTmpExcel(int UserId);

        DataSet UpdFinanceTransferMul(int userId, string fileName, int fileSize, string IP, ref Hashtable error);

        DataSet UpdFinanceCrossTransferMul(int userId, string fileName, int fileSize, string IP, string forexName, ref Hashtable error);

        List<DailyTransaction> GetDailyTransaction(DateTime transDate, int pageIndex, int pageSize,
                                                   int status, string type, string creator, ref int totalPages,
                                                   string verifyBy);

        List<DailyTransaction> GetDailyAllTransaction(DateTime transDate, string type, string creator, int pageIndex, int pageSize, ref int totalPage);

        //DailyTransactionDTO GetDailyAllTransaction(DateTime transDate, string type, string creator, int pageIndex, int pageSize, int fromcache);

        bool VerfiyDailyTransactions(User user, string refNos);

        bool UnverifyTransaction(User user, string refNo);

        /// <summary>
        /// Get dictionary group, key contanins refNo, value is the total number transaction
        /// has the same ref no
        /// </summary>
        /// <param name="transList"></param>
        /// <returns></returns>
        Dictionary<string, int> GetListTotalTransPerGroup(List<DailyTransaction> transList);

        int GetListTotalTransPerGroup(List<DailyTransaction> transList, string RefNo);

        List<Entities.Statement> GetCustomerStatement(User customer, DateTime fromDate, DateTime toDate, DateTime balanceUpDay,
                                             bool isInlucdeAccBal);

        int GetCustomerIdByUsername(string custName);

        List<FinanceBettype> GetFinanceBettyles();

        Result InsertCustomerToGroup(int custId, int roleId, string system);
    }
}