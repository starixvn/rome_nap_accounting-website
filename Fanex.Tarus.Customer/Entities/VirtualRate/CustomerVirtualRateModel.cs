// --------------------------------------------------------------------------
// <copyright file="CustomerVirtualRateModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Customer Virtual Rate Model.
    /// </summary>
    [Serializable]
    public class CustomerVirtualRateModel
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string RoleName { get; set; }

        public bool EnableVirtualRate { get; set; }

        public decimal MaxVirtualRate { get; set; }

        public decimal MinVirtualRate { get; set; }

        public string ActualExchangeRate { get; set; }

        public List<BaseModel> BetTypeList { get; set; }

        public List<SportVirtualRateModel> SportVirtualRateList { get; set; }
    }
}
