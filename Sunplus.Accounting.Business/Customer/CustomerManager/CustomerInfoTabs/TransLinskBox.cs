namespace Sunplus.Accounting.Business.Customer
{
    using System.Web;
    using Core.Common;
    using Core.Configurations;
    using Core.Web;
    using DNAide.Web;
    using Security;

    public partial class CustomerManager
    {
        private static string GenTransLinskBox(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            string tplTransLink = WebUtils.LoadTemplateBySystem("CustomerInfo/TransLinksBox.htm");
            string downlineListURL = string.Empty;
            string customerBalanceURL = string.Empty;
            string lblDownlineList = string.Empty;
            Globals.AccountType accLevel = (Globals.AccountType)customerInfoViewModel.RoleId;

            string statementLink = string.Empty;
            if (!customerInfoViewModel.IsInternalUser)
            {
                statementLink = "Statement/CustomerStatement.aspx?custname={custname}&isinternal=" + customerInfoViewModel.IsInternalUser;
            }
            else
            {
                statementLink = "CustomerStatement/Index?CustID=" + customerInfoViewModel.CustId + "&CustName={custname}";
            }
            TemplateHandler.Assign(ref tplTransLink, "statementLink", statementLink);

            bool enableDeposit = WebSettings.IsApplySplitingDepositeCustomer;
            if (enableDeposit && customerInfoViewModel.IsDeposit && accLevel != Globals.AccountType.Member)
            {
                TemplateHandler.RemoveBlock(ref tplTransLink, "downlinelistbtn");
            }

            switch (accLevel)
            {
                case Globals.AccountType.Super:
                    if (!customerInfoViewModel.IsInternalUser)
                    {
                        downlineListURL = "Customer/MasterAgentList.aspx?superId=" + customerInfoViewModel.CustId; 
                        if (WebSettings.OfferDoubleComm)
                        {
                            downlineListURL += "&doubleCommSuper=" + customerInfoViewModel.Doublecommlosing.ToString().ToLower();
                        }
                    }
                    else
                    {
                        downlineListURL = "FinanceCustomerList/Index?upline=" + customerInfoViewModel.CustId + "&roleid=4";
                    }

                    customerBalanceURL = "CustomerBalance/Index?custid=" + customerInfoViewModel.CustId.ToString() + "&roleId=4&userName=" + customerInfoViewModel.UserName;
                    TemplateHandler.RemoveBlock(ref tplTransLink, customerInfoViewModel.IsDeposit ? "balance" : null);
                    lblDownlineList = "Master List";
                    break;

                case Globals.AccountType.Master:
                    if (!customerInfoViewModel.IsInternalUser)
                    {
                        downlineListURL = "Customer/AgentList.aspx?masterId=" + customerInfoViewModel.CustId;

                        if (WebSettings.OfferDoubleComm)
                        {
                            downlineListURL += "&doubleCommMaster=" + customerInfoViewModel.Doublecommlosing.ToString().ToLower();
                        }
                    }
                    else
                    {
                        downlineListURL = "FinanceCustomerList/Index?upline=" + customerInfoViewModel.CustId + "&roleid=3";
                    }

                    customerBalanceURL = "CustomerBalance/Index?custid=" + customerInfoViewModel.CustId.ToString() + "&roleId=3&userName=" + customerInfoViewModel.UserName;
                    TemplateHandler.RemoveBlock(ref tplTransLink, customerInfoViewModel.IsDeposit ? "balance" : null);
                    lblDownlineList = "Agent List";
                    break;

                case Globals.AccountType.Agent:
                    if (!customerInfoViewModel.IsInternalUser)
                    {
                        bool allowHR = !WebSettings.disabledHR;
                        downlineListURL = "Customer/MemberList.aspx?agentId=" + customerInfoViewModel.CustId + "&hr=" + allowHR.ToString().ToLower() + "&internal=" + customerInfoViewModel.IsInternalUser.ToString().ToLower();
                        if (WebSettings.OfferDoubleComm)
                        {
                            downlineListURL += "&doubleCommAgent=" + customerInfoViewModel.Doublecommlosing.ToString().ToLower();
                        }
                    }
                    else
                    {
                        downlineListURL = "FinanceCustomerList/Index?upline=" + customerInfoViewModel.CustId + "&roleid=2";
                    }

                    customerBalanceURL = "Customer/CustomerBalance.aspx?custid=" + customerInfoViewModel.CustId.ToString() + "&userName=" + customerInfoViewModel.UserName;
                    lblDownlineList = "Member List";
                    break;

                case Globals.AccountType.Member:
                    downlineListURL = "TarusBetList/BetListByCustInfo?CustID=" + customerInfoViewModel.CustId;
                    lblDownlineList = "Bet List";
                    TemplateHandler.RemoveBlock(ref tplTransLink, "balance");
                    break;
            }

            TemplateHandler.Assign(ref tplTransLink, "{IsInternal}", customerInfoViewModel.IsInternalUser.ToString().ToLower());

            TemplateHandler.Assign(ref tplTransLink, "{custname}", HttpUtility.UrlEncode(customerInfoViewModel.UserName));
            if (Permissions.HasPermission(Enumerations.Functions.Customer.CustomerBalance))
            {
                TemplateHandler.Assign(ref tplTransLink, "{CustomerBalanceURL}", customerBalanceURL);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref tplTransLink, "balance");
            }

            TemplateHandler.Assign(ref tplTransLink, "downlineLink", downlineListURL);
            TemplateHandler.Assign(ref tplTransLink, "lblDownline", lblDownlineList);

            TemplateHandler.Assign(ref htmlcontent, "{TransLinksBox}", tplTransLink);

            TemplateHandler.Assign(ref htmlcontent, "{fromCustName}", AppSession.CustUName);
            TemplateHandler.Assign(ref htmlcontent, "{fromCustID}", AppSession.CustID);
            TemplateHandler.Assign(ref htmlcontent, "{CustName}", AppSession.CustUName);
            TemplateHandler.Assign(ref htmlcontent, "{CustID}", AppSession.CustID);

            return htmlcontent;
        }

    }
}
