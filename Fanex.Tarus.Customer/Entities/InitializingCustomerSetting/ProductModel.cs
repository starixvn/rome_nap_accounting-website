namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class ProductModel
    {
        public ProductModel(int productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
            Sports = new List<SportModel>();
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public List<SportModel> Sports { get; set; }
    }
}
