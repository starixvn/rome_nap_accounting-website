using Sunplus.Accounting.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.ViewModels.CustomerSetting.Suspend.Single
{
    public class SingleUpdateAffectedDownline
    {
        public SingleUpdateAffectedDownline(int level, int affectedDownlines, int remainUnchangedDownlines)
        {
            Level = level;
            AffectedDownlines = affectedDownlines;
            RemainUnchangedDownlines = remainUnchangedDownlines;
        }

        public int Level { get; }

        public string LevelName { get
            {
                switch (Level)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        return AccCommon.GetRoleName(Level);
                    default:
                        return "Total";
                }
                
            }
        }

        public int AffectedDownlines { get; }

        public int RemainUnchangedDownlines { get; }
    }
}
