namespace Fanex.Tarus.Finance.Entities
{
    public class CustomerInfo
    {
        public long Id { get; set; }

        public long Order { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Type { get; set; }

        public long ParentId { get; set; }

        public int RoleId { get; set; }

        public int IsAgent { get; set; }

        public bool DisabledHR { get; set; }

        public bool DisableCS { get; set; }

        public bool DisabledFI { get; set; }

        public bool BingoGameEnabled { get; set; }

        public bool EnableLiveCasino { get; set; }

        public bool VisibleP2PSetting { get; set; }

        public bool IsDirectMaster { get; set; }
    }
}