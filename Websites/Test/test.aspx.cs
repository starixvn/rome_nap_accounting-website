using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Utils;

using Fanex.Caching;
using Fanex.Data;
using Fanex.Tarus.BetList.Helper;


using Sunplus.Accounting.Business.BootStrappers;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;
using DNAide.Core;
using System.Globalization;

public partial class test : System.Web.UI.Page
{
    private static readonly ICache _cacheManager;
    protected void Page_Load(object sender, EventArgs e)
    {
         
        try
        {
            //_cacheManager.Set(
            System.Web.HttpUtility.UrlDecode("VNM-CICV-CASH+INH");

        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        
    }
   
}
