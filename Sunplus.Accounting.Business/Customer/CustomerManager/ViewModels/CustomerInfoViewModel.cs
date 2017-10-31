namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Collections;
    using System.Data;
    using Core.Common;
    using Core.Configurations;
    using DataAccess;
    using DNAide.Core;
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

        public bool IsAllDayBalTransfer => customerInfoRow.Table.Columns.Contains("isalldaybaltransfer") && Convert.ToBoolean(customerInfoRow["isalldaybaltransfer"]);

        public bool DisableAutoMPT => !Convert.IsDBNull(cashRow["DisableAutoMPT"]) && !Convert.ToBoolean(cashRow["DisableAutoMPT"]);

        public bool DisableOutright => !Convert.IsDBNull(customerInfoRow["DisableOutright"]) && Convert.ToBoolean(customerInfoRow["DisableOutright"]);

        public bool IsHideCricketSettingLink { get; private set; } = true;

        public bool HideEnableAndEditFishingWorld { get; private set; } = true;

        public bool HideKenoLottery { get; private set; } = true;

        public bool IsHideVirtualSports2 { get; private set; } = true;

        public bool IsSyncUplineGoldDeluxe => Converter.ToBool(customerInfoRow["IsSyncUplineGoldDeluxe"]);

        public bool IsEnableHorseRacingFixedOdds => !Convert.IsDBNull(customerInfoRow["DisableHRFixedOdds"]) && !Convert.ToBoolean(customerInfoRow["DisableHRFixedOdds"]);

        public bool DisableDownlinesHorseRacingFixedOdds => !Convert.IsDBNull(customerInfoRow["DisableDownlinesHRFixedOdds"]) && Convert.ToBoolean(customerInfoRow["DisableDownlinesHRFixedOdds"]);

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
                            IsHideVirtualSports = (CastAide.AsBoolean(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports;
                            break;
                        case "16":
                            IsHideSportsbook2 = (CastAide.AsBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowSportsbook2 || IsDeposit;
                            break;
                        case "19":
                            IsHideCockFighting = (CastAide.AsBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowCockFighting || IsDeposit;
                            break;
                        case "20":
                            IsHideCricketSettingLink = (CastAide.AsBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowCricket || !IsIMBC || IsDeposit;
                            break;
                        case "21":
                            IsHideAllBet = (Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowAllBet || IsDeposit;
                            IsSyncAllbet = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSync"]);
                            IsHideAllbetUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            IsSyncAllbetUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);
                            break;

                        case "22":
                            IsHideVoidbridge = (Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowVoidbridge;
                            IsSyncVoidbridge = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSync"]);
                            IsHideVoidbridgeUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            IsSyncVoidbridgeUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);
                            break;

                        case "23":
                            IsHideNG2 = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowNG2;
                            IsSyncNG2 = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSync"]);
                            IsHideNG2Upline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            IsSyncNG2Upline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);

                            break;
                        case "24":
                            bool isHideUplineFishingWorld = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]);
                            bool isSyncUplineFishingWorld = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);

                            HideEnableAndEditFishingWorld = (RoleId < 4 && isHideUplineFishingWorld) || !isSyncUplineFishingWorld;

                            break;
                        case "27":
                            IsHideVirtualSports2 = (CastAide.AsBoolean(row["IsHiddenUpline"]) && RoleId < 4) || !WebSettings.EnableVirtualSports;
                            break;

                        case "28": // We do NOT have Unsync case for Keno Lottery.
                            bool isHideKenoLottery = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]);
                            bool isHideUplineKenoLottery = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]);

                            HideKenoLottery = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery == 0 ||
                                IsDeposit ||
                                (RoleId < 4 && (isHideKenoLottery || isHideUplineKenoLottery));
                            break;

                        case "26":
                            IsHideLucky3 = (Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowLucky3;
                            IsHideLucky3Upline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            break;
                    }
                }
            }
        }
    }
}
