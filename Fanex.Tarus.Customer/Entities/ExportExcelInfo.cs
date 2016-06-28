// --------------------------------------------------------------------------
// <copyright file="ExportExcelInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities
{
    using System;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Export Excel Info.
    /// </summary>
    [Serializable]
    public class ExportExcelInfo
    {
        public int StartRow { get; set; }

        public int StartColumn { get; set; }

        public int CurrentRow { get; set; }

        public int CurrentColumn { get; set; }

        public int MaxColumnIndex { get; set; }

        public HSSFWorkbook ExcelFile { get; set; }

        public ISheet Worksheet { get; set; }

        public IRow Row { get; set; }

        public void CreateSheet(string sheetName)
        {
            Worksheet = ExcelFile.CreateSheet(sheetName);
        }

        public void ResetCurrentColumn()
        {
            CurrentColumn = StartColumn;
        }

        public void MoveToNextRow()
        {
            CurrentRow++;
            ResetCurrentColumn();
        }

        public void SetMaxColumnIndex(int columnSpan)
        {
            MaxColumnIndex = StartColumn + columnSpan;
        }

        public void CreateCurrentRow()
        {
            Row = Worksheet.CreateRow(CurrentRow);
        }

        public void RenderCurrentCell(object cellValue, ICellStyle cellStyle)
        {
            CurrentColumn = NpoiExcelExtensions.RenderCell(Row, CurrentColumn, cellValue, cellStyle);
        }

        public void RenderCellRange(object cellValue, ICellStyle cellStyle, int endRow, int endColumn)
        {
            NpoiExcelExtensions.RenderMergeCell(this, endRow, endColumn, cellValue, cellStyle);
        }

        public void RenderCellRangeBySpanning(object cellValue, ICellStyle cellStyle, int rowSpan, int columnSpan)
        {
            NpoiExcelExtensions.RenderMergeCell(this, CurrentRow + rowSpan, CurrentColumn + columnSpan, cellValue, cellStyle);
        }

        public void AddMergedRegionBySpanning(int rowSpan, int colSpan)
        {
            var cellRange = new CellRangeAddress(CurrentRow, CurrentRow + rowSpan, CurrentColumn, CurrentColumn + colSpan);
            Worksheet.AddMergedRegion(cellRange);
        }

        public void SetColumnWidthByFontSize(int columnIndex, int columnWidth)
        {
            Worksheet.SetColumnWidth(columnIndex, columnWidth * NpoiExcelExtensions.FontWidth);
        }

        public void ResponseFile(string fileName)
        {
            ExcelFile.ToResponseStream(fileName);
        }
    }
}