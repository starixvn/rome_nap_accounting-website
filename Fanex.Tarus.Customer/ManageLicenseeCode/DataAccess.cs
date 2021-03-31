namespace Fanex.Tarus.Customer.ManageLicenseeCode
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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

        NonQueryResponse ExecuteNonQueryWithResultValidate(ICriteria criteria);


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
                                Status = reader["LicenseeStatus"].ToString(),
                                FromDate = Convert.IsDBNull(reader["FromDate"]) ? string.Empty : Convert.ToDateTime(reader["FromDate"]).ToString("MMM dd, yyyy", DateTimeFormatInfo.InvariantInfo),
                                ToDate = Convert.IsDBNull(reader["ToDate"]) ? string.Empty : Convert.ToDateTime(reader["ToDate"]).ToString("MMM dd, yyyy", DateTimeFormatInfo.InvariantInfo),
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

        public NonQueryResponse ExecuteNonQueryWithResultValidate(ICriteria criteria)
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
                        var validateResults = new List<Result>();
                        while (reader.Read())
                        {
                            validateResults.Add(new Result
                            {
                                ErrorCode = int.Parse(reader["ErrorId"].ToString()),
                                ErrorMessage = reader["ErrorMessage"].ToString()
                            });
                        }

                        var outputParams = db.GetOuputParams();
                        var result = new Result(Converter.ToInt(outputParams["errorcode"]), Converter.ToString2(outputParams["errormessage"]));

                        return new NonQueryResponse
                        {
                            ValidateResponse = validateResults,
                            NonQueryResult = result
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                var result = new Result
                {
                    ErrorCode = -1,
                    ErrorMessage = "Internal server error."
                };

                return new NonQueryResponse
                {
                    ValidateResponse = new List<Result>(),
                    NonQueryResult = result
                };
            }
        }
    }
}
