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
                if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    var rows = dbResult.ResultSet.Tables[0].Rows;
                    listGroup.AddRange(from DataRow row in rows
                                       select
                                           new Group()
                                               {
                                                   GroupId = CastAide.AsInt32(row["groupid"]),
                                                   HeadUsername = row["username"].ToString(),
                                                   Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(row["currency"])),
                                                   Checker = row["checkername"].ToString(),
                                                   Permisssion = false
                                             });
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return listGroup;
        }
    }
}
