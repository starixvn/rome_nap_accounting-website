namespace Fanex.Tarus.Admin.Business.IMBCRestriction
{
    using Fanex.Tarus.Admin.DataAccess.IMBCRestriction;

    public class IMBCAdminService
    {
        private readonly IMBCDAO _dao = new IMBCDAO();

        public bool IMBCRestrictionStatus()
        {
            return _dao.IMBCRestrictionStatus();
        }

        public bool UpdateIMBCRestrictionStatus(bool isAllowed)
        {
            return _dao.UpdateIMBCRestrictionStatus(isAllowed);
        }
    }
}
