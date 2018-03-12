namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using Core.Configurations;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core;

    public partial class CustomerInfoViewModel
    {
        public int CustId => Converter.ToInt(customerInfoRow["custid"]);

        public int RoleId => Converter.ToInt(customerInfoRow["roleid"]);

        public string UserName => Converter.ToString2(customerInfoRow["username"]);

        public int ParentId { get; private set; }

        public string Firstname => Converter.ToString2(customerInfoRow["firstname"]);

        public string Lastname => Converter.ToString2(customerInfoRow["lastname"]);

        public string Fullname => $"{Firstname} - {Lastname}";

        public string DownlineName { get; private set; }

        public bool Important => Utils.ToBoolean(customerInfoRow["important"]);

        public bool Suspend => Utils.ToBoolean(customerInfoRow["suspended"]);

        public bool Autosuspend => Utils.ToBoolean(customerInfoRow["suspended"]);

        public bool UplineAutoSuspend => Utils.ToBoolean(lineInfoRow["uplineautosuspend"]);

        public bool IsDeposit => WebSettings.IsApplySplitingDepositeCustomer && Utils.ToBoolean(customerInfoRow["IsDeposit"]);

        public bool IsIMBC
        {
            get
            {
                var site = Converter.ToString2(customerInfoRow["site"]);
                if (string.IsNullOrWhiteSpace(site))
                    return Converter.ToString2(customerInfoRow["membersite"]).ToUpper().Equals("IMBC");
                return site.ToUpper().Equals("IMBC");
            }
        }

        public string LastLoginDate => Convert.IsDBNull(customerInfoRow["lastlogged"]) ? string.Empty : Converter.ToDateTime(customerInfoRow["lastlogged"]).ToShortDateString();

        public string LastLoginTime => Convert.IsDBNull(customerInfoRow["lastlogged"]) ? string.Empty : $" {Converter.ToDateTime(customerInfoRow["lastlogged"]).ToShortTimeString()}";

        public string LastIp => Converter.ToString2(customerInfoRow["lastloginip"]);

        public string CustomerSite => RoleId.Equals(1) ? Converter.ToString2(customerInfoRow["membersite"]) : Converter.ToString2(customerInfoRow["site"]);

        public string Createdate => Convert.IsDBNull(customerInfoRow["createdate"]) ? string.Empty : Converter.ToDateTime(customerInfoRow["createdate"]).ToShortDateString();

        public string Createtime => Convert.IsDBNull(customerInfoRow["createdate"]) ? string.Empty : Converter.ToDateTime(customerInfoRow["createdate"]).ToShortTimeString();

        public string PwdExpiryDate => Convert.IsDBNull(customerInfoRow["pwdexpiry"]) ? string.Empty : Converter.ToDateTime(customerInfoRow["pwdexpiry"]).ToShortDateString();

        public string PwdExpiryTime => Convert.IsDBNull(customerInfoRow["pwdexpiry"]) ? string.Empty : $" {Converter.ToDateTime(customerInfoRow["pwdexpiry"]).ToShortTimeString()}";

        public string LastTxnDate => Convert.IsDBNull(cashRow["lastTxn"]) ? string.Empty : Converter.ToDateTime(cashRow["lastTxn"]).ToShortDateString();

        public string LastTxnTime => Convert.IsDBNull(cashRow["lastTxn"]) ? string.Empty : $" {Converter.ToDateTime(cashRow["lastTxn"]).ToShortTimeString()}";

        public string Phone => Converter.ToString2(customerInfoRow["phone"]);

        public string Mobile => Converter.ToString2(customerInfoRow["mobilephone"]);

        public string Fax => Converter.ToString2(customerInfoRow["fax"]);

        public string Nickname => Converter.ToString2(customerInfoRow["username2"]);

        public bool Enabled => Utils.ToBoolean(customerInfoRow["Enabled"]);

        public string ClosedReason => Converter.ToString2(customerInfoRow["closedreason"]);

        public bool Doublecommlosing => Utils.ToBoolean(customerInfoRow["DoubleCommLosing"]);

        public bool NicknameLoginOnly => Utils.ToBoolean(customerInfoRow["NicknameLoginOnly"]);

        public string LastChangeSecurityCodeDate => Converter.ToString2(customerInfoRow["LastChangedSecurityCodeDate"]);

        public string LastChangeOTPCodeDate => Converter.ToString2(customerInfoRow["OTPCreateDate"]);

        public int Groupheadlineid => Converter.ToInt(customerInfoRow["groupheadlineid"]);

        public string Headline => Converter.ToString2(customerInfoRow["headline"]);

        public int LimitPerIP => Converter.ToInt(customerInfoRow["LimitPerIP"]);

        public int Danger => Converter.ToInt(customerInfoRow["danger"]);

        public int Danger2 => Converter.ToInt(customerInfoRow["danger2"]);

        public int Danger3 => Converter.ToInt(customerInfoRow["danger3"]);

        public bool HasDownline => Utils.ToBoolean(customerInfoRow["HasDownline"]);

        public int Abnormal => Converter.ToInt(customerInfoRow["abnormal"]);

        public bool SelectedBetting => Utils.ToBoolean(customerInfoRow["SelectedBetting"]);

        public bool IsInternalUser => Utils.ToBoolean(customerInfoRow["IsInternal"]);

        public bool Closed => Utils.ToBoolean(customerInfoRow["closed"]);

        public bool IsBA => Utils.ToBoolean(customerInfoRow["isba"]);

        public int ExchangeId => Converter.ToInt(customerInfoRow["exchangeid"]);

        public int SuperId => Converter.ToInt(lineInfoRow["SuperID"]);

        public string SuperName => Converter.ToString2(lineInfoRow["SuperName"]);

        public int MasterId => Converter.ToInt(lineInfoRow["MasterID"]);

        public string MasterName => Converter.ToString2(lineInfoRow["MasterName"]);

        public int AgentId => Converter.ToInt(lineInfoRow["AgentID"]);

        public string AgentName => Converter.ToString2(lineInfoRow["AgentName"]);

        public int MemberId => Converter.ToInt(lineInfoRow["MemberID"]);

        public string MemberName => Converter.ToString2(lineInfoRow["MemberName"]);

        public bool SuperEnabled => Utils.ToBoolean(lineInfoRow["SuperEnabled"]);

        public bool MasterEnabled => Utils.ToBoolean(lineInfoRow["MasterEnabled"]);

        public bool AgentEnabled => Utils.ToBoolean(lineInfoRow["AgentEnabled"]);
    }
}
