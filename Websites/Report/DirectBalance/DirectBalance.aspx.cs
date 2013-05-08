using System;
using System.Web;
using DNAide.Web;
using System.Text;
using DNAide.Core;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using System.Xml;
using ExcelUtil.Spreadsheet;

public partial class Report_DirectBalance : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {


        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        
        int isExport = CastAide.AsInt32(Request["isexport"], 0);
        this.ReportTitle.Text = Resources.Language.DirectBalance;
        reportHeader.Text = getHeaderLink();

        if (isExport == 1)
        {
            string strContent = string.Empty;
            

            string display = CastAide.AsString(Request["display"]);
            string fileName = "DirectBalance";
            if (display == "member")
            {
                fileName = fileName + "_member";
            }
            else if (display == "agent")
            {
                fileName = fileName + "_agent";
            }
            else if (display == "master")
            {
                fileName = fileName + "_master";
            }


            //apply cache for direct balance
            if (!string.IsNullOrEmpty(ReportCache.REPORT_DIRECT_BAL))
                strContent = ReportCache.REPORT_DIRECT_BAL;
            else
            {
                IServiceDirectBalance direct = new ServiceDirectBalance();
                strContent = direct.GetDirectBalance(display);
            }
            if (strContent.Contains("divDirectBalanceTemplate"))
            {
                ExportToExcel(Response, strContent, fileName);
            }
            else
            {
                ExportToExcel2(Response, strContent, fileName);
            }
        }
        else
        {
            string display = CastAide.AsString(Request["display"]);
            if  (display == "member" || display == "master" || display == "agent")
            {
                reportHeader.Text = reportHeader.Text + "<script>ViewBalance('" + display  + "');</script>";
            }
        }
    }
    private string getHeaderLink()
    {

        string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("DirectBalance/Header.htm");
        //assign header name
        TemplateHandler.Assign(ref tpl, "{DirectBalance}", Resources.Language.DirectBalance);
        TemplateHandler.Assign(ref tpl, "{Refresh}", Resources.Language.refresh);
        TemplateHandler.Assign(ref tpl, "{DirectMaster}", Resources.Language.DirectMaster);
        TemplateHandler.Assign(ref tpl, "{DirectAgent}", Resources.Language.DirectAgent);
        TemplateHandler.Assign(ref tpl, "{DirectMember}", Resources.Language.DirectMember);
        return tpl;
    }
    
    #region ExportToExcel
    
    public void ExportToExcel(HttpResponse Response, string data, string filename)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
        Response.Charset = "utf-8";
        Response.ContentType = "application/vnd.xls";

        StringBuilder content = new StringBuilder();
        content.AppendLine(ReportHTML.CSSExcelReport());
        content.AppendLine(data);
        content.Append("<table><tr><td class='tdhidden'></td></tr></table>");

        content.Replace("href=\"", "href=\"#");
        content.Replace("href='", "href='#");

        Response.Write(content.ToString());
        Response.End();
    }

    private void ExportToExcel2(HttpResponse Response, string data, string filename)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
        Response.Charset = "utf-8";
        Response.ContentType = "application/vnd.xls";

        var excelFile = new ExcelFile();
        var wsReport = excelFile.Worksheets.Add("Sheet1");



        data = data.Replace("&nbsp;", string.Empty);
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(data);

        int currentRow = 0;
        int currentColumn = 0;

        #region header
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Merged = true;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4)
            .Style.Borders.SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Value = GetElementById(xmlDoc.GetElementsByTagName("b"), "bhead1").InnerText;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));

        ++currentRow;

        wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Rows[currentRow].Cells[currentColumn].Value = GetElementById(xmlDoc.GetElementsByTagName("b"), "bhead2").InnerText;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
        wsReport.Columns[currentColumn].Width = 255 * 6;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

        currentColumn = 1;
        wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Rows[currentRow].Cells[currentColumn].Value = GetElementById(xmlDoc.GetElementsByTagName("b"), "bhead3").InnerText;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
        wsReport.Columns[currentColumn].Width = 255 * 18;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

        currentColumn = 2;
        wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Rows[currentRow].Cells[currentColumn].Value = GetElementById(xmlDoc.GetElementsByTagName("b"), "bhead4").InnerText;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
        wsReport.Columns[currentColumn].Width = 255 * 12;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

        currentColumn = 3;
        wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Rows[currentRow].Cells[currentColumn].Value = GetElementById(xmlDoc.GetElementsByTagName("b"), "bhead5").InnerText;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
        wsReport.Columns[currentColumn].Width = 255 * 24;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

        currentColumn = 4;
        wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Rows[currentRow].Cells[currentColumn].Value = GetElementById(xmlDoc.GetElementsByTagName("b"), "bhead6").InnerText;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
        wsReport.Columns[currentColumn].Width = 255 * 24;
        wsReport.Rows[currentRow].Cells[currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;
        #endregion

        #region no information
        if (data.Contains("There is no information available"))
        {
            ++currentRow;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4)
                .Style.Borders.SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Value = "There is no information available";
            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 4).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            goto end;
        }
        #endregion

        #region body
        var tcontentdl = GetElementById(xmlDoc.GetElementsByTagName("table"), "tcontentdl");
        var trRows = tcontentdl.ChildNodes[0].SelectNodes("tr");

        var even = true;
        foreach (XmlNode tr in trRows)
        {
            ++currentRow;
            currentColumn = 0;

            wsReport.Rows[currentRow].Height = 300;

            //--------
            wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentRow].Cells[currentColumn].Value = tr.ChildNodes[currentColumn].SelectNodes("div")[0].InnerText;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            if (even)
            {
                wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
            }
            //--------

            //--------
            currentColumn++;
            wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentRow].Cells[currentColumn].Value = tr.ChildNodes[currentColumn].SelectNodes("div/span")[0].InnerText;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            if (even)
            {
                wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
            }
            //--------

            //--------
            currentColumn++;
            wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentRow].Cells[currentColumn].Value = tr.ChildNodes[currentColumn].SelectNodes("div")[0].InnerText;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            if (even)
            {
                wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
            }
            //--------

            //--------
            currentColumn++;
            wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentRow].Cells[currentColumn].Value = tr.ChildNodes[currentColumn].SelectNodes("div/span")[0].InnerText;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            if (even)
            {
                wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
            }
            //--------

            //--------
            currentColumn++;
            wsReport.Rows[currentRow].Cells[currentColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentRow].Cells[currentColumn].Value = tr.ChildNodes[currentColumn].SelectNodes("div/span")[0].InnerText;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            wsReport.Rows[currentRow].Cells[currentColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            if (even)
            {
                wsReport.Rows[currentRow].Cells[currentColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
            }
            //--------

            even = !even;
        }
        #endregion

        #region footer
        ++currentRow;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3).Merged = true;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3).Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3)
            .Style.Borders.SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3).Value = GetElementById(xmlDoc.GetElementsByTagName("div"), "divFooter1").InnerText;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3).Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(204, 255, 204));

        wsReport.Rows[currentRow].Cells[4].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
            , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
        wsReport.Rows[currentRow].Cells[4].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(204, 255, 204));
        wsReport.Rows[currentRow].Cells[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Rows[currentRow].Cells[4].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Rows[currentRow].Cells[4].Value = GetElementById(xmlDoc.GetElementsByTagName("div"), "divFooterTotalValue").InnerText;
        #endregion

        end:
        excelFile.SaveXls(Response.OutputStream);
    }

    private XmlNode GetElementById(XmlNodeList xmlNodeList, string id)
    {
        foreach (XmlNode xmlNode in xmlNodeList)
        {
            if (xmlNode.Attributes["id"] != null && xmlNode.Attributes["id"].Value == id)
                return xmlNode;
        }
        return null;
    }
    #endregion
}
