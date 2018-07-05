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

    public partial class CustomerInfoViewModel
    {
        private DataRow customerInfoRow { get; set; }

        private DataRow lineInfoRow { get; set; }

        private DataRow cashRow { get; set; }

        private DataRow downlineStatusInfoRow { get; set; }

        private DataTable dynamicSettingtable { get; set; }

        public CustomerInfoViewModel(
            DataRow customerInfoRow,
            DataRow lineInfoRow,
            DataRow cashRow,
            DataRow downlineStatusInfoRow,
            DataTable dynamicSettingtable)
        {
            this.customerInfoRow = customerInfoRow;
            this.lineInfoRow = lineInfoRow;
            this.cashRow = cashRow;
            this.downlineStatusInfoRow = downlineStatusInfoRow;
            this.dynamicSettingtable = dynamicSettingtable;

            JsParam = new Hashtable();
            JsParam.Add("custid", CustId);
            JsParam.Add("roleid", RoleId);

            GetExtraCustomerInfo(customerInfoRow, lineInfoRow, cashRow);
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

        public bool DisableAutoMPT => Utils.ToBoolean(cashRow["DisableAutoMPT"]);

        public bool DisableOutright => Utils.ToBoolean(customerInfoRow["DisableOutright"]);

        public bool HideCricketCredit { get; private set; } = true;

        public bool HideEnableAndEditFishingWorld { get; private set; } = true;

        public bool HideKenoLottery { get; private set; } = true;

        public bool HideVirtualSports2 { get; private set; } = true;

        public bool ShowSettingLinkAGCasinoDeposit => WebSettings.EnableAGCasino && IsDeposit;

        public bool ShowSettingLinkGoldDeluxeCredit => WebSettings.IsShowGoldDeluxeSettingForCredit && !IsDeposit;

        public bool SyncUplineGoldDeluxe => Utils.ToBoolean(customerInfoRow["IsSyncUplineGoldDeluxe"]);

        public bool ShowEnableCheckBoxKeno => KenoSection.Instance.Enable;

        public bool ShowSettingLinkKenoCredit => KenoSection.Instance.ShowSetting && !IsDeposit;

        public bool ShowMicrogamingSettingLinkDeposit => WebSettings.EnableMicrogaming && IsDeposit;

        public bool ShowEnableBetTradeDeposit => WebSettings.EnableBetTrade && RoleId.Equals(4) && IsDeposit;

        public bool ShowEnableCashOutDeposit => WebSettings.EnableCashOut && RoleId.Equals(4) && IsDeposit;

        public bool ShowEnableMicrogamingDeposit => WebSettings.EnableMicrogaming && IsDeposit;

        public bool ShowMacauDeposit => WebSettings.EnableMacau && RoleId.Equals(4) && IsDeposit;

        public bool HideSportsbook2 { get; private set; } = true;

        public bool HideSettingLinkVirtualSports { get; private set; } = true;

        public bool HideCheckBoxVirtualSports { get; private set; } = true;

        public bool HideSettingBullfighting { get; private set; } = true;

        public bool HideSettingPokerGames { get; private set; } = true;

        public CustomerDetail CustParent => ParentId > 0 ? IBCProvider.IBCCustomer.GetCustomerStatus(ParentId) : new CustomerDetail();

        private void GetShowedHiddenProduct(DataTable dynamicSettingtable)
        {
            foreach (DataRow row in dynamicSettingtable.Rows)
            {
                if (row["productID"] != null)
                {
                    var productId = row["productID"].ToString();
                    switch (productId)
                    {
                        case "5":
                            HideCheckBoxVirtualSports = (Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports;
                            HideSettingLinkVirtualSports = (Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports || IsDeposit;
                            break;
                        case "16":
                            HideSportsbook2 = (Utils.ToBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowSportsbook2 || IsDeposit;
                            break;
                        case "20":
                            HideCricketCredit = (Utils.ToBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowCricket || !IsIMBC || IsDeposit;
                            break;
                        case "21":
                            SyncAllbet = Utils.ToBoolean(row["IsSync"]);
                            HideAllbetUpline = Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            SyncAllbetUpline = Utils.ToBoolean(row["IsSyncedUpline"]);
                            break;

                        case "22":
                            SyncVoidbridge = Utils.ToBoolean(row["IsSync"]);
                            HideVoidbridgeUpline = Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            SyncVoidbridgeUpline = Utils.ToBoolean(row["IsSyncedUpline"]);
                            break;

                        case "23":
                            HideNG2 = Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowNG2;
                            SyncNG2 = Utils.ToBoolean(row["IsSync"]);
                            HideNG2Upline = Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            SyncNG2Upline = Utils.ToBoolean(row["IsSyncedUpline"]);

                            break;
                        case "24":
                            bool isHideUplineFishingWorld = Utils.ToBoolean(row["IsHiddenUpline"]);
                            bool isSyncUplineFishingWorld = Utils.ToBoolean(row["IsSyncedUpline"]);

                            HideEnableAndEditFishingWorld = (RoleId < 4 && isHideUplineFishingWorld) || !isSyncUplineFishingWorld;

                            break;
                        case "27":
                            HideVirtualSports2 = (Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports;
                            break;

                        case "28": // We do NOT have Unsync case for Keno Lottery.
                            bool isHideKenoLottery = Utils.ToBoolean(row["IsHidden"]);
                            bool isHideUplineKenoLottery = Utils.ToBoolean(row["IsHiddenUpline"]);

                            HideKenoLottery = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery == 0 ||
                                IsDeposit || (RoleId < 4 && (isHideKenoLottery || isHideUplineKenoLottery));
                            break;

                        case "26":
                            HideLucky3 = (Utils.ToBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowLucky3;
                            HideLucky3Upline = Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            break;

                        case "30":
                            bool HideBullfighting = Utils.ToBoolean(row["IsHidden"]);
                            bool HideUplineBullfighting = Utils.ToBoolean(row["IsHiddenUpline"]);
                            HideSettingBullfighting = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdBullfighting == 0 ||
                                IsDeposit || (RoleId < 4 && (HideBullfighting || HideUplineBullfighting));
                            break;

                        case "33":
                            bool HidePokerGames = Utils.ToBoolean(row["IsHidden"]);
                            bool HideUplinePokerGames = Utils.ToBoolean(row["IsHiddenUpline"]);
                            HideSettingPokerGames = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdPokerGames == 0 ||
                                IsDeposit || (RoleId < 4 && (HidePokerGames || HideUplinePokerGames));
                            break;
                    }
                }
            }
        }
    }
}
