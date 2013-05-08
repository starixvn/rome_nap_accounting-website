using System.Configuration;

namespace Fanex.Tarus.Infrastructure.Configurations.StoreProcedureGroup
{
    public class LogDisplaySection : ConfigurationSection
    {
        public enum StoreCollection
        {
            Stores
        }

        [ConfigurationProperty("Stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Stores
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Stores"];
                return spCollection;
            }
        }

        public static LogDisplaySection Instance
        {
            get
            {
                var logDisplaySession = ConfigurationManager.GetSection("StoreProcedureGroup/LogDisplay") as LogDisplaySection;
                return logDisplaySession;
            }
        }


        public StoreProcedureCollection this[StoreCollection name]
        {
            get
            {

                var spCollection = (StoreProcedureCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }
    }
}
