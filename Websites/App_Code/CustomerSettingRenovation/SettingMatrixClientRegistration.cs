using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.CustomerSetting.MatrixClient;
using System.Web.Routing;
using System.Web.Mvc;

namespace Sunplus.Accounting.CustomerSettingRenovation
{
    public class SettingMatrixClientRegistration : SettingMatrixRegistration
    {
        public override bool VerifyPermission()
        {
            return true;
        }
    }
}