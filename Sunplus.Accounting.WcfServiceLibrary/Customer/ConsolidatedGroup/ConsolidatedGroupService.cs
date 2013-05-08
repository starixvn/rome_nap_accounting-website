using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Accounting.Business.Customer.ConsolidatedGroup;
using Sunplus.Accounting.Entities.Customer.ConsolidatedGroup;
using System.ServiceModel.Activation;
using Sunplus.Data.DataAbstract.BootStrappers;
using System.Web;

namespace Sunplus.Accounting.WcfServiceLibrary.Customer.ConsolidatedGroup
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ConsolidatedGroupService : IConsolidatedGroupService
    {
        public List<ConsolidatedGroupTable> GetAllGroup()
        {
            var consolidatedGroupManager = new ConsolidatedGroupManager();
            return consolidatedGroupManager.GetAllGroup();
        }

        public Sunplus.Accounting.Entities.Result UpdateGroup(string GroupID, string UserName, bool isHeader, string Remark)
        {
            var consolidatedGroupManager = new ConsolidatedGroupManager();
            return consolidatedGroupManager.UpdateGroup(GroupID, UserName, isHeader, Remark);
        }

        public List<ConsolidatedMemberTable> GetMemberByGroupID(int GroupID)
        {
            var consolidatedGroupManager = new ConsolidatedGroupManager();
            return consolidatedGroupManager.GetMemberByGroupID(GroupID);
        }

        public Sunplus.Accounting.Entities.Result AddUserToGroup(int GroupID, string UserName)
        {
            var consolidatedGroupManager = new ConsolidatedGroupManager();
            return consolidatedGroupManager.AddUserToGroup(GroupID, UserName);
        }

        public Sunplus.Accounting.Entities.Result RemoveUsers(int GroupID, string CustIDList)
        {
            var consolidatedGroupManager = new ConsolidatedGroupManager();
            return consolidatedGroupManager.RemoveUsers(GroupID, CustIDList, HttpContext.Current.Request.UserHostAddress);
        }

        public Sunplus.Accounting.Entities.Result ChangeHeadline(int GroupID, int CustID, string CustName)
        {
            var consolidatedGroupManager = new ConsolidatedGroupManager();
            return consolidatedGroupManager.ChangeHeadline(GroupID, CustID, CustName,HttpContext.Current.Request.UserHostAddress);
        }
    }
}
