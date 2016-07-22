using System;

namespace Fanex.Tarus.Admin.Entities.SMAManagement
{
    [Serializable]
    public class SMAEntity
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public bool IsSuspend { get; set; }

        public bool IsClosed { get; set; }

        public bool IsEnable { get; set; }

        public string Status { get; set; }

        public string Site { get; set; }

        public string Currency { get; set; }

        public int RoleId { get; set; }

        public string Position { get; set; }

        public int? HeadlineTypeId { get; set; }

        public string HeadlineType { get; set; }

        public string Owner { get; set; }
    }
}
