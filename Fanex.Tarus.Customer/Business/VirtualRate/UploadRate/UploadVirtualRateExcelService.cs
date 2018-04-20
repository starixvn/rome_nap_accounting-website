// --------------------------------------------------------------------------
// <copyright file="UploadVirtualRateExcelService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.VirtualRate.UploadRate
{
    using System;
    using System.Linq;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Extensions;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Upload Virtual Rate Excel Service.
    /// </summary>
    public partial class UploadVirtualRateService
    {
        /// <summary>
        /// Downloads the import rate template.
        /// </summary>
        public void DownloadImportRateTemplate()
        {
            try
            {
                ExportExcelInfo exportExcelInfo = new ExportExcelInfo
                {
                    ExcelFile = new HSSFWorkbook()
                };

                exportExcelInfo.CreateSheet(Customer_Labels.VirtualRate_UploadFile_SheetName);
                ExportImportRateTemplateHeader(exportExcelInfo);
                ExportImportRateTemplateContent(exportExcelInfo);
                exportExcelInfo.ResponseFile(Customer_Labels.VirtualRate_UploadFile_TemplateName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Exports the import rate template header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportImportRateTemplateHeader(ExportExcelInfo exportExcelInfo)
        {
            var headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile);
            exportExcelInfo.CreateCurrentRow();

            var betTypeList = VirtualRateSectionService.BetTypeListByFunctionName(VirtualRateSectionService.UploadVirtualFunction);
            var betTypeCount = betTypeList.Count;
            var currentColumn = 0;
            exportExcelInfo.SetColumnWidthByFontSize(currentColumn++, 120);
            exportExcelInfo.SetColumnWidthByFontSize(currentColumn++, 120);
            exportExcelInfo.SetColumnWidthByFontSize(currentColumn++, 120);
            for (var i = 0; i < betTypeCount; i++)
            {
                exportExcelInfo.SetColumnWidthByFontSize(currentColumn++, 110);
            }

            NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, Customer_Labels.Member, headerStyle);
            NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, Customer_Labels.Sport, headerStyle);
            NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, Customer_Labels.Live_NonLive, headerStyle);
            for (var i = 0; i < betTypeCount; i++)
            {
                var betType = betTypeList[i];
                var title = VirtualRateSectionService.ShortBetTypeName(betType.Id);
                var multiBetTypeLength = betType.MultiBetTypeNames != null ? betType.MultiBetTypeNames.Length : 0;
                for (var j = 0; j < multiBetTypeLength; j++)
                {
                    title += "\n" + betType.MultiBetTypeNames[j];
                }

                NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, title, headerStyle);
            }

            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the content of the import rate template.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportImportRateTemplateContent(ExportExcelInfo exportExcelInfo)
        {
            var templateData = ImportRateTemplateData.Template();
            var textStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackGroundColorIndex, false, Alignment.Left);
            var rateStyle = NpoiExcelExtensions.IntPercentStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackGroundColorIndex, false);

            var rowCount = templateData.Count;
            var betTypeList = VirtualRateSectionService.BetTypeListByFunctionName(VirtualRateSectionService.UploadVirtualFunction)
                .Select(bettype => bettype.Id)
                .ToList();

            var maxColumnIndex = betTypeList.Count + 3 - 1;
            for (var i = 0; i < rowCount; i++)
            {
                var group = templateData[i];
                exportExcelInfo.CreateCurrentRow();
                exportExcelInfo.ResetCurrentColumn();
                NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, group.GroupName, textStyle);

                var rateCount = group.ImportVirtualRateResultList.Count;
                for (var j = 0; j < rateCount; j++)
                {
                    var rate = group.ImportVirtualRateResultList[j];
                    if (j == 0)
                    {
                        NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, rate.SportName, textStyle);
                        NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, rate.LiveTypeName, textStyle);
                    }

                    if (rate == null && j <= maxColumnIndex)
                    {
                        NpoiExcelExtensions.RenderCurrentCell(exportExcelInfo, string.Empty, rateStyle);
                        continue;
                    }
                    else if (rate == null || !betTypeList.Contains(rate.BetTypeId))
                    {
                        continue;
                    }

                    decimal rateValue;
                    NpoiExcelExtensions.RenderCurrentCell(
                        exportExcelInfo,
                        decimal.TryParse(rate.VirtualRateString, out rateValue) ? (object)(rateValue / 100) : rate.VirtualRateString,
                        rateStyle);
                }

                exportExcelInfo.CurrentRow++;
            }
        }
    }
}
