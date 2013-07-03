using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.CreditTerm;
using Sunplus.Accounting.Entities.CreditTerm;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.WcfServiceLibrary.CreditTerm
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CreditTermService : ICreditTermService
    {
        public List<Group> GetAllGroup()
        {
            var creditManager = CommonBootStrapper.Locator.GetInstance<ICreditTermManager>();
            return creditManager.GetAllGroup();
        }

        public Result UpdateConsolidationReportAllowStatus(int custId, bool isViewReport)
        {
            var creditManager = CommonBootStrapper.Locator.GetInstance<ICreditTermManager>();
            return creditManager.UpdateConsolidationReportAllowStatus(custId, isViewReport);
        }
    }
}
