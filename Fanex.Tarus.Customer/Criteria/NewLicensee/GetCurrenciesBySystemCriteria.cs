// --------------------------------------------------------------------------
// <copyright file="GetFinancialReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.NewLicensee
{
    public class GetCurrenciesBySystemCriteria
    {
        public string GetSettingKey()
        {
            return "Customer_NewLicensee_GetCurrencies";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
