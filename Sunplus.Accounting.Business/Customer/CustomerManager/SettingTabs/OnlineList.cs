namespace Sunplus.Accounting.Business.Customer
{
    using System.Configuration;
    using System.Data;
    using Core.Web;
    using DNAide.Web;

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
    }
}
