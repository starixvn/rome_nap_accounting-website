namespace Fanex.Tarus.Customer.DataAccess.Player3rd
{
    using CustomerSetting.Module.Core;
    using Data;
    using Fanex.Tarus.Customer.Entities.Player3rd;
    using NAP.Framework.Data;
    using System.Collections.Generic;

    public interface Player3rdDataAccess
    {
        List<Player3rd> Search(ICriteria criteria);
    }

    public class Player3rdDataAccessImpl : Player3rdDataAccess
    {
        public List<Player3rd> Search(ICriteria criteria)
        {
            var result = new List<Player3rd>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Player3rd
                    {
                        MemberId = Utils.ToInt32(reader["MemberId"]),
                        MemberName = Utils.ToString(reader["MemberName"]),
                        AgentId = Utils.ToInt32(reader["AgentId"]),
                        AgentName = Utils.ToString(reader["AgentName"]),
                        MasterId = Utils.ToInt32(reader["MasterId"]),
                        MasterName = Utils.ToString(reader["MasterName"]),
                        SuperId = Utils.ToInt32(reader["SuperId"]),
                        SuperName = Utils.ToString(reader["SuperName"]),
                        PlayerID3rd = Utils.ToInt32(reader["PlayerID3rd"]),
                    });
                }
            }

            return result;
        }

    }
}
