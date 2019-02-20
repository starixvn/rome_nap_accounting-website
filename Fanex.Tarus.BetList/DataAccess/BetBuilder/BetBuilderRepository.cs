namespace Fanex.Tarus.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using BetDetails.BetBuilder.Criteria;
    using BetDetails.BetBuilder.DataAccesses;
    using BetDetails.BetBuilder.Model;
    using Criteria.BetBuilder;
    using Data;

    public class BetBuilderRepository : IBetBuilderDataAccess
    {
        public IEnumerable<BetBuilderDetailModel> GetBetBuilderDetail(IGetBetBuilderDetailParameter criteria)
        {
            IList<BetBuilderDetailModel> details = new List<BetBuilderDetailModel>();
            var parameter = new GetBetBuilderDetailCriteria(criteria);

            using (var db = new ObjectDb(parameter.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(parameter.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        details.Add(ToBetBuilderDetailModel(reader));
                    }
                }
            }

            return details;
        }

        private static BetBuilderDetailModel ToBetBuilderDetailModel(IDataReader reader)
        {
            var model = new BetBuilderDetailModel
            {
                SelectionType = Convert.ToInt32(reader["selectiontype"]),
                Result = reader["result"].ToString()
            };
            

            return model;
        }
    }
}
