namespace Fanex.BetList.Accounting.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using BetList.Core.TicketHelper;
    using Core;
    using Criteria;
    using Models;
    using Models.Excel;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Repositories;

    public class SystemParlayExcel : ISystemParlayExcel
    {
        private readonly ISystemParlayRepository _systemParlayRepository;

        private HSSFWorkbook _workbook;
        private ISheet _sheet;

        private int _startColumnIndex = 0;
        private int _currentRowIndex = 0;

        public SystemParlayExcel(ISystemParlayRepository systemParlayRepository = null)
        {
            _systemParlayRepository = systemParlayRepository ?? new SystemParlayRepository();
        }

        public void ExportCombinationDetails(BetListSystemParlayByRefnoCriteria criteria, Stream stream)
        {
            var balanceUp = ReportService.BalanceUp;
            criteria.MoveBetDay = balanceUp.MoveBetsDay;
            criteria.BalanceUpDay = balanceUp.BalanceUpDay;

            var model = _systemParlayRepository.CombinationDetails(criteria);

            _workbook = new HSSFWorkbook();
            _sheet = _workbook.CreateSheet("Combination details");

            int[] columnsWidth = new int[] { 200, 300, 120, 120, 120, 150 };
            string[] columnsName = new string[] { "Date", "Event", "Odds", "Stake", "Win Loss", "Status" };

            RenderTitle("Combination details", columnsWidth);
            RenderHeader(columnsName);
            RenderCombinationDetails(model.EventList);
            RenderCombinationDetailsSubTotal(model.SubTotal, "Sub total");

            _workbook.Write(stream);
        }

        public void ExportSystemParlayDetails(BetListSystemParlayDetailsCriteria criteria, Stream stream)
        {
            var balanceUp = ReportService.BalanceUp;
            criteria.MoveBetDay = balanceUp.MoveBetsDay;
            criteria.BalanceUpDay = balanceUp.BalanceUpDay;

            var model = _systemParlayRepository.GetSystemParlayDetailExcel(criteria);

            _workbook = new HSSFWorkbook();
            _sheet = _workbook.CreateSheet("System Parlay Details");

            int[] columnsWidth = new int[] { 150, 300, 120, 120, 120, 150 };
            string[] columnsName = new string[] { "Date", "Event", "Odds", "Stake", "Win Loss", "Status" };

            RenderTitle("System Parlay Details", columnsWidth);
            RenderHeader(columnsName);
            //RenderSystemParlayDetails(model.Events);

            //RenderCombinationDetailsSubTotal(model.Outstanding, "Outstanding");
            //_currentRowIndex++;

            RenderCombinationDetailsSubTotal(model.Total, "Total");

            _workbook.Write(stream);
        }

        private void RenderCombinationDetails(IList<Event> events)
        {
            if (events == null || events.Count == 0)
            {
                return;
            }

            IRow row;
            ExcelCell cell;
            Event eventObject;
            Match matchObject;
            int columnIndex, eventLength;
            StringBuilder matchBuilder;

            ICellStyle textCenter = NpoiExcelUtils.TextStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Center);
            ICellStyle textLeft = NpoiExcelUtils.TextStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Left);
            ICellStyle number = NpoiExcelUtils.DecimalStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, false, false);
            ICellStyle negativeNumber = NpoiExcelUtils.DecimalStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, true, true);

            eventLength = events.Count;
            for (int i = 0; i < eventLength; i++)
            {
                eventObject = events[i];
                columnIndex = _startColumnIndex;
                matchBuilder = new StringBuilder(128);

                if (i == 0)
                {
                    ExcelCellRange dateCellRange = new ExcelCellRange(
                        _currentRowIndex,
                        _currentRowIndex + eventLength - 1,
                        columnIndex,
                        columnIndex,
                        eventObject.WinLossDate.ToString(),
                        textCenter);

                    dateCellRange.RenderCellRange(_sheet);
                }

                columnIndex++;
                row = _sheet.GetRow(_currentRowIndex);

                for (int j = 0, max = eventObject.MatchList.Count; j < max; j++)
                {
                    matchObject = eventObject.MatchList[j];
                    matchBuilder
                        .Append(CacheRef.GetTeamName(matchObject.HomeID))
                        .Append(" - ")
                        .Append(CacheRef.GetTeamName(matchObject.AwayID));

                    if (j < max - 1)
                    {
                        matchBuilder.Append("\n");
                    }
                }

                cell = new ExcelCell(columnIndex++, matchBuilder.ToString(), textLeft);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, eventObject.Odds, number);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, eventObject.Stake, number);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, eventObject.WinLoss, negativeNumber);
                cell.RenderCell(row);

                cell = new ExcelCell(columnIndex++, eventObject.Status, textCenter);
                cell.RenderCell(row);

                _currentRowIndex++;
            }
        }

        private void RenderCombinationDetailsSubTotal(decimal subTotal, string columnTitle)
        {
            int columnIndex = _startColumnIndex;

            ICellStyle textRight = NpoiExcelUtils.TextStyle(_workbook, NpoiExcelUtils.HeaderTitleColorIndex, true, HorizontalAlignment.Right);
            ICellStyle negativeNumber = NpoiExcelUtils.DecimalStyle(_workbook, NpoiExcelUtils.HeaderTitleColorIndex, true, true);

            ExcelCellRange textSubTotal = new ExcelCellRange(_currentRowIndex, _currentRowIndex, columnIndex, columnIndex + 3, columnTitle, textRight);
            int lastColumn = textSubTotal.RenderCellRange(_sheet);
            columnIndex += lastColumn;

            IRow row = _sheet.GetRow(_currentRowIndex);

            ExcelCell valueSubTotal = new ExcelCell(columnIndex++, subTotal, negativeNumber);
            valueSubTotal.RenderCell(row);

            ExcelCell emptyCell = new ExcelCell(columnIndex++, string.Empty, textRight);
            emptyCell.RenderCell(row);
        }

        private void RenderSystemParlayDetails(IList<EventExcel> events)
        {
            if (events == null || events.Count == 0)
            {
                return;
            }

            int columnIndex;
            int eventLength;
            decimal subTotal;

            EventExcel loseEvent = null;
            ICellStyle textCenter = NpoiExcelUtils.TextStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Center);

            var groupEvents = events.GroupBy(e => e.WinLossDate);

            foreach (var grp in groupEvents)
            {
                subTotal = 0;
                columnIndex = _startColumnIndex;
                eventLength = grp.Count();

                ExcelCellRange dateCellRange = new ExcelCellRange(
                    _currentRowIndex,
                    _currentRowIndex + eventLength - 1,
                    columnIndex,
                    columnIndex,
                    grp.Key,
                    textCenter);

                columnIndex = dateCellRange.RenderCellRange(_sheet);

                foreach (var eventObject in grp)
                {
                    subTotal += eventObject.WinLoss + eventObject.PlayerComm;

                    if (eventObject.Status.Equals("LOSE", StringComparison.OrdinalIgnoreCase))
                    {
                        loseEvent = eventObject;
                        continue;
                    }

                    RenderSystemParlayDetailsMatches(eventObject, columnIndex, _currentRowIndex);

                    columnIndex = _startColumnIndex + 1;
                    _currentRowIndex++;
                }

                // Render LOSE
                if (loseEvent != null)
                {
                    RenderSystemParlayDetailsMatches(loseEvent, columnIndex, _currentRowIndex, true);
                    loseEvent = null;
                    _currentRowIndex++;
                }

                RenderCombinationDetailsSubTotal(subTotal, "Sub total");
                _currentRowIndex++;
            }
        }

        private void RenderSystemParlayDetailsMatches(EventExcel eventObject, int columnIndex, int rowIndex, bool isLoseEvent = false)
        {
            IRow row = _sheet.GetRow(rowIndex);
            ExcelCell cell;

            Match matchObject;
            StringBuilder matchBuilder = new StringBuilder(128);

            ICellStyle textCenter = NpoiExcelUtils.TextStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Center);
            ICellStyle textLeft = NpoiExcelUtils.TextStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, false, HorizontalAlignment.Left);
            ICellStyle number = NpoiExcelUtils.DecimalStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, false, false);
            ICellStyle negativeNumber = NpoiExcelUtils.DecimalStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, true, true);

            if (isLoseEvent)
            {
                matchBuilder.Append("Total of losing tickets");
            }
            else
            {
                for (int j = 0, max = eventObject.Matches.Count; j < max; j++)
                {
                    matchObject = eventObject.Matches[j];
                    matchBuilder
                        .Append(CacheRef.GetTeamName(matchObject.HomeID))
                        .Append(" - ")
                        .Append(CacheRef.GetTeamName(matchObject.AwayID));

                    if (j < max - 1)
                    {
                        matchBuilder.Append("\n");
                    }
                }
            }

            cell = new ExcelCell(columnIndex++, matchBuilder.ToString(), textLeft);
            cell.RenderCell(row);

            if (isLoseEvent)
            {
                cell = new ExcelCell(columnIndex++, "-", number);
            }
            else
            {
                cell = new ExcelCell(columnIndex++, eventObject.Odds, number);
            }

            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, eventObject.Stake, number);
            cell.RenderCell(row);

            string winLossPlayerComm = string.Format(
                "{0}\n{1}", 
                eventObject.WinLoss.ToString("N2"), 
                eventObject.PlayerComm.ToString("N2"));
            cell = new ExcelCell(columnIndex++, winLossPlayerComm, negativeNumber);
            cell.RenderCell(row);

            cell = new ExcelCell(columnIndex++, eventObject.Status, textCenter);
            cell.RenderCell(row);
        }

        private void RenderTitle(string title, int[] columnsWidth)
        {
            NpoiExcelUtils.InitColumnWidthByFontWidth(_sheet, _startColumnIndex, columnsWidth);

            // Title.
            ICellStyle titleStyle = NpoiExcelUtils.TextStyle(_workbook, NpoiExcelUtils.BackgroundColorIndex, true, HorizontalAlignment.Left);
            ExcelCellRange cellrange = new ExcelCellRange(_currentRowIndex, _currentRowIndex + 1, _startColumnIndex, columnsWidth.Length - 1, title, titleStyle);
            cellrange.RenderCellRange(_sheet);
            _currentRowIndex += 2;

            // One more empty row.
            ExcelCellRange emptyRow = new ExcelCellRange(_currentRowIndex, _currentRowIndex, _startColumnIndex, columnsWidth.Length - 1, string.Empty, null);
            _currentRowIndex++;
        }

        private void RenderHeader(string[] columnsName)
        {
            int columnIndex = _startColumnIndex;
            IRow row = _sheet.CreateRow(_currentRowIndex);
            row.Height = 400;
            ICellStyle headerStyle = NpoiExcelUtils.HeaderStyle(_workbook);

            ExcelCell cell;
            foreach (string col in columnsName)
            {
                cell = new ExcelCell(columnIndex++, col, headerStyle);
                cell.RenderCell(row);
            }

            _currentRowIndex++;
        }
    }
}
