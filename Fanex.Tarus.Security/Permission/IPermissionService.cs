// ***********************************************************************
// Assembly         : Fanex.Tarus.Security
// Author           : ethan.nguyen
// Created          : 08-16-2013
//
// Last Modified By : ethan.nguyen
// Last Modified On : 09-17-2013
// ***********************************************************************
// <copyright file="IPermissionService.cs" company="Microsoft">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Permission namespace.
/// </summary>
namespace Fanex.Tarus.Security.Permission
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface IPermissionService.
    /// </summary>
    internal interface IPermissionService
    {
        /// <summary>
        /// Determines whether the specified action path has permission.
        /// </summary>
        /// <param name="actionPath">The action path.</param>
        /// <param name="pageAccess">The page access.</param>
        /// <returns><c>true</c> if the specified action path has permission; otherwise, <c>false</c>.</returns>
        bool HasPermission(string actionPath, Dictionary<string, int> pageAccess);

        /// <summary>
        /// Gets the host permission.
        /// </summary>
        /// <typeparam name="T">The function type.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="roleAccess">The role access.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess);

        /// <summary>
        /// Gets the host permission.
        /// </summary>
        /// <param name="actionPath">The action path.</param>
        /// <param name="pageAccess">The page access.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool GetHostPermission(string actionPath, Dictionary<string, int> pageAccess);
    }
}