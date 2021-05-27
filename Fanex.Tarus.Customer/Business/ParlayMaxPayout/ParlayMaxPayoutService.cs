namespace Fanex.Tarus.Customer.Business.ParlayMaxPayout
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.ParlayMaxPayout;
    using Fanex.Tarus.Customer.DataAccess.ParlayMaxPayout;
    using Fanex.Tarus.Customer.Requests.ParlayMaxPayout;
    using Fanex.Tarus.Customer.ViewModels.ParlayMaxPayout;
    using Sunplus.Accounting.Core.Configurations;
    using System.Collections.Generic;

    public interface IParlayMaxPayoutService
    {
        ParlayMaxPayoutViewModel Select(ParlayMaxPayoutSelectRequest request);

        Result Update(ParlayMaxPayoutUpdateRequest request);
    }

    public class ParlayMaxPayoutService : IParlayMaxPayoutService
    {
        private readonly IParlayMaxPayoutDataAccess _parlayMaxPayoutDataAccess;

        public ParlayMaxPayoutService() : this(null) { }

        public ParlayMaxPayoutService(IParlayMaxPayoutDataAccess parlayMaxPayoutDataAccess)
        {
            _parlayMaxPayoutDataAccess = parlayMaxPayoutDataAccess ?? new ParlayMaxPayoutDataAccess();
        }

        public ParlayMaxPayoutViewModel Select(ParlayMaxPayoutSelectRequest request)
            => new ParlayMaxPayoutViewModel
            {
                CustId = request.CustId,
                CustName = request.CustName,
                ListEntityModel = _parlayMaxPayoutDataAccess.Select(new MaxPayoutSelectCriteria { CustId = request.CustId, SettingStatus = WebSettings.SettingStatus })
            };

        public Result Update(ParlayMaxPayoutUpdateRequest request) => _parlayMaxPayoutDataAccess.Update(new MaxPayoutUpdateCriteria(request));
    }
}
