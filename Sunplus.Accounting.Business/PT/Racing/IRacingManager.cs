using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.PositionTaking.Racing;

namespace Sunplus.Accounting.Business.PT.Racing
{
    
    public interface IRacingManager
    {
        Setting GetStatusRacebook(int custId);

        Setting GetCustomerSetting(int custId, int roleId);

        Result UpdateCustomerSetting(Setting setting);

        Result UpdateCustomerStatus(Setting setting);
    }
}
