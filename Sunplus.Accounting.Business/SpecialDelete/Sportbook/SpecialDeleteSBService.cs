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
    
    public partial class SpecialDeleteService
    {
        #region BettransSelSpecialDel
        public SpecialDeleteDTO BettransSelSpecialDel(SpecialDeleteParam param)
        {
            SpecialDeleteDTO specialDeleteDto = new SpecialDeleteDTO();
            specialDeleteDto.Items = new List<SpecialDeleteByType>();

            specialDeleteDto.ErrorCode = 0;
            specialDeleteDto.ErrorMessage = string.Empty;

            try
            {
                DbResult dbResult = _specialDeleteDao.BettransSelSpecialDel(param);
                if(dbResult != null && dbResult.ResultSet.Tables.Count>0)
                {
                    DataSet dataSet = dbResult.ResultSet;
                    var collections = SpecialDeleteSection.Instance.DeleteBet;

                    int index = 0;
                    foreach (DataTable dataTable in dataSet.Tables)
                    {
                        
                        SpecialDeleteByType deleteByType = new SpecialDeleteByType();
                        List<SpecialDeleteBO> lstByType = new List<SpecialDeleteBO>();

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
                }
                else
                {
                    specialDeleteDto.ErrorCode = 101;
                    specialDeleteDto.ErrorMessage = "There is no information available.";
                }

                bool IsHasData = false;
                dbResult = _specialDeleteDao.SelMatchInfo(CastAide.AsInt32(param.MatchId), CastAide.AsInt64(0));
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    DataRow[] rows = dbResult.ResultSet.Tables[0].Select();
                    foreach (DataRow dataRow in rows)
                    {
                        IsHasData = true;
                        specialDeleteDto.AwayName = CacheRef.GetTeamName(dataRow["awayid"]);
                        specialDeleteDto.HomeName = CacheRef.GetTeamName(dataRow["homeid"]);
                        specialDeleteDto.EventDate = CastAide.AsDateTime(dataRow["eventdate"]).ToShortDateString();
                        specialDeleteDto.MatchCode = CastAide.AsString(dataRow["MatchCode"]);
                    }
                }


                if (!IsHasData)
                {
                    specialDeleteDto.ErrorCode = 100;
                    specialDeleteDto.ErrorMessage = "NO MATCH WITH SUCH MATCH ID";
                }
             
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return specialDeleteDto;
        }
        #endregion
    }
}
