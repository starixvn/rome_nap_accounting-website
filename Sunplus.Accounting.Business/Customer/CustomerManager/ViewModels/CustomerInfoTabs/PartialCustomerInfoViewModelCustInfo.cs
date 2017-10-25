namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using Core.Common;
    using Core.Configurations;
    using DNAide.Core;

    public partial class CustomerInfoViewModel
    {
        public int ParentId { get; set; }

        public int CustId => CastAide.AsInt32(row1["custid"]);

        public int RoleId => CastAide.AsInt32(row1["roleid"]);

        public string UserName => CastAide.AsString(row1["username"]);

        public string Firstname => CastAide.AsString(row1["firstname"], string.Empty);

        public string Lastname => CastAide.AsString(row1["lastname"], string.Empty);

        public string Fullname => $"{Firstname} - {Lastname}";

        public string DownlineName { get; set; }

        public string SiteName => string.IsNullOrWhiteSpace(CastAide.AsString(row1["site"]))? CastAide.AsString(row1["membersite"]) : CastAide.AsString(row1["site"]);

        public bool Important => CastAide.AsBoolean(row1["important"], false);

        public bool Suspend => CastAide.AsBoolean(row1["suspended"], false);

        public bool Autosuspend => CastAide.AsBoolean(row1["suspended"], false);

        public bool UplineAutoSuspend => CastAide.AsBoolean(row2["uplineautosuspend"]);

        public bool IsDeposit => WebSettings.IsApplySplitingDepositeCustomer && CastAide.AsBoolean(row1["IsDeposit"]);

        public bool IsLicensee => IsDeposit;

        public bool IsIMBC => SiteName.ToString().ToUpper().Equals("IMBC");

        public string LastLoginDate => Convert.IsDBNull(row1["lastlogged"]) ? string.Empty : Convert.ToDateTime(row1["lastlogged"]).ToShortDateString();

        public string LastLoginTime => Convert.IsDBNull(row1["lastlogged"]) ? string.Empty : " " + Convert.ToDateTime(row1["lastlogged"]).ToShortTimeString();

        public string LastIp => Convert.IsDBNull(row1["lastloginip"]) ? string.Empty : row1["lastloginip"].ToString();

        public string CustomerSite => (RoleId == (int)Globals.AccountType.Member) ? CastAide.AsString(row1["membersite"], string.Empty) : CastAide.AsString(row1["site"], string.Empty);

        public string Createdate => Convert.IsDBNull(row1["createdate"]) ? string.Empty : Convert.ToDateTime(row1["createdate"]).ToShortDateString();

        public string Createtime => Convert.IsDBNull(row1["createdate"]) ? string.Empty : Convert.ToDateTime(row1["createdate"]).ToShortTimeString();

        public string PwdExpiryDate => Convert.IsDBNull(row1["pwdexpiry"]) ? string.Empty : Convert.ToDateTime(row1["pwdexpiry"]).ToShortDateString();

        public string PwdExpiryTime => Convert.IsDBNull(row1["pwdexpiry"]) ? string.Empty : " " + Convert.ToDateTime(row1["pwdexpiry"]).ToShortTimeString();
        public string LastTxnDate
            => row3 != null && RoleId <= 4 ? Convert.IsDBNull(row3["lastTxn"]) ? string.Empty : Convert.ToDateTime(row3["lastTxn"]).ToShortDateString() : string.Empty;

        public string LastTxnTime
            => row3 != null && RoleId <= 4 ? Convert.IsDBNull(row3["lastTxn"]) ? string.Empty : " " + Convert.ToDateTime(row3["lastTxn"]).ToShortTimeString() : string.Empty;

        public string Phone => CastAide.AsString(row1["phone"]);

        public string Mobile => CastAide.AsString(row1["mobilephone"]);

        public string Fax => CastAide.AsString(row1["fax"]);

        public string Nickname => CastAide.AsString(row1["username2"], string.Empty);

        public int SuperId => CastAide.AsInt32(row2["SuperID"]);

        public string SuperName => CastAide.AsString(row2["SuperName"]);

        public int MasterId => CastAide.AsInt32(row2["MasterID"]);

        public string MasterName => CastAide.AsString(row2["MasterName"]);

        public int AgentId => CastAide.AsInt32(row2["AgentID"]);

        public string AgentName => CastAide.AsString(row2["AgentName"]);

        public int MemberId => CastAide.AsInt32(row2["MemberID"]);

        public string MemberName => CastAide.AsString(row2["MemberName"]);

        public bool SuperEnabled => Convert.ToBoolean(row2["SuperEnabled"]);

        public bool MasterEnabled => Convert.ToBoolean(row2["MasterEnabled"]);

        public bool AgentEnabled => Convert.ToBoolean(row2["AgentEnabled"]);

        public bool Enabled => CastAide.AsBoolean(row1["Enabled"]);

        public string ClosedReason => CastAide.AsString(row1["closedreason"]);

        public bool Doublecommlosing => CastAide.AsBoolean(row1["DoubleCommLosing"], false);

        public bool NicknameLoginOnly => CastAide.AsBoolean(row1["NicknameLoginOnly"], false);

        public string LastChangeSecurityCodeDate => Convert.IsDBNull(row1["LastChangedSecurityCodeDate"]) ? string.Empty : Convert.ToString(row1["LastChangedSecurityCodeDate"]);

        public string LastChangeOTPCodeDate => Convert.IsDBNull(row1["OTPCreateDate"]) ? string.Empty : Convert.ToString(row1["OTPCreateDate"]);

        public int Groupheadlineid => CastAide.AsInt32(row1["groupheadlineid"], 0);

        public string Headline => CastAide.AsString(row1["headline"]);

        public int LimitPerIP => CastAide.AsInt16(row1["LimitPerIP"]);

        public int Danger => CastAide.AsInt32(row1["danger"], 0);

        public int Danger2 => CastAide.AsInt32(row1["danger2"], 0);

        public int Danger3 => CastAide.AsInt32(row1["danger3"], 0);

        public bool HasDownline => CastAide.AsBoolean(row1["HasDownline"]);

        public int Abnormal => CastAide.AsInt32(row1["abnormal"], 0);

        public bool SelectedBetting => CastAide.AsBoolean(row1["SelectedBetting"], false);

        public bool IsInternalUser => CastAide.AsBoolean(row1["IsInternal"], false);

        public bool Closed => CastAide.AsBoolean(row1["closed"], false);

        public bool IsBA => CastAide.AsBoolean(row1["isba"]);
    }
}
