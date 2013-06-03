// --------------------------------------------------------------------------
// <copyright file="ReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.DataAccess
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using DNAide.Data;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ReportDataAccess
    {
        private static StoreProcedureElement GetReportStoreProcedure(ReportSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        public static class BalanceUp
        {
            private static DateTime _balanceUpDay;
            private static DateTime _moveBetsDay;

            private static ICacheManager _cache;

            static BalanceUp()
            {
                _cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            }

            private static void GetData()
            {
                if (_cache.GetData("BalanceUpDay") == null || _cache.GetData("MoveBetsDay") == null)
                {
                    DbResult dr = Age_BalDaySel();
                    DataRow r = dr.ResultSet.Tables[0].Rows[0];
                    _balanceUpDay = CastAide.AsDateTime(r["BalanceUpDay"]);
                    _moveBetsDay = CastAide.AsDateTime(r["MoveBetsDay"]);

                    _cache.Add("BalanceUpDay", _balanceUpDay, CacheItemPriority.None, null, new AbsoluteTime(TimeSpan.FromHours(2)));
                    _cache.Add("MoveBetsDay", _moveBetsDay, CacheItemPriority.None, null, new AbsoluteTime(TimeSpan.FromHours(2)));
                }
                else
                {
                    _balanceUpDay = CastAide.AsDateTime(_cache.GetData("BalanceUpDay"));
                    _moveBetsDay = CastAide.AsDateTime(_cache.GetData("MoveBetsDay"));
                }
            }

            public static DateTime BalanceUpDay
            {
                get
                {
                    GetData();
                    return _balanceUpDay;
                }
            }

            public static DateTime MoveBetsDay
            {
                get
                {
                    GetData();
                    return _moveBetsDay;
                }
            }

            private static DbResult Age_BalDaySel()
            {
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Others, "SelBalanceUpDay");
                return DbHelper.Execute(spElement.Name, spElement.DbName);
            }
        }
    }
}
