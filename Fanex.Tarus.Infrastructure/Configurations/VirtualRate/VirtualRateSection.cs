// --------------------------------------------------------------------------
// <copyright file="VirtualRateSection.cs" company="Nexcel Solutions Vietnam">
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
    /// Virtual Rate Section.
    /// </summary>
    public class VirtualRateSection : ConfigurationSection
    {
        public static VirtualRateSection Instance
        {
            get
            {
                return ConfigurationManager.GetSection("VirtualRateSection") as VirtualRateSection;
            }
        }

        [ConfigurationProperty("Sports", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(VirtualRateCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public VirtualRateCollection Sports
        {
            get
            {
                return (VirtualRateCollection)base["Sports"];
            }
        }

        [ConfigurationProperty("Functions", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(VirtualRateCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public VirtualRateCollection Functions
        {
            get
            {
                return (VirtualRateCollection)base["Functions"];
            }
        }
    }
}
