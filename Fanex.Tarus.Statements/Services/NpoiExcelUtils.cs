namespace Fanex.Tarus.Statements.Services
{
    using System.Drawing;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public static class NpoiExcelUtils
    {
        public const int FontWidth = 30;
        public const int FontSize = 8;

        public static readonly Color BackgroundColor = Color.White;
        public static readonly Color HeaderTitleColor = Color.FromArgb(188, 202, 226);
        public static readonly Color EvenRowColor = Color.FromArgb(228, 242, 253);
        public static readonly Color OddRowColor = Color.White;

        public static readonly short BackgroundColorIndex = HSSFColor.White.Index;
        public static readonly short HeaderTitleColorIndex = HSSFColor.Pink.Index;
        public static readonly short EvenRowColorIndex = HSSFColor.Coral.Index;

        private const string FontName = "Tahoma";
        private const string IntNumberFormat = "#,##0";
        private const string TwoDecimalDigitNumber = "##,##0.00";
        private const string NegativeNumberFormat = "#,##0.00;[RED]-#,##0.00";

        private const int VerticalAlignTop = 0;
        private const int VerticalAlignCenter = 1;
        private const int VerticalAlignBottom = 2;
        private const int VerticalAlignJustify = 3;
        private const int VerticalAlignDistributed = 4;

        public static void SetColorAtIndex(HSSFWorkbook workbook, Color color, short colorIndex)
        {
            HSSFPalette palette = workbook.GetCustomPalette();
            palette.SetColorAtIndex(colorIndex, color.R, color.G, color.B);
        }

        public static void InitColumnWidthByFontWidth(ISheet sheet, int columnIndex, int[] columnsWidth)
        {
            var columnLength = columnsWidth.Length;
            for (var i = 0; i < columnLength; i++)
            {
                sheet.SetColumnWidth(columnIndex++, columnsWidth[i] * FontWidth);
            }
        }

        public static ICellStyle HeaderStyle(HSSFWorkbook excelFile)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            HSSFPalette palette = excelFile.GetCustomPalette();
            IFont font = excelFile.CreateFont();
            palette.SetColorAtIndex(HSSFColor.Pink.Index, HeaderTitleColor.R, HeaderTitleColor.G, HeaderTitleColor.B);
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Black.Index;
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

        public static ICellStyle TextStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            HorizontalAlignment align,
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
            cellStyle.Alignment = align;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        public static ICellStyle DecimalStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            bool negativedFormat = true,
            short fontColor = 8)
        {
            return negativedFormat ?
                StyleWithFormula(excelFile, cellColor, bold, NegativeNumberFormat, fontColor) :
                StyleWithFormula(excelFile, cellColor, bold, TwoDecimalDigitNumber, fontColor);
        }

        public static ICellStyle IntStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            HorizontalAlignment align,
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

            cellStyle.Alignment = align;
            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.DataFormat = dataFormat.GetFormat(IntNumberFormat);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        private static ICellStyle SetBorderToCell(ICellStyle cellStyle)
        {
            var thin = BorderStyle.Thin;
            cellStyle.BorderBottom = thin;
            cellStyle.BorderLeft = thin;
            cellStyle.BorderRight = thin;
            cellStyle.BorderTop = thin;

            var grey80PercentIndex = HSSFColor.Grey80Percent.Index;
            cellStyle.BottomBorderColor = grey80PercentIndex;
            cellStyle.LeftBorderColor = grey80PercentIndex;
            cellStyle.RightBorderColor = grey80PercentIndex;
            cellStyle.TopBorderColor = grey80PercentIndex;

            return cellStyle;
        }

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
    }
}