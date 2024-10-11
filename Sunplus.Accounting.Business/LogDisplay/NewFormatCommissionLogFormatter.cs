using System.Collections.Generic;
using System.Xml.Linq;
using Fanex.RemotingRef;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;

namespace Sunplus.Accounting.Business.LogDisplay
{
    public class NewFormatCommissionLogFormatter
    {
        private readonly IList<int> logActionIds = new List<int> { 271000, 271001, 2612 };
        private readonly IRemotingRefService _remotingRefService;

        private readonly IList<string> ignoreDisplayComm = new List<string> { "1_4", "1_5", "2_2" };

        public NewFormatCommissionLogFormatter() : this(null)
        { }

        public NewFormatCommissionLogFormatter(RemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService ?? new RemotingRefService();
        }

        public bool IsNewFormatCommissionActionId(int actionId) => logActionIds.Contains(actionId);

        public TrackingLog GenerateDetail(string trackingValue, string spName, string actionName)
        {
            var detail = new TrackingLog();
            var xmlLoad = XElement.Parse(trackingValue);
            if (xmlLoad == null) return detail;

            detail.SP = spName;
            detail.Attrs = new List<TrackingElement>();
            detail.Vals = new List<TrackingNode>();

            if (xmlLoad.HasAttributes)
            {
                var trackingNode = new TrackingNode
                {
                    Vals = new List<TrackingElement>(),
                    N = actionName
                };
                foreach (var attr in xmlLoad.Attributes())
                {
                    trackingNode.Vals.Add(LogFormater.FormatElement(attr));
                }

                if (xmlLoad.HasElements)
                {
                    trackingNode.Groups = new List<TrackingGroup>
                    {
                        ParseComm(xmlLoad)
                    };
                }

                detail.Vals.Add(trackingNode);
            }

            return detail;
        }

        private TrackingGroup ParseComm(XElement xmlLoad)
        {
            var trackingGroup = new TrackingGroup
            {
                N = LogFormater.GetResourceName("Comm"),
                Vals = new List<TrackingElement>()
            };
            trackingGroup.Tables = trackingGroup.Tables ?? new List<TrackingTable>();
            foreach (var item in xmlLoad.Elements())
            {
                var sportBetType = $"{item.Attribute("SportType").Value}_{item.Attribute("BetType").Value}";
                if (!ignoreDisplayComm.Contains(sportBetType))
                {
                    var productName = _remotingRefService.GetResourceDataByRefIdAndResourceId("Product_Sport_BT", sportBetType, "en_US", sportBetType);
                    var ele = LogFormater.FormatElement(item.Attribute("PlayerDiscount") ?? item.Attribute("Discount"), null, $"simplify-{sportBetType}");
                    ele.N = productName;
                    trackingGroup.Vals.Add(ele);
                }
            }

            return trackingGroup;
        }
    }
}