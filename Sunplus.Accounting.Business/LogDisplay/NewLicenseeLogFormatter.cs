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
                    if(ele.Name.LocalName.Equals("GeneralInfo"))
                    {
                        detail.Vals.Add(ParseGeneralInfo(ele));
                    }
                    else if(ele.Name.LocalName.Equals("PT"))
                    {
                        detail.Vals.Add(ParsePositionTaking(ele));
                    }
                    else if (ele.Name.LocalName.Equals("BS"))
                    {
                        detail.Vals.Add(ParseBetSetting(ele));
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
            foreach(var ele in element.Elements())
            {
                trackingNode.Vals.Add(new TrackingElement() {
                    N =_remotingRefService.GetResourceDataByRefIdAndResourceId("Product", ele.Attribute("ID").Value, "en-US", null),
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
                trackingGroup.N = _remotingRefService.GetResourceDataByRefIdAndResourceId("Product", ele.Attribute("ID").Value, "en-US", null);
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
    }
}