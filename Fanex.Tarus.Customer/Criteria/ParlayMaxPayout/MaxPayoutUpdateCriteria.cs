namespace Fanex.Tarus.Customer.Criteria.ParlayMaxPayout
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Requests.ParlayMaxPayout;
    using Fanex.Tarus.Customer.ViewModels.ParlayMaxPayout;
    using System.Collections.Generic;
    using System.Text;

    public class MaxPayoutUpdateCriteria : CriteriaBase
    {
        public MaxPayoutUpdateCriteria()
        { }

        public MaxPayoutUpdateCriteria(ParlayMaxPayoutUpdateRequest request)
        {
            this.CustId = request.CustId;
            this.IP = request.IP;
            this.UserId = request.UserId;
            this.MaxPayoutLimit = request.ParlayMaxPayoutList?.Count > 0 ? this.ListMaxPayoutToXML(request.ParlayMaxPayoutList) : string.Empty;
        }

        public int CustId { get; set; }

        public string MaxPayoutLimit { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override bool IsValid()
        {
            return CustId > 0 && UserId > 0 && !string.IsNullOrWhiteSpace(MaxPayoutLimit);
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MaxPayoutUpd";
        }

        private string ListMaxPayoutToXML(List<ParlayMaxPayoutUpdateEntityModel> listMaxpayout)
        {
            StringBuilder builder = new StringBuilder("<ROOT>");
            foreach (var maxpayout in listMaxpayout)
            {
                builder.Append("<MaxPayoutLimit ");
                builder.Append($"SportType=\"{maxpayout.SportType}\" ");
                builder.Append($"BetType=\"{maxpayout.Bettype}\" ");
                builder.Append(maxpayout.SingleMatchParlay == null ? string.Empty : $"SingleMatchParlay=\"{maxpayout.SingleMatchParlay}\" ");
                builder.Append(maxpayout.Combo2 == null ? string.Empty : $"Combo2=\"{maxpayout.Combo2}\" ");
                builder.Append(maxpayout.Combo35 == null ? string.Empty : $"Combo35=\"{maxpayout.Combo35}\" ");
                builder.Append(maxpayout.ComboOver6 == null ? string.Empty : $"ComboOver6=\"{maxpayout.ComboOver6}\" ");
                builder.Append("/>");
            }

            builder.Append("</ROOT>");
            return builder.ToString();
        }
    }
}
