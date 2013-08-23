using System;
using System.Collections.Generic;
using System.Linq;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities;

public partial class _Default : PageBase
{
    #region local variables

    protected string topMenu = "";
    public string accUsername = "";
    private int currentParentId = 0;

    #endregion local variables

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        try
        {
            AccCommon.LeaveSSL();
            accUsername = AppSession.AccUserName;
            var menu = (List<MenuBO>)AppSession.BAMenu;
            WriteMenu(menu.Where(item => item.ParentId == 0).Select(s => s).ToList(), 0);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }

    private void WriteMenu(IEnumerable<MenuBO> parentMenu, int level)
    {
        if (parentMenu.Count() <= 0) return;
        string url;
        var parentIndex = 0;

        topMenu += "<ul" + (level == 0 ? " class=\"sf-menu sf-js-enabled sf-shadow\"" : " style=\"display: none; visibility: hidden;\"") + ">";
        int index = 0;
        foreach (var menu in parentMenu)
        {
            string parentId = string.Format("pId{0}", parentIndex);
            url = menu.Url.IndexOf(".aspx") > 0 ? menu.Url : "javaScript:void(0);";
            string target = menu.Target == "_blank" ? "_blank" : "centerFr";
            if (level == 0)
            {
                topMenu += "<li " + (index == 0 ? " class=\"first_item\"" : "") + "><a id=\"" + parentId + "\" href=\"" + url + "\" target=\"" + target + "\"><span><span>" + menu.Name + "</span></span></a>";
            }
            else
            {
                topMenu += "<li " + (index == 0 ? " class=\"first_item\"" : "") + "><span><a href=\"" + url + "\" target=\"" + target + "\" onclick=\"activeMain('" + string.Format("pId{0}", currentParentId) + "')\">" + menu.Name + "</a></span>";
            }
            WriteMenu(menu.SubMenu, level + 1);
            topMenu += "</li>";
            index++;

            if (level != 0) continue;
            parentIndex++;
            currentParentId = parentIndex;
        }
        topMenu += "</ul>";
    }

    //private void WriteMenu(IEnumerable<MenuBO> parentMenu, int level)
    //{
    //    if (parentMenu.Count() <= 0) return;
    //    string url;
    //    var parentIndex = 0;

    //    topMenu += "<ul >";
    //    foreach (var menu in parentMenu)
    //    {
    //        string parentId = string.Format("id=\"pId{0}\"", parentIndex);

    //        url = menu.Url.IndexOf(".aspx") > 0 ? menu.Url : "javaScript:void(0);";
    //        string target = menu.Target == "_blank" ? "_blank" : "centerFr";

    //        topMenu += "<li "+ (level == 0 ? parentId : "") + " class=\" menu_" + level + "\"><a class=\"rmLink\" href=\"" + url
    //                + "\" target=\"" + target + "\" " + (level != 0 && menu.SubMenu.Count() == 0 ? "onclick=\"closedSubmenu('pId"
    //                + currentParentId + "', " + (url.Equals("javaScript:void(0);")  ? 1 : 0) + ");"
    //                + (target == "_blank" ? "jQuery.unblockUI();" : "") + "\"" : "")
    //                + "><span class=\"" + (level == 0 && menu.SubMenu.Count() > 0 ? "rmExpandDown" : "rmExpand") + "\">" + menu.Name + "</span></a>";

    //        WriteMenu(menu.SubMenu, level + 1);
    //        topMenu += "</li>";

    //        if (level != 0) continue;
    //        parentIndex++;
    //        currentParentId = parentIndex;
    //    }
    //    topMenu += "</ul>";
    //}
}