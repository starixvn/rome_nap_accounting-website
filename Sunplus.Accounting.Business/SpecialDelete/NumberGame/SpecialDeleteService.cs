using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.SpecialDelete;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.SpecialDelete;
using System.Data;
using DNAide.Core;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Extensions;

namespace Sunplus.Accounting.Business.SpecialDelete.NumberGame
{
    
    public partial class SpecialDeleteService : SpecialDeleteBase, ISpecialDeleteService 
    {
        private readonly ISpecialDeleteDAO _specialDeleteDao ;


        public SpecialDeleteService(ISpecialDeleteDAO specialDeleteDao)
        {
            _specialDeleteDao = specialDeleteDao;
        }


        #region SelSpecialDeleteNumberGame
        public SpecialDeleteDTO SelSpecialDeleteNumberGame(int matchId, long fromRefNo, long toRefNo, string live, string dead, string userName)
        {
            DbResult dbResult = _specialDeleteDao.SelSpecialDeleteNumberGame(matchId, fromRefNo, toRefNo, live, dead, userName);
            SpecialDeleteDTO specialDeleteDto = new SpecialDeleteDTO();
            specialDeleteDto.Items = new List<SpecialDeleteByType>();
            // business here

            DataSet dataSet = dbResult.ResultSet;
            int index = 0;
            List<SpecialDeleteBO> list = new List<SpecialDeleteBO>();
          
            foreach (DataTable dataTable in dataSet.Tables)
            {

                SpecialDeleteByType deleteByType = new SpecialDeleteByType();
                List<SpecialDeleteBO> lstByType = new List<SpecialDeleteBO>();

                var collections = SpecialDeleteSection.Instance.DeleteBet;
                SpecialDeleteElement element = collections[index];

                string type = element.Name;

                AssignData(dataTable, type, ref lstByType);

                deleteByType.Type = type;
                deleteByType.Items = new List<SpecialDeleteBO>();
                deleteByType.Items = lstByType;
                deleteByType.IsAllowOptionDelete = element.IsAllowOptionDelete == "1";
                deleteByType.OptionType = element.DeleteoOtionDefault;

                specialDeleteDto.Items.Add(deleteByType);

                index++;
            }

            specialDeleteDto.rows = list;
            specialDeleteDto.AwayName = string.Empty;
            specialDeleteDto.HomeName = string.Empty;
            specialDeleteDto.EventDate = DateTime.Today.ToShortDateString();
            specialDeleteDto.MatchCode = string.Empty;

            dbResult = _specialDeleteDao.SelMatchInfo(matchId, 0);
            bool IsHasData = false;
            foreach (DataRow dataRow in dbResult.ResultSet.Tables[0].Rows)
            {
                IsHasData = true;
                specialDeleteDto.AwayName = CacheRef.GetTeamName(dataRow["homeid"]);
                specialDeleteDto.HomeName = CacheRef.GetTeamName(dataRow["awayid"]);
                specialDeleteDto.EventDate = CastAide.AsDateTime(dataRow["eventdate"]).ToShortDateString();
                specialDeleteDto.MatchCode = CastAide.AsString(dataRow["MatchCode"]);
            }

            if (!IsHasData)
            {
                specialDeleteDto.ErrorCode = 100;
                specialDeleteDto.ErrorMessage = "Invalid MatchId";
            }
            else
            {
                specialDeleteDto.ErrorCode = 0;
                specialDeleteDto.ErrorMessage = string.Empty;
            }
            return specialDeleteDto;
        }
        #endregion
        
        

        #region DeleteSpecialTransactionNumberGame
        public Result DeleteSpecialTransactionNumberGame(int matchId, long frefno, long trefno, string live, string dead, int DelOption, string transids, int DelOption_BD, string transids_BD, int DelOption_12EU, string transids_12EU, int DelOption_MHC, string transids_MHC, Message message)
        {
            Result result = new Result(0, string.Empty);
            if (!Permissions.GetHostPermission(Enumerations.Functions.SpecialDelete.SpecialDeleteNumberGame))
            {
                result.ErrorCode = 500;
                result.ErrorMessage = result.ErrorMessage = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"), AlertMessage.MsgType.Error); 
                return result;
            }
            string username = message.UserName;
            int custId = message.UserId;
            string userpwd =  DNAide.Utils.Encryption.CfsEnCode(message.userPwd);
            string ip = Utils.GetClientIP();


            var delResult = _specialDeleteDao.DeleteSpecialTransactionNumberGame(matchId, frefno, trefno, live, dead,
                                                                                     username, custId, userpwd,
                                                                                     DelOption, transids,
                                                                                     DelOption_BD, transids_BD,
                                                                                     DelOption_12EU, transids_12EU, DelOption_MHC, transids_MHC, ip);
            
            if(delResult.Error.ErrorCode > 0)
            {
                result.ErrorCode = delResult.Error.ErrorCode;
                result.ErrorMessage = AlertMessage.GetMsg(delResult.Error.ErrorMessage, AlertMessage.MsgType.Error);
            }
            else
            {
                //send private message to users
                bool f = SendPrivateMsgToUsers(delResult.PrivateMessage, matchId, message);
                if (!f)
                {
                    result.ErrorCode = 100;
                    result.ErrorMessage = AlertMessage.GetMsg("Have a problem when sending private message to users. Please infom to IT about this.", AlertMessage.MsgType.Error);
                }
                else
                {
                    result.ErrorCode = 0;
                    result.ErrorMessage = SendEmailToCSD(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, true);

                    SendEmailWhenDeleteBetToInternalGroup(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, true, false, true); // real currency
                    SendEmailWhenDeleteBetToInternalGroup(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, true, false, false); // test currency
                }
            }
            return result;
        }
        #endregion




       
    }
}
