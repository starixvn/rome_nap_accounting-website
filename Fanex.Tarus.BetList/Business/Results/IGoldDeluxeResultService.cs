// --------------------------------------------------------------------------
// <copyright file="IGoldDeluxeResultService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2015.05.22">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    /// <summary>
    /// The interface IGoldDeluxeResultService.
    /// </summary>
    public interface IGoldDeluxeResultService
    {
        string GetResult(long transID, int bettype);
    }
}