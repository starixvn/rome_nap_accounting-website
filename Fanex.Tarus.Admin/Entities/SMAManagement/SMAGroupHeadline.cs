using System;
using System.Collections.Generic;

namespace Fanex.Tarus.Admin.Entities.SMAManagement
{
    [Serializable]
    public class SMAGroupHeadline
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Owner { get; set; }

        public IList<SMAEntity> SMAEntities { get; set; }
    }
}
