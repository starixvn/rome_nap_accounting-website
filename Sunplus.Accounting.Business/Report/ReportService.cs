using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using DNAide.Core;
using Microsoft.Office.Interop.Excel;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Report.MatchWL;
using Sunplus.Accounting.Entities.Report;
using System.Data;
using Sunplus.Accounting.Entities.Report.CheckBet;
using Sunplus.Core.LoggingLib;
using DataTable = System.Data.DataTable;

namespace Sunplus.Accounting.Business.Report
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportService:IReportService
    {
        private readonly IOthersDAO _reportMenuDao ;
        
        public ReportService(IOthersDAO ireReportMenuDao)
        {
            _reportMenuDao = ireReportMenuDao;
        }
        
        #region GetMenuList
        public IEnumerable<MenuReport> GetMenuList(string system)
        {
            DbResult dbResult = _reportMenuDao.GetMenuList(system);
            var menus = new List<MenuReport>();
            DataTable dataTable = dbResult.ResultSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var menu = new MenuReport
                {
                    Id = CastAide.AsInt32(dataRow["MenuId"]) ,
                    Name = CastAide.AsString(dataRow["Menu"]),
                    Url = CastAide.AsString(dataRow["url"]),
                    Order = CastAide.AsInt32(dataRow["Order"]) 
                };

                var parentId = CastAide.AsInt32(dataRow["parentId"]);
                if (parentId > 0)
                {
                    menu.Parent = new MenuReport { Id = parentId };
                }

                menus.Add(menu);
            }
            return menus;
        }
        #endregion

        #region IReportService Members
        public CheckBet GetCheckBet(string fromDate)
        {
            var checkBetData = _reportMenuDao.GetCheckBet(CastAide.AsDateTime(fromDate));

            try
            {
                if (checkBetData != null && checkBetData.Tables[0].Rows.Count > 0)
                {
                    var result = new CheckBet()
                    {
                        CheckDate = fromDate,
                        Check1 = CastAide.AsInt32(checkBetData.Tables[0].Rows[0]["check1"].ToString()),
                        Check2 = CastAide.AsInt32(checkBetData.Tables[0].Rows[0]["check2"].ToString())
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return new CheckBet();
        }

        #endregion
    }
}
