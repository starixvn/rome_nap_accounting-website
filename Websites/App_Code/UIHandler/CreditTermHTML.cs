using Fanex.Common.Extensions;

namespace Sunplus.Accounting.Presentation
{
    using DNAide.Core;
    using DNAide.Web;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Web;
    using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
    using Sunplus.Accounting.WcfServiceLibrary.CreditTerm;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;

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
            var hasTransferPermission = Permissions.GetHostPermission(Enumerations.Functions.Transfer.OtherTrans);
            string formHtml = html;

            try
            {
                TemplateHandler.RemoveBlock(ref formHtml, "Filter");
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
                if (type > 0)
                {
                    if (!Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust))
                    {
                        TemplateHandler.Assign(ref formHtml, "{msg}",
                            "<script>showAlertMessage('errMsg',\"You don\'t have permission to access\",1);</script>");
                    }
                    else
                    {
                        if (type == 1)
                        {
                            AddUsersToGroup(ref formHtml, groupId, username, userid);
                        }
                        else if (type == 3) // 3 - Remove Selected Users from This Group
                        {
                            RemoveSelectedUsersFromGroup(ref formHtml, groupId, userid, listname, ref username_selected, ref custid_selected, ref msg, ref count_normal_user_selected);
                        }
                        else if (type == 2) // 2 - Set Selected User to Headline
                        {
                            SetSelectedUserToBeGroupHeadline(ref formHtml, groupId, userid, listname);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref formHtml, "{msg}", "");
                        }

                        Thread.Sleep(2000);
                    }
                }

                var creditTermService = new CreditTermService();
                var memberList = creditTermService.GetMemberByGroupId(groupId);

                int totalmember = memberList.Count;
                if (totalmember <= 1)
                {
                    TemplateHandler.RemoveBlock(ref formHtml, "DataContent");
                    TemplateHandler.Assign(ref formHtml, "{scriptRedirectWhenNoUser}", "<script>RedirectToGroupList();</script>");
                }
                else
                {
                    TemplateHandler.Assign(ref formHtml, "{scriptRedirectWhenNoUser}", string.Empty);
                    string userrow = TemplateHandler.CopyBlock(formHtml, "UserRow");
                    TemplateHandler.RemoveBlock(ref formHtml, "UserRow");

                    var userrows = new StringBuilder();
                    string headline = string.Empty;
                    string _userrow = string.Empty;
                    string headlineclass = string.Empty;
                    string headlineclasstext = string.Empty;
                    string removeitem = string.Empty;
                    string jsheadline = string.Empty;
                    string title = string.Empty;
                    string roweven = "evenrow";
                    string jsTranferPopup = string.Empty;
                    int index = 0;
                    var removeUserFunction = "ExecuteCreditTerm('3', 0, false)";

                    foreach (var member in memberList)
                    {
                        if (member.CustId > 0)
                        {
                            _userrow = (string)userrow.Clone();

                            if (hasTransferPermission)
                            {
                                jsTranferPopup = "OpenPopupTransfer" + "('1','" + member.YesterdayBalance + "','" + member.Username + "');";
                                TemplateHandler.Assign(ref _userrow, "{canClick}", "canClick");
                            }
                            if (member.IsGroupHeadline)
                            {
                                TemplateHandler.Assign(ref formHtml, "{headlineName}", member.Username);
                                jsheadline = string.Empty;
                                headline = "(HEAD LINE)";
                                headlineclass = "../App_Themes/Images/headlineH.gif";
                                headlineclasstext = "headlinetext";
                                if (totalmember == 2)
                                {
                                    removeitem = "<input type='checkbox' value='" + member.CustId + "|" + member.Username +
                                            "' name='chkuser' id='chkuser' />";
                                    removeUserFunction = "ExecuteCreditTerm('3', 0, true)";
                                }
                                else
                                {
                                    removeitem = string.Empty;
                                }

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
                                jsheadline = "ExecuteCreditTerm('2','" + member.CustId + "|" + member.Username + "');";
                            }

                            roweven = roweven == string.Empty ? "evenrow" : string.Empty;

                            TemplateHandler.Assign(ref _userrow, "{row-even}", roweven);
                            TemplateHandler.Assign(ref _userrow, "{No}", ++index);
                            TemplateHandler.Assign(ref _userrow, "{CustId}", member.Username + " " + headline);
                            TemplateHandler.Assign(ref _userrow, "{OnlyUserName}", member.Username);
                            TemplateHandler.Assign(ref _userrow, "{UserName}", member.Username + " " + headline);
                            TemplateHandler.Assign(ref _userrow, "{headline}", headlineclass);
                            TemplateHandler.Assign(ref _userrow, "{Currency}", member.Currency);
                            TemplateHandler.Assign(
                               ref _userrow,
                               "{YesterdayTotalBalance}",
                               member.TotalBalance.StartsWith("-") ?
                                   string.Format("<span style=\"color:#ee0000\">{0}</span>", member.YesterdayBalance) :
                                   member.YesterdayBalance);
                            TemplateHandler.Assign(ref _userrow, "{YesterdayBalanceNoneFormat}", member.YesterdayBalance);
                            TemplateHandler.Assign(ref _userrow, "{jsTranferPopup}", jsTranferPopup);

                            TemplateHandler.Assign(
                                ref _userrow,
                                "{TotalBalance}",
                                member.TotalBalance.StartsWith("-") ?
                                    string.Format("<span style=\"color:#ee0000\">{0}</span>", member.TotalBalance) :
                                    member.TotalBalance);

                            TemplateHandler.Assign(
                            ref _userrow,
                            "{PendingTranfer3rd}",
                            member.PendingTransfer3rd.StartsWith("-") ?
                                string.Format("<span style=\"color:#ee0000\">{0}</span>", member.PendingTransfer3rd) :
                                member.PendingTransfer3rd);

                            TemplateHandler.Assign(ref _userrow, "{TotalBalanceNoneFormat}", member.TotalBalance);
                            TemplateHandler.Assign(ref _userrow, "{PendingTranfer3rdNoneFormat}", member.PendingTransfer3rd);
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
                                ref formHtml,
                                "{SumYesterdayTotalBalance}",
                                member.YesterdayBalance.StartsWith("-") ?
                                    string.Format("<span style=\"color:#ee0000\">{0}</span>", member.YesterdayBalance) :
                                    member.YesterdayBalance);
                            TemplateHandler.Assign(
                                ref formHtml,
                                "{SumTotalBalance}",
                                member.TotalBalance.StartsWith("-") ?
                                    string.Format("<span style=\"color:#ee0000\">{0}</span>", member.TotalBalance) :
                                    member.TotalBalance);
                            TemplateHandler.Assign(
                               ref formHtml,
                               "{SumPendingTranfer3rd}",
                               member.PendingTransfer3rd.StartsWith("-") ?
                                   string.Format("<span style=\"color:#ee0000\">{0}</span>", member.PendingTransfer3rd) :
                                   member.PendingTransfer3rd);

                            groupMaxLoss = member.MaxLoss;
                        }
                    }
                    TemplateHandler.Assign(ref formHtml, "{removeUserFunction}", removeUserFunction);
                    TemplateHandler.Assign(ref formHtml, "{UserRowList}", userrows.ToString());
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                TemplateHandler.Assign(ref formHtml, "{UserRowList}", "<tr><td colspan='3' class='c'>" + ex.Message + "</td></tr>");
            }

            return formHtml;
        }

        private static void RemoveSelectedUsersFromGroup(ref string formHtml, int groupid, int userid, string listname, ref string username_selected, ref string custid_selected, ref string msg, ref int count_normal_user_selected)
        {
            var stringseparator = new[] { "," };
            var affectedSupers = new List<string>();
            var username = "";

            foreach (string item in listname.Split(stringseparator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (item != string.Empty)
                {
                    username_selected = item.Split('|')[1];
                    custid_selected = item.Split('|')[0];
                    var rs = AccountCredit.Acc_CreditTermUpdRemoveFromGroup(custid_selected, groupid, userid);

                    if (rs != null)
                    {
                        affectedSupers.Add(rs.AffectedSupers);
                        if (rs.ErrorCode != 0)
                        {
                            msg = "<script>showAlertMessage('errMsg','" + rs.ErrorMessage +
                                  "',1);</script>";
                        }
                        else
                        {
                            username += username_selected + ", ";
                            count_normal_user_selected++;
                        }
                    }
                }
            }

            username = username.Trim().Trim(',');
            affectedSupers.RemoveAll(super => super == null || super == string.Empty);

            if (count_normal_user_selected > 0)
            {
                if (affectedSupers.Count > 0)
                {
                    msg += "The selected supers have been successfully removed from the Headline Group. The following supers are enqueued to be removed from their old External Groups at 0:00 on tomorrow: " + String.Join(",", affectedSupers);
                }
                else
                {
                    msg += username + " has been removed.";
                }
                msg = "<script>showAlertMessage('errMsg','" + msg + "',0);</script>";
            }

            TemplateHandler.Assign(ref formHtml, "{msg}", msg);
        }

        private static void SetSelectedUserToBeGroupHeadline(ref string formHtml, int groupid, int userid, string listname)
        {
            var rs = AccountCredit.Acc_CreditTermUpdSetGroupHeadline(Convert.ToInt32(listname), groupid,
                                       userid);
            if (rs != null)
            {
                if (rs.ErrorCode != 0)
                {
                    TemplateHandler.Assign(ref formHtml, "{msg}",
                                           "<script>showAlertMessage('errMsg','" +
                                           rs.ErrorMessage + "',1);</script>");
                }
                else
                {
                    TemplateHandler.Assign(ref formHtml, "{msg}",
                                           "<script>showAlertMessage('errMsg','Group Headline has been changed.',0);</script>");
                }
            }
        }

        private static void AddUsersToGroup(ref string formHtml, int groupid, string usernames, int userid)
        {
            StringBuilder errorMsg = new StringBuilder();
            StringBuilder successMsg = new StringBuilder();
            var affectedSuper = new List<string>();

            if (ValidateUserNameInput(ref formHtml, usernames))
            {
                List<string> listUsername = usernames.Split(',').Select(m => m.Trim()).ToList();
                foreach (string username in listUsername)
                {
                    var result = AccountCredit.Acc_CreditTermUpdAddToGroup(username, groupid, userid);
                    if (result.ErrorCode != 0)
                    {
                        if (result.ErrorCode == 101)
                        {
                            result.ErrorMessage = "You don\'t have permission to access";
                        }

                        errorMsg.Append(username + ": " + result.ErrorMessage + "<br />");
                    }
                    else
                    {
                        successMsg.Append(username + ", ");
                    }
                    affectedSuper.AddRange(result.AffectedSupers.Split(',').Select(m => m.Trim()));
                }
                affectedSuper.RemoveAll(sp => sp.IsNullOrTrimEmpty());
                var affectedSupers = string.Join(",", affectedSuper.Distinct());
                successMsg.Length -= successMsg.Length > 2 ? 2 : 0;
                if (affectedSupers.Length > 0)
                {
                    if (errorMsg.Length == 0)
                    {
                        TemplateHandler.Assign(ref formHtml, "{msg}",
                                              "<script>showAlertMessage('errMsg','" + string.Format("The selected supers have been successfully added. The following supers are enqueued to be removed from their old External Groups on 0:00 tomorrow: {0}.", affectedSupers) + "',0);</script>");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref formHtml, "{msg}",
                                               "<script>showAlertMessage('errMsg','" + string.Format("The selected supers have been successfully added. The following supers are enqueued to be removed from their old External Groups on 0:00 tomorrow: {0}.", affectedSupers) + "',0);"
                                               + "showAlertMessage('errMsg1', \"" + errorMsg + "\", 1);</script>");
                    }
                }
                else if (errorMsg.Length == 0)
                {
                    TemplateHandler.Assign(ref formHtml, "{msg}",
                                                  "<script>showAlertMessage('errMsg','All accounts have been successfully added.',0);</script>");
                }
                else if (successMsg.Length > 0)
                {
                    TemplateHandler.Assign(ref formHtml, "{msg}",
                                               "<script>showAlertMessage('errMsg','" + string.Format("Successfully added accounts: {0}.", successMsg) + "',0);"
                                               + "showAlertMessage('errMsg1', \"" + errorMsg + "\", 1);</script>");
                }
                else
                {
                    TemplateHandler.Assign(ref formHtml, "{msg}",
                                                  "<script>showAlertMessage('errMsg',\"" + errorMsg + "\",1);</script>");
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

            if (usernames.Split(',').Select(m => m.Trim()).ToList().Count > 500)
            {
                TemplateHandler.Assign(ref from, "{msg}",
                    "<script>showAlertMessage('errMsg','The function can only create up to 500 accounts per request.',1);</script>");
                return false;
            }

            return true;
        }

        #endregion Credit Term
    }
}