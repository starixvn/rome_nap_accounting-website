using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Web;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Admin.Criteria.SMAManagement;
using Fanex.Tarus.Admin.DataAccess.SMAManagement;
using Fanex.Tarus.Admin.Entities;
using Fanex.Tarus.Admin.Entities.SMAManagement;
using Fanex.Tarus.Admin.Enums;
using Fanex.Tarus.Admin.Resource;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;

namespace Fanex.Tarus.Admin.Business.SMAManagement
{
    public class SMAManagementService : ISMAManagementService
    {
        private const string SitesSessionKey = "Fanex.Tarus.Admin.Business.SMAManagement.Sites";
        private const string CurrenciesSessionKey = "Fanex.Tarus.Admin.Business.SMAManagement.Currencies";
        private const string SMAGroupHeadlinesSessionKey = "Fanex.Tarus.Admin.Business.SMAManagement.SMAListByGroupHeadline";

        private readonly ISMAManagementAccess _smaManagementAccess;

        public SMAManagementService(ISMAManagementAccess smaManagementAccess = null)
        {
            _smaManagementAccess = smaManagementAccess ?? new SMAManagementAccess();
        }

        public IEnumerable<IdNameEntity> GetSites()
        {
            return GetFromSession(
                SitesSessionKey,
                () =>
                {
                    return _smaManagementAccess.GetSites();
                });
        }

        public IEnumerable<IdNameEntity> GetCurrencies()
        {
            return GetFromSession(
                CurrenciesSessionKey,
                () =>
                {
                    // Eliminate test currencies: 20 - uus; 27 - h; 28 - c.
                    return _smaManagementAccess.GetCurrencies()
                        .Where(c => c.Id != 20 && c.Id != 27 && c.Id != 28)
                        .ToList();
                });
        }

        public SMAManagerModel GetSMAGroupHeadlines(GetSMAGroupHeadlineCriteria criteria)
        {
            SMAManagerModel result = null;
            bool fromSession = criteria.PageIndex != 1;

            try
            {
                if (fromSession)
                {
                    result = (SMAManagerModel)SessionExtension.GetSession(HttpContext.Current.Session, SMAGroupHeadlinesSessionKey);
                }

                if (result == null)
                {
                    HttpContext.Current.Session.Remove(SMAGroupHeadlinesSessionKey);

                    result = GetSMAGroupHeadlinesFromDatabase(criteria);

                    SessionExtension.AddSession(HttpContext.Current.Session, SMAGroupHeadlinesSessionKey, result);
                }

                if (result != null && result.TotalGroups > 0)
                {
                    var pagingResult = new SMAManagerModel
                    {
                        TotalGroups = result.TotalGroups,
                        SMAGroupHeadlines = result.SMAGroupHeadlines
                            .Skip(criteria.PageSize * (criteria.PageIndex - 1))
                            .Take(criteria.PageSize)
                            .ToList()
                    };

                    pagingResult.TotalMemberNotAssignedToGroup = pagingResult.SMAGroupHeadlines.Count(q => q.GroupId == 0);

                    return pagingResult;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public ExcelFile ExportSMAGroupHeadlinesToExcel(string sheetName, GetSMAGroupHeadlineCriteria criteria)
        {
            int startRow = 1;
            ExcelFile excelFile = new ExcelFile();

            try
            {
                var getSession = GetFromSession(
                    SMAGroupHeadlinesSessionKey,
                    () =>
                    {
                        return GetSMAGroupHeadlinesFromDatabase(criteria);
                    });

                var worksheet = excelFile.Worksheets.Add(sheetName);
                ExportSMAListToExcelFileAssignHeader(ref worksheet, startRow++);

                if (getSession != null && getSession.TotalGroups > 0)
                {
                    int nextRow = 0,
                        notAssignedToGroupCount = getSession.TotalMemberNotAssignedToGroup;

                    if (notAssignedToGroupCount > 0)
                    {
                        var cellStyle = new ExcelGemboxUtils().GetStyleTextCenter(false);
                        var notAssignedGroupCell = worksheet.Cells.GetSubrangeAbsolute(startRow, 1, startRow + notAssignedToGroupCount - 1, 1);

                        notAssignedGroupCell.Merged = true;
                        notAssignedGroupCell.Style = cellStyle;
                        notAssignedGroupCell.Value = "Not assigned to group";

                        notAssignedGroupCell = worksheet.Cells.GetSubrangeAbsolute(startRow + nextRow, 2, startRow + nextRow + (notAssignedToGroupCount - 1), 2);

                        notAssignedGroupCell.Merged = true;
                        notAssignedGroupCell.Style = cellStyle;
                        notAssignedGroupCell.Value = string.Empty;
                    }

                    for (int i = 0; i < getSession.TotalGroups; i++)
                    {
                        ExportSMAListToExcelFileAssignBody(ref worksheet, startRow, ref nextRow, getSession.SMAGroupHeadlines[i], i + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return excelFile;
        }

        public Result UpdateHeadlineType(UpdateHeadlineTypeCriteria criteria)
        {
            return _smaManagementAccess.UpdateHeadlineType(criteria);
        }

        public Result UpdateOwner(UpdateOwnerCriteria criteria)
        {
            return _smaManagementAccess.UpdateOwner(criteria);
        }

        private SMAManagerModel GetSMAGroupHeadlinesFromDatabase(GetSMAGroupHeadlineCriteria criteria)
        {
            int totalGroups = 0,
                totalMemberNotAssignedToGroup = 0;

            IList<SMAGroupHeadline> smaGroupHeadlines = new List<SMAGroupHeadline>(512);

            var groupHeadlines = _smaManagementAccess.GetSMAGroupHeadlines(criteria);

            foreach (var group in groupHeadlines)
            {
                if (group.GroupId != 0)
                {
                    totalGroups++;
                    smaGroupHeadlines.Add(group);
                }

                foreach (var entity in group.SMAEntities)
                {
                    entity.HeadlineType = MapHeadlineType(entity.HeadlineTypeId);
                    entity.Status = MapStatus(entity.IsClosed, entity.IsSuspend, entity.IsEnable);
                    entity.Position = MapRole(entity.RoleId);

                    if (group.GroupId == 0)
                    {
                        totalMemberNotAssignedToGroup++;

                        smaGroupHeadlines.Add(new SMAGroupHeadline
                        {
                            GroupId = group.GroupId,
                            GroupName = group.GroupName,
                            Owner = entity.Owner,
                            SMAEntities = new List<SMAEntity> { entity }
                        });
                    }
                }
            }

            return new SMAManagerModel
            {
                TotalGroups = totalGroups + totalMemberNotAssignedToGroup,
                TotalMemberNotAssignedToGroup = totalMemberNotAssignedToGroup,
                SMAGroupHeadlines = smaGroupHeadlines
            };
        }

        private void ExportSMAListToExcelFileAssignHeader(ref ExcelWorksheet worksheet, int startRow)
        {
            int startCol = 0;
            ExcelCell excelCell;
            var headerFormat = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            excelCell = worksheet.Cells[startRow, startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "#";
            worksheet.Columns[startCol].Width = 255 * 10;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Group Headline";
            worksheet.Columns[startCol].Width = 255 * 20;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Headline Type";
            worksheet.Columns[startCol].Width = 255 * 15;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Owner";
            worksheet.Columns[startCol].Width = 255 * 15;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Username";
            worksheet.Columns[startCol].Width = 255 * 15;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Status";
            worksheet.Columns[startCol].Width = 255 * 15;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Site";
            worksheet.Columns[startCol].Width = 255 * 15;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Currency";
            worksheet.Columns[startCol].Width = 255 * 10;

            excelCell = worksheet.Cells[startRow, ++startCol];
            excelCell.Style = headerFormat;
            excelCell.Value = "Position";
            worksheet.Columns[startCol].Width = 255 * 10;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private void ExportSMAListToExcelFileAssignBody(
            ref ExcelWorksheet worksheet,
            int startRow,
            ref int nextRow,
            SMAGroupHeadline groupHeadline,
            int groupIndex)
        {
            CellStyle cellStyle = new ExcelGemboxUtils().GetStyleTextCenter(false);

            int startCol;
            SMAEntity smaEntity;
            Color backgroundColor;

            CellRange cellRange;
            ExcelCell excelCell;

            int totalItems = groupHeadline != null ? groupHeadline.SMAEntities.Count : 0;

            for (int i = 0; i < totalItems; i++)
            {
                startCol = 0;
                smaEntity = groupHeadline.SMAEntities[i];
                backgroundColor = GetBackgroudColorByStatus(smaEntity.IsClosed, smaEntity.IsSuspend, smaEntity.IsEnable);

                if (i == 0)
                {
                    cellRange = worksheet.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems - 1), startCol);

                    cellRange.Merged = true;
                    cellRange.Style = cellStyle;
                    cellRange.Value = groupIndex;

                    if (groupHeadline.GroupId != 0)
                    {
                        cellRange = worksheet.Cells.GetSubrangeAbsolute(
                            startRow + nextRow,
                            startCol + 1,
                            startRow + nextRow + (totalItems - 1),
                            startCol + 1);

                        cellRange.Merged = true;
                        cellRange.Style = cellStyle;
                        cellRange.Value = groupHeadline.GroupName;

                        cellRange = worksheet.Cells.GetSubrangeAbsolute(
                            startRow + nextRow,
                            startCol + 2,
                            startRow + nextRow + (totalItems - 1),
                            startCol + 2);

                        cellRange.Merged = true;
                        cellRange.Style = cellStyle;
                        cellRange.Value = smaEntity.HeadlineType;
                    }

                    cellRange = worksheet.Cells.GetSubrangeAbsolute(
                            startRow + nextRow,
                            startCol + 3,
                            startRow + nextRow + (totalItems - 1),
                            startCol + 3);

                    cellRange.Merged = true;
                    cellRange.Style = cellStyle;
                    cellRange.Value = smaEntity.Owner;
                }

                startCol += 3;

                excelCell = worksheet.Cells[startRow + nextRow, ++startCol];
                excelCell.Style = cellStyle;
                excelCell.Style.FillPattern.SetSolid(backgroundColor);
                excelCell.Value = smaEntity.Username;

                excelCell = worksheet.Cells[startRow + nextRow, ++startCol];
                excelCell.Style = cellStyle;
                excelCell.Style.FillPattern.SetSolid(backgroundColor);
                excelCell.Value = smaEntity.Status;

                excelCell = worksheet.Cells[startRow + nextRow, ++startCol];
                excelCell.Style = cellStyle;
                excelCell.Style.FillPattern.SetSolid(backgroundColor);
                excelCell.Value = smaEntity.Site;

                excelCell = worksheet.Cells[startRow + nextRow, ++startCol];
                excelCell.Style = cellStyle;
                excelCell.Style.FillPattern.SetSolid(backgroundColor);
                excelCell.Value = smaEntity.Currency;

                excelCell = worksheet.Cells[startRow + nextRow, ++startCol];
                excelCell.Style = cellStyle;
                excelCell.Style.FillPattern.SetSolid(backgroundColor);
                excelCell.Value = smaEntity.Position;

                nextRow++;
            }
        }

        private string MapHeadlineType(int? headlineTypeId)
        {
            if (!headlineTypeId.HasValue)
            {
                return string.Empty;
            }

            return ((HeadlineType)headlineTypeId.Value).ToString();
        }

        private string MapStatus(bool isClosed, bool isSuspended, bool isEnable)
        {
            // The priority will be Disabled - Closed - Suspended - Open.
            var status = Language.Open;

            if (!isEnable)
            {
                status = Language.Disabled;
            }
            else if (isClosed)
            {
                status = Language.Closed;
            }
            else if (isSuspended)
            {
                status = Language.Suspended;
            }

            return status;
        }

        private string MapRole(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return Language.Member;

                case 2:
                    return Language.Agent;

                case 3:
                    return Language.Master;

                case 4:
                    return Language.Super;

                default:
                    return string.Empty;
            }
        }

        private Color GetBackgroudColorByStatus(bool isClosed, bool isSuspended, bool isEnable)
        {
            // The top priority will be Disabled - Closed - Suspended - Open.
            if (!isEnable)
            {
                return Color.FromArgb(160, 169, 122);
            }

            if (isClosed)
            {
                return Color.FromArgb(204, 199, 186);
            }

            if (isSuspended)
            {
                return Color.FromArgb(248, 222, 215);
            }

            return Color.FromArgb(255, 255, 255);
        }

        private T GetFromSession<T>(string key, Func<T> acquire)
        {
            object value = SessionExtension.GetSession(HttpContext.Current.Session, key);

            if (value != null)
            {
                return (T)value;
            }

            var result = acquire();

            if (result != null)
            {
                SessionExtension.AddSession(HttpContext.Current.Session, key, result);
            }

            return result;
        }
    }
}
