using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover;
using System.Data;
using DNAide.Core;

namespace Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover
{
    public class SMATurnoverService: ISMATurnoverService
    {
        public ISMATurnoverAccess _SMATurnoverAccess;

        public SMATurnoverService()
            : this(null)
        { }

        public SMATurnoverService(ISMATurnoverAccess SMATurnoverAccess)
        {
            _SMATurnoverAccess = SMATurnoverAccess ?? new SMATurnoverAccess();
        }

        public List<BaseModel> GetSportType(int mode)
        {
            var sportList = new List<BaseModel>();
            DataTable dtb = _SMATurnoverAccess.GetSportType(mode);
            foreach (DataRow row in dtb.Rows)
            {
                var sport = new BaseModel()
                {
                    Id = CastAide.AsInt32(row["sporttype"]),
                    Name = CastAide.AsString(row["sportname"])
                };
                sportList.Add(sport);
            }
            return sportList;
        }

        public List<BaseModel> GetLeague(int sportType, DateTime fromDate, DateTime toDate)
        {
            var leagueList = new List<BaseModel>();
            var dtb = _SMATurnoverAccess.GetLeague(sportType, fromDate, toDate);
            foreach (DataRow row in dtb.Rows)
            {
                var league = new BaseModel() { 
                    Id = CastAide.AsInt32(row["leagueid"]),
                    Name = CastAide.AsString(row["leaguename"])
                };
                leagueList.Add(league);
            }
            return leagueList;
        }
    }
}
