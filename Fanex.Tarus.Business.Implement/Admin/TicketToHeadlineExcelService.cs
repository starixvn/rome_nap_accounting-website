using Fanex.Tarus.DataAccess.Implement.Admin;
using Fanex.Tarus.DataAccess.Interface.Admin;
using Fanex.Tarus.Web.Models.Admin;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;

namespace Fanex.Tarus.Business.Implement.Admin
{
    public class TicketToHeadlineExcelService
    {
        private ITicketToHeadlineRepository _ticketToHeadlineRepository;

        private IWorkbook _workbook;

        private ISheet _sheet;

        private TicketToHeadlineViewModel _data;

        private List<TicketToHeadline> _normalHeadlineData;

        private List<TicketToHeadline> _vipHeadlineData;

        private List<TicketToHeadline> _errorData;

        private ICellStyle _headerCellStyle;

        private ICellStyle _oddCellStyle;

        private ICellStyle _evenCellStyle;

        private ICellStyle _oddErrorCellStyle;

        private ICellStyle _evenErrorCellStyle;

        private IFont _normalFont;

        private IFont _boldFont;

        private IFont _errorFont;

        private int _currentRow;

        private int _maxRowPerSheet = 1000;

        private bool toggleFlag = true;

        public TicketToHeadlineExcelService(TicketToHeadlineViewModel data)
        {
            this._data = data;
            _normalHeadlineData = new List<TicketToHeadline>();
            _vipHeadlineData = new List<TicketToHeadline>();
            _errorData = new List<TicketToHeadline>();
            this._workbook = new HSSFWorkbook();
            this._ticketToHeadlineRepository = new TicketToHeadlineRepository();
        }

        public IWorkbook Export()
        {
            CreateFont();
            CreateCellStyles();
            FilterData();
            if (_vipHeadlineData.Count > 0)
            {
                ExportData("VIP", _vipHeadlineData);
            }

            if (_normalHeadlineData.Count > 0)
            {
                ExportData("Normal", _normalHeadlineData);
            }

            if (_errorData.Count > 0)
            {
                ExportData("Error", _errorData);
            }

            return this._workbook;
        }

        private void ExportData(string sheetName, List<TicketToHeadline> data)
        {
            int sheetNo = 1;
            this._currentRow = 0;
            this._sheet = this._workbook.CreateSheet(string.Format("{0} ({1})", sheetName, sheetNo++));
            this.RenderHeader();
            string cacheGroupHeadline = string.Empty;
            toggleFlag = true;
            foreach (var dataRow in data)
            {
                if (this._currentRow > _maxRowPerSheet)
                {
                    ColumnAutoWidth();
                    this._sheet = this._workbook.CreateSheet(string.Format("{0} ({1})", sheetName, sheetNo++));
                    this._currentRow = 0;
                    this.RenderHeader();
                }

                var currentRow = this._sheet.CreateRow(this._currentRow++);
                string[] rowValues;
                if (sheetName != "Error")
                {
                    rowValues = new string[9]
                        {
                            dataRow.RowNum.ToString(),
                            dataRow.GroupHeadline,
                            dataRow.HeadlineType,
                            dataRow.Currency,
                            dataRow.Super,
                            dataRow.Master,
                            dataRow.Agent,
                            dataRow.Member,
                            dataRow.RefNo.ToString()
                        };
                }
                else
                {
                    rowValues = new string[9]
                       {
                            dataRow.RowNum.ToString(),
                            dataRow.ErrorMessage,
                            dataRow.ErrorMessage,
                            dataRow.ErrorMessage,
                            dataRow.ErrorMessage,
                            dataRow.ErrorMessage,
                            dataRow.ErrorMessage,
                            dataRow.Member,
                            dataRow.RefNo.ToString()
                       };
                }
                
                toggleFlag = cacheGroupHeadline != dataRow.GroupHeadline || dataRow.ErrorCode != 0 ? !toggleFlag : toggleFlag;
                for (int i = 0; i < rowValues.Length; i++)
                {
                    var currentCell = currentRow.CreateCell(i);
                    currentCell.SetCellValue(rowValues[i]);
                    currentCell.CellStyle = (dataRow.ErrorCode != 0 && i >= 1 && i <= 6) ? (this._currentRow % 2 != 0 ? this._evenErrorCellStyle : this._oddErrorCellStyle) : (this.toggleFlag ? this._evenCellStyle : this._oddCellStyle);
                }

                cacheGroupHeadline = dataRow.GroupHeadline;
                GenerateMergeRegion(dataRow);
            }

            ColumnAutoWidth();
        }

        private void GenerateMergeRegion(TicketToHeadline dataRow)
        {
            if (dataRow.ErrorCode != 0)
            {
                this._sheet.AddMergedRegion(new CellRangeAddress(this._currentRow - 1, this._currentRow - 1, 1, 6));
                return;
            }

            if (dataRow.MergeRowNumGroupHeadline > 0)
            {
                this._sheet.AddMergedRegion(new CellRangeAddress(this._currentRow - 1, this._currentRow - 1 + dataRow.MergeRowNumGroupHeadline - 2, 1, 1));
                this._sheet.AddMergedRegion(new CellRangeAddress(this._currentRow - 1, this._currentRow - 1 + dataRow.MergeRowNumGroupHeadline - 2, 2, 2));
            }

            if (dataRow.MergeRowNumSuper > 0)
            {
                this._sheet.AddMergedRegion(new CellRangeAddress(this._currentRow - 1, this._currentRow - 1 + dataRow.MergeRowNumSuper - 2, 3, 3));
                this._sheet.AddMergedRegion(new CellRangeAddress(this._currentRow - 1, this._currentRow - 1 + dataRow.MergeRowNumSuper - 2, 4, 4));
            }

            if (dataRow.MergeRowNumMaster > 0)
            {
                this._sheet.AddMergedRegion(new CellRangeAddress(this._currentRow - 1, this._currentRow - 1 + dataRow.MergeRowNumMaster - 2, 5, 5));
            }

            if (dataRow.MergeRowNumAgent > 0)
            {
                this._sheet.AddMergedRegion(new CellRangeAddress(this._currentRow - 1, this._currentRow - 1 + dataRow.MergeRowNumAgent - 2, 6, 6));
            }
        }

        private void RenderHeader()
        {
            if (this._sheet == null)
            {
                return;
            }

            var headerRow = this._sheet.CreateRow(this._currentRow++);
            var headerValues = new string[9] { "#", "Group Headline", "Type", "Currency", "Super", "Master", "Agent", "Member", "Ref No" };
            for (int i = 0; i < headerValues.Length; i++)
            {
                var currentCell = headerRow.CreateCell(i);
                currentCell.SetCellValue(headerValues[i]);
                currentCell.CellStyle = this._headerCellStyle;
            }
        }

        private void CreateFont()
        {
            this._normalFont = this._workbook.CreateFont();
            this._normalFont.FontName = "Tahoma";
            this._normalFont.FontHeightInPoints = 8;
            this._normalFont.Color = HSSFColor.Black.Index;

            this._boldFont = this._workbook.CreateFont();
            this._boldFont.FontName = "Tahoma";
            this._boldFont.FontHeightInPoints = 8;
            this._boldFont.Boldweight = (short)FontBoldWeight.Bold;
            this._boldFont.Color = HSSFColor.White.Index;

            this._errorFont = this._workbook.CreateFont();
            this._errorFont.FontName = "Tahoma";
            this._errorFont.FontHeightInPoints = 8;
            this._errorFont.Boldweight = (short)FontBoldWeight.Bold;
            this._errorFont.Color = HSSFColor.Red.Index;
        }

        private void CreateCellStyles()
        {
            this._headerCellStyle = this._workbook.CreateCellStyle();
            this._headerCellStyle.Alignment = HorizontalAlignment.Center;
            this._headerCellStyle.SetFont(this._boldFont);
            this._headerCellStyle.FillForegroundColor = HSSFColor.LightBlue.Index;
            this._headerCellStyle.FillPattern = FillPattern.SolidForeground;
            this._headerCellStyle.BorderBottom = this._headerCellStyle.BorderLeft = this._headerCellStyle.BorderRight = this._headerCellStyle.BorderTop = BorderStyle.Thin;
            this._headerCellStyle.BottomBorderColor = this._headerCellStyle.TopBorderColor = this._headerCellStyle.LeftBorderColor = this._headerCellStyle.RightBorderColor = HSSFColor.Black.Index;

            this._oddCellStyle = this._workbook.CreateCellStyle();
            this._oddCellStyle.Alignment = HorizontalAlignment.Center;
            this._oddCellStyle.SetFont(this._normalFont);
            this._oddCellStyle.FillForegroundColor = 43;
            this._oddCellStyle.FillPattern = FillPattern.SolidForeground;
            this._oddCellStyle.BorderBottom = this._oddCellStyle.BorderLeft = this._oddCellStyle.BorderRight = this._oddCellStyle.BorderTop = BorderStyle.Thin;
            this._oddCellStyle.BottomBorderColor = this._oddCellStyle.TopBorderColor = this._oddCellStyle.LeftBorderColor = this._oddCellStyle.RightBorderColor = HSSFColor.Black.Index;
            this._oddCellStyle.VerticalAlignment = VerticalAlignment.Top;


            this._evenCellStyle = this._workbook.CreateCellStyle();
            this._evenCellStyle.Alignment = HorizontalAlignment.Center;
            this._evenCellStyle.SetFont(this._normalFont);
            this._evenCellStyle.BorderBottom = this._evenCellStyle.BorderLeft = this._evenCellStyle.BorderRight = this._evenCellStyle.BorderTop = BorderStyle.Thin;
            this._evenCellStyle.BottomBorderColor = this._evenCellStyle.TopBorderColor = this._evenCellStyle.LeftBorderColor = this._evenCellStyle.RightBorderColor = HSSFColor.Black.Index;
            this._evenCellStyle.VerticalAlignment = VerticalAlignment.Top;

            this._oddErrorCellStyle = this._workbook.CreateCellStyle();
            this._oddErrorCellStyle.Alignment = HorizontalAlignment.Center;
            this._oddErrorCellStyle.SetFont(this._errorFont);
            this._oddErrorCellStyle.FillForegroundColor = 43;
            this._oddErrorCellStyle.FillPattern = FillPattern.SolidForeground;
            this._oddErrorCellStyle.BorderBottom = this._oddErrorCellStyle.BorderLeft = this._oddErrorCellStyle.BorderRight = this._oddErrorCellStyle.BorderTop = BorderStyle.Thin;
            this._oddErrorCellStyle.BottomBorderColor = this._oddErrorCellStyle.TopBorderColor = this._oddErrorCellStyle.LeftBorderColor = this._oddErrorCellStyle.RightBorderColor = HSSFColor.Black.Index;
            this._oddErrorCellStyle.VerticalAlignment = VerticalAlignment.Top;

            this._evenErrorCellStyle = this._workbook.CreateCellStyle();
            this._evenErrorCellStyle.Alignment = HorizontalAlignment.Center;
            this._evenErrorCellStyle.SetFont(this._errorFont);
            this._evenErrorCellStyle.BorderBottom = this._oddErrorCellStyle.BorderLeft = this._oddErrorCellStyle.BorderRight = this._oddErrorCellStyle.BorderTop = BorderStyle.Thin;
            this._evenErrorCellStyle.BottomBorderColor = this._oddErrorCellStyle.TopBorderColor = this._oddErrorCellStyle.LeftBorderColor = this._oddErrorCellStyle.RightBorderColor = HSSFColor.Black.Index;
            this._evenErrorCellStyle.VerticalAlignment = VerticalAlignment.Top;
        }

        private void FilterData()
        {
            foreach (var dataRow in this._data.TicketToHeadlineCollection)
            {
                if (dataRow.ErrorCode != 0)
                {

                    _errorData.Add(dataRow);
                }
                else if (dataRow.HeadlineType == "VIP")
                {
                    _vipHeadlineData.Add(dataRow);
                }
                else
                {
                    _normalHeadlineData.Add(dataRow);
                }
            }

            _ticketToHeadlineRepository.RePaginationData(_normalHeadlineData, this._maxRowPerSheet);
            _ticketToHeadlineRepository.RePaginationData(_vipHeadlineData, this._maxRowPerSheet);
            _ticketToHeadlineRepository.RePaginationData(_errorData, this._maxRowPerSheet);
        }

        private void ColumnAutoWidth()
        {
            this._sheet.AutoSizeColumn(0);

            for (int i = 1; i < 10; i++)
            {
                this._sheet.SetColumnWidth(i, 5000);
            }
        }
    }
}
