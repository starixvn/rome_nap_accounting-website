namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;

    public class ViewCommDCSRepository : IViewCommDCSRepository
    {
        public IList<DCSProduct> GetProducts()
        {
            var criteria = new ViewCommDCSCriteria();
            var products = new List<DCSProduct>();

            criteria.SettingType = 3;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        products.Add(new DCSProduct
                        {
                            ProductId = Converter.ToInt(reader["ProductID"]),
                            ProductName = Converter.ToString2(reader["Name"])
                        });
                    }
                }
            }

            criteria.SettingType = 5;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        products.Add(new DCSProduct
                        {
                            ProductId = Converter.ToInt(reader["ProductID"]),
                            ProductName = Converter.ToString2(reader["Name"])
                        });
                    }
                }
            }

            criteria.SettingType = 14;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        products.Add(new DCSProduct
                        {
                            ProductId = Converter.ToInt(reader["ProductID"]),
                            ProductName = Converter.ToString2(reader["Name"])
                        });
                    }
                }
            }

            return products;
        }
    }
}
