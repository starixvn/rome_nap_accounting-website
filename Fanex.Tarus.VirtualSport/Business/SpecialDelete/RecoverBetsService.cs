
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using Fanex.Tarus.Business.Implement;
using Fanex.Tarus.Infrastructure.Caching;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.VirtualSport.Business.Constants;
using Fanex.Tarus.VirtualSport.DataAccess.SpecialDelete;
using Fanex.Tarus.VirtualSport.Entities;
using Fanex.Tarus.VirtualSport.Entities.SpecialDelete;


namespace Fanex.Tarus.VirtualSport.Business.SpecialDelete
{

    public class RecoverBetsService : BaseSpecialDelete, IRecoverBetsService
    {
        #region IFields

        //
        // GET: /RecoverBetsAccess/
        private readonly IRecoverBetsAccess _recoverBetsAccess;

        #endregion IFields
        #region IConstructions

        public RecoverBetsService()
            : this(null)
        {
        }

        public RecoverBetsService(IRecoverBetsAccess recoverBetsAccess)
        {
            _recoverBetsAccess = recoverBetsAccess ?? new RecoverBetsAccess();
        }

        #endregion IConstructions
        #region IMethods
        #region RecoverBets

        public SpecialDeleteBetInfoList SelRecoverBets(string transid)
        {
            SpecialDeleteBetInfoList specialDeleteDto = new SpecialDeleteBetInfoList();

            try
            {
                DataSet dbResult = _recoverBetsAccess.SelMatchInfo(0, CastAide.AsInt64(transid));
                DataTable dataTable = dbResult.Tables[0];

                specialDeleteDto.EventDate = DateTime.Today.ToShortDateString();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    specialDeleteDto.MatchCode = CastAide.AsString(dataRow["matchcode"]);
                    specialDeleteDto.HomeName = CacheRef.GetTeamName(dataRow["homeid"]);
                    specialDeleteDto.AwayName = CacheRef.GetTeamName(dataRow["awayid"]);
                    specialDeleteDto.EventDate = CastAide.AsDateTime(dataRow["eventdate"]).ToShortDateString();

                    if (specialDeleteDto.HomeName.Length == 0 && specialDeleteDto.AwayName.Length == 0)
                    {
                        specialDeleteDto.HomeName = "Number Game No " + specialDeleteDto.MatchCode;
                    }
                }

                List<BetTransactionInfomation> bets = new List<BetTransactionInfomation>();
                Hashtable outputParam = new Hashtable();
                dbResult = _recoverBetsAccess.SelRecoverSpecialBets(transid, ref outputParam);
                specialDeleteDto.ErrorCode = CastAide.AsInt32(outputParam["errcode"]);
                specialDeleteDto.ErrorMessage = CastAide.AsString(outputParam["errmsg"]);
                if (specialDeleteDto.ErrorCode == 0)
                {
                    if (dbResult.Tables.Count > 0)
                    {
                        dataTable = dbResult.Tables[0];
                        AssignData(dataTable, "RecoverBets", ref bets);
                        specialDeleteDto.TransactionInfo = bets;
                    }
                }
            }
            catch (Exception ex)
            {
                specialDeleteDto.ErrorCode = ErrorCode.SystemError;
                specialDeleteDto.ErrorMessage = ex.Message;
                LogExtensions.ExceptionHandler(ex);
            }

            return specialDeleteDto;
        }

        public Result RecoverBets(string transid)
        {
            Result result = new Result(0, string.Empty);
            try
            {
                if (!PermissionService.HasHostPermission(Enumerations.Functions.SpecialDelete.SpecialDeleteVirtualSports))
                {
                    result.ErrorCode = ErrorCode.NotAuthorized;
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500");
                    return result;
                }
                var userId = AppSession.AccId;
                var userIp = Utils.GetClientIP();
                Hashtable outputParam = new Hashtable();

                DataSet dbResult = _recoverBetsAccess.RecoverSpecialBets(transid, userId, userIp, ref outputParam);
                if (dbResult != null)
                {
                    result.ErrorCode = CastAide.AsInt32(outputParam["errcode"]);
                    result.ErrorMessage = CastAide.AsString(outputParam["errmsg"]);

                    if (result.ErrorCode == 0)
                    {
                        result.ErrorMessage = "Transaction " + transid + " was recovered.";
                        result.ErrorMessage += SendEmailWhenRecoverBet(result, dbResult, true, transid, VirtualSportsName); // real currency
                        SendEmailWhenRecoverBet(result, dbResult, false, transid, VirtualSportsName); //  test currency
                        _recoverBetsAccess.RemovePrivateMsg(transid, userId);
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorCode = ErrorCode.SystemError;
                result.ErrorMessage = e.Message;
            }
            return result;
        }


        #endregion RecoverBets
        #endregion IMethods

    }
}
