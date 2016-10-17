namespace Fanex.Tarus.Security.Services
{

    /// <summary>
    /// Interface ISmartPunterPermissionService.
    /// </summary>
    public interface ISmartPunterPermissionService
    {
        /// <summary>
        /// Check punter is smart or not.
        /// </summary>
        /// <param name="custID">Customer identify.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool IsDeniedViewSmartPunter(int? custID, string userName);
    }
}