using System;
using System.Collections.Generic;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.BetList.Business;
using Fanex.Tarus.BetList.Entities.BetList;

namespace Fanex.Tarus.BetList.ViewModel
{
    public class BetListByRefNoViewModel
    {
        public BetListByRefNoViewModel(IEnumerable<ProductEntity> productList)
        {
            ProductList = productList;
        }

        public BetListByRefNoViewModel()
        {
            ProductList = new List<ProductEntity>();
        }

        public DateTime BalanceUpDay => ReportService.BalanceUp.BalanceUpDay;

        public IEnumerable<ProductEntity> ProductList { get; set; }
    }
}
