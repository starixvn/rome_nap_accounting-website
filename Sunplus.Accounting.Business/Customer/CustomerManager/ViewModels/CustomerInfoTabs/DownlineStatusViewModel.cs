namespace Sunplus.Accounting.Business.Customer
{
    using DNAide.Core;

    public partial class CustomerInfoViewModel
    {
        public int MAgentActive => CastAide.AsInt32(downlineStatusInfoRow["MAgentActive"], 0);

        public int MAgentClosed => CastAide.AsInt32(downlineStatusInfoRow["MAgentClosed"], 0);

        public int MAgentSuspended => CastAide.AsInt32(downlineStatusInfoRow["MAgentSuspended"], 0);

        public int AgentActive => CastAide.AsInt32(downlineStatusInfoRow["AgentActive"], 0);

        public int AgentClosed => CastAide.AsInt32(downlineStatusInfoRow["AgentClosed"], 0);

        public int AgentSuspended => CastAide.AsInt32(downlineStatusInfoRow["AgentSuspended"], 0);

        public int MemberActive => CastAide.AsInt32(downlineStatusInfoRow["MemberActive"], 0);

        public int MemberClosed => CastAide.AsInt32(downlineStatusInfoRow["MemberClosed"], 0);

        public int MemberSuspended => CastAide.AsInt32(downlineStatusInfoRow["MemberSuspended"], 0);
    }
}
