using Fanex.BetList.Accounting.Repositories;

namespace Fanex.BetList.Accounting.Services
{
    public class BetListDetailService : IBetListDetailService
    {
        private readonly IBetListRepository _betListRepository;

        public BetListDetailService(IBetListRepository betListRepository = null)
        {
            _betListRepository = betListRepository ?? new BetListRepository();
        }
    }
}
