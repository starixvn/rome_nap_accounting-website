namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.Common;

    public partial class CustomerInfoViewModel
    {
        public int MAgentActive => Converter.ToInt(downlineStatusInfoRow["MAgentActive"]);

        public int MAgentClosed => Converter.ToInt(downlineStatusInfoRow["MAgentClosed"]);

        public int MAgentSuspended => Converter.ToInt(downlineStatusInfoRow["MAgentSuspended"]);

        public int AgentActive => Converter.ToInt(downlineStatusInfoRow["AgentActive"]);

        public int AgentClosed => Converter.ToInt(downlineStatusInfoRow["AgentClosed"]);

        public int AgentSuspended => Converter.ToInt(downlineStatusInfoRow["AgentSuspended"]);

        public int MemberActive => Converter.ToInt(downlineStatusInfoRow["MemberActive"]);

        public int MemberClosed => Converter.ToInt(downlineStatusInfoRow["MemberClosed"]);

        public int MemberSuspended => Converter.ToInt(downlineStatusInfoRow["MemberSuspended"]);
    }
}
