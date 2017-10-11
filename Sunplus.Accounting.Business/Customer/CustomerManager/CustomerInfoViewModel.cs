namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Collections;
    using System.Data;
    using Core.Common;
    using Core.Configurations;
    using DNAide.Core;

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

            jsparam = new Hashtable();
            jsparam.Add("custid", custId);
            jsparam.Add("roleid", roleId);

            switch (roleId)
            {
                case 1: // member
                    parentId = Convert.ToInt32(row2["agentId"]);
                    accOutstanding = Convert.IsDBNull(row3["outstanding"]) ? 0 : Convert.ToDecimal(row3["outstanding"]);
                    cash = Convert.IsDBNull(row3["cash"]) ? 0 : Convert.ToDecimal(row3["cash"]);
                    netPosition = Convert.ToDecimal(row1["netPosition"]);
                    betCredit = credit + cash - accOutstanding + netPosition;
                    yesterdayCash = Convert.IsDBNull(row3["Yesterday_Cash"]) ? 0 : Convert.ToDecimal(row3["Yesterday_Cash"]);
                    downlinename = "";

                    break;

                case 2: // agent
                    parentId = Convert.ToInt32(row2["masterid"]);
                    accOutstanding = Convert.IsDBNull(row3["agentPositionTaking"]) ? 0 : Convert.ToDecimal(row3["agentPositionTaking"]);
                    outStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                    cash = Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]);
                    totalBal = cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"]));
                    yesterdayCash = Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]);
                    yesterdayBal = yesterdayCash + (Convert.IsDBNull(row3["Yesterday_CustomerCash"]) ? 0 : Convert.ToDecimal(row3["Yesterday_CustomerCash"]));
                    usedCredit = IBCUtils.FormatHandler.GetDecimal(row3["agentUsedCredit"]);
                    downlinename = "Member";

                    break;

                case 3: // master
                    parentId = Convert.ToInt32(row2["superId"]);
                    accOutstanding = Convert.IsDBNull(row3["masterPositionTaking"]) ? 0 : Convert.ToDecimal(row3["masterPositionTaking"]);
                    outStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                    cash = Convert.IsDBNull(row3["masterCash"]) ? 0 : Convert.ToDecimal(row3["masterCash"]);
                    totalBal = cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"])) +
                        (Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]));
                    yesterdayCash = Convert.IsDBNull(row3["yesterday_masterCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_masterCash"]);
                    yesterdayBal = yesterdayCash + (Convert.IsDBNull(row3["yesterday_customerCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_customerCash"])) +
                        (Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]));
                    usedCredit = IBCUtils.FormatHandler.GetDecimal(row3["masterUsedCredit"]);
                    downlinename = "Agent";

                    break;

                case 4: // super
                    parentId = 0;
                    accOutstanding = Convert.IsDBNull(row3["superPositionTaking"]) ? 0 : Convert.ToDecimal(row3["superPositionTaking"]);
                    outStanding = Convert.IsDBNull(row3["customerOutstanding"]) ? 0 : Convert.ToDecimal(row3["customerOutstanding"]);
                    cash = Convert.IsDBNull(row3["superCash"]) ? 0 : Convert.ToDecimal(row3["superCash"]);
                    totalBal = cash + (Convert.IsDBNull(row3["customerCash"]) ? 0 : Convert.ToDecimal(row3["customerCash"]))
                        + (Convert.IsDBNull(row3["agentCash"]) ? 0 : Convert.ToDecimal(row3["agentCash"]))
                        + (Convert.IsDBNull(row3["masterCash"]) ? 0 : Convert.ToDecimal(row3["masterCash"]));
                    yesterdayCash = Convert.IsDBNull(row3["yesterday_superCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_superCash"]);
                    yesterdayBal = yesterdayCash + (Convert.IsDBNull(row3["yesterday_customerCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_customerCash"]))
                        + (Convert.IsDBNull(row3["yesterday_agentCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_agentCash"]))
                        + (Convert.IsDBNull(row3["yesterday_masterCash"]) ? 0 : Convert.ToDecimal(row3["yesterday_masterCash"]));
                    usedCredit = IBCUtils.FormatHandler.GetDecimal(row3["superUsedCredit"]);
                    downlinename = "Master";

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
                            isHideVirtualSports = (CastAide.AsBoolean(row["IsHiddenUpline"]) && roleId < 4) || !WebSettings.EnableVirtualSports;
                            break;
                        case "16":
                            isHideSportsbook2 = (CastAide.AsBoolean(row["IsHidden"]) && roleId < 4) || !WebSettings.IsShowSportsbook2 || isDeposit;
                            break;
                        case "19":
                            isHideCockFighting = (CastAide.AsBoolean(row["IsHidden"]) && roleId < 4) || !WebSettings.IsShowCockFighting || isDeposit;
                            break;
                        case "20":
                            isHideCricketSettingLink = (CastAide.AsBoolean(row["IsHidden"]) && roleId < 4) || !WebSettings.IsShowCricket || !isIMBC || isDeposit;
                            break;
                        case "21":
                            isHideAllBet = (Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) && roleId < 4) || !WebSettings.IsShowAllBet || isDeposit;
                            isSyncAllbet = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSync"]);
                            isHideAllbetUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && roleId < 4;
                            isSyncAllbetUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);
                            isSyncAllbetDownline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedDownline"]);
                            break;

                        case "22":
                            isHideVoidbridge = (Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) && roleId < 4) || !WebSettings.IsShowVoidbridge;
                            isSyncVoidbridge = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSync"]);
                            isHideVoidbridgeUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && roleId < 4;
                            isSyncVoidbridgeUpline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);
                            isSyncVoidbridgeDownline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedDownline"]);
                            break;

                        case "23":
                            isHideNG2 = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowNG2;
                            isSyncNG2 = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSync"]);
                            isHideNG2Upline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && roleId < 4;
                            isSyncNG2Upline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);
                            break;
                        case "24":
                            bool isHideUplineFishingWorld = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]);
                            bool isSyncUplineFishingWorld = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsSyncedUpline"]);

                            hideEnableAndEditFishingWorld = (roleId < 4 && isHideUplineFishingWorld) || !isSyncUplineFishingWorld;

                            break;
                        case "27":
                            isHideVirtualSports2 = (CastAide.AsBoolean(row["IsHiddenUpline"]) && roleId < 4) || !WebSettings.EnableVirtualSports;
                            break;

                        case "28": // We do NOT have Unsync case for Keno Lottery.
                            bool isHideKenoLottery = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]);
                            bool isHideUplineKenoLottery = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]);

                            hideKenoLottery = Fanex.Tarus.Infrastructure.Configurations.AppSettings.ProductIdKenoLottery == 0 ||
                                isDeposit ||
                                (roleId < 4 && (isHideKenoLottery || isHideUplineKenoLottery));
                            break;

                        case "26":
                            isHideLucky3 = (Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHidden"]) && roleId < 4) || !WebSettings.IsShowLucky3;
                            isHideLucky3Upline = Fanex.CustomerSetting.Module.Core.Utils.ToBoolean(row["IsHiddenUpline"]) && roleId < 4;
                            break;
                    }
                }
            }
        }

        public string username => row1["username"].ToString();

        public string firstname => Convert.IsDBNull(row1["firstname"]) ? "" : row1["firstname"].ToString();

        public string lastname => Convert.IsDBNull(row1["lastname"]) ? "" : row1["lastname"].ToString();

        public string downlinename { get; set; }

        public int custId => Convert.ToInt32(row1["custid"]);

        public int roleId => Convert.ToInt32(row1["roleid"]);

        public string siteName 
            => row1["site"] == null || string.IsNullOrWhiteSpace(row1["site"].ToString()) ? row1["membersite"].ToString() : row1["site"].ToString();

        public bool isDeposit => WebSettings.IsApplySplitingDepositeCustomer ? DNAide.Core.CastAide.AsBoolean(row1["IsDeposit"]) : false;

        public bool isLicensee => isDeposit;

        public bool isIMBC => siteName.ToString().ToUpper().Equals("IMBC");

        public Hashtable jsparam { get; set; }

        public string lastLoginDate => Convert.IsDBNull(row1["lastlogged"]) ? "" : Convert.ToDateTime(row1["lastlogged"]).ToShortDateString();

        public string lastLoginTime => Convert.IsDBNull(row1["lastlogged"]) ? "" : " " + Convert.ToDateTime(row1["lastlogged"]).ToShortTimeString();

        public string lastIp => Convert.IsDBNull(row1["lastloginip"]) ? "" : row1["lastloginip"].ToString();

        public string customerSite => (roleId == (int)Globals.AccountType.Member) ? CastAide.AsString(row1["membersite"], "") : CastAide.AsString(row1["site"], "");

        public string createdate => Convert.IsDBNull(row1["createdate"]) ? "" : Convert.ToDateTime(row1["createdate"]).ToShortDateString();

        public string createtime => Convert.IsDBNull(row1["createdate"]) ? "" : Convert.ToDateTime(row1["createdate"]).ToShortTimeString();

        public string pwdExpiryDate => Convert.IsDBNull(row1["pwdexpiry"]) ? "" : Convert.ToDateTime(row1["pwdexpiry"]).ToShortDateString();

        public string pwdExpiryTime => Convert.IsDBNull(row1["pwdexpiry"]) ? "" : " " + Convert.ToDateTime(row1["pwdexpiry"]).ToShortTimeString();

        public decimal credit => Convert.ToDecimal(row1["credit"]);

        public decimal actualRate => Convert.ToDecimal(row1["actualrate"]);

        public string currency => row1["currency"].ToString();

        public bool important => Convert.IsDBNull(row1["important"]) ? false : Convert.ToBoolean(row1["important"]);

        public bool suspend => Convert.IsDBNull(row1["suspended"]) ? false : Convert.ToBoolean(row1["suspended"]);

        public bool autosuspend => Convert.IsDBNull(row1["autosuspend"]) ? false : Convert.ToBoolean(row1["autosuspend"]);

        public bool UplineAutoSuspend => CastAide.AsBoolean(row2["uplineautosuspend"]);

        public bool isHideColossusBets => (CastAide.AsBoolean(row1["HideColossusBet"]) || !WebSettings.IsShowColossusBets) && (roleId < 4);

        public bool isHideSportsbook2 { get; set; } = true;

        public bool isHideCockFighting { get; set; } = true;

        public bool isHideCricketSettingLink { get; set; } = true;

        public bool isHideAllBet { get; set; } = true;

        public bool isHideAllbetUpline { get; set; } = true;

        public bool isSyncAllbet { get; set; } = true;

        public bool isSyncAllbetUpline { get; set; } = true;

        public bool isSyncAllbetDownline { get; set; } = true;

        public bool isHideVoidbridge { get; set; } = true;

        public bool isHideVoidbridgeUpline { get; set; } = true;

        public bool isSyncVoidbridge { get; set; } = true;

        public bool isSyncVoidbridgeUpline { get; set; } = true;

        public bool isSyncVoidbridgeDownline { get; set; } = true;

        public bool isHideNG2 { get; set; } = true;

        public bool isHideNG2Upline { get; set; } = true;
        public bool isSyncNG2 { get; set; } = true;

        public bool isSyncNG2Upline { get; set; } = true;

        public bool isHideVirtualSports { get; set; } = true;

        public bool isHideVirtualSports2 { get; set; } = true;

        public bool hideEnableAndEditFishingWorld { get; set; } = true;

        public bool hideKenoLottery { get; set; } = true;

        public bool isHideLucky3 { get; set; } = true;

        public bool isHideLucky3Upline { get; set; } = true;

        public bool closed => Convert.IsDBNull(row1["closed"]) ? false : Convert.ToBoolean(row1["closed"]);

        public bool isBA => CastAide.AsInt32(row1["isba"]) == 1 ? true : false;

        public int parentId { get; set; }

        public string lastTxnDate
            => row3 != null && roleId <= 4 ? Convert.IsDBNull(row3["lastTxn"]) ? "" : Convert.ToDateTime(row3["lastTxn"]).ToShortDateString() : string.Empty;

        public string lastTxnTime
            => row3 != null && roleId <= 4 ? Convert.IsDBNull(row3["lastTxn"]) ? "" : " " + Convert.ToDateTime(row3["lastTxn"]).ToShortTimeString() : string.Empty;

        public string phone => row1["phone"] + "";

        public string mobile => row1["mobilephone"] + "";

        public string fax => row1["fax"] + "";

        public string nickname => CastAide.AsString(row1["username2"], string.Empty);

        public decimal outStanding { get; set; }

        public decimal accOutstanding { get; set; }

        public decimal cash { get; set; }

        public decimal totalBal { get; set; }

        public decimal yesterdayCash { get; set; }

        public decimal yesterdayBal { get; set; }
        
        public decimal betCredit { get; set; }

        public decimal usedCredit { get; set; }

        public decimal netPosition { get; set; }

        public bool synccasino => CastAide.AsBoolean(row1["synccasino"], false);

        public bool syncbingo => CastAide.AsBoolean(row1["syncbingo"], false);

        public bool synclivecasino => WebSettings.EnableLiveCasino ? CastAide.AsBoolean(row1["synclivecasino"], false) : false;

        public bool Enabled => CastAide.AsBoolean(row1["Enabled"]);

        public string closedreason => row1["closedreason"].ToString();
    }
}
