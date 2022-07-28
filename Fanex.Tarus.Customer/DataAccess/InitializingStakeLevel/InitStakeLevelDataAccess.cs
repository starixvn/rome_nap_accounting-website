namespace Fanex.Tarus.Customer.DataAccess.InitializingBetLimit
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingBetLimit;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingBetLimit;
    using Fanex.Tarus.Infrastructure.Helper;

    public interface IInitBetLimitDataAccess
    {
        IEnumerable<BaseModel> GetProducts();

        IEnumerable<BaseModel> GetSportTypes(int productId);

        IEnumerable<BaseModel> GetBettypes(int productId, string sportTypes);

        IEnumerable<BaseModel> GetCurrency(int mode);

        IEnumerable<BaseModel> GetSites(string siteIds);

        BetLimitResponse GetBetLimits(BetLimitSelCriteria criteria);

        ValidateBetLimitResponse CheckSetting(BetLimitCheckCriteria criteria);

        Result InsertSetting(BetLimitInsCriteria criteria);
    }

    public class InitBetLimitDataAccess : IInitBetLimitDataAccess
    {
        private const string FormatDisplay = "[{0}] {1}";

        public ValidateBetLimitResponse CheckSetting(BetLimitCheckCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.ExecuteReader(criteria.GetSpParams());
                var outputParams = objDb.GetOuputParams();

                return new ValidateBetLimitResponse
                {
                    BetLimits = criteria.BetLimits,
                    ProductId = DataUtils.ToInt32(outputParams["productid"]),
                    Error = new Result(DataUtils.ToInt32(outputParams["errcode"]), outputParams["errmsg"].ToString()),
                    Warning = new Result(DataUtils.ToInt32(outputParams["noticode"]), outputParams["notimsg"].ToString()),
                };
            }
        }

        public IEnumerable<BaseModel> GetBettypes(int productId, string sportTypes)
        {
            var criteria = new BetTypeSelCriteria(productId, sportTypes);
            var entities = new List<BaseModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var betypeId = DataUtils.ToInt32(reader["bettype"]);
                    var bettypeName = DataUtils.ToString(reader["typenamee"]);
                    entities.Add(new BaseModel
                    {
                        Id = betypeId,
                        Name = string.Format(FormatDisplay, betypeId, bettypeName)
                    });
                }

                reader.Close();
            }

            return entities;
        }

        public IEnumerable<BaseModel> GetCurrency(int mode)
        {
            var criteria = new CurrencySelCriteria(mode);
            var entities = new List<BaseModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var exchangeID = DataUtils.ToInt32(reader["exchangeid"]);
                    var currency = DataUtils.ToString(reader["currency"]);
                    entities.Add(new BaseModel
                    {
                        Id = exchangeID,
                        Name = string.Format(FormatDisplay, exchangeID, currency)
                    });
                }

                reader.Close();
            }

            return entities;
        }

        public IEnumerable<BaseModel> GetProducts()
        {
            var criteria = new ProductSelCriteria();
            var entities = new List<BaseModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var productId = DataUtils.ToInt32(reader["productid"]);
                    var productName = DataUtils.ToString(reader["productname"]);
                    entities.Add(new BaseModel
                    {
                        Id = productId,
                        Name = string.Format(FormatDisplay, productId, productName)
                    });
                }

                reader.Close();
            }

            return entities;
        }

        public IEnumerable<BaseModel> GetSites(string siteIds)
        {
            var criteria = new SiteSelCriteria(siteIds);
            var entities = new List<BaseModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    entities.Add(new BaseModel
                    {
                        Id = DataUtils.ToInt32(reader["siteid"]),
                        Name = DataUtils.ToString(reader["site"])
                    });
                }

                reader.Close();
            }

            return entities;
        }

        public IEnumerable<BaseModel> GetSportTypes(int productId)
        {
            var criteria = new SportTypeSelCriteria(productId);
            var entities = new List<BaseModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var sportId = DataUtils.ToInt32(reader["sporttype"]);
                    var sportName = DataUtils.ToString(reader["sportname"]);
                    entities.Add(new BaseModel
                    {
                        Id = sportId,
                        Name = string.Format(FormatDisplay, sportId, sportName)
                    });
                }

                reader.Close();
            }

            return entities;
        }

        public BetLimitResponse GetBetLimits(BetLimitSelCriteria criteria)
        {
            var betLimits = new List<BetLimitModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var betLimitModel = new BetLimitModel
                    {
                        SportTypeId = DataUtils.ToInt32(reader["sporttype"]),
                        SportType = DataUtils.ToString(reader["sportname"]),
                        BetTypeId = DataUtils.ToInt32(reader["bettype"]),
                        BetType = DataUtils.ToString(reader["typenamee"]),
                        Site = DataUtils.ToString(reader["site"]),
                        CreditCurrency = DataUtils.ToString(reader["currencycredit"]),
                        DepositCurrency = DataUtils.ToString(reader["currencydeposit"]),
                        MinBet = DataUtils.ToDecimal(reader["curminbet"]),
                        MaxBet = DataUtils.ToDecimal(reader["curmaxbet"]),
                        MaxPerMatch = DataUtils.ToDecimal(reader["curmaxpermatch"]),
                        MaxPerBall = DataUtils.ToDecimal(reader["curmaxperball"]),
                        NewMinBet = DataUtils.ToDecimal(reader["newminbet"]),
                        NewMaxBet = DataUtils.ToDecimal(reader["newmaxbet"]),
                        NewMaxPerMatch = DataUtils.ToDecimal(reader["newmaxpermatch"]),
                        NewMaxPerBall = DataUtils.ToDecimal(reader["newmaxperball"]),
                        GameGroupId = DataUtils.ToInt32(reader["gamegroupid"])
                    };

                    betLimits.Add(betLimitModel);
                }

                reader.Close();

                var outputParams = objDb.GetOuputParams();

                return new BetLimitResponse
                {
                    TotalRows = DataUtils.ToInt32(outputParams["totalrows"]),
                    PageSize = criteria.PageSize,
                    PageIndex = criteria.PageIndex,
                    BetLimits = betLimits
                };
            }
        }

        public Result InsertSetting(BetLimitInsCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.ExecuteReader(criteria.GetSpParams());
                var outputParams = objDb.GetOuputParams();

                return new Result(DataUtils.ToInt32(outputParams["errcode"]), outputParams["errmsg"].ToString());
            }
        }
    }
}
