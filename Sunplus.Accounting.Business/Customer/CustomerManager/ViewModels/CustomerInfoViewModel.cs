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

    public class CustomerInfoViewModel
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

            switch (RoleId)
            {
                case 1: // member
                    ParentId = Convert.ToInt32(row2["agentId"]);
                    AccOutstanding = Convert.IsDBNull(row3["outstanding"]) ? 0 : Convert.ToDecimal(row3["outstanding"]);
                    Cash = Convert.IsDBNull(row3["cash"]) ? 0 : Convert.ToDecimal(row3["cash"]);
                    NetPosition = Convert.ToDecimal(row1["netPosition"]);
                    BetCredit = Credit + Cash - AccOutstanding + NetPosition;
                    YesterdayCash = Convert.IsDBNull(row3["Yesterday_Cash"]) ? 0 : Convert.ToDecimal(row3["Yesterday_Cash"]);
                    DownlineName = string.Empty;

                    break;

                case 2: // agent
                    ParentId = Convert.ToInt32(row2["masterid"]);
                    AccOutstanding = Convert.IsDBNull(row3["agentPositionTaking"]) ? 0 : Convert.ToDecimal(row3["agentPositionTaking"]);
                    OutStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                    Cash = Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]);
                    TotalBal = Cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"]));
                    YesterdayCash = Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]);
                    YesterdayBal = YesterdayCash + (Convert.IsDBNull(row3["Yesterday_CustomerCash"]) ? 0 : Convert.ToDecimal(row3["Yesterday_CustomerCash"]));
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(row3["agentUsedCredit"]);
                    DownlineName = "Member";

                    break;

                case 3: // master
                    ParentId = Convert.ToInt32(row2["superId"]);
                    AccOutstanding = Convert.IsDBNull(row3["masterPositionTaking"]) ? 0 : Convert.ToDecimal(row3["masterPositionTaking"]);
                    OutStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                    Cash = Convert.IsDBNull(row3["masterCash"]) ? 0 : Convert.ToDecimal(row3["masterCash"]);
                    TotalBal = Cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"])) +
                        (Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]));
                    YesterdayCash = Convert.IsDBNull(row3["yesterday_masterCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_masterCash"]);
                    YesterdayBal = YesterdayCash + (Convert.IsDBNull(row3["yesterday_customerCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_customerCash"])) +
                        (Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]));
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(row3["masterUsedCredit"]);
                    DownlineName = "Agent";

                    break;

                case 4: // super
                    ParentId = 0;
                    AccOutstanding = Convert.IsDBNull(row3["superPositionTaking"]) ? 0 : Convert.ToDecimal(row3["superPositionTaking"]);
                    OutStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                    Cash = Convert.IsDBNull(row3["superCash"]) ? 0 : Convert.ToDecimal(row3["superCash"]);
                    TotalBal = Cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"]))
                        + (Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]))
                        + (Convert.IsDBNull(row3["masterCash"]) ? 0 : Convert.ToDecimal(row3["masterCash"]));
                    YesterdayCash = Convert.IsDBNull(row3["yesterday_superCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_superCash"]);
                    YesterdayBal = YesterdayCash + (Convert.IsDBNull(row3["yesterday_customerCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_customerCash"]))
                        + (Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]))
                        + (Convert.IsDBNull(row3["yesterday_masterCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_masterCash"]));
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(row3["superUsedCredit"]);
                    DownlineName = "Master";

                    break;

                default:
                    break;
            }

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
                            IsSyncAllbetDownline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedDownline"]);
                            break;

                        case "22":
                            IsHideVoidbridge = (Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) && RoleId < 4) || !WebSettings.IsShowVoidbridge;
                            IsSyncVoidbridge = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSync"]);
                            IsHideVoidbridgeUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && RoleId < 4;
                            IsSyncVoidbridgeUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);
                            IsSyncVoidbridgeDownline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedDownline"]);
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

        public string UserName => row1["username"].ToString();

        public string Firstname => Convert.IsDBNull(row1["firstname"]) ? string.Empty : row1["firstname"].ToString();

        public string Lastname => Convert.IsDBNull(row1["lastname"]) ? string.Empty : row1["lastname"].ToString();

        public string Fullname => Firstname + " - " + Lastname;

        public string DownlineName { get; set; }

        public int CustId => Convert.ToInt32(row1["custid"]);

        public int RoleId => Convert.ToInt32(row1["roleid"]);

        public string SiteName 
            => row1["site"] == null || string.IsNullOrWhiteSpace(row1["site"].ToString()) ? row1["membersite"].ToString() : row1["site"].ToString();

        public bool IsDeposit => WebSettings.IsApplySplitingDepositeCustomer && DNAide.Core.CastAide.AsBoolean(row1["IsDeposit"]);

        public bool IsLicensee => IsDeposit;

        public bool IsIMBC => SiteName.ToString().ToUpper().Equals("IMBC");

        public Hashtable JsParam { get; set; }

        public string LastLoginDate => Convert.IsDBNull(row1["lastlogged"]) ? string.Empty : Convert.ToDateTime(row1["lastlogged"]).ToShortDateString();

        public string LastLoginTime => Convert.IsDBNull(row1["lastlogged"]) ? string.Empty : " " + Convert.ToDateTime(row1["lastlogged"]).ToShortTimeString();

        public string LastIp => Convert.IsDBNull(row1["lastloginip"]) ? string.Empty : row1["lastloginip"].ToString();

        public string CustomerSite => (RoleId == (int)Globals.AccountType.Member) ? CastAide.AsString(row1["membersite"], string.Empty) : CastAide.AsString(row1["site"], string.Empty);

        public string Createdate => Convert.IsDBNull(row1["createdate"]) ? string.Empty : Convert.ToDateTime(row1["createdate"]).ToShortDateString();

        public string Createtime => Convert.IsDBNull(row1["createdate"]) ? string.Empty : Convert.ToDateTime(row1["createdate"]).ToShortTimeString();

        public string PwdExpiryDate => Convert.IsDBNull(row1["pwdexpiry"]) ? string.Empty : Convert.ToDateTime(row1["pwdexpiry"]).ToShortDateString();

        public string PwdExpiryTime => Convert.IsDBNull(row1["pwdexpiry"]) ? string.Empty : " " + Convert.ToDateTime(row1["pwdexpiry"]).ToShortTimeString();

        public decimal Credit => Convert.ToDecimal(row1["credit"]);

        public decimal ActualRate => Convert.ToDecimal(row1["actualrate"]);

        public string Currency => row1["currency"].ToString();

        public bool Important => !Convert.IsDBNull(row1["important"]) && Convert.ToBoolean(row1["important"]);

        public bool Suspend => !Convert.IsDBNull(row1["suspended"]) && Convert.ToBoolean(row1["suspended"]);

        public bool Autosuspend => !Convert.IsDBNull(row1["autosuspend"]) && Convert.ToBoolean(row1["autosuspend"]);

        public bool UplineAutoSuspend => CastAide.AsBoolean(row2["uplineautosuspend"]);

        public bool IsHideColossusBets => (CastAide.AsBoolean(row1["HideColossusBet"]) || !WebSettings.IsShowColossusBets) && (RoleId < 4);

        public bool IsHideSportsbook2 { get; set; } = true;

        public bool IsHideCockFighting { get; set; } = true;

        public bool IsHideCricketSettingLink { get; set; } = true;

        public bool IsHideAllBet { get; set; } = true;

        public bool IsHideAllbetUpline { get; set; } = true;

        public bool IsSyncAllbet { get; set; } = true;

        public bool IsSyncAllbetUpline { get; set; } = true;

        public bool IsSyncAllbetDownline { get; set; } = true;

        public bool IsHideVoidbridge { get; set; } = true;

        public bool IsHideVoidbridgeUpline { get; set; } = true;

        public bool IsSyncVoidbridge { get; set; } = true;

        public bool IsSyncVoidbridgeUpline { get; set; } = true;

        public bool IsSyncVoidbridgeDownline { get; set; } = true;

        public bool IsHideNG2 { get; set; } = true;

        public bool IsHideNG2Upline { get; set; } = true;
        public bool IsSyncNG2 { get; set; } = true;

        public bool IsSyncNG2Upline { get; set; } = true;

        public bool IsHideVirtualSports { get; set; } = true;

        public bool IsHideVirtualSports2 { get; set; } = true;

        public bool HideEnableAndEditFishingWorld { get; set; } = true;

        public bool HideKenoLottery { get; set; } = true;

        public bool IsHideLucky3 { get; set; } = true;

        public bool IsHideLucky3Upline { get; set; } = true;

        public bool Closed => !Convert.IsDBNull(row1["closed"]) && Convert.ToBoolean(row1["closed"]);

        public bool IsBA => CastAide.AsInt32(row1["isba"]) == 1;

        public int ParentId { get; set; }

        public string LastTxnDate
            => row3 != null && RoleId <= 4 ? Convert.IsDBNull(row3["lastTxn"]) ? string.Empty : Convert.ToDateTime(row3["lastTxn"]).ToShortDateString() : string.Empty;

        public string LastTxnTime
            => row3 != null && RoleId <= 4 ? Convert.IsDBNull(row3["lastTxn"]) ? string.Empty : " " + Convert.ToDateTime(row3["lastTxn"]).ToShortTimeString() : string.Empty;

        public string Phone => row1["phone"].ToString();

        public string Mobile => row1["mobilephone"].ToString();

        public string Fax => row1["fax"].ToString();

        public string Nickname => CastAide.AsString(row1["username2"], string.Empty);

        public decimal OutStanding { get; set; }

        public decimal AccOutstanding { get; set; }

        public decimal Cash { get; set; }

        public decimal TotalBal { get; set; }

        public decimal YesterdayCash { get; set; }

        public decimal YesterdayBal { get; set; }
        
        public decimal BetCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public decimal NetPosition { get; set; }

        public bool SyncCasino => CastAide.AsBoolean(row1["synccasino"], false);

        public bool SyncBingo => CastAide.AsBoolean(row1["syncbingo"], false);

        public bool SyncLiveCasino => WebSettings.EnableLiveCasino && CastAide.AsBoolean(row1["synclivecasino"], false);

        public bool Enabled => CastAide.AsBoolean(row1["Enabled"]);

        public string ClosedReason => row1["closedreason"].ToString();

        public bool Doublecommlosing => !Convert.IsDBNull(row1["DoubleCommLosing"]) && Convert.ToBoolean(row1["DoubleCommLosing"]);

        public bool NicknameLoginOnly => !Convert.IsDBNull(row1["NicknameLoginOnly"]) && Convert.ToBoolean(row1["NicknameLoginOnly"]);

        public string LastChangeSecurityCodeDate => Convert.IsDBNull(row1["LastChangedSecurityCodeDate"]) ?
                    string.Empty :
                    Convert.ToString(row1["LastChangedSecurityCodeDate"]);

        public string LastChangeOTPCodeDate => Convert.IsDBNull(row1["OTPCreateDate"]) ?
                  string.Empty :
                  Convert.ToString(row1["OTPCreateDate"]);

        public bool IsAllowEditCS =>
            ParentId.Equals(0) || Permissions.IsAllowEditCasino(ParentId, CustParent.IsCasinoSync);

        public bool DisabledCSParent => !ParentId.Equals(0) && CustParent.IsDisableCasino;

        public bool IsAllowEditBI =>
            ParentId.Equals(0) || Permissions.IsAllowEditBingo(ParentId, CustParent.IsBingoSync);

        public bool DisabledBIParent => !ParentId.Equals(0) && CustParent.IsDisabledBingoGame;

        public bool IsAllowEditLCS =>
            ParentId.Equals(0) || Permissions.IsAllowEditLiveCasino(ParentId, CustParent.IsLiveCasinoSync);

        public bool DisabledLCSParent => !ParentId.Equals(0) && CustParent.IsDisableLiveCasino;

        public bool DisalbedVSParent => !ParentId.Equals(0) && CustParent.IsDisableVirtualSports;

        public bool DisabledEsportsParent => !ParentId.Equals(0) && CustParent.IsDisableEsports;

        public bool DisabledEsports => !Convert.IsDBNull(row1["DisableEsports"]) && Convert.ToBoolean(row1["DisableEsports"]);

        public bool DisabledEsportsDownline => !Convert.IsDBNull(row1["disabledownlinesesports"]) && Convert.ToBoolean(row1["disabledownlinesesports"]);

        public int LimitPerIP => CastAide.AsInt16(row1["LimitPerIP"]);

        public int Danger => Convert.IsDBNull(row1["danger"]) ? 0 : Convert.ToInt32(row1["danger"]);
        
        public int Danger2 => Convert.IsDBNull(row1["danger2"]) ? 0 : Convert.ToInt32(row1["danger2"]);

        public int Danger3 => Convert.IsDBNull(row1["danger3"]) ? 0 : Convert.ToInt32(row1["danger3"]);

        public bool HasDownline => Converter.ToBool(row1["HasDownline"]);

        public bool IsSyncUplineGoldDeluxe => Converter.ToBool(row1["IsSyncUplineGoldDeluxe"]);

        public int Abnormal => Convert.IsDBNull(row1["abnormal"]) ? 0 : Convert.ToInt32(row1["abnormal"]);

        public bool SelectedBetting => !Convert.IsDBNull(row1["SelectedBetting"]) && Convert.ToBoolean(row1["SelectedBetting"]);

        public bool IsInternalUser => !Convert.IsDBNull(row1["IsInternal"]) && Convert.ToBoolean(row1["IsInternal"]);

        public bool IsHideDirectStatus => Converter.ToBool(row1["IsHideDirectStatus"]);

        public bool IsCheckedDirectStatus => Converter.ToBool(row1["IsCheckDirectStatus"]);

        public bool IsDisabledDirectStatus => Converter.ToBool(row1["IsDisabledDirectStatus"]);

        public bool IsAllDayBalTransfer => row1.Table.Columns.Contains("isalldaybaltransfer") && Convert.ToBoolean(row1["isalldaybaltransfer"]);

        public bool DisableAutoMPT => !Convert.IsDBNull(row3["DisableAutoMPT"]) && !Convert.ToBoolean(row3["DisableAutoMPT"]);

        public bool DisableOutright => !Convert.IsDBNull(row1["DisableOutright"]) && Convert.ToBoolean(row1["DisableOutright"]);

        public bool IsEnableHorseRacingFixedOdds => !Convert.IsDBNull(row1["DisableHRFixedOdds"]) && !Convert.ToBoolean(row1["DisableHRFixedOdds"]);

        public bool DisableDownlinesHorseRacingFixedOdds => !Convert.IsDBNull(row1["DisableDownlinesHRFixedOdds"]) && Convert.ToBoolean(row1["DisableDownlinesHRFixedOdds"]);

        public bool DisabledNumberGame => !Convert.IsDBNull(row1["DisableNumber"]) && Convert.ToBoolean(row1["DisableNumber"]);

        public bool DisabledNumberGameDL => !Convert.IsDBNull(row1["DisableDownlinesNumber"]) && Convert.ToBoolean(row1["DisableDownlinesNumber"]);

        public bool DisabledCasino => !Convert.IsDBNull(row1["disablecasino"]) && Convert.ToBoolean(row1["disablecasino"]);

        public bool DisabledCSDL => !Convert.IsDBNull(row1["DisableDownlinesCS"]) && Convert.ToBoolean(row1["DisableDownlinesCS"]);

        public bool DisabledLiveCasino => !WebSettings.EnableLiveCasino || !Convert.IsDBNull(row1["disableLiveCasino"]) && Convert.ToBoolean(row1["disableLiveCasino"]);

        public bool DisabledLCSDL => !(!WebSettings.EnableLiveCasino || Convert.IsDBNull(row1["DisableDownlinesLiveCS"])) && Convert.ToBoolean(row1["DisableDownlinesLiveCS"]);

        public int Groupheadlineid => Convert.IsDBNull(row1["groupheadlineid"]) ? 0 : Convert.ToInt32(row1["groupheadlineid"]);

        public string Headline => row1["headline"].ToString();

        public int SuperId => CastAide.AsInt32(row2["SuperID"]);

        public string  SuperName => CastAide.AsString(row2["SuperName"]);

        public int MasterId => CastAide.AsInt32(row2["MasterID"]);

        public string MasterName => CastAide.AsString(row2["MasterName"]);

        public int AgentId => CastAide.AsInt32(row2["AgentID"]);

        public string AgentName => CastAide.AsString(row2["AgentName"]);

        public int MemberId => CastAide.AsInt32(row2["MemberID"]);

        public string MemberName => CastAide.AsString(row2["MemberName"]);

        public bool SuperEnabled => Convert.ToBoolean(row2["SuperEnabled"]);

        public bool MasterEnabled => Convert.ToBoolean(row2["MasterEnabled"]);

        public bool AgentEnabled => Convert.ToBoolean(row2["AgentEnabled"]);

        public bool EnableVirtualRate => Convert.ToBoolean(row1["EnableVirtualRate"]);

        public bool HasReadPermission_AutoSuspend => Permissions.HasPermission(Enumerations.Functions.Customer.AutoSuspend);

        public bool IsManageCustomerAccess => Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust);

        public bool IsAllowUpdateGivenCredit => Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

        public bool IsViewFiAcc => Permissions.FinancePermission.IsViewFiAcc(IsInternalUser);

        public bool IsShowKenoStatus => KenoSection.Instance.Enable;

        public bool IsShowEnableBetTrade => WebSettings.EnableBetTrade && RoleId.Equals(4) && IsDeposit;

        public bool IsShowEnableCashOut => WebSettings.EnableCashOut && RoleId == 4 && IsDeposit;

        public bool IsShowEnableMicrogaming => WebSettings.EnableMicrogaming && IsDeposit;

        public bool IsShowMacau => WebSettings.EnableMacau && RoleId == 4 && IsDeposit;

        public bool IsShowColossusBetsForCredit => !IsHideColossusBets;

        public bool IsShowSportsbook2 => !IsHideSportsbook2 && !IsDeposit;

        public bool IsShowCockFighting => !IsHideCockFighting && !IsDeposit;

        public bool HideCheckBoxAllbet => !WebSettings.IsShowAllBet || (RoleId < 4 && IsHideAllbetUpline) || !IsSyncAllbetUpline;

        public bool HideCheckBoxVoidbridge => !WebSettings.IsShowVoidbridge || (RoleId < 4 && IsHideVoidbridgeUpline) || !IsSyncVoidbridgeUpline;

        public bool HideCheckBoxNG2 => !WebSettings.IsShowNG2 || IsHideNG2Upline || !IsSyncNG2Upline || !IsSyncNG2 || IsHideNG2;

        public bool HideCheckBoxLucky3 => !WebSettings.IsShowLucky3 || IsHideLucky3Upline || IsHideLucky3;

        public CustomerDetail CustParent => ParentId > 0 ? IBCProvider.IBCCustomer.GetCustomerStatus(ParentId) : new CustomerDetail();
    }
}
