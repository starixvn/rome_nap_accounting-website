using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.PT.Bingo;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Bingo.Entities;

/// <summary>
/// Bingo Controller
/// </summary>
namespace Sunplus.Accounting.WcfServiceLibrary.Bingo
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BingoService : IBingoService
    {
        public Sunplus.Bingo.Entities.Customer GetCustomerSetting(int custID, int updatedByRoleID)
        {
            var bingoService = new Sunplus.Accounting.Business.PT.Bingo.BingoManager();
            return bingoService.GetCustomerSetting(custID, updatedByRoleID);
        }

        public Result UpdateCustomerSetting(Sunplus.Bingo.Entities.Customer customer, int updatedByRoleID)
        {
            var bingoService = new Sunplus.Accounting.Business.PT.Bingo.BingoManager();
            return bingoService.UpdateCustomerSetting(customer, updatedByRoleID);
        }

        public Result ResetMaxTransfer(int custId)
        {
            var bingoService = new Sunplus.Accounting.Business.PT.Bingo.BingoManager();
            return bingoService.ResetMaxTransfer(custId);
        }

        public void AutoInsertBingoAccount(CSCustomerDetail customerInfo, int userId, string userName, string ip)
        {
            var bingoService = new BingoManager();
            bingoService.AutoInsertBingoAccount(customerInfo, userId, userName, ip, WebSettings.IsAllowMoveToBingo);
        }
    }
}