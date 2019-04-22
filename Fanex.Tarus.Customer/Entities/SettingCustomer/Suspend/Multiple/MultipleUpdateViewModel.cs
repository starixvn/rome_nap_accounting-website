namespace Fanex.Tarus.Customer.ViewModels.CustomerSetting.Suspend.Multiple
{
    public class MultipleUpdateViewModel
    {
        public MultipleUpdateViewModel(
            string userName, 
            bool suspended, 
            int affectedMaster,
            int affectedAgent,
            int affectedMember,
            int remainUnchangedMaster,
            int remainUnchangedAgent,
            int remainUnchangedMember)
        {
            UserName = userName;
            Suspended = suspended;
            AffectedMaster = affectedMaster;
            AffectedAgent = affectedAgent;
            AffectedMember = affectedMember;
            RemainUnchangedMaster = remainUnchangedMaster;
            RemainUnchangedAgent = remainUnchangedAgent;
            RemainUnchangedMember = remainUnchangedMember;
        }

        public string UserName { get; }

        public bool Suspended { get; }

        public int AffectedMaster { get; }

        public int AffectedAgent { get; }

        public int AffectedMember { get; }

        public int RemainUnchangedMaster { get; }

        public int RemainUnchangedAgent { get; }

        public int RemainUnchangedMember { get; }
    }
}
