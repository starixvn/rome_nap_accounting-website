namespace Fanex.Tarus.BetList.DataAccess
{
    using Fanex.Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IBookedBetRepository
    {   
       IList<BookedBet> FetchWithOutputs(dynamic criteria, Func<DataRow, BookedBet> mapObjectFunction, ref int totalRows);
    }

    public class BookedBetRepository : IBookedBetRepository
    {

        public IList<BookedBet> FetchWithOutputs(dynamic criteria, Func<DataRow, BookedBet> mapObjectFunction, ref int totalRows)
        {
            var listData = new List<BookedBet>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        listData.Add(mapObjectFunction(row));
                    }
                }
                
                totalRows = (int)db.GetOuputParams()["totalrows"];
            }

            return listData;
        }

    }
}