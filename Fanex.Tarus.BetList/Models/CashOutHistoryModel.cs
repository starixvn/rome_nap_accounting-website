using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Fanex.Tarus.BetList.Models
{
    public class CashOutHistoryModel
    {
        public long TransId { get; set; }

        public string CashoutExtraInfo { get; set; }

        public List<CashoutHistoryDetail> DetailHistory
        {
            get
            {
                var detailHistory = new List<CashoutHistoryDetail>();
                if (!string.IsNullOrEmpty(CashoutExtraInfo))
                {
                    var root = XElement.Parse(CashoutExtraInfo);
                    foreach (XElement item in root.Elements())
                    {
                        var time = item.Attribute("ct").Value;
                        var amount = item.Attribute("w").Value;
                        var action = item.Attribute("s").Value;

                        detailHistory.Add(new CashoutHistoryDetail(DateTime.Parse(time), decimal.Parse(amount), action));
                    }
                }

                return detailHistory;
            }
        }
    }
    
    public class CashoutHistoryDetail
    {
        public CashoutHistoryDetail(DateTime changeTime, decimal amount, string actionId)
        {
            ChangeTime = changeTime;
            CashoutAmount = amount;
            ActionId = actionId;
        }
        private DateTime ChangeTime { get; }

        public string ChangeTimeText => ChangeTime.ToString();

        public decimal CashoutAmount { get; }

        private string ActionId { get; }

        public string ActionName

        {
            get
            {
                var actionList = new Dictionary<string, string>
                {
                    { "1", "Cash Out" },
                    { "6", "Cash Out Cancelled" },
                    { "8", "Cash Out Recovered" }
                };

                return actionList.ContainsKey(ActionId) ? actionList[ActionId] : ActionId;
            }


        }
    }
}
