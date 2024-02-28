namespace Sunplus.Accounting.Business.Customer
{
    using System.Data;
    using DNAide.Core;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Tarus.Infrastructure.Configurations;
    using static Fanex.Tarus.Infrastructure.Common.Enumerations;

    public partial class CustomerInfoViewModel
    {
        public decimal OutStanding { get; private set; }

        public decimal AccOutstanding { get; private set; }

        public decimal Cash { get; private set; }

        public decimal TotalBal { get; private set; }

        public decimal YesterdayCash { get; private set; }

        public decimal YesterdayBal { get; private set; }

        public decimal BetCredit { get; private set; }

        public decimal UsedCredit { get; private set; }

        public decimal NetPosition { get; private set; }

        public decimal PendingTransfer3rd { get; private set; }

        public decimal Credit => Utils.ToDecimal(customerInfoRow["credit"]);

        public decimal ActualRate => Converter.ToDecimal(customerInfoRow["actualrate"]);

        public string Currency => Converter.ToString2(customerInfoRow["currency"]);

        public bool EnableVirtualRate => CastAide.AsDecimal(customerInfoRow["EnableVirtualRate"]) != 0;

        private void GetExtraCustomerInfo(
            DataRow customerInfoRow,
            DataRow lineInfoRow,
            DataRow cashRow,
            decimal memberOutstandingMBC,
            decimal memberBalanceMBC,
            decimal memberNetPositionMBC,
            decimal member3rdPendingTransfer)
        {
            switch (SearchAccountRoleId)
            {
                case 1: // member       
                    PendingTransfer3rd = member3rdPendingTransfer;
                    ParentId = Converter.ToInt(lineInfoRow["agentId"]);
                    YesterdayCash = Converter.ToDecimal(cashRow["Yesterday_Cash"]);
                    AccOutstanding = IsInternalUser ? Converter.ToDecimal(cashRow["outstanding"]) : memberOutstandingMBC;
                    Cash = IsInternalUser ? Converter.ToDecimal(cashRow["cash"]) : memberBalanceMBC + memberOutstandingMBC + PendingTransfer3rd;
                    NetPosition = IsInternalUser ? Converter.ToDecimal(customerInfoRow["netPosition"]) : memberNetPositionMBC;
                    BetCredit = IsInternalUser ? Credit + Cash - AccOutstanding + NetPosition : Credit + memberBalanceMBC + memberNetPositionMBC;
                    break;

                case 2: // agent
                    PendingTransfer3rd = Converter.ToDecimal(cashRow["PendingTransfer3rd"]);
                    ParentId = Converter.ToInt(lineInfoRow["masterid"]);
                    AccOutstanding = Converter.ToDecimal(cashRow["agentPositionTaking"]);
                    OutStanding = Converter.ToDecimal(cashRow["customerOutstanding"]);
                    Cash = Converter.ToDecimal(cashRow["agentCash"]);
                    TotalBal = Cash + Converter.ToDecimal(cashRow["customerCash"]);
                    YesterdayCash = Converter.ToDecimal(cashRow["yesterday_agentCash"]);
                    YesterdayBal = YesterdayCash + Converter.ToDecimal(cashRow["Yesterday_CustomerCash"]);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["agentUsedCredit"]);
                    break;

                case 3: // master
                    PendingTransfer3rd = Converter.ToDecimal(cashRow["PendingTransfer3rd"]);
                    ParentId = Converter.ToInt(lineInfoRow["superId"]);
                    AccOutstanding = Converter.ToDecimal(cashRow["masterPositionTaking"]);
                    OutStanding = Converter.ToDecimal(cashRow["customerOutstanding"]);
                    Cash = Converter.ToDecimal(cashRow["masterCash"]);
                    TotalBal = Cash + Converter.ToDecimal(cashRow["customerCash"]) + Converter.ToDecimal(cashRow["agentCash"]);
                    YesterdayCash = Converter.ToDecimal(cashRow["yesterday_masterCash"]);
                    YesterdayBal = YesterdayCash + Converter.ToDecimal(cashRow["yesterday_customerCash"]) + Converter.ToDecimal(cashRow["yesterday_agentCash"]);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["masterUsedCredit"]);
                    break;

                case 4: // super
                    PendingTransfer3rd = Converter.ToDecimal(cashRow["PendingTransfer3rd"]);
                    ParentId = 0;
                    AccOutstanding = Converter.ToDecimal(cashRow["superPositionTaking"]);
                    OutStanding = Converter.ToDecimal(cashRow["customerOutstanding"]);
                    Cash = Converter.ToDecimal(cashRow["superCash"]);
                    TotalBal = Cash + Converter.ToDecimal(cashRow["customerCash"])
                        + Converter.ToDecimal(cashRow["agentCash"])
                        + Converter.ToDecimal(cashRow["masterCash"]);
                    YesterdayCash = Converter.ToDecimal(cashRow["yesterday_superCash"]);
                    YesterdayBal = YesterdayCash + Converter.ToDecimal(cashRow["yesterday_customerCash"])
                        + Converter.ToDecimal(cashRow["yesterday_agentCash"])
                        + Converter.ToDecimal(cashRow["yesterday_masterCash"]);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["superUsedCredit"]);
                    break;

                case 0:
                    YesterdayCash = Converter.ToDecimal(cashRow["YesterdayCash"]);
                    YesterdayBal = Converter.ToDecimal(cashRow["YesterdayBalance"]);
                    break;

                default:
                    break;
            }
        }
    }
}
