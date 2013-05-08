// -----------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Core
{
    using System;
    using Fanex.Tarus.Reports.DataAccess;
    using System.Data;
    using DNAide.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSession
    {
        public static DateTime BalanceUpDay
        {
            get 
            {
                var balanceUpDay = System.Web.HttpContext.Current.Session["BalanceUpDay"];
                if (balanceUpDay == null)
                { 
                    DataTable dt = BaseDataAccess.SelBalanceUpDay();
                    if(dt != null && dt.Rows.Count>0)
                    {
                        balanceUpDay = CastAide.AsDateTime(dt.Rows[0]["BalanceUpDay"]);
                        System.Web.HttpContext.Current.Session["BalanceUpDay"] = balanceUpDay;
                    }                     
                }
                return CastAide.AsDateTime(balanceUpDay);
            }
            set 
            {
                System.Web.HttpContext.Current.Session["BalanceUpDay"] = value;
            }
        }

        public static DateTime MoveBetsDay 
        {
            get
            {
                var moveBetsDay = System.Web.HttpContext.Current.Session["MoveBetsDay"] ;
                if (moveBetsDay == null)
                {
                    DataTable dt = BaseDataAccess.SelBalanceUpDay();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        moveBetsDay = CastAide.AsDateTime(dt.Rows[0]["MoveBetsDay"]);
                        System.Web.HttpContext.Current.Session["MoveBetsDay"] = moveBetsDay; 
                    }  
                }
                return CastAide.AsDateTime(moveBetsDay);
            }
            set
            {
                System.Web.HttpContext.Current.Session["MoveBetsDay"] = value;
            }
        }   
    }
}
