using System.Web;

namespace Sunplus.Accounting.Business.PositionTaking.SportBook
{
    /// <summary>
    /// Summary description for PtService
    /// </summary>
    public interface IPtService
    {
        string Insert(HttpRequest httpRequest);

        string Update(HttpRequest httpRequest);

        string Select(PtManager.Level level, PtManager.PtType ptType);
    }
}