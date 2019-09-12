namespace Fanex.Tarus.CN88.Models.PositionTaking
{
    using System.Collections.Generic;

    public class LineCommModel
    {
        public LineCommModel()
        {
            Customers = new List<CustInfoModel>();
            CustComm = new List<CustomerCommModel>();
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public int RoleId { get; set; }

        public List<CustInfoModel> Customers { get; set; }

        public List<CustomerCommModel> CustComm { get; set; }
    }
}
