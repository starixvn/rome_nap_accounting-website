
namespace Fanex.Tarus.Finance.Business.Statement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Tarus.Finance.DataAccess.Statement;
    using Fanex.Tarus.Finance.Model.Statement;
    using Fanex.Tarus.Finance.Entities;

    public interface IFinanceStatementBusiness
    {
        List<StatementDetail> GetFinanceStatementBusiness(string refNoParam, string custName, string filterBy, string fromdate, string toDay, string balanceUpDay, int IncludeAccBall);

        StatementDetail GetFinanceStatementDetail(StatementDetail statementDetail, string username);

        Result VerifyTransactionBusiness(string refNo, bool isVerify, string CheckerName, int CheckerID);

        Result DeleteTransBusiness(string RefNo, int CustID, int UserID);

        void ExportExcelFinanceStatement(string refNoParam, string custName, string filterBy, string fromdate, string toDay, string balanceUpDay, int IncludeAccBall, string reportTitle);
    }
}
