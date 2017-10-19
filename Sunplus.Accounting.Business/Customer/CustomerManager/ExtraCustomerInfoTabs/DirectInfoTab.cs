namespace Sunplus.Accounting.Business.Customer
{
    using System.Data;
    using DataAccess.IBCDLL.Customer;
    using DNAide.Web;

    public partial class CustomerManager
    {
        private static string RenderDirectInfoTab(string htmlcontent, CustomerInfoViewModel customerInfoViewModel)
        {
            var directCustomerInfo = SelectCustomer.CustomerSelDirectInfo(customerInfoViewModel.CustId);
            DataRow directCustomerInfoRow = null;

            if (directCustomerInfo.ResultSet.Tables.Count > 0 && directCustomerInfo.ResultSet.Tables[0].Rows.Count > 0)
            {
                directCustomerInfoRow = directCustomerInfo.ResultSet.Tables[0].Rows[0];
            }

            if (directCustomerInfoRow == null)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "directinfo");
                TemplateHandler.RemoveBlock(ref htmlcontent, "directinfotab");
                TemplateHandler.RemoveBlock(ref htmlcontent, "directinfotabcontent");
            }
            else
            {
                //TO DO
                TemplateHandler.Assign(ref htmlcontent, "{CurrencyInfo}", directCustomerInfoRow["currency"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{MaxLoss}", IBCUtils.FormatHandler.FormatNumber20(directCustomerInfoRow["maxloss"]));
                TemplateHandler.Assign(ref htmlcontent, "{Creator}", directCustomerInfoRow["createdby"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{PersonInCharge}", directCustomerInfoRow["processedby"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{Boss}", directCustomerInfoRow["boss"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{Requestor}", directCustomerInfoRow["requestby"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{Approver}", directCustomerInfoRow["approvedby"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{Mobile}", directCustomerInfoRow["mobilephone"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{CurrencyTab}", directCustomerInfoRow["currency"].ToString());
                TemplateHandler.Assign(ref htmlcontent, "{MaxLossTab}", IBCUtils.FormatHandler.FormatNumber20(directCustomerInfoRow["maxloss"]));
                TemplateHandler.Assign(ref htmlcontent, "{LinkDirectInfo}", string.Format("Customer/EditDirectCustomerInfo.aspx?roleId={0}&custId={1}&userName={2}", customerInfoViewModel.RoleId, customerInfoViewModel.CustId, customerInfoViewModel.UserName));
            }

            return htmlcontent;
        }
    }
}
