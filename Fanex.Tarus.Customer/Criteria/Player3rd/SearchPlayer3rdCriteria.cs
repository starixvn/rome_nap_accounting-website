using Fanex.NAP.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Fanex.Tarus.Customer.Criteria.Player3rd
{
    public class SearchPlayer3rdCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public byte SiteType { get; set; }

        public int ProductId { get; set; }

        public string PlayerIDs { get; set; }

        public string Usernames { get; set; }

        public override bool IsValid()
        {
            return ProductId > 0 
                && (!string.IsNullOrWhiteSpace(PlayerIDs) || !string.IsNullOrWhiteSpace(Usernames))
                && UserId > 0;
        }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelPlayerId";
        }
    }
}
