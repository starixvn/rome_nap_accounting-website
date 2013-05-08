using System.Collections.Generic;
using Sunplus.Accounting.Entities.CreditTerm;

namespace Sunplus.Accounting.Business.CreditTerm
{
    public interface ICreditTermManager
    {
        List<Group> GetAllGroup();
    }
}