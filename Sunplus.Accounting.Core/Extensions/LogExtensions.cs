using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Common;



namespace Sunplus.Accounting.Core.Extensions
{
    using System.Web;

    /**
     * public class Log Extensions
     * @Author: bobby.nguyen
     * @Date: 11012011
     **/
    public static class LogExtensions
    {
        private static BackgroundWorker _worker = null;
        private static Queue<Exception> _queueEx = new Queue<Exception>();
        /**
         * Function: Exception Handler
         */
        public static void ExceptionHandler(Exception ex)
        {
            string timeFrom = "", timeTo = "";

            //var umCtrl = new UMBusiness();

            //var isDownTime = umCtrl.isUnderMaintainence(Sites.AccountSite, WebSettings.LbServiceUrl, ref timeFrom,
            //                                            ref timeTo); //Get Agnet Site UM Status.
            //if (!isDownTime)
                Fanex.Logging.Logger.Log.Error(ex.Message, ex);
        }

        /**
         * Function: BackgroundWorker running
         */
        private static void WorkerRunning(object sender, DoWorkEventArgs e)
        {
            if (_queueEx.Count > 0)
            {
                var ex = _queueEx.Dequeue();
                
                Fanex.Logging.Logger.Log.Error(ex.Message, ex);
            }
            else
            {
                _worker.CancelAsync();
            }
        }
        /**
         * Function: BackgroundWorker commpleted
         */
        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO
        }

        private static string GetName<T>(T item) where T : class
        {
            var properties = typeof(T).GetProperties();
            //Enforce.That(properties.Length == 1);
            return properties[0].Name;
        }
    }
}
