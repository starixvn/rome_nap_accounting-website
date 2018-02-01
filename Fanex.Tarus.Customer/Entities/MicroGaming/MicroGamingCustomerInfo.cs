namespace Fanex.Tarus.Customer.Entities.MicroGaming
{
    using System.Collections.Generic;

    public class MicroGamingCustomerInfo
    {
        public SelectResult SelectResult { get; set; }

        public int CustomerId { get; set; }

        public int RoleId { get; set; }

        public int ParentId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public List<GameTypeModel> GameTypeList { get; set; }

        public string GetSettingKey()
        {
            return "Acc_CustomerUpdAGCasinoSetting";
        }
    }
}
