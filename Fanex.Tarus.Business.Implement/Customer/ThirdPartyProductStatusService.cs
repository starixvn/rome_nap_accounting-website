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

        public ThirdPartyProductStatusViewModel Select(int custId, string custName, int roleId, int productId, bool isDeposit = false)
        {
            return new ThirdPartyProductStatusViewModel()
            {
                CustId = custId,
                CustName = custName,
                RoleId = roleId,
                HasCheckAll = productId == KenoProductId,
                GroupBySport = productId == RacingProductId,
                ProductStatusList = statusSettingService.GetProductStatus(new GetProductStatusRequest() {
                    CustId = custId,
                    ProductId = productId,
                    RoleId = roleId,
                    IsDeposit = isDeposit
                }),
                IsDeposit = isDeposit
            };
        }

        public List<ProductStatusResult> Update(
            int custId,
            int subAccId,
            int productId,
            int userId,
            int systemId,
            string ip,
            List<ProductStatus> productStatus,
            bool isDeposit = false)
        {
            return statusSettingService.UpdateProductStatus(new UpdateProductStatusRequest() {
                CustId = custId,
                SubAccId = subAccId,
                ProductId = productId,
                UserId = userId,
                SystemId = systemId,
                Ip = ip,
                ProductStatus = productStatus,
                IsDeposit = isDeposit
            });
        }
    }
}
