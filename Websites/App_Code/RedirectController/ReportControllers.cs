using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Tarus.Web.Controllers;
using System.Web.Mvc;

/// <summary>
/// Summary description for ReportControllers
/// </summary>
public class ReportControllers: BaseController
{
    public RedirectResult Index()
    {
        return Redirect("httpL//www.google.com.vn");
    }
}