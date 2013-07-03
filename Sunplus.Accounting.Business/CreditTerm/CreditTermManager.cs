using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.ICreditTermDAO;
using Sunplus.Accounting.Entities.CreditTerm;
using Sunplus.Accounting.Entities.PositionTaking.Racing;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.Business.CreditTerm
{
    public class CreditTermManager : ICreditTermManager
    {
        private ICreditTermGroupDAO _creditTermDAO = null;
        public CreditTermManager(ICreditTermGroupDAO creditTermDAO)
        {
            _creditTermDAO = creditTermDAO;
        }

        public List<Group> GetAllGroup()
        {
            var listGroup = new List<Group>();
            try
            {
                var dbResult = _creditTermDAO.GetAllGroup();
                if (dbResult != null)
                {
                    var rows = dbResult.Rows;
                    var random = new Random();
                    listGroup.AddRange(from DataRow row in rows
                                       select
                                           new Group()
                                               {
                                                   GroupId = CastAide.AsInt32(row["groupid"]),
                                                   CustId = CastAide.AsInt32(row["custid"]),
                                                   HeadUsername = row["username"].ToString(),
                                                   Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(row["currency"])),
                                                   Members = CastAide.AsInt32(row["TotalMember"]),
                                                   Checker = row["checkername"].ToString(),
                                                   ConsolidationReport = CastAide.AsBoolean(row["IsRptConsol"]),
                                                   Permisssion = false
                                             });

                    listGroup = listGroup.OrderBy(m => m.Currency).ThenBy(m => m.HeadUsername).ToList();
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return listGroup;
        }

        public Result UpdateConsolidationReportAllowStatus(int custId, bool isViewReport)
        {
            var result = _creditTermDAO.UpdateConsolidationReportAllowStatus(custId, isViewReport, AppSession.AccId);

            return result;
        }
    }
}
