namespace Fanex.Tarus.Customer.ManageLicenseeCode
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.ManageLicenseeCode.Criterias;
    using Fanex.Tarus.Customer.ManageLicenseeCode.Models;
    using Fanex.Tarus.Infrastructure.Extensions;

    public interface IDataAccess
    {
        IEnumerable<Licensee> GetActiveLicensee();

        SelLicenseeCodeResponse GetLicenseeCode(LicenseeTransferCodeSelCriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);


        IEnumerable<Result> ValidateLicenseeCode(LicenseeTransferCodeValidateCriteria criteria);
    }
    public class DataAccess : IDataAccess
    {
        public IEnumerable<Licensee> GetActiveLicensee()
        {
            try
            {
                var criteria = new LicenseeSelCriteria();

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var activeLicensees = new List<Licensee>();
                        while (reader.Read())
                        {
                            var licensee = new Licensee
                            {
                                Id = reader["LicenseeId"].ToString(),
                                Name = reader["LicenseeName"].ToString()
                            };

                            activeLicensees.Add(licensee);
                        }

                        return activeLicensees;
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                return new List<Licensee>();
            }
        }

        public SelLicenseeCodeResponse GetLicenseeCode(LicenseeTransferCodeSelCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
                }

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    var response = new SelLicenseeCodeResponse();
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var licenseeCodes = new List<LicenseeCode>();

                        while (reader.Read())
                        {
                            var licensee = new LicenseeCode
                            {
                                RowNumber = int.Parse(reader["RowNumber"].ToString()),
                                Name = reader["LicenseeName"].ToString(),
                                Code = reader["LicenseeCode"].ToString(),
                                Status = reader["LicenseeStatus"].ToString()
                            };

                            licenseeCodes.Add(licensee);
                        }

                        response.LicenseeCodes = licenseeCodes;
                    }

                    var output = db.GetOuputParams();
                    response.Totals = int.Parse(output["totalrows"].ToString());

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                return new SelLicenseeCodeResponse
                {
                    LicenseeCodes = new List<LicenseeCode>(),
                    Totals = 0
                };
            }
        }

        public IEnumerable<Result> ValidateLicenseeCode(LicenseeTransferCodeValidateCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
                }

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var results = new List<Result>();
                        while (reader.Read())
                        {
                            results.Add(new Result
                            {
                                ErrorCode = int.Parse(reader["ErrorId"].ToString()),
                                ErrorMessage = reader["ErrorMessage"].ToString()
                            });
                        }


                        return results;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                return new List<Result>();
            }
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
                }

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    var outputParams = db.GetOuputParams();
                    var result = new Result(Converter.ToInt(outputParams["errorcode"]), Converter.ToString2(outputParams["errormessage"]));

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                return new Result
                {
                    ErrorCode = -1,
                    ErrorMessage = "Internal server error."
                };
            }
        }
    }
}
