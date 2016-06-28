// --------------------------------------------------------------------------
// <copyright file="KenoBetTypesCollection.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Keno Bet Types Collection.
    /// </summary>
    public class KenoBetTypesCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public KenoBetTypesElement this[int index]
        {
            get
            {
                return (KenoBetTypesElement)BaseGet(index);
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

        public new KenoBetTypesElement this[string name]
        {
            get
            {
                return (KenoBetTypesElement)BaseGet(name);
            }
        }

        public void Add(KenoBetTypesElement kenoSportsElement)
        {
            BaseAdd(kenoSportsElement);
        }

        public void Remove(KenoBetTypesElement kenoSportsElement)
        {
            if (BaseIndexOf(kenoSportsElement) >= 0)
            {
                BaseRemove(kenoSportsElement.Name);
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

        public int IndexOf(KenoBetTypesElement kenoBetTypesElement)
        {
            return BaseIndexOf(kenoBetTypesElement);
        }

        protected override void BaseAdd(ConfigurationElement kenoBetTypesElement)
        {
            BaseAdd(kenoBetTypesElement, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new KenoBetTypesElement();
        }

        protected override object GetElementKey(ConfigurationElement kenoBetTypesElement)
        {
            return ((KenoBetTypesElement)kenoBetTypesElement).Name;
        }
    }
}
