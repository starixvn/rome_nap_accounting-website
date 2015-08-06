namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;
    using Fanex.Common;

    public class MicroGamingBetTypesElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = 0, IsRequired = true, IsKey = true)]
        public int Id
        {
            get
            {
                return Converter.ToInt(this["id"]);
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
                return Converter.ToString2(this["name"]);
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
                return Converter.ToBool(this["active"]);
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
                return Converter.ToString2(this["gameType"]);
            }

            set
            {
                this["gameType"] = value;
            }
        }
    }
}
