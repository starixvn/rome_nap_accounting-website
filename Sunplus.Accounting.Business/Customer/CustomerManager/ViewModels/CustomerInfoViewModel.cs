namespace Sunplus.Accounting.Business.Customer
{
    using System.Collections;
    using System.Data;
    using Core.Common;
    using Core.Configurations;
    using DataAccess;
    using Entities;
    using Fanex.Common;
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

        public bool HideDirectStatus => Converter.ToBool(customerInfoRow["IsHideDirectStatus"]);

        public bool CheckedDirectStatus => Converter.ToBool(customerInfoRow["IsCheckDirectStatus"]);

        public bool DisabledDirectStatus => Converter.ToBool(customerInfoRow["IsDisabledDirectStatus"]);

        public bool AllDayBalTransfer => Converter.ToBool(customerInfoRow["isalldaybaltransfer"]);

        public bool DisableAutoMPT => Converter.ToBool(cashRow["DisableAutoMPT"]);

        public bool DisableOutright => Converter.ToBool(customerInfoRow["DisableOutright"]);

        public bool HideCricketSettingLinkCredit { get; private set; } = true;

        public bool HideEnableAndEditFishingWorld { get; private set; } = true;

        public bool HideKenoLottery { get; private set; } = true;

        public bool HideVirtualSports2 { get; private set; } = true;

        public bool ShowSettingLinkAGCasinoDeposit => WebSettings.EnableAGCasino && IsDeposit;

        public bool ShowSettingLinkGoldDeluxeCredit => WebSettings.IsShowGoldDeluxeSettingForCredit && !IsDeposit;

        public bool SyncUplineGoldDeluxe => Converter.ToBool(customerInfoRow["IsSyncUplineGoldDeluxe"]);

        public bool EnableHorseRacingFixedOdds => !Converter.ToBool(customerInfoRow["DisableHRFixedOdds"]);

        public bool DisableDownlinesHorseRacingFixedOdds => Converter.ToBool(customerInfoRow["DisableDownlinesHRFixedOdds"]);

        public bool ShowEnableCheckBoxKenoCredit => KenoSection.Instance.Enable && !IsDeposit;

        public bool ShowSettingLinkKenoCredit => KenoSection.Instance.ShowSetting && !IsDeposit;

        public bool ShowMicrogamingSettingLinkDeposit => WebSettings.EnableMicrogaming && IsDeposit;

        public bool ShowHorseRacingLinkCredit => !WebSettings.disabledHR && !IsDeposit;

        public bool ShowEnableBetTradeDeposit => WebSettings.EnableBetTrade && RoleId.Equals(4) && IsDeposit;

        public bool ShowEnableCashOutDeposit => WebSettings.EnableCashOut && RoleId.Equals(4) && IsDeposit;

        public bool ShowEnableMicrogamingDeposit => WebSettings.EnableMicrogaming && IsDeposit;

        public bool ShowMacauDeposit => WebSettings.EnableMacau && RoleId.Equals(4) && IsDeposit;

        public bool HideSportsbook2Credit { get; private set; } = true;

        public bool HideCockFightingCredit { get; private set; } = true;

        public bool HideVirtualSportsCredit { get; private set; } = true;

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
                            HideVirtualSportsCredit = (Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports || IsDeposit;
                            break;
                        case "16":
                            HideSportsbook2Credit = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowSportsbook2 || IsDeposit;
                            break;
                        case "19":
                            HideCockFightingCredit = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowCockFighting || IsDeposit;
                            break;
                        case "20":
                            HideCricketSettingLinkCredit = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowCricket || !IsIMBC || IsDeposit;
                            break;
                        case "21":
                            SyncAllbet = Converter.ToBool(row["IsSync"]);
                            HideAllbetUpline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            SyncAllbetUpline = Converter.ToBool(row["IsSyncedUpline"]);
                            break;

                        case "22":
                            SyncVoidbridge = Converter.ToBool(row["IsSync"]);
                            HideVoidbridgeUpline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            SyncVoidbridgeUpline = Converter.ToBool(row["IsSyncedUpline"]);
                            break;

                        case "23":
                            HideNG2 = Converter.ToBool(row["IsHidden"]) || !WebSettings.IsShowNG2;
                            SyncNG2 = Converter.ToBool(row["IsSync"]);
                            HideNG2Upline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            SyncNG2Upline = Converter.ToBool(row["IsSyncedUpline"]);

                            break;
                        case "24":
                            bool isHideUplineFishingWorld = Converter.ToBool(row["IsHiddenUpline"]);
                            bool isSyncUplineFishingWorld = Converter.ToBool(row["IsSyncedUpline"]);

                            HideEnableAndEditFishingWorld = (RoleId < 4 && isHideUplineFishingWorld) || !isSyncUplineFishingWorld;

                            break;
                        case "27":
                            HideVirtualSports2 = (Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports;
                            break;

                        case "28": // We do NOT have Unsync case for Keno Lottery.
                            bool isHideKenoLottery = Converter.ToBool(row["IsHidden"]);
                            bool isHideUplineKenoLottery = Converter.ToBool(row["IsHiddenUpline"]);

                            HideKenoLottery = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery == 0 ||
                                IsDeposit ||
                                (RoleId < 4 && (isHideKenoLottery || isHideUplineKenoLottery));
                            break;

                        case "26":
                            HideLucky3 = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowLucky3 || IsDeposit;
                            HideLucky3Upline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            break;
                    }
                }
            }
        }
    }
}
