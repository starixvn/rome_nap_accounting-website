namespace Sunplus.Accounting.Business.Customer
{
    using Core.Web;
    using DNAide.Web;
    using IBCUtils.Objects;

    public partial class CustomerManager
    {
        private static string GenUplineBox(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var uplineboxhtml = WebUtils.LoadTemplate("CustomerInfo/UplineBox.htm");

            if (customerInfoViewModel.RoleId <= UserInfo.ROLE_SUPER)
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{SuperId}", customerInfoViewModel.SuperId);
                TemplateHandler.Assign(ref uplineboxhtml, "{SuperName}", customerInfoViewModel.SuperName);
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden1}", string.Empty);
                if (!customerInfoViewModel.SuperEnabled)
                {
                    TemplateHandler.RemoveBlock(ref uplineboxhtml, "SuperLogin");
                }
            }
            else
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden1}", "hidden");
            }

            if (customerInfoViewModel.RoleId <= UserInfo.ROLE_MASTER)
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{MasterId}", customerInfoViewModel.MasterId);
                TemplateHandler.Assign(ref uplineboxhtml, "{MasterName}", customerInfoViewModel.MasterName);
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden2}", string.Empty);
                if (!customerInfoViewModel.MasterEnabled)
                {
                    TemplateHandler.RemoveBlock(ref uplineboxhtml, "MasterLogin");
                }
            }
            else
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden2}", "hidden");
            }

            if (customerInfoViewModel.RoleId <= UserInfo.ROLE_AGENT)
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{AgentId}", customerInfoViewModel.AgentId);
                TemplateHandler.Assign(ref uplineboxhtml, "{AgentName}", customerInfoViewModel.AgentName);
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden3}", string.Empty);
                if (!customerInfoViewModel.AgentEnabled)
                {
                    TemplateHandler.RemoveBlock(ref uplineboxhtml, "AgentLogin");
                }
            }
            else
            {
                TemplateHandler.Assign(ref uplineboxhtml, "{IsHidden3}", "hidden");
            }

            TemplateHandler.Assign(ref htmlcontent, "{UplineBox}", uplineboxhtml);

            return htmlcontent;
        }
    }
}
