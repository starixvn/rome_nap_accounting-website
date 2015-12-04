// --------------------------------------------------------------------------
// <copyright file="VirtualRateElement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System.Configuration;
    using Fanex.Common;

    /// <summary>
    /// Virtual Rate Bet Types Element.
    /// </summary> 
    public class VirtualRateElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = 0, IsRequired = true, IsKey = true)]
        public int Id
        {
            get
            {
                return (int)this["id"];
            }

            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("active", DefaultValue = true, IsRequired = true, IsKey = false)]
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

        [ConfigurationProperty("name", DefaultValue = "NA", IsRequired = false, IsKey = false)]
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

        [ConfigurationProperty("notes", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Notes
        {
            get
            {
                return (string)this["notes"];
            }

            set
            {
                this["notes"] = value;
            }
        }

        [ConfigurationProperty("multibettypes", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string MultiBetTypes
        {
            get
            {
                return (string)this["multibettypes"];
            }

            set
            {
                this["multibettypes"] = value;
            }
        }

        [ConfigurationProperty("sportList", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SportList
        {
            get
            {
                return (string)this["sportList"];
            }

            set
            {
                this["sportList"] = value;
            }
        }

        [ConfigurationProperty("supportBetTypes", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SupportBetTypes
        {
            get
            {
                return (string)this["supportBetTypes"];
            }

            set
            {
                this["supportBetTypes"] = value;
            }
        }

        [ConfigurationProperty("bettypeOrder", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string BetTypeOrder
        {
            get
            {
                return Converter.ToString2(this["bettypeOrder"]);
            }

            set
            {
                this["bettypeOrder"] = value;
            }
        }

        [ConfigurationProperty("BetTypes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(VirtualRateCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public VirtualRateCollection BetTypes
        {
            get
            {
                var betTypes = (VirtualRateCollection)base["BetTypes"];
                return betTypes;
            }
        }

        [ConfigurationProperty("SubSports", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(VirtualRateCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public VirtualRateCollection SubSports
        {
            get
            {
                return (VirtualRateCollection)base["SubSports"];
            }
        }
    }
}
