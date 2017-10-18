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
            Globals.AccountType accLevel = (Globals.AccountType)customerInfoViewModel.roleId;

            string statementLink = string.Empty;
            if (!customerInfoViewModel.isInternalUser)
            {
                statementLink = "Statement/CustomerStatement.aspx?custname={custname}&isinternal=" + customerInfoViewModel.isInternalUser;
            }
            else
            {
                statementLink = "CustomerStatement/Index?CustID=" + customerInfoViewModel.custId + "&CustName={custname}";
            }
            TemplateHandler.Assign(ref tplTransLink, "statementLink", statementLink);

            bool enableDeposit = WebSettings.IsApplySplitingDepositeCustomer;
            if (enableDeposit && customerInfoViewModel.isDeposit && accLevel != Globals.AccountType.Member)
            {
                TemplateHandler.RemoveBlock(ref tplTransLink, "downlinelistbtn");
            }

            switch (accLevel)
            {
                case Globals.AccountType.Super:
                    if (!customerInfoViewModel.isInternalUser)
                    {
                        if (WebSettings.OfferDoubleComm)
                        {
                            downlineListURL = "CustomerList/MasterAgentList?superId=" + customerInfoViewModel.custId + "&doubleCommSuper=" + customerInfoViewModel.doublecommlosing.ToString().ToLower();
                        }
                        else
                        {
                            downlineListURL = "Customer/MasterAgentList.aspx?superId=" + customerInfoViewModel.custId;
                        }
                    }
                    else
                    {
                        downlineListURL = "FinanceCustomerList/Index?upline=" + customerInfoViewModel.custId + "&roleid=4";
                    }

                    customerBalanceURL = "CustomerBalance/Index?custid=" + customerInfoViewModel.custId.ToString() + "&roleId=4&userName=" + customerInfoViewModel.username;
                    TemplateHandler.RemoveBlock(ref tplTransLink, customerInfoViewModel.isDeposit ? "balance" : null);
                    lblDownlineList = "Master List";
                    break;

                case Globals.AccountType.Master:
                    if (!customerInfoViewModel.isInternalUser)
                    {
                        if (WebSettings.OfferDoubleComm)
                        {
                            downlineListURL = "CustomerList/AgentList?masterId=" + customerInfoViewModel.custId + "&doubleCommMaster=" + customerInfoViewModel.doublecommlosing.ToString().ToLower();
                        }
                        else
                        {
                            downlineListURL = "Customer/AgentList.aspx?masterId=" + customerInfoViewModel.custId;
                        }
                    }
                    else
                    {
                        downlineListURL = "FinanceCustomerList/Index?upline=" + customerInfoViewModel.custId + "&roleid=3";
                    }

                    customerBalanceURL = "CustomerBalance/Index?custid=" + customerInfoViewModel.custId.ToString() + "&roleId=3&userName=" + customerInfoViewModel.username;
                    TemplateHandler.RemoveBlock(ref tplTransLink, customerInfoViewModel.isDeposit ? "balance" : null);
                    lblDownlineList = "Agent List";
                    break;

                case Globals.AccountType.Agent:
                    if (!customerInfoViewModel.isInternalUser)
                    {
                        bool allowHR = !WebSettings.disabledHR;
                        if (WebSettings.OfferDoubleComm)
                        {
                            downlineListURL = "CustomerList/MemberList?agentId=" + customerInfoViewModel.custId + "&hr=" + allowHR.ToString().ToLower() + "&internal=" + customerInfoViewModel.isInternalUser.ToString().ToLower() + "&doubleCommAgent=" + customerInfoViewModel.doublecommlosing.ToString().ToLower();
                        }
                        else
                        {
                            downlineListURL = "Customer/MemberList.aspx?agentId=" + customerInfoViewModel.custId + "&hr=" + allowHR.ToString().ToLower() + "&internal=" + customerInfoViewModel.isInternalUser.ToString().ToLower();
                        }
                    }
                    else
                    {
                        downlineListURL = "FinanceCustomerList/Index?upline=" + customerInfoViewModel.custId + "&roleid=2";
                    }

                    customerBalanceURL = "Customer/CustomerBalance.aspx?custid=" + customerInfoViewModel.custId.ToString() + "&userName=" + customerInfoViewModel.username;
                    lblDownlineList = "Member List";
                    break;

                case Globals.AccountType.Member:
                    downlineListURL = "TarusBetList/BetListByCustInfo?CustID=" + customerInfoViewModel.custId;
                    lblDownlineList = "Bet List";
                    TemplateHandler.RemoveBlock(ref tplTransLink, "balance");
                    break;
            }

            TemplateHandler.Assign(ref tplTransLink, "{IsInternal}", customerInfoViewModel.isInternalUser.ToString().ToLower());

            TemplateHandler.Assign(ref tplTransLink, "{custname}", HttpUtility.UrlEncode(customerInfoViewModel.username));
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
