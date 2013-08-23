using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.CustomerAdapter.ConsolidatedGroup;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Customer.ConsolidatedGroup;

namespace Sunplus.Accounting.Business.Customer.ConsolidatedGroup
{
    public class ConsolidatedGroupManager : IConsolidatedGroupManager
    {
        public List<ConsolidatedGroupTable> GetAllGroup()
        {
            var result = ConsolidatedGroupAdapter.GetAllGroup();
            if (result == null || result.Tables.Count <= 0) return null;
            var dataTable = result.Tables[0];

            var datasetCurrency = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.ExchangeSelRpt(AppSession.SYSTEM);
            if (datasetCurrency == null || datasetCurrency.ResultSet.Tables.Count <= 0) return null;
            var dataTableCurrency = datasetCurrency.ResultSet.Tables[0];

            var listConsolidatedGroup = new List<ConsolidatedGroupTable>();
            foreach (DataRow row in dataTable.Rows)
            {
                listConsolidatedGroup.Add(new ConsolidatedGroupTable()
                {
                    GroupID = CastAide.AsInt32(row["GroupID"].ToString())
                    ,
                    CustID = CastAide.AsInt32(row["CustID"].ToString())
                    ,
                    UserName = row.IsNull("username") ? string.Empty : row["username"].ToString()
                    ,
                    Currency = row.IsNull("currency") ? string.Empty : row["currency"].ToString()
                    ,
                    CurrencyName = this.GetCurrencyName(dataTableCurrency, row.IsNull("currency") ? string.Empty : row["currency"].ToString())
                    ,
                    TotalMember = CastAide.AsInt32(row.IsNull("TotalMember") ? "0" : row["TotalMember"].ToString())
                });
            }
            return listConsolidatedGroup;
        }

        private string GetCurrencyName(DataTable dataTable, string ExchangeID)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["ExchangeID"].ToString() == ExchangeID) return row["Currency"].ToString();
            }
            return string.Empty;
        }

        public Sunplus.Accounting.Entities.Result UpdateGroup(string GroupID, string UserName, bool isHeader, string Remark)
        {
            try
            {
                return ConsolidatedGroupAdapter.UpdateGroup(UserName, CastAide.AsInt32(GroupID), isHeader ? 1 : 0,
                Remark, AppSession.AccId, HttpContext.Current.Request.UserHostAddress);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                return new Result();
            }
        }

        private int GetCustID(string UserName)
        {
            var customerService = new CustomerService();
            var ds = customerService.GetCustomerInfo(UserName, AppSession.SYSTEM);
            if (ds == null || ds.ResultSet.Tables.Count != 0 && ds.ResultSet.Tables[0].Rows.Count > 0)
            {
                DataTable dataTable = ds.ResultSet.Tables[0];
                DataRow row = ds.ResultSet.Tables[0].Rows[0];
                return CastAide.AsInt32(row["custid"]);
            }
            return -1;
        }

        public List<ConsolidatedMemberTable> GetMemberByGroupID(int GroupID)
        {
            try
            {
                var result = ConsolidatedGroupAdapter.GetMemberByGroupID(GroupID);
                if (result == null || result.Tables.Count <= 0) return null;
                var dataTable = result.Tables[0];

                var datasetCurrency = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.ExchangeSelRpt(AppSession.SYSTEM);
                if (datasetCurrency == null || datasetCurrency.ResultSet.Tables.Count <= 0) return null;
                var dataTableCurrency = datasetCurrency.ResultSet.Tables[0];

                var listConsolidatedMembers = new List<ConsolidatedMemberTable>();
                foreach (DataRow row in dataTable.Rows)
                {
                    listConsolidatedMembers.Add(new ConsolidatedMemberTable()
                    {
                        GroupID = CastAide.AsInt32(row["Groupid"].ToString()),
                        CustID = CastAide.AsInt32(row["Custid"].ToString()),
                        UserName = row.IsNull("Username") ? string.Empty : row["Username"].ToString(),
                        IsHeader = CastAide.AsBoolean(row["isHeader"].ToString()),
                        CurrencyName = this.GetCurrencyName(dataTableCurrency, row.IsNull("currency") ? string.Empty : row["currency"].ToString())
                    });
                }
                return listConsolidatedMembers;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<ConsolidatedMemberTable>();
            }
        }

        public Sunplus.Accounting.Entities.Result AddUserToGroup(int GroupID, string UserName)
        {
            try
            {
                return ConsolidatedGroupAdapter.AddUserToGroup(GroupID, UserName, HttpContext.Current.Request.UserHostAddress);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result();
            }
        }

        public Sunplus.Accounting.Entities.Result RemoveUsers(int GroupID, string CustIDList, string IP)
        {
            try
            {
                string[] customers = CustIDList.Split(',');
                List<string> listCustID = new List<string>();
                List<string> listCustName = new List<string>();
                foreach (string customer in customers)
                {
                    var arr = customer.Split(';');
                    listCustID.Add(arr[1]);
                    listCustName.Add(arr[0]);
                }
                for (int i = 0; i < listCustID.Count; ++i)
                {
                    if (listCustID[i] == "-1")
                    {
                        var custInfo = CustomerUtils.GetCustomer(listCustName[i]);
                        listCustID[i] = custInfo.CustId.ToString();
                    }
                }
                return ConsolidatedGroupAdapter.RemoveUsers(GroupID, string.Join(",", listCustID.ToArray()), HttpContext.Current.Request.UserHostAddress);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result();
            }
        }

        public Sunplus.Accounting.Entities.Result ChangeHeadline(int GroupID, int CustID, string CustName, string IP)
        {
            try
            {
                if (CustID == -1)
                {
                    var custInfo = CustomerUtils.GetCustomer(CustName);
                    CustID = custInfo.CustId;
                }
                return ConsolidatedGroupAdapter.ChangeHeadline(GroupID, CustID, IP);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result();
            }
        }
    }
}