namespace Sunplus.Accounting.Business.Customer
{
    using System.Data;
    using DNAide.Core;
    using Fanex.Common;

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

        public decimal Credit => Converter.ToDecimal(customerInfoRow["credit"]);

        public decimal ActualRate => Converter.ToDecimal(customerInfoRow["actualrate"]);

        public string Currency => Converter.ToString2(customerInfoRow["currency"]);

        public bool EnableVirtualRate => CastAide.AsDecimal(customerInfoRow["EnableVirtualRate"]) != 0;

        private void GetExtraCustomerInfo(DataRow customerInfoRow, DataRow lineInfoRow, DataRow cashRow)
        {
            switch (RoleId)
            {
                case 1: // member
                    ParentId = Converter.ToInt(lineInfoRow["agentId"]);
                    DownlineName = string.Empty;
                    AccOutstanding = Converter.ToDecimal(cashRow["outstanding"]);
                    Cash = Converter.ToDecimal(cashRow["cash"]);
                    NetPosition = Converter.ToDecimal(customerInfoRow["netPosition"]);
                    BetCredit = Credit + Cash - AccOutstanding + NetPosition;
                    YesterdayCash = Converter.ToDecimal(cashRow["Yesterday_Cash"]);
                    break;

                case 2: // agent
                    ParentId = Converter.ToInt(lineInfoRow["masterid"]);
                    DownlineName = "Member";
                    AccOutstanding = Converter.ToDecimal(cashRow["agentPositionTaking"]);
                    OutStanding = Converter.ToDecimal(cashRow["customerOutstanding"]);
                    Cash = Converter.ToDecimal(cashRow["agentCash"]);
                    TotalBal = Cash + Converter.ToDecimal(cashRow["customerCash"]);
                    YesterdayCash = Converter.ToDecimal(cashRow["yesterday_agentCash"]);
                    YesterdayBal = YesterdayCash + Converter.ToDecimal(cashRow["Yesterday_CustomerCash"]);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["agentUsedCredit"]);
                    break;

                case 3: // master
                    ParentId = Converter.ToInt(lineInfoRow["superId"]);
                    DownlineName = "Agent";
                    AccOutstanding = Converter.ToDecimal(cashRow["masterPositionTaking"]);
                    OutStanding = Converter.ToDecimal(cashRow["customerOutstanding"]);
                    Cash = Converter.ToDecimal(cashRow["masterCash"]);
                    TotalBal = Cash + Converter.ToDecimal(cashRow["customerCash"]) + Converter.ToDecimal(cashRow["agentCash"]);
                    YesterdayCash = Converter.ToDecimal(cashRow["yesterday_masterCash"]);
                    YesterdayBal = YesterdayCash + Converter.ToDecimal(cashRow["yesterday_customerCash"]) + Converter.ToDecimal(cashRow["yesterday_agentCash"]);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["masterUsedCredit"]);
                    break;

                case 4: // super
                    ParentId = 0;
                    DownlineName = "Master";
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

                default:
                    break;
            }
        }
    }
}
