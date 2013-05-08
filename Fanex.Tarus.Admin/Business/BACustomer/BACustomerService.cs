// -----------------------------------------------------------------------
// <copyright file="BACustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Business.BACustomer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Admin.Entities;
using Fanex.Tarus.Admin.DataAccess.BACustomer;
    using System.Data;
    using DNAide.Core;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Common;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BACustomerService: IBACustomerService
    {

        private readonly IBACustomerAccess _baCustomerAccess;
        private string _keySelBACustomer = AppSession.Username + ".SelBACustomer";

        public BACustomerService():this(null)
        {
            
        }

        public BACustomerService(IBACustomerAccess baCustomerAccess)
        {
            _baCustomerAccess = baCustomerAccess ?? new BACustomerAccess();
        }

        public List<Licensee> GetLicenseeBA()
        {
            var licenseeList = new List<Licensee>();
            try
            {
                var datatable = _baCustomerAccess.GetLicensee();
                foreach (DataRow row in datatable.Rows)
                {
                    if (CastAide.AsBoolean(row["EnableBA"]))
                    {
                        licenseeList.Add(new Licensee()
                        {
                            LicenseeID = CastAide.AsInt32(row["licenseeId"]),
                            LicenseeName = CastAide.AsString(row["licenseeName"]),
                            EnableBA = CastAide.AsBoolean(row["enableBA"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return licenseeList;
        }


        public List<FollowedCustomer> SelBACustomerListFromDB(int LicenseeId, int Status, string Username)
        {
            
            var followedCustomerList = new List<FollowedCustomer>();
            try
            {
                var datatable = _baCustomerAccess.GetBACustomerList(LicenseeId, Status, Username);
                foreach (DataRow row in datatable.Rows)
                {
                    followedCustomerList.Add(new FollowedCustomer { 
                         CustId = CastAide.AsString(row["custid"]),
                         Username = row["username"].ToString(),
                         IsFollowed = CastAide.AsBoolean(row["isfollowed"]),
                         Licensee = CastAide.AsString(row["licenseename"])
                    });
                }
               
                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelBACustomer, followedCustomerList);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return followedCustomerList;
        }

        public FollowedCustomerDTO SelBACustomerList(int LicenseeId, int Status, string Username, int pageSize, int pageIndex, int fromCache)
        {
            var followedCusPaging = new FollowedCustomerDTO();
            var followedCustomerList = new List<FollowedCustomer>();
            var getObj = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keySelBACustomer);
            var FollowedCust = new List<FollowedCustomer>();
            if (getObj == null || fromCache == 0)
                followedCustomerList = SelBACustomerListFromDB(LicenseeId, Status, Username);
            else
                followedCustomerList = (List<FollowedCustomer>)getObj;
            followedCusPaging.FollowedCus = followedCustomerList.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            followedCusPaging.TotalCustomer = followedCustomerList.Count;
            return followedCusPaging;
        }


        public Result InsBACustomerList(string strCustId, string Rate)
        {
            var result = new Result();
            result = _baCustomerAccess.InsBACustomerList(strCustId, Rate, AppSession.AccId, HttpContext.Current.Request.UserHostAddress);
            return result;
        }
    }
}
