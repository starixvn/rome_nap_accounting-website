using System.Collections.Generic;
using DNAide.Core;
using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Criteria.AutoTransfer;
using Fanex.Tarus.Customer.Entities.AutoTransfer;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.Customer.DataAccess.AutoTransfer
{
    public interface IAutoTransferDatalAccess
    {
        bool GetAutoTransferJobStatus(int custId);
        TurnOffAutoTransferResultModel TurnOffAutoTransfer(ICriteria criteria);
        void InsertToastMessage(ICriteria criteria);
        long InsertPersonalMessage(ICriteria criteria);
        void AssignPersonalMessage(long messageId, int custId, string custName);
    }

    public class AutoTransferDatalAccess : IAutoTransferDatalAccess
    {
        public bool GetAutoTransferJobStatus(int custId)
        {
            bool result = false;
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(custId);
            var criteria = new GetAutoTransferJobStatusCriteria();
            var key = siteDB.IsSiteDB ? $"SiteDB{siteDB.SiteDBGroupID}_{criteria.GetSettingKey()}" : criteria.GetSettingKey();

            using (var db = new ObjectDb(key))
            {
                db.ExecuteNonQueryScaleoutDB(criteria.GetSpParams(), siteDB.AGIP);
                result = CastAide.AsBoolean(db.GetParameterValue("IsAutoJobRunning"));
            }

            return result;
        }

        public TurnOffAutoTransferResultModel TurnOffAutoTransfer(ICriteria criteria)
        {
            var result = new TurnOffAutoTransferResultModel
            {
                AffectedDownlines = new List<AffectedDownlinesInfoModel>()
            };

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());

                while (reader.Read())
                {
                    result.AffectedDownlines.Add(new AffectedDownlinesInfoModel
                    {
                        CustId = int.Parse(reader["CustId"].ToString()),
                        Name = reader["Username"].ToString(),
                        Downlines = reader["Names"].ToString(),
                    });
                }
            }

            return result;
        }

        public void InsertToastMessage(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputParams = db.GetOuputParams();
            }
        }

        public long InsertPersonalMessage(ICriteria criteria)
        {
            long result = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());

                while (reader.Read())
                {
                    result = long.Parse(reader["MessageId"].ToString());
                }
            }

            return result;
        }

        public void AssignPersonalMessage(long messageId, int custId, string custName)
        {
            var criteria = new AssignPersonalMessageCriteria(messageId, custId, custName);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }
    }
}
