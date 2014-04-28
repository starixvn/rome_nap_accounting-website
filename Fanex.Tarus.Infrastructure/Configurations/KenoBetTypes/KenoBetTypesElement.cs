// --------------------------------------------------------------------------
// <copyright file="KenoBetTypesElement.cs" company="Nexcel Solutions Vietnam">
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
    /// Keno Bet Types Element.
    /// </summary> 
    public class KenoBetTypesElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
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
                return (string)this["name"];
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
                return (bool)this["active"];
            }

            set
            {
                this["active"] = value;
            }
        }

        [ConfigurationProperty("minbet", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public string Minbet
        {
            get
            {
                return (string)this["minbet"];
            }

            set
            {
                this["minbet"] = value;
            }
        }

        [ConfigurationProperty("maxbet", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public string Maxbet
        {
            get
            {
                return (string)this["maxbet"];
            }

            set
            {
                this["maxbet"] = value;
            }
        }

        [ConfigurationProperty("maxpermatch", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public string Maxpermatch
        {
            get
            {
                return (string)this["maxpermatch"];
            }

            set
            {
                this["maxpermatch"] = value;
            }
        }
    }
}
