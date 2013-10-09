using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.External;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class ExternalReportDAO : DataAccessReport, IExternalReportDAO
    {
        private StoreProcedureElement getSPMapping(string key)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.External , key);
        }

        public DbResult SelWinLossDailyByCom(string winlostdate)
        {
            DbResult result = null;
            try
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("winlostdate", winlostdate);
                hashtable.Add("system", AppSession.SYSTEM.ToString());

                string spName = "WinLossDaily_ComEx" + GetStoreMappingSubfix(CastAide.AsDateTime(winlostdate));

                using(ObjectDb db = new ObjectDb(spName, 600))
                {
                    db.AssignParamValues(hashtable);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }


        public DbResult SelWinLossDailyBySuper(string winlostdate, int custid)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlostdate);
            hashtable.Add("custid", custid);

            var element = getSPMapping("WinLossDaily_SAgEx" + GetStoreMappingSubfix(CastAide.AsDateTime(winlostdate)));

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 600);
        }


        public DbResult SelWinLossDailyByMaster(string winlostdate, int custid)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlostdate);
            hashtable.Add("custid", custid);

            var element = getSPMapping("WinLossDaily_MAgEx" + GetStoreMappingSubfix(CastAide.AsDateTime(winlostdate)));

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 600);
        }


        public DbResult SelWinLossDailyByAgent(string winlostdate, int custid)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlostdate);
            hashtable.Add("custid", custid);

            var element = getSPMapping("WinLossDaily_AgEx" + GetStoreMappingSubfix(CastAide.AsDateTime(winlostdate)));

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 600);
        }


        public DbResult SelWinLossDetailByCom(string param)
        {
            string[] strarray = param.Split(new Char[] { '-' });
            Hashtable hashtable = new Hashtable();
            for (int i = 0; i < strarray.Length; i++)
            {
                string[] subarr = strarray[i].Split(new Char[] { '=' });
                if (i > 1)
                    hashtable.Add(subarr[0], subarr[1] == "1" ? true : false);
                else
                    hashtable.Add(subarr[0], subarr[1]);
            }
            hashtable.Add("system", AppSession.SYSTEM.ToString());
            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);

            var element = getSPMapping("WinLossDetail_ComEx");

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 600);
        }


        public DbResult SelWinLossDetailBySuper(int custid, string param, int pageIndex, int pageSize)
        {
            string[] strarray = param.Split(new Char[] { '-' });
            Hashtable hashtable = new Hashtable();
            hashtable.Add("custid", custid);
            for (int i = 0; i < strarray.Length; i++)
            {
                string[] subarr = strarray[i].Split(new Char[] { '=' });
                if (i > 1)
                    hashtable.Add(subarr[0], subarr[1] == "1" ? true : false);
                else
                    hashtable.Add(subarr[0], subarr[1]);
            }

            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            hashtable.Add("pageindex", pageIndex);
            hashtable.Add("pageSize", pageSize);

            var element = getSPMapping("WinLossDetail_SAg");

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 600);
        }


        public DbResult SelWinLossDetailByMaster(int custid, string param, int pageIndex, int pageSize)
        {
            string[] strarray = param.Split(new Char[] { '-' });
            Hashtable hashtable = new Hashtable();
            hashtable.Add("custid", custid);
            for (int i = 0; i < strarray.Length; i++)
            {
                string[] subarr = strarray[i].Split(new Char[] { '=' });
                if (i > 1)
                    hashtable.Add(subarr[0], subarr[1] == "1" ? true : false);
                else
                    hashtable.Add(subarr[0], subarr[1]);
            }

            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            hashtable.Add("pageindex", pageIndex);
            hashtable.Add("pageSize", pageSize);

            var element = getSPMapping("WinLossDetail_MAg_New");

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 600);
        }

        public DbResult SelWinLossDetailByAgent(int custid, string param, int pageIndex, int pageSize)
        {
            string[] strarray = param.Split(new Char[] { '-' });
            Hashtable hashtable = new Hashtable();
            hashtable.Add("custid", custid);
            for (int i = 0; i < strarray.Length; i++)
            {
                string[] subarr = strarray[i].Split(new Char[] { '=' });
                if (i > 1)
                    hashtable.Add(subarr[0], subarr[1] == "1" ? true : false);
                else
                    hashtable.Add(subarr[0], subarr[1]);
            }

            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            hashtable.Add("pageindex", pageIndex);
            hashtable.Add("pageSize", pageSize);

            var element = getSPMapping("WinLossDetail_Ag");

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 600);
        }
    }
}
