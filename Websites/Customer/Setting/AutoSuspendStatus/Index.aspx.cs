// --------------------------------------------------------------------------
// <copyright file="Index.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;
using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.Entities.AutoSuspend;

/// <summary>
/// TODO: Update summary.
/// </summary>
public partial class Customer_Setting_AutoSuspendStatus_Index : Page
{
    private readonly Dictionary<string, int> _listPage = new Dictionary<string, int>() 
    {
        { "SuperUpdate.aspx",  RoleIdConstants.SUPER }, 
        { "MasterUpdate.aspx",  RoleIdConstants.MASTER },
        { "AgentUpdate.aspx",  RoleIdConstants.AGENT }, 
        { "MemberUpdate.aspx",  RoleIdConstants.MEMBER },
        { "SuperOnMemberUpdate.aspx", RoleIdConstants.SuperOnMember }, 
        { "MasterOnMemberUpdate.aspx", RoleIdConstants.MasterOnMember },
        { "SuperMinUpdate.aspx", RoleIdConstants.SuperMin }, 
        { "MasterMinMaxUpdate.aspx", RoleIdConstants.MasterMinMax },
        { "AgentMinMaxUpdate.aspx", RoleIdConstants.AgentMinMax }
    };

    public string ViewResources { get; set; }

    /// <summary>
    /// Gets the view resources.
    /// </summary>
    public void GetViewResources()
    {
        var custid = Request["custid"];
        var jsonData = new ViewResources
        {
            CustomerId = custid,
            CurrentRole = GetCurrentPage()
        };

        jsonData.PageMD5Key = IBCCrypt.IBCCrypt.GenMD5Key(custid + jsonData.CurrentRole.ToString()) +
            IBCCrypt.IBCCrypt.GenMD5Key("^") + custid + jsonData.CurrentRole.ToString();
        jsonData.Labels = new Dictionary<string, string>();
        var serializer = new JavaScriptSerializer();
        ViewResources = serializer.Serialize(jsonData);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        GetViewResources();
    }

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Gets the current page.
    /// </summary>
    /// <returns>Level id of current page.</returns>
    private int GetCurrentPage()
    {
        var levelId = 0;
        string url = Request.CurrentExecutionFilePath;
        if (Request.ApplicationPath.Equals("/"))
        {
            url = "/" + url;
        }

        url = url.Replace(Request.ApplicationPath + "/", string.Empty).ToLower();
        Parallel.ForEach(
            _listPage,
            page =>
            {
                if (url.IndexOf(page.Key.ToLower()) != -1)
                {
                    levelId = page.Value;
                }
            });

        return levelId;
    }
}
