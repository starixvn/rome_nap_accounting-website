namespace Sunplus.Accounting.Business.Customer
{
    using System.Collections;
    using System.Data;
    using Core.Common;
    using Core.Configurations;
    using DataAccess;
    using Entities;
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
    using Security;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;

    public partial class CustomerInfoViewModel
    {
        private CustomerDetail customerDetail;

        private DataRow customerInfoRow { get; set; }

        private DataRow lineInfoRow { get; set; }

        private DataRow cashRow { get; set; }

        private DataRow downlineStatusInfoRow { get; set; }

        private DataTable dynamicSettingtable { get; set; }

        private DataRow memberCN88CreditRow { get; set; }

        private DataRow memberCN88BetStakeRow { get; set; }

        public CustomerInfoViewModel(
            DataRow customerInfoRow,
            DataRow lineInfoRow,
            DataRow cashRow,
            DataRow downlineStatusInfoRow,
            DataTable dynamicSettingtable,
            DataRow memberCN88CreditRow,
            DataRow memberCN88BetStakeRow,
            decimal memberOutstandingMBC,
            decimal memberBalanceMBC,
            decimal memberNetPositionMBC,
            decimal member3rdPendingTransfer)
        {
            this.customerInfoRow = customerInfoRow;
            this.lineInfoRow = lineInfoRow;
            this.cashRow = cashRow;
            this.downlineStatusInfoRow = downlineStatusInfoRow;
            this.dynamicSettingtable = dynamicSettingtable;
            this.memberCN88CreditRow = memberCN88CreditRow;
            this.memberCN88BetStakeRow = memberCN88BetStakeRow;

            JsParam = new Hashtable();
            JsParam.Add("custid", CustId);
            JsParam.Add("roleid", SearchAccountRoleId);

            GetExtraCustomerInfo(customerInfoRow, lineInfoRow, cashRow, memberOutstandingMBC, memberBalanceMBC, memberNetPositionMBC, member3rdPendingTransfer);
            GetShowedHiddenProduct(dynamicSettingtable);
        }

        public Hashtable JsParam { get; set; }

        public bool HasReadPermission_AutoSuspend => Permissions.HasPermission(Enumerations.Functions.Customer.AutoSuspend);

        public bool ManageCustomerAccess => Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust);

        public bool AllowUpdateGivenCredit => Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

        public bool ViewFiAcc => Permissions.FinancePermission.IsViewFiAcc(IsInternalUser);

        public bool HideDirectStatus => Utils.ToBoolean(customerInfoRow["IsHideDirectStatus"]);

        public bool CheckedDirectStatus => Utils.ToBoolean(customerInfoRow["IsCheckDirectStatus"]);

        public bool DisabledDirectStatus => Utils.ToBoolean(customerInfoRow["IsDisabledDirectStatus"]);

        public bool AllDayBalTransfer => Utils.ToBoolean(customerInfoRow["isalldaybaltransfer"]);

        public bool AllDayBalTransferOnMaster => Utils.ToBoolean(customerInfoRow["IsAutoEnableADBMaster"]);

        public bool IsShowAllDayBalTransfer => Utils.ToBoolean(customerInfoRow["IsShowAllDayBalTransfer"]);

        public bool DisableAutoMPT => Utils.ToBoolean(cashRow["DisableAutoMPT"]);

        public bool DisableOutright => Utils.ToBoolean(customerInfoRow["DisableOutright"]);

        public bool HideEnableAndEditFishingWorld { get; private set; } = true;

        public bool HideVirtualGames { get; private set; } = true;

        public bool HideBitcoin { get; private set; } = true;

        public bool HideRNGLottery { get; private set; } = true;

        public bool HideVirtualSports2 { get; private set; } = true;

        public bool HideSabaclub { get; private set; } = true;

        public bool HideVGaming { get; private set; } = true;

        public bool HideSabaVirtualSports { get; private set; } = true;

        public bool ShowSettingLinkAGCasinoDeposit => WebSettings.EnableAGCasino && IsDeposit;

        public bool ShowSettingLinkGoldDeluxeCredit => WebSettings.IsShowGoldDeluxeSettingForCredit && !IsDeposit;

        public bool SyncUplineGoldDeluxe => Utils.ToBoolean(customerInfoRow["IsSyncUplineGoldDeluxe"]);

        public bool ShowEnableCheckBoxKeno => KenoSection.Instance.Enable;

        public bool ShowSettingLinkKenoCredit => KenoSection.Instance.ShowSetting && !IsDeposit;

        public bool ShowEnableBetTradeDeposit => WebSettings.EnableBetTrade && SearchAccountRoleId.Equals(4) && IsDeposit;

        public bool ShowEnableCashOutDeposit => WebSettings.EnableCashOut && SearchAccountRoleId.Equals(4) && IsDeposit;

        public bool ShowMacauDeposit => WebSettings.EnableMacau && SearchAccountRoleId.Equals(4) && IsDeposit;

        public bool HideSportsbook2 { get; private set; } = true;

        public bool HideSettingLinkVirtualSports { get; private set; } = true;

        public bool HideCheckBoxVirtualSports { get; private set; } = true;

        public bool LockUnsuspend => Utils.ToBoolean(customerInfoRow["LockUnsuspend"]);

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

                            case "16":
                                HideSportsbook2 = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4) || !WebSettings.IsShowSportsbook2 || IsDeposit;
                                break;

                            case "41":
                                //SyncLiveCasino = Utils.ToBoolean(row["IsSync"]);
                                HideLiveCasinoUpline = HideLiveCasinoUpline && Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4;
                                HideLiveCasino = HideLiveCasino && Utils.ToBoolean(row["IsHidden"]);
                                //SyncLiveCasinoUpline = Utils.ToBoolean(row["IsSyncedUpline"]);
                                break;

                            case "22":
                                SyncVoidbridge = Utils.ToBoolean(row["IsSync"]);
                                HideVoidbridgeUpline = Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4;
                                SyncVoidbridgeUpline = Utils.ToBoolean(row["IsSyncedUpline"]);
                                break;

                            case "23":
                                HideNG2 = Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowNG2;
                                SyncNG2 = Utils.ToBoolean(row["IsSync"]);
                                HideNG2Upline = Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4;
                                SyncNG2Upline = Utils.ToBoolean(row["IsSyncedUpline"]);

                                break;

                            case "24":
                                bool isHideUplineFishingWorld = Utils.ToBoolean(row["IsHiddenUpline"]);
                                bool isSyncUplineFishingWorld = Utils.ToBoolean(row["IsSyncedUpline"]);

                                HideEnableAndEditFishingWorld = (SearchAccountRoleId < 4 && isHideUplineFishingWorld) || !isSyncUplineFishingWorld;

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
                            case "38":
                                bool isHideVirtualGames = Utils.ToBoolean(row["IsHidden"]);
                                HideVirtualGames = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdVirtualGames == 0 || isHideVirtualGames;
                                break;
                            case "42":
                                HideSabaclub = Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowSabaclub;
                                SyncSabaclub = Utils.ToBoolean(row["IsSync"]);
                                HideSabaclubUpline = Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4;
                                SyncSabaclubUpline = Utils.ToBoolean(row["IsSyncedUpline"]);
                                break;
                            case "54":
                                HideSabaVirtualSports = Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowSabaVirtualSports;
                                break;
                            case "66":
                                HideVGaming = Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowVGaming;
                                SyncVGaming = Utils.ToBoolean(row["IsSync"]);
                                HideVGamingUpline = Utils.ToBoolean(row["IsHiddenUpline"]) && SearchAccountRoleId < 4;
                                SyncVGamingUpline = Utils.ToBoolean(row["IsSyncedUpline"]);
                                break;
                            case "69":
                                bool isHideBitcoin = Utils.ToBoolean(row["IsHidden"]);
                                HideBitcoin = !WebSettings.IsShowBitcoinBetting || isHideBitcoin;
                                break;
                        }
                    }
                }
            }
        }
    }
}