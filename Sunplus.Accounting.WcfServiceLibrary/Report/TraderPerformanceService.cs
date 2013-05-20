using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using DNAide.Core;
using Sunplus.Accounting.Business.Report.Other;
using Sunplus.Accounting.Entities.Report;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TraderPerformanceService : ITraderPerformanceService
    {
        public TraderPerformanceDTO GetTraderPerformance(string fromDate, string toDate, int isSoccer)
        {
            var traderPerformance = new TraderPerfomanceManager();
            var result = traderPerformance.GetTraderPerformance(fromDate, toDate, isSoccer);
            return result;
        }

        public void TraderPerformanceExport2Excel(string fromDate, string toDate, int isSoccer)
        {
            var traderPerformance = new TraderPerfomanceManager();
            traderPerformance.TraderPerformanceExport2Excel(fromDate, toDate, isSoccer);
        }

        public TraderPerformanceDetailDTO GetTraderPerformanceReportDetail(string fromDate,
            string toDate,
            int isSoccer,
            int pageIndex,
            int pageSize,
            bool getNewData,
            string sortField,
            int sortFactor,
            string sortDataType)
        {
            #region get data
            var traderPerformanceDetail = new TraderPerfomanceManager();
            var traderResult = new TraderPerformanceDetailDTO();
            traderResult = traderPerformanceDetail.GetTraderPerformanceDetail(fromDate, toDate, isSoccer, getNewData);
            #endregion

            #region assign and return
            if (traderResult != null && traderResult.rows != null)
            {
                var traderPaging = new TraderPerformanceDetailDTO();
                traderPaging.pageindex = pageIndex;
                traderPaging.total = traderResult.rows != null ? traderResult.rows.Count() : 0;
                if (!string.IsNullOrEmpty(sortField))
                {
                    traderResult.rows = this.SortTraderPerformanceReportDetail(sortField,
                        sortFactor,
                        sortDataType,
                        traderResult.rows);
                }

                var query = (from trader in traderResult.rows select trader).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                traderPaging.rows = query.ToList();
                traderPaging.footer = traderResult.footer;
                return traderPaging;
            }
            else
                return traderResult;
            #endregion
        }

        public void TraderPerformanceDetailExport2Excel(string fromDate, string toDate, int isSoccer)
        {
            var traderPerformance = new TraderPerfomanceManager();
            traderPerformance.TraderPerformanceDetailExport2Excel(fromDate, toDate, isSoccer);
        }

        private List<T> SortByProperty<T>(List<T> dataSource,
            string sortType,
            string sortCol,
            bool isNumberSort)
        {
            PropertyInfo propInfo = null;
            if (sortCol != "UserName")
            {
                propInfo = typeof(T).GetProperty(sortCol);
                if (propInfo == null || dataSource == null || dataSource.Count == 0)
                {
                    return null;
                }
            }

            bool isAsc = false;
            if (sortType.ToLower().Equals("asc"))
            {
                isAsc = true;
            }
            else
            {
                isAsc = false;
            }

            Comparison<T> compare = delegate(T a, T b)
            {
                if (sortCol == "UserName")
                {
                    var nameA = isAsc ?
                        (a as TraderPerformanceDetailBO).children.ElementAt(0).UserName :
                        (b as TraderPerformanceDetailBO).children.ElementAt(0).UserName;
                    var nameB = isAsc ?
                        (b as TraderPerformanceDetailBO).children.ElementAt(0).UserName :
                        (a as TraderPerformanceDetailBO).children.ElementAt(0).UserName;

                    if (nameA == nameB)
                    {
                        return 0;
                    }

                    if ((a as TraderPerformanceDetailBO).children.ElementAt(0).UserName.ToLower() == "(unassigned)")
                    {
                        return -1;
                    }

                    if ((b as TraderPerformanceDetailBO).children.ElementAt(0).UserName.ToLower() == "(unassigned)")
                    {
                        return 1;
                    }

                    return nameA is IComparable ? ((IComparable)nameA).CompareTo(nameB) : 0;
                }
                else
                {
                    object valueA;
                    object valueB;
                    if (isNumberSort)
                    {
                        valueA = isAsc
                            ? CastAide.AsDecimal(propInfo.GetValue(a, null))
                            : CastAide.AsDecimal(propInfo.GetValue(b, null));

                        valueB = isAsc
                            ? CastAide.AsDecimal(propInfo.GetValue(b, null))
                            : CastAide.AsDecimal(propInfo.GetValue(a, null));
                    }
                    else
                    {
                        valueA = isAsc
                            ? CastAide.AsString(propInfo.GetValue(a, null))
                            : CastAide.AsString(propInfo.GetValue(b, null));

                        valueB = isAsc
                            ? CastAide.AsString(propInfo.GetValue(b, null))
                            : CastAide.AsString(propInfo.GetValue(a, null));
                    }

                    return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                }
            };

            dataSource.Sort(compare);
            return dataSource;
        }

        private IEnumerable<TraderPerformanceDetailBO> SortTraderPerformanceReportDetail(string sortField,
            int sortFactor,
            string sortDataType,
            IEnumerable<TraderPerformanceDetailBO> details)
        {
            var listDetail = details.ToList();
            var sortType = sortFactor > 0 ? "asc" : "desc";
            var isNumberSort = sortDataType == "number";
            if (sortField == "UserName")
            {
                listDetail = this.SortByProperty<TraderPerformanceDetailBO>(listDetail,
                    sortType,
                    sortField,
                    isNumberSort);
            }
            else
            {
                var detailLength = listDetail.Count;
                for (int i = 0; i < detailLength; i++)
                {
                    listDetail[i].children = this.SortByProperty<TraderPerformanceDetail>(listDetail[i].children.ToList(),
                        sortType,
                        sortField,
                        isNumberSort);
                }
            }

            return listDetail;
        }
    }
}