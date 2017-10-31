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

        public decimal Credit => CastAide.AsDecimal(row1["credit"]);

        public decimal ActualRate => CastAide.AsDecimal(row1["actualrate"]);

        public string Currency => CastAide.AsString(row1["currency"]);

        public bool EnableVirtualRate => CastAide.AsBoolean(row1["EnableVirtualRate"]);

        private void GetExtraCustomerInfo(DataRow row1, DataRow row2, DataRow row3)
        {
            switch (RoleId)
            {
                case 1: // member
                    ParentId = CastAide.AsInt32(row2["agentId"]);
                    AccOutstanding = CastAide.AsDecimal(row3["outstanding"], 0);
                    Cash = CastAide.AsDecimal(row3["cash"], 0);
                    NetPosition = CastAide.AsDecimal(row1["netPosition"], 0);
                    BetCredit = Credit + Cash - AccOutstanding + NetPosition;
                    YesterdayCash = CastAide.AsDecimal(row3["Yesterday_Cash"], 0);
                    DownlineName = string.Empty;
                    break;

                case 2: // agent
                    ParentId = CastAide.AsInt32(row2["masterid"]);
                    AccOutstanding = CastAide.AsDecimal(row3["agentPositionTaking"], 0);
                    OutStanding = CastAide.AsDecimal(row3["customerOutstanding"], 0);
                    Cash = CastAide.AsDecimal(row3["agentCash"], 0);
                    TotalBal = Cash + CastAide.AsDecimal(row3["customerCash"], 0);
                    YesterdayCash = CastAide.AsDecimal(row3["yesterday_agentCash"], 0);
                    YesterdayBal = YesterdayCash + CastAide.AsDecimal(row3["Yesterday_CustomerCash"], 0);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(row3["agentUsedCredit"]);
                    DownlineName = "Member";
                    break;

                case 3: // master
                    ParentId = CastAide.AsInt32(row2["superId"]);
                    AccOutstanding = CastAide.AsDecimal(row3["masterPositionTaking"], 0);
                    OutStanding = CastAide.AsDecimal(row3["customerOutstanding"], 0);
                    Cash = CastAide.AsDecimal(row3["masterCash"], 0);
                    TotalBal = Cash + CastAide.AsDecimal(row3["customerCash"], 0) + CastAide.AsDecimal(row3["agentCash"], 0);
                    YesterdayCash = CastAide.AsDecimal(row3["yesterday_masterCash"], 0);
                    YesterdayBal = YesterdayCash + CastAide.AsDecimal(row3["yesterday_customerCash"], 0) + CastAide.AsDecimal(row3["yesterday_agentCash"], 0);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(row3["masterUsedCredit"]);
                    DownlineName = "Agent";
                    break;

                case 4: // super
                    ParentId = 0;
                    AccOutstanding = CastAide.AsDecimal(row3["superPositionTaking"], 0);
                    OutStanding = CastAide.AsDecimal(row3["customerOutstanding"], 0);
                    Cash = CastAide.AsDecimal(row3["superCash"], 0);
                    TotalBal = Cash + CastAide.AsDecimal(row3["customerCash"], 0)
                        + CastAide.AsDecimal(row3["agentCash"], 0)
                        + CastAide.AsDecimal(row3["masterCash"], 0);
                    YesterdayCash = CastAide.AsDecimal(row3["yesterday_superCash"], 0);
                    YesterdayBal = YesterdayCash + CastAide.AsDecimal(row3["yesterday_customerCash"], 0)
                        + CastAide.AsDecimal(row3["yesterday_agentCash"], 0)
                        + CastAide.AsDecimal(row3["yesterday_masterCash"], 0);
                    UsedCredit = IBCUtils.FormatHandler.GetDecimal(row3["superUsedCredit"]);
                    DownlineName = "Master";
                    break;

                default:
                    break;
            }
        }
    }
}
