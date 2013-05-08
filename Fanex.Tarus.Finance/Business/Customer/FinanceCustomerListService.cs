using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Finance.Entities;
using Fanex.Tarus.Finance.Model;
using Sunplus.Accounting.Core.Extensions;
using Fanex.Tarus.Finance.DataAccess.Customer;
using DNAide.Core;
using System.Data;
using Fanex.Tarus.Finance.Model.Customer;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Bingo.Core;
using Sunplus.Accounting.Business.Customer;

namespace Fanex.Tarus.Finance.Business.Customer
{
    public class FinanceCustomerListService : IFinanceCustomerListService
    {
        public readonly IFinanceCustomerListAccess _financeCustomerListAccess;

        public FinanceCustomerListService()
        {
            _financeCustomerListAccess = new FinanceCustomerListAccess();
        }

        public CustomerList GetSuperList(List<PostParam> postParams)
        {
            try
            {
                long total = 0;
                var table = _financeCustomerListAccess.GetSuperList(postParams, out total);
                var listEntity = new List<CustomerInfo>();
                if (table == null) return new CustomerList();
                foreach (DataRow row in table.Rows)
                {
                    var customer = BuildSuperEntity(row);
                    listEntity.Add(customer);
                }
                return new CustomerList
                {
                    Total = total,
                    Customers = listEntity
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new CustomerList();
            }
        }

        private CustomerInfo BuildSuperEntity(DataRow row)
        {
            var status = string.Empty;
            if (CastAide.AsBoolean(row["closed"]))
                status = "Closed";
            else if (CastAide.AsBoolean(row["suspended"]))
                status = "Suspended";
            else status = "Open";

            var p2pSetting = PokerSection.Instance[PokerSection.PokerCollections.CustomerSetting]["visibleP2PSetting"];

            var custInfo = new CustomerInfo
            {
                Id = CastAide.AsInt64(row["custid"]),
                UserName = CastAide.AsString(row["username"]),
                UserName2 = CastAide.AsString(row["username2"]),
                FirstName = CastAide.AsString(row["firstname"]),
                LastName = CastAide.AsString(row["lastname"]),
                Status = status,
                Order = CastAide.AsInt64(row["rownumber"]),
                Type = "Super",
                RoleId = CastAide.AsInt32(row["roleid"]),
                ParentId = 0,
                IsAgent = 0,
                DisabledHR = WebSettings.disabledHR,
                DisableCS = WebSettings.disableCS,
                DisabledFI = WebSettings.disabledFI,
                BingoGameEnabled = BingoSettings.BingoGame.Enabled,
                EnableLiveCasino = WebSettings.EnableLiveCasino,
                VisibleP2PSetting = CastAide.AsBoolean(p2pSetting.Value)
            };
            if (custInfo.RoleId == 2)
            {
                custInfo.IsDirectMaster = CustomerUtils.IsTransferDaily(custInfo.Id.ToString());
            }
            return custInfo;
        }


        public List<Exchange> GetCurrencyList()
        {
            try
            {
                var lisEntity = new List<Exchange>();
                var table = _financeCustomerListAccess.GetCurrencyList();
                if (table == null) return lisEntity;
                foreach (DataRow row in table.Rows)
                {
                    lisEntity.Add(new Exchange 
                    {
                        Id = CastAide.AsInt32(row["exchangeid"]),
                        Name = CastAide.AsString(row["currency"])
                    });
                }
                return lisEntity;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<Exchange>();
            }
        }


        public CustomerList GetMasterList(List<PostParam> postParams)
        {
            try
            {
                long total = 0;
                var table = _financeCustomerListAccess.GetMasterList(postParams, out total);
                var listEntity = new List<CustomerInfo>();
                if (table == null) return new CustomerList();
                foreach (DataRow row in table.Rows)
                {
                    var customer = BuildSuperEntity(row);
                    listEntity.Add(customer);
                }
                return new CustomerList
                {
                    Total = total,
                    Customers = listEntity
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new CustomerList();
            }
        }

        public CustomerList GetAgentList(List<PostParam> postParams)
        {
            try
            {
                long total = 0;
                var table = _financeCustomerListAccess.GetAgentList(postParams, out total);
                var listEntity = new List<CustomerInfo>();
                if (table == null) return new CustomerList();
                foreach (DataRow row in table.Rows)
                {
                    var customer = BuildSuperEntity(row);
                    listEntity.Add(customer);
                }
                return new CustomerList
                {
                    Total = total,
                    Customers = listEntity
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new CustomerList();
            }
        }

        public CustomerList GetMemberList(List<PostParam> postParams)
        {
            try
            {
                long total = 0;
                var table = _financeCustomerListAccess.GetMemberList(postParams, out total);
                var listEntity = new List<CustomerInfo>();
                if (table == null) return new CustomerList();
                foreach (DataRow row in table.Rows)
                {
                    var customer = BuildSuperEntity(row);
                    listEntity.Add(customer);
                }
                return new CustomerList
                {
                    Total = total,
                    Customers = listEntity
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new CustomerList();
            }
        }
    }
}
