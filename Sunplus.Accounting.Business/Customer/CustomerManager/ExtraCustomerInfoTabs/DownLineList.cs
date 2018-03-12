namespace Sunplus.Accounting.Business.Customer
{
    using System.Collections;
    using System.Data;
    using System.Web;
    using Core.Web;
    using Core.Web.Controls;
    using DataAccess;
    using DNAide.Data;
    using DNAide.Web;

    public partial class CustomerManager
    {
        public Hashtable GenDownlineList(int custId, int roleId, int pageIndex)
        {
            Hashtable returndata = new Hashtable();
            string content = "";
            string tplDownlineList = WebUtils.LoadTemplate("CustomerInfo/DownlineList.htm");
            string tplRowData = TemplateHandler.CopyBlock(tplDownlineList, "rowdata");
            if (roleId <= IBCUtils.Objects.UserInfo.ROLE_MEMBER || roleId > IBCUtils.Objects.UserInfo.ROLE_SUPER)
            {
                content = "<div style='width:250px;text-align:center;'>No downline list</div>";
                returndata.Add("Content", content);
                returndata.Add("Totalpages", 1);
                return returndata;
            }
            DbResult lsDownline = SiteProvider.Customer().GetDownLineList(custId, 100, pageIndex);
            DataRowCollection rows = lsDownline.ResultSet.Tables[0].Rows;
            int totalDownline = lsDownline.ResultSet.Tables[0].Rows.Count;
            int TotalPages = (int)lsDownline.Outputs["totalpages"];

            if (totalDownline == 0)
            {
                content = "<div style='width:250px;text-align:center;'>No downline list</div>";
                returndata.Add("Content", content);
                returndata.Add("Totalpages", 1);
                return returndata;
            }
            content = "";
            for (int i = 0; i < totalDownline; i = i + 2)
            {
                DataRow row = rows[i];
                string rowHtml = tplRowData;
                TemplateHandler.Assign(ref rowHtml, "{user1}", HttpUtility.HtmlDecode(row["username"] + ""));
                if (i + 1 < totalDownline)
                {
                    row = rows[i + 1];
                    TemplateHandler.Assign(ref rowHtml, "{user2}", HttpUtility.HtmlDecode(row["username"] + ""));
                }
                else
                {
                    TemplateHandler.Assign(ref rowHtml, "{user2}", "&nbsp;");
                }

                content += rowHtml;
            }
            TemplateHandler.ReplaceBlock(ref tplDownlineList, "rowdata", content);
            string strpager = "";
            if (TotalPages > 1)
            {
                strpager = Pager.InitPager(TotalPages, pageIndex, "pageritemclick");
            }

            returndata.Add("Content", tplDownlineList);
            returndata.Add("Totalpages", TotalPages);
            returndata.Add("Pager", strpager);
            return returndata;
        }
    }
}
