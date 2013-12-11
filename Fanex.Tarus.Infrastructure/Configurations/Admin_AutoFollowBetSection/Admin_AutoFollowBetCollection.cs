// --------------------------------------------------------------------------
// <copyright file="Admin_AutoFollowBetCollection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Admin Auto Follow Bet Collection.
    /// </summary>
    public class Admin_AutoFollowBetCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public Admin_AutoFollowBetElement this[int index]
        {
            get
            {
                return (Admin_AutoFollowBetElement)BaseGet(index);
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

        public new Admin_AutoFollowBetElement this[string name]
        {
            get
            {
                return (Admin_AutoFollowBetElement)BaseGet(name);
            }
        }

        public void Add(Admin_AutoFollowBetElement element)
        {
            BaseAdd(element);
        }

        public void Remove(Admin_AutoFollowBetElement element)
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

        public int IndexOf(Admin_AutoFollowBetElement element)
        {
            return BaseIndexOf(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new Admin_AutoFollowBetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Admin_AutoFollowBetElement)element).Name;
        }
    }
}
