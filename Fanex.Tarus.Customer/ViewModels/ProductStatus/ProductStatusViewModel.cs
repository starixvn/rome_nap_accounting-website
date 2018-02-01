using System.Collections.Generic;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.Tarus.Customer.ViewModels.ProductStatus
{
    public class ProductStatusViewModel
    {
        public int ProductId { get; set; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustRoleId { get; set; }

        public int TotalColumns
        {
            get
            {
                if (IsSuper(CustRoleId))
                {
                    return 8;
                }

                if (IsMaster(CustRoleId))
                {
                    return 6;
                }

                if (IsAgent(CustRoleId))
                {
                    return 4;
                }

                if (IsMember(CustRoleId))
                {
                    return 2;
                }

                return 0;
            }
        }

        public IEnumerable<ProductStatusModel> ProductStatuses { get; set; }

        public bool IsSuper(byte roleId)
        {
            return roleId == 4;
        }

        public bool IsMaster(byte roleId)
        {
            return roleId == 3;
        }

        public bool IsAgent(byte roleId)
        {
            return roleId == 2;
        }

        public bool IsMember(byte roleId)
        {
            return roleId == 1;
        }

        public string GetRoleNameById(byte roleId)
        {
            if (IsSuper(roleId))
            {
                return Resources.Common.Super;
            }

            if (IsMaster(roleId))
            {
                return Resources.Common.Master;
            }

            if (IsAgent(roleId))
            {
                return Resources.Common.Agent;
            }

            if (IsMember(roleId))
            {
                return Resources.Common.Member;
            }

            return string.Empty;
        }
    }
}
