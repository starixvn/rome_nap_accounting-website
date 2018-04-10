namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.BetList.Criteria.Results;

    public class AGCasinoResultDataAccess : IAGCasinoResultDataAccess
    {
        public List<Fanex.Result.Core.AGCasino.DTO.AGCasino> GetResult(GetAGCasinoResultCriteria criteria)
        {
            var result = new List<Fanex.Result.Core.AGCasino.DTO.AGCasino>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new Fanex.Result.Core.AGCasino.DTO.AGCasino
                        {
                            Bettype = criteria.BetType,
                            Result = Converter.ToString2(reader["transdesc"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
