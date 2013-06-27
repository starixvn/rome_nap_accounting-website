// -----------------------------------------------------------------------
// <copyright file="NumberGameReportForLinceseeSection.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.SendEmailCore.Configurations
{
    using System;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MappingSpecialDeleteUrlSection : ConfigurationSection
    {
        [ConfigurationProperty("SpecialDeleteUrl", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SpecialDeleteUrlCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteUrlCollection SpecialDeleteUrl
        {
            get
            {
                var urlCollection =
                    (SpecialDeleteUrlCollection)base["SpecialDeleteUrl"];
                return urlCollection;
            }
        }

        public static MappingSpecialDeleteUrlSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("MappingSpecialDeleteUrlSection") as MappingSpecialDeleteUrlSection;
                return spSection;
            }
        }
    }

    public class SpecialDeleteUrlCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(SpecialDeleteUrlElement SpecialDeleteUrlElement)
        {
            BaseAdd(SpecialDeleteUrlElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(SpecialDeleteUrlElement ReportUrlElement)
        {
            if (BaseIndexOf(ReportUrlElement) >= 0)
                BaseRemove(ReportUrlElement.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        public void Clear()
        {
            BaseClear();
        }

        public SpecialDeleteUrlElement this[int index]
        {
            get
            {
                return (SpecialDeleteUrlElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SpecialDeleteUrlElement this[string name]
        {
            get
            {
                return (SpecialDeleteUrlElement)BaseGet(name);
            }
        }

        public int IndexOf(SpecialDeleteUrlElement ReportUrlElement)
        {
            return BaseIndexOf(ReportUrlElement);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new SpecialDeleteUrlElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SpecialDeleteUrlElement)element).Name;
        }
    }

    public class SpecialDeleteUrlElement : ConfigurationElement
    {        
        [ConfigurationProperty("name", DefaultValue = "NA", IsRequired = true, IsKey = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
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

        [ConfigurationProperty("accurl", DefaultValue = "", IsRequired = true, IsKey = true)]
        public String AccUrl
        {
            get
            {
                var accUrl = (String)this["accurl"];
                return string.IsNullOrWhiteSpace(accUrl) ? string.Empty : accUrl.Trim();
            }
            set
            {
                this["accurl"] = value;
            }
        }

        [ConfigurationProperty("napurl", DefaultValue = "", IsRequired = true, IsKey = true)]
        public String NapUrl
        {
            get
            {
                var napUrl = (String)this["napurl"];
                return string.IsNullOrWhiteSpace(napUrl) ? string.Empty : napUrl.Trim();
            }
            set
            {
                this["napurl"] = value;
            }
        }
    }
}
