// --------------------------------------------------------------------------
// <copyright file="Admin_AutoFollowBetElement.cs" company="Nexcel Solutions Vietnam">
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
    /// Admin Auto Follow Bet Element.
    /// </summary>
    public class Admin_AutoFollowBetElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)this["Name"];
            }

            set
            {
                this["Name"] = value;
            }
        }

        [ConfigurationProperty("Id", IsRequired = false)]
        public string Id
        {
            get
            {
                return (string)this["Id"];
            }

            set
            {
                this["Id"] = value;
            }
        }

        [ConfigurationProperty("Active", IsRequired = false)]
        public bool Active
        {
            get
            {
                return (bool)this["Active"];
            }

            set
            {
                this["Active"] = value;
            }
        }

        [ConfigurationProperty("SubTypes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(Admin_AutoFollowBetCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public Admin_AutoFollowBetCollection SubTypes
        {
            get
            {
                var stores = (Admin_AutoFollowBetCollection)base["SubTypes"];
                return stores;
            }
        }
    }
}
