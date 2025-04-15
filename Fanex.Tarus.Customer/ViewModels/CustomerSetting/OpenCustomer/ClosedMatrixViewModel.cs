namespace Fanex.Tarus.Customer.ViewModels.CustomerSetting.OpenCustomer
{
    using System.Collections.Generic;
    using System.Linq;

    public class ClosedMatrixViewModel
    {
        private readonly IEnumerable<CustomerInfo> customerInfoList;

        private readonly IEnumerable<ClosedMatrix> closedMatrixList;

        public ClosedMatrixViewModel(
            IEnumerable<CustomerInfo> customerInfoList,
            IEnumerable<ClosedMatrix> closedMatrixList)
        {
            this.customerInfoList = customerInfoList;
            this.closedMatrixList = closedMatrixList;
        }

        public IReadOnlyCollection<CustomerInfo> CustomerInfoList
        {
            get
            {
                return customerInfoList.OrderBy(m => m.RoleId).ToList().AsReadOnly();
            }
        }

        public IReadOnlyCollection<ClosedMatrix> ClosedMatrixList
        {
            get
            {
                return closedMatrixList.OrderByDescending(m => m.RoleId).ToList().AsReadOnly();
            }
        }
    }

    public class CustomerInfo
    {
        public int RoleId { get; }

        public string UserName { get; }

        public int CustId { get; }

        public CustomerInfo(int roleId, string userName, int custId)
        {
            RoleId = roleId;
            UserName = userName;
            CustId = custId;
        }
    }

    public class ClosedMatrix
    {
        public int RoleId { get; }

        public bool Closed { get; }

        public bool SClosed { get; }

        public bool MClosed { get; }

        public bool AClosed { get; }

        public bool PClosed { get; }

        public int AgentId { get; }

        public ClosedMatrix(
            int roleId,
            bool closed,
            bool sclosed,
            bool mclosed,
            bool aclosed,
            bool pclosed,
            int agentId
            )
        {
            RoleId = roleId;
            Closed = closed;
            SClosed = sclosed;
            MClosed = mclosed;
            AClosed = aclosed;
            PClosed = pclosed;
            AgentId = agentId;
        }
    }
}