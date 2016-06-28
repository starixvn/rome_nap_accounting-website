// --------------------------------------------------------------------------
// <copyright file="LimitNewMemberAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.Tarus.Admin.DataAccess.LimitNewMember
{
    using System;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Admin.Criteria.LimitNewMember;
    using Fanex.Tarus.Admin.Entities;

    /// <summary>
    /// LimitNewMemberAccess class.
    /// </summary>
    public class LimitNewMemberAccess : ILimitNewMemberAccess
    {
        /// <summary>
        /// Gets the limit new member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The limit member object.</returns>
        public LimitMember GetLimitNewMember(LimitNewMemberCriteria criteria)
        {
            LimitMember limitSuper = null;
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AddInParameter("username", DbType.String, criteria.Super);
                db.AddInParameter("mode", DbType.String, criteria.Mode);
                db.AddInParameter("creator", DbType.Int32, criteria.Creator);
                db.AddInParameter("language", DbType.String, criteria.Language);
                db.AddInParameter("ip", DbType.String, criteria.IP);

                db.AddOutParameter("limit", DbType.Int32, 4);
                db.AddOutParameter("errcode", DbType.Int32, 4);
                db.AddOutParameter("errmsg", DbType.String, 4000);

                int effectedRows = db.ExecuteNonQuery();
                var outputResult = db.GetOuputParams();
                if (outputResult != null)
                {
                    limitSuper = new LimitMember
                    {
                        Super = criteria.Super,
                        MaxPerDay = Converter.ToInt(outputResult["limit"]),
                        OutputResult = new Result 
                        {
                            ErrorCode = Converter.ToInt(outputResult["errcode"]),
                            ErrorMessage = Converter.ToString2(outputResult["errmsg"]),
                        }
                    };   
                }
            }

            return limitSuper;
        }

        /// <summary>
        /// Updates the limit new member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        public Result UpdateLimitNewMember(LimitNewMemberCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AddInParameter("username", DbType.String, criteria.Super);
                db.AddInParameter("mode", DbType.String, criteria.Mode);
                db.AddInParameter("creator", DbType.Int32, criteria.Creator);
                db.AddInParameter("language", DbType.String, criteria.Language);
                db.AddInParameter("ip", DbType.String, criteria.IP);
                db.AddInParameter("limit", DbType.Int32, criteria.Limit);

                db.AddOutParameter("errcode", DbType.Int32, 4);
                db.AddOutParameter("errmsg", DbType.String, 4000);

                int effectedRows = db.ExecuteNonQuery();
                var outputResult = db.GetOuputParams();
                return outputResult == null
                    ? null
                    : new Result
                    {
                        ErrorCode = Converter.ToInt(outputResult["errcode"]),
                        ErrorMessage = Converter.ToString2(outputResult["errmsg"]),
                    };
            }
        }
    }
}
