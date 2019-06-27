using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fanex.Tarus.Customer.ViewModels.CustomerSetting.Suspend
{
    public class SuspendedCustomerDetailViewModel
    {
        private readonly IEnumerable<UnsuspendDownlineDetail> _downlinesDetail;

        public SuspendedCustomerDetailViewModel(int totalRows, IEnumerable<UnsuspendDownlineDetail> downlinesDetail)
        {
            TotalRows = totalRows;
            _downlinesDetail = downlinesDetail;
        }

        public int TotalRows { get; }

        public ReadOnlyCollection<UnsuspendDownlineDetail> AffectedDownlines
        {
            get
            {
                return _downlinesDetail.ToList().AsReadOnly();
            }
        }
    }
}
