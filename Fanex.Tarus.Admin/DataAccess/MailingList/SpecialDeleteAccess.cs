// -----------------------------------------------------------------------
// <copyright file="SpecialDeleteAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.DataAccess.MailingList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data;
    using System.Data;
    using Sunplus.Accounting.Core.Extensions;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Admin.Entities.MailingList;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SpecialDeleteAccess : ISpecialDeleteAccess
    {
        public DataTable GetMailingList()
        {
            DataTable datatable = null;
            try
            {
                using (var dbContext = new ObjectDb("Acc_MailingListSel"))
                {
                    var ds = dbContext.ExecuteDataSet();
                    if (ds.Tables.Count > 0)
                        datatable = ds.Tables[0];
                }
            }

            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return datatable;
        }

        /// <summary>
        /// Update insert mailing 
        /// </summary>
        /// <param name="?"></param>
        /// <param name="isUpdate">true is update, false is insert</param>
        /// <returns></returns>
        public Result UpdateInsertMailing(MailLicensee mailLicensee, bool isUpdate)
        {
            try
            {
                var sp = isUpdate ? "Acc_MailingListUpd" : "Acc_MailingListIns";
                using (ObjectDb db = new ObjectDb(sp))
                {
                    db.AddInParameter("userid", DbType.Int32, AppSession.AccId);
                    db.AddInParameter("siteid", DbType.Int32, mailLicensee.SiteID);
                    db.AddInParameter("tolist", DbType.String, string.IsNullOrEmpty(mailLicensee.ToList) ? string.Empty : mailLicensee.ToList);
                    db.AddInParameter("cclist", DbType.String, string.IsNullOrEmpty(mailLicensee.CCList) ? string.Empty : mailLicensee.CCList);
                    db.AddInParameter("bcclist", DbType.String, string.IsNullOrEmpty(mailLicensee.BCCList) ? string.Empty : mailLicensee.BCCList);
                    db.AddOutParameter("errmsg", DbType.String, 500);
                    db.AddOutParameter("errcode", DbType.Int32, sizeof(int));

                    db.ExecuteNonQuery();

                    var errMsg = DNAide.Core.CastAide.AsString(db.GetParameterValue("ErrMsg"));
                    var errCode = DNAide.Core.CastAide.AsInt32(db.GetParameterValue("errcode"));
                    return new Result { ErrorCode = errCode, ErrorMessage = errMsg };
                }
            }
            catch (Exception e)
            {
                return new Result { ErrorCode = 1, ErrorMessage = e.Message };
            }
        }
    }
}
