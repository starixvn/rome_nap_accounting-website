namespace Fanex.Tarus.CN88.Repositories.PositionTaking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.CN88.Constants;
    using Fanex.Tarus.CN88.Criteria.PositionTaking;
    using Fanex.Tarus.CN88.Models;
    using Fanex.Tarus.CN88.Models.PositionTaking;
    using Fanex.Tarus.CN88.Resources;

    public class PTRepository : IPTRepository
    {
        public LinePTModel GetPT(GetPTCriteria criteria)
        {
            var result = new LinePTModel();
            if (!criteria.IsValid())
            {
                return result;
            }

            var pts = new List<SportPTDTO>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var cust = new CustInfoModel
                        {
                            CustId = Converter.ToInt(reader["CustId"]),
                            Username = Converter.ToString2(reader["Username"]),
                            RoleId = Converter.ToInt(reader["RoleId"])
                        };

                        cust.RoleName = CN88Resources.ResourceManager.GetString($"Role_{cust.RoleId}");
                        result.Customers.Add(cust);
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            pts.Add(new SportPTDTO
                            {
                                SportType = Converter.ToInt(reader["Sporttype"]),
                                BetType = Converter.ToInt(reader["Bettype"]),
                                RoleId = Converter.ToInt(reader["RoleId"]),
                                MaxPTValue = Converter.ToString2(reader["MaxPT"]),
                                MinPTValue = Converter.ToString2(reader["MinPT"]),
                                MaxPT2Value = Converter.ToString2(reader["MaxPT2"]),
                                MinPT2Value = Converter.ToString2(reader["MinPT2"]),
                                SuperPT = Converter.ToString2(reader["SuperPT"]),
                                MasterPT = Converter.ToString2(reader["MasterPT"]),
                                AgentPT = Converter.ToString2(reader["AgentPT"]),
                                SuperPT2 = Converter.ToString2(reader["SuperPT2"]) ?? string.Empty,
                                MasterPT2 = Converter.ToString2(reader["MasterPT2"]) ?? string.Empty,
                                AgentPT2 = Converter.ToString2(reader["AgentPT2"]) ?? string.Empty
                            });
                        }
                    }

                    reader.Close();
                }

                var outputParams = objDb.GetOuputParams();
                result.ErrorCode = Converter.ToInt(outputParams["errcode"]);
                result.ErrorMessage = Converter.ToString2(outputParams["errmsg"]);
            }

            if (result.ErrorCode == 0)
            {
                GroupPTInfoByLevel(result, pts);
            }

            return result;
        }

        public LineCommModel GetComm(GetCommCriteria criteria)
        {
            var result = new LineCommModel();
            if (!criteria.IsValid())
            {
                return result;
            }

            var comms = new List<CommDTO>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var cust = new CustInfoModel
                        {
                            CustId = Converter.ToInt(reader["CustId"]),
                            Username = Converter.ToString2(reader["Username"]),
                            RoleId = Converter.ToInt(reader["RoleId"])
                        };

                        cust.RoleName = CN88Resources.ResourceManager.GetString($"Role_{cust.RoleId}");
                        result.Customers.Add(cust);
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            comms.Add(new CommDTO
                            {
                                SportType = Converter.ToInt(reader["Sporttype"]),
                                BetType = Converter.ToInt(reader["Bettype"]),
                                IsLive = Converter.ToBool(reader["IsLive"]),
                                RoleId = Converter.ToInt(reader["RoleId"]),
                                GroupName = Converter.ToString2(reader["GroupName"]),
                                Comm = Converter.ToDecimal(reader["Comm"])
                            });
                        }
                    }

                    reader.Close();
                }

                var outputParams = objDb.GetOuputParams();
                result.ErrorCode = Converter.ToInt(outputParams["errcode"]);
                result.ErrorMessage = Converter.ToString2(outputParams["errmsg"]);
            }

            if (result.ErrorCode == 0)
            {
                GroupCommInfoByLevel(result, comms);
            }

            return result;
        }

        private void GroupPTInfoByLevel(LinePTModel result, List<SportPTDTO> pts)
        {
            result.RoleId = result.Customers.Last().RoleId;
            var roleId = result.RoleId;

            Func<int, int, SportPTDTO> getPt = (role, sportId) => pts.FirstOrDefault(pt => pt.RoleId == role && pt.SportType == sportId);

            var sport = getPt(RoleIdConstants.Super, SportTypeConstants.Sportsbook);
            result.SportsbookMinPTValue = sport.MinPTValue;
            result.SportsbookMaxPTValue = sport.MaxPTValue;
            result.SportsbookMinPT2Value = sport.MinPT2Value;
            result.SportsbookMaxPT2Value = sport.MaxPT2Value;

            sport = getPt(RoleIdConstants.Super, SportTypeConstants.Nonsports);
            result.NonsportsMinPTValue = sport.MinPTValue;
            result.NonsportsMaxPTValue = sport.MaxPTValue;
            result.NonsportsMinPT2Value = sport.MinPT2Value;
            result.NonsportsMaxPT2Value = sport.MaxPT2Value;

            if (roleId >= RoleIdConstants.Super)
            {
                return;
            }

            result.CustPTs.Add(new CustomerPTModel
            {
                RoleId = RoleIdConstants.Super,
                RoleName = CN88Resources.Role_4,
                SportsbookPT = getPt(RoleIdConstants.Master, SportTypeConstants.Sportsbook).SuperPT,
                NonSportPT = getPt(RoleIdConstants.Master, SportTypeConstants.Nonsports).SuperPT,
                OldSportsbookPT = getPt(RoleIdConstants.Master, SportTypeConstants.Sportsbook).SuperPT2,
                OldNonSportPT = getPt(RoleIdConstants.Master, SportTypeConstants.Nonsports).SuperPT2
            });

            if (roleId == RoleIdConstants.Master)
            {
                return;
            }

            result.CustPTs.Add(new CustomerPTModel
            {
                RoleId = RoleIdConstants.Master,
                RoleName = CN88Resources.Role_3,
                SportsbookPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Sportsbook).MasterPT,
                NonSportPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Nonsports).MasterPT,
                OldSportsbookPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Sportsbook).MasterPT2,
                OldNonSportPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Nonsports).MasterPT2
            });

            result.CustPTs.Add(new CustomerPTModel
            {
                RoleId = RoleIdConstants.Agent,
                RoleName = CN88Resources.Role_2,
                SportsbookPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Sportsbook).AgentPT,
                NonSportPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Nonsports).AgentPT,
                OldSportsbookPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Sportsbook).AgentPT2,
                OldNonSportPT = getPt(RoleIdConstants.Agent, SportTypeConstants.Nonsports).AgentPT2
            });
        }

        private void GroupCommInfoByLevel(LineCommModel result, List<CommDTO> comms)
        {
            result.RoleId = result.Customers.Last().RoleId;
            var roleId = result.RoleId;

            result.CustComm.Add(GetCustComm(comms, RoleIdConstants.Super));
            if (roleId == RoleIdConstants.Super)
            {
                return;
            }

            result.CustComm.Add(GetCustComm(comms, RoleIdConstants.Master));
            if (roleId == RoleIdConstants.Master)
            {
                return;
            }

            result.CustComm.Add(GetCustComm(comms, RoleIdConstants.Agent));
            if (roleId == RoleIdConstants.Agent)
            {
                return;
            }

            var memberComm = GetCustComm(comms, RoleIdConstants.Member);
            memberComm.CommGroup = comms.FirstOrDefault(c => c.RoleId == RoleIdConstants.Member).GroupName.ToUpperInvariant();
            result.CustComm.Add(memberComm);
        }

        private CustomerCommModel GetCustComm(List<CommDTO> comms, int roleId)
        {
            var result = new CustomerCommModel();
            var sportId = SportTypeConstants.Soccer;
            result.Soccer = new SportCommModel
            {
                _1x2Group = GetComm(comms, roleId, sportId, BetTypeConstants._1x2Group, string.Empty, false),
                OtherGroup = GetComm(comms, roleId, sportId, BetTypeConstants.OtherGroup, string.Empty, false),
                InplayOtherGroup = GetComm(comms, roleId, sportId, BetTypeConstants.OtherGroup, string.Empty, true)
            };

            result.Soccer.HandicapGroup["A"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "A", false);
            result.Soccer.HandicapGroup["B"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "B", false);
            result.Soccer.HandicapGroup["C"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "C", false);
            result.Soccer.HandicapGroup["D"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "D", false);
            result.Soccer.InplayHandicapGroup["A"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "A", true);
            result.Soccer.InplayHandicapGroup["B"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "B", true);
            result.Soccer.InplayHandicapGroup["C"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "C", true);
            result.Soccer.InplayHandicapGroup["D"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "D", true);

            sportId = SportTypeConstants.Basketball;
            result.Basketball = new SportCommModel
            {
                MoneylineGroup = GetComm(comms, roleId, sportId, BetTypeConstants.MoneylineGroup, string.Empty, false),
                OtherGroup = GetComm(comms, roleId, sportId, BetTypeConstants.OtherGroup, string.Empty, false)
            };

            result.Basketball.HandicapGroup["A"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "A", false);
            result.Basketball.HandicapGroup["B"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "B", false);
            result.Basketball.HandicapGroup["C"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "C", false);
            result.Basketball.HandicapGroup["D"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "D", false);
            result.Basketball.InplayHandicapGroup["A"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "A", true);
            result.Basketball.InplayHandicapGroup["B"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "B", true);
            result.Basketball.InplayHandicapGroup["C"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "C", true);
            result.Basketball.InplayHandicapGroup["D"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "D", true);

            sportId = SportTypeConstants.OtherSports;
            result.OtherSports = new SportCommModel
            {
                _1x2Group = GetComm(comms, roleId, sportId, BetTypeConstants._1x2Group, string.Empty, false),
                OtherGroup = GetComm(comms, roleId, sportId, BetTypeConstants.OtherGroup, string.Empty, false)
            };

            result.OtherSports.HandicapGroup["A"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "A", false);
            result.OtherSports.HandicapGroup["B"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "B", false);
            result.OtherSports.HandicapGroup["C"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "C", false);
            result.OtherSports.HandicapGroup["D"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "D", false);
            result.OtherSports.InplayHandicapGroup["A"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "A", true);
            result.OtherSports.InplayHandicapGroup["B"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "B", true);
            result.OtherSports.InplayHandicapGroup["C"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "C", true);
            result.OtherSports.InplayHandicapGroup["D"] = GetComm(comms, roleId, sportId, BetTypeConstants.HandicapGroup, "D", true);

            sportId = SportTypeConstants.AllSports;
            result.AllSports = new SportCommModel
            {
                OutrightGroup = GetComm(comms, roleId, sportId, BetTypeConstants.OutrightGroup, string.Empty, false)
            };

            return result;
        }

        private decimal GetComm(List<CommDTO> comms, int role, int sportType, int betType, string group, bool isLive)
            => comms.FirstOrDefault(comm =>
            comm.RoleId == role &&
            comm.SportType == sportType &&
            comm.BetType == betType &&
            string.Equals(group, comm.GroupName, StringComparison.OrdinalIgnoreCase) &&
            comm.IsLive == isLive)?.Comm ?? 0;
    }
}
