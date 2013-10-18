
namespace Fanex.Tarus.DataAccess.Implement.Customer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using DNAide.Utils;
    using Fanex.Data;
    using Fanex.Tarus.DataAccess.Implement.Constants;
    using Fanex.Tarus.DataAccess.Interface.Customer;
    using Fanex.Tarus.Entities.Customer.DepositCustomer;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Helper;
    using Sunplus.Accounting.Entities;

    public class DepositCustomerAccess : BaseAccess, IDepositCustomerAccess
    {
        private Hashtable GetAddNewHashTableParams(NewLicenseePostParam newLicenseePostParam)
        {
            string paramName;
            string sportId;
            Dictionary<string, object> dictionaryParam = new Dictionary<string, object>();
            foreach (var param in newLicenseePostParam.PostParams)
            {
                paramName = param.Name.ToLower();
                if (dictionaryParam.ContainsKey(paramName))
                {
                    continue;
                }

                dictionaryParam.Add(paramName, param.Value);
            }

            var hashParam = new Hashtable();
            hashParam.Add("creator", dictionaryParam["creator"]);
            hashParam.Add("site", dictionaryParam["site"]);
            hashParam.Add("currency", dictionaryParam["currency"]);
            hashParam.Add("disablenumber", dictionaryParam["disablenumber"]);

            #region Sportsbook

            var sports = SportsSection.Instance.Sports;
            foreach (SportsElement sport in SportsSection.Instance.Sports)
            {
                sportId = sport.Id.ToLower();
                if (string.Equals(sportId, "161", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                paramName = "minbetv" + sportId;
                hashParam.Add(paramName, dictionaryParam[paramName]);

                paramName = "maxbetv" + sportId;
                hashParam.Add(paramName, dictionaryParam[paramName]);

                paramName = "maxpermatchv" + sportId;
                hashParam.Add(paramName, dictionaryParam[paramName]);

                if (string.Equals(sportId, "99mp", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var betTypes = sport.Bettype.Split(new char[] { ',' });
                var betTypesLength = betTypes.Length;
                for (var l = 0; l < betTypesLength; l++)
                {
                    paramName = "v" + sportId + "spositiontaking_" + betTypes[l];
                    hashParam.Add(paramName, dictionaryParam[paramName]);

                    paramName = "v" + sportId + "mpositiontaking_" + betTypes[l];
                    hashParam.Add(paramName, dictionaryParam[paramName]);

                    paramName = "v" + sportId + "apositiontaking_" + betTypes[l];
                    hashParam.Add(paramName, dictionaryParam[paramName]);

                    if (sportId == "1")
                    {
                        paramName = "v" + sportId + "slivepositiontaking_" + betTypes[l];
                        hashParam.Add(paramName, dictionaryParam[paramName]);

                        paramName = "v" + sportId + "mlivepositiontaking_" + betTypes[l];
                        hashParam.Add(paramName, dictionaryParam[paramName]);

                        paramName = "v" + sportId + "alivepositiontaking_" + betTypes[l];
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                    }
                }
            }

            #endregion Sportsbook
            #region NumberGame

            sportId = "161";
            paramName = "minbetv" + sportId;
            hashParam.Add(paramName, dictionaryParam[paramName]);
            paramName = "maxbetv" + sportId;
            hashParam.Add(paramName, dictionaryParam[paramName]);
            paramName = "maxpermatchv" + sportId;
            hashParam.Add(paramName, dictionaryParam[paramName]);
            paramName = "maxperballv" + sportId;
            hashParam.Add(paramName, dictionaryParam[paramName]);

            var levelType = new string[] { "s", "m", "a" };
            var levelTypeLength = levelType.Length;
            for (var i = 0; i < levelTypeLength; i++)
            {
                var type = levelType[i];
                foreach (SportsElement sport in sports)
                {
                    if (!string.Equals(sport.Id.Trim(), sportId) ||
                        sport.Bettype == null ||
                        sport.Bettype == "NA" ||
                        sport.Bettype == string.Empty ||
                        string.IsNullOrWhiteSpace(sport.Bettype.Trim()))
                    {
                        continue;
                    }

                    var betTypes = sport.Bettype.Split(new char[] { ',' });
                    foreach (var betType in betTypes)
                    {
                        var betTypeAfterTrim = betType.Trim();
                        paramName = "v" + sportId + type + "positiontaking_" + betTypeAfterTrim;
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                        paramName = "v" + sportId + type + "livepositiontaking_" + betTypeAfterTrim;
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                    }
                }
            }

            #endregion NumberGame
            #region Racing

            var racings = RacingSection.Instance.Racings;
            foreach (RacingElement racing in racings)
            {
                if (!racing.Active)
                {
                    continue;
                }

                foreach (ToteElement tote in racing.Totes)
                {
                    if (!tote.Active)
                    {
                        continue;
                    }

                    paramName = "v" + racing.Id + "minbet_" + tote.Id;
                    hashParam.Add(paramName, dictionaryParam[paramName]);
                    paramName = "v" + racing.Id + "maxbet_" + tote.Id;
                    hashParam.Add(paramName, dictionaryParam[paramName]);
                    paramName = "v" + racing.Id + "maxpermatch_" + tote.Id;
                    hashParam.Add(paramName, dictionaryParam[paramName]);
                    if (tote.BetType == null ||
                        tote.BetType == "NA" ||
                        tote.BetType == string.Empty ||
                        string.IsNullOrWhiteSpace(tote.BetType.Trim()))
                    {
                        continue;
                    }

                    var betTypes = tote.BetType.Split(new char[] { ',' });
                    foreach (var betType in betTypes)
                    {
                        paramName = "v" + racing.Id + "discount_" + betType;
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                        paramName = "v" + racing.Id + "spositiontaking_" + betType;
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                        paramName = "v" + racing.Id + "mpositiontaking_" + betType;
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                        paramName = "v" + racing.Id + "apositiontaking_" + betType;
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                        paramName = "v" + racing.Id + "ibcpositiontaking_" + betType;
                        hashParam.Add(paramName, dictionaryParam[paramName]);
                    }

                    paramName = "disable" + racing.Id + "_" + tote.Id;
                    hashParam.Add(paramName, dictionaryParam[paramName]);
                }
            }


            #endregion Racing
            #region VirtualSports

            var virtualSports = VirtualSportsSection.Instance.Sports;

            paramName = "disablevirtualsports";
            hashParam.Add(paramName, dictionaryParam[paramName]);
            var virtualSportsParams = "<products>";
            foreach (VirtualSportsElement sport in virtualSports)
            {
                if (!sport.Active)
                {
                    continue;
                }

                var betTypes = sport.Bettype;
                sportId = sport.Id;

                virtualSportsParams += string.Format("<product SportType=\"{0}\"" +
                    " MaxBet=\"{1}\"" +
                    " MinBet=\"{2}\"" +
                    " MaxPerMatch=\"{3}\"" +
                    " sPositionTaking=\"{4}\"" +
                    " mPositionTaking=\"{5}\"" +
                    " aPositionTaking=\"{6}\"></product>",
                    sportId,
                    dictionaryParam["v" + sportId + "maxbet"],
                    dictionaryParam["v" + sportId + "minbet"],
                    dictionaryParam["v" + sportId + "maxpermatch"],
                    dictionaryParam["v" + sportId + "spositiontaking"],
                    dictionaryParam["v" + sportId + "mpositiontaking"],
                    dictionaryParam["v" + sportId + "apositiontaking"]);
            }

            virtualSportsParams += "</products>";
            hashParam.Add("virtualsportlist", virtualSportsParams);

            #endregion VirtualSports

            return hashParam;
        }

        private Hashtable GetAddNewHashTableParams_DummyUser(NewLicenseePostParam newLicenseePostParam)
        {
            string paramName;
            string sportId;
            Dictionary<string, object> dictionaryParam = new Dictionary<string, object>();
            foreach (var param in newLicenseePostParam.PostParams)
            {
                paramName = param.Name.ToLower();
                if (dictionaryParam.ContainsKey(paramName))
                {
                    continue;
                }

                dictionaryParam.Add(paramName, param.Value);
            }

            var hashParam = new Hashtable();
            hashParam.Add("creator", dictionaryParam["creator"]);
            hashParam.Add("site", dictionaryParam["site"]);
            hashParam.Add("currency", dictionaryParam["currency"]);
            hashParam.Add("disablenumber", dictionaryParam["disablenumber"]);

            #region Sportsbook

            var sports = SportsSection.Instance.Sports;
            foreach (SportsElement sport in SportsSection.Instance.Sports)
            {
                sportId = sport.Id;
                if (string.Equals(sportId, "99mp", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(sportId, "161", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                paramName = "minbetv" + sportId;
                hashParam.Add(paramName, 0);

                paramName = "maxbetv" + sportId;
                hashParam.Add(paramName, 0);

                paramName = "maxpermatchv" + sportId;
                hashParam.Add(paramName, 0);

                var betTypes = sport.Bettype.Split(new char[] { ',' });
                var betTypesLength = betTypes.Length;
                for (var l = 0; l < betTypesLength; l++)
                {
                    paramName = "v" + sportId + "spositiontaking_" + betTypes[l];
                    hashParam.Add(paramName, 0);

                    paramName = "v" + sportId + "mpositiontaking_" + betTypes[l];
                    hashParam.Add(paramName, 0);

                    paramName = "v" + sportId + "apositiontaking_" + betTypes[l];
                    hashParam.Add(paramName, 0);

                    if (sportId == "1")
                    {
                        paramName = "v" + sportId + "slivepositiontaking_" + betTypes[l];
                        hashParam.Add(paramName, 0);

                        paramName = "v" + sportId + "mlivepositiontaking_" + betTypes[l];
                        hashParam.Add(paramName, 0);

                        paramName = "v" + sportId + "alivepositiontaking_" + betTypes[l];
                        hashParam.Add(paramName, 0);
                    }
                }
            }

            #endregion Sportsbook
            #region NumberGame

            sportId = "161";
            paramName = "minbetv" + sportId;
            hashParam.Add(paramName, 0);
            paramName = "maxbetv" + sportId;
            hashParam.Add(paramName, 0);
            paramName = "maxpermatchv" + sportId;
            hashParam.Add(paramName, 0);
            paramName = "maxperballv" + sportId;
            hashParam.Add(paramName, 0);

            var levelType = new string[] { "s", "m", "a" };
            var levelTypeLength = levelType.Length;
            for (var i = 0; i < levelTypeLength; i++)
            {
                var type = levelType[i];
                foreach (SportsElement sport in sports)
                {
                    if (!string.Equals(sport.Id.Trim(), sportId) ||
                        sport.Bettype == null ||
                        sport.Bettype == "NA" ||
                        sport.Bettype == string.Empty ||
                        string.IsNullOrWhiteSpace(sport.Bettype.Trim()))
                    {
                        continue;
                    }

                    var betTypes = sport.Bettype.Split(new char[] { ',' });
                    foreach (var betType in betTypes)
                    {
                        paramName = "v" + sportId + type + "positiontaking_" + betType;
                        hashParam.Add(paramName, 0);
                        paramName = "v" + sportId + type + "livepositiontaking_" + betType;
                        hashParam.Add(paramName, 0);
                    }
                }
            }

            #endregion NumberGame
            #region Racing

            var racings = RacingSection.Instance.Racings;
            foreach (RacingElement racing in racings)
            {
                if (!racing.Active)
                {
                    continue;
                }

                foreach (ToteElement tote in racing.Totes)
                {
                    if (!tote.Active)
                    {
                        continue;
                    }

                    paramName = "v" + racing.Id + "minbet_" + tote.Id;
                    hashParam.Add(paramName, 0);
                    paramName = "v" + racing.Id + "maxbet_" + tote.Id;
                    hashParam.Add(paramName, 0);
                    paramName = "v" + racing.Id + "maxpermatch_" + tote.Id;
                    hashParam.Add(paramName, 0);
                    if (tote.BetType == null ||
                        tote.BetType == "NA" ||
                        tote.BetType == string.Empty ||
                        string.IsNullOrWhiteSpace(tote.BetType.Trim()))
                    {
                        continue;
                    }

                    var betTypes = tote.BetType.Split(new char[] { ',' });
                    foreach (var betType in betTypes)
                    {
                        paramName = "v" + racing.Id + "discount_" + betType;
                        hashParam.Add(paramName, 0);
                        paramName = "v" + racing.Id + "spositiontaking_" + betType;
                        hashParam.Add(paramName, 0);
                        paramName = "v" + racing.Id + "mpositiontaking_" + betType;
                        hashParam.Add(paramName, 0);
                        paramName = "v" + racing.Id + "apositiontaking_" + betType;
                        hashParam.Add(paramName, 0);
                        paramName = "v" + racing.Id + "ibcpositiontaking_" + betType;
                        hashParam.Add(paramName, 0);
                    }

                    paramName = "disable" + racing.Id + "_" + tote.Id;
                    hashParam.Add(paramName, dictionaryParam[paramName]);
                }
            }


            #endregion Racing
            #region VirtualSports

            var virtualSports = VirtualSportsSection.Instance.Sports;

            paramName = "disablevirtualsports";
            hashParam.Add(paramName, dictionaryParam[paramName]);
            var virtualSportsParams = "<VTSports>";
            foreach (VirtualSportsElement sport in virtualSports)
            {
                if (!sport.Active)
                {
                    continue;
                }

                var betTypes = sport.Bettype;
                sportId = sport.Id;

                virtualSportsParams += string.Format("<VTSport SportType = \"{0}\"" +
                    " MaxBet=\"{1}\"" +
                    " MinBet=\"{2}\"" +
                    " MaxPerMatch=\"{3}\"" +
                    " sPositionTaking=\"{4}\"" +
                    " mPositionTaking=\"{5}\"" +
                    " aPositionTaking=\"{6}\"></VTSport>",
                    sportId,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0);
            }

            virtualSportsParams += "/<VTSports>";
            hashParam.Add("virtualsportsPparams", virtualSportsParams);

            #endregion VirtualSports

            return hashParam;
        }

        public Result NewLicenseeAddNewSuper(NewLicenseePostParam newLicenseePostParam)
        {
            var hashParam = GetAddNewHashTableParams(newLicenseePostParam);
            hashParam.Add("username", newLicenseePostParam.SuperName);
            hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
            hashParam.Add("custid", 0);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);
            if (!hashParam.ContainsKey("userpwd"))
            {
                hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
            }

            Result result;
            using (var objDb = new ObjectDb("NewLicensee_AdNewSuper"))
            {
                objDb.AssignParamValues(hashParam);
                objDb.ExecuteNonQuery();

                newLicenseePostParam.SuperId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                result = new Result
                {
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode")),
                    ErrorMessage = CastAide.AsString(objDb.GetParameterValue("ErrMsg"))
                };
            }

            if (result.ErrorCode != 0)
            {
                return result;
            }

            List<int> succeededIdList = new List<int>();
            succeededIdList.Add(newLicenseePostParam.SuperId);
            if (string.IsNullOrWhiteSpace(newLicenseePostParam.MasterName))
            {
                result.ErrorMessage = string.Join(",", succeededIdList);
                return result;
            }

            result = NewLicenseeAddNewMaster(newLicenseePostParam);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            succeededIdList.Add(newLicenseePostParam.MasterId);
            if (string.IsNullOrWhiteSpace(newLicenseePostParam.AgentName))
            {
                result.ErrorMessage = string.Join(",", succeededIdList);
                return result;
            }

            result = NewLicenseeAddNewAgent(newLicenseePostParam);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            succeededIdList.Add(newLicenseePostParam.AgentId);
            if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyMasterName))
            {
                result.ErrorMessage = string.Join(",", succeededIdList);
                return result;
            }

            result = NewLicenseeAddNewDummyUser(ref newLicenseePostParam,
                RoleConstants.Master,
                ref result,
                ref succeededIdList);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyAgentName))
            {
                result.ErrorMessage = string.Join(",", succeededIdList);
                return result;
            }

            result = NewLicenseeAddNewDummyUser(ref newLicenseePostParam,
                RoleConstants.Agent,
                ref result,
                ref succeededIdList);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyMemberName))
            {
                result.ErrorMessage = string.Join(",", succeededIdList);
                return result;
            }

            var dummyMemberNameList = newLicenseePostParam.DummyMemberName.Split(new char[] { ',' });
            foreach (var dummyMemberName in dummyMemberNameList)
            {
                newLicenseePostParam.DummyMemberName = dummyMemberName;
                result = NewLicenseeAddNewDummyUser(ref newLicenseePostParam,
                    RoleConstants.Member,
                    ref result,
                    ref succeededIdList);
                if (result.ErrorCode != 0)
                {
                    return result;
                }
            }

            result.ErrorMessage = string.Join(",", succeededIdList);
            return result;
        }

        public Result NewLicenseeAddNewMaster(NewLicenseePostParam newLicenseePostParam)
        {
            var hashParam = GetAddNewHashTableParams(newLicenseePostParam);
            hashParam.Add("srecommend", newLicenseePostParam.SuperId);
            hashParam.Add("username", newLicenseePostParam.MasterName);
            hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

            hashParam.Add("custid", 0);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);

            if (!hashParam.ContainsKey("userpwd"))
            {
                hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
            }

            using (var objDb = new ObjectDb("NewLicensee_AdNewMaster"))
            {
                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.MasterId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                    errMsg = newLicenseePostParam.MasterId.ToString();
                }

                return new Result(errCode, errMsg);
            }
        }

        private Result NewLicenseeAddNewDummyUser(ref NewLicenseePostParam newLicenseePostParam,
            int roleId,
            ref Result result,
            ref List<int> succeededIdList)
        {
            switch (roleId)
            {
                case RoleConstants.Master:
                    if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyMasterName))
                    {
                        result.ErrorMessage = string.Join(",", succeededIdList);
                        return result;
                    }

                    break;
                case RoleConstants.Agent:
                    if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyAgentName))
                    {
                        result.ErrorMessage = string.Join(",", succeededIdList);
                        return result;
                    }

                    break;
                case RoleConstants.Member:
                    if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyMemberName))
                    {
                        result.ErrorMessage = string.Join(",", succeededIdList);
                        return result;
                    }

                    break;
                default:
                    break;
            }

            var addResult = NewLicenseeAddNewDummyUser(ref newLicenseePostParam, roleId);
            if (result.ErrorCode != 0)
            {
                result = addResult;
                return result;
            }

            switch (roleId)
            {
                case RoleConstants.Master:
                    succeededIdList.Add(newLicenseePostParam.DummyMasterId);
                    break;
                case RoleConstants.Agent:
                    succeededIdList.Add(newLicenseePostParam.DummyAgentId);
                    break;
                case RoleConstants.Member:
                    succeededIdList.Add(newLicenseePostParam.DummyMemberId);
                    break;
                default:
                    break;
            }

            return result;
        }

        private Result NewLicenseeAddNewDummyUser(ref NewLicenseePostParam newLicenseePostParam,
            int roleId)
        {
            if (roleId != RoleConstants.Master &&
                roleId != RoleConstants.Agent &&
                roleId != RoleConstants.Member)
            {
                return new Result(1, "Wrong role ID level!");
            }

            var hashParam = GetAddNewHashTableParams_DummyUser(newLicenseePostParam);
            hashParam.Add("ip", Utils.GetClientIP());
            hashParam.Add("custid", 0);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);
            if (!hashParam.ContainsKey("userpwd"))
            {
                hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
            }

            string storeProcedureName = string.Empty;
            switch (roleId)
            {
                case RoleConstants.Master:
                    storeProcedureName = "NewLicensee_AdNewMaster";
                    hashParam.Add("srecommend", newLicenseePostParam.SuperId);
                    hashParam.Add("username", newLicenseePostParam.DummyMasterName);
                    break;
                case RoleConstants.Agent:
                    storeProcedureName = "NewLicensee_AdNewAgent";
                    hashParam.Add("mrecommend", newLicenseePostParam.DummyMasterId);
                    hashParam.Add("username", newLicenseePostParam.DummyAgentName);
                    break;
                case RoleConstants.Member:
                    storeProcedureName = "NewLicensee_AdNewMember";
                    hashParam.Add("recommend", newLicenseePostParam.DummyAgentId);
                    hashParam.Add("username", newLicenseePostParam.DummyMemberName);
                    break;
                default:
                    return new Result(1, "Wrong role ID level!");
            }

            int errCode = 0;
            string errMsg = string.Empty;
            int custid = 0;
            using (var objDb = new ObjectDb(storeProcedureName))
            {
                objDb.AssignParamValues(hashParam);
                objDb.ExecuteNonQuery();
                errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));
                custid = CastAide.AsInt32(objDb.GetParameterValue("custid"));
            }

            if (errCode != 0)
            {
                return new Result(errCode, errMsg);
            }

            switch (roleId)
            {
                case RoleConstants.Master:
                    newLicenseePostParam.DummyMasterId = custid;
                    break;
                case RoleConstants.Agent:
                    newLicenseePostParam.DummyAgentId = custid;
                    break;
                case RoleConstants.Member:
                    newLicenseePostParam.DummyMemberId = custid;
                    break;
                default:
                    errCode = 0;
                    errMsg = string.Empty;
                    break;
            }

            return new Result(errCode, errMsg);
        }

        public Result NewLicenseeAddNewAgent(NewLicenseePostParam newLicenseePostParam)
        {
            var hashParam = GetAddNewHashTableParams(newLicenseePostParam);
            hashParam.Add("mrecommend", newLicenseePostParam.MasterId);
            hashParam.Add("username", newLicenseePostParam.AgentName);
            hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

            hashParam.Add("custid", 0);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);

            if (!hashParam.ContainsKey("userpwd"))
            {
                hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
            }

            using (var objDb = new ObjectDb("NewLicensee_AdNewAgent"))
            {
                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.AgentId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                    errMsg = newLicenseePostParam.AgentId.ToString();
                }

                return new Result(errCode, errMsg);
            }
        }

        public Result NewLicenseeAddNewMember(NewLicenseePostParam newLicenseePostParam)
        {
            var hashParam = GetAddNewHashTableParams(newLicenseePostParam);
            hashParam.Add("recommend", newLicenseePostParam.AgentId);
            hashParam.Add("username", newLicenseePostParam.MemberName);
            hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

            hashParam.Add("custid", 0);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);

            if (!hashParam.ContainsKey("userpwd"))
            {
                hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
            }

            using (var objDb = new ObjectDb("NewLicensee_AdNewMember"))
            {
                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.MemberId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                    errMsg = newLicenseePostParam.MemberId.ToString();
                }

                return new Result(errCode, errMsg);
            }
        }

        public DataSet GetSuperSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetSuperSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetMasterSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetMasterSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetAgentSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetAgentSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetMemberSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetMemberSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public Result UpdateSuperSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateSuperSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateMasterSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateMasterSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateAgentSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateAgentSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateMemberSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateMemberSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public DataTable SelAllLicensee()
        {
            using (var objDb = new ObjectDb("Dep_LicenseeSelAll"))
            {
                var dataSet = objDb.ExecuteDataSet();
                if (dataSet.Tables.Count == 0) return null;
                return dataSet.Tables[0];
            }
        }

        public DataSet GetMasterSettingForAddNew(int custId)
        {
            using (var objDb = new ObjectDb("CustomerSelMember_MAgInfo"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetAgentSettingForAddNew(int custId)
        {
            using (var objDb = new ObjectDb("CustomerSelMember_AgInfo"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }
    }
}
