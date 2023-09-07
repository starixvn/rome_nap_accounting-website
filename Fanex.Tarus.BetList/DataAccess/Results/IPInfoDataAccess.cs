using Alpha.IPInfo.DataAccess;
using Alpha.IPInfo.Models;
using Fanex.Common;
using Fanex.Data;
using Fanex.Tarus.BetList.Criteria.IPInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Tarus.BetList.DataAccess.Results
{
    public class IPInfoDataAccess : IIPInfoDataAccess
    {
        public Task<IEnumerable<IPV4Information>> GetIPV4Info(string ipList)
        {
            var criteria = new GetIPV4InfoCriteria(ipList);
            var result = new List<IPV4Information>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindResultIPV4(reader));
                    }
                }
            }
            return Task.FromResult(result.AsEnumerable());
        }

        private IPV4Information BindResultIPV4(System.Data.IDataReader reader)
        {
            return new IPV4Information(
                string.Empty,
                Converter.To<long>(reader["ipFrom"]),
                Converter.To<long>(reader["ipTo"]),
                Converter.ToString2(reader["countryShort"]),
                Converter.ToString2(reader["countryLong"])
            );
        }

        public Task<IEnumerable<IPV4Information>> GetIPV4InfoByTopCountries(string countries)
        {
            var criteria = new GetIPV4InfoByTopCountryCriteria(countries);
            var result = new List<IPV4Information>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindResultIPV4(reader));
                    }
                }
            }
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<IPV6Information>> GetIPV6Info(string ipList)
        {

            var criteria = new GetIPV6InfoCriteria(ipList);
            var result = new List<IPV6Information>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindResultIPV6(reader));
                    }
                }
            }
            return Task.FromResult(result.AsEnumerable());
        }

        private IPV6Information BindResultIPV6(System.Data.IDataReader reader)
        {
            return new IPV6Information(
                            Converter.ToString2(reader["ip"]),
                            Array.Empty<byte>(),
                            Array.Empty<byte>(),                            
                            Converter.ToString2(reader["countryShort"]),
                            Converter.ToString2(reader["countryLong"])
                        );
        }
    }
}
