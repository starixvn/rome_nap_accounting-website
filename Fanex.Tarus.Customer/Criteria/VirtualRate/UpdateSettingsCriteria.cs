// --------------------------------------------------------------------------
// <copyright file="UpdateSettingsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.VirtualRate
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Constants;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;

    /// <summary>
    /// Update Settings Criteria.
    /// </summary>
    public class UpdateSettingsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string RoleName { get; set; }

        public bool EnableVirtualRate { get; set; }

        public decimal MaxVirtualRate { get; set; }

        public decimal MinVirtualRate { get; set; }

        public string ActualExchangeRate { get; set; }

        public List<SportVirtualRateModel> SportVirtualRateList { get; set; }

        public CustomerVirtualRateModel LastSettings { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public string IP
        {
            get
            {
                return Utils.GetClientIP();
            }
        }

        public string Language
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        public override string GetSettingKey()
        {
            return "Customer_VirtualRate_UpdateVirtualRate";
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
            storeProcedureParams.Add("custid", CustId);
            storeProcedureParams.Add("enablevirtualrate", EnableVirtualRate);
            if (EnableVirtualRate)
            {
                storeProcedureParams.Add("virtualmultiplier", VirtualRateParam());
            }

            storeProcedureParams.Add("userid", UserId);
            storeProcedureParams.Add("ip", IP);
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

            var sportCount = SportVirtualRateList.Count;
            for (var i = 0; i < sportCount; i++)
            {
                var sportVirtualRate = SportVirtualRateList[i];
                var sportId = sportVirtualRate.Id;
                var betTypeCount = sportVirtualRate.BetTypeList.Count;
                for (var j = 0; j < betTypeCount; j++)
                {
                    var betType = sportVirtualRate.BetTypeList[j];
                    var virtualMultiplierFormat = "<Rate SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" VirtualMultiplier=\"{3}\" /> ";
                    if (sportVirtualRate.SubSportList != null && sportVirtualRate.SubSportList.Count > 0)
                    {
                        var subSportLength = sportVirtualRate.SubSportList.Count;
                        for (var k = 0; k < subSportLength; k++)
                        {
                            var subSport = sportVirtualRate.SubSportList[k];
                            var liveRate = sportVirtualRate.SubSportList[k].NonLiveVirtualRate[j];
                            var oldLiveRate = LastSettings.SportVirtualRateList[i].SubSportList[k].NonLiveVirtualRate[j];
                            paramXML += AddBetTypeParam(
                               liveRate,
                               oldLiveRate,
                               subSport.Id,
                               betType,
                               LiveTypeConstants.Live,
                               virtualMultiplierFormat);
                            paramXML += AddBetTypeParam(
                               liveRate,
                               oldLiveRate,
                               subSport.Id,
                               betType,
                               LiveTypeConstants.NonLive,
                               virtualMultiplierFormat);
                        }
                    }
                    else
                    {
                        paramXML += AddBetTypeParam(
                            sportVirtualRate.LiveVirtualRate[j],
                            LastSettings.SportVirtualRateList[i].LiveVirtualRate[j],
                            sportVirtualRate.Id,
                            betType,
                            LiveTypeConstants.Live,
                            virtualMultiplierFormat);
                        paramXML += AddBetTypeParam(
                            sportVirtualRate.NonLiveVirtualRate[j],
                            LastSettings.SportVirtualRateList[i].NonLiveVirtualRate[j],
                            sportVirtualRate.Id,
                            betType,
                            LiveTypeConstants.NonLive,
                            virtualMultiplierFormat);
                    }
                }
            }

            paramXML += "</ROOT>";
            return paramXML;
        }

        /// <summary>
        /// Adds the bet type parameter.
        /// </summary>
        /// <param name="liveRate">The live rate.</param>
        /// <param name="oldLiveRate">The old live rate.</param>
        /// <param name="sportId">The sport identifier.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="liveType">Type of the live.</param>
        /// <param name="virtualMultiplierFormat">The virtual multiplier format.</param>
        /// <returns>
        /// The bet type parameter.
        /// </returns>
        private string AddBetTypeParam(
            VirtualRateModel liveRate,
            VirtualRateModel oldLiveRate,
            int sportId,
            BetTypeModel betType,
            int liveType,
            string virtualMultiplierFormat)
        {
            var paramXML = string.Empty;
            if (liveRate.PercentRate == oldLiveRate.PercentRate)
            {
                return paramXML;
            }

            paramXML += string.Format(virtualMultiplierFormat, sportId, betType.Id, liveType, liveRate.PercentRate);
            if (betType.MultiBetTypes != null && betType.MultiBetTypes.Length > 0)
            {
                for (var k = 0; k < betType.MultiBetTypes.Length; k++)
                {
                    paramXML += string.Format(virtualMultiplierFormat, sportId, betType.MultiBetTypes[k], liveType, liveRate.PercentRate);
                }
            }

            return paramXML;
        }
    }
}
