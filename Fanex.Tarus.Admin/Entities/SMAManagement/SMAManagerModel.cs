using System;
using System.Collections.Generic;

namespace Fanex.Tarus.Admin.Entities.SMAManagement
{
    [Serializable]
    public class SMAManagerModel
    {
        public int TotalGroups { get; set; }

        public int TotalMemberNotAssignedToGroup { get; set; }

        public IList<SMAGroupHeadline> SMAGroupHeadlines { get; set; }
    }
}
