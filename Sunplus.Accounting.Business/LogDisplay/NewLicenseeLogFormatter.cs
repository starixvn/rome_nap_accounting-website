namespace Sunplus.Accounting.Business.LogDisplay
{
    using Fanex.RemotingRef;
    using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class NewLicenseeLogFormatter
    {
        private readonly int logActionId = 2214;

        private readonly IRemotingRefService _remotingRefService;

        private readonly List<string> listBetSettingKey= new List<string>() { "BS", "BS1", "BS2" };

        public NewLicenseeLogFormatter() : this(null)
        { }

        public NewLicenseeLogFormatter(RemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService ?? new RemotingRefService();
        }

        public bool IsNewLicenseeActionId(int actionId) => actionId == logActionId;

        public TrackingLog GenerateDetail(string trackingValue, string spName, string actionName)
        {
            var detail = new TrackingLog();
            var xmlLoad = XElement.Parse(trackingValue);
            if (xmlLoad == null) return detail;

            detail.SP = spName;
            detail.Attrs = new List<TrackingElement>();
            detail.Vals = new List<TrackingNode>();
            if (xmlLoad.HasElements)
            {
                foreach (var ele in xmlLoad.Elements())
                {
                    if (ele.Name.LocalName.Equals("GeneralInfo"))
                    {
                        detail.Vals.Add(ParseGeneralInfo(ele));
                    }
                    else if (ele.Name.LocalName.Equals("PT"))
                    {
                        detail.Vals.Add(ParsePositionTaking(ele));
                    }
                    else if (listBetSettingKey.Contains(ele.Name.LocalName))
                    {
                        detail.Vals.Add(ParseBetSetting(ele));
                    }
                    else if (ele.Name.LocalName.Equals("SL"))
                    {
                        var SLNodes = ParseStakeLevel(ele);
                        foreach (var node in SLNodes)
                        {
                            detail.Vals.Add(node);
                        }
                    }
                }
            }

            return detail;
        }

        private TrackingNode ParseGeneralInfo(XElement element)
        {
            var trackingNode = new TrackingNode();
            trackingNode.Vals = new List<TrackingElement>();
            trackingNode.N = LogFormater.GetResourceName(element.Name.LocalName);
            foreach (var attr in element.Attributes())
            {
                trackingNode.Vals.Add(LogFormater.FormatElement(attr));
            }

            return trackingNode;
        }

        private TrackingNode ParsePositionTaking(XElement element)
        {
            var trackingNode = new TrackingNode();
            trackingNode.Vals = new List<TrackingElement>();
            trackingNode.N = LogFormater.GetResourceName(element.Name.LocalName);
            foreach (var ele in element.Elements())
            {
                trackingNode.Vals.Add(new TrackingElement()
                {
                    N = GetGroupName("Product", ele.Attribute("ID").Value),
                    P = ele.Attribute("ID").Name.LocalName,
                    Val = ele.Attribute("PT").Value
                });
            }

            return trackingNode;
        }

        private TrackingNode ParseBetSetting(XElement element)
        {
            var trackingNode = new TrackingNode();
            trackingNode.Vals = new List<TrackingElement>();
            trackingNode.Groups = new List<TrackingGroup>();
            trackingNode.N = LogFormater.GetResourceName(element.Name.LocalName);
            foreach (var ele in element.Elements())
            {
                var trackingGroup = new TrackingGroup();
                trackingGroup.N = GetGroupName(ele.Name.ToString(), ele.Attribute("ID").Value);
                trackingGroup.Vals = new List<TrackingElement>();
                foreach (var attr in ele.Attributes())
                {
                    if (!attr.Name.LocalName.Equals("ID"))
                    {
                        trackingGroup.Vals.Add(LogFormater.FormatElement(attr));
                    }
                }

                trackingNode.Groups.Add(trackingGroup);
            }

            return trackingNode;
        }

        private List<TrackingNode> ParseStakeLevel(XElement element)
        {
            var trackingNodes = new List<TrackingNode>();
            foreach (var product in element.Elements())
            {
                var productNode = new TrackingNode();
                productNode.N = "Stake Level " + GetGroupName("Product", product.Attribute("ID").Value);
                productNode.Vals = new List<TrackingElement>();
                productNode.Groups = new List<TrackingGroup>();
                foreach (var bettype in product.Elements())
                {
                    var bettypeGroup = new TrackingGroup();
                    bettypeGroup.N = GetGroupName(bettype.Name.ToString(), bettype.Attribute("ID").Value);
                    bettypeGroup.Vals = new List<TrackingElement>();
                    foreach (var attr in bettype.Attributes())
                    {
                        if (!attr.Name.LocalName.Equals("ID"))
                        {
                            bettypeGroup.Vals.Add(LogFormater.FormatElement(attr));
                        }
                    }
                    productNode.Groups.Add(bettypeGroup);
                }
                trackingNodes.Add(productNode);
            }

            return trackingNodes;
        }

        private string GetGroupName(string elementName, string Id)
        {
            if (elementName == "Product")
            {
                return _remotingRefService.GetResourceDataByRefIdAndResourceId("Product", Id, "en-US", null);
            }
            else if (elementName == "Sport")
            {
                if (Id == "1999")
                {
                    return "Soccer Parlay";
                }
                else if (Id == "999")
                {
                    return "Mix Sports Parlay";
                }
                return _remotingRefService.GetSportName(Id, "en-US");
            }
            else if (elementName == "Bettype")
            {
                return _remotingRefService.GetBetTypeName(Id, "en-US");
            }
            else
            {
                return LogFormater.GetResourceName(elementName);
            }
        }
    }
}