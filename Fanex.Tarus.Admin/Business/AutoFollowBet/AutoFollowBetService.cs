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

        /// <summary>
        /// Gets the follow type.
        /// </summary>
        /// <returns>The follow type.</returns>
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

        /// <summary>
        /// Gets the followed customer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The followed customer.</returns>
        public List<FollowedCustomer> GetAutoFollowedBetCustomer(GetAutoFollowedBetCustomerCriteria criteria)
        {
            try
            {
                var rawData = _autoFollowBetAccess.GetAutoFollowedBetCustomer(criteria);
                var listFollowedCustomer = new List<FollowedCustomer>();
                foreach (var data in rawData)
                {
                    var followedCustomer = new FollowedCustomer
                    {
                        Username = data.Username,
                        Rate = data.Rate,
                        BAStatus = data.BAStatus,

                        SourceId = data.SourceId,
                        LiveStatus = data.LiveStatus,
                        FollowedHDP = data.FollowedHDP,
                        Followed1stHDP = data.Followed1stHDP,
                        FollowedOverUnder = data.FollowedOverUnder,
                        Followed1stOverUnder = data.Followed1stOverUnder
                    };

                    followedCustomer.Source = GetSourceTypeName(followedCustomer.SourceId);
                    listFollowedCustomer.Add(followedCustomer);
                }

                return listFollowedCustomer;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Deletes the followed customer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The followed customer.</returns>
        public Result DeleteAutoFollowedBetCustomer(DeleteAutoFollowedBetCustomerCriteria criteria)
        {
            try
            {
                var result = new Result();
                result = _autoFollowBetAccess.DeleteAutoFollowedBetCustomer(criteria);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the followed customer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The followed customer.</returns>
        public Result UpdateAutoFollowedBetCustomer(UpdateAutoFollowedBetCustomerCriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    return null;
                }

                var result = _autoFollowBetAccess.UpdateAutoFollowedBetCustomer(criteria);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Add the followed customer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The followed customer.</returns>
        public Result AddAutoFollowedBetCustomer(AddAutoFollowedBetCustomerCriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    return null;
                }

                var result = _autoFollowBetAccess.AddAutoFollowedBetCustomer(criteria);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the name of the source type.
        /// </summary>
        /// <param name="sourceId">The source identifier.</param>
        /// <returns>The name of the source type.</returns>
        private string GetSourceTypeName(int sourceId)
        {
            string result = string.Empty;
            switch (sourceId)
            {
                case SourceTypeConstants.All:
                    result = "All";
                    break;
                case SourceTypeConstants.Auto:
                    result = "Auto";
                    break;
                case SourceTypeConstants.Manual:
                    result = "Manual";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
    }
}