using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Data;
using Fanex.Logging.Extensions.Web;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities.ExtraPtGroup;

namespace Fanex.Tarus.Customer.DataAccess.ExtraPtGroup
{
    public class ExtraPtRepository : IExtraPtRepository
    {
        public IList<GroupByProduct> GetGroupsByProduct(CriteriaBase criteria)
        {
            var result = new List<GroupByProduct>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new GroupByProduct
                            {
                                EffectiveDate = reader.ToDateTime("EffectiveDate"),
                                EndDate = reader.ToDateTime("EndDate"),
                                ExtraPt = reader.ToDecimal("ExtraPT"),
                                Group = new Group { GroupId = reader.ToByte("GroupId"), GroupName = reader.ToString("GroupName") },
                                Product = new Product { ProductId = reader.ToByte("ProductId"), ProductName = reader.ToString("ProductName") },
                                IsAllowDeleted = reader.ToBoolean("IsAllowDeleted")
                            });
                        }
                    }
                }
            }
            
            return result;
        }

        public IList<Group> GetGroups(CriteriaBase criteria)
        {
            var result = new List<Group>();
 
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Group
                            {
                                GroupId = reader.ToByte("GroupId"),
                                GroupName = reader.ToString("GroupName")
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<Product> GetProducts(CriteriaBase criteria)
        {
            var result = new List<Product>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Product
                            {
                                ProductId = reader.ToByte("ProductId"),
                                ProductName = reader.ToString("ProductName")
                            });
                        }
                    }
                }
            }

            return result;
        }

        public Result InsertGroupByProduct(CriteriaBase criteria)
        {
            var result = new Result();
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    result.ErrorCode = Utils.ToInt32(db.GetParameterValue("errcode"));
                    result.ErrorMessage = Utils.ToString(db.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, TraceEventType.Information);
                result = new Result(999, ex.Message);
            }

            return result;
        }

        public Result UpdateGroupByProduct(CriteriaBase criteria)
        {
            var result = new Result();
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    result.ErrorCode = Utils.ToInt32(db.GetParameterValue("errcode"));
                    result.ErrorMessage = Utils.ToString(db.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, TraceEventType.Information);
                result = new Result(999, ex.Message);
            }

            return result;
        }

        public Result DeleteGroupByProduct(CriteriaBase criteria)
        {
            var result = new Result();
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    result.ErrorCode = Utils.ToInt32(db.GetParameterValue("errcode"));
                    result.ErrorMessage = Utils.ToString(db.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, TraceEventType.Information);
                result = new Result(999, ex.Message);
            }

            return result;
        }
    }
}
