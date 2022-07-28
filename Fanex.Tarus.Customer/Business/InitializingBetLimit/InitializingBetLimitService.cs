namespace Fanex.Tarus.Customer.Business.InitializingBetLimit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Fanex.Tarus.Customer.Criteria.InitializingBetLimit;
    using Fanex.Tarus.Customer.DataAccess.InitializingBetLimit;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingBetLimit;
    using Fanex.Tarus.Customer.Requests.InitializingBetLimit;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Security.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public interface IInitializingBetLimitService
    {
        FilterModel GetFilterData();

        IEnumerable<BaseModel> GetSportTypes(int productId);

        IEnumerable<BaseModel> GetBetTypes(int productId, string sportTypes);

        BetLimitResponse GetBetLimits(GetBetLimitRequest request);

        IWorkbook GetExcelFile();

        ValidateBetLimitResponse ValidateSetting(HttpPostedFileBase uploadFile);

        Result InsertSetting(IEnumerable<BetLimitModel> betLimitModels, int userId, string ip);
    }
    public class InitializingBetLimitService : IInitializingBetLimitService
    {
        private const int CreditCurrency = 1;
        private const int DepositCurrency = 2;
        private const string SportCacheKey = "InitCust-Sport-{0}";
        private const string BettypeCacheKey = "InitCust-BetType-{0}";
        private const string _cacheGetDataCriteria = "CachedBetLimitCriteria";
        private readonly IInitBetLimitDataAccess dataAccess;
        private readonly MemoryCache _MemoryCache;

        public InitializingBetLimitService()
        {
            dataAccess = new InitBetLimitDataAccess();
            _MemoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public IEnumerable<BaseModel> GetBetTypes(int productId, string sportTypes)
        {
            return dataAccess.GetBettypes(productId, sportTypes);
        }

        public FilterModel GetFilterData()
        {
            var products = dataAccess.GetProducts();
            var creditCurrencies = dataAccess.GetCurrency(CreditCurrency);
            var depositCurrencies = dataAccess.GetCurrency(DepositCurrency);

            return new FilterModel
            {
                Products = products,
                CreditCurrencies = creditCurrencies,
                DepositCurrencies = depositCurrencies
            };
        }

        public IEnumerable<BaseModel> GetSportTypes(int productId)
        {
            return dataAccess.GetSportTypes(productId);
        }

        public BetLimitResponse GetBetLimits(GetBetLimitRequest request)
        {
            var criteria = new BetLimitSelCriteria(request);

            HttpContext.Current.Session[_cacheGetDataCriteria] = request;
            return dataAccess.GetBetLimits(criteria);
        }

        public IWorkbook GetExcelFile()
        {
            var criteria = GetCachedCriteria();
            criteria.ExportData = 1;

            var betLimits = dataAccess.GetBetLimits(criteria).BetLimits.ToList();

            #region ExcelTypes

            IWorkbook excelFile = new HSSFWorkbook();
            string sheetName = "BetLimit";
            var workSheet = excelFile.CreateSheet(sheetName);
            int currentRow = 0;

            #endregion ExcelTypes


            var boldFont = excelFile.CreateFont();
            boldFont.FontName = "Tahoma";
            boldFont.FontHeightInPoints = 8;
            boldFont.Boldweight = (short)FontBoldWeight.Bold;
            boldFont.Color = HSSFColor.White.Index;

            #region Header

            var headerCellStyle = excelFile.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.Center;
            headerCellStyle.SetFont(boldFont);
            headerCellStyle.FillForegroundColor = HSSFColor.LightBlue.Index;
            headerCellStyle.FillPattern = FillPattern.SolidForeground;
            headerCellStyle.BorderBottom = headerCellStyle.BorderLeft = headerCellStyle.BorderRight = headerCellStyle.BorderTop = BorderStyle.Thin;
            headerCellStyle.BottomBorderColor = headerCellStyle.TopBorderColor = headerCellStyle.LeftBorderColor = headerCellStyle.RightBorderColor = HSSFColor.Black.Index;

            var headerRow = workSheet.CreateRow(currentRow);
            var headerValues = new string[10] { "SportType", "Bet Type", "Site", "Credit Currency", "Deposit Currency", "New Min Bet", "New Max Bet", "Current Min Bet", "Current Max Bet", "Game Group Id" };
            for (int i = 0; i < headerValues.Length; i++)
            {
                workSheet.SetColumnWidth(i, 20*256);
                var currentCell = headerRow.CreateCell(i);
                currentCell.SetCellValue(headerValues[i]);
                currentCell.CellStyle = headerCellStyle;
            }
            #endregion Header

            #region Body
            var normalFont = excelFile.CreateFont();
            normalFont.FontName = "Tahoma";
            normalFont.FontHeightInPoints = 8;
            normalFont.Color = HSSFColor.Black.Index;

            var cellStyle = excelFile.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.SetFont(normalFont);
            cellStyle.BorderBottom = cellStyle.BorderLeft = cellStyle.BorderRight = cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BottomBorderColor = cellStyle.TopBorderColor = cellStyle.LeftBorderColor = cellStyle.RightBorderColor = HSSFColor.Black.Index;
            cellStyle.VerticalAlignment = VerticalAlignment.Top;

            BetLimitModel betLimitModel = null;
            for (int i = 0; i < betLimits.Count; i++)
            {
                currentRow++;
                var row = workSheet.CreateRow(currentRow);

                betLimitModel = betLimits[i];
                var rowValues = new string[14]
                        {
                            betLimitModel.SportTypeId.ToString(),
                            betLimitModel.BetTypeId.ToString(),
                            betLimitModel.Site,
                            betLimitModel.CreditCurrency,
                            betLimitModel.DepositCurrency,
                            betLimitModel.NewMinBet.ToString("G29"),
                            betLimitModel.NewMaxBet.ToString("G29"),
                            betLimitModel.NewMaxPerMatch.ToString("G29"),
                            betLimitModel.NewMaxPerBall.ToString("G29"),
                            betLimitModel.MinBet.ToString("G29"),
                            betLimitModel.MaxBet.ToString("G29"),
                            betLimitModel.MaxPerMatch.ToString("G29"),
                            betLimitModel.MaxPerBall.ToString("G29"),
                            betLimitModel.GameGroupId.ToString()
                        };
                for (int j = 0; j < rowValues.Length; j++)
                {
                    var currentCell = row.CreateCell(j);
                    currentCell.SetCellValue(rowValues[j]);
                    currentCell.CellStyle = cellStyle;
                }
            }

            #endregion Body

            return excelFile;
        }

        public ValidateBetLimitResponse ValidateSetting(HttpPostedFileBase uploadFile)
        {
            var betLimits = new List<BetLimitModel>();
            var siteIds = new List<int>();
            int rowCount = 1;
            try
            {
                IWorkbook workbook = WorkbookFactory.Create(uploadFile.InputStream);
                ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                var sportTypeIds = new List<int>();
                var betTypeIds = new List<int>();
                var creditCurrency = "";
                var depositCurrency = "";
                rows.MoveNext();
                while (rows.MoveNext())
                {
                    rowCount++;
                    IRow row = (IRow)rows.Current;
                    if (!IsHaveDataRow(row))
                    {
                        continue;
                    }

                    var checkRequiredFieldsResult = CheckRequiredField(row, rowCount);
                    if (checkRequiredFieldsResult != null)
                    {
                        return new ValidateBetLimitResponse
                        {
                            Error = checkRequiredFieldsResult
                        };
                    }

                    var betLimit = new BetLimitModel
                    {
                        SportTypeId = int.Parse(row.GetCell(0).ToString()),
                        BetTypeId = int.Parse(row.GetCell(1).ToString()),
                        SiteId = row.GetCell(2) == null || string.IsNullOrWhiteSpace(row.GetCell(2).ToString()) ? 0 : int.Parse(row.GetCell(2).ToString()),
                        CreditCurrency = row.GetCell(3) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(3).ToString().Replace(" ", string.Empty)),
                        DepositCurrency = row.GetCell(4) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(4).ToString().Replace(" ", string.Empty)),
                        NewMinBet = string.IsNullOrWhiteSpace(row.GetCell(5).ToString()) ? 0 : decimal.Parse(row.GetCell(5).ToString()),
                        NewMaxBet = string.IsNullOrWhiteSpace(row.GetCell(6).ToString()) ? 0 : decimal.Parse(row.GetCell(6).ToString()),
                        NewMaxPerMatch = string.IsNullOrWhiteSpace(row.GetCell(7).ToString()) ? 0 : decimal.Parse(row.GetCell(7).ToString()),
                        NewMaxPerBall = string.IsNullOrWhiteSpace(row.GetCell(8).ToString()) ? 0 : decimal.Parse(row.GetCell(8).ToString()),
                        MinBet = row.GetCell(9) == null || string.IsNullOrWhiteSpace(row.GetCell(9).ToString()) ? 0 : decimal.Parse(row.GetCell(9).ToString()),
                        MaxBet = row.GetCell(10) == null || string.IsNullOrWhiteSpace(row.GetCell(10).ToString()) ? 0 : decimal.Parse(row.GetCell(10).ToString()),
                        MaxPerMatch = row.GetCell(11) == null || string.IsNullOrWhiteSpace(row.GetCell(11).ToString()) ? 0 : decimal.Parse(row.GetCell(11).ToString()),
                        MaxPerBall = row.GetCell(12) == null || string.IsNullOrWhiteSpace(row.GetCell(12).ToString()) ? 0 : decimal.Parse(row.GetCell(12).ToString()),
                        GameGroupId = row.GetCell(13) == null || string.IsNullOrWhiteSpace(row.GetCell(13).ToString()) ? 0 : int.Parse(row.GetCell(13).ToString()),
                    };

                    betLimit.SportType = GetSportName(betLimit.SportTypeId);
                    betLimit.BetType = GetBetTypeName(betLimit.BetTypeId);
                    siteIds.Add(betLimit.SiteId);
                    if (!sportTypeIds.Contains(betLimit.SportTypeId))
                    {
                        sportTypeIds.Add(betLimit.SportTypeId);
                    }

                    if (!betTypeIds.Contains(betLimit.BetTypeId))
                    {
                        betTypeIds.Add(betLimit.BetTypeId);
                    }

                    if (!string.IsNullOrWhiteSpace(betLimit.CreditCurrency))
                    {
                        creditCurrency = string.Join(",", creditCurrency, betLimit.CreditCurrency);
                    }

                    if (!string.IsNullOrWhiteSpace(betLimit.DepositCurrency))
                    {
                        depositCurrency = string.Join(",", depositCurrency, betLimit.DepositCurrency);
                    }

                    betLimits.Add(betLimit);
                }

                var sites = dataAccess.GetSites(string.Join(",", siteIds));
                foreach (var betLimit in betLimits)
                {
                    betLimit.Site = sites.FirstOrDefault(site => site.Id == betLimit.SiteId)?.Name;
                }

                var criteria = new BetLimitCheckCriteria
                {
                    BetLimits = betLimits
                };

                var result = dataAccess.CheckSetting(criteria);
                result.CreditCurrencies = creditCurrency.Split(',').Distinct();
                result.DepositCurrencies = depositCurrency.Split(',').Distinct();
                result.SportTypeIds = sportTypeIds;
                result.BetTypeIds = betTypeIds;

                return result;
            }
            catch (Exception ex)
            {
                var result = new Result(1, "Invalid file uploaded. Error at row: " + rowCount.ToString() + ". " + ex.Message);
                LogExtensions.ExceptionHandler(ex);

                return new ValidateBetLimitResponse
                {
                    Error = result
                };
            }
        }

        private bool IsHaveDataRow(IRow row)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (row.GetCell(i) != null && !string.IsNullOrWhiteSpace(row.GetCell(i).ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private Result CheckRequiredField(IRow row, int rowCount)
        {
            if (row.GetCell(0) == null || string.IsNullOrWhiteSpace(row.GetCell(0).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The Sport Type is required field.");
            }

            if (row.GetCell(1) == null || string.IsNullOrWhiteSpace(row.GetCell(1).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The Bet Type is required field.");
            }

            if ((row.GetCell(3) == null || string.IsNullOrWhiteSpace(row.GetCell(3).ToString()))
                && (row.GetCell(4) == null || string.IsNullOrWhiteSpace(row.GetCell(4).ToString())))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The Currency is required field.");
            }

            if (row.GetCell(5) == null || string.IsNullOrWhiteSpace(row.GetCell(5).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The New Min Bet is required field.");
            }

            if (row.GetCell(6) == null || string.IsNullOrWhiteSpace(row.GetCell(6).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The New Max Bet is required field.");
            }

            return null;
        }

        private string GetSportName(object sportId)
        {
            var key = string.Format(SportCacheKey, sportId);
            if (!_MemoryCache.TryGet(key, out string sportName))
            {
                sportName = CacheRef.GetSportName(sportId);
                _MemoryCache.Set(key, sportName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return sportName;
        }

        private string GetBetTypeName(object bettypeId)
        {
            var key = string.Format(BettypeCacheKey, bettypeId);
            if (!_MemoryCache.TryGet(key, out string bettypeName))
            {
                bettypeName = CacheRef.GetBettype(bettypeId);
                _MemoryCache.Set(key, bettypeName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return bettypeName;
        }

        public Result InsertSetting(IEnumerable<BetLimitModel> betLimitModels, int userId, string ip)
        {
            var criteria = new BetLimitInsCriteria
            {
                BetLimits = betLimitModels,
                UserID = userId,
                IP = ip
            };

            return dataAccess.InsertSetting(criteria);
        }

        public BetLimitSelCriteria GetCachedCriteria()
        {
            if (HttpContext.Current.Session[_cacheGetDataCriteria] == null || HttpContext.Current.Session[_cacheGetDataCriteria].GetType() != typeof(GetBetLimitRequest))
            {
                throw new InvalidOperationException("Filter Data in Session can not be null. Must Submit before export excel.");
            }

            return new BetLimitSelCriteria((GetBetLimitRequest)HttpContext.Current.Session[_cacheGetDataCriteria]);
        }

        public bool CheckPermission(int accId)
        {
            var permissionService = new PermissionService();
            var pageAccess = permissionService.GetPageAccess(accId);
            var pathActionName = "InitBetLimit/Index";
            bool hasPermission = permissionService.GetHostPermission(pathActionName, pageAccess);

            return hasPermission;
        }
    }
}
