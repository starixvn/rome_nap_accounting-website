namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.Suspend
{
    using Criteria.Suspend;
    using CustomerSetting.Module.Core;
    using CustomerSetting.Module.Core.Models;
    using Data;
    using Fanex.Tarus.Customer.ViewModels.CustomerSetting.Suspend.Single;
    using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
    using System.Collections.Generic;
    using ViewModels.CustomerSetting.Suspend;

    public class SuspendStatusService
    {
        public SingleUpdateViewModel GetViewModel(int custId, string userName)
        {
            var criteria = new GetSingleSuspendCustomerInfoCriteria(custId);
            var listAffectedDownline = new List<SingleUpdateAffectedDownline>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    listAffectedDownline.Add(new SingleUpdateAffectedDownline(
                        Utils.ToInt32(reader["roleid"]),
                        Utils.ToInt32(reader["cntAffected"]),
                        Utils.ToInt32(reader["cntUnchanged"])));
                }
            }

            return new SingleUpdateViewModel(custId, userName, listAffectedDownline);
        }

        public SuspendedCustomerDetailViewModel GetDownlinesDetailInfo(int custId, bool isAffected, int downlineRoleId, int pageSize = 50, int pageIndex = 0)
        {
            var listDownlinesDetail = new List<UnsuspendDownlineDetail>();
            var criteria = new GetSuspendCustomerDetailInfoCriteria(custId, isAffected, downlineRoleId, pageSize, pageIndex);
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    listDownlinesDetail.Add(new UnsuspendDownlineDetail(Utils.ToInt32(reader["roleid"]), Utils.ToString(reader["username"])));
                }

                reader.Close();
                return new SuspendedCustomerDetailViewModel(Utils.ToInt32(objDb.GetOuputParams()["totalrows"]), listDownlinesDetail);
            }
        }

        public Result Update(int custId, int accId, bool suspended, string ip)
        {
            try
            {
                CustomerUpdateStatus.CustomerUpdateSuspend(accId, custId, suspended, ip);
            }
            catch (AccException accException)
            {
                return new Result(100, accException.Message);
            }

            return new Result(0, "Updated successfully.");
        }

        public SuspendMatrixViewModel GetSuspendMatrixInfo(string userName)
        {
            var listCustInfo = new List<CustomerInfo>();
            var listSuspendMatrix = new List<SuspendMatrix>();
            var criteria = new GetSuspendInfoMatrixCriteria(userName);
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    listCustInfo.Add(new CustomerInfo(
                        Utils.ToInt32(reader["roleid"]),
                        Utils.ToString(reader["username"]),
                        Utils.ToInt32(reader["custid"])));
                    listSuspendMatrix.Add(new SuspendMatrix(
                        Utils.ToInt32(reader["roleid"]),
                        Utils.ToBoolean(reader["suspended"]),
                        Utils.ToBoolean(reader["SuspendBySuper"]),
                        Utils.ToBoolean(reader["SuspendByMaster"]),
                        Utils.ToBoolean(reader["SuspendByAgent"]),
                        Utils.ToBoolean(reader["SuspendMember"])));
                }
            }

            return new SuspendMatrixViewModel(listCustInfo, listSuspendMatrix);
        }
    }
}
