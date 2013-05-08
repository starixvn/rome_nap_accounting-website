//using Sunplus.Core.LoggingLib;

namespace Fanex.Tarus.Finance.Infrastructure.Configurations
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Summary description for Settings
    /// </summary>
    public static class AppSettings
    {



        public static string CrossBaseCurrency { get; set; }

        static AppSettings()
        {
            try
            {

                CrossBaseCurrency = ConfigurationManager.AppSettings["Fanex.Tarus.Finance.CrossBaseCurrency"];

            }
            catch (Exception ex)
            {
                
                {
                    //.ExceptionHandler(ex);
                }

                
            }
        }


        
    }
}
