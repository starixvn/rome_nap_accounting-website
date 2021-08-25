namespace Fanex.Tarus.Customer.Business.CopySuperSetting
{
    using Fanex.Tarus.Customer.Criteria.CopySuperSetting;
    using Fanex.Tarus.Customer.DataAccess.CopySuperSetting;
    using Fanex.Tarus.Customer.Entities.CopySuperSetting;
    using Fanex.Tarus.Customer.Requests.CopySuperSetting;

    public interface ICopySuperSettingService
    {
        CheckSuperListResult CheckSuperList(CheckSuperListRequest request);
    }

    public class CopySuperSettingService : ICopySuperSettingService
    {
        private readonly ICopySuperSettingDataAccess dataAccess;

        public CopySuperSettingService()
        {
            dataAccess = new CopySuperSettingDataAccess();
        }

        public CheckSuperListResult CheckSuperList(CheckSuperListRequest request)
        {
            var criteria = new CheckSuperListCriteria
            {
                SuperList = request.SuperList,
                Username = request.SourceSuper
            };

            return dataAccess.CheckSuperList(criteria);
        }
    }
}
