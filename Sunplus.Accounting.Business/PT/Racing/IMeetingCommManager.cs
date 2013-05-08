using System.Collections.Generic;
using Sunplus.Accounting.Entities.PositionTaking.Racing;

namespace Sunplus.Accounting.Business.PT.Racing
{
    public interface IMeetingCommManager
    {
        List<Partner> GetPartnerList();

        List<PartnerItem> GetPartnerDetail(int partnerId);
    }
}