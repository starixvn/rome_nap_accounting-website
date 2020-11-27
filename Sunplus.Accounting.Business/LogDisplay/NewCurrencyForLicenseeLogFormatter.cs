namespace Sunplus.Accounting.Business.LogDisplay
{
    using Fanex.RemotingRef;
    using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class NewCurrencyForLicenseeLogFormatter : NewLicenseeLogFormatter
    {
        private readonly int logActionId = 2216;

        public NewCurrencyForLicenseeLogFormatter() : base()
        {
        }

        public NewCurrencyForLicenseeLogFormatter(RemotingRefService remotingRefService) : base(remotingRefService)
        {
        }

        public bool IsNewCurrencyForLicenseeActionId(int actionId) => actionId == logActionId;

        protected override TrackingNode ParseGeneralInfo(XElement element)
        {
            var trackingNode = new TrackingNode
            {
                Vals = new List<TrackingElement>(),
                N = LogFormater.GetResourceName(element.Name.LocalName)
            };

            foreach (var attr in element.Attributes())
            {
                if (!attr.Name.ToString().ToLower().Equals("currency"))
                {
                    trackingNode.Vals.Add(LogFormater.FormatElement(attr));
                }
            }

            return trackingNode;
        }
    }
}