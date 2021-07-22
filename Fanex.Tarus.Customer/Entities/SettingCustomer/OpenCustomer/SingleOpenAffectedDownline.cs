namespace Fanex.Tarus.Customer.Entities.SettingCustomer.OpenCustomer
{
    using Sunplus.Accounting.Core.Common;

    public class SingleOpenAffectedDownline
    {
        public SingleOpenAffectedDownline(int level, int affectedDownlines, int remainUnchangedDownlines)
        {
            Level = level;
            AffectedDownlines = affectedDownlines;
            RemainUnchangedDownlines = remainUnchangedDownlines;
        }

        public int Level { get; }

        public string LevelName
        {
            get
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