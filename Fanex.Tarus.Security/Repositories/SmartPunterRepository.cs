using System.Collections;
using Fanex.Common;
using Fanex.Data;
using Fanex.Tarus.Security.Criteria;

namespace Fanex.Tarus.Security.Repositories
{
    public class SmartPunterRepository : ISmartPunterRepository
    {
        public bool CheckSmartPunter(CheckSmartPunterCriteria criteria)
        {
            bool isSmartPunter;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                Hashtable hash = db.GetOuputParams();
                isSmartPunter = Converter.To<bool>(hash["issmartpunter"]);
            }

            return isSmartPunter;
        }
    }
}