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
    using Fanex.Tarus.Customer.Entities;
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

        public List<BaseModel> BetTypeList { get; set; }

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
            var betTypeCount = BetTypeList.Count;
            for (var i = 0; i < sportCount; i++)
            {
                var sportVirtualRate = SportVirtualRateList[i];
                for (var j = 0; j < betTypeCount; j++)
                {
                    var betType = BetTypeList[j];
                    var liveRate = sportVirtualRate.LiveVirtualRate[j].PercentRate;
                    var oldLiveRate = LastSettings.SportVirtualRateList[i].LiveVirtualRate[j].PercentRate;
                    var virtualMultiplierFormat = "<Rate SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" VirtualMultiplier=\"{3}\" /> ";
                    if (liveRate != oldLiveRate)
                    {
                        paramXML += string.Format(
                            virtualMultiplierFormat,
                            sportVirtualRate.Id,
                            betType.Id,
                            LiveTypeConstants.Live,
                            liveRate + 1);
                    }

                    liveRate = sportVirtualRate.NonLiveVirtualRate[j].PercentRate;
                    oldLiveRate = LastSettings.SportVirtualRateList[i].NonLiveVirtualRate[j].PercentRate;
                    if (liveRate != oldLiveRate)
                    {
                        paramXML += string.Format(
                            virtualMultiplierFormat,
                            sportVirtualRate.Id,
                            betType.Id,
                            LiveTypeConstants.NonLive,
                            liveRate + 1);
                    }
                }
            }

            paramXML += "</ROOT>";
            return paramXML;
        }
    }
}
