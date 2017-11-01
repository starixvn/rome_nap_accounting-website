namespace Sunplus.Accounting.Business.Customer
{
    using System;
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

        public bool IsManageCustomerAccess => Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust);

        public bool IsAllowUpdateGivenCredit => Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

        public bool IsViewFiAcc => Permissions.FinancePermission.IsViewFiAcc(IsInternalUser);

        public bool IsHideDirectStatus => Converter.ToBool(customerInfoRow["IsHideDirectStatus"]);

        public bool IsCheckedDirectStatus => Converter.ToBool(customerInfoRow["IsCheckDirectStatus"]);

        public bool IsDisabledDirectStatus => Converter.ToBool(customerInfoRow["IsDisabledDirectStatus"]);

        public bool IsAllDayBalTransfer => customerInfoRow.Table.Columns.Contains("isalldaybaltransfer") && Converter.ToBool(customerInfoRow["isalldaybaltransfer"]);

        public bool DisableAutoMPT => !Convert.IsDBNull(cashRow["DisableAutoMPT"]) && !Converter.ToBool(cashRow["DisableAutoMPT"]);

        public bool DisableOutright => !Convert.IsDBNull(customerInfoRow["DisableOutright"]) && Converter.ToBool(customerInfoRow["DisableOutright"]);

        public bool IsHideCricketSettingLink { get; private set; } = true;

        public bool HideEnableAndEditFishingWorld { get; private set; } = true;

        public bool HideKenoLottery { get; private set; } = true;

        public bool IsHideVirtualSports2 { get; private set; } = true;

        public bool IsSyncUplineGoldDeluxe => Converter.ToBool(customerInfoRow["IsSyncUplineGoldDeluxe"]);

        public bool IsEnableHorseRacingFixedOdds => !Convert.IsDBNull(customerInfoRow["DisableHRFixedOdds"]) && !Converter.ToBool(customerInfoRow["DisableHRFixedOdds"]);

        public bool DisableDownlinesHorseRacingFixedOdds => !Convert.IsDBNull(customerInfoRow["DisableDownlinesHRFixedOdds"]) && Converter.ToBool(customerInfoRow["DisableDownlinesHRFixedOdds"]);

        public bool IsShowKenoStatus => KenoSection.Instance.Enable;

        public bool IsShowEnableBetTrade => WebSettings.EnableBetTrade && RoleId.Equals(4) && IsDeposit;

        public bool IsShowEnableCashOut => WebSettings.EnableCashOut && RoleId.Equals(4) && IsDeposit;

        public bool IsShowEnableMicrogaming => WebSettings.EnableMicrogaming && IsDeposit;

        public bool IsShowMacau => WebSettings.EnableMacau && RoleId.Equals(4) && IsDeposit;

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
                            IsHideVirtualSports = (Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports;
                            break;
                        case "16":
                            IsHideSportsbook2 = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowSportsbook2 || IsDeposit;
                            break;
                        case "19":
                            IsHideCockFighting = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowCockFighting || IsDeposit;
                            break;
                        case "20":
                            IsHideCricketSettingLink = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowCricket || !IsIMBC || IsDeposit;
                            break;
                        case "21":
                            IsHideAllBet = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowAllBet || IsDeposit;
                            IsSyncAllbet = Converter.ToBool(row["IsSync"]);
                            IsHideAllbetUpline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            IsSyncAllbetUpline = Converter.ToBool(row["IsSyncedUpline"]);
                            break;

                        case "22":
                            IsHideVoidbridge = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowVoidbridge;
                            IsSyncVoidbridge = Converter.ToBool(row["IsSync"]);
                            IsHideVoidbridgeUpline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            IsSyncVoidbridgeUpline = Converter.ToBool(row["IsSyncedUpline"]);
                            break;

                        case "23":
                            IsHideNG2 = Converter.ToBool(row["IsHidden"]) || !WebSettings.IsShowNG2;
                            IsSyncNG2 = Converter.ToBool(row["IsSync"]);
                            IsHideNG2Upline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            IsSyncNG2Upline = Converter.ToBool(row["IsSyncedUpline"]);

                            break;
                        case "24":
                            bool isHideUplineFishingWorld = Converter.ToBool(row["IsHiddenUpline"]);
                            bool isSyncUplineFishingWorld = Converter.ToBool(row["IsSyncedUpline"]);

                            HideEnableAndEditFishingWorld = (RoleId < 4 && isHideUplineFishingWorld) || !isSyncUplineFishingWorld;

                            break;
                        case "27":
                            IsHideVirtualSports2 = (Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports;
                            break;

                        case "28": // We do NOT have Unsync case for Keno Lottery.
                            bool isHideKenoLottery = Converter.ToBool(row["IsHidden"]);
                            bool isHideUplineKenoLottery = Converter.ToBool(row["IsHiddenUpline"]);

                            HideKenoLottery = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery == 0 ||
                                IsDeposit ||
                                (RoleId < 4 && (isHideKenoLottery || isHideUplineKenoLottery));
                            break;

                        case "26":
                            IsHideLucky3 = (Converter.ToBool(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowLucky3;
                            IsHideLucky3Upline = Converter.ToBool(row["IsHiddenUpline"]) && RoleId < 4;
                            break;
                    }
                }
            }
        }
    }
}
