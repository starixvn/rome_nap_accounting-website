using Fanex.RemotingRef;
using Sunplus.Accounting.Entities.LogDisplay;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Sunplus.Accounting.Business.LogDisplay
{
    public class CN88LogFormatter
    {
        private readonly string _resourceClassName = "ViewlogV2";
        private readonly List<int> _cn88LogActionIdCollection = new List<int>
        {
            100000, 105000, 110000,
            119997, 119998, 119999,
            120000, 125000, 130000,
            135000, 139996, 139997,
            139998, 139999, 140000,
            145000, 149996, 149997,
            149998, 149999, 150000,
            155000 };

        private readonly Dictionary<string, string> _groupResourceKeyDictionary = new Dictionary<string, string>() {
            {"10","HandicapOverUnderOddEven" },
            {"11","InPlayHandicapOverUnderOddEven" },
            {"50","1x2InPlay1x2" },
            {"200","MoneylineInPlayMoneyline" },
            {"99990","OtherBetType" },
            {"99991","InPlayOtherBetType" },
            {"100","Outright" }
        };

        private readonly Dictionary<string, string> _sportResourceKeyDictionary = new Dictionary<string, string>()
        {
            {"1","Sportsbook" },
            {"9998","Non-Sport Products" }
        };

        private readonly IRemotingRefService _remotingRefService;
        public CN88LogFormatter() : this(null)
        {
        }

        public CN88LogFormatter(RemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService ?? new RemotingRefService();
        }

        public bool IsCN88LogActionId(int actionId)
        {
            return _cn88LogActionIdCollection.Contains(actionId);
        }

        public TrackingLog GenerateDetail(int actionId, string trackingValue, string SPName, string actionName)
        {
            if (IsUpdatePTLog(actionId))
            {
                return GeneratePTDetail(trackingValue, SPName, actionName);
            }
            else
            {
                return GenerateCommissionAndBetSettingDetail(trackingValue, SPName, actionName);
            }
        }

        private bool IsUpdatePTLog(int actionId)
        {
            return actionId >= 100000 && actionId < 120000;
        }

        private TrackingLog GenerateCommissionAndBetSettingDetail(string trackingValue, string SPName, string actionName)
        {
            var detail = new TrackingLog();
            var xmlLoad = XElement.Parse(trackingValue);
            if (xmlLoad == null) { return detail; }

            detail.SP = SPName;
            detail.Attrs = new List<TrackingElement>();
            detail.Vals = new List<TrackingNode>();
            if (xmlLoad.HasAttributes)
            {
                var trackingNode = new TrackingNode();
                trackingNode.Groups = new List<TrackingGroup>();
                trackingNode.Vals = new List<TrackingElement>();
                trackingNode.N = actionName;
                foreach (var attr in xmlLoad.Attributes())
                {
                    trackingNode.Vals.Add(LogFormater.FormatElement(attr));
                }

                detail.Vals.Add(trackingNode);
            }

            if (xmlLoad.HasElements)
            {
                foreach (var group in xmlLoad.Elements().GroupBy(e => e.Attribute("Sport").Value))
                {
                    var trackingNode = new TrackingNode();
                    trackingNode.Groups = new List<TrackingGroup>();
                    trackingNode.Vals = new List<TrackingElement>();

                    foreach (var ele in group)
                    {
                        trackingNode.N = GetSportName(ele.Attribute("Sport")?.Value);
                        var groupResourceKey = GetBettypeName(ele.Attribute("BT")?.Value, ele.Attribute("InPlay")?.Value);
                        var trackingGroup = new TrackingGroup();
                        trackingGroup.N = LogFormater.GetResourceName(groupResourceKey);
                        trackingGroup.Vals = new List<TrackingElement>();
                        foreach (var attr in ele.Attributes())
                        {
                            if (!attr.Name.LocalName.Equals("Sport")
                                && !attr.Name.LocalName.Equals("BT")
                                && !attr.Name.LocalName.Equals("InPlay"))
                            {
                                trackingGroup.Vals.Add(LogFormater.FormatElement(attr));
                            }
                        }

                        trackingNode.Groups.Add(trackingGroup);
                    }

                    detail.Vals.Add(trackingNode);
                }
            }

            return detail;
        }

        private TrackingLog GeneratePTDetail(string trackingValue, string SPName, string actionName)
        {
            var detail = new TrackingLog();
            var xmlLoad = XElement.Parse(trackingValue);
            if (xmlLoad == null) { return detail; }

            detail.SP = SPName;
            detail.Attrs = new List<TrackingElement>();
            detail.Vals = new List<TrackingNode>();
            if (xmlLoad.HasAttributes)
            {
                var trackingNode = new TrackingNode();
                trackingNode.Groups = new List<TrackingGroup>();
                trackingNode.Vals = new List<TrackingElement>();
                trackingNode.N = actionName;
                foreach (var attr in xmlLoad.Attributes())
                {
                    trackingNode.Vals.Add(LogFormater.FormatElement(attr));
                }

                detail.Vals.Add(trackingNode);
            }

            if (xmlLoad.HasElements)
            {
                foreach (var ele in xmlLoad.Elements())
                {
                    var trackingNode = new TrackingNode();
                    trackingNode.Groups = new List<TrackingGroup>();
                    trackingNode.Vals = new List<TrackingElement>();
                    var sporttype = ele.Attribute("Sport")?.Value;
                    trackingNode.N = sporttype.Equals("1") ? "Sportsbook" : GetSportName(sporttype);
                    foreach (var attr in ele.Attributes())
                    {
                        if (!attr.Name.LocalName.Equals("Sport")
                            && !attr.Name.LocalName.Equals("InPlay"))
                        {
                            trackingNode.Vals.Add(LogFormater.FormatElement(attr));
                        }
                    }

                    detail.Vals.Add(trackingNode);
                }
            }
            return detail;
        }

        private string GetSportName(string sport)
        {
            string sportName = GetFromResource($"cn88_sporttype_{sport}");
            sportName = sportName ?? _remotingRefService.GetSportName(sport, "en-US", sport);
            return sportName ?? sport;
        }

        private string GetBettypeName(string bettype, string inplay)
        {
            string bettypeName = GetFromResource($"cn88_bettype_{bettype}_inplay_{inplay}");
            return bettypeName ?? bettype;
        }

        private string GetFromResource(string key)
        {
            try
            {
                return HttpContext.GetGlobalResourceObject(_resourceClassName, key)?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
