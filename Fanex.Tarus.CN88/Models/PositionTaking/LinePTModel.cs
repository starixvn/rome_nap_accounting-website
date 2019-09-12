namespace Fanex.Tarus.CN88.Models.PositionTaking
{
    using System.Collections.Generic;
    using Fanex.Tarus.CN88.Helpers;

    public class LinePTModel
    {
        public LinePTModel()
        {
            Customers = new List<CustInfoModel>();
            CustPTs = new List<CustomerPTModel>();
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string SportsbookMinPTValue { get; set; }

        public string SportsbookMaxPTValue { get; set; }

        public string NonsportsMinPTValue { get; set; }

        public string NonsportsMaxPTValue { get; set; }

        public string SportsbookMinPT2Value { get; set; }

        public string SportsbookMaxPT2Value { get; set; }

        public string NonsportsMinPT2Value { get; set; }

        public string NonsportsMaxPT2Value { get; set; }

        public int RoleId { get; set; }

        public List<CustInfoModel> Customers { get; set; }

        public List<CustomerPTModel> CustPTs { get; set; }

        public bool IsSportsbookChanged =>
            PositionTakingHelper.IsUpdatePTTime() &&
            (SportsbookMinPTValue != SportsbookMinPT2Value || SportsbookMaxPTValue != SportsbookMaxPT2Value);

        public bool IsNonSportChanged =>
            PositionTakingHelper.IsUpdatePTTime() &&
            (NonsportsMinPTValue != NonsportsMinPT2Value || NonsportsMaxPTValue != NonsportsMaxPT2Value);
    }
}
