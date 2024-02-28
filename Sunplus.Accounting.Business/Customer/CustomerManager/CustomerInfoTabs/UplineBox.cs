namespace Sunplus.Accounting.Business.Customer
{
    using System.Collections.Generic;
    using Core.Web;
    using DNAide.Web;
    using IBCUtils.Objects;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;

    public partial class CustomerManager
    {
        private static string GenUplineBox(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var uplineboxhtml = WebUtils.LoadTemplate("CustomerInfo/UplineBox.htm");
            var robotIds = new List<int>();
           
         

            if (customerInfoViewModel.SearchAccountRoleId <= (int)Role.Super
                && customerInfoViewModel.SearchAccountRoleId != (int)Role.Headline)
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{SuperId}", customerInfoViewModel.SuperId);
                TemplateHandler.Assign(ref uplineboxhtml, "{SuperName}", customerInfoViewModel.SuperName);
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden1}", string.Empty);
                robotIds.Add(customerInfoViewModel.SuperId);
                if (customerInfoViewModel.IsInternalUser || !customerInfoViewModel.SuperEnabled)
                {
                    TemplateHandler.RemoveBlock(ref uplineboxhtml, "SuperLogin");
                }
            }
            else
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden1}", "hidden");
                TemplateHandler.RemoveBlock(ref uplineboxhtml, "SuperInfo");
            }

            if (customerInfoViewModel.SearchAccountRoleId <= UserInfo.ROLE_MASTER
                && customerInfoViewModel.SearchAccountRoleId != (int)Role.Headline)
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{MasterId}", customerInfoViewModel.MasterId);
                TemplateHandler.Assign(ref uplineboxhtml, "{MasterName}", customerInfoViewModel.MasterName);
                robotIds.Add(customerInfoViewModel.MasterId);
                if (customerInfoViewModel.IsInternalUser || !customerInfoViewModel.MasterEnabled)
                {
                    TemplateHandler.RemoveBlock(ref uplineboxhtml, "MasterLogin");
                }
            }
            else
            {
                TemplateHandler.RemoveBlock(ref uplineboxhtml, "MasterInfo");
            }

            if (customerInfoViewModel.SearchAccountRoleId <= UserInfo.ROLE_AGENT
                && customerInfoViewModel.SearchAccountRoleId != (int)Role.Headline)
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{AgentId}", customerInfoViewModel.AgentId);
                TemplateHandler.Assign(ref uplineboxhtml, "{AgentName}", customerInfoViewModel.AgentName);
                robotIds.Add(customerInfoViewModel.AgentId);
                if (customerInfoViewModel.IsInternalUser || !customerInfoViewModel.AgentEnabled)
                {
                    TemplateHandler.RemoveBlock(ref uplineboxhtml, "AgentLogin");
                }
            }
            else
            {
                TemplateHandler.RemoveBlock(ref uplineboxhtml, "AgentInfo");
            }

            if (customerInfoViewModel.SearchAccountRoleId <= UserInfo.ROLE_MEMBER
                && customerInfoViewModel.SearchAccountRoleId != (int)Role.Headline)
            {
                robotIds.Add(customerInfoViewModel.MemberId);
            }

            TemplateHandler.Assign(ref uplineboxhtml, "{RobotIds}", string.Join(",", robotIds));
            TemplateHandler.Assign(ref htmlcontent, "{UplineBox}", uplineboxhtml);

            return htmlcontent;
        }
    }
}
