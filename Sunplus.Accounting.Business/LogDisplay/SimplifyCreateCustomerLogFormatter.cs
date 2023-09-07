using Fanex.Tarus.Infrastructure.Caching;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Sunplus.Accounting.Business.LogDisplay
{
    public class SimplifyCreateCustomerLogFormatter
    {
        private enum DisplayType
        {
            Product = 1,
            SportType = 2,
            Bettype = 3,
            SportTypeBettype = 4,
            G_GT_SportType_Bettype = 5,
            SB_Com = 6
        }

        private const int SettingsLogActionId_Master = 95088;

        private const int CompactLogActionId_Master = 95089;

        private const int SettingsLogActionId_Agent = 95092;

        private const int CompactLogActionId_Agent = 95093;

        private const int SettingsLogActionId_Member = 95095;

        private const int CompactLogActionId_Member = 95096;

        private const int PTTemplateIns_Master = 475;

        private const int PTTemplateUpd_Master = 476;

        private const int PTTemplateDel_Master = 477;

        private const int PTTemplateIns_Agent = 480;

        private const int PTTemplateUpd_Agent = 481;

        private const int PTTemplateDel_Agent = 482;

        private const int PTTemplateIns_Member = 483;

        private const int PTTemplateUpd_Member = 484;

        private const int PTTemplateDel_Member = 485;

        private const int SabaCasinoProductId = 6;

        private readonly int[] ActionIds = {
            SettingsLogActionId_Master,
            CompactLogActionId_Master,
            SettingsLogActionId_Agent,
            CompactLogActionId_Agent,
            SettingsLogActionId_Member,
            CompactLogActionId_Member,
            PTTemplateIns_Master,
            PTTemplateUpd_Master,
            PTTemplateDel_Master,
            PTTemplateIns_Agent,
            PTTemplateUpd_Agent,
            PTTemplateDel_Agent,
            PTTemplateIns_Member,
            PTTemplateUpd_Member,
            PTTemplateDel_Member
        };

        public bool IsSimplifyCreateCustomerActionId(int actionId)
            => ActionIds.Contains(actionId);

        public TrackingLog GenerateDetail(int actionId, string trackingValue, string spName, string actionName)
        {
            var detail = new TrackingLog();
            try
            {
                var xmlLoad = XElement.Parse(trackingValue);
                if (xmlLoad == null) return detail;

                detail.SP = spName;
                detail.Attrs = new List<TrackingElement>();
                detail.Vals = new List<TrackingNode>();
                var isMember = actionId == CompactLogActionId_Member || actionId == SettingsLogActionId_Member;
                if (xmlLoad.HasElements)
                {
                    foreach (var ele in xmlLoad.Elements().Distinct())
                    {
                        if (IsCompactLogActionId(actionId))
                        {
                            detail.Vals.Add(ParseCompactLog(ele));
                        }
                        else
                        {
                            detail.Vals.Add(ParseSettingsLog(ele, GetCustLevelByActionId(actionId)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }

            return detail;
        }

        private static bool IsCompactLogActionId(int actionId)
        {
            return actionId == CompactLogActionId_Master
                || actionId == CompactLogActionId_Agent
                || actionId == CompactLogActionId_Member;
        }

        private static bool IsPTTemplateLogActionId(int actionId)
        {
            return actionId == PTTemplateIns_Master
                || actionId == PTTemplateUpd_Master
                || actionId == PTTemplateDel_Master
                || actionId == PTTemplateIns_Agent
                || actionId == PTTemplateUpd_Agent
                || actionId == PTTemplateDel_Agent
                || actionId == PTTemplateIns_Member
                || actionId == PTTemplateUpd_Member
                || actionId == PTTemplateDel_Member;
        }

        private TrackingNode ParsePTTemplateLog(XElement element, int custLevel)
        {
            var trackingNode = new TrackingNode();

            return trackingNode;
        }

        private TrackingNode ParseCompactLog(XElement element)
        {
            var trackingNode = new TrackingNode();
            trackingNode.Vals = new List<TrackingElement>();
            if (element.HasElements)
            {
                var groupName = element.Element("Name");
                trackingNode.N = groupName.Value;
                var groupTypesEles = element.Elements("GroupTypes");
                trackingNode.Groups = new List<TrackingGroup>();
                foreach (var groupType in groupTypesEles)
                {
                    if (groupType.HasElements)
                    {
                        var groupTypeId = int.Parse(groupType.Element("Id").Value);
                        var groupTypeName = groupType.Element("Name");
                        var trackingGroup = new TrackingGroup();
                        trackingGroup.N = groupTypeName.Value;
                        trackingGroup.Vals = new List<TrackingElement>();
                        trackingGroup.Tables = new List<TrackingTable>();
                        var subGroupsEles = groupType.Elements("SubGroups");
                        if (groupTypeId == 156)
                        {
                            foreach (var subGroupEle in subGroupsEles)
                            {
                                trackingGroup.Vals.Add(new TrackingElement()
                                {
                                    N = subGroupEle.Element("Name").Value,
                                    Val = subGroupEle.Element("Attributes").Element("Value").Value
                                });
                            }
                        }
                        else
                        {
                            var trackingTable = new TrackingTable();
                            trackingTable.HeaderColumns = new List<string>();
                            trackingTable.Rows = new List<List<string>>();
                            foreach (var subGroupEle in subGroupsEles)
                            {
                                var row = new List<string>();
                                foreach (var subGroupItem in subGroupEle.Elements())
                                {
                                    if (subGroupItem.Name.LocalName == "Name") // Row's label
                                    {
                                        row.Add(subGroupItem.Value);
                                        trackingTable.HeaderColumns.Add(string.Empty);
                                    }
                                    else if (subGroupItem.Name.LocalName == "Attributes") // Row's values
                                    {
                                        var key = GetCreateCustomerResource(subGroupItem.Element("Key").Value);
                                        var value = subGroupItem.Element("Value").Value;
                                        row.Add(value);
                                        if (!trackingTable.HeaderColumns.Contains(key))
                                        {
                                            trackingTable.HeaderColumns.Add(key);
                                        }
                                    }
                                }

                                trackingTable.Rows.Add(row);
                            }

                            trackingGroup.Tables.Add(trackingTable);
                        }

                        trackingNode.Groups.Add(trackingGroup);
                    }
                }
            }

            return trackingNode;
        }

        private TrackingNode ParseSettingsLog(XElement productGroup, int custLevel)
        {
            var groupId = int.Parse(productGroup.Attribute("ID").Value);
            var trackingNode = new TrackingNode();
            trackingNode.Vals = new List<TrackingElement>();
            trackingNode.N = CacheRef.GetProductGroupName(groupId);
            trackingNode.Groups = new List<TrackingGroup>();
            var groupProductGroup = from groupType in productGroup.Elements()
                                    group groupType by groupType.Attribute("GroupType").Value;
            foreach (var groupType in groupProductGroup)
            {
                var groupTypeId = int.Parse(groupType.Key);
                TrackingGroup trackingGroup = new TrackingGroup();
                switch (groupTypeId)
                {
                    case 150: // SB Comm
                    case 156: // Comm
                        trackingGroup = ParseCommSettingLog(groupType, groupId, IsMember(custLevel));
                        break;
                    case 155: // Win Loss Limit
                        trackingGroup = ParseTrackingTable(groupType, groupId, DisplayType.SportType);
                        break;
                    case 151: // SB Bet Setting
                    case 152: // Parlay Max Payout
                    case 154: // Bet Setting
                    case 157: // Bet Setting
                        trackingGroup = ParseTrackingTable(groupType, groupId, DisplayType.G_GT_SportType_Bettype);
                        break;
                    case 153: // PT
                        trackingGroup = groupId == 1 ?
                            ParseSBPositionTakingTrackingTable(groupType, groupId, custLevel) :
                            ParsePositionTakingTrackingTable(groupType, custLevel);
                        break;
                }

                trackingNode.Groups.Add(trackingGroup);
            }

            return trackingNode;
        }

        private TrackingGroup ParseTrackingTable(IGrouping<string, XElement> groupType, int groupId, DisplayType displayType)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            var groupTypeId = int.Parse(groupType.Key);
            trackingGroup.N = GetCreateCustomerResource($"GroupType_{groupTypeId}");
            trackingGroup.Vals = new List<TrackingElement>();
            trackingGroup.Tables = new List<TrackingTable>();
            var trackingTable = new TrackingTable();
            trackingTable.HeaderColumns = new List<string>();
            trackingTable.HeaderColumns.Add(string.Empty);
            trackingTable.Rows = new List<List<string>>();
            foreach (var item in groupType.Elements())
            {
                var row = new List<string>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var betType = int.Parse(item.Attribute("BetType").Value);
                if (displayType == DisplayType.SportType)
                {
                    row.Add(IsSabaCasino(sportType, betType) ? CacheRef.GetProductName(SabaCasinoProductId) : CacheRef.GetSportName(sportType));
                }
                else
                {
                    row.Add(CacheRef.GetNameByGroupSubGroup(groupId, groupTypeId, sportType, betType));
                }

                foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType") && !i.Name.LocalName.Equals("BetType")))
                {
                    var headerColumnName = GetCreateCustomerResource(attr.Name.LocalName);
                    if (!trackingTable.HeaderColumns.Contains(headerColumnName))
                    {
                        trackingTable.HeaderColumns.Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatValue(attr));
                }

                trackingTable.Rows.Add(row);
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParseSBPositionTakingTrackingTable(IGrouping<string, XElement> groupType, int groupId, int custLevel, TrackingGroup concatTrackingGroup = null)
        {
            TrackingGroup trackingGroup = concatTrackingGroup ?? new TrackingGroup();
            var groupTypeId = int.Parse(groupType.Key);
            trackingGroup.N = GetCreateCustomerResource($"GroupType_{groupTypeId}");
            trackingGroup.Vals = new List<TrackingElement>();
            trackingGroup.Tables = trackingGroup.Tables ?? new List<TrackingTable>();
            var previousSportType = 0;

            var trackingTable = new TrackingTable();
            trackingTable.HeaderColumns = new List<string>();
            trackingTable.Rows = new List<List<string>>();

            var mixSportParlayTrackingTable = new TrackingTable();
            mixSportParlayTrackingTable.HeaderColumns = new List<string>();
            mixSportParlayTrackingTable.Rows = new List<List<string>>();

            foreach (var item in groupType.Elements())
            {
                var row = new List<string>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var sportName = CacheRef.GetSportName(sportType);
                var betType = int.Parse(item.Attribute("BetType").Value);
                var isLive = int.Parse(item.Attribute("IsLive").Value) > 0;
                if (IsMixSportsParlay(sportType, betType))
                {
                    var mixSportParlayRow = new List<string>();
                    mixSportParlayRow.Add((isLive ? "Live " : string.Empty) + CacheRef.GetBettype(betType));
                    mixSportParlayTrackingTable.HeaderColumns = trackingTable.HeaderColumns.Select(col => col).ToList();
                    mixSportParlayTrackingTable.HeaderColumns[0] = CacheRef.GetNameByGroupSubGroup(groupId, groupTypeId, sportType, betType);
                    foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType") && !i.Name.LocalName.Equals("BetType") && !i.Name.LocalName.Equals("IsLive")))
                    {
                        mixSportParlayRow.Add(LogFormater.FormatDecimalTrailingZeros(attr.Value));
                    }

                    mixSportParlayTrackingTable.Rows.Add(mixSportParlayRow);
                }
                else
                {
                    previousSportType = previousSportType == 0 ? sportType : previousSportType;
                    row.Add((isLive ? "Live " : string.Empty) + CacheRef.GetBettype(betType));
                    if (previousSportType != 0 && !previousSportType.Equals(sportType))
                    {
                        trackingGroup.Tables.Add(trackingTable);
                        trackingTable = new TrackingTable();
                        trackingTable.HeaderColumns = new List<string>();
                        trackingTable.Rows = new List<List<string>>();
                        previousSportType = sportType;
                        if (mixSportParlayTrackingTable != null && mixSportParlayTrackingTable.Rows.Any())
                        {
                            trackingGroup.Tables.Add(mixSportParlayTrackingTable);
                            mixSportParlayTrackingTable = null;
                        }
                    }

                    if (!trackingTable.HeaderColumns.Any())
                    {
                        trackingTable.HeaderColumns.Add(sportName);
                    }

                    foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType") && !i.Name.LocalName.Equals("BetType") && !i.Name.LocalName.Equals("IsLive")))
                    {
                        var headerColumnName = GetCreateCustomerResourceByCustomerLevel(attr.Name.LocalName, custLevel + 1);
                        if (!trackingTable.HeaderColumns.Contains(headerColumnName))
                        {
                            trackingTable.HeaderColumns.Add(headerColumnName);
                        }

                        row.Add(LogFormater.FormatDecimalTrailingZeros(attr.Value));
                    }

                    trackingTable.Rows.Add(row);
                }
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParsePositionTakingTrackingTable(IGrouping<string, XElement> groupType, int custLevel)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            var groupTypeId = int.Parse(groupType.Key);
            trackingGroup.N = GetCreateCustomerResource($"GroupType_{groupTypeId}");
            trackingGroup.Vals = new List<TrackingElement>();
            trackingGroup.Tables = new List<TrackingTable>();

            var trackingTable = new TrackingTable();
            trackingTable.HeaderColumns = new List<string>();
            trackingTable.HeaderColumns.Add(string.Empty);
            trackingTable.Rows = new List<List<string>>();

            foreach (var item in groupType.Elements())
            {
                var row = new List<string>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var betType = int.Parse(item.Attribute("BetType").Value);
                var sportName = IsSabaCasino(sportType, betType) ? CacheRef.GetProductName(SabaCasinoProductId) : CacheRef.GetSportName(sportType);
                var isLive = int.Parse(item.Attribute("IsLive").Value) > 0;
                row.Add((isLive ? "Live " : string.Empty) + sportName);

                foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType") && !i.Name.LocalName.Equals("BetType") && !i.Name.LocalName.Equals("IsLive")))
                {
                    var headerColumnName = GetCreateCustomerResourceByCustomerLevel(attr.Name.LocalName, custLevel + 1);
                    if (!trackingTable.HeaderColumns.Contains(headerColumnName))
                    {
                        trackingTable.HeaderColumns.Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatDecimalTrailingZeros(attr.Value));
                }

                trackingTable.Rows.Add(row);
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParseCommSettingLog(IGrouping<string, XElement> groupType, int groupId, bool isMember)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            var groupTypeId = int.Parse(groupType.Key);
            trackingGroup.N = GetCreateCustomerResource($"GroupType_{groupTypeId}");
            trackingGroup.Vals = new List<TrackingElement>();

            if (isMember)
            {
                trackingGroup.Tables = new List<TrackingTable>();
                var trackingTable = new TrackingTable();
                trackingTable.HeaderColumns = new List<string>();
                trackingTable.HeaderColumns.Add(string.Empty);
                trackingTable.Rows = new List<List<string>>();
                foreach (var item in groupType.Elements())
                {
                    var row = new List<string>();
                    var sportType = int.Parse(item.Attribute("SportType").Value);
                    var betType = int.Parse(item.Attribute("BetType").Value);
                    row.Add(groupId == 1 ?
                        CacheRef.GetSportsbookCommMemberName(groupId, groupTypeId, sportType, betType) :
                        CacheRef.GetNameByGroupSubGroup(groupId, groupTypeId, sportType, betType));
                    foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType") && !i.Name.LocalName.Equals("BetType")))
                    {
                        var headerColumnName = GetCreateCustomerResource(attr.Name.LocalName);
                        if (!trackingTable.HeaderColumns.Contains(headerColumnName))
                        {
                            trackingTable.HeaderColumns.Add(headerColumnName);
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
                    var sportType = int.Parse(item.Attribute("SportType").Value);
                    var betType = int.Parse(item.Attribute("BetType").Value);
                    trackingGroup.Vals.Add(FormatSMACommElement(item.Attribute("Discount"), groupId, groupTypeId, sportType, betType));
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
                case SettingsLogActionId_Master:
                case CompactLogActionId_Master:
                    return 3;
                case SettingsLogActionId_Agent:
                case CompactLogActionId_Agent:
                    return 2;
                case SettingsLogActionId_Member:
                case CompactLogActionId_Member:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException("actionId");
            }
        }

        private bool IsSabaCasino(int sportType, int betType)
            => sportType == 208 && betType == 2101;
    }
}
