using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

//using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for FIAutoDummyTranfer
/// </summary>
public partial class ReportHTML
{
    #region AutoDummyTransfer_Finance

    public static void AutoDummyTransfer_Finance()
    {
        try
        {
            DbResult rs = MatchWinLostReport.AutoDummyTransfer_Finance();
            int errcode = CastAide.AsInt32(rs.Outputs["errcode"]);

            if (errcode == 0) return;

            string msg = "";
            var dt = new DataTable();
            string tpl = WebUtils.LoadTemplate("WinLost/FIMailAutoDummyTranfer.htm");
            string tpltable2 = TemplateHandler.CopyBlock(tpl, "table2");
            TemplateHandler.RemoveBlock(ref tpl, "table2");
            var rows_table2 = new StringBuilder();
            switch (errcode)
            {
                case 101:
                    if (rs.ResultSet.Tables.Count > 0)
                    {
                        dt = rs.ResultSet.Tables[0];
                        msg = rs.Outputs["errmsg"] + " with transids: ";
                        foreach (DataRow dr in dt.Rows)
                        {
                            msg += dr["transid"] + ", ";
                        }
                    }
                    break;

                case 102:
                    if (rs.ResultSet.Tables.Count > 0)
                    {
                        dt = rs.ResultSet.Tables[0];
                        msg = rs.Outputs["errmsg"] + ":";
                        string tplcontent_table2 = TemplateHandler.CopyBlock(tpltable2, "content-table2");
                        string row = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = tplcontent_table2;
                            TemplateHandler.Assign(ref row, "TransID", dr["TransID"]);
                            TemplateHandler.Assign(ref row, "Betteam", dr["Betteam"]);
                            TemplateHandler.Assign(ref row, "Hdp1", dr["Hdp1"]);
                            TemplateHandler.Assign(ref row, "Hdp2", dr["Hdp2"]);
                            TemplateHandler.Assign(ref row, "Status", dr["Status"]);
                            rows_table2.Append(row);
                        }
                        TemplateHandler.ReplaceBlock(ref tpltable2, "content-table2", rows_table2.ToString());
                        msg += tpltable2;
                    }
                    break;

                case 103:
                    if (rs.ResultSet.Tables.Count > 0)
                    {
                        dt = rs.ResultSet.Tables[0];
                        msg = rs.Outputs["errmsg"] + " with the currencies: ";
                        foreach (DataRow dr in dt.Rows)
                        {
                            msg += dr["currency"] + ", ";
                        }
                    }
                    break;
            }

            TemplateHandler.Assign(ref tpl, "content", msg);

            if (errcode > 0)
            {
                if (msg == "")
                    msg = "Auto dummy transfer of Financial Product has encountered";
                string sContentBefore = ReadLogAutoDummyTransfer();
                if (msg != sContentBefore)
                {
                    WriteLogAutoDummyTransfer(msg);
                    string subject = "Auto dummy transfer of Financial Product has encountered";
                    TemplateHandler.Assign(ref tpl, "datetime", DateTime.Now.ToString());
                    bool f = SendMailService.SendMail2(MailSection.MailServer, MailSection.ToMailFIAutoDummyTransfer,
                                                       MailSection.FromMail, MailSection.CCMailFIAutoDummyTransfer,
                                                       MailSection.BCCMailFIAutoDummyTransfer, subject, tpl);
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    private static void WriteLogAutoDummyTransfer(string content)
    {
        string path = HttpContext.Current.Server.MapPath("Logs"); // +"/LogMailSpecialDelete.html";
        int index = path.IndexOf("Report");
        string root = path.Substring(0, index - 1) + "\\Logs\\LogAutoDummyTransfer.html";

        // Specify file, instructions, and privelegdes
        using (var file = new FileStream(root, FileMode.Create, FileAccess.Write))
        {
            // Create a new stream to write to the file
            var sw = new StreamWriter(file);

            // Write a string to the file
            sw.WriteLine(content);

            // Close StreamWriter
            sw.Close();

            // Close file
            file.Close();
        }
    }

    private static string ReadLogAutoDummyTransfer()
    {
        string sContent = "";
        string path = HttpContext.Current.Server.MapPath("Logs");
        int index = path.IndexOf("Report");
        string root = path.Substring(0, index - 1) + "\\Logs\\LogAutoDummyTransfer.html";

        if (!File.Exists(root))
            return sContent;

        using (var file = new FileStream(root, FileMode.Open, FileAccess.Read))
        {
            var sw = new StreamReader(file);
            //why do not use readtoend????
            sContent = sw.ReadLine();
            sw.Close();
            file.Close();
        }

        return sContent;
    }

    #endregion AutoDummyTransfer_Finance
}