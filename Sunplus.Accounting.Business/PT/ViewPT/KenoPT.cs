// -----------------------------------------------------------------------
// <copyright file="KenoPT.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Business.PT.ViewPT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
    using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
    using System.Data;
    using DNAide.Core;
    using Sunplus.Accounting.Core.Extensions;
    using Bettype = Sunplus.Accounting.Entities.Customer.PT.ViewPT.Bettype;
    using Sunplus.Accounting.Entities.PositionTaking.Racing;
    using DNAide.Utils;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KenoPT
    {
        private static string _lcsPt = "{0}positiontaking";

        public static Dictionary<int, string> Bettypes = new Dictionary<int, string>()
            {
                {1501, "Australia Acttab"},
                {1503, "Slovakia Eklub"},
                {1504, "Canada Keno"},
                {1505, "West Canada Keno"},
                {1507, "Massac"},
                {1508, "Ohio"},
                {1509, "Malta"}
            };

        public BettypeDTO GetBettypeByKeno()
        {
            var result = new BettypeDTO { BettypeList = new List<Bettype>() };

            foreach (var bettype in KenoPT.Bettypes)
            {
                result.BettypeList.Add(new Bettype()
                {
                    BettypeId = bettype.Key,
                    BettypeName = bettype.Value
                });
            }

            return result;
        }
    }
}
