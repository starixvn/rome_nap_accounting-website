// --------------------------------------------------------------------------
// <copyright file="AddSMAListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.SMATurnover
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Sunplus.Accounting.Core.Common;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class AddSMAListCriteria
    {
        public int UserID
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public string ListSMA { get; set; }

        public string IP
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string GetSettingKey()
        {
            return "Customer_SMATurnover_AddSMAList";
        }

        public bool IsValid()
        {
            return true;
        }

    }
}
