namespace Fanex.Tarus.BetList.Helper
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.Tarus.BetList.Models.CN88;

    public static class CN88BetListHelpers
    {
        private const string AbnormalBetXmlTag = "p1";
        private const string AbnormalBetAttributeName = "ab";

        public static string Stake(object value)
            => string.Format("{0:#,##0.00}", value);

        public static string TicketRowClass(TicketDto ticket)
        {
            if (IsAbnormalBet(ticket))
            {
                return "row-abnormal";
            }

            return $"row-{ticket.Status.ToLowerInvariant()}";
        }

        public static bool IsAbnormalBet(ITicket ticket)
        {
            var status = ticket.Status.ToLowerInvariant();

            return status.Equals(BetStatus.Void, StringComparison.InvariantCulture)
               && ticket.ExtraInfo.GetXmlTagAttributeValue(AbnormalBetXmlTag, AbnormalBetAttributeName) == "1";
        }

        public static string GetXmlTagAttributeValue(this string value, string xmlTag, string attributeName)
        {
            if (value.HasXmlTag(xmlTag))
            {
                var xmlDoc = value.ParseXml();

                return xmlDoc.Descendants(xmlTag).FirstOrDefault().Attribute(attributeName)?.Value;
            }

            return string.Empty;
        }

        public static bool HasXmlTag(this string value, string xmlTag)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var xmlDoc = value.ParseXml();

                return xmlDoc.Descendants(xmlTag)?.FirstOrDefault() != null;
            }

            return false;
        }

        public static XDocument ParseXml(this string value) => XDocument.Parse($"<root>{value}</root>");

        public static bool IsInvalidTicket(ITicket ticket)
        {
            return IsAbnormalBet(ticket) ||
                 string.Equals(ticket.Status, "void", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(ticket.Status, "reject", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(ticket.Status, "refund", StringComparison.OrdinalIgnoreCase);
        }
    }
}
