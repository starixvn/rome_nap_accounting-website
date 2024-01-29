using Fanex.Tarus.Infrastructure.Caching;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.LogDisplay;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Sunplus.Accounting.Business.LogDisplay.CreditCustomerSetting
{
    public class CustomerSettingLogFormatter
    {
        private RecordSp _recordSp;

        private enum DisplayType
        {
            Product = 1,
            SportType = 2,
            Bettype = 3,
            SportTypeBettype = 4,
            G_GT_SportType_Bettype = 5,
            SB_Com = 6
        }

        private const int CreateSuperPTActionId = 263000;
        private const int CreateSuperCommActionId = 261000;
        private const int CreateSuperBetSettingActionId = 262000;
        private const int CreateMasterPTActionId = 263001;
        private const int CreateMasterCommActionId = 261001;
        private const int CreateMasterBetSettingActionId = 262001;
        private const int CreateAgentPTActionId = 263002;
        private const int CreateAgentCommActionId = 261002;
        private const int CreateAgentBetSettingActionId = 262002;
        private const int CreateMemberPTActionId = 263003;
        private const int CreateMemberCommActionId = 261003;
        private const int CreateMemberBetSettingActionId = 262003;
        private const int UpdateSuperPTActionId = 511;

        private readonly int[] ActionIds = {
            CreateSuperPTActionId,
            CreateSuperCommActionId,
            CreateSuperBetSettingActionId,
            CreateMasterPTActionId,
            CreateMasterCommActionId,
            CreateMasterBetSettingActionId,
            CreateAgentPTActionId,
            CreateAgentCommActionId,
            CreateAgentBetSettingActionId,
            CreateMemberPTActionId,
            CreateMemberCommActionId,
            CreateMemberBetSettingActionId,
            //UpdateSuperPTActionId,
        };

        public bool IsCreditCustomerSettingActionId(int actionId)
            => ActionIds.Contains(actionId);

        public List<List<TrackingValue>> GenerateDiffs(
            string trackingValue,
            string trackingValue2,
            Func<string, (List<Dictionary<string, TrackingValue>> trackingParams, bool hasAttr)> generateDetailFunction)
        {
            var result = new List<List<TrackingValue>>();
            var diffs = new List<TrackingValue>();
            if (!trackingValue.Equals(trackingValue2) && !string.IsNullOrEmpty(trackingValue2))
            {
                trackingValue = trackingValue.Replace("\a", "");
                trackingValue2 = trackingValue2.Replace("\a", "");
                var (listVal, hasAttr) = generateDetailFunction(trackingValue);
                var (listVal2, hasAttr2) = generateDetailFunction(trackingValue2);

                if (listVal.Count == listVal2.Count && hasAttr == hasAttr2)
                {
                    var index = 0;
                    foreach (var elms in listVal)
                    {
                        var isDiff = false;
                        diffs = new List<TrackingValue>();
                        foreach (var elm in elms)
                        {
                            try
                            {
                                var elm2 = listVal2.ElementAt(index)[elm.Key];
                                if (!elm.Value.Val.Equals(elm2.Val, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    isDiff = true;
                                    elm2.DiffRowIndex = index + (hasAttr ? 1 : 0);
                                    elm2.P = "";
                                    diffs.Add(elm2);
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        ++index;

                        if (isDiff)
                        {
                            result.Add(diffs);
                        }
                    }
                }
            }

            return result;
        }

        public void GenerateDetail(RecordSp record, int actionId, string trackingValue, string trackingValueOld, string spName, string actionName)
        {
            this._recordSp = record;
            var detail = new TrackingLog();
            var diff = new List<List<TrackingValue>>();
            var listDiffs = new List<TrackingValue>();
            try
            {
                var xmlLoad = XElement.Parse(trackingValue);
                if (xmlLoad == null) return;

                detail.SP = spName;
                detail.Attrs = new List<TrackingElement>();
                detail.Vals = new List<TrackingNode>();
                //if (!IsCreateNewLogs(actionId))
                //{
                //    detail.Vals.Add(ParseSettingsLog(xmlLoad, GetCustLevelByActionId(actionId)));
                //}
                //else
                //{
                    if (xmlLoad.HasElements)
                    {
                        foreach (var ele in xmlLoad.Elements().Distinct())
                        {
                            if (ele.HasElements)
                            {
                                detail.Vals.Add(ParseSettingsLog(ele, GetCustLevelByActionId(actionId)));
                            }
                        }
                    }
                //}
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            diff.Add(listDiffs);
            record.Detail = detail;
            record.Diffs = diff;
        }

        private TrackingNode ParseSettingsLog(XElement ele, int custLevel)
        {
            var trackingNode = new TrackingNode();
            trackingNode.Groups = new List<TrackingGroup>();
            trackingNode.N = CacheRef.GetProductGroupName(1);
            string groupType = ele.Name.LocalName;

            TrackingGroup trackingGroup = new TrackingGroup();
            switch (groupType)
            {
                case "PT": // PT
                    trackingGroup = ParseSBPositionTakingTrackingTable(ele, custLevel);
                    break;
                case "Discount": // Comm
                    trackingGroup = ParseCommSettingLog(ele, 1, IsMember(custLevel));
                    break;
                case "BetSetting": // SB Bet Setting
                    trackingGroup = ParseTrackingTable(ele, 1, 151, DisplayType.G_GT_SportType_Bettype);
                    break;
                case "ParlayMaxPayout": // Parlay Max Payout
                    trackingGroup = ParseTrackingTable(ele, 1, 152, DisplayType.G_GT_SportType_Bettype);
                    break;
            }

            trackingNode.Groups.Add(trackingGroup);

            return trackingNode;
        }

        private TrackingGroup ParseTrackingTable(XElement groupType, int groupId, int groupTypeId, DisplayType displayType)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            trackingGroup.N = GetCreateCustomerResource($"GroupType_{groupTypeId}");
            trackingGroup.Vals = new List<TrackingElement>();
            trackingGroup.Tables = new List<TrackingTable>();
            var trackingTable = new TrackingTable();
            trackingTable.HeaderRows = new List<List<string>>();
            trackingTable.HeaderRows.Add(new List<string> { string.Empty });
            trackingTable.Rows = new List<List<string>>();
            List<string> existedSporttypeRow = new List<string>();
            int columnCount = 0;
            foreach (var item in groupType.Elements())
            {
                if (HasData(item))
                {
                    continue;
                }

                var row = new List<string>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var betType = int.Parse(item.Attribute("BetType").Value);
                var settingLevel = item.Attribute("SettingLevel") == null ? 0 : int.Parse(item.Attribute("SettingLevel").Value);
                var productId = item.Attribute("ProductID") == null ? 0 : int.Parse(item.Attribute("ProductID").Value);
                var rowLabel = string.Empty;
                if (displayType == DisplayType.SportType)
                {
                    switch (settingLevel)
                    {
                        case 1:
                            rowLabel = CacheRef.GetProductName(productId);
                            break;

                        case 2:
                            rowLabel = CacheRef.GetSportName(sportType);
                            break;

                        case 3:
                            rowLabel = CacheRef.GetBettype(betType);
                            break;

                        case 4:
                            rowLabel = "&emsp;" + CacheRef.GetBettype(betType);
                            var sportName = CacheRef.GetSportName(sportType);
                            if (!existedSporttypeRow.Contains(sportName))
                            {
                                trackingTable.Rows.Add(new List<string> { sportName });
                                existedSporttypeRow.Add(sportName);
                            }

                            break;

                        default:
                            rowLabel = CacheRef.GetSportName(sportType);
                            break;
                    }
                }
                else
                {
                    rowLabel = CacheRef.GetNameByGroupSubGroup(groupId, groupTypeId, sportType, betType);
                }

                row.Add(rowLabel);
                foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType")
                    && !i.Name.LocalName.Equals("BetType")
                    && !i.Name.LocalName.Equals("SettingLevel")
                    && !i.Name.LocalName.Equals("ProductID")))
                {
                    var headerColumnName = GetCreateCustomerResource(attr.Name.LocalName);
                    if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                    {
                        trackingTable.HeaderRows[0].Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatValue(attr));
                }

                trackingTable.Rows.Add(row);
                columnCount = Math.Max(columnCount, row.Count);
            }

            foreach (var row in trackingTable.Rows)
            {
                var gapCols = columnCount - row.Count;
                if (gapCols > 0)
                {
                    for (int i = 0; i < gapCols; i++)
                    {
                        row.Add(string.Empty);
                    }
                }
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParseSBPositionTakingTrackingTable(XElement groupType, int custLevel)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            trackingGroup.N = GetCreateCustomerResource($"GroupType_153");
            trackingGroup.Vals = new List<TrackingElement>();
            trackingGroup.Tables = trackingGroup.Tables ?? new List<TrackingTable>();
            var previousSportType = 0;

            var trackingTable = new TrackingTable();
            trackingTable.HeaderRows = new List<List<string>>();
            trackingTable.HeaderRows.Add(new List<string>());
            trackingTable.Rows = new List<List<string>>();
            foreach (var item in groupType.Elements())
            {
                if (HasData(item))
                {
                    continue;
                }

                var row = new List<string>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var sportName = CacheRef.GetSportName(sportType);
                var betType = int.Parse(item.Attribute("BetType").Value);
                var isLive = int.Parse(item.Attribute("IsLive").Value) > 0;

                previousSportType = previousSportType == 0 ? sportType : previousSportType;
                row.Add((isLive ? "Live " : string.Empty) + CacheRef.GetBettype(betType));
                if (previousSportType != 0 && !previousSportType.Equals(sportType))
                {
                    trackingGroup.Tables.Add(trackingTable);
                    trackingTable = new TrackingTable();
                    trackingTable.HeaderRows = new List<List<string>>();
                    trackingTable.HeaderRows.Add(new List<string>());
                    trackingTable.Rows = new List<List<string>>();
                    previousSportType = sportType;
                }

                if (!trackingTable.HeaderRows.Any(headerRow => headerRow.Any()))
                {
                    trackingTable.HeaderRows[0].Add(sportName);
                }

                foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType")
                    && !i.Name.LocalName.Equals("BetType")
                    && !i.Name.LocalName.Equals("IsLive")
                    && !i.Name.LocalName.Equals("SettingLevel")
                    && !i.Name.LocalName.Equals("ProductID")))
                {
                    var headerColumnName = GetCreateCustomerResourceByCustomerLevel(attr.Name.LocalName, custLevel + 1);
                    if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                    {
                        trackingTable.HeaderRows[0].Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatDecimalTrailingZeros(attr.Value));
                }

                trackingTable.Rows.Add(row);
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParsePositionTakingTrackingTable(XElement groupType, int custLevel)
        {
            TrackingGroup trackingGroup = new TrackingGroup();

            trackingGroup.Vals = new List<TrackingElement>();
            trackingGroup.Tables = new List<TrackingTable>();

            var trackingTable = new TrackingTable();
            trackingTable.HeaderRows = new List<List<string>>();
            trackingTable.HeaderRows.Add(new List<string> { string.Empty });
            trackingTable.Rows = new List<List<string>>();

            foreach (var item in groupType.Elements())
            {
                if (HasData(item))
                {
                    continue;
                }

                var row = new List<string>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var betType = int.Parse(item.Attribute("BetType").Value);
                var isLive = int.Parse(item.Attribute("IsLive").Value) > 0;
                var settingLevel = item.Attribute("SettingLevel") == null ? 0 : int.Parse(item.Attribute("SettingLevel").Value);
                var productId = item.Attribute("ProductID") == null ? 0 : int.Parse(item.Attribute("ProductID").Value);
                var labelName = settingLevel == 1 ? CacheRef.GetProductName(productId) : CacheRef.GetSportName(sportType);
                row.Add((isLive ? "Live " : string.Empty) + labelName);

                foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType")
                    && !i.Name.LocalName.Equals("BetType")
                    && !i.Name.LocalName.Equals("IsLive")
                    && !i.Name.LocalName.Equals("SettingLevel")
                    && !i.Name.LocalName.Equals("ProductID")))
                {
                    var headerColumnName = GetCreateCustomerResourceByCustomerLevel(attr.Name.LocalName, custLevel);
                    if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                    {
                        trackingTable.HeaderRows[0].Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatDecimalTrailingZeros(attr.Value));
                }

                trackingTable.Rows.Add(row);
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParseCommSettingLog(XElement groupType, int groupId, bool isMember)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            trackingGroup.N = GetCreateCustomerResource($"GroupType_150");
            trackingGroup.Vals = new List<TrackingElement>();

            if (isMember && groupId == 1)
            {
                trackingGroup.Tables = new List<TrackingTable>();
                var trackingTable = new TrackingTable();
                trackingTable.HeaderRows = new List<List<string>>();
                trackingTable.HeaderRows.Add(new List<string> { string.Empty });
                trackingTable.Rows = new List<List<string>>();
                foreach (var item in groupType.Elements())
                {
                    if (HasData(item))
                    {
                        continue;
                    }

                    var row = new List<string>();
                    var sportType = int.Parse(item.Attribute("SportType").Value);
                    var betType = int.Parse(item.Attribute("BetType").Value);
                    row.Add(groupId == 1 ?
                        CacheRef.GetSportsbookCommMemberName(1, 150, sportType, betType) :
                        CacheRef.GetNameByGroupSubGroup(1, 150, sportType, betType));
                    foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType") && !i.Name.LocalName.Equals("BetType")))
                    {
                        var headerColumnName = GetCreateCustomerResource(attr.Name.LocalName);
                        if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                        {
                            trackingTable.HeaderRows[0].Add(headerColumnName);
                        }

                        row.Add(LogFormater.FormatDecimalTrailingZeros(attr.Value));
                    }

                    trackingTable.Rows.Add(row);
                }

                trackingGroup.Tables.Add(trackingTable);
            }
            else
            {
                foreach (var item in groupType.Elements())
                {
                    if (HasData(item))
                    {
                        continue;
                    }

                    var sportType = int.Parse(item.Attribute("SportType").Value);
                    var betType = int.Parse(item.Attribute("BetType").Value);
                    trackingGroup.Vals.Add(FormatSMACommElement(item.Attribute(isMember ? "PlayerDiscount" : "Discount"), 1, 150, sportType, betType));
                }
            }

            return trackingGroup;
        }

        private TrackingElement FormatSMACommElement(
            XAttribute attr,
            int groupId,
            int groupTypeId,
            int sportTypeId,
            int betTypeId)
        {
            var ele = new TrackingElement();
            ele.N = groupId == 1 ? CacheRef.GetSportsbookCommSMAName(groupId, groupTypeId, sportTypeId, betTypeId) : CacheRef.GetSportName(sportTypeId);
            ele.Val = LogFormater.FormatDecimalTrailingZeros(attr.Value);
            return ele;
        }

        private bool IsMixSportsParlay(int sportType, int betType)
            => sportType == 99 && betType == 9;

        private string GetCustomerLevelText(int custLevel)
        {
            switch (custLevel)
            {
                case 1:
                    return "Member";

                case 2:
                    return "Agent";

                case 3:
                    return "Master";

                case 4:
                    return "Super";

                default:
                    return custLevel.ToString();
            }
        }

        private bool IsMember(int custLevel)
            => custLevel == 1;

        private string GetCreateCustomerResourceByCustomerLevel(string key, int custLevel)
            => string.Format(GetCreateCustomerResource(key), GetCustomerLevelText(custLevel));

        private string GetCreateCustomerResource(string key)
        {
            try
            {
                return HttpContext.GetGlobalResourceObject(ResourceExtension.Resources.CreateCustomer.ToString(), key).ToString();
            }
            catch (Exception)
            {
                return key;
            }
        }

        private int GetCustLevelByActionId(int actionId)
        {
            if (!ActionIds.Contains(actionId))
            {
                throw new ArgumentOutOfRangeException("actionId");
            }

            switch (actionId)
            {
                case CreateSuperPTActionId:
                case CreateSuperCommActionId:
                case CreateSuperBetSettingActionId:
                    return 4;
                case CreateMasterPTActionId:
                case CreateMasterCommActionId:
                case CreateMasterBetSettingActionId:
                    return 3;
                case CreateAgentPTActionId:
                case CreateAgentCommActionId:
                case CreateAgentBetSettingActionId:
                    return 2;
                case CreateMemberPTActionId:
                case CreateMemberCommActionId:
                case CreateMemberBetSettingActionId:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException("actionId");
            }
        }

        private bool HasData(XElement ele)
        {
            if (ele.HasElements)
            {
                foreach (var item in ele.Elements())
                {
                    if (item.HasElements)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsCreateNewLogs(int actionId)
            => actionId >= CreateSuperPTActionId;
    }
}