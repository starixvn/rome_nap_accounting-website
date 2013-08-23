using System;
using System.Collections;
using System.Text;
using System.Xml;
using DNAide.Data;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess;

public partial class Report_HRAPI : System.Web.UI.Page
{
    private static StringBuilder logs = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadXMLFile();
    }

    private void LoadXMLFile()
    {
        StringBuilder str = new StringBuilder();

        string fileXML = Server.MapPath("HRAPIUserInfo.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(fileXML);
        XmlNodeList list = doc.GetElementsByTagName("user");

        for (int i = 0; i < list.Count; i++)
        {
            XmlNode node = list[i];
            string UserName = "CustId: " + node["username"].InnerText;
            string password = "password: " + node["password"].InnerText;
            string cmty = "cmty: " + node["cmty"].InnerText;
            string exchangeid = "exchangeid: " + node["exchangeid"].InnerText;
            string agent = "agent: " + node["agent"].InnerText;

            //string userinfo = CustId + " | " + password + " | " + cmty + " | " + exchangeid + " | " + agent + "<br>";
            //str.Append(userinfo);
            GetTurnoverByCurrency(node);
        }

        Response.Write(logs.ToString());
    }

    #region //Get turnover by node in xmldocument

    private static void GetTurnoverByCurrency(XmlNode node)
    {
        gpHeader transSec = new gpHeader();
        string strUserName = node["username"].InnerText;
        int exchangeID = Convert.ToInt32(node["exchangeid"].InnerText);
        transSec.userName = strUserName;
        transSec.passWord = node["password"].InnerText;
        transSec.cmty = node["cmty"].InnerText;

        gws amtClass = new gws();
        amtClass.Url = node["url"].InnerText;

        gpInitializeGateway gInit = new gpInitializeGateway();

        #region Init header by input us/pw/cmty and allow channel

        try
        {
            gInit = amtClass.AMTinitializeGateway(transSec);
            if (gInit.errorCode != 0)
            {
                WriteLog(strUserName, "Initialize", gInit.errorCode, gInit.error);
                return;
            }
            gpAllocChannel channel = amtClass.AMTallocChannel(transSec);
            if (channel.errorCode != 0)
            {
                WriteLog(strUserName, "Alloc Channel", channel.errorCode, channel.error);
                return;
            }
            transSec.channel = Convert.ToUInt16(channel.chan.ToString());
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);

            return;
        }

        #endregion Init header by input us/pw/cmty and allow channel

        string[] strListAgent = node["agent"].InnerText.Split(',');
        decimal dcBalance = 0;

        #region Get turnover by each agentid

        for (int j = 0; j < strListAgent.Length; j++)
        {
            string strAgent = strListAgent[j];
            //Get turnover by agent number below currency
            try
            {
                AgentInfo agInfo = new AgentInfo();
                agInfo.agentNumber = strAgent;
                agInfo.agentPassWord = strAgent;

                gpAgentIn agIn = amtClass.AMTagentIn(transSec, agInfo);//Login agent in order to get turnover
                if (agIn.errorCode != 0)
                {
                    WriteLog(strAgent, "AgentIn", agIn.errorCode, agIn.error);
                    TurnoverInsAge(Convert.ToInt32(strAgent), exchangeID, 0, 0, 0, agIn.error, false);
                    continue;
                }
                gpAgentBalance agBalance = amtClass.AMTagentBalance(transSec);//Get turnover
                if (agBalance.errorCode != 0)
                {
                    WriteLog(strAgent, "GetBalance", agBalance.errorCode, agBalance.error);
                    TurnoverInsAge(Convert.ToInt32(strAgent), exchangeID, 0, 0, 0, agBalance.error, false);
                }
                else
                {
                    dcBalance = (decimal)agBalance.agentBalance.betAmount;
                    AgentBalance ag = new AgentBalance();
                    ag = agBalance.agentBalance;
                    TurnoverInsAge(Convert.ToInt32(strAgent), exchangeID, ag.betAmount, ag.betCount, ag.balanceAmount, string.Empty, true);
                }
                gpAgentEnd agEnd = amtClass.AMTagentEnd(transSec);//Logout agent
                if (agEnd.errorCode != 0)
                {
                    WriteLog(strAgent, "AgentEnd", agEnd.errorCode, agEnd.error);
                }
                WriteLog(strAgent, "Get agent turnover", 0, "Successfully!");
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion Get turnover by each agentid

        amtClass.AMTfreeChannel(transSec);//free channel
    }

    #endregion //Get turnover by node in xmldocument

    #region WriteLog

    private static void WriteLog(string AgentID, string Function, int errCode, string errMsg)
    {
        string strErrMsg = errMsg;
        if (errCode != 0)
        {
            strErrMsg = "Error " + errCode.ToString() + ": " + errMsg;
        }
        logs.Append("- Agent: " + AgentID + "--- Function: " + Function + "--- " + strErrMsg + "<br>");
    }

    #endregion WriteLog

    #region TurnoverInsAge

    private static DbResult TurnoverInsAge(int AgentId, int Currency, double BetAmount, int BetCount, double BalanceAmount, string strDesc, bool IsDone)
    {
        Hashtable hsParams = new Hashtable();
        hsParams.Add("agentid", AgentId);
        hsParams.Add("currency", Currency);
        hsParams.Add("betamount", BetAmount);
        hsParams.Add("betcount", BetCount);
        hsParams.Add("balanceamount", BalanceAmount);
        hsParams.Add("isdone", IsDone);
        hsParams.Add("desc", strDesc);
        return DbHelper.Execute("HRAPI_TurnoverInsAg", hsParams, DBType.ARCHIVE, 300);
    }

    #endregion TurnoverInsAge
}