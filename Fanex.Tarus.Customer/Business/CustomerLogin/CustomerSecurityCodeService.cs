// --------------------------------------------------------------------------
// <copyright file="CustomerSecurityCodeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.CustomerLogin
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using DNAide.Core;
    using Fanex.Security;
    using Fanex.Tarus.Customer.Criteria.CustomerLogin;
    using Fanex.Tarus.Customer.DataAccess.CustomerLogin;
    using Fanex.Tarus.Customer.Entities;
    using IBCUtils;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CustomerSecurityCodeService : ICustomerSecurityCodeService
    {
        private readonly ICustomerSecurityCodeDataAccess _reportRepository;

        public CustomerSecurityCodeService()
            : this(null)
        {
        }

        public CustomerSecurityCodeService(ICustomerSecurityCodeDataAccess reportRepository)
        {
            this._reportRepository = reportRepository ?? new CustomerSecurityCodeDataAccess();
        }

        public Result ResetSecurityCode(ResetSecurityCodeCriteria criteria)
        {
            try
            {
                criteria.IP = HttpContext.Current.Request.UserHostAddress;
                criteria.Userid = AppSession.AccId;
                var plainSecurityCode = criteria.Securitycode;
                if (criteria.RoleId == 1) // member
                {
                    criteria.Securitycode = Encryption.CfsEnCode(plainSecurityCode);
                }
                else
                {
                    var md5 = Cryptography.ComputeHash("MD5", criteria.Custid.ToString(), "1qaw3+{:P)");
                    criteria.Securitycode = Cryptography.ComputeHash("SHA1", plainSecurityCode, md5);
                }

                Hashtable outputParams = new Hashtable();
                this._reportRepository.ResetSecurityCode(criteria, ref outputParams);
                return new Result
                {
                    ErrorCode = CastAide.AsInt32(outputParams["errcode"]),
                    ErrorMessage = CastAide.AsString(outputParams["errmsg"])
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }
    }
}
