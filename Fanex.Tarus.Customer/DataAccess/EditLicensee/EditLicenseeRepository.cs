// --------------------------------------------------------------------------
// <copyright file="EditLicenseeRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.EditLicensee
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.EditLicensee;
    using Fanex.Tarus.Customer.Entities.EditLicensee;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Constants;
    using Sunplus.Accounting.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditLicenseeRepository : IEditLicenseeRepository
    {
        private const string EmptyBetType = "NA";
        public LicenseeInfo GetLicenseeInfo(GetLicenseeInfoCriteria criteria, ref Hashtable outputParams)
        {
            DataSet dataset;
            Hashtable param = new Hashtable();
            param.Add("licenseename", criteria.LicenseeName);
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(param);
                dataset = objDb.ExecuteDataSet();
                outputParams = objDb.GetOuputParams();
            }

            LicenseeInfo licenseeInfo = null;
            DataTable dataTable = new DataTable();
            if (dataset != null && dataset.Tables.Count == 0)
            {
                return licenseeInfo;
            }

            dataTable = dataset.Tables[0];
            var fakeSettingValue = 0.3;
            licenseeInfo = new LicenseeInfo
            {
                Site = "12bet1",
                Currency = "UUS1",
                EnableNumberGame = true
            };

            var settingInfo = new List<SettingInfo>();

            #region Sportsbook

            string paramName;
            string sportId;
            var sports = SportsSection.Instance.Sports;
            foreach (SportsElement sport in sports)
            {
                sportId = sport.Id.ToLower();
                if (string.Equals(sportId, Sports.NumberGame.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                paramName = "minbetv" + sportId;
                settingInfo.Add(new SettingInfo
                {
                    Name = paramName,
                    Value = fakeSettingValue
                });

                paramName = "maxbetv" + sportId;
                settingInfo.Add(new SettingInfo
                {
                    Name = paramName,
                    Value = fakeSettingValue
                });

                paramName = "maxpermatchv" + sportId;
                settingInfo.Add(new SettingInfo
                {
                    Name = paramName,
                    Value = fakeSettingValue
                });

                if (string.Equals(sportId, "99mp", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var betTypes = sport.Bettype.Split(new char[] { ',' });
                var betTypesLength = betTypes.Length;
                for (var l = 0; l < betTypesLength; l++)
                {
                    paramName = "v" + sportId + "spositiontaking_" + betTypes[l];
                    settingInfo.Add(new SettingInfo
                    {
                        Name = paramName,
                        Value = fakeSettingValue
                    });

                    paramName = "v" + sportId + "mpositiontaking_" + betTypes[l];
                    settingInfo.Add(new SettingInfo
                    {
                        Name = paramName,
                        Value = fakeSettingValue
                    });

                    paramName = "v" + sportId + "apositiontaking_" + betTypes[l];
                    settingInfo.Add(new SettingInfo
                    {
                        Name = paramName,
                        Value = fakeSettingValue
                    });

                    if (string.Equals(sportId, Sports.Soccer.ToString()))
                    {
                        paramName = "v" + sportId + "slivepositiontaking_" + betTypes[l];
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });

                        paramName = "v" + sportId + "mlivepositiontaking_" + betTypes[l];
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });

                        paramName = "v" + sportId + "alivepositiontaking_" + betTypes[l];
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });
                    }
                }
            }

            #endregion Sportsbook
            #region NumberGame

            sportId = Sports.NumberGame.ToString();
            paramName = "minbetv" + sportId;
            settingInfo.Add(new SettingInfo
            {
                Name = paramName,
                Value = fakeSettingValue
            });

            paramName = "maxbetv" + sportId;
            settingInfo.Add(new SettingInfo
            {
                Name = paramName,
                Value = fakeSettingValue
            });

            paramName = "maxpermatchv" + sportId;
            settingInfo.Add(new SettingInfo
            {
                Name = paramName,
                Value = fakeSettingValue
            });

            paramName = "maxperballv" + sportId;
            settingInfo.Add(new SettingInfo
            {
                Name = paramName,
                Value = fakeSettingValue
            });

            var levelType = new string[] { 
                SMALevelNameConstants.Super,
                SMALevelNameConstants.Master,
                SMALevelNameConstants.Agent
            };

            var levelTypeLength = levelType.Length;
            for (var i = 0; i < levelTypeLength; i++)
            {
                var type = levelType[i];
                foreach (SportsElement sport in sports)
                {
                    if (!string.Equals(sport.Id.Trim(), sportId) ||
                        sport.Bettype == null ||
                        sport.Bettype == EmptyBetType ||
                        sport.Bettype == string.Empty ||
                        string.IsNullOrWhiteSpace(sport.Bettype.Trim()))
                    {
                        continue;
                    }

                    var betTypes = sport.Bettype.Split(new char[] { ',' });
                    foreach (var betType in betTypes)
                    {
                        var betTypeAfterTrim = betType.Trim();
                        paramName = "v" + sportId + type + "positiontaking_" + betTypeAfterTrim;
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });

                        paramName = "v" + sportId + type + "livepositiontaking_" + betTypeAfterTrim;
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });
                    }
                }
            }

            #endregion NumberGame
            #region Racing

            var racings = RacingSection.Instance.Racings;
            foreach (RacingElement racing in racings)
            {
                if (!racing.Active)
                {
                    continue;
                }

                foreach (ToteElement tote in racing.Totes)
                {
                    if (!tote.Active)
                    {
                        continue;
                    }

                    paramName = "v" + racing.Id + "minbet_" + tote.Id;
                    settingInfo.Add(new SettingInfo
                    {
                        Name = paramName,
                        Value = fakeSettingValue
                    });

                    paramName = "v" + racing.Id + "maxbet_" + tote.Id;
                    settingInfo.Add(new SettingInfo
                    {
                        Name = paramName,
                        Value = fakeSettingValue
                    });

                    paramName = "v" + racing.Id + "maxpermatch_" + tote.Id;
                    settingInfo.Add(new SettingInfo
                    {
                        Name = paramName,
                        Value = fakeSettingValue
                    });

                    if (tote.BetType == null ||
                        tote.BetType == EmptyBetType ||
                        tote.BetType == string.Empty ||
                        string.IsNullOrWhiteSpace(tote.BetType.Trim()))
                    {
                        continue;
                    }

                    var betTypes = tote.BetType.Split(new char[] { ',' });
                    foreach (var betType in betTypes)
                    {
                        paramName = "v" + racing.Id + "discount_" + betType;
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });

                        paramName = "v" + racing.Id + "spositiontaking_" + betType;
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });

                        paramName = "v" + racing.Id + "mpositiontaking_" + betType;
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });

                        paramName = "v" + racing.Id + "apositiontaking_" + betType;
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });

                        paramName = "v" + racing.Id + "ibcpositiontaking_" + betType;
                        settingInfo.Add(new SettingInfo
                        {
                            Name = paramName,
                            Value = fakeSettingValue
                        });
                    }

                    paramName = "disable" + racing.Id + "_" + tote.Id;
                    settingInfo.Add(new SettingInfo
                    {
                        Name = paramName,
                        Value = true
                    });
                }
            }


            #endregion Racing
            #region VirtualSports

            var virtualSports = VirtualSportsSection.Instance.Sports;

            paramName = "disablevirtualsports";
            settingInfo.Add(new SettingInfo
            {
                Name = paramName,
                Value = false
            });

            Dictionary<string, bool> activeVirtualSportsInfo = new Dictionary<string, bool>();
            foreach (VirtualSportsElement sport in virtualSports)
            {
                sportId = sport.Id;
                if (activeVirtualSportsInfo.ContainsKey(sportId))
                {
                    continue;
                }

                activeVirtualSportsInfo.Add(sportId, sport.Active);
            }

            var virtualSportsParams = "<products><product SportType=\"180\" MaxBet=\"0.3\" MinBet=\"0.3\" MaxPerMatch=\"0.3\" sPositionTaking=\"0.3\" mPositionTaking=\"0.3\" aPositionTaking=\"0.3\"></product><product SportType=\"181\" MaxBet=\"0.3\" MinBet=\"0.3\" MaxPerMatch=\"0.3\" sPositionTaking=\"0.3\" mPositionTaking=\"0.3\" aPositionTaking=\"0.3\"></product><product SportType=\"186\" MaxBet=\"0.3\" MinBet=\"0.3\" MaxPerMatch=\"0.3\" sPositionTaking=\"0.3\" mPositionTaking=\"0.3\" aPositionTaking=\"0.3\"></product><product SportType=\"182\" MaxBet=\"0.3\" MinBet=\"0.3\" MaxPerMatch=\"0.3\" sPositionTaking=\"0.3\" mPositionTaking=\"0.3\" aPositionTaking=\"0.3\"></product><product SportType=\"183\" MaxBet=\"0.3\" MinBet=\"0.3\" MaxPerMatch=\"0.3\" sPositionTaking=\"0.3\" mPositionTaking=\"0.3\" aPositionTaking=\"0.3\"></product><product SportType=\"184\" MaxBet=\"0.3\" MinBet=\"0.3\" MaxPerMatch=\"0.3\" sPositionTaking=\"0.3\" mPositionTaking=\"0.3\" aPositionTaking=\"0.3\"></product><product SportType=\"185\" MaxBet=\"0.3\" MinBet=\"0.3\" MaxPerMatch=\"0.3\" sPositionTaking=\"0.3\" mPositionTaking=\"0.3\" aPositionTaking=\"0.3\"></product></products>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(virtualSportsParams);
            string xpath = "/products/product";
            var virtualSportsNodes = xmlDoc.SelectNodes(xpath);
            List<VirtualSportsSettingInfo> virtualSportsSettingInfoList = new List<VirtualSportsSettingInfo>();
            foreach (XmlNode childrenNode in virtualSportsNodes)
            {
                sportId = childrenNode.Attributes["SportType"].Value;
                if (!activeVirtualSportsInfo[sportId])
                {
                    continue;
                }

                VirtualSportsSettingInfo virtualSportsSettingInfo = new VirtualSportsSettingInfo
                {
                    Id = sportId,
                    Name = CacheRef.GetSportName(sportId),
                    SettingInfo = new List<SettingInfo>()
                };

                virtualSportsSettingInfo.SettingInfo.Add(new SettingInfo
                {
                    Name = "maxbetv" + sportId,
                    Value = childrenNode.Attributes["MaxBet"].Value
                });

                virtualSportsSettingInfo.SettingInfo.Add(new SettingInfo
                {
                    Name = "minbetv" + sportId,
                    Value = childrenNode.Attributes["MinBet"].Value
                });

                virtualSportsSettingInfo.SettingInfo.Add(new SettingInfo
                {
                    Name = "maxpermatchv" + sportId,
                    Value = childrenNode.Attributes["MaxPerMatch"].Value
                });

                virtualSportsSettingInfo.SettingInfo.Add(new SettingInfo
                {
                    Name = "v" + sportId + "spositiontaking",
                    Value = childrenNode.Attributes["sPositionTaking"].Value
                });

                virtualSportsSettingInfo.SettingInfo.Add(new SettingInfo
                {
                    Name = "v" + sportId + "mpositiontaking",
                    Value = childrenNode.Attributes["mPositionTaking"].Value
                });

                virtualSportsSettingInfo.SettingInfo.Add(new SettingInfo
                {
                    Name = "v" + sportId + "apositiontaking",
                    Value = childrenNode.Attributes["aPositionTaking"].Value
                });

                virtualSportsSettingInfoList.Add(virtualSportsSettingInfo);
            }

            #endregion VirtualSports

            licenseeInfo.SettingInfo = settingInfo;
            licenseeInfo.VirtualSportsSettingInfo = virtualSportsSettingInfoList;
            return licenseeInfo;
        }
    }
}
