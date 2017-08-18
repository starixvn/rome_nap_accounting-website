using Fanex.Tarus.Web.Models.Customer.ThirdPartyProductStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThirdPartyProducts.StatusSetting.Model;

namespace Fanex.Tarus.Business.Interface.Customer
{
    public interface IThirdPartyProductStatusService
    {
        ThirdPartyProductStatusViewModel Select(int custId, string custName, int roleId, int productId, bool isDeposit = false);

        List<ProductStatusResult> Update(
            int custId,
            int subAccId,
            int productId,
            int userId,
            int systemId,
            string ip,
            List<ProductStatus> productStatus,
            bool isDeposit = false);
    }
}
