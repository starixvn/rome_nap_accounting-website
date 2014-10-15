//-------------------------------------------------------------------------
// <copyright file="UpdateDirectStatusEffectCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount
{
    using System.Collections;

    /// <summary>
    /// Update Direct Status Effect Criteria.
    /// </summary>
    public class UpdateDirectStatusEffectCriteria
    {
        public int CustomerId { get; set; }

        public string Username { get; set; }

        public bool IsCheckedDirectStatus { get; set; }

        public string GetSettingKey()
        {
            return "Customer_ConfigureDirectAccount_UpdateStatusEffect";
        }

        public bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>Store Procedure Parameters.</returns>
        public Hashtable GetSPParams()
        {
            Hashtable storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("custid", CustomerId);
            return storeProcedureParams;
        }
    }
}