namespace Fanex.Tarus.Customer.DataAccess.InitializingStakeLevel
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;
    using Fanex.Tarus.Infrastructure.Helper;

    public interface IInitStakeLevelDataAccess
    {
        IEnumerable<BaseModel> GetProducts();

        IEnumerable<BaseModel> GetSportTypes(int productId);

        IEnumerable<BaseModel> GetBettypes(int productId, string sportTypes);

        IEnumerable<BaseModel> GetCurrency(int mode);

        IEnumerable<BaseModel> GetSites(string siteIds);

        StakeLevelResponse GetStakeLevels(StakeLevelSelCriteria criteria);

        ValidateStakeLevelResponse CheckSetting(StakeLevelCheckCriteria criteria);

        Result InsertSetting(StakeLevelInsCriteria criteria);
    }

    public class InitStakeLevelDataAccess : IInitStakeLevelDataAccess
    {
        private const string FormatDisplay = "[{0}] {1}";

        public ValidateStakeLevelResponse CheckSetting(StakeLevelCheckCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.ExecuteReader(criteria.GetSpParams());
                var outputParams = objDb.GetOuputParams();

                return new ValidateStakeLevelResponse
                {
                    StakeLevels = criteria.StakeLevels,
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

        public StakeLevelResponse GetStakeLevels(StakeLevelSelCriteria criteria)
        {
            var stakeLevels = new List<StakeLevelModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var stakeLevel = new StakeLevelModel
                    {
                        SportTypeId = DataUtils.ToInt32(reader["sporttype"]),
                        SportType = DataUtils.ToString(reader["sportname"]),
                        BetTypeId = DataUtils.ToInt32(reader["bettype"]),
                        BetType = DataUtils.ToString(reader["typenamee"]),
                        Site = DataUtils.ToString(reader["site"]),
                        CreditCurrency = DataUtils.ToString(reader["currencycredit"]),
                        DepositCurrency = DataUtils.ToString(reader["currencydeposit"]),
                        NewMinBet = DataUtils.ToDecimal(reader["newminstake"]),
                        NewMaxBet = DataUtils.ToDecimal(reader["newmaxstake"]),
                        MinBet = DataUtils.ToDecimal(reader["curminstake"]),
                        MaxBet = DataUtils.ToDecimal(reader["curmaxstake"]),
                        GameGroupId = DataUtils.ToInt32(reader["gamegroupid"])
                    };

                    stakeLevels.Add(stakeLevel);
                }

                reader.Close();

                var outputParams = objDb.GetOuputParams();

                return new StakeLevelResponse
                {
                    TotalRows = DataUtils.ToInt32(outputParams["totalrows"]),
                    PageSize = criteria.PageSize,
                    PageIndex = criteria.PageIndex,
                    StakeLevels = stakeLevels
                };
            }
        }

        public Result InsertSetting(StakeLevelInsCriteria criteria)
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
