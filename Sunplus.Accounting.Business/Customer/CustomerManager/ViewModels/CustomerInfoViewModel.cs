namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Configurations;
    using DataAccess;
    using Entities;
    using Fanex.CustomerSetting.Module.Core;
    using Security;
    using System.Collections;
    using System.Data;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;

    public partial class CustomerInfoViewModel
    {
        private CustomerDetail customerDetail;

        private DataRow customerInfoRow { get; set; }

        private DataRow lineInfoRow { get; set; }

        private DataRow cashRow { get; set; }

        private DataRow downlineStatusInfoRow { get; set; }

        public CustomerInfoViewModel(
            DataRow customerInfoRow,
            DataRow lineInfoRow,
            DataRow cashRow,
            DataRow downlineStatusInfoRow,
            DataTable dynamicSettingtable,
            decimal memberOutstandingMBC,
            decimal memberBalanceMBC,
            decimal memberNetPositionMBC,
            decimal member3rdPendingTransfer)
        {
            this.customerInfoRow = customerInfoRow;
            this.lineInfoRow = lineInfoRow;
            this.cashRow = cashRow;
            this.downlineStatusInfoRow = downlineStatusInfoRow;

            JsParam = new Hashtable
            {
                { "custid", CustId },
                { "roleid", SearchAccountRoleId }
            };

            GetExtraCustomerInfo(customerInfoRow, lineInfoRow, cashRow, memberOutstandingMBC, memberBalanceMBC, memberNetPositionMBC, member3rdPendingTransfer);
            GetShowedHiddenProduct(dynamicSettingtable);
        }

        public Hashtable JsParam { get; set; }

        public bool HasReadPermission_AutoSuspend => Permissions.HasPermission(Enumerations.Functions.Customer.AutoSuspend);

        public bool ManageCustomerAccess => Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust);

        public bool AllowUpdateGivenCredit => Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

        public bool ViewFiAcc => Permissions.FinancePermission.IsViewFiAcc(IsInternalUser);

        public bool IsSabaCoins => Utils.ToBoolean(customerInfoRow["IsSabaCoins"]);

        public int ShadowCustId => Utils.ToInt32(customerInfoRow["ShadowCustId"]);

        public bool HideDirectStatus => Utils.ToBoolean(customerInfoRow["IsHideDirectStatus"]);

        public bool CheckedDirectStatus => Utils.ToBoolean(customerInfoRow["IsCheckDirectStatus"]);

        public bool DisabledDirectStatus => Utils.ToBoolean(customerInfoRow["IsDisabledDirectStatus"]);

        public bool AllDayBalTransfer => Utils.ToBoolean(customerInfoRow["isalldaybaltransfer"]);

        public bool AllDayBalTransferOnMaster => Utils.ToBoolean(customerInfoRow["IsAutoEnableADBMaster"]);

        public bool IsShowAllDayBalTransfer => Utils.ToBoolean(customerInfoRow["IsShowAllDayBalTransfer"]);

        public bool DisableAutoMPT => Utils.ToBoolean(customerInfoRow["DisableAutoMPT"]);

        public bool DisableOutright => Utils.ToBoolean(customerInfoRow["DisableOutright"]);

        public bool HideBitcoin { get; private set; } = true;

        public bool HideCashout { get; private set; } = true;

        public bool HideRNGLottery { get; private set; } = true;

        public bool HideVirtualSports2 { get; private set; } = true;

        public bool HideSabaVirtualSports { get; private set; } = true;

        public bool ShowSettingLinkAGCasinoDeposit => WebSettings.EnableAGCasino && IsDeposit;

        public bool ShowEnableBetTradeDeposit => WebSettings.EnableBetTrade && SearchAccountRoleId.Equals(4) && IsDeposit;

        public bool ShowMacauDeposit => WebSettings.EnableMacau && SearchAccountRoleId.Equals(4) && IsDeposit;

        public bool HideSettingLinkVirtualSports { get; private set; } = true;

        public bool HideCheckBoxVirtualSports { get; private set; } = true;

        public bool LockUnsuspend => Utils.ToBoolean(customerInfoRow["LockUnsuspend"]);

        public bool HideMuayThai { get; private set; } = true;

        public bool HideLiveAndRNGCasino { get; private set; } = true;

        public CustomerDetail CustParent
        {
            get
            {
                if (customerDetail == null)
                {
                    customerDetail = ParentId > 0 ? IBCProvider.IBCCustomer.GetCustomerStatus(ParentId, WebSettings.SettingStatus) : new CustomerDetail();
                }

                return customerDetail;
            }
        }

        private void GetShowedHiddenProduct(DataTable dynamicSettingtable)
        {
            if (this.SearchAccountRoleId != (int)Role.Headline)
            {
                foreach (DataRow row in dynamicSettingtable.Rows)
                {
                    if (row["productID"] != null)
                    {
                        var productId = row["productID"].ToString();
                        switch (productId)
                        {
                            case "5":
                                HideCheckBoxVirtualSports = (Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4) || !WebSettings.EnableVirtualSports;
                                HideSettingLinkVirtualSports = (Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4) || !WebSettings.EnableVirtualSports || IsDeposit;
                                break;

                            case "25":
                                HideCashout = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4) || !WebSettings.EnableCashOut;
                                break;

                            case "27":
                                HideVirtualSports2 = (Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4) || !WebSettings.EnableVirtualSports;
                                break;

                            case "44": // We do NOT have Unsync case for RNG Lottery.
                                bool isHideRNGLottery = Utils.ToBoolean(row["IsHidden"]);
                                bool isHideUplineRNGLottery = Utils.ToBoolean(row["IsHiddenUpline"]);

                                HideRNGLottery = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdRNGLottery == 0 ||
                                    IsDeposit || (SearchAccountRoleId < 4 && (isHideRNGLottery || isHideUplineRNGLottery));
                                break;

                            case "54":
                                HideSabaVirtualSports = Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowSabaVirtualSports;
                                break;

                            case "69":
                                bool isHideBitcoin = Utils.ToBoolean(row["IsHidden"]);
                                HideBitcoin = !WebSettings.IsShowBitcoinBetting || isHideBitcoin;
                                break;
                        }
                    }

                    if (!this.IsDeposit && row["GroupID"] != null && int.TryParse(row["GroupID"].ToString(), out int productGroupId))
                    {
                        switch (productGroupId)
                        {
                            case ProductGroupIdConstants.SportsBook:
                                HideMuayThai = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4);
                                break;

                            case ProductGroupIdConstants.LiveAndRNGCasino:
                                HideLiveAndRNGCasino = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4);
                                break;
                        }
                    }
                }
            }
        }
    }
}