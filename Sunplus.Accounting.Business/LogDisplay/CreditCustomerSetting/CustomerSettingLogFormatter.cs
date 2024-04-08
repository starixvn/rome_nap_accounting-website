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
        private const int UpdateSuperPTOthersActionId = 523;
        private const int UpdateSuperMinPTActionId = 520;
        private const int UpdateMasterPTActionId = 512;
        private const int UpdateMasterMinPTActionId = 521;
        private const int UpdateAgentPTActionId = 517;
        private const int UpdateAgentMinPTActionId = 522;
        private const int UpdateAgentPTOnMemberActionId = 516;
        private const int UpdateMasterPTOnMemberActionId = 519;
        private const int UpdateSuperPTOnMemberActionId = 518;
        private const int UpdateAgentPTOnGroupedMemberActionId = 513;
        private const int UpdateMasterPTOnGroupedMemberActionId = 514;
        private const int UpdateSuperPTOnGroupedMemberActionId = 515;

        private const int UpdateSuperParlayMaxPayoutActionId = 220000;
        private const int UpdateSuperIndirectParlayMaxPayoutActionId = 220001;
        private const int UpdateMasterParlayMaxPayoutActionId = 220002;
        private const int UpdateMasterIndirectParlayMaxPayoutActionId = 220003;
        private const int UpdateAgentParlayMaxPayoutActionId = 220004;
        private const int UpdateAgentIndirectParlayMaxPayoutActionId = 220005;
        private const int UpdateMemberParlayMaxPayoutActionId = 220006;

        private const int UpdateSuperSBCommActionId = 251;
        private const int UpdateMasterSBCommActionId = 252;
        private const int UpdateAgentSBCommActionId = 253;
        private const int UpdateMemberSBCommActionId = 254;
        private const int UpdateOthersCommActionId = 255;


        private const int UpdateSuperSBBetSettingActionId = 220037;
        private const int UpdateSuperIndirectSBBetSettingActionId = 220038;
        private const int UpdateMasterSBBetSettingActionId = 220039;
        private const int UpdateMasterIndirectSBBetSettingActionId = 220040;
        private const int UpdateAgentSBBetSettingActionId = 220041;
        private const int UpdateAgentIndirectSBBetSettingActionId = 220042;
        private const int UpdateMemberSBBetSettingActionId = 220043;

        private const int UpdateStakeLevel158ActionId = 220050;
        private const int UpdateWinLossLimit155ActionId = 852;

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
            UpdateSuperPTActionId,
            UpdateSuperPTOthersActionId,
            UpdateSuperMinPTActionId,
            UpdateMasterPTActionId,
            UpdateMasterMinPTActionId,
            UpdateAgentPTActionId,
            UpdateAgentMinPTActionId,
            UpdateAgentPTOnMemberActionId,
            UpdateMasterPTOnMemberActionId,
            UpdateSuperPTOnMemberActionId,
            UpdateAgentPTOnGroupedMemberActionId,
            UpdateMasterPTOnGroupedMemberActionId,
            UpdateSuperPTOnGroupedMemberActionId,
            UpdateSuperParlayMaxPayoutActionId,
            UpdateSuperIndirectParlayMaxPayoutActionId,
            UpdateMasterParlayMaxPayoutActionId,
            UpdateMasterIndirectParlayMaxPayoutActionId,
            UpdateAgentParlayMaxPayoutActionId,
            UpdateAgentIndirectParlayMaxPayoutActionId,
            UpdateMemberParlayMaxPayoutActionId,
            UpdateSuperSBCommActionId,
            UpdateMasterSBCommActionId,
            UpdateAgentSBCommActionId,
            UpdateMemberSBCommActionId,
            UpdateOthersCommActionId,
            UpdateSuperSBBetSettingActionId,
            UpdateSuperIndirectSBBetSettingActionId,
            UpdateMasterSBBetSettingActionId,
            UpdateMasterIndirectSBBetSettingActionId,
            UpdateAgentSBBetSettingActionId,
            UpdateAgentIndirectSBBetSettingActionId,
            UpdateMemberSBBetSettingActionId,
            UpdateStakeLevel158ActionId,
            UpdateWinLossLimit155ActionId,
        };

        public bool IsCreditCustomerSettingActionId(int actionId)
            => ActionIds.Contains(actionId);

        public void GenerateDetail(RecordSp record, int actionId, string trackingValue, string trackingValueOld, string spName, string actionName)
        {
            this._recordSp = record;
            var detail = new TrackingLog();
            try
            {
                var xmlLoad = XElement.Parse(trackingValue);
                if (xmlLoad == null) return;

                detail.SP = spName;
                detail.Attrs = new List<TrackingElement>();
                detail.Vals = new List<TrackingNode>();
                if (IsCreateNewLogs(actionId))
                {
                    if (xmlLoad.HasElements)
                    {
                        foreach (var ele in xmlLoad.Elements().Distinct())
                        {
                            if (ele.HasElements)
                            {
                                detail.Vals.Add(ParseCreateNewSettingsLog(ele, GetCustLevelByActionId(actionId)));
                            }
                        }
                    }
                }
                else
                {
                    detail.Vals.Add(ParseUpdateSettingsLog(xmlLoad, actionId, GetCustLevelByActionId(actionId)));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            record.Detail = detail;
        }

        private TrackingNode ParseUpdateSettingsLog(XElement ele, int actionId, int custLevel)
        {
            var logAction = GetLogAction(actionId);
            var trackingNode = new TrackingNode();
            trackingNode.Groups = new List<TrackingGroup>();
            TrackingGroup trackingGroup = new TrackingGroup();
            int groupId = 1;
            switch (logAction)
            {
                case LogAction.SBPT: // PT
                    trackingGroup = ParseSBPositionTakingTrackingTable(ele, custLevel);
                    break;
                case LogAction.PT: // PT Others
                    trackingGroup = ParsePositionTakingTrackingTable(ele, custLevel);
                    break;
                case LogAction.SBComm: // Comm
                    trackingGroup = ParseSBCommSettingLog(ele, IsMember(custLevel));
                    break;
                case LogAction.Comm: // Comm Others
                    trackingGroup = ParseCommSettingLog(ele, out groupId);
                    break;
                case LogAction.BetSetting: // SB Bet Setting
                    trackingGroup = ParseTrackingTable(ele, 151, DisplayType.G_GT_SportType_Bettype, out groupId);
                    break;
                case LogAction.ParlayMaxPayout: // Parlay Max Payout
                    trackingGroup = ParseTrackingTable(ele, 152, DisplayType.G_GT_SportType_Bettype, out groupId);
                    break;
                case LogAction.StakeLevel: // Stake Level
                    trackingGroup = ParseTrackingTable(ele, 158, DisplayType.SportType, out groupId);
                    break;
                case LogAction.WinlossLimit: // Stake Level
                    trackingGroup = ParseTrackingTable(ele, 155, DisplayType.SportType, out groupId);
                    break;
            }

            trackingNode.Groups.Add(trackingGroup);
            trackingNode.N = CacheRef.GetProductGroupName(groupId);

            return trackingNode;
        }

        private TrackingNode ParseCreateNewSettingsLog(XElement ele, int custLevel)
        {
            var trackingNode = new TrackingNode();
            trackingNode.Groups = new List<TrackingGroup>();
            string groupType = ele.Name.LocalName;
            int groupId = 1;
            TrackingGroup trackingGroup = new TrackingGroup();
            switch (groupType)
            {
                case "PT": // PT
                    trackingGroup = ParseSBPositionTakingTrackingTable(ele, custLevel);
                    break;
                case "Discount": // Comm
                    trackingGroup = ParseSBCommSettingLog(ele, IsMember(custLevel));
                    break;
                case "BetSetting": // SB Bet Setting
                    trackingGroup = ParseTrackingTable(ele, 151, DisplayType.G_GT_SportType_Bettype, out groupId);
                    break;
                case "ParlayMaxPayout": // Parlay Max Payout
                    trackingGroup = ParseTrackingTable(ele, 152, DisplayType.G_GT_SportType_Bettype, out groupId);
                    break;
            }

            trackingNode.Groups.Add(trackingGroup);
            trackingNode.N = CacheRef.GetProductGroupName(groupId);
            return trackingNode;
        }

        private TrackingGroup ParseTrackingTable(XElement groupType, int groupTypeId, DisplayType displayType, out int groupId)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            trackingGroup.N = GetCreateCustomerResource($"GroupType_{groupTypeId}");
            trackingGroup.Vals = new List<TrackingElement>();
            trackingGroup.Tables = new List<TrackingTable>();
            var trackingTable = new TrackingTable();
            trackingTable.HeaderRows = new List<List<string>>();
            trackingTable.HeaderRows.Add(new List<string> { string.Empty });
            trackingTable.Rows = new List<List<TrackingValue>>();
            List<string> existedSporttypeRow = new List<string>();
            groupId = 1;
            int columnCount = 0;
            if (groupType.HasAttributes)
            {
                foreach (var attr in groupType.Attributes())
                {
                    trackingGroup.Vals.Add(LogFormater.FormatElement(attr));
                }
            }

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
                groupId = item.Attribute("GroupID") == null ? 1 : int.Parse(item.Attribute("GroupID").Value);
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
                                trackingTable.Rows.Add(new List<TrackingValue> { new TrackingValue { Val = "sportName" } });
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
                    && !i.Name.LocalName.Equals("ProductID")
                    && !i.Name.LocalName.Equals("GroupID")))
                {
                    var headerColumnName = GetCreateCustomerResource(attr.Name.LocalName);
                    if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                    {
                        trackingTable.HeaderRows[0].Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatTrackingValue(attr, $"simplify-{sportType}-{betType}"));
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
            trackingTable.Rows = new List<List<TrackingValue>>();
            if (groupType.HasAttributes)
            {
                foreach (var attr in groupType.Attributes())
                {
                    trackingGroup.Vals.Add(LogFormater.FormatElement(attr));
                }
            }

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
                }

                if (!trackingTable.HeaderRows.Any(headerRow => headerRow.Any()))
                {
                    trackingTable.HeaderRows[0].Add(sportName);
                }

                foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType")
                    && !i.Name.LocalName.Equals("BetType")
                    && !i.Name.LocalName.Equals("IsLive")
                    && !i.Name.LocalName.Equals("SettingLevel")
                    && !i.Name.LocalName.Equals("ProductID")
                    && !i.Name.LocalName.Equals("GroupID")))
                {
                    var headerColumnName = GetCreateCustomerResourceByCustomerLevel(attr.Name.LocalName, custLevel);
                    if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                    {
                        trackingTable.HeaderRows[0].Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatTrackingValue(attr, $"simplify-{sportType}-{betType}"));
                }

                trackingTable.Rows.Add(row);
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParsePositionTakingTrackingTable(XElement groupType, int custLevel)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            trackingGroup.N = GetCreateCustomerResource($"GroupType_153");
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
                    && !i.Name.LocalName.Equals("ProductID")
                    && !i.Name.LocalName.Equals("GroupID")))
                {
                    var headerColumnName = GetCreateCustomerResourceByCustomerLevel(attr.Name.LocalName, custLevel);
                    if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                    {
                        trackingTable.HeaderRows[0].Add(headerColumnName);
                    }

                    row.Add(LogFormater.FormatTrackingValue(attr, $"simplify-{sportType}-{betType}"));
                }

                trackingTable.Rows.Add(row);
            }

            trackingGroup.Tables.Add(trackingTable);
            return trackingGroup;
        }

        private TrackingGroup ParseCommSettingLog(XElement groupType, out int groupId)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            trackingGroup.N = GetCreateCustomerResource($"GroupType_150");
            trackingGroup.Vals = new List<TrackingElement>();
            groupId = 1;
            foreach (var item in groupType.Elements())
            {
                if (HasData(item))
                {
                    continue;
                }

                var sportType = int.Parse(item.Attribute("SportType").Value);
                var betType = int.Parse(item.Attribute("BetType").Value);
                groupId = int.Parse(item.Attribute("GroupID").Value);
                var ele = LogFormater.FormatElement(item.Attribute("Discount"), null, $"simplify-{sportType}-{betType}");
                ele.N = CacheRef.GetSportName(sportType);
                trackingGroup.Vals.Add(ele);
            }

            return trackingGroup;
        }

        private TrackingGroup ParseSBCommSettingLog(XElement groupType, bool isMember)
        {
            TrackingGroup trackingGroup = new TrackingGroup();
            trackingGroup.N = GetCreateCustomerResource($"GroupType_150");
            trackingGroup.Vals = new List<TrackingElement>();
            if (isMember)
            {
                trackingGroup.Tables = new List<TrackingTable>();
                var trackingTable = new TrackingTable();
                trackingTable.HeaderRows = new List<List<string>>();
                trackingTable.HeaderRows.Add(new List<string> { string.Empty });
                trackingTable.Rows = new List<List<TrackingValue>>();
                if (groupType.HasAttributes)
                {
                    foreach (var attr in groupType.Attributes())
                    {
                        trackingGroup.Vals.Add(LogFormater.FormatElement(attr));
                    }
                }

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
                        Val = CacheRef.GetSportsbookCommMemberName(1, 150, sportType, betType)
                    });
                    foreach (var attr in item.Attributes().Where(i => !i.Name.LocalName.Equals("SportType")
                        && !i.Name.LocalName.Equals("BetType")
                        && !i.Name.LocalName.Equals("ProductID")
                        && !i.Name.LocalName.Equals("SettingLevel")))
                    {
                        var headerColumnName = GetCreateCustomerResource(attr.Name.LocalName);
                        if (!trackingTable.HeaderRows[0].Contains(headerColumnName))
                        {
                            trackingTable.HeaderRows[0].Add(headerColumnName);
                        }

                        row.Add(LogFormater.FormatTrackingValue(attr, $"simplify-{sportType}-{betType}"));
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
                    trackingGroup.Vals.Add(FormatSMACommElement(item.Attribute("Discount"), 1, 150, sportType, betType));
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
            ele.P = $"simplify-{sportTypeId}-{betTypeId}" + attr.Name.ToString();
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
                var value = HttpContext.GetGlobalResourceObject(ResourceExtension.Resources.CreateCustomer.ToString(), key).ToString();
                return value.Equals(key) ? HttpContext.GetGlobalResourceObject(ResourceExtension.Resources.ViewlogV2.ToString(), key).ToString() : value;
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
                case UpdateSuperPTActionId:
                case UpdateSuperPTOthersActionId:
                case UpdateSuperMinPTActionId:
                case UpdateSuperParlayMaxPayoutActionId:
                case UpdateSuperIndirectParlayMaxPayoutActionId:
                case UpdateSuperSBCommActionId:
                case UpdateOthersCommActionId:
                case UpdateSuperSBBetSettingActionId:
                case UpdateSuperIndirectSBBetSettingActionId:
                case UpdateStakeLevel158ActionId:
                case UpdateWinLossLimit155ActionId:
                    return 4;
                case CreateMasterPTActionId:
                case CreateMasterCommActionId:
                case CreateMasterBetSettingActionId:
                case UpdateMasterPTActionId:
                case UpdateMasterMinPTActionId:
                case UpdateMasterParlayMaxPayoutActionId:
                case UpdateMasterIndirectParlayMaxPayoutActionId:
                case UpdateMasterSBCommActionId:
                case UpdateMasterSBBetSettingActionId:
                case UpdateMasterIndirectSBBetSettingActionId:
                    return 3;
                case CreateAgentPTActionId:
                case CreateAgentCommActionId:
                case CreateAgentBetSettingActionId:
                case UpdateAgentPTActionId:
                case UpdateAgentMinPTActionId:
                case UpdateAgentParlayMaxPayoutActionId:
                case UpdateAgentIndirectParlayMaxPayoutActionId:
                case UpdateAgentSBCommActionId:
                case UpdateAgentSBBetSettingActionId:
                case UpdateAgentIndirectSBBetSettingActionId:
                    return 2;
                case CreateMemberPTActionId:
                case CreateMemberCommActionId:
                case CreateMemberBetSettingActionId:
                case UpdateAgentPTOnMemberActionId:
                case UpdateMasterPTOnMemberActionId:
                case UpdateSuperPTOnMemberActionId:
                case UpdateAgentPTOnGroupedMemberActionId:
                case UpdateMasterPTOnGroupedMemberActionId:
                case UpdateSuperPTOnGroupedMemberActionId:
                case UpdateMemberParlayMaxPayoutActionId:
                case UpdateMemberSBCommActionId:
                case UpdateMemberSBBetSettingActionId:
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
            => actionId >= 260000;

        private bool IsSBPTActionId(int actionId)
            => actionId == CreateSuperPTActionId
            || actionId == CreateMasterPTActionId
            || actionId == CreateAgentPTActionId
            || actionId == CreateMemberPTActionId
            || actionId == UpdateSuperPTActionId
            || actionId == UpdateSuperMinPTActionId
            || actionId == UpdateMasterPTActionId
            || actionId == UpdateMasterMinPTActionId
            || actionId == UpdateAgentPTActionId
            || actionId == UpdateAgentMinPTActionId
            || actionId == UpdateAgentPTOnMemberActionId
            || actionId == UpdateMasterPTOnMemberActionId
            || actionId == UpdateSuperPTOnMemberActionId
            || actionId == UpdateAgentPTOnGroupedMemberActionId
            || actionId == UpdateMasterPTOnGroupedMemberActionId
            || actionId == UpdateSuperPTOnGroupedMemberActionId;

        private bool IsOthersPTActionId(int actionId)
            => actionId == UpdateSuperPTOthersActionId;

        private bool IsBetSettingActionId(int actionId)
            => actionId == CreateSuperBetSettingActionId
            || actionId == CreateMasterBetSettingActionId
            || actionId == CreateAgentBetSettingActionId
            || actionId == CreateMemberBetSettingActionId
            || actionId == UpdateSuperSBBetSettingActionId
            || actionId == UpdateSuperIndirectSBBetSettingActionId
            || actionId == UpdateMasterSBBetSettingActionId
            || actionId == UpdateMasterIndirectSBBetSettingActionId
            || actionId == UpdateAgentSBBetSettingActionId
            || actionId == UpdateAgentIndirectSBBetSettingActionId
            || actionId == UpdateMemberSBBetSettingActionId;

        private bool IsSBCommActionId(int actionId)
            => actionId == CreateSuperCommActionId
            || actionId == CreateMasterCommActionId
            || actionId == CreateAgentCommActionId
            || actionId == CreateMemberCommActionId
            || actionId == UpdateSuperSBCommActionId
            || actionId == UpdateMasterSBCommActionId
            || actionId == UpdateAgentSBCommActionId
            || actionId == UpdateMemberSBCommActionId;

        private bool IsCommActionId(int actionId)
            => actionId == UpdateOthersCommActionId;

        private bool IsParlayMaxPaoutActionId(int actionId)
            => actionId == UpdateSuperParlayMaxPayoutActionId
            || actionId == UpdateSuperIndirectParlayMaxPayoutActionId
            || actionId == UpdateMasterParlayMaxPayoutActionId
            || actionId == UpdateMasterIndirectParlayMaxPayoutActionId
            || actionId == UpdateAgentParlayMaxPayoutActionId
            || actionId == UpdateAgentIndirectParlayMaxPayoutActionId
            || actionId == UpdateMemberParlayMaxPayoutActionId;

        private bool IsStakeLevelActionId(int actionId)
            => actionId == UpdateStakeLevel158ActionId;
        private bool IsWinlossLimitActionId(int actionId)
            => actionId == UpdateWinLossLimit155ActionId;

        private LogAction GetLogAction(int actionId)
        {
            if (IsSBPTActionId(actionId))
                return LogAction.SBPT;
            if (IsOthersPTActionId(actionId))
                return LogAction.PT;
            if (IsSBCommActionId(actionId))
                return LogAction.SBComm;
            if (IsCommActionId(actionId))
                return LogAction.Comm;
            if (IsBetSettingActionId(actionId))
                return LogAction.BetSetting;
            if (IsParlayMaxPaoutActionId(actionId))
                return LogAction.ParlayMaxPayout;
            if (IsStakeLevelActionId(actionId))
                return LogAction.StakeLevel;
            if (IsWinlossLimitActionId(actionId))
                return LogAction.WinlossLimit;
            throw new InvalidOperationException("ActionId is invalid or not supported.");
        }
    }

    public enum LogAction
    {
        SBPT,
        PT,
        SBComm,
        Comm,
        BetSetting,
        ParlayMaxPayout,
        WinlossLimit,
        StakeLevel,
    }
}