using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DNAide.Utils;
using Fanex.Data;

public partial class Areas_Test_FunctionUsed : System.Web.UI.Page
{
    protected string customerBalanceTableHtml = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        var custId = HttpUtility.UrlDecode(Request.QueryString["custid"]);
        var rawData = GetData(custId);
        customerBalanceTableHtml = String.Concat(RenderData(rawData.Tables[0]), "<br><br>", RenderData1(rawData.Tables[1]));
    }

    private DataSet GetData(string custId)
    {
        var storeProcedureParams = new Hashtable
        {
           { "custid", custId }
        };

        DataSet result = null;
        using (var objDb = new ObjectDb("Acc_BalanceMemberSelectAll"))
        {
            objDb.AssignParamValues(storeProcedureParams);
            result = objDb.ExecuteDataSet();
        }

        return result;
    }



    private string RenderData(DataTable rawData)
    {
        var tableHtml = "<table class=\"tableBalance\" cellspacing=\"0\">";

        var tableColumns = new Dictionary<string, dynamic>();
        tableColumns.Add("username", new { Text = "User Name", Width = "120", DataType = "Text" });
        tableColumns.Add("credit", new { Text = "Credit", Width = "120", DataType = "Number" });
        tableColumns.Add("cashbalance", new { Text = "Cash Balance", Width = "120", DataType = "Number" });
        tableColumns.Add("CreditSports", new { Text = "Credit Sports", Width = "120", DataType = "Number" });
        tableColumns.Add("DebitSports", new { Text = "Debit Sports", Width = "120", DataType = "Number" });
        tableColumns.Add("CreditCasino", new { Text = "Credit Casino", Width = "120", DataType = "Number" });
        tableColumns.Add("DebitCasino", new { Text = "Debit Casino", Width = "120", DataType = "Number" });
        tableColumns.Add("CreditOthers", new { Text = "Credit Others", Width = "120", DataType = "Number" });
        tableColumns.Add("DebitOthers", new { Text = "Debit Others", Width = "120", DataType = "Number" });
        tableColumns.Add("CreditTransfer", new { Text = "Credit Transfer", Width = "120", DataType = "Number" });
        tableColumns.Add("DebitTransfer", new { Text = "Debit Transfer", Width = "120", DataType = "Number" });
        tableColumns.Add("CreditOld", new { Text = "Credit Old", Width = "120", DataType = "Number" });
        tableColumns.Add("DebitOld", new { Text = "Debit Old", Width = "120", DataType = "Number" });
        tableColumns.Add("cashbalanceOld", new { Text = "Cash Balance Old", Width = "120", DataType = "Number" });

        tableHtml += "<thead>";
        tableHtml += "<tr>";
        foreach (var keyValue in tableColumns)
        {
            tableHtml += string.Format(
                "<th width=\"{0}\">{1}</th>",
                keyValue.Value.Width,
                keyValue.Value.Text);
        }

        tableHtml += "</tr>";
        tableHtml += "</thead>";

        var dataCount = rawData.Rows.Count;
        tableHtml += "<tbody>";
        if (dataCount == 0)
        {
            tableHtml += string.Format(
                "<tr><td colspan=\"{0}\" class=\"align-center\">{1}</td>",
                tableColumns.Count,
                "There is no information available.");
        }
        else
        {
            foreach (DataRow dataRow in rawData.Rows)
            {
                tableHtml += "<tr>";
                foreach (var keyValue in tableColumns)
                {
                    tableHtml += string.Format(
                        "<td datatype=\"{0}\">{1}</td>",
                        keyValue.Value.DataType,
                        dataRow[keyValue.Key]);
                }

                tableHtml += "</tr>";
            }
        }

        tableHtml += "</tbody>";
        tableHtml += "</table>";
        return tableHtml;
    }

    private string RenderData1(DataTable rawData)
    {
        var tableHtml = "<table class=\"tableBalance\" cellspacing=\"0\">";

        var tableColumns = new Dictionary<string, dynamic>();
        tableColumns.Add("MatchId", new { Text = "Match Id", Width = "120", DataType = "Number" });
        tableColumns.Add("NPCredit", new { Text = "NP Credit", Width = "120", DataType = "Number" });
        tableColumns.Add("NPDebit", new { Text = "NP Debit", Width = "120", DataType = "Number" });

        tableHtml += "<thead>";
        tableHtml += "<tr>";
        foreach (var keyValue in tableColumns)
        {
            tableHtml += string.Format(
                "<th width=\"{0}\">{1}</th>",
                keyValue.Value.Width,
                keyValue.Value.Text);
        }

        tableHtml += "</tr>";
        tableHtml += "</thead>";

        var dataCount = rawData.Rows.Count;
        tableHtml += "<tbody>";
        if (dataCount == 0)
        {
            tableHtml += string.Format(
                "<tr><td colspan=\"{0}\" class=\"align-center\">{1}</td>",
                tableColumns.Count,
                "There is no information available.");
        }
        else
        {
            foreach (DataRow dataRow in rawData.Rows)
            {
                tableHtml += "<tr>";
                foreach (var keyValue in tableColumns)
                {
                    tableHtml += string.Format(
                        "<td datatype=\"{0}\">{1}</td>",
                        keyValue.Value.DataType,
                        dataRow[keyValue.Key]);
                }

                tableHtml += "</tr>";
            }
        }

        tableHtml += "</tbody>";
        tableHtml += "</table>";
        return tableHtml;
    }
}