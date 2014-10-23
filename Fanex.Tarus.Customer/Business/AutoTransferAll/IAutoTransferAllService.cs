namespace Fanex.Tarus.Customer.Business.AutoTransferAll
{
    using System.IO;
    using System.Web;
    using Fanex.Tarus.Customer.Criteria.AutoTransferAll;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.AutoTransferAll;

    public interface IAutoTransferAllService
    {
        AutoTransferAll GetAutoTranfers(SelectTransferAgentCriteria criteria);

        Result UpdatedTransfer(UpdatedTransferCriteria criteria);

        Result DeletedTransfer(DeletedTransferCriteria criteria);

        Result InsertAgentsFromPopup(InsertAgentsFromPopupCriteria criteria);

        Result ClearBalanceDownlines(ClearBalanceDownlineCriteria criteria);

        Result ClearBalanceMembers(ClearBalanceMembersCriteria criteria);

        ValidAgents InsertAgentsFromFile(HttpPostedFileBase fileUploaded);

        ValidAgents CheckValidAgentsFromFile(HttpPostedFileBase fileUploaded);

        MemoryStream ExportExcelFile();
    }
}
