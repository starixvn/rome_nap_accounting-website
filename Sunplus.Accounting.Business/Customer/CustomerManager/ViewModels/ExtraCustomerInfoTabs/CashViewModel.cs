namespace Sunplus.Accounting.Business.Customer
{
    using System.Data;
    using DNAide.Core;

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

        public decimal Credit => CastAide.AsDecimal(customerInfoRow["credit"]);

        public decimal ActualRate => CastAide.AsDecimal(customerInfoRow["actualrate"]);

        public string Currency => CastAide.AsString(customerInfoRow["currency"]);

        public bool EnableVirtualRate => CastAide.AsBoolean(customerInfoRow["EnableVirtualRate"]);

        private void GetExtraCustomerInfo(DataRow customerInfoRow, DataRow lineInfoRow, DataRow cashRow)
        {
            switch (RoleId)
            {
                case 1: // member
                    ParentId = CastAide.AsInt32(lineInfoRow["agentId"]);
                    DownlineName = string.Empty;
                    AccOutstanding = CastAide.AsDecimal(cashRow["outstanding"], 0);
                    Cash = CastAide.AsDecimal(cashRow["cash"], 0);
                    NetPosition = CastAide.AsDecimal(customerInfoRow["netPosition"], 0);
                    BetCredit = Credit + Cash - AccOutstanding + NetPosition;
                    YesterdayCash = CastAide.AsDecimal(cashRow["Yesterday_Cash"], 0);
                    break;

                case 2: // agent
                    ParentId = CastAide.AsInt32(lineInfoRow["masterid"]);
                    DownlineName = "Member";
                    AccOutstanding = CastAide.AsDecimal(cashRow["agentPositionTaking"], 0);
                    OutStanding = CastAide.AsDecimal(cashRow["customerOutstanding"], 0);
                    Cash = CastAide.AsDecimal(cashRow["agentCash"], 0);
                    TotalBal = Cash + CastAide.AsDecimal(cashRow["customerCash"], 0);
                    YesterdayCash = CastAide.AsDecimal(cashRow["yesterday_agentCash"], 0);
                    YesterdayBal = YesterdayCash + CastAide.AsDecimal(cashRow["Yesterday_CustomerCash"], 0);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["agentUsedCredit"]);
                    break;

                case 3: // master
                    ParentId = CastAide.AsInt32(lineInfoRow["superId"]);
                    DownlineName = "Agent";
                    AccOutstanding = CastAide.AsDecimal(cashRow["masterPositionTaking"], 0);
                    OutStanding = CastAide.AsDecimal(cashRow["customerOutstanding"], 0);
                    Cash = CastAide.AsDecimal(cashRow["masterCash"], 0);
                    TotalBal = Cash + CastAide.AsDecimal(cashRow["customerCash"], 0) + CastAide.AsDecimal(cashRow["agentCash"], 0);
                    YesterdayCash = CastAide.AsDecimal(cashRow["yesterday_masterCash"], 0);
                    YesterdayBal = YesterdayCash + CastAide.AsDecimal(cashRow["yesterday_customerCash"], 0) + CastAide.AsDecimal(cashRow["yesterday_agentCash"], 0);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["masterUsedCredit"]);
                    break;

                case 4: // super
                    ParentId = 0;
                    DownlineName = "Master";
                    AccOutstanding = CastAide.AsDecimal(cashRow["superPositionTaking"], 0);
                    OutStanding = CastAide.AsDecimal(cashRow["customerOutstanding"], 0);
                    Cash = CastAide.AsDecimal(cashRow["superCash"], 0);
                    TotalBal = Cash + CastAide.AsDecimal(cashRow["customerCash"], 0)
                        + CastAide.AsDecimal(cashRow["agentCash"], 0)
                        + CastAide.AsDecimal(cashRow["masterCash"], 0);
                    YesterdayCash = CastAide.AsDecimal(cashRow["yesterday_superCash"], 0);
                    YesterdayBal = YesterdayCash + CastAide.AsDecimal(cashRow["yesterday_customerCash"], 0)
                        + CastAide.AsDecimal(cashRow["yesterday_agentCash"], 0)
                        + CastAide.AsDecimal(cashRow["yesterday_masterCash"], 0);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(cashRow["superUsedCredit"]);
                    break;

                default:
                    break;
            }
        }
    }
}
