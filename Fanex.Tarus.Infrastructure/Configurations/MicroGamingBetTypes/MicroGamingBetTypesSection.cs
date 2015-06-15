namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;

    public class MicroGamingBetTypesSection : ConfigurationSection
    {
        public static MicroGamingBetTypesSection Instance
        {
            get
            {
                var sportSection =
                 ConfigurationManager.GetSection("MicroGamingBetTypesSection") as MicroGamingBetTypesSection;
                return sportSection;
            }
        }

        [ConfigurationProperty("BetTypes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(KenoBetTypesCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MicroGamingBetTypesCollection BetTypes
        {
            get
            {
                return (MicroGamingBetTypesCollection)base["BetTypes"];
            }
        }
    }
}
