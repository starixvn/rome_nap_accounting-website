// -----------------------------------------------------------------------
// <copyright file="RacingDefaultSettingAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.DataAccess.RacingDefaultSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Admin.Entities;
    using System.Data;
    using Fanex.Data;
    using DNAide.Core;
    using System.Collections;
    using Fanex.Tarus.Infrastructure.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RacingDefaultSettingAccess : IRacingDefaultSettingAccess
    {
        public DataSet SelRacingDefaultSetting(int SportType, int Bettype, string Currency)
        {
            var dsResult = new DataSet();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_RacingBetSettingSel"))
                {
                    var param = new Hashtable();
                    if (Currency != "All")
                    {
                        param.Add("currency", Currency);
                    }
                    if (SportType != -1)
                    {
                        param.Add("sporttype", CastAide.AsInt32(SportType));
                    }
                    if (Bettype != -1)
                    {
                        param.Add("bettype", (byte)(Bettype));
                    }

                    objDB.AssignParamValues(param);
                    dsResult = objDB.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dsResult;
        }

       public  Result UpdateRacingDefaulSetting(int UserID, int Sporttype, int Bettype, int CurrencyID, int MinBet)
        {
            var result = new Result();
            try
            {
                using (var dbContext = new ObjectDb("Acc_RacingBetSettingUpd"))
                {
                    dbContext.AddInParameter("userid", DbType.Int32, UserID);
                    dbContext.AddInParameter("sporttype", DbType.Int32, Sporttype);
                    dbContext.AddInParameter("bettype", DbType.Int32, Bettype);
                    dbContext.AddInParameter("currency", DbType.Int32, CurrencyID);
                    dbContext.AddInParameter("minbet", DbType.Int32, MinBet);
                    dbContext.AddOutParameter("ErrCode", DbType.Int32, sizeof(int));
                    dbContext.AddOutParameter("ErrMsg", DbType.String, 1000);
                    dbContext.ExecuteNonQuery();
                    result.ErrorMessage = CastAide.AsString(dbContext.GetParameterValue("ErrMsg"));
                    result.ErrorCode = CastAide.AsInt32(dbContext.GetParameterValue("ErrCode"));
                }
            }

            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}
