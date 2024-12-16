namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.Common;

    public partial class CustomerInfoViewModel
    {
        public int SAgentActive => Converter.ToInt(downlineStatusInfoRow["SAgentActive"]);

        public int SAgentViewOnly => Converter.ToInt(downlineStatusInfoRow["SAgentViewOnly"]);

        public int SAgentSuspended => Converter.ToInt(downlineStatusInfoRow["SAgentSuspended"]);

        public int SAgentInactive => Converter.ToInt(downlineStatusInfoRow["SAgentInactive"]);

        public int MAgentActive => Converter.ToInt(downlineStatusInfoRow["MAgentActive"]);

        public int MAgentClosed => Converter.ToInt(downlineStatusInfoRow["MAgentClosed"]);

        public int MAgentSuspended => Converter.ToInt(downlineStatusInfoRow["MAgentSuspended"]);

        public int MAgentViewOnly => Converter.ToInt(downlineStatusInfoRow["MAgentViewOnly"]);

        public int MAgentInactive => Converter.ToInt(downlineStatusInfoRow["MAgentInactive"]);

        public int AgentActive => Converter.ToInt(downlineStatusInfoRow["AgentActive"]);

        public int AgentClosed => Converter.ToInt(downlineStatusInfoRow["AgentClosed"]);

        public int AgentSuspended => Converter.ToInt(downlineStatusInfoRow["AgentSuspended"]);

        public int AgentViewOnly => Converter.ToInt(downlineStatusInfoRow["AgentViewOnly"]);

        public int AgentInactive => Converter.ToInt(downlineStatusInfoRow["AgentInactive"]);

        public int MemberActive => Converter.ToInt(downlineStatusInfoRow["MemberActive"]);

        public int MemberClosed => Converter.ToInt(downlineStatusInfoRow["MemberClosed"]);

        public int MemberSuspended => Converter.ToInt(downlineStatusInfoRow["MemberSuspended"]);

        public int MemberViewOnly => Converter.ToInt(downlineStatusInfoRow["MemberViewOnly"]);

        public int MemberInactive => Converter.ToInt(downlineStatusInfoRow["MemberInactive"]);

        public int DirectMemberActive => Converter.ToInt(downlineStatusInfoRow["DirectMemberActive"]);
        public int DirectMemberClosed => Converter.ToInt(downlineStatusInfoRow["DirectMemberClosed"]);
        public int DirectMemberSuspended => Converter.ToInt(downlineStatusInfoRow["DirectMemberSuspended"]);
    }
}
