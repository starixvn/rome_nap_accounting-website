// -----------------------------------------------------------------------
// <copyright file="NpoiExcelExtensions.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Customer.Entities
{
    using System;
    using System.Drawing;
    using System.Web;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NpoiExcelExtensions
    {
        public const int FontWidth = 30;
        public const int FontHeight = 20;
        public const int FontSize = 8;
        public const int MaxRowPerSheet = 20000;

        public static readonly Color BackGroundColor = Color.White;
        public static readonly Color ReportTitleColor = Color.FromArgb(255, 217, 97);
        public static readonly Color HeaderTitleColor = Color.FromArgb(60, 94, 165);
        public static readonly Color EvenRowColor = Color.FromArgb(228, 242, 253);
        public static readonly Color OddRowColor = Color.White;
        public static readonly Color SubTotalRowColor = Color.FromArgb(244, 244, 190);
        public static readonly Color TotalRowColor = Color.FromArgb(244, 244, 190);
        public static readonly Color GrandTotalRowColor = Color.FromArgb(219, 225, 187);
        public static readonly Color GrandTotalSeparatorColor = Color.FromArgb(255, 102, 0);
        public static readonly Color RevenueLicenseeColor = Color.FromArgb(213, 132, 52);
        public static readonly Color ParentColor = Color.FromArgb(194, 222, 236);
        public static readonly Color ChildColor = Color.FromArgb(194, 226, 236);
        public static readonly Color NoteInformationCellColor = Color.FromArgb(181, 196, 208);
        public static readonly Color BorderColor = Color.Gray;

        public static readonly short BackGroundColorIndex = HSSFColor.White.Index;

        private const string FontName = "Tahoma";
        private const string IntNumberFormat = "#,##0";
        private const string TwoDecimalDigitNumber = "##,##0.00";
        private const string IntPercentNumber = "#,##0%";
        private const string NegativeNumberFormat = "[>=0]#,##0{0}_);[RED][<0]-#,##0{1}";
        private const string NormalNumberFormat = "#,##0{0}_);-#,##0{1}";
        private const string PositiveNumberFormat = "[RED][>0]#,##0{0}_);[<0]-#,##0{1};#,##0{2}";

        private const int VerticalAlignTop = 0;
        private const int VerticalAlignCenter = 1;
        private const int VerticalAlignBottom = 2;
        private const int VerticalAlignJustify = 3;
        private const int VerticalAlignDistributed = 4;

        /// <summary>
        /// Style for header.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle HeaderStyle(HSSFWorkbook excelFile)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            HSSFPalette palette = excelFile.GetCustomPalette();
            IFont font = excelFile.CreateFont();
            palette.SetColorAtIndex(HSSFColor.Pink.Index, (byte)60, (byte)94, (byte)165);
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.White.Index;
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;

            cellStyle.FillForegroundColor = HSSFColor.Pink.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Style for text.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The color of cell.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="align">Align text(value is right, left or center).</param>
        /// <param name="fontColor">The color of font.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle TextStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            Alignment align,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;

            switch (align)
            {
                case Alignment.Right:
                    cellStyle.Alignment = HorizontalAlignment.Right;
                    break;
                case Alignment.Left:
                    cellStyle.Alignment = HorizontalAlignment.Left;
                    break;
                case Alignment.Center:
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    break;
            }

            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Style for decimal value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle DecimalStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            short fontColor = 8)
        {
            return StyleWithFormula(excelFile, cellColor, bold, TwoDecimalDigitNumber, fontColor);
        }

        /// <summary>
        /// Style for integer percent value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle IntPercentStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            short fontColor = 8)
        {
            return StyleWithFormula(excelFile, cellColor, bold, IntPercentNumber, fontColor);
        }

        /// <summary>
        /// Style for integer value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle IntStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            short fontColor = 8)
        {
            return StyleWithFormula(excelFile, cellColor, bold, IntNumberFormat, fontColor);
        }

        /// <summary>
        /// Style for integer value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="align">Align text(value is right, left or center).</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle IntStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            Alignment align,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            IDataFormat dataFormat = excelFile.CreateDataFormat();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.Alignment = ConvertAlignment(align, cellStyle.Alignment);
            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.DataFormat = dataFormat.GetFormat(IntNumberFormat);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Render cell.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="endRow">The next row.</param>
        /// <param name="endColumn">The next column.</param>
        /// <param name="cellValue">The label of cell.</param>
        /// <param name="cellStyle">The style of cell.</param>
        public static void RenderCell(
            ExportExcelInfo exportExcelInfo,
            int endRow,
            int endColumn,
            object cellValue,
            ICellStyle cellStyle)
        {
            RenderCell(exportExcelInfo, exportExcelInfo.CurrentRow, endRow, endColumn, cellValue, cellStyle);
        }

        public static void RenderCurrentCell(
            ExportExcelInfo exportExcelInfo,
            object cellValue,
            ICellStyle cellStyle)
        {
            exportExcelInfo.Row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ??
                  exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICell cell = exportExcelInfo.Row.GetCell(exportExcelInfo.CurrentColumn) ??
                exportExcelInfo.Row.CreateCell(exportExcelInfo.CurrentColumn);
            SetCellValue(cell, cellValue);
            cell.CellStyle = cellStyle;
            exportExcelInfo.CurrentColumn++;
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <param name="endColumn">The end column.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        public static void RenderCell(
            ExportExcelInfo exportExcelInfo,
            int startRow,
            int endRow,
            int endColumn,
            object cellValue,
            ICellStyle cellStyle)
        {
            for (var i = startRow; i <= endRow; i++)
            {
                exportExcelInfo.Row = exportExcelInfo.Worksheet.GetRow(i) ??
                    exportExcelInfo.Worksheet.CreateRow(i);

                for (var j = exportExcelInfo.CurrentColumn; j <= endColumn; j++)
                {
                    ICell cell = exportExcelInfo.Row.GetCell(j) ??
                        exportExcelInfo.Row.CreateCell(j);
                    if (i == startRow &&
                        j == exportExcelInfo.CurrentColumn)
                    {
                        SetCellValue(cell, cellValue);
                    }

                    if (i == startRow ||
                        i == endRow ||
                        j == exportExcelInfo.CurrentColumn ||
                        j == endColumn)
                    {
                        cell.CellStyle = cellStyle;
                    }
                }
            }

            exportExcelInfo.Worksheet.AddMergedRegion(new CellRangeAddress(
                startRow,
                endRow,
                exportExcelInfo.CurrentColumn,
                endColumn));
            exportExcelInfo.CurrentColumn = endColumn + 1;
        }

        public static void ResponseStreamExcel(this IWorkbook workbook, string fileName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
            workbook.Write(response.OutputStream);
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="cell">The cell of row.</param>
        /// <param name="cellValue">The cell value.</param>
        public static void SetCellValue(ICell cell, object cellValue)
        {
            if (cellValue is string)
            {
                cell.SetCellValue(Convert.ToString(cellValue));
            }
            else if (cellValue is int)
            {
                cell.SetCellValue(Convert.ToInt32(cellValue));
            }
            else if (cellValue is long)
            {
                cell.SetCellValue(Convert.ToInt64(cellValue));
            }
            else if (cellValue is double)
            {
                cell.SetCellValue(Convert.ToDouble(cellValue));
            }
            else if (cellValue is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(cellValue));
            }
            else if (cellValue is bool)
            {
                cell.SetCellValue(Convert.ToBoolean(cellValue));
            }
            else if (cellValue is System.DateTime)
            {
                cell.SetCellValue(Convert.ToDateTime(cellValue));
            }
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="row">The row of report.</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>Current column index.</returns>
        public static int RenderCell(IRow row, int currentColumn, object cellValue, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(currentColumn);
            SetCellValue(cell, cellValue);
            cell.CellStyle = cellStyle;
            currentColumn += 1;
            return currentColumn;
        }

        /// <summary>
        /// Renders the merge cell.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="endRow">The end row.</param>
        /// <param name="endColumn">The end column.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>Current column index.</returns>
        public static int RenderMergeCell(
            ExportExcelInfo exportExcelInfo,
            int endRow,
            int endColumn,
            object cellValue,
            ICellStyle cellStyle)
        {
            ICell cell = exportExcelInfo.Row.CreateCell(exportExcelInfo.CurrentColumn);
            SetCellValue(cell, cellValue);
            cell.CellStyle = cellStyle;
            exportExcelInfo.Worksheet.AddMergedRegion(new CellRangeAddress(exportExcelInfo.CurrentRow, endRow, exportExcelInfo.CurrentColumn, endColumn));
            exportExcelInfo.CurrentColumn = endColumn + 1;
            return exportExcelInfo.CurrentColumn;
        }

        /// <summary>
        /// Exports Excel file to response stream.
        /// </summary>
        /// <param name="workbook">The workbook contains the data.</param>
        /// <param name="fileName">The file name.</param>
        public static void ToResponseStream(this IWorkbook workbook, string fileName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
            workbook.Write(response.OutputStream);
        }

        /// <summary>
        /// Set border to cell.
        /// </summary>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>The style of cell.</returns>
        private static ICellStyle SetBorderToCell(ICellStyle cellStyle)
        {
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.BottomBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.LeftBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.RightBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.TopBorderColor = HSSFColor.Grey80Percent.Index;

            return cellStyle;
        }

        /// <summary>
        /// Get style with formula.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="dataFormula">The data formula.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>
        /// The cell style.
        /// </returns>
        private static ICellStyle StyleWithFormula(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            string dataFormula,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            IDataFormat dataFormat = excelFile.CreateDataFormat();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.DataFormat = dataFormat.GetFormat(dataFormula);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Converts the alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <param name="currentAlign">The current align.</param>
        /// <returns>The horizontal alignment.</returns>
        private static HorizontalAlignment ConvertAlignment(
            Alignment alignment,
            HorizontalAlignment currentAlign)
        {
            switch (alignment)
            {
                case Alignment.Right:
                    return HorizontalAlignment.Right;
                case Alignment.Left:
                    return HorizontalAlignment.Left;
                case Alignment.Center:
                    return HorizontalAlignment.Center;
                default:
                    return currentAlign;
            }
        }
    }
}
