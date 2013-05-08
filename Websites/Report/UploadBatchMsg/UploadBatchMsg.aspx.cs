using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Data;
using System.Data;
using System.Collections;
using DNAide.Core;
using Sunplus.Accounting.Business.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;


public partial class Report_UploadBatchMsg_UploadBatchMsg : PageBase
{
    public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["txt"] == "upload")
        {
            string msgEn = CastAide.AsString(Request["txtEn"]);
            string msgCs = CastAide.AsString(Request["txtCs"]);
            string msgCh = CastAide.AsString(Request["txtCh"]);

            if (msgEn == "" || msgCs == "")
                msg = AlertMessage.GetMsg("Please fill in the content.", AlertMessage.MsgType.Warning);
            else
                UploadMsg(msgEn, msgCs, msgCh);
        }
    }

    void UploadMsg(string msgEn, string msgCs, string msgCh)
    {
        try
        {
            #region send private message to users
            DbResult rs = DbHelper.Execute("DBA_SelBet_BatchMsg", DBType.REPORT);
            bool IsSuccess = false;
            int cnt = 0;
            DataTable dtMsg = new DataTable();
            if (rs.ResultSet.Tables.Count > 0)
            {
                dtMsg = rs.ResultSet.Tables[0];
                if (dtMsg.Rows.Count > 0)
                {
                    Hashtable list = new Hashtable();

                    for (int i = 0; i < dtMsg.Rows.Count; i++)
                    {

                        string[] ArrColumnsNameDownLineID = { "custid", "recommend", "mrecommend", "srecommend" };
                        string[] ArrColumnsNameDownLineUserName = { "CustUserName", "AgUserName", "MagUserName", "SagUserName" };

                        for (int j = 0; j < ArrColumnsNameDownLineID.Length; j++)
                        {
                            string cnameID = ArrColumnsNameDownLineID[j];
                            string cnameUser = ArrColumnsNameDownLineUserName[j];

                            string custid = dtMsg.Rows[i][cnameID].ToString();
                            string uname = dtMsg.Rows[i][cnameUser].ToString();

                            string key = custid + "^" + uname;
                            string ticknum = dtMsg.Rows[i]["transid"].ToString();

                            if (list[key] != null)
                            {
                                if (list[key].ToString().IndexOf(ticknum) == -1)
                                {
                                    string _tmp = list[key].ToString();
                                    list.Remove(key);
                                    list.Add(key, _tmp + "," + ticknum);
                                }
                            }
                            else
                                list.Add(key, ticknum);
                        }
                    }

                    DataTable dtDeletedBet = new DataTable();
                    dtDeletedBet.Columns.Add("custid", typeof(int));
                    dtDeletedBet.Columns.Add("username", typeof(string));
                    dtDeletedBet.Columns.Add("transids", typeof(string));
                    dtDeletedBet.Columns.Add("betcount", typeof(int));

                    foreach (DictionaryEntry Key in list)
                    {
                        int custid = CastAide.AsInt32(Key.Key.ToString().Split('^')[0]); // This is a CustID of ticket that is deleted [TargetID Field]
                        string uname = Key.Key.ToString().Split('^')[1];//This is a CustId of ticket that is deleted [TargetName Field]

                        DataRow dr = dtDeletedBet.NewRow();
                        dr["custid"] = custid;
                        dr["username"] = uname;
                        string[] ArrTransids = Key.Value.ToString().Split(',');
                        Array.Sort(ArrTransids);
                        string strTransIDs = "";
                        for (int i = 0; i < ArrTransids.Length; i++)
                        {
                            strTransIDs += ArrTransids[i] + ", ";
                        }
                        strTransIDs = strTransIDs.Substring(0, strTransIDs.Length - 2);
                        dr["transids"] = strTransIDs;
                        dr["betcount"] = Key.Value.ToString().Split(',').Length;
                        dtDeletedBet.Rows.Add(dr);
                    }
                    DataView dv = dtDeletedBet.DefaultView;
                    dv.Sort = "betcount asc, transids asc";
                    DataTable dtBets = dv.ToTable();

                    //Set batch Message
                    Message pmsg = new Message();
                    pmsg.Content_en = msgEn;
                    pmsg.Content_ch = msgCh;
                    pmsg.Content_cs = msgCs;
                    pmsg.Content_it = "";
                    pmsg.Content_jp = "";
                    pmsg.Content_th = "";
                    pmsg.Content_es = "";

                    pmsg.CreatorId = Convert.ToInt32(AppSession.AccId);
                    pmsg.CreatorName = AppSession.AccUserName;
                    string strTransID_Current = "", strTransID_Previous = "";
                    if (dtBets.Rows.Count > 0)
                        strTransID_Current = CastAide.AsString(dtBets.Rows[0]["transids"]);
                    int CountBetsBelong1Member = CastAide.AsInt32(dtBets.Compute("count(transids)", "transids='" + strTransID_Current + "'"));
                    bool IsAllBetsBelong1Member = dtBets.Rows.Count == CountBetsBelong1Member;
                    int BatchId = 0;
                    //if(!IsAllBetsBelong1Member) BatchId = Messenger.SetBatchMessage(pmsg);

                    // send private message to users for cancel their transactions                                                                                                                
                    int MessageId = 0;
                    int matchId = 0;
                    foreach (DataRow dr in dtBets.Rows)
                    {
                        int[] custid = { CastAide.AsInt32(dr["custid"]) }; // This is a CustID of ticket that is deleted [TargetID Field]
                        string[] uname = { CastAide.AsString(dr["username"]) };//This is a CustId of ticket that is deleted [TargetName Field]

                        if (CastAide.AsInt32(dr["custid"]) < 100) continue;//SRecommend < 100 and roleid=3 
                        string[] bettransid = { CastAide.AsString(dr["transids"]) };

                        if (MessageId == 0)
                            MessageId = Messenger.SetMessage(pmsg, custid, uname, matchId, bettransid);
                        else
                            Messenger.AddTargt2Message(MessageId, custid, uname, bettransid);

                        IsSuccess = true;
                        cnt++;
                    }
                }
            }
            #endregion

                if(IsSuccess)
                    msg = (AlertMessage.GetMsg("Update Batch Msg successfully!, " + cnt.ToString() + " rows effected", AlertMessage.MsgType.Success));
                    //Response.Write("<span style='color:blue;font-weight:bold'>Update Batch Msg successfully!, " + cnt.ToString() + " rows effected </span>");
                else
                    msg = (AlertMessage.GetMsg("Have a problem while updating batch MSG!, " + cnt.ToString() + " rows effected. Return " + dtMsg.Rows.Count.ToString() + " row(s) from sp DBA_SelBet_BatchMsg", AlertMessage.MsgType.Error));
        }
        catch (Exception ex)
        {
           msg = (AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error ));
        }
    }
}
