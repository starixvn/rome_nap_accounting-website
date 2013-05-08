using System.Data;
using Fanex.Data;
using System.Collections;
using Fanex.Tarus.Finance.Model;
using System.Collections.Generic;
using DNAide.Core;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Core.Common;
namespace Fanex.Tarus.Finance.DataAccess.Customer
{
    public class FinanceCustomerListAccess : IFinanceCustomerListAccess
    {
        public DataTable GetSuperList(List<PostParam> postParams, out long total)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_CustomerSelSFinance"))
            {
                var param = new Hashtable();
                foreach (PostParam p in postParams)
                {
                    param.Add(p.Name.ToLower(), p.Value);
                }
                param.Add("totalrow", 0);
                objDB.AssignParamValues(param);
                var dataSet = objDB.ExecuteDataSet();
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    total = 0;
                    return null;
                }
                total = CastAide.AsInt64(objDB.GetParameterValue("totalrow"));
                return dataSet.Tables[0];
            }
        }


        public DataTable GetCurrencyList()
        {
            var ds = SelectCustomer.ExchangeSelRpt(AppSession.SYSTEM);
            if (ds == null || ds.ResultSet.Tables.Count == 0) return null;
            return ds.ResultSet.Tables[0];
        }


        public DataTable GetMasterList(List<PostParam> postParams, out long total)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_CustomerSelBySFinance"))
            {
                var param = new Hashtable();
                foreach (PostParam p in postParams)
                {
                    param.Add(p.Name.ToLower(), p.Value);
                }
                param.Add("totalrow", 0);
                objDB.AssignParamValues(param);
                var dataSet = objDB.ExecuteDataSet();
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    total = 0;
                    return null;
                }
                total = CastAide.AsInt64(objDB.GetParameterValue("totalrow"));
                return dataSet.Tables[0];
            }
        }

        public DataTable GetAgentList(List<PostParam> postParams, out long total)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_CustomerSelByMFinance"))
            {
                var param = new Hashtable();
                foreach (PostParam p in postParams)
                {
                    param.Add(p.Name.ToLower(), p.Value);
                }
                param.Add("totalrow", 0);
                objDB.AssignParamValues(param);
                var dataSet = objDB.ExecuteDataSet();
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    total = 0;
                    return null;
                }
                total = CastAide.AsInt64(objDB.GetParameterValue("totalrow"));
                return dataSet.Tables[0];
            }
        }

        public DataTable GetMemberList(List<PostParam> postParams, out long total)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_CustomerSelByAgFinance"))
            {
                var param = new Hashtable();
                foreach (PostParam p in postParams)
                {
                    param.Add(p.Name.ToLower(), p.Value);
                }
                param.Add("totalrow", 0);
                objDB.AssignParamValues(param);
                var dataSet = objDB.ExecuteDataSet();
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    total = 0;
                    return null;
                }
                total = CastAide.AsInt64(objDB.GetParameterValue("totalrow"));
                return dataSet.Tables[0];
            }
        }
    }
}
