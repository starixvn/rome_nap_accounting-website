using System.Collections.Generic;
using System.Linq;

namespace Fanex.Tarus.Customer.ViewModels.CustomerSetting.Suspend
{
    public class SuspendMatrixViewModel
    {
        private IEnumerable<CustomerInfo> _customerInfo;

        private IEnumerable<SuspendMatrix> _suspendMatrix;

        public SuspendMatrixViewModel(IEnumerable<CustomerInfo> customerInfo, IEnumerable<SuspendMatrix> suspendMatrix)
        {
            _customerInfo = customerInfo;
            _suspendMatrix = suspendMatrix;
        }

        public IReadOnlyCollection<CustomerInfo> CustomerInfo
        {
            get
            {
                return _customerInfo.OrderBy(m => m.RoleId).ToList().AsReadOnly();
            }
        }

        public IReadOnlyCollection<SuspendMatrix> SuspendMatrix
        {
            get
            {
                return _suspendMatrix.OrderByDescending(m => m.RoleId).ToList().AsReadOnly();
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

    public class SuspendMatrix
    {
        public int RoleId { get; }

        public bool Suspend { get; }

        public bool sSuspend { get; }

        public bool mSuspend { get; }

        public bool aSuspend { get; }

        public bool pSuspend { get; }

        public int AgentId { get; }

        public SuspendMatrix(int roleId, bool suspend, bool ssuspend, bool msuspend, bool asuspend, bool psuspend, int agentid)
        {
            RoleId = roleId;
            Suspend = suspend;
            sSuspend = ssuspend;
            mSuspend = msuspend;
            aSuspend = asuspend;
            pSuspend = psuspend;
            AgentId = agentid;
        }
    }
}
