// --------------------------------------------------------------------------
// <copyright file="AutoFollowBetService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Business.AutoFollowBet
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.Common;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;
    using Fanex.Tarus.Admin.DataAccess.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Admin.Models.AutoFollowBet;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Extensions;

    /// <summary>
    /// Auto Follow Bet Service.
    /// </summary>
    public class AutoFollowBetService : IAutoFollowBetService
    {
        private readonly IAutoFollowBetAccess _autoFollowBetAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFollowBetService"/> class.
        /// </summary>
        public AutoFollowBetService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFollowBetService"/> class.
        /// </summary>
        /// <param name="crossTransferAccess">The cross transfer access.</param>
        public AutoFollowBetService(IAutoFollowBetAccess crossTransferAccess)
        {
            _autoFollowBetAccess = crossTransferAccess ?? new AutoFollowBetAccess();
        }

        #region IAutoFollowBetService Members

        /// <summary>
        /// Selects the followed customer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Return list of followed customer.</returns>
        public List<FollowedCustomer> SelFollowedCustomer(GetAutoFollowedBetCriteria criteria)
        {
            var listFollowedCustomer = new List<FollowedCustomer>();
            DataTable dtb = _autoFollowBetAccess.AutoFollowBetSelCustomer(criteria);
            dtb.DefaultView.Sort = "username DESC";
            foreach (DataRow row in dtb.Rows)
            {
                listFollowedCustomer.Add(new FollowedCustomer(CastAide.AsString(row["username"]), CastAide.AsDouble(row["rate"]), CastAide.AsBoolean(row["IsBA"])));
            }

            return listFollowedCustomer;
        }

        /// <summary>
        /// Deletes the followed customer.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Return Result.</returns>
        public Result DelFollowedCustomer(string username)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetDelCustomer(username);
            return result;
        }

        /// <summary>
        /// Increases the followed customer.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="rate">The currency rate.</param>
        /// <returns></returns>
        public Entities.Result InsFollowedCustomer(string username, double rate)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetInsCustomer(username, rate);
            return result;
        }

        /// <summary>
        /// Updates the followed customer.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="rate">The currency rate.</param>
        /// <returns></returns>
        public Entities.Result UpdFollowedCustomer(string username, double rate)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetUpdCustomer(username, rate);
            return result;
        }

        public List<FollowTypeInfo> GetFollowType()
        {
            try
            {
                Admin_AutoFollowBetCollection collection = Admin_AutoFollowBetSection.Instance.FollowType;
                List<FollowTypeInfo> result = new List<FollowTypeInfo>();
                foreach (Admin_AutoFollowBetElement element in collection)
                {
                    if (!element.Active)
                    {
                        continue;
                    }

                    var followType = new FollowTypeInfo()
                    {
                        Id = Converter.ToString2(element.Id),
                        Name = Converter.ToString2(element.Name),
                        ChildsInfo = new List<FollowTypeInfo>()
                    };

                    foreach (Admin_AutoFollowBetElement subType in element.SubTypes)
                    {
                        followType.ChildsInfo.Add(new FollowTypeInfo()
                        {
                            Id = Converter.ToString2(subType.Id),
                            Name = Converter.ToString2(subType.Name)
                        });
                    }

                    result.Add(followType);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        #endregion IAutoFollowBetService Members
    }
}