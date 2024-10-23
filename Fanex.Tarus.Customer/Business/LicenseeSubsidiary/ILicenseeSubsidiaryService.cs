using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.Business.LicenseeSubsidiary
{
    internal interface ILicenseeSubsidiaryService
    {
        IEnumerable<string> Generate(string username, int number);

        Result Create(string username, string existedUsername, int userId, string ip);
    }
}