using Fanex.Tarus.Infrastructure.Caching;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private const int SettingsLogActionId_MasterNew = 95124;

        private const int CompactLogActionId_Master = 95089;

        private const int SettingsLogActionId_Agent = 95092;
        private const int SettingsLogActionId_AgentNew = 95125;

        private const int CompactLogActionId_Agent = 95093;

        private const int SettingsLogActionId_Member = 95095;
        private const int SettingsLogActionId_MemberNew = 95126;

        private const int SettingsLogActionId_SuperDirectMember = 264001;
        private const int SettingsLogActionId_SuperDirectMemberNew = 264006;

        private const int SettingsLogActionId_CashMember = 264003;
        private const int SettingsLogActionId_CashMemberNew = 264007;

        private const int SettingsLogActionId_RegisterCashMember = 264005;

        private const int CompactLogActionId_Member = 95096;

        private const int PTTemplateIns_Master = 475;

        private const int PTTemplateUpd_Master = 476;

        //private const int PTTemplateDel_Master = 477;

        private const int PTTemplateIns_Agent = 480;

        private const int PTTemplateUpd_Agent = 481;

        //private const int PTTemplateDel_Agent = 482;

        private const int PTTemplateIns_Member = 483;

        private const int PTTemplateUpd_Member = 484;

        //private const int PTTemplateDel_Member = 485;

        private readonly int[] ActionIds = {
            SettingsLogActionId_Master,
            SettingsLogActionId_MasterNew,
            CompactLogActionId_Master,
            SettingsLogActionId_Agent,
            SettingsLogActionId_AgentNew,
            CompactLogActionId_Agent,
            SettingsLogActionId_Member,
            SettingsLogActionId_MemberNew,
            CompactLogActionId_Member,
            SettingsLogActionId_SuperDirectMember,
            SettingsLogActionId_SuperDirectMemberNew,
            SettingsLogActionId_CashMember,
            SettingsLogActionId_CashMemberNew,
            SettingsLogActionId_RegisterCashMember
            //PTTemplateIns_Master,
            //PTTemplateUpd_Master,
            ////PTTemplateDel_Master,
            //PTTemplateIns_Agent,
            //PTTemplateUpd_Agent,
            ////PTTemplateDel_Agent,
            //PTTemplateIns_Member,
            //PTTemplateUpd_Member,
            ////PTTemplateDel_Member
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
                //if (IsPTTemplateLogActionId(actionId))
                //{
                //    detail.Vals.Add(ParsePTTemplateLog(actionName, xmlLoad, GetCustLevelByActionId(actionId)));
                //}
                //else
                //{
                if (xmlLoad.HasElements)
                {
                    foreach (var ele in xmlLoad.Elements().Distinct())
                    {
                        if (HasData(ele))
                        {
                            if (IsCompactLogActionId(actionId))
                            {
                                detail.Vals.Add(ParseSnapshootLog(ele));
                            }
                            else
                            {
                                detail.Vals.Add(ParseSettingsLog(ele, GetCustLevelByActionId(actionId)));
                            }
                        }
                    }
                    //}
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

        public bool IsPTTemplateLogActionId(int actionId)
        {
            return actionId == PTTemplateIns_Master
                || actionId == PTTemplateUpd_Master
                //|| actionId == PTTemplateDel_Master
                || actionId == PTTemplateIns_Agent
                || actionId == PTTemplateUpd_Agent
                //|| actionId == PTTemplateDel_Agent
                || actionId == PTTemplateIns_Member
                || actionId == PTTemplateUpd_Member;
            //|| actionId == PTTemplateDel_Member;
        }

        private TrackingNode ParseSnapshootLog(XElement element)
        {
            var trackingNode = new TrackingNode();
            trackingNode.Vals = new List<TrackingElement>();
            if (element.HasElements)
            {
                var groupId = int.Parse(element.Element("Id").Value);
                trackingNode.N = CacheRef.GetProductGroupName(groupId);
                var groupTypesEles = element.Elements("GroupTypes");
                trackingNode.Groups = new List<TrackingGroup>();
                foreach (var groupType in groupTypesEles)
                {
                    if (groupType.HasElements)
                    {
                        var groupTypeId = int.Parse(groupType.Element("Id").Value);
                        var groupTypeName = GetCreateCustomerResource($"GroupType_{groupTypeId}");
                        var trackingGroup = new TrackingGroup();
                        trackingGroup.N = groupTypeName;
                        trackingGroup.Vals = new List<TrackingElement>();
                        trackingGroup.Tables = new List<TrackingTable>();
                        var subGroupsEles = groupType.Elements("SubGroups");
                        if (groupTypeId == 156)
                        {
                            foreach (var subGroupEle in subGroupsEles)
                            {
                                trackingGroup.Vals.Add(new TrackingElement()
                                {
                                    N = CacheRef.GetSubGroupName(int.Parse(subGroupEle.Element("Id").Value)),
                                    Val = subGroupEle.Element("Attributes").Element("Value").Value
                                });
                            }
                        }
                        else
                        {
                            var trackingTable = new TrackingTable();
                            trackingTable.HeaderRows = new List<List<string>>();
                            trackingTable.Rows = new List<List<TrackingValue>>();
                            foreach (var subGroupEle in subGroupsEles)
                            {
                                var row = new List<TrackingValue>();
                                foreach (var subGroupItem in subGroupEle.Elements())
                                {
                                    if (subGroupItem.Name.LocalName == "Id") // Row's label
                                    {
                                        row.Add(new TrackingValue { Val = CacheRef.GetSubGroupName(int.Parse(subGroupItem.Value)) });
                                        trackingTable.HeaderRows.Add(new List<string> { string.Empty });
                                    }
                                    else if (subGroupItem.Name.LocalName == "Attributes") // Row's values
                                    {
                                        var key = GetCreateCustomerResource(subGroupItem.Element("Key").Value);
                                        var value = subGroupItem.Element("Value").Value;
                                        row.Add(new TrackingValue { Val = value });
                                        if (!trackingTable.HeaderRows[0].Contains(key))
                                        {
                                            trackingTable.HeaderRows[0].Add(key);
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
                    case 158: // Stake Levels
                    case 159: // Group Stake Levels
                    case 161: // Win Limit
                    case 162: // Potential Win Limit
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
            trackingTable.HeaderRows = new List<List<string>>();
            trackingTable.HeaderRows.Add(new List<string> { string.Empty });
            trackingTable.Rows = new List<List<TrackingValue>>();
            List<string> existedSporttypeRow = new List<string>();
            int columnCount = 0;
            foreach (var item in groupType.Elements())
            {
                if (HasData(item))
                {
                    continue;
                }

                var row = new List<TrackingValue>();
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
                                trackingTable.Rows.Add(new List<TrackingValue> { new TrackingValue { Val = sportName } });
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

                row.Add(new TrackingValue { Val = rowLabel });
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

                    row.Add(LogFormater.FormatTrackingValue(attr));
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
                        row.Add(new TrackingValue { Val = string.Empty });
                    }
                }
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
            trackingTable.HeaderRows = new List<List<string>>();
            trackingTable.HeaderRows.Add(new List<string>());
            trackingTable.Rows = new List<List<TrackingValue>>();

            var mixSportParlayTrackingTable = new TrackingTable();
            mixSportParlayTrackingTable.HeaderRows = new List<List<string>>();
            mixSportParlayTrackingTable.HeaderRows.Add(new List<string>());
            mixSportParlayTrackingTable.Rows = new List<List<TrackingValue>>();

            foreach (var item in groupType.Elements())
            {
                if (HasData(item))
                {
                    continue;
                }

                var row = new List<TrackingValue>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var sportName = CacheRef.GetSportName(sportType);
                var betType = int.Parse(item.Attribute("BetType").Value);
                var isLive = int.Parse(item.Attribute("IsLive").Value) > 0;
                var settingLevel = item.Attribute("SettingLevel") == null ? 0 : int.Parse(item.Attribute("SettingLevel").Value);
                var productId = item.Attribute("ProductID") == null ? 0 : int.Parse(item.Attribute("ProductID").Value);
                if (IsMixSportsParlay(sportType, betType))
                {
                    var mixSportParlayRow = new List<TrackingValue>();
                    mixSportParlayRow.Add(new TrackingValue { Val = (isLive ? "Live " : string.Empty) + CacheRef.GetBettype(betType) });
                    mixSportParlayTrackingTable.HeaderRows = new List<List<string>>(trackingTable.HeaderRows);
                    mixSportParlayTrackingTable.HeaderRows[0][0] = CacheRef.GetNameByGroupSubGroup(groupId, groupTypeId, sportType, betType);
                    foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType")
                        && !i.Name.LocalName.Equals("BetType")
                        && !i.Name.LocalName.Equals("IsLive")
                        && !i.Name.LocalName.Equals("SettingLevel")
                        && !i.Name.LocalName.Equals("ProductID")))
                    {
                        mixSportParlayRow.Add(new TrackingValue { Val = LogFormater.FormatDecimalTrailingZeros(attr.Value) });
                    }

                    mixSportParlayTrackingTable.Rows.Add(mixSportParlayRow);
                }
                else
                {
                    previousSportType = previousSportType == 0 ? sportType : previousSportType;
                    row.Add(new TrackingValue { Val = (isLive ? "Live " : string.Empty) + CacheRef.GetBettype(betType) });
                    if (previousSportType != 0 && !previousSportType.Equals(sportType))
                    {
                        trackingGroup.Tables.Add(trackingTable);
                        trackingTable = new TrackingTable();
                        trackingTable.HeaderRows = new List<List<string>>();
                        trackingTable.HeaderRows.Add(new List<string>());
                        trackingTable.Rows = new List<List<TrackingValue>>();
                        previousSportType = sportType;
                        if (mixSportParlayTrackingTable != null && mixSportParlayTrackingTable.Rows.Any())
                        {
                            trackingGroup.Tables.Add(mixSportParlayTrackingTable);
                            mixSportParlayTrackingTable = null;
                        }
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

                        row.Add(LogFormater.FormatTrackingValue(attr));
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
            trackingTable.HeaderRows = new List<List<string>>();
            trackingTable.HeaderRows.Add(new List<string> { string.Empty });
            trackingTable.Rows = new List<List<TrackingValue>>();

            foreach (var item in groupType.Elements())
            {
                if (HasData(item))
                {
                    continue;
                }

                var row = new List<TrackingValue>();
                var sportType = int.Parse(item.Attribute("SportType").Value);
                var betType = int.Parse(item.Attribute("BetType").Value);
                var isLive = int.Parse(item.Attribute("IsLive").Value) > 0;
                var settingLevel = item.Attribute("SettingLevel") == null ? 0 : int.Parse(item.Attribute("SettingLevel").Value);
                var productId = item.Attribute("ProductID") == null ? 0 : int.Parse(item.Attribute("ProductID").Value);
                var labelName = settingLevel == 1 ? CacheRef.GetProductName(productId) : CacheRef.GetSportName(sportType);
                row.Add(new TrackingValue { Val = (isLive ? "Live " : string.Empty) + labelName });

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

                    row.Add(LogFormater.FormatTrackingValue(attr));
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

            if (isMember && groupId == 1)
            {
                trackingGroup.Tables = new List<TrackingTable>();
                var trackingTable = new TrackingTable();
                trackingTable.HeaderRows = new List<List<string>>();
                trackingTable.HeaderRows.Add(new List<string> { string.Empty });
                trackingTable.Rows = new List<List<TrackingValue>>();
                foreach (var item in groupType.Elements())
                {
                    if (HasData(item))
                    {
                        continue;
                    }

                    var row = new List<TrackingValue>();
                    var sportType = int.Parse(item.Attribute("SportType").Value);
                    var betType = int.Parse(item.Attribute("BetType").Value);
                    row.Add(new TrackingValue
                    {
                        Val = groupId == 1 ?
                        CacheRef.GetSportsbookCommMemberName(groupId, groupTypeId, sportType, betType) :
                        CacheRef.GetNameByGroupSubGroup(groupId, groupTypeId, sportType, betType)
                    });
                    foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType") && !i.Name.LocalName.Equals("BetType")))
                    {
                        var headerColumnName = GetCreateCustomerResource(attr.Name.LocalName);
                        if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                        {
                            trackingTable.HeaderRows[0].Add(headerColumnName);
                        }

                        row.Add(LogFormater.FormatTrackingValue(attr));
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
                    trackingGroup.Vals.Add(FormatSMACommElement(item.Attribute(isMember ? "PlayerDiscount" : "Discount"), groupId, groupTypeId, sportType, betType));
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
                case SettingsLogActionId_MasterNew:
                case CompactLogActionId_Master:
                case PTTemplateIns_Master:
                case PTTemplateUpd_Master:
                    //case PTTemplateDel_Master:
                    return 3;

                case SettingsLogActionId_Agent:
                case SettingsLogActionId_AgentNew:
                case CompactLogActionId_Agent:
                case PTTemplateIns_Agent:
                case PTTemplateUpd_Agent:
                    //case PTTemplateDel_Agent:
                    return 2;

                case SettingsLogActionId_Member:
                case SettingsLogActionId_MemberNew:
                case SettingsLogActionId_SuperDirectMember:
                case SettingsLogActionId_SuperDirectMemberNew:
                case SettingsLogActionId_CashMember:
                case SettingsLogActionId_CashMemberNew:
                case SettingsLogActionId_RegisterCashMember:
                case CompactLogActionId_Member:
                case PTTemplateIns_Member:
                case PTTemplateUpd_Member:
                    //case PTTemplateDel_Member:
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
    }
}