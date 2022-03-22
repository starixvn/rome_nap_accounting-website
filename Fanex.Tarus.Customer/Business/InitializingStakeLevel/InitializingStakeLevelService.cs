namespace Fanex.Tarus.Customer.Business.InitializingStakeLevel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Fanex.Tarus.Customer.Criteria.InitializingStakeLevel;
    using Fanex.Tarus.Customer.DataAccess.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Requests.InitializingStakeLevel;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Security.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public interface IInitializingStakeLevelService
    {
        FilterModel GetFilterData();

        IEnumerable<BaseModel> GetSportTypes(int productId);

        IEnumerable<BaseModel> GetBetTypes(int productId, string sportTypes);

        StakeLevelResponse GetStakeLevels(GetStakeLevelRequest request);

        IWorkbook GetExcelFile();

        ValidateStakeLevelResponse ValidateSetting(HttpPostedFileBase uploadFile);

        Result InsertSetting(IEnumerable<StakeLevelModel> stakeLevels, int userId, string ip);
    }
    public class InitializingStakeLevelService : IInitializingStakeLevelService
    {
        private const int CreditCurrency = 1;
        private const int DepositCurrency = 2;
        private const string SportCacheKey = "InitCust-Sport-{0}";
        private const string BettypeCacheKey = "InitCust-BetType-{0}";
        private const string _cacheGetDataCriteria = "CachedStakeLevelCriteria";
        private readonly IInitStakeLevelDataAccess dataAccess;
        private readonly MemoryCache _MemoryCache;

        public InitializingStakeLevelService()
        {
            dataAccess = new InitStakeLevelDataAccess();
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

        public StakeLevelResponse GetStakeLevels(GetStakeLevelRequest request)
        {
            var criteria = new StakeLevelSelCriteria(request);

            HttpContext.Current.Session[_cacheGetDataCriteria] = request;
            return dataAccess.GetStakeLevels(criteria);
        }

        public IWorkbook GetExcelFile()
        {
            var criteria = GetCachedCriteria();
            criteria.ExportData = 1;

            var stakeLevels = dataAccess.GetStakeLevels(criteria).StakeLevels.ToList();

            #region ExcelTypes

            IWorkbook excelFile = new HSSFWorkbook();
            string sheetName = "StakeLevel";
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
            var headerValues = new string[10] { "SportType", "Bet Type", "Site", "Credit Currency", "Deposit Currency", "New Min Bet", "New Max Bet", "Current Min Bet", "Current Min Bet", "Game Group Id" };
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

            StakeLevelModel stakeLevel = null;
            for (int i = 0; i < stakeLevels.Count; i++)
            {
                currentRow++;
                var row = workSheet.CreateRow(currentRow);

                stakeLevel = stakeLevels[i];
                var rowValues = new string[10]
                        {
                            stakeLevel.SportTypeId.ToString(),
                            stakeLevel.BetTypeId.ToString(),
                            stakeLevel.Site,
                            stakeLevel.CreditCurrency,
                            stakeLevel.DepositCurrency,
                            stakeLevel.NewMinBet.ToString("G29"),
                            stakeLevel.NewMaxBet.ToString("G29"),
                            stakeLevel.MinBet.ToString("G29"),
                            stakeLevel.MaxBet.ToString("G29"),
                            stakeLevel.GameGroupId.ToString()
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

        public ValidateStakeLevelResponse ValidateSetting(HttpPostedFileBase uploadFile)
        {
            var stakeLevels = new List<StakeLevelModel>();
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
                        return new ValidateStakeLevelResponse
                        {
                            Error = checkRequiredFieldsResult
                        };
                    }

                    var stakeLevel = new StakeLevelModel
                    {
                        SportTypeId = Int32.Parse(row.GetCell(0).ToString()),
                        BetTypeId = Int32.Parse(row.GetCell(1).ToString()),
                        SiteId = row.GetCell(2) == null || string.IsNullOrWhiteSpace(row.GetCell(2).ToString()) ? 0 : Int32.Parse(row.GetCell(2).ToString()),
                        CreditCurrency = row.GetCell(3) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(3).ToString().Replace(" ", String.Empty)),
                        DepositCurrency = row.GetCell(4) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(4).ToString().Replace(" ", String.Empty)),
                        NewMinBet = string.IsNullOrWhiteSpace(row.GetCell(5).ToString()) ? 0 : Decimal.Parse(row.GetCell(5).ToString()),
                        NewMaxBet = string.IsNullOrWhiteSpace(row.GetCell(6).ToString()) ? 0 : Decimal.Parse(row.GetCell(6).ToString()),
                        MinBet = string.IsNullOrWhiteSpace(row.GetCell(7).ToString()) ? 0 : Decimal.Parse(row.GetCell(7).ToString()),
                        MaxBet = string.IsNullOrWhiteSpace(row.GetCell(8).ToString()) ? 0 : Decimal.Parse(row.GetCell(8).ToString()),
                        GameGroupId = row.GetCell(9) == null || string.IsNullOrWhiteSpace(row.GetCell(9).ToString()) ? 0 : Int32.Parse(row.GetCell(9).ToString()),
                    };

                    stakeLevel.SportType = GetSportName(stakeLevel.SportTypeId);
                    stakeLevel.BetType = GetBetTypeName(stakeLevel.BetTypeId);
                    siteIds.Add(stakeLevel.SiteId);
                    if (!sportTypeIds.Contains(stakeLevel.SportTypeId))
                    {
                        sportTypeIds.Add(stakeLevel.SportTypeId);
                    }

                    if (!betTypeIds.Contains(stakeLevel.BetTypeId))
                    {
                        betTypeIds.Add(stakeLevel.BetTypeId);
                    }

                    if (!string.IsNullOrWhiteSpace(stakeLevel.CreditCurrency))
                    {
                        creditCurrency = string.Join(",", creditCurrency, stakeLevel.CreditCurrency);
                    }

                    if (!string.IsNullOrWhiteSpace(stakeLevel.DepositCurrency))
                    {
                        depositCurrency = string.Join(",", depositCurrency, stakeLevel.DepositCurrency);
                    }

                    stakeLevels.Add(stakeLevel);
                }

                var sites = dataAccess.GetSites(string.Join(",", siteIds));
                foreach (var stakeLevel in stakeLevels)
                {
                    stakeLevel.Site = sites.FirstOrDefault(site => site.Id == stakeLevel.SiteId)?.Name;
                }

                var criteria = new StakeLevelCheckCriteria
                {
                    StakeLevels = stakeLevels
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

                return new ValidateStakeLevelResponse
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

        public Result InsertSetting(IEnumerable<StakeLevelModel> stakeLevels, int userId, string ip)
        {
            var criteria = new StakeLevelInsCriteria
            {
                StakeLevels = stakeLevels,
                UserID = userId,
                IP = ip
            };

            return dataAccess.InsertSetting(criteria);
        }

        public StakeLevelSelCriteria GetCachedCriteria()
        {
            if (HttpContext.Current.Session[_cacheGetDataCriteria] == null || HttpContext.Current.Session[_cacheGetDataCriteria].GetType() != typeof(GetStakeLevelRequest))
            {
                throw new InvalidOperationException("Filter Data in Session can not be null. Must Submit before export excel.");
            }

            return new StakeLevelSelCriteria((GetStakeLevelRequest)HttpContext.Current.Session[_cacheGetDataCriteria]);
        }

        public bool CheckPermission(int accId)
        {
            var permissionService = new PermissionService();
            var pageAccess = permissionService.GetPageAccess(accId);
            var pathActionName = "InitStakeLevel/Index";
            bool hasPermission = permissionService.GetHostPermission(pathActionName, pageAccess);

            return hasPermission;
        }
    }
}
