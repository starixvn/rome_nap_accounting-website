using Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.ViewModels.MultipleUpdateSuperStatus
{
    public class SupersStatusViewModel
    {
        public List<SuperStatusEntity> Supers { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public SupersStatusViewModel()
        {
            Supers = new List<SuperStatusEntity>();
        }
    }
}
