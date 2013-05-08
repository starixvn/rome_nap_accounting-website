using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Text;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using System.Collections;

using Sunplus.Accounting.Core.Web.Controls;

/// <summary>
/// Summary description for AB88Winloss
/// </summary>
namespace BLL.Report
{
    public class ReportAB88WinLoss : ReportBase
    {

        public override string RenderHTML(Hashtable input)
        {
            throw new NotImplementedException();
        }
    }
}
