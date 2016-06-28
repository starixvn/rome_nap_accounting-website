// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerSettingCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.AGCasino
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.AGCasino;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;

    /// <summary>
    /// Update Customer Setting Criteria.
    /// </summary>
    public class UpdateCustomerSettingCriteria
    {
        public string CustomerId { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public string IP
        {
            get
            {
                return Utils.GetClientIP();
            }
        }

        public string Language
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        public int RoleId { get; set; }

        public List<ProductInfo> ProductInfo { get; set; }

        public BaseModel DefaultAGCasinoGroup { get; set; }

        public BaseModel ApplyGroupToMember { get; set; }

        public PositionTaking PT { get; set; }

        public string GetSettingKey()
        {
            return "Acc_CustomerUpdAGCasinoSetting";
        }

        public bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>Store Procedure Parameters.</returns>
        public Hashtable GetSPParams()
        {
            Hashtable storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("custid", CustomerId);
            storeProcedureParams.Add("userid", UserId);
            storeProcedureParams.Add("ip", IP);

            var gameTypeLength = ProductInfo == null ? 0 : ProductInfo.Count;

            var checkedGroup = new List<string>();
            var addedGroup = new List<string>();
            var deletedGroup = new List<string>();
            for (var i = 0; i < gameTypeLength; i++)
            {
                var gameType = ProductInfo[i];
                var gameTypeName = gameType.Name.ToLower();
                var gameKindGroupLength = gameType.GameTypeGroup.Count;
                for (var j = 0; j < gameKindGroupLength; j++)
                {
                    var group = gameType.GameTypeGroup[j];
                    var groupName = group.Name.ToLower();
                    if (group.Checked)
                    {
                        checkedGroup.Add(group.Name);
                    }

                    if (group.Checked && !group.OldChecked)
                    {
                        addedGroup.Add(group.Name);
                    }

                    if (!group.Checked && group.OldChecked)
                    {
                        deletedGroup.Add(group.Name);
                    }
                }
            }

            storeProcedureParams.Add("checkedgroup", string.Join(",", checkedGroup));
            storeProcedureParams.Add("addedgroup", string.Join(",", addedGroup));
            storeProcedureParams.Add("deletedgroup", string.Join(",", deletedGroup));

            storeProcedureParams.Add("applygrouptomember", ApplyGroupToMember.Name);
            storeProcedureParams.Add("defaultgroup", DefaultAGCasinoGroup.Name);
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            storeProcedureParams.Add("langkey", Language);
            storeProcedureParams.Add("v250_1801pt", PT.Current);
            return storeProcedureParams;
        }
    }
}
