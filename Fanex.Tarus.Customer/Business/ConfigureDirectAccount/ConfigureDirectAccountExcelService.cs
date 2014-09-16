// --------------------------------------------------------------------------
// <copyright file="ConfigureDirectAccountExcelService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.ConfigureDirectAccount
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using Fanex.Common;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Extensions;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Accounting.Core.Constant;

    /// <summary>
    /// Configure Direct Account Service.
    /// </summary>
    public partial class ConfigureDirectAccountService
    {
        /// <summary>
        /// Downloads the import direct account template.
        /// </summary>
        public void DownloadImportDirectAccountTemplate()
        {
            try
            {
                ExportExcelInfo exportExcelInfo = new ExportExcelInfo
                {
                    ExcelFile = new HSSFWorkbook()
                };

                exportExcelInfo.CreateSheet(Customer_Labels.Direct_Account_Template);
                ExportImportDirectAccountTemplateHeader(exportExcelInfo);
                ExportImportDirectAccountTemplateContent(exportExcelInfo);
                exportExcelInfo.ResponseFile(Customer_Labels.ConfigureDirectAccount_Import_TemplateName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Reads the import direct account file.
        /// </summary>
        /// <param name="fileUploaded">The file uploaded.</param>
        /// <param name="importResult">The import result.</param>
        /// <returns>
        /// Username list.
        /// </returns>
        /// <exception cref="System.Exception">Invalid Format Exception.</exception>
        private List<string> ReadImportDirectAccountFile(HttpPostedFileBase fileUploaded, ImportDirectAccountResult importResult)
        {
            IWorkbook workbook = CreateImportDirectAccountFileReader(fileUploaded, importResult);
            var usernameList = new List<string>();
            if (importResult.ImportResultCode > 0)
            {
                return usernameList;
            }

            ISheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();
            var usernameDataTable = new DataTable();
            usernameDataTable.Columns.Add("Username");

            while (rows.MoveNext())
            {
                IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                DataRow dataRow = usernameDataTable.NewRow();
                var count = 0;
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);
                    count += ReadImportDirectAccountTemplateCell(cell, dataRow, importResult, i);
                }

                if (count != row.LastCellNum && row.RowNum > 0)
                {
                    usernameDataTable.Rows.Add(dataRow);
                    usernameList.Add(Converter.ToString2(dataRow[0]));
                }
            }

            return usernameList;
        }

        /// <summary>
        /// Creates the import direct account file reader.
        /// </summary>
        /// <param name="fileUploaded">The file uploaded.</param>
        /// <param name="importResult">The import result.</param>
        /// <returns>The import direct account file reader.</returns>
        private IWorkbook CreateImportDirectAccountFileReader(HttpPostedFileBase fileUploaded, ImportDirectAccountResult importResult)
        {
            IWorkbook workbook = null;
            try
            {
                workbook = WorkbookFactory.Create(fileUploaded.InputStream);
            }
            catch (Exception ex)
            {
                importResult.ImportResultCode = ErrorCode.InvalidFormatFile;
                importResult.ImportResultMessage = Customer_Labels.ConfigureDirectAccount_Import_InvalidFormat;
                LogExtensions.ExceptionHandler(ex);
            }

            return workbook;
        }

        private void ExportImportDirectAccountTemplateHeader(ExportExcelInfo exportExcelInfo)
        {
            exportExcelInfo.SetColumnWidthByFontSize(0, 160);
            var headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile);
            exportExcelInfo.CreateCurrentRow();
            NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, Customer_Labels.Username, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the content of the import direct account template.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportImportDirectAccountTemplateContent(ExportExcelInfo exportExcelInfo)
        {
            var textStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackGroundColorIndex, false, Alignment.Left);
            string[] usernameList = Customer_Labels.ConfigureDirectAccount_Import_SampleUserNames.Split(',');
            for (var i = 0; i < usernameList.Length; i++)
            {
                exportExcelInfo.CreateCurrentRow();
                exportExcelInfo.ResetCurrentColumn();
                NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, usernameList[i], textStyle);
                exportExcelInfo.CurrentRow++;
            }
        }

        /// <summary>
        /// Reads the import direct account template cell.
        /// </summary>
        /// <param name="cell">The excel cell.</param>
        /// <param name="dataRow">The data row.</param>
        /// <param name="importResult">The import result.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>Number of cell empty.</returns>
        private int ReadImportDirectAccountTemplateCell(
            ICell cell,
            DataRow dataRow,
            ImportDirectAccountResult importResult,
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
                        importResult.ImportResultCode = ErrorCode.InvalidFormatFile;
                        importResult.ImportResultMessage = Customer_Labels.ConfigureDirectAccount_Import_InvalidFormat;
                        LogExtensions.ExceptionHandler(ex);
                    }

                    break;
            }

            return count;
        }
    }
}
