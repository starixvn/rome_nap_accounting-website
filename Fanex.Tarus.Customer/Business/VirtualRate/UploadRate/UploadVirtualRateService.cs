// --------------------------------------------------------------------------
// <copyright file="UploadVirtualRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.VirtualRate.UploadRate
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.Common;
    using Fanex.Tarus.Customer.Constants;
    using Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.DataAccess.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Constants;
    using Fanex.Tarus.Infrastructure.Extensions;
    using NPOI.SS.UserModel;
    using Sunplus.Accounting.Core.Constant;

    /// <summary>
    /// Upload Virtual Rate Service.
    /// </summary>
    public partial class UploadVirtualRateService : IUploadVirtualRateService
    {
        private readonly IUploadRateRepository _uploadRateRepository;

        public UploadVirtualRateService()
            : this(null)
        {
        }

        public UploadVirtualRateService(IUploadRateRepository uploadRateRepository)
        {
            _uploadRateRepository = uploadRateRepository ?? new UploadRateRepository();
        }

        /// <summary>
        /// Checks the import direct account.
        /// </summary>
        /// <param name="fileUploaded">The file uploaded.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// Import Direct Account Result.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public UploadResult CheckImportVirtualRate(HttpPostedFileBase fileUploaded, int userId)
        {
            try
            {
                CheckImportResult checkImportResult = new CheckImportResult
                {
                    FailedInfoList = new List<ImportVirtualRateResult>(),
                    SucceededInfoList = new List<ImportVirtualRateResult>()
                };

                var importList = ReadImportVirtualRateFile(fileUploaded, checkImportResult);
                if (checkImportResult.ResultCode != ErrorCode.NoneError)
                {
                    return GroupCheckImportResult(checkImportResult);
                }

                var uploadCriteria = new InsertTempRateCriteria
                {
                    UserId = userId,
                    CustomerVirtualRateList = CustomerVirtualRateList(importList)
                };

                if (uploadCriteria.CustomerVirtualRateList.Count == 0)
                {
                    checkImportResult.ResultCode = ErrorCode.InvalidFormatFile;
                    checkImportResult.ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidFormat;
                    return GroupCheckImportResult(checkImportResult);
                }

                _uploadRateRepository.DeleteTempRate(new DeleteTempRateCriteria
                {
                    UserId = userId
                });

                var databaseCheckResult = _uploadRateRepository.InsertTempRate(new InsertTempRateCriteria
                {
                    UserId = userId,
                    CustomerVirtualRateList = uploadCriteria.CustomerVirtualRateList
                });

                if (databaseCheckResult.ResultCode != ErrorCode.NoneError)
                {
                    checkImportResult.FailedInfoList.Add(new ImportVirtualRateResult
                    {
                        ResultCode = databaseCheckResult.ResultCode,
                        ResultMessage = databaseCheckResult.ResultMessage
                    });

                    return GroupCheckImportResult(checkImportResult);
                }

                var failListCount = databaseCheckResult.FailedInfoList.Count;
                for (var i = 0; i < failListCount; i++)
                {
                    checkImportResult.FailedInfoList.Add(databaseCheckResult.FailedInfoList[i]);
                }

                databaseCheckResult = _uploadRateRepository.CheckImport(new CheckImportCriteria
                {
                    UserId = userId
                });

                failListCount = databaseCheckResult.FailedInfoList.Count;
                for (var i = 0; i < failListCount; i++)
                {
                    checkImportResult.FailedInfoList.Add(databaseCheckResult.FailedInfoList[i]);
                }

                var successListCount = databaseCheckResult.SucceededInfoList.Count;
                for (var i = 0; i < successListCount; i++)
                {
                    checkImportResult.SucceededInfoList.Add(databaseCheckResult.SucceededInfoList[i]);
                }

                return GroupCheckImportResult(checkImportResult);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the import rate.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public UploadResult UpdateImportRate(UpdateImportRateCriteria criteria)
        {
            try
            {
                var overRowPerUpdating = 10;
                var totalPage = (criteria.TotalSuccessRow / overRowPerUpdating) +
                    (criteria.TotalSuccessRow % overRowPerUpdating != 0 ? 1 : 0);
                CheckImportResult checkImportResult = new CheckImportResult
                {
                    FailedInfoList = new List<ImportVirtualRateResult>()
                };

                var pageSize = overRowPerUpdating - 1;
                for (var i = 0; i < totalPage; i++)
                {
                    var pageUpdateResult = _uploadRateRepository.UpdateImportRate(new UpdateImportRateCriteria
                    {
                        FileName = criteria.FileName,
                        FileSize = criteria.FileSize,
                        Index = i + 1
                    });

                    if (pageUpdateResult.ResultCode != ErrorCode.NoneError)
                    {
                        var startIndex = i * pageSize;
                        var endIndex = startIndex + pageSize;
                        for (var j = startIndex; j < endIndex && j < criteria.SuccessRecords.Count; j++)
                        {
                            checkImportResult.FailedInfoList.Add(criteria.SuccessRecords[j]);
                        }
                    }
                }

                if (checkImportResult.FailedInfoList.Count == 0)
                {
                    _uploadRateRepository.DeleteTempRate(new DeleteTempRateCriteria
                    {
                        UserId = AppSession.AccId
                    });

                    return new UploadResult
                    {
                        ResultCode = ErrorCode.NoneError,
                        ResultMessage = Customer_Labels.VirtualRate_UploadFile_Success
                    };
                }
                else
                {
                    return new UploadResult
                    {
                        ResultCode = ErrorCode.SYSTEM_ERROR,
                        ResultMessage = Customer_Labels.VirtualRate_UploadFile_UpdateFailedSomeAccounts,
                        FailedInfoList = GroupByUsername(checkImportResult.FailedInfoList)
                    };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new UploadResult
                {
                    ResultCode = ErrorCode.SYSTEM_ERROR,
                    ResultMessage = Customer_Labels.VirtualRate_UploadFile_SystemError
                };
            }
        }

        /// <summary>
        /// Deletes the temporary rate.
        /// </summary>
        /// <returns>Delete result.</returns>
        public Result DeleteTempRate()
        {
            try
            {
                return _uploadRateRepository.DeleteTempRate(new DeleteTempRateCriteria
                {
                    UserId = AppSession.AccId
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Reads the import virtual rate file.
        /// </summary>
        /// <param name="fileUploaded">The file uploaded.</param>
        /// <param name="uploadResult">The upload result.</param>
        /// <returns>
        /// Import rate list.
        /// </returns>
        /// <exception cref="System.Exception">Invalid Format Exception.</exception>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<ImportVirtualRateInfo> ReadImportVirtualRateFile(HttpPostedFileBase fileUploaded, CheckImportResult uploadResult)
        {
            IWorkbook workbook = CreateImportVirtualRateFileReader(fileUploaded, uploadResult);
            var importList = new List<ImportVirtualRateInfo>();
            if (uploadResult.ResultCode > 0)
            {
                return importList;
            }

            ISheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();
            var betTypeList = VirtualRateSectionService.BetTypeListByFunctionName(VirtualRateSectionService.UploadVirtualFunction);
            var betTypeCount = betTypeList.Count;

            var customerVirtualRateDataTable = new DataTable();
            customerVirtualRateDataTable.Columns.Add("Username");
            customerVirtualRateDataTable.Columns.Add("Sport");
            customerVirtualRateDataTable.Columns.Add("LiveType");
            for (var i = 0; i < betTypeCount; i++)
            {
                customerVirtualRateDataTable.Columns.Add("BetType" + betTypeList[i].Id);
            }

            if (!rows.MoveNext())
            {
                uploadResult.ResultCode = ErrorCode.SYSTEM_ERROR;
                uploadResult.ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidNumberOfColumn;
                uploadResult.ResultNote = Customer_Labels.VirtualRate_UploadFile_InvalidNumberOfColumn_Note;
                return importList;
            }

            var sportIdFromNameDictionary = VirtualRateSectionService.SportIdFromNameDictionary();
            do
            {
                var row = (NPOI.SS.UserModel.IRow)rows.Current;
                if (row.LastCellNum == 0)
                {
                    uploadResult.ResultCode = ErrorCode.SYSTEM_ERROR;
                    uploadResult.ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidNumberOfColumn;
                    uploadResult.ResultNote = Customer_Labels.VirtualRate_UploadFile_InvalidNumberOfColumn_Note;
                    return importList;
                }

                DataRow dataRow = customerVirtualRateDataTable.NewRow();
                var columnCount = customerVirtualRateDataTable.Columns.Count;
                var count = 0;
                for (int i = 0; i < row.LastCellNum && i < columnCount; i++)
                {
                    ICell cell = row.GetCell(i);
                    count += ReadImportVirtualRateTemplateCell(cell, dataRow, uploadResult, i);
                }

                if (count != row.LastCellNum && row.RowNum > 0)
                {
                    var username = Converter.ToString2(dataRow[0]).Trim();
                    var sportName = Converter.ToString2(dataRow[1]).Trim();
                    var liveType = Converter.ToString2(dataRow[2]).Trim();
                    if (string.IsNullOrEmpty(username))
                    {
                        uploadResult.FailedInfoList.Add(new ImportVirtualRateResult
                        {
                            ResultCode = ErrorCode.InvalidFormatFile,
                            ResultMessage = Customer_Labels.VirtualRate_UploadFile_EmptyCusomer,
                            SportName = sportName,
                            LiveTypeName = liveType
                        });

                        continue;
                    }

                    var sportId = sportIdFromNameDictionary.ContainsKey(sportName) ?
                        sportIdFromNameDictionary[sportName] :
                        0;
                    if (sportId == 0)
                    {
                        uploadResult.FailedInfoList.Add(new ImportVirtualRateResult
                        {
                            ResultCode = ErrorCode.InvalidFormatFile,
                            ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidSport,
                            Username = username,
                            SportName = sportName,
                            LiveTypeName = liveType
                        });

                        continue;
                    }

                    var liveTypeId = LiveTypeId(liveType);
                    if (liveTypeId == LiveTypeConstants.NotLiveType)
                    {
                        uploadResult.FailedInfoList.Add(new ImportVirtualRateResult
                        {
                            ResultCode = ErrorCode.InvalidFormatFile,
                            ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidLiveType,
                            Username = username,
                            SportName = sportName,
                            LiveTypeName = liveType
                        });

                        continue;
                    }

                    var customerVirtualRateModel = new CustomerVirtualRateModel
                    {
                        Username = Converter.ToString2(dataRow[0]).Trim(),
                        SportVirtualRateList = new List<SportVirtualRateModel>
                        {
                             new SportVirtualRateModel
                             {
                                 Id = sportId,
                                 Name = sportName
                             }
                        }
                    };

                    for (var i = 0; i < betTypeCount; i++)
                    {
                        var betType = betTypeList[i];
                        var rateString = Converter.ToString2(dataRow[3 + i]).Trim();
                        if (string.IsNullOrEmpty(rateString))
                        {
                            continue;
                        }

                        decimal valueRate;
                        var oldRateString = rateString;
                        rateString = rateString.EndsWith("%") ?
                            rateString.Substring(0, rateString.Length - 1) :
                            rateString;
                        if (!decimal.TryParse(rateString, out valueRate))
                        {
                            uploadResult.FailedInfoList.Add(new ImportVirtualRateResult
                            {
                                ResultCode = ErrorCode.InvalidFormatFile,
                                ResultMessage = Customer_Labels.VirtualRate_UploadFile_NotNumberRate,
                                Username = username,
                                SportName = sportName,
                                LiveTypeName = liveType,
                                BetTypeName = betType.Name,
                                VirtualRateString = rateString
                            });

                            continue;
                        }

                        if (valueRate % 5 != 0)
                        {
                            uploadResult.FailedInfoList.Add(new ImportVirtualRateResult
                            {
                                ResultCode = ErrorCode.InvalidFormatFile,
                                ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidRate,
                                Username = username,
                                SportName = sportName,
                                LiveTypeName = liveType,
                                BetTypeName = betType.Name,
                                VirtualRateString = oldRateString
                            });

                            continue;
                        }

                        importList.Add(new ImportVirtualRateInfo
                        {
                            Username = username,
                            SportType = sportId,
                            LiveType = liveTypeId,
                            BetType = betType.Id,
                            SportName = sportName,
                            LiveTypeName = liveType,
                            BetTypeName = betType.Name,
                            VirtualRate = valueRate
                        });
                    }
                }
            }
            while (rows.MoveNext());

            var resultList = new List<ImportVirtualRateInfo>();
            var repeatGroups = importList.GroupBy(data => new
            {
                Username = data.Username,
                SportName = data.SportName,
                LiveTypeName = data.LiveTypeName,
                BetTypeName = data.BetTypeName
            }).ToList();

            foreach (var group in repeatGroups)
            {
                var repeatCount = group.Count();
                if (repeatCount != 1)
                {
                    uploadResult.FailedInfoList.Add(new ImportVirtualRateResult
                    {
                        ResultCode = ErrorCode.InvalidFormatFile,
                        ResultMessage = string.Format(Customer_Labels.VirtualRate_UploadFile_RepeatBetType, repeatCount),
                        Username = group.Key.Username,
                        SportName = group.Key.SportName,
                        LiveTypeName = group.Key.LiveTypeName,
                        BetTypeName = group.Key.BetTypeName,
                        VirtualRateString = string.Empty
                    });

                    continue;
                }

                foreach (var data in group)
                {
                    resultList.Add(data);
                }
            }

            return resultList;
        }

        /// <summary>
        /// Creates the import virtual rate file reader.
        /// </summary>
        /// <param name="fileUploaded">The file uploaded.</param>
        /// <param name="uploadResult">The import result.</param>
        /// <returns>
        /// The import virtual rate file reader.
        /// </returns>
        private IWorkbook CreateImportVirtualRateFileReader(HttpPostedFileBase fileUploaded, CheckImportResult uploadResult)
        {
            IWorkbook workbook = null;
            try
            {
                workbook = WorkbookFactory.Create(fileUploaded.InputStream);
                if (workbook.GetSheet(Customer_Labels.VirtualRate_UploadFile_SheetName) == null)
                {
                    uploadResult.ResultCode = ErrorCode.SYSTEM_ERROR;
                    uploadResult.ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidFormatFile;
                    uploadResult.ResultNote = Customer_Labels.VirtualRate_UploadFile_InvalidFormatFile_Note;
                }
            }
            catch (Exception ex)
            {
                uploadResult.ResultCode = ErrorCode.InvalidFormatFile;
                uploadResult.ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidFormat;
                LogExtensions.ExceptionHandler(ex);
            }

            return workbook;
        }

        /// <summary>
        /// Reads the import direct account template cell.
        /// </summary>
        /// <param name="cell">The excel cell.</param>
        /// <param name="dataRow">The data row.</param>
        /// <param name="uploadResult">The upload result.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        /// Number of cell empty.
        /// </returns>
        private int ReadImportVirtualRateTemplateCell(
            ICell cell,
            DataRow dataRow,
            CheckImportResult uploadResult,
            int columnIndex)
        {
            if (cell == null)
            {
                return 1;
            }

            var count = 0;
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    dataRow[columnIndex] = cell.ToString();
                    break;
                default:
                    try
                    {
                        if (string.IsNullOrEmpty(cell.StringCellValue))
                        {
                            count++;
                            break;
                        }

                        dataRow[columnIndex] = cell.StringCellValue;
                    }
                    catch (Exception ex)
                    {
                        uploadResult.ResultCode = ErrorCode.InvalidFormatFile;
                        uploadResult.ResultMessage = Customer_Labels.VirtualRate_UploadFile_InvalidFormat;
                        LogExtensions.ExceptionHandler(ex);
                    }

                    break;
            }

            return count;
        }

        /// <summary>
        /// Gets the live type identifier.
        /// </summary>
        /// <param name="liveType">Type of the live.</param>
        /// <returns>The live type identifier.</returns>
        private int LiveTypeId(string liveType)
        {
            switch (liveType)
            {
                case LiveTypeNameConstants.Live:
                    return LiveTypeConstants.Live;
                case LiveTypeNameConstants.NonLive:
                    return LiveTypeConstants.NonLive;
                default:
                    return LiveTypeConstants.NotLiveType;
            }
        }

        /// <summary>
        /// Gets the customer virtual rate list.
        /// </summary>
        /// <param name="importList">The import list.</param>
        /// <returns>The customer virtual rate list.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<CustomerVirtualRateModel> CustomerVirtualRateList(List<ImportVirtualRateInfo> importList)
        {
            List<CustomerVirtualRateModel> customerVirtualRateList = new List<CustomerVirtualRateModel>();
            var userGroups = importList.GroupBy(data => data.Username);

            var functionName = VirtualRateSectionService.UploadVirtualFunction;
            Dictionary<int, int> betTypeIndexList = VirtualRateSectionService.BetTypeIndexListByFunctionName(functionName);
            foreach (var userGroup in userGroups)
            {
                var customerVirtualRateModel = new CustomerVirtualRateModel
                {
                    Username = userGroup.Key,
                    SportVirtualRateList = new List<SportVirtualRateModel>()
                };

                var sportGroups = userGroup.GroupBy(data => data.SportType);
                foreach (var sportGroup in sportGroups)
                {
                    var sportRate = new SportVirtualRateModel
                    {
                        Id = sportGroup.Key,
                        Name = Sports.SportName(sportGroup.Key),
                        BetTypeList = VirtualRateSectionService.BetTypeListByFunctionName(functionName),
                        LiveVirtualRate = VirtualRateSectionService.DefaultVirtualRateList(betTypeIndexList.Count),
                        NonLiveVirtualRate = VirtualRateSectionService.DefaultVirtualRateList(betTypeIndexList.Count)
                    };

                    foreach (var betType in sportGroup)
                    {
                        if (!betTypeIndexList.ContainsKey(betType.BetType))
                        {
                            continue;
                        }

                        var betTypeIndex = betTypeIndexList[betType.BetType];
                        if (betType.LiveType == LiveTypeConstants.Live)
                        {
                            sportRate.LiveVirtualRate[betTypeIndex].HasData = true;
                            sportRate.LiveVirtualRate[betTypeIndex].PercentRate = betType.VirtualRate;
                        }
                        else
                        {
                            sportRate.NonLiveVirtualRate[betTypeIndex].HasData = true;
                            sportRate.NonLiveVirtualRate[betTypeIndex].PercentRate = betType.VirtualRate;
                        }
                    }

                    customerVirtualRateModel.SportVirtualRateList.Add(sportRate);
                }

                customerVirtualRateList.Add(customerVirtualRateModel);
            }

            return customerVirtualRateList;
        }

        /// <summary>
        /// Groups the check import result.
        /// </summary>
        /// <param name="checkImportResult">The check import result.</param>
        /// <returns>Import result groups.</returns>
        private UploadResult GroupCheckImportResult(CheckImportResult checkImportResult)
        {
            return new UploadResult
            {
                ResultCode = checkImportResult.ResultCode,
                ResultMessage = checkImportResult.ResultMessage,
                ResultNote = checkImportResult.ResultNote,
                FailedInfoList = GroupByUsername(checkImportResult.FailedInfoList),
                SucceededInfoList = GroupByUsername(checkImportResult.SucceededInfoList),
                SuccessRecords = checkImportResult.SucceededInfoList,
                TotalSuccessRow = checkImportResult.SucceededInfoList.Count
            };
        }

        /// <summary>
        /// Groups the by username.
        /// </summary>
        /// <param name="importList">The import list.</param>
        /// <returns>Groups by user name.</returns>
        private List<ImportVirtualRateResultGroup> GroupByUsername(List<ImportVirtualRateResult> importList)
        {
            List<ImportVirtualRateResultGroup> result = new List<ImportVirtualRateResultGroup>();
            if (importList == null)
            {
                return result;
            }

            var groups = importList.GroupBy(data => data.Username);
            foreach (var group in groups)
            {
                result.Add(new ImportVirtualRateResultGroup
                {
                    GroupName = group.Key,
                    ImportVirtualRateResultList = group.ToList()
                });
            }

            return result.OrderBy(data => data.GroupName).ToList();
        }
    }
}
