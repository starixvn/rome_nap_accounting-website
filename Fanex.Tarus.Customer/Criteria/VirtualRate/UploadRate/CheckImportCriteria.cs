// --------------------------------------------------------------------------
// <copyright file="CheckImportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Check Import Virtual Rate Criteria.
    /// </summary>
    public class CheckImportCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_VirtualRate_CheckImport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
