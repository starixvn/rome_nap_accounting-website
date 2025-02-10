namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Configurations;
    using DataAccess;
    using Entities;
    using Fanex.CustomerSetting.Module.Core;
    using Security;
    using System.Collections;
    using System.Data;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;

    public partial class CustomerInfoViewModel
    {
        private CustomerDetail customerDetail;

        private DataRow customerInfoRow { get; set; }

        private DataRow lineInfoRow { get; set; }

        private DataRow cashRow { get; set; }

        private DataRow downlineStatusInfoRow { get; set; }

        private SabaCoinModel sabaCoinModel { get; set; }

        public CustomerInfoViewModel(
            DataRow customerInfoRow,
            DataRow lineInfoRow,
            DataRow cashRow,
            DataRow downlineStatusInfoRow,
            DataTable dynamicSettingtable,
            decimal memberOutstandingMBC,
            decimal memberBalanceMBC,
            decimal memberNetPositionMBC,
            decimal member3rdPendingTransfer)
        {
            this.customerInfoRow = customerInfoRow;
            this.lineInfoRow = lineInfoRow;
            this.cashRow = cashRow;
            this.downlineStatusInfoRow = downlineStatusInfoRow;
            this.sabaCoinModel = new SabaCoinModel(customerInfoRow);

            JsParam = new Hashtable
            {
                { "custid", CustId },
                { "roleid", SearchAccountRoleId }
            };

            GetExtraCustomerInfo(customerInfoRow, lineInfoRow, cashRow, memberOutstandingMBC, memberBalanceMBC, memberNetPositionMBC, member3rdPendingTransfer);
            GetShowedHiddenProduct(dynamicSettingtable);
        }

        public Hashtable JsParam { get; set; }

        public bool HasReadPermission_AutoSuspend => Permissions.HasPermission(Enumerations.Functions.Customer.AutoSuspend);

        public bool ManageCustomerAccess => Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust);

        public bool AllowUpdateGivenCredit => Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

        public bool ViewFiAcc => Permissions.FinancePermission.IsViewFiAcc(IsInternalUser);

        public bool ShowSabaCoinTab => sabaCoinModel.IsRealAccount;

        public long SabaId => sabaCoinModel.SabaId;

        public bool IsShadowMember => sabaCoinModel.IsShadowMember;

        public bool HideDirectStatus => Utils.ToBoolean(customerInfoRow["IsHideDirectStatus"]);

        public bool CheckedDirectStatus => Utils.ToBoolean(customerInfoRow["IsCheckDirectStatus"]);

        public bool DisabledDirectStatus => Utils.ToBoolean(customerInfoRow["IsDisabledDirectStatus"]);

        public bool AllDayBalTransfer => Utils.ToBoolean(customerInfoRow["isalldaybaltransfer"]);

        public bool AllDayBalTransferOnMaster => Utils.ToBoolean(customerInfoRow["IsAutoEnableADBMaster"]);

        public bool IsShowAllDayBalTransfer => Utils.ToBoolean(customerInfoRow["IsShowAllDayBalTransfer"]);

        public bool DisableAutoMPT => Utils.ToBoolean(customerInfoRow["DisableAutoMPT"]);

        public bool DisableOutright => Utils.ToBoolean(customerInfoRow["DisableOutright"]);

        public bool HideCashout { get; private set; } = true;

        public bool HideSabaVirtualSports { get; private set; } = true;

        public bool ShowSettingLinkAGCasinoDeposit => WebSettings.EnableAGCasino && IsDeposit;

        public bool ShowEnableBetTradeDeposit => WebSettings.EnableBetTrade && SearchAccountRoleId.Equals(4) && IsDeposit;

        public bool ShowMacauDeposit => WebSettings.EnableMacau && SearchAccountRoleId.Equals(4) && IsDeposit;

        public bool LockUnsuspend => Utils.ToBoolean(customerInfoRow["LockUnsuspend"]);

        public bool HideMuayThai { get; private set; } = true;

        public bool HideLiveAndRNGCasino { get; private set; } = true;

        public bool HideRNGGames { get; private set; } = true;

        public bool SuperDirectMember => (SearchAccountRoleId == (int)Role.Member &&
                                         Utils.ToInt32(lineInfoRow["AgentID"]) == -4);


        public bool MasterDirectMember => (SearchAccountRoleId == (int)Role.Member &&
                                          Utils.ToInt32(lineInfoRow["AgentID"]) == -3);

        public CustomerDetail CustParent
        {
            get
            {
                if (customerDetail == null)
                {
                    customerDetail = ParentId > 0 ? IBCProvider.IBCCustomer.GetCustomerStatus(ParentId, WebSettings.SettingStatus) : new CustomerDetail();
                }

                return customerDetail;
            }
        }

       

        private void GetShowedHiddenProduct(DataTable dynamicSettingtable)
        {
            if (SearchAccountRoleId != (int)Role.Headline)
            {
                foreach (DataRow row in dynamicSettingtable.Rows)
                {
                    if (row["productID"] != null)
                    {
                        var productId = row["productID"].ToString();
                        switch (productId)
                        {
                            case "25":
                                HideCashout = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4) || !WebSettings.EnableCashOut;
                                break;

                            case "54":
                                HideSabaVirtualSports = Utils.ToBoolean(row["IsHidden"]) || !WebSettings.IsShowSabaVirtualSports;
                                break;
                        }
                    }

                    if (!IsDeposit && row["GroupID"] != null && int.TryParse(row["GroupID"].ToString(), out int productGroupId))
                    {
                        switch (productGroupId)
                        {
                            case ProductGroupIdConstants.SportsBook:
                                HideMuayThai = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4);
                                break;

                            case ProductGroupIdConstants.RNGGames:
                                HideRNGGames = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4);
                                break;

                            case ProductGroupIdConstants.LiveAndRNGCasino:
                                HideLiveAndRNGCasino = (Utils.ToBoolean(row["IsHidden"]) && SearchAccountRoleId < 4);
                                break;
                        }
                    }
                }
            }
        }
    }

    public class SabaCoinModel
    {
        public SabaCoinModel(DataRow dataRow)
        {
            SabaId = Utils.ToInt64(dataRow["SabaID"]);
            CustType = Utils.ToInt16(dataRow["CustType"]);

            IsShadowMember = CustType == 1 && SabaId != 0;
            IsRealAccount = SabaId != 0 && !IsShadowMember;
        }

        public bool IsRealAccount { get; set; }

        public bool IsShadowMember { get; set; }

        public long SabaId { get; set; }

        private int CustType { get; set; }
    }
}