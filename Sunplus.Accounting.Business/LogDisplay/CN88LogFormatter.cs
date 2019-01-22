using Fanex.RemotingRef;
using Sunplus.Accounting.Entities.LogDisplay;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sunplus.Accounting.Business.LogDisplay
{
    public class CN88LogFormatter
    {
        private readonly List<int> _cn88LogActionIdCollection = new List<int>
        { 139996, 139997, 139998, 139999, 120000, 125000, 130000, 135000, 149996, 149997, 149998, 149999, 140000, 145000, 150000, 155000 };

        private readonly Dictionary<string, string> _groupResourceKeyDictionary = new Dictionary<string, string>() {
            {"10","HandicapOverUnderOddEven" },
            {"11","LiveHandicapOverUnderOddEven" },
            {"50","1x2Live1x2" },
            {"99990","OtherBetType" },
            {"99991","LiveOtherBetType" },
            {"100","Outright" }
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

        public TrackingLog GenerateDetail(string trackingValue, string SPName)
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
                trackingNode.N = "General Information";
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
                        var sport = ele.Attribute("Sport")?.Value;
                        trackingNode.N = sport.Equals("9999") ? "All Sports" : _remotingRefService.GetSportName(sport, "en-US");
                        var bettype = ele.Attribute("BT")?.Value;
                        var isLive = ele.Attribute("IsL")?.Value;
                        var groupResourceKey = _groupResourceKeyDictionary[bettype + isLive];
                        var trackingGroup = new TrackingGroup();
                        trackingGroup.N = LogFormater.GetResourceName(groupResourceKey);
                        trackingGroup.Vals = new List<TrackingElement>();
                        foreach (var attr in ele.Attributes())
                        {
                            if (!attr.Name.LocalName.Equals("Sport")
                                && !attr.Name.LocalName.Equals("BT")
                                && !attr.Name.LocalName.Equals("IsL"))
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

        public List<List<TrackingValue>> GenerateDiff(string trackingValue, string trackingValueOld)
        {
            var diff = new List<List<TrackingValue>>();

            return diff;
        }
    }
}
