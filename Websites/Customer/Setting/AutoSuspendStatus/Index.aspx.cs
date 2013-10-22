using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;
using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.Entities.AutoSuspend;

public partial class Customer_Setting_AutoSuspendStatus_Index : Page
{
    public string ViewResourses;

    private readonly Dictionary<string, int> _listPage = new Dictionary<string, int>(){
        {"SuperUpdate.aspx",  RoleIdConstants.SUPER}, 
        {"MasterUpdate.aspx",  RoleIdConstants.MASTER},
        {"AgentUpdate.aspx",  RoleIdConstants.AGENT}, 
        {"MemberUpdate.aspx",  RoleIdConstants.MEMBER},
        {"SuperOnMemberUpdate.aspx", RoleIdConstants.SuperOnMember}, 
        {"MasterOnMemberUpdate.aspx", RoleIdConstants.MasterOnMember},
        {"SuperMinUpdate.aspx", RoleIdConstants.SuperMin}, 
        {"MasterMinMaxUpdate.aspx", RoleIdConstants.MasterMinMax},
        {"AgentMinMaxUpdate.aspx", RoleIdConstants.AgentMinMax}
    };

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        GetViewResourses();
    }

    public void GetViewResourses()
    {
        var custid = Request["custid"];
        var jsonData = new ViewResourses
        {
            CustomerId = custid,
            CurrentRole = GetCurrentPage()
        };

        jsonData.PageMD5Key = IBCCrypt.IBCCrypt.GenMD5Key(custid + jsonData.CurrentRole.ToString()) +
            IBCCrypt.IBCCrypt.GenMD5Key("^") + custid + jsonData.CurrentRole.ToString();
        jsonData.Labels = new Dictionary<string, string>();
        var serializer = new JavaScriptSerializer();
        ViewResourses = serializer.Serialize(jsonData);
    }

    private int GetCurrentPage()
    {
        var levelId = 0;
        string url = Request.CurrentExecutionFilePath;
        if (Request.ApplicationPath.Equals("/"))
        {
            url = "/" + url;
        }

        url = url.Replace(Request.ApplicationPath + "/", "").ToLower();
        Parallel.ForEach(_listPage, page =>
        {
            if (url.IndexOf(page.Key.ToLower()) != -1)
            {
                levelId = page.Value;
            }
        });

        return levelId;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
