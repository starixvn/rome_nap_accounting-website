// --------------------------------------------------------------------------
// <copyright file="VirtualRateCollection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Virtual Rate Bet Types Collection.
    /// </summary>
    public class VirtualRateCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public VirtualRateElement this[int index]
        {
            get
            {
                return (VirtualRateElement)BaseGet(index);
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

        public new VirtualRateElement this[string name]
        {
            get
            {
                return (VirtualRateElement)BaseGet(name);
            }
        }

        public void Add(VirtualRateElement element)
        {
            BaseAdd(element);
        }

        public void Remove(VirtualRateElement element)
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

        public int IndexOf(VirtualRateElement element)
        {
            return BaseIndexOf(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new VirtualRateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((VirtualRateElement)element).Name;
        }
    }
}
