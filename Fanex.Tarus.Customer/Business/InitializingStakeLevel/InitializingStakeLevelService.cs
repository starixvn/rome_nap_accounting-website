namespace Fanex.Tarus.Customer.Business.InitializingStakeLevel
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Customer.Criteria.InitializingStakeLevel;
    using Fanex.Tarus.Customer.DataAccess.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Requests.InitializingStakeLevel;

    public interface IInitializingStakeLevelService
    {
        FilterModel GetFilterData();

        IEnumerable<BaseModel> GetSportTypes(int productId);

        IEnumerable<BaseModel> GetBetTypes(int productId, string sportTypes);

        StakeLevelResponse GetStakeLevels(GetStakeLevelRequest request);

        ExcelFile GetExcelFile(GetStakeLevelRequest request);
    }
    public class InitializingStakeLevelService : IInitializingStakeLevelService
    {
        private const int CreditCurrency = 1;
        private const int DepositCurrency = 2;
        private readonly IInitStakeLevelDataAccess dataAccess;

        public InitializingStakeLevelService()
        {
            dataAccess = new InitStakeLevelDataAccess();
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
    }
}
