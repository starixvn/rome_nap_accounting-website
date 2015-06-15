namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;

    public class MicroGamingBetTypesElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }

            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("name", DefaultValue = "NA", IsRequired = true, IsKey = false)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("active", DefaultValue = true, IsRequired = true, IsKey = true)]
        public bool Active
        {
            get
            {
                return (bool)this["active"];
            }

            set
            {
                this["active"] = value;
            }
        }

        [ConfigurationProperty("gameType", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string GameType
        {
            get
            {
                return (string)this["gameType"];
            }

            set
            {
                this["gameType"] = value;
            }
        }
    }
}
