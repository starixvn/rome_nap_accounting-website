namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;

    public class MicroGamingBetTypesCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public MicroGamingBetTypesElement this[int index]
        {
            get
            {
                return (MicroGamingBetTypesElement)BaseGet(index);
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

        public new MicroGamingBetTypesElement this[string name]
        {
            get
            {
                return (MicroGamingBetTypesElement)BaseGet(name);
            }
        }

        public void Add(MicroGamingBetTypesElement element)
        {
            BaseAdd(element);
        }

        public void Remove(MicroGamingBetTypesElement element)
        {
            if (BaseIndexOf(element) >= 0)
            {
                BaseRemove(element.Name);
            }
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

        public int IndexOf(MicroGamingBetTypesElement element)
        {
            return BaseIndexOf(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new MicroGamingBetTypesElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MicroGamingBetTypesElement)element).Name;
        }
    }
}
