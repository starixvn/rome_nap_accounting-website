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
        private DataRow row1 { get; set; }
        private DataRow row2 { get; set; }
        private DataRow row3 { get; set; }
        private DataTable dynamicSettingtable { get; set; }

        public CustomerInfoViewModel(DataRow row1, DataRow row2, DataRow row3, DataTable dynamicSettingtable)
        {
            this.row1 = row1;
            this.row2 = row2;
            this.row3 = row3;
            this.dynamicSettingtable = dynamicSettingtable;

            JsParam = new Hashtable();
            JsParam.Add("custid", CustId);
            JsParam.Add("roleid", RoleId);

            GetExtraCustomerInfo(row1, row2, row3);
            GetShowedHiddenProduct(dynamicSettingtable);
        }

        public Hashtable JsParam { get; set; }

        public bool HasReadPermission_AutoSuspend => Permissions.HasPermission(Enumerations.Functions.Customer.AutoSuspend);

        public bool IsManageCustomerAccess => Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust);

        public bool IsAllowUpdateGivenCredit => Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

        public bool IsViewFiAcc => Permissions.FinancePermission.IsViewFiAcc(IsInternalUser);

        public bool IsHideDirectStatus => Converter.ToBool(row1["IsHideDirectStatus"]);

        public bool IsCheckedDirectStatus => Converter.ToBool(row1["IsCheckDirectStatus"]);

        public bool IsDisabledDirectStatus => Converter.ToBool(row1["IsDisabledDirectStatus"]);

        public bool IsAllDayBalTransfer => row1.Table.Columns.Contains("isalldaybaltransfer") && Convert.ToBoolean(row1["isalldaybaltransfer"]);

        public bool DisableAutoMPT => !Convert.IsDBNull(row3["DisableAutoMPT"]) && !Convert.ToBoolean(row3["DisableAutoMPT"]);

        public bool DisableOutright => !Convert.IsDBNull(row1["DisableOutright"]) && Convert.ToBoolean(row1["DisableOutright"]);

        public bool IsHideCricketSettingLink { get; set; } = true;

        public bool HideEnableAndEditFishingWorld { get; set; } = true;

        public bool HideKenoLottery { get; set; } = true;

        public bool IsHideVirtualSports2 { get; set; } = true;

        public bool IsSyncUplineGoldDeluxe => Converter.ToBool(row1["IsSyncUplineGoldDeluxe"]);

        public bool IsEnableHorseRacingFixedOdds => !Convert.IsDBNull(row1["DisableHRFixedOdds"]) && !Convert.ToBoolean(row1["DisableHRFixedOdds"]);

        public bool DisableDownlinesHorseRacingFixedOdds => !Convert.IsDBNull(row1["DisableDownlinesHRFixedOdds"]) && Convert.ToBoolean(row1["DisableDownlinesHRFixedOdds"]);

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
