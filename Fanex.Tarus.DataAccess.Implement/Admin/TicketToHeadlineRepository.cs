using Fanex.CustomerSetting.Module.Core;
using Fanex.Data;
using Fanex.Tarus.DataAccess.Interface.Admin;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Tarus.DataAccess.Implement.Admin
{
    public class TicketToHeadlineRepository : ITicketToHeadlineRepository
    {

        private List<int> groupHeadlineDataChangeRowNumber;
        private List<int> superDataChangeRowNumber;
        private List<int> masterDataChangeRowNumber;
        private List<int> agentDataChangeRowNumber;

        public TicketToHeadlineRepository()
        {
            groupHeadlineDataChangeRowNumber = new List<int>();
            superDataChangeRowNumber = new List<int>();
            masterDataChangeRowNumber = new List<int>();
            agentDataChangeRowNumber = new List<int>();
        }

        public TicketToHeadlineViewModel GetData(TicketToHeadlineCriteria criteria)
        {
            var result = new TicketToHeadlineViewModel();
            TicketToHeadline trackingDataRow = new TicketToHeadline();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var reader = db.ExecuteReader();
                var rowCount = 0;
                while (reader.Read())
                {
                    rowCount++;
                    var currentDataRow = new TicketToHeadline()
                    {
                        RowNum = Utils.ToInt32(reader["RowNum"]),
                        Agent = Utils.ToString(reader["Agent"]),
                        Currency = Utils.ToString(reader["Currency"]),
                        ErrorCode = Utils.ToInt32(reader["ErrorCode"]),
                        GroupHeadline = Utils.ToString(reader["GroupHeadLine"]),
                        HeadlineType = Utils.ToString(reader["HeadlineType"]),
                        Master = Utils.ToString(reader["Master"]),
                        Member = Utils.ToString(reader["Member"]),
                        RefNo = Utils.ToInt64(reader["Refno"]),
                        Super = Utils.ToString(reader["Super"])
                    };

                    result.TicketToHeadlineCollection.Add(currentDataRow);
                    GenerateMergeRowNumber(trackingDataRow, currentDataRow, rowCount, criteria.PageSize);
                }

                reader.Close();
                result.TotalRows = Utils.ToInt32(db.GetOuputParams()["totalrows"]);
            }

            criteria.PageSize = criteria.PageSize ?? 1000;
            if (result.TicketToHeadlineCollection.Count % (int)criteria.PageSize != 0)
            {
                groupHeadlineDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)criteria.PageSize);
                superDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)criteria.PageSize);
                masterDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)criteria.PageSize);
                agentDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)criteria.PageSize);
            }

            ReassignMergeRowNum(result.TicketToHeadlineCollection);
            return result;
        }

        public TicketToHeadlineViewModel GetFakeData(int? pageSize, int? pageIndex, int rowNo = 400)
        {
            pageSize = pageSize ?? 1000;
            var result = new TicketToHeadlineViewModel();
            TicketToHeadline trackingDataRow = new TicketToHeadline();
            result.TicketToHeadlineCollection = new List<TicketToHeadline>();
            int groupHeadlineGroupNo = 0;
            int superGroupNo = 0;
            int masterGroupNo = 0;
            int agentGroupNo = 0;
            for (int i = 0; i < rowNo; i++)
            {
                groupHeadlineGroupNo = i % 81 == 0 ? ++groupHeadlineGroupNo : groupHeadlineGroupNo;
                superGroupNo = i % 27 == 0 ? ++superGroupNo : superGroupNo;
                masterGroupNo = i % 9 == 0 ? ++masterGroupNo : masterGroupNo;
                agentGroupNo = i % 3 == 0 ? ++agentGroupNo : agentGroupNo;
                var currentDataRow = new TicketToHeadline()
                {
                    GroupHeadline = "GroupHeadline" + groupHeadlineGroupNo.ToString(),
                    Agent = "Agent" + agentGroupNo.ToString(),
                    Currency = "Currency" + superGroupNo.ToString(),
                    ErrorCode = 0,
                    HeadlineType = i < rowNo / 2 ? "VIP" : "Normal",// "HeadlineType" + groupHeadlineGroupNo.ToString(),
                    Master = "Master" + masterGroupNo.ToString(),
                    Member = "Member" + (i + 1),
                    RefNo = 123456,
                    RowNum = i + 1,
                    Super = "Super" + superGroupNo.ToString(),
                };

                result.TicketToHeadlineCollection.Add(currentDataRow);
                GenerateMergeRowNumber(trackingDataRow, currentDataRow, i + 1);
            }

            groupHeadlineDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)pageSize);
            superDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)pageSize);
            masterDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)pageSize);
            agentDataChangeRowNumber.Add(result.TicketToHeadlineCollection.Count % (int)pageSize);
            result.TotalRows = rowNo;
            ReassignMergeRowNum(result.TicketToHeadlineCollection);
            if (pageSize != null && pageIndex != null)
            {
                result.TicketToHeadlineCollection = result.TicketToHeadlineCollection.Where(m => m.RowNum >= (pageIndex - 1) * pageSize + 1 && m.RowNum <= pageIndex * pageSize).ToList();
            }

            return result;
        }

        public void RePaginationData(List<TicketToHeadline> data, int pageSize)
        {
            groupHeadlineDataChangeRowNumber = new List<int>();
            superDataChangeRowNumber = new List<int>();
            masterDataChangeRowNumber = new List<int>();
            agentDataChangeRowNumber = new List<int>();
            TicketToHeadline trackingDataRow = new TicketToHeadline();
            for (int i = 0; i < data.Count; i++)
            {
                data[i].MergeRowNumAgent = 0;
                data[i].MergeRowNumGroupHeadline = 0;
                data[i].MergeRowNumMaster = 0;
                data[i].MergeRowNumSuper = 0;
                GenerateMergeRowNumber(trackingDataRow, data[i], i + 1);
            }

            groupHeadlineDataChangeRowNumber.Add(data.Count % pageSize);
            superDataChangeRowNumber.Add(data.Count % pageSize);
            masterDataChangeRowNumber.Add(data.Count % pageSize);
            agentDataChangeRowNumber.Add(data.Count % pageSize);
            ReassignMergeRowNum(data);
        }

        private void ReassignMergeRowNum(List<TicketToHeadline> data)
        {
            int startRow = 0;
            int endRow = 0;
            if (data.Count == 0)
            {
                return;
            }

            for (int i = 0; i < groupHeadlineDataChangeRowNumber.Count; i++)
            {
                endRow = groupHeadlineDataChangeRowNumber[i];
                data[startRow].MergeRowNumGroupHeadline = endRow - startRow + 1;
                startRow = endRow;
            }

            startRow = 0;
            endRow = 0;
            for (int i = 0; i < superDataChangeRowNumber.Count; i++)
            {
                endRow = superDataChangeRowNumber[i];
                data[startRow].MergeRowNumSuper = endRow - startRow + 1;
                startRow = endRow;
            }

            startRow = 0;
            endRow = 0;
            for (int i = 0; i < masterDataChangeRowNumber.Count; i++)
            {
                endRow = masterDataChangeRowNumber[i];
                data[startRow].MergeRowNumMaster = endRow - startRow + 1;
                startRow = endRow;
            }

            startRow = 0;
            endRow = 0;
            for (int i = 0; i < agentDataChangeRowNumber.Count; i++)
            {
                endRow = agentDataChangeRowNumber[i];
                data[startRow].MergeRowNumAgent = endRow - startRow + 1;
                startRow = endRow;
            }
        }

        private void GenerateMergeRowNumber(TicketToHeadline trackingDataRow, TicketToHeadline currentDataRow, int currentRowIndex, int? pageSize = 1000)
        {
            pageSize = pageSize ?? 1000;
            int currentPageIndex = currentRowIndex / (int)pageSize;
            if (currentRowIndex == 1)
            {
                trackingDataRow.Clone(currentDataRow);
                return;
            }

            if (currentRowIndex % (int)pageSize == 1)
            {
                trackingDataRow.Clone(currentDataRow);
                groupHeadlineDataChangeRowNumber.Add(currentPageIndex * (int)pageSize);
                superDataChangeRowNumber.Add(currentPageIndex * (int)pageSize);
                masterDataChangeRowNumber.Add(currentPageIndex * (int)pageSize);
                agentDataChangeRowNumber.Add(currentPageIndex * (int)pageSize);
                return;
            }

            if (trackingDataRow.GroupHeadline != currentDataRow.GroupHeadline)
            {
                groupHeadlineDataChangeRowNumber.Add(currentRowIndex - 1);
                trackingDataRow.GroupHeadline = currentDataRow.GroupHeadline;
            }

            if (trackingDataRow.Super != currentDataRow.Super)
            {
                superDataChangeRowNumber.Add(currentRowIndex - 1);
                trackingDataRow.Super = currentDataRow.Super;
            }

            if (trackingDataRow.Master != currentDataRow.Master)
            {
                masterDataChangeRowNumber.Add(currentRowIndex - 1);
                trackingDataRow.Master = currentDataRow.Master;
            }

            if (trackingDataRow.Agent != currentDataRow.Agent)
            {
                agentDataChangeRowNumber.Add(currentRowIndex - 1);
                trackingDataRow.Agent = currentDataRow.Agent;
            }
        }

    }
}
