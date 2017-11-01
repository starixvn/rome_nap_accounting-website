namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using Core.Common;
    using Core.Configurations;
    using DNAide.Core;

    public partial class CustomerInfoViewModel
    {
        public int CustId => CastAide.AsInt32(customerInfoRow["custid"]);

        public int RoleId => CastAide.AsInt32(customerInfoRow["roleid"]);

        public string UserName => CastAide.AsString(customerInfoRow["username"]);

        public int ParentId { get; private set; }

        public string Firstname => CastAide.AsString(customerInfoRow["firstname"], string.Empty);

        public string Lastname => CastAide.AsString(customerInfoRow["lastname"], string.Empty);

        public string Fullname => $"{Firstname} - {Lastname}";

        public string DownlineName { get; private set; }

        public string SiteName => string.IsNullOrWhiteSpace(CastAide.AsString(customerInfoRow["site"]))? CastAide.AsString(customerInfoRow["membersite"]) : CastAide.AsString(customerInfoRow["site"]);

        public bool Important => CastAide.AsBoolean(customerInfoRow["important"], false);

        public bool Suspend => CastAide.AsBoolean(customerInfoRow["suspended"], false);

        public bool Autosuspend => CastAide.AsBoolean(customerInfoRow["suspended"], false);

        public bool UplineAutoSuspend => CastAide.AsBoolean(lineInfoRow["uplineautosuspend"]);

        public bool IsDeposit => WebSettings.IsApplySplitingDepositeCustomer && CastAide.AsBoolean(customerInfoRow["IsDeposit"]);

        public bool IsIMBC => SiteName.ToString().ToUpper().Equals("IMBC");

        public string LastLoginDate => Convert.IsDBNull(customerInfoRow["lastlogged"]) ? string.Empty : Convert.ToDateTime(customerInfoRow["lastlogged"]).ToShortDateString();

        public string LastLoginTime => Convert.IsDBNull(customerInfoRow["lastlogged"]) ? string.Empty : " " + Convert.ToDateTime(customerInfoRow["lastlogged"]).ToShortTimeString();

        public string LastIp => Convert.IsDBNull(customerInfoRow["lastloginip"]) ? string.Empty : customerInfoRow["lastloginip"].ToString();

        public string CustomerSite => (RoleId == (int)Globals.AccountType.Member) ? CastAide.AsString(customerInfoRow["membersite"], string.Empty) : CastAide.AsString(customerInfoRow["site"], string.Empty);

        public string Createdate => Convert.IsDBNull(customerInfoRow["createdate"]) ? string.Empty : Convert.ToDateTime(customerInfoRow["createdate"]).ToShortDateString();

        public string Createtime => Convert.IsDBNull(customerInfoRow["createdate"]) ? string.Empty : Convert.ToDateTime(customerInfoRow["createdate"]).ToShortTimeString();

        public string PwdExpiryDate => Convert.IsDBNull(customerInfoRow["pwdexpiry"]) ? string.Empty : Convert.ToDateTime(customerInfoRow["pwdexpiry"]).ToShortDateString();

        public string PwdExpiryTime => Convert.IsDBNull(customerInfoRow["pwdexpiry"]) ? string.Empty : " " + Convert.ToDateTime(customerInfoRow["pwdexpiry"]).ToShortTimeString();

        public string LastTxnDate
            => cashRow != null && RoleId <= 4 ? Convert.IsDBNull(cashRow["lastTxn"]) ? string.Empty : Convert.ToDateTime(cashRow["lastTxn"]).ToShortDateString() : string.Empty;

        public string LastTxnTime
            => cashRow != null && RoleId <= 4 ? Convert.IsDBNull(cashRow["lastTxn"]) ? string.Empty : " " + Convert.ToDateTime(cashRow["lastTxn"]).ToShortTimeString() : string.Empty;

        public string Phone => CastAide.AsString(customerInfoRow["phone"]);

        public string Mobile => CastAide.AsString(customerInfoRow["mobilephone"]);

        public string Fax => CastAide.AsString(customerInfoRow["fax"]);

        public string Nickname => CastAide.AsString(customerInfoRow["username2"], string.Empty);

        public bool Enabled => CastAide.AsBoolean(customerInfoRow["Enabled"]);

        public string ClosedReason => CastAide.AsString(customerInfoRow["closedreason"]);

        public bool Doublecommlosing => CastAide.AsBoolean(customerInfoRow["DoubleCommLosing"], false);

        public bool NicknameLoginOnly => CastAide.AsBoolean(customerInfoRow["NicknameLoginOnly"], false);

        public string LastChangeSecurityCodeDate => Convert.IsDBNull(customerInfoRow["LastChangedSecurityCodeDate"]) ? string.Empty : Convert.ToString(customerInfoRow["LastChangedSecurityCodeDate"]);

        public string LastChangeOTPCodeDate => Convert.IsDBNull(customerInfoRow["OTPCreateDate"]) ? string.Empty : Convert.ToString(customerInfoRow["OTPCreateDate"]);

        public int Groupheadlineid => CastAide.AsInt32(customerInfoRow["groupheadlineid"], 0);

        public string Headline => CastAide.AsString(customerInfoRow["headline"]);

        public int LimitPerIP => CastAide.AsInt16(customerInfoRow["LimitPerIP"]);

        public int Danger => CastAide.AsInt32(customerInfoRow["danger"], 0);

        public int Danger2 => CastAide.AsInt32(customerInfoRow["danger2"], 0);

        public int Danger3 => CastAide.AsInt32(customerInfoRow["danger3"], 0);

        public bool HasDownline => CastAide.AsBoolean(customerInfoRow["HasDownline"]);

        public int Abnormal => CastAide.AsInt32(customerInfoRow["abnormal"], 0);

        public bool SelectedBetting => CastAide.AsBoolean(customerInfoRow["SelectedBetting"], false);

        public bool IsInternalUser => CastAide.AsBoolean(customerInfoRow["IsInternal"], false);

        public bool Closed => CastAide.AsBoolean(customerInfoRow["closed"], false);

        public bool IsBA => CastAide.AsBoolean(customerInfoRow["isba"]);

        public int ExchangeId => CastAide.AsInt32(customerInfoRow["exchangeid"]);

        public int SuperId => CastAide.AsInt32(lineInfoRow["SuperID"]);

        public string SuperName => CastAide.AsString(lineInfoRow["SuperName"]);

        public int MasterId => CastAide.AsInt32(lineInfoRow["MasterID"]);

        public string MasterName => CastAide.AsString(lineInfoRow["MasterName"]);

        public int AgentId => CastAide.AsInt32(lineInfoRow["AgentID"]);

        public string AgentName => CastAide.AsString(lineInfoRow["AgentName"]);

        public int MemberId => CastAide.AsInt32(lineInfoRow["MemberID"]);

        public string MemberName => CastAide.AsString(lineInfoRow["MemberName"]);

        public bool SuperEnabled => Convert.ToBoolean(lineInfoRow["SuperEnabled"]);

        public bool MasterEnabled => Convert.ToBoolean(lineInfoRow["MasterEnabled"]);

        public bool AgentEnabled => Convert.ToBoolean(lineInfoRow["AgentEnabled"]);
    }
}
