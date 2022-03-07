namespace Fanex.Tarus.Customer.DataAccess.InitializingStakeLevel
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Infrastructure.Helper;

    public interface IInitStakeLevelDataAccess
    {
        IEnumerable<BaseModel> GetProducts();

        IEnumerable<BaseModel> GetSportTypes(int productId);

        IEnumerable<BaseModel> GetBettypes(int productId, string sportTypes);

        IEnumerable<BaseModel> GetCurrency(int mode);
    }

    public class InitStakeLevelDataAccess : IInitStakeLevelDataAccess
    {
        private const string FormatDisplay = "[{0}] {1}";

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
    }
}
