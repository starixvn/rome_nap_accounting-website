namespace Sunplus.Accounting.DataAccess
{
    using DNAide.Data;

    /// <summary>
    /// Summary description for IUserDAO
    /// </summary>
    public interface IExchangeRateDAO
    {
        DbResult ExChangeRateUpdateBaseRate(string currency, string baserate, string minbet, string minbetlive);

        DbResult ExChangeRateUpdateOpenTote(string currency, string GBPRate, string AUDRate, string ZARRate, string EURORate, string USDRate);
    }
}