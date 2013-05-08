using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Sunplus.Accounting.Business.Report.Deposit
{
    public interface IBetlistWLBySiteService
    {
        string RenderHTML(System.Collections.Hashtable input);

        string RenderHTMLV2(System.Collections.Hashtable input);
    }
}
