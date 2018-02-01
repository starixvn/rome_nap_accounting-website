// --------------------------------------------------------------------------
// <copyright file="LiveTypeConstants.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Constants
{
    using Fanex.Tarus.Customer.Resources;

    /// <summary>
    /// Live Type Constants.
    /// </summary>
    public class LiveTypeConstants
    {
        public const int NonLive = 0;

        public const int Live = 1;

        public const int NotLiveType = -2;

        /// <summary>
        /// Gets the live type name.
        /// </summary>
        /// <param name="liveType">Type of the live.</param>
        /// <returns>The live type name.</returns>
        public static string LiveTypeName(int liveType)
        {
            switch (liveType)
            {
                case NonLive:
                    return Customer_Labels.Non_Live;
                case Live:
                    return Customer_Labels.Live;
                default:
                    return string.Empty;
            }
        }
    }
}
