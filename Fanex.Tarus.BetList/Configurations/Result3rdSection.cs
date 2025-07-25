using System.Configuration;

namespace Fanex.Tarus.BetList.Configurations
{  
    public sealed class Result3rdSection : ConfigurationSection
    {
        public static Result3rdSection Instance { get { return (Result3rdSection)ConfigurationManager.GetSection("Result3rdSection"); } }
    

        [ConfigurationProperty("Result3rdCollection", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(Result3rdCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public Result3rdCollection InternalUrlCollection
            => (Result3rdCollection)base["Result3rdCollection"];
    }

    public sealed class Result3rdCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
            => new Result3rdElement();

        protected override object GetElementKey(ConfigurationElement element)
            => ((Result3rdElement)element).Name;

        new public Result3rdElement this[string name]
        {
            get
            {
                return (Result3rdElement)base.BaseGet(name);
            }
        }
    }

    public sealed class Result3rdElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"].ToString();
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("SportTypeId", IsRequired = true)]
        public int SportTypeId
        {
            get
            {
                return (int)this["SportTypeId"];
            }
            set
            {
                this["SportTypeId"] = value;
            }
        }


        [ConfigurationProperty("VendorId", IsRequired = false)]
        public int VendorId
        {
            get
            {
                return (int)this["VendorId"];
            }
            set
            {
                this["VendorId"] = value;
            }
        }

        [ConfigurationProperty("Url", IsRequired = true)]
        public string Url
        {
            get
            {
                return this["Url"].ToString();
            }
            set
            {
                this["Url"] = value;
            }
        }

        [ConfigurationProperty("Debug", IsRequired = true)]
        public int Debug
        {
            get
            {
                return (int) this["Debug"];
            }
            set
            {
                this["Debug"] = value;
            }
        }

        [ConfigurationProperty("IsPostMethod", IsRequired = true)]
        public bool IsPostMethod
        {
            get
            {
                return (bool)this["IsPostMethod"];
            }
            set
            {
                this["IsPostMethod"] = value;
            }
        }
    }
}
