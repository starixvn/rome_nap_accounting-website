using System.Collections;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

/// <summary>
/// Summary description for VirtualRateHistory
/// </summary>

namespace BLL.Report
{
    public class VirtualRateHistory : ReportBase
    {
        public override string RenderHTML(Hashtable Input)
        {
            var rowContent = new StringBuilder();
            string strHeader = WebUtils.LoadTemplate("Report/VirtualRateHistoryHeader.htm");
            string strRow = WebUtils.LoadTemplate("Report/VirtualRateHistoryRow.htm");
            DataRowCollection dtRow = VirtualRate.ViewVRHistory(Input).ResultSet.Tables[0].Rows;
            if (dtRow != null && dtRow.Count > 0)
            {
                int i = 0;
                foreach (DataRow row in dtRow)
                {
                    string sRow = strRow;
                    string rowBgClass = (i % 2 == 0) ? "report-row-even" : "report-row-odd";
                    i += 1;
                    TemplateHandler.Assign(ref sRow, "{bgrow}", rowBgClass);
                    TemplateHandler.Assign(ref sRow, "{no}", i.ToString());
                    TemplateHandler.Assign(ref sRow, "{logdate}", CastAide.AsString(row["logdate"]));
                    TemplateHandler.Assign(ref sRow, "{virtualrate}", CastAide.AsString(row["virtualrate"]));
                    TemplateHandler.Assign(ref sRow, "{username}", CastAide.AsString(row["username"]));
                    rowContent.Append(sRow);
                }
            }
            else
            {
                string tplNoInfo = WebUtils.LoadTemplate("NoInformation.htm");
                TemplateHandler.Assign(ref tplNoInfo, "{tdno}", "4");
                TemplateHandler.Assign(ref tplNoInfo, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                rowContent.Append(tplNoInfo);
            }
            return strHeader + rowContent;
        }
    }
}