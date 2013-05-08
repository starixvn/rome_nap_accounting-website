using System.Collections.Generic;

namespace Sunplus.Accounting.Services.ReportServices
{
    /// <summary>
    /// Summary description for IWinLossService
    /// </summary>
    public interface IWinLossService<T>
    {
        void AppendTotalEntity(ref List<T> list);
    }
}