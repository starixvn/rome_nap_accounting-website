// --------------------------------------------------------------------------
// <copyright file="InsertTempRateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Constants;
    using Fanex.Tarus.Customer.Entities.VirtualRate;

    /// <summary>
    /// Check Import Virtual Rate Criteria.
    /// </summary>
    public class InsertTempRateCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public List<CustomerVirtualRateModel> CustomerVirtualRateList { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_VirtualRate_InsertTemp";
        }

        public override bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>Store Procedure Parameters.</returns>
        public override Hashtable GetSpParams()
        {
            Hashtable storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("userid", UserId);
            storeProcedureParams.Add("virtualrate", VirtualRateParam());
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            return storeProcedureParams;
        }

        /// <summary>
        /// Gets the virtual rate parameter.
        /// </summary>
        /// <returns>The virtual rate parameter.</returns>
        private string VirtualRateParam()
        {
            var paramXML = "<ROOT>";

            var customerCount = CustomerVirtualRateList.Count;
            for (var i = 0; i < customerCount; i++)
            {
                var customerVirtualRate = CustomerVirtualRateList[i];
                paramXML += string.Format("<customer username=\"{0}\">", customerVirtualRate.Username);
                var sportCount = customerVirtualRate.SportVirtualRateList.Count;
                for (var j = 0; j < sportCount; j++)
                {
                    var sportVirtualRate = customerVirtualRate.SportVirtualRateList[j];
                    var betTypeCount = sportVirtualRate.BetTypeList.Count;
                    for (var k = 0; k < betTypeCount; k++)
                    {
                        var betType = sportVirtualRate.BetTypeList[k];
                        var virtualMultiplierFormat = "<rate sporttype=\"{0}\" bettype=\"{1}\" islive=\"{2}\" rate=\"{3}\" /> ";

                        paramXML += AddBetTypeParam(
                            sportVirtualRate.LiveVirtualRate[k],
                            sportVirtualRate.Id,
                            betType,
                            LiveTypeConstants.Live,
                            virtualMultiplierFormat);
                        paramXML += AddBetTypeParam(
                            sportVirtualRate.NonLiveVirtualRate[k],
                            sportVirtualRate.Id,
                            betType,
                            LiveTypeConstants.NonLive,
                            virtualMultiplierFormat);
                    }
                }

                paramXML += "</customer>";
            }

            paramXML += "</ROOT>";
            return paramXML;
        }

        /// <summary>
        /// Adds the bet type parameter.
        /// </summary>
        /// <param name="liveRate">The live rate.</param>
        /// <param name="sportId">The sport identifier.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="liveType">Type of the live.</param>
        /// <param name="virtualMultiplierFormat">The virtual multiplier format.</param>
        /// <returns>
        /// The bet type parameter.
        /// </returns>
        private string AddBetTypeParam(
            VirtualRateModel liveRate,
            int sportId,
            BetTypeModel betType,
            int liveType,
            string virtualMultiplierFormat)
        {
            var paramXML = string.Empty;
            if (!liveRate.HasData)
            {
                return paramXML;
            }

            var rateValue = (liveRate.PercentRate + 100) / 100;
            paramXML += string.Format(virtualMultiplierFormat, sportId, betType.Id, liveType, rateValue);
            if (betType.MultiBetTypes != null && betType.MultiBetTypes.Length > 0)
            {
                for (var k = 0; k < betType.MultiBetTypes.Length; k++)
                {
                    paramXML += string.Format(virtualMultiplierFormat, sportId, betType.MultiBetTypes[k], liveType, rateValue);
                }
            }

            return paramXML;
        }
    }
}
