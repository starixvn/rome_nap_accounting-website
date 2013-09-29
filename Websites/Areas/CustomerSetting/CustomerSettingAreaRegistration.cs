using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Summary description for CustomerSettingAreaRegistration
/// </summary>
public class CustomerSettingAreaRegistration : AreaRegistration
{
	public CustomerSettingAreaRegistration()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public override string AreaName
    {
        get
        {
            return "CustomerSetting";
        }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
        RouteConfig.RegisterRoutes(this.AreaName, context);
    }
}