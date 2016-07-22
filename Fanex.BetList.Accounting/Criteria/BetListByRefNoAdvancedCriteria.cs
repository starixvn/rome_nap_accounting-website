// --------------------------------------------------------------------------
// <copyright file="BetListByRefNoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Criteria
{
    public class BetListByProductCriteria : CriteriaBase
    {
        private string _gameId;
        private string _username;
        private string _betId;

        public long UserId { get; set; }


        public byte ProductId { get; set; }

        public int CustId { get; set; }

        public string Username
        {
            get { return _username ?? ""; }
            set { _username = value; }
        }
        public string GameId
        {
            get { return _gameId ?? ""; }
            set { _gameId = value; }
        }

        public string BetId
        {
            get { return _betId ?? ""; }
            set { _betId = value; }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_SelByProduct";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}