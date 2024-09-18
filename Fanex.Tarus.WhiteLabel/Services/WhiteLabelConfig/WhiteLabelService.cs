using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig;
using Fanex.Tarus.WhiteLabel.Models;
using Fanex.Tarus.WhiteLabel.Models.WhiteLabelConfig;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Tarus.WhiteLabel.Services.WhiteLabelConfig
{
    public interface IWhiteLabelService
    {
        IEnumerable<SelectListItem> GetWhiteLabelInfo();

        Result Check(CheckWhiteLabelSettingCriteria criteria);

        IEnumerable<WhiteLabelViewModel> Search(GetWhiteLabelSettingCriteria criteria);

        WhiteLabelViewModel GetAgentInfo(GetAgentInfoCriteria criteria);

        Result Update(EditWhiteLabelSettingCriteria criteria);

        Result Add(AddWhiteLabelSettingCriteria criteria);
    }

    public class WhiteLabelService : IWhiteLabelService
    {
        private readonly IDynamicRepository dynamicRepository;

        public WhiteLabelService() : this(null)
        { }

        public WhiteLabelService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public IEnumerable<SelectListItem> GetWhiteLabelInfo()
        {
            yield return new SelectListItem
            {
                Text = "All",
                Value = string.Empty
            };
            using (var objDb = new ObjectDb("Acc_WhiteLabelInfo_Sel"))
            {
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    yield return new SelectListItem {
                        Text = reader["WhiteLabel"].ToString(),
                        Value = reader["CustId"].ToString()
                    };
                }
            }
        }
        public Result Check(CheckWhiteLabelSettingCriteria criteria)
        {
            using (var objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objDB.AssignParamValues(criteria.GetSpParams());
                objDB.ExecuteNonQuery();
                var outputs = objDB.GetOuputParams();
                return new Result(DataUtils.ToInt32(outputs["errcode"]), DataUtils.ToString(outputs["errmsg"]), outputs);
            }
        }

        public IEnumerable<WhiteLabelViewModel> Search(GetWhiteLabelSettingCriteria criteria)
            => dynamicRepository.Fetch<WhiteLabelViewModel>(criteria);

        public WhiteLabelViewModel GetAgentInfo(GetAgentInfoCriteria criteria)
            => dynamicRepository.Fetch<WhiteLabelViewModel>(criteria).FirstOrDefault();

        public Result Update(EditWhiteLabelSettingCriteria criteria) => ExecuteNonQuery(criteria);

        public Result Add(AddWhiteLabelSettingCriteria criteria) => ExecuteNonQuery(criteria);

        private Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objDB.AssignParamValues(criteria.GetSpParams());
                objDB.ExecuteNonQuery();
                var outputs = objDB.GetOuputParams();
                return new Result(DataUtils.ToInt32(outputs["errcode"]), DataUtils.ToString(outputs["errmsg"]));
            }
        }
    }
}
