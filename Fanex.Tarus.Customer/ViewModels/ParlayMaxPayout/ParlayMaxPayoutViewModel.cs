using Fanex.Tarus.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.ViewModels.ParlayMaxPayout
{
    public class ParlayMaxPayoutViewModel
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public int RoleId { get; set; }

        public List<ParlayMaxPayoutEntityModel> ListEntityModel { get; set; }
    }

    public class ParlayMaxPayoutEntityModel
    {
        public int SportType { get; set; }

        public string SportName => CacheRef.GetSportName(SportType);

        public int Bettype { get; set; }

        public string BettypeName => CacheRef.GetBettype(Bettype);

        public decimal SingleMatchParlay { get; set; }

        public decimal UplineSingleMatchParlay { get; set; }

        public bool IsAllowUnlimitedSingleMatchParlay { get; set; }

        public decimal Combo2 { get; set; }

        public decimal UplineCombo2 { get; set; }

        public bool IsAllowUnlimitedCombo2 { get; set; }

        public decimal Combo35 { get; set; }

        public decimal UplineCombo35 { get; set; }

        public bool IsAllowUnlimitedCombo35 { get; set; }

        public decimal ComboOver6 { get; set; }

        public decimal UplineComboOver6 { get; set; }

        public bool IsAllowUnlimitedComboOver6 { get; set; }
    }
}
