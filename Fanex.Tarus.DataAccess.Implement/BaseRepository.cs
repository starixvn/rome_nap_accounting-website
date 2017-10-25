using Fanex.Common;
using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Sunplus.Accounting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.DataAccess.Implement
{
    public class BaseRepository
    {
        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();
                var outputs = db.GetOuputParams();
                return new Result() {
                    ErrorCode = Converter.ToInt(outputs["errcode"]),
                    ErrorMessage = Converter.ToString2(outputs["errmsg"])
                };
            }
        }
    }
}
