using System;
using System.ServiceModel.Activation;

/// <summary>
/// Summary description for ReportWL
/// </summary>

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportWL : IReportWL
    {
        #region IReportWL Members

        public string WL2a(string date, int dbtype)
        {
            string result = "";
            /* try
            {
                ReportBase wl = new Report2a();
                result =  wl.RenderHTML(null);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }*/
            return result;
        }

        public string LeagueWL(string date, int dbtype)
        {
            throw new NotImplementedException();
        }

        public string Hello()
        {
            return "get";
        }

        #endregion
    }
}