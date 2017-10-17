using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using DNAide.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess;

namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerManager
    {
        public string GenOnlineList(int custId)
        {
            string tplOnlineList = WebUtils.LoadTemplate("CustomerInfo/OnlineList.htm");
            string tplRowData = TemplateHandler.CopyBlock(tplOnlineList, "rowdata");
            string data = "";
            DataRowCollection rows = DataAccess.IBCDLL.UserController.CustomerSelOnline(custId);
            if (rows != null && rows.Count > 0)
            {
                for (int i = 0; i < rows.Count; ++i)
                {
                    DataRow row = rows[i];
                    string rowhtml = tplRowData;
                    TemplateHandler.Assign(ref rowhtml, "{no}", (i + 1) + "");
                    TemplateHandler.Assign(ref rowhtml, "{ip}", "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", IpManagement.ReplaceVNIP(row["IP"].ToString())) + "\" target=\"_blank\">" + IpManagement.ReplaceVNIP(row["IP"].ToString()) + "</a>");
                    TemplateHandler.Assign(ref rowhtml, "{lastcheckin}", IBCUtils.FormatHandler.FormatDateTimeEn12(row["reload_time"]));
                    data += rowhtml;
                }
            }
            else
            {
                data += "<tr class='calign'><td colspan='3'>This user is not online</td></tr>";
            }
            TemplateHandler.ReplaceBlock(ref tplOnlineList, "rowdata", data);
            return tplOnlineList;
        }

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
