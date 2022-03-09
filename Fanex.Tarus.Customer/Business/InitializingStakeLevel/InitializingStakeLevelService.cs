namespace Fanex.Tarus.Customer.Business.InitializingStakeLevel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Web;
    using ExcelUtil.Spreadsheet;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Fanex.Tarus.Customer.Criteria.InitializingStakeLevel;
    using Fanex.Tarus.Customer.DataAccess.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Requests.InitializingStakeLevel;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Extensions;
    using NPOI.SS.UserModel;

    public interface IInitializingStakeLevelService
    {
        FilterModel GetFilterData();

        IEnumerable<BaseModel> GetSportTypes(int productId);

        IEnumerable<BaseModel> GetBetTypes(int productId, string sportTypes);

        StakeLevelResponse GetStakeLevels(GetStakeLevelRequest request);

        ExcelFile GetExcelFile(GetStakeLevelRequest request);

        ValidateStakeLevelResponse ValidateSetting(HttpPostedFileBase uploadFile);
    }
    public class InitializingStakeLevelService : IInitializingStakeLevelService
    {
        private const int CreditCurrency = 1;
        private const int DepositCurrency = 2;
        private const string SportCacheKey = "InitCust-Sport-{0}";
        private const string BettypeCacheKey = "InitCust-BetType-{0}";
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
            var criteria = new StakeLevelSelCriteria
            {
                ProductID = request.ProductID,
                SportTypes = request.SportTypes,
                BetTypes = request.BetTypes,
                CreditCurrencies = request.CreditCurrency,
                DepositCurrencies = request.DepositCurrency,
                ExportData = 0,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };

            return dataAccess.GetStakeLevels(criteria);
        }

        public ExcelFile GetExcelFile(GetStakeLevelRequest request)
        {
            var criteria = new StakeLevelSelCriteria
            {
                ProductID = request.ProductID,
                SportTypes = request.SportTypes,
                BetTypes = request.BetTypes,
                CreditCurrencies = request.CreditCurrency,
                DepositCurrencies = request.DepositCurrency,
                ExportData = 1,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };

            var stakeLevels = dataAccess.GetStakeLevels(criteria).StakeLevels.ToList();

            #region ExcelTypes

            ExcelFile excelFile = new ExcelFile();
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            string sheetName = "StakeLevel";
            ExcelWorksheet workSheet = worksheets.Add(sheetName);
            int startRow = 0;
            int startColumn = 0;
            int currentRow = startRow;
            int currentColumn = startColumn;

            ExcelCell currentCell;

            #endregion ExcelTypes

            var excelUtils = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils();
            var headerTextStyle = excelUtils.GetStyleTitle(Color.FromArgb(181, 196, 208));

            #region Header

            // set columns width
            int fontWidth = 30;
            int maxColumnIndex = 9;
            for (int i = 0; i < maxColumnIndex; i++)
            {
                workSheet.Columns[startColumn + i].Width = 150 * fontWidth;
            }

            // excel header
            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "SportType";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "Bet Type";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "Credit Currency";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "Deposit Currency";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "New Min Bet";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "New Max Bet";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "Current Min Bet";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "Current Max Bet";
            currentCell.Style = headerTextStyle;
            currentColumn++;

            currentCell = workSheet.Cells[currentRow, currentColumn];
            currentCell.Value = "Game Group Id";
            currentCell.Style = headerTextStyle;

            #endregion Header

            #region Body
            // style
            var textStyle = excelUtils.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Center);

            StakeLevelModel stakeLevel = null;
            for (int i = 0; i < stakeLevels.Count; i++)
            {
                currentRow++;
                currentColumn = startColumn;

                stakeLevel = stakeLevels[i];

                // SportType
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.SportTypeId;
                currentCell.Style = textStyle;
                currentColumn++;

                // BetType
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.BetTypeId;
                currentCell.Style = textStyle;
                currentColumn++;

                // Credit Currency
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.CreditCurrency;
                currentCell.Style = textStyle;
                currentColumn++;

                // Deposit Currency
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.DepositCurrency;
                currentCell.Style = textStyle;
                currentColumn++;

                // New Min Bet
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.NewMinBet;
                currentCell.Style = textStyle;
                currentColumn++;

                // New Max Bet
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.NewMaxBet;
                currentCell.Style = textStyle;
                currentColumn++;

                // Current Min Bet
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.MinBet;
                currentCell.Style = textStyle;
                currentColumn++;

                // Current Max Bet
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.MaxBet;
                currentCell.Style = textStyle;
                currentColumn++;

                // Consolidated Group
                currentCell = workSheet.Cells[currentRow, currentColumn];
                currentCell.Value = stakeLevel.GameGroupId;
                currentCell.Style = textStyle;
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
                rows.MoveNext();
                while (rows.MoveNext())
                {
                    rowCount++;
                    IRow row = (IRow)rows.Current;
                    var stakeLevel = new StakeLevelModel
                    {
                        SportTypeId = string.IsNullOrWhiteSpace(row.GetCell(0).ToString()) ? 0 : Int32.Parse(row.GetCell(0).ToString()),
                        BetTypeId = string.IsNullOrWhiteSpace(row.GetCell(1).ToString()) ? 0 : Int32.Parse(row.GetCell(1).ToString()),
                        SiteId = string.IsNullOrWhiteSpace(row.GetCell(2).ToString()) ? 0 : Int32.Parse(row.GetCell(2).ToString()),
                        CreditCurrency = HttpUtility.HtmlEncode(row.GetCell(3).ToString()),
                        DepositCurrency = HttpUtility.HtmlEncode(row.GetCell(4).ToString()),
                        NewMinBet = string.IsNullOrWhiteSpace(row.GetCell(5).ToString()) ? 0 : Decimal.Parse(row.GetCell(5).ToString()),
                        NewMaxBet = string.IsNullOrWhiteSpace(row.GetCell(6).ToString()) ? 0 : Decimal.Parse(row.GetCell(6).ToString()),
                        MinBet = string.IsNullOrWhiteSpace(row.GetCell(7).ToString()) ? 0 : Decimal.Parse(row.GetCell(7).ToString()),
                        MaxBet = string.IsNullOrWhiteSpace(row.GetCell(8).ToString()) ? 0 : Decimal.Parse(row.GetCell(8).ToString()),
                        GameGroupId = string.IsNullOrWhiteSpace(row.GetCell(9).ToString()) ? 0 : Int32.Parse(row.GetCell(9).ToString()),
                    };

                    stakeLevel.SportType = GetSportName(stakeLevel.SportTypeId);
                    stakeLevel.BetType = GetBetTypeName(stakeLevel.BetTypeId);
                    siteIds.Add(stakeLevel.SiteId);
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

                return dataAccess.CheckSetting(criteria);
             
            }
            catch (Exception ex)
            {
                var result = new Result(1, "Invalid file uploaded. Error at line: " + rowCount.ToString() + ". " + ex.Message);
                LogExtensions.ExceptionHandler(ex);

                return new ValidateStakeLevelResponse {
                    Error = result
                };
            }
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
    }
}
