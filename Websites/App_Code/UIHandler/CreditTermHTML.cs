namespace Sunplus.Accounting.Presentation
{
    using DNAide.Core;
    using DNAide.Web;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Web;
    using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
    using Sunplus.Accounting.Entities;
    using Sunplus.Accounting.WcfServiceLibrary.CreditTerm;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Linq;

    /// <summary>
    /// Summary description for CreditTermHTML
    /// </summary>
    public class CreditTermHTML
    {
        public static string html = string.Empty;

        #region Credit Term

        public static string GetContentCreditTermFilter(HttpContext context, string groupMaxLoss)
        {
            html = WebUtils.LoadTemplate("CreditTeam/Creditterm_Group_Feed_Form.htm");

            string result = TemplateHandler.CopyBlock(html, "Filter");
            int groupId = 0;

            if (!string.IsNullOrEmpty(context.Request["groupid"]))
            {
                groupId = Convert.ToInt32(context.Request["groupid"]);
                AppSession.CreditTermGroupID = groupId;
            }
            else
            {
                groupId = AppSession.CreditTermGroupID;
            }

            TemplateHandler.Assign(ref result, "{groupid}", groupId.ToString());
            TemplateHandler.Assign(ref result, "{GroupHeadlineMaxLoss}", groupMaxLoss);

            return result;
        }

        public static string GetContentCreditTerm(HttpContext context, ref string groupMaxLoss)
        {
            html = WebUtils.LoadTemplate("CreditTeam/Creditterm_Group_Feed_Form.htm");
            string from = html;

            try
            {
                TemplateHandler.RemoveBlock(ref from, "Filter");
                int groupId = 0;

                if (!string.IsNullOrEmpty(context.Request["groupid"]))
                {
                    groupId = Convert.ToInt32(context.Request["groupid"]);
                    AppSession.CreditTermGroupID = groupId;
                }
                else
                {
                    groupId = AppSession.CreditTermGroupID;
                }

                string username = context.Request["username"];
                int userid = AppSession.AccId;
                string listname = context.Request["lstlistname"];
                int type = CastAide.AsInt32(context.Request["type"]);
                //type:
                // 1 - Add this user to group >>
                // 2 - Set Selected User to Headline
                // 3 - Remove Selected Users from This Group

                string username_selected = string.Empty, custid_selected = string.Empty, msg = string.Empty;
                int count_normal_user_selected = 0;

                if (type == 1)
                {
                    AddUsersToGroup(ref from, groupId, username, userid);
                }
                else if (type == 3) // 3 - Remove Selected Users from This Group
                {
                    RemoveSelectedUsersFromGroup(ref from, groupId, userid, listname, ref username_selected, ref custid_selected, ref msg, ref count_normal_user_selected);
                }
                else if (type == 2) // 2 - Set Selected User to Headline
                {
                    SetSelectedUserToBeGroupHeadline(ref from, groupId, userid, listname);
                }
                else
                {
                    TemplateHandler.Assign(ref from, "{msg}", "");
                }

                if (type > 0)
                {
                    var newThread = new Thread(ThreadMethod);
                    newThread.Start();
                    Thread.Sleep(2000);
                }

                var creditTermService = new CreditTermService();
                var memberList = creditTermService.GetMemberByGroupId(groupId);

                string userrow = TemplateHandler.CopyBlock(from, "UserRow");
                TemplateHandler.RemoveBlock(ref from, "UserRow");

                var userrows = new StringBuilder();
                string headline = string.Empty;
                string _userrow = string.Empty;
                string headlineclass = string.Empty;
                string headlineclasstext = string.Empty;
                string removeitem = string.Empty;
                string jsheadline = string.Empty;
                string title = string.Empty;
                string roweven = "evenrow";
                int index = 0;

                foreach (var member in memberList)
                {
                    if (member.CustId > 0)
                    {
                        _userrow = (string)userrow.Clone();

                        if (member.IsGroupHeadline)
                        {
                            jsheadline = string.Empty;
                            headline = "(HEAD LINE)";
                            headlineclass = "../App_Themes/Images/headlineH.gif";
                            headlineclasstext = "headlinetext";
                            removeitem = string.Empty;
                            title = "Headline user";
                        }
                        else
                        {
                            title = "Set this user to headline";
                            headlineclass = "../App_Themes/Images/headline.gif";
                            headlineclasstext = string.Empty;
                            headline = string.Empty;
                            removeitem = "<input type='checkbox' value='" + member.CustId + "|" + member.Username +
                                         "' name='chkuser' id='chkuser' />";
                            jsheadline = "ExecuteCreditTerm('2','" + member.CustId + "');";
                        }

                        roweven = roweven == string.Empty ? "evenrow" : string.Empty;

                        TemplateHandler.Assign(ref _userrow, "{row-even}", roweven);
                        TemplateHandler.Assign(ref _userrow, "{No}", ++index);
                        TemplateHandler.Assign(ref _userrow, "{CustId}", member.Username + " " + headline);
                        TemplateHandler.Assign(ref _userrow, "{UserName}", member.Username + " " + headline);
                        TemplateHandler.Assign(ref _userrow, "{headline}", headlineclass);
                        TemplateHandler.Assign(ref _userrow, "{Currency}", member.Currency);
                        TemplateHandler.Assign(
                            ref _userrow,
                            "{TotalBalance}",
                            member.TotalBalance.StartsWith("-") ?
                                string.Format("<span style=\"color:#ee0000\">{0}</span>", member.TotalBalance) :
                                member.TotalBalance);
                        TemplateHandler.Assign(ref _userrow, "{MaxLoss}", member.MaxLoss);
                        TemplateHandler.Assign(ref _userrow, "{headlineclasstext}", headlineclasstext);
                        TemplateHandler.Assign(ref _userrow, "{removeitem}", removeitem);
                        TemplateHandler.Assign(ref _userrow, "{jsaddheadline}", jsheadline);
                        TemplateHandler.Assign(ref _userrow, "{title}", title);

                        userrows.Append(_userrow);
                    }
                    else
                    {
                        TemplateHandler.Assign(
                            ref from,
                            "{SumTotalBalance}",
                            member.TotalBalance.StartsWith("-") ?
                                string.Format("<span style=\"color:#ee0000\">{0}</span>", member.TotalBalance) :
                                member.TotalBalance);

                        groupMaxLoss = member.MaxLoss;
                    }
                }

                TemplateHandler.Assign(ref from, "{UserRowList}", userrows.ToString());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                TemplateHandler.Assign(ref from, "{UserRowList}", "<tr><td colspan='3' class='c'>" + ex.Message + "</td></tr>");
            }

            return from;
        }

        private static void RemoveSelectedUsersFromGroup(ref string from, int groupid, int userid, string listname, ref string username_selected, ref string custid_selected, ref string msg, ref int count_normal_user_selected)
        {
            var stringseparator = new[] { "," };

            foreach (string item in listname.Split(stringseparator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (item != string.Empty)
                {
                    username_selected = item.Split('|')[1];
                    custid_selected = item.Split('|')[0];
                    var rs = AccountCredit.Acc_CreditTermUpdRemoveFromGroup(custid_selected, groupid, userid);

                    if (rs != null)
                    {
                        if (rs.ErrorCode != 0)
                        {
                            msg = "<script>showAlertMessage('errMsg','" + rs.ErrorMessage +
                                  "',1);</script>";
                        }
                        else
                        {
                            msg += username_selected + ", ";
                            count_normal_user_selected++;
                        }
                    }
                }
            }

            if (msg.LastIndexOf(",") > 0)
            {
                msg = msg.Substring(0, msg.Length - 2);
            }

            if (count_normal_user_selected > 0)
            {
                msg += " has been removed.";
                msg = "<script>showAlertMessage('errMsg','" + msg + "',0);</script>";
            }

            TemplateHandler.Assign(ref from, "{msg}", msg);
        }

        private static void SetSelectedUserToBeGroupHeadline(ref string from, int groupid, int userid, string listname)
        {
            var rs = AccountCredit.Acc_CreditTermUpdSetGroupHeadline(Convert.ToInt32(listname), groupid,
                                       userid);
            if (rs != null)
            {
                if (rs.ErrorCode != 0)
                {
                    TemplateHandler.Assign(ref from, "{msg}",
                                           "<script>showAlertMessage('errMsg','" +
                                           rs.ErrorMessage + "',1);</script>");
                }
                else
                {
                    TemplateHandler.Assign(ref from, "{msg}",
                                           "<script>showAlertMessage('errMsg','Group Headline has been changed.',0);</script>");
                }
            }
        }

        private static void AddUsersToGroup(ref string from, int groupid, string usernames, int userid)
        {
            StringBuilder errorMsg = new StringBuilder();
            StringBuilder successMsg = new StringBuilder();
            if (ValidateUserNameInput(ref from, usernames))
            {
                List<string> listUsername = usernames.Split(',').Select(m => m.Trim()).ToList();
                foreach (string username in listUsername)
                {
                    var result = AccountCredit.Acc_CreditTermUpdAddToGroup(username, groupid, userid);
                    if (result.ErrorCode != 0)
                    {
                        errorMsg.Append(username + ": " + result.ErrorMessage + "<br />");
                    }
                    else
                    {
                        successMsg.Append(username + ", ");
                    }
                }

                successMsg.Length -= successMsg.Length > 2 ? 2 : 0;
                if (errorMsg.Length == 0)
                {
                    TemplateHandler.Assign(ref from, "{msg}",
                                                  "<script>showAlertMessage('errMsg','All accounts have been successfully added.',0);</script>");
                }
                else if (successMsg.Length > 0)
                {
                    TemplateHandler.Assign(ref from, "{msg}",
                                               "<script>showAlertMessage('errMsg','" + string.Format("Successfully added accounts: {0}.", successMsg.ToString()) + "',0);"
                                               + "showAlertMessage('errMsg1', '" + errorMsg.ToString() + "', 1);</script>");
                }
                else
                {
                    TemplateHandler.Assign(ref from, "{msg}",
                                                  "<script>showAlertMessage('errMsg','" + errorMsg.ToString() + "',1);</script>");
                }
            }
        }

        private static bool ValidateUserNameInput(ref string from, string usernames)
        {
            if (string.IsNullOrWhiteSpace(usernames.Trim()))
            {
                TemplateHandler.Assign(ref from, "{msg}",
                    "<script>showAlertMessage('errMsg','Please enter Username.',1);</script>");
                return false;
            }

            if (usernames.Contains("\r")
                || usernames.Contains("\n")
                || usernames.Contains("\\"))
            {
                TemplateHandler.Assign(ref from, "{msg}",
                    "<script>showAlertMessage('errMsg','You are not allowed to input [Enter] and backslash &apos;&#92;&apos; into the Usernames field.',1);</script>");
                return false;
            }

            if (usernames.Split(',').Select(m => m.Trim()).ToList().Count > 50)
            {
                TemplateHandler.Assign(ref from, "{msg}",
                    "<script>showAlertMessage('errMsg','The function can only create up to 50 accounts per request.',1);</script>");
                return false;
            }

            return true;
        }

        #endregion

        private static void ThreadMethod()
        {
            Thread.Sleep(10);
        }
    }
}
