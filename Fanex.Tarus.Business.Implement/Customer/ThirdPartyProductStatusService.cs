using Fanex.Tarus.Business.Interface.Customer;
using Fanex.Tarus.Web.Models.Customer.ThirdPartyProductStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThirdPartyProducts.StatusSetting.Model;
using ThirdPartyProducts.StatusSetting.Services;

namespace Fanex.Tarus.Business.Implement.Customer
{
    public class ThirdPartyProductStatusService : IThirdPartyProductStatusService
    {
        private StatusSettingService statusSettingService = new StatusSettingService();

        private const int KenoProductId = 8;
        private const int RacingProductId = 2;

        public ThirdPartyProductStatusViewModel Select(int custId, int roleId, int productId)
        {
            return new ThirdPartyProductStatusViewModel()
            {
                CustId = custId,
                RoleId = roleId,
                HasCheckAll = productId == KenoProductId,
                GroupBySport = productId == RacingProductId,
                ProductStatusList = statusSettingService.GetProductStatus(custId, productId, roleId)
            };
        }

        public List<ProductStatusResult> Update(
            int custId,
            int subAccId,
            int productId,
            int userId,
            int systemId,
            string ip,
            List<ProductStatus> productStatus)
        {
            return statusSettingService.UpdateProductStatus(
                custId,
                subAccId,
                0,
                productId,
                userId,
                systemId,
                ip,
                productStatus);
        }
    }
}
