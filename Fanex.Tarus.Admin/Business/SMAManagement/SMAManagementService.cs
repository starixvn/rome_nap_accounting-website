// -----------------------------------------------------------------------
// <copyright file="SMAManagementService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Business.SMAManagement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using DNAide.Core;
    using Fanex.Tarus.Admin.DataAccess.SMAManagement;
    using Fanex.Tarus.Admin.Entities.SMAManagement;
    using Sunplus.Accounting.Core.Extensions;
    using ExcelUtil.Spreadsheet;
    using Sunplus.Accounting.Core.Helper;
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAManagementService: ISMAManagementService
    {
        private readonly ISMAManagementAccess _smaManagementAccess;
        private readonly string _currenciesSessionKey = "Fanex.Tarus.Admin.Business.SMAManagement.Currencies";
        private readonly string _smaListSessionKey = "Fanex.Tarus.Admin.Business.SMAManagement.SMAListByGroupHeadline";
        public SMAManagementService() : this(null)
        { 
        }

        public SMAManagementService(ISMAManagementAccess smaManagementAccess)
        {
            _smaManagementAccess = smaManagementAccess ?? new SMAManagementAccess();
        }

        public FilterInfo SelFilterInfo()
        {
            var result = new FilterInfo();
            try
            {
                #region select currencies
                result.Currencies = new List<FilterInfoDetail>();
                var dtCurrencies = _smaManagementAccess.SelAllCurrencies();

                foreach (DataRow currency in dtCurrencies.Rows)
                {
                    result.Currencies.Add(new FilterInfoDetail()
                    {
                        Id = CastAide.AsInt32(currency["exchangeid"]),
                        Name = CastAide.AsString(currency["currency"])
                    });
                }
                SessionExtension.AddSession(HttpContext.Current.Session, _currenciesSessionKey, result.Currencies);
                #endregion

                #region select sites
                result.Sites = new List<FilterInfoDetail>();
                var dtSites = _smaManagementAccess.SelAllSites();
                foreach (DataRow site in dtSites.Rows)
                {
                    result.Sites.Add(new FilterInfoDetail()
                    {
                        Id = CastAide.AsInt32(site["siteid"]),
                        Name = CastAide.AsString(site["site"])
                    });
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public SMAGroups SelSMAListByGroupHeadline(string siteId, int currency, bool isOpen,
            bool isSusupended, bool isClosed, bool isDisabled, int roleId, string username, 
            string groupHeadline, int pageIndex, int pageSize)
        {
            var result = new SMAGroups();
            try
            {
                #region clear session
                HttpContext.Current.Session.Remove(_smaListSessionKey);
                #endregion clear session

                #region get new data
                result = SelSMAListByGroupHeadlineFromDatabase(siteId, currency, isOpen, isSusupended, isClosed, isDisabled, roleId, username, groupHeadline);
                #endregion

                if (result != null && result.SMAGroupsInfo != null && result.SMAGroupsInfo.Count > 0)
                {  
                    #region paging
                    var pagingResult = new SMAGroups();
                    pagingResult.SMAGroupsInfo = new List<SMAListByGroupHeadline>();
                    pagingResult.TotalGroups = result.TotalGroups;
                    pagingResult.SMAGroupsInfo = result.SMAGroupsInfo.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    var totalMembersNotAssignedToGroup = (from q in pagingResult.SMAGroupsInfo where q.GroupId == 0 select q).Count();
                    pagingResult.TotalMemberNotAssignedToGroup = totalMembersNotAssignedToGroup;

                    return pagingResult;
                    #endregion paging
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public SMAGroups SelSMAListByGroupHeadlineFromCache(string siteId, int currency, bool isOpen,
            bool isSusupended, bool isClosed, bool isDisabled, int roleId, string username,
            string groupHeadline, int pageIndex, int pageSize)
        {
            var result = new SMAGroups();
            try
            {
                var smaList = SessionExtension.GetSession(HttpContext.Current.Session, _smaListSessionKey);
                if (smaList != null)
                {
                    result = (SMAGroups)smaList;
                }
                else
                {
                    result = SelSMAListByGroupHeadlineFromDatabase(siteId, currency, isOpen, isSusupended, isClosed, isDisabled, roleId, username, groupHeadline);
                }

                if (result != null && result.SMAGroupsInfo != null && result.SMAGroupsInfo.Count > 0)
                {
                    #region paging
                    var pagingResult = new SMAGroups();
                    pagingResult.SMAGroupsInfo = new List<SMAListByGroupHeadline>();
                    pagingResult.TotalGroups = result.TotalGroups;
                    pagingResult.SMAGroupsInfo = result.SMAGroupsInfo.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    var totalMembersNotAssignedToGroup = (from q in pagingResult.SMAGroupsInfo where q.GroupId == 0 select q).Count();
                    pagingResult.TotalMemberNotAssignedToGroup = totalMembersNotAssignedToGroup;

                    return pagingResult;
                    #endregion paging
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        private SMAGroups SelSMAListByGroupHeadlineFromDatabase(string siteId, int currency, bool isOpen,
            bool isSusupended, bool isClosed, bool isDisabled, int roleId, string username,
            string groupHeadline)
        {
            var result = new SMAGroups();
            #region get new data
            var dtSMAList = _smaManagementAccess.SelSMAListByGoupHeadline(siteId, currency, isOpen, isSusupended, isClosed,
                                                                            isDisabled, roleId, username, groupHeadline);
            #endregion get new data

            if (dtSMAList != null && dtSMAList.Rows.Count > 0)
            {
                #region local variables
                var getAllCurrencies = GetCurrenciesFromCache();

                result.SMAGroupsInfo = new List<SMAListByGroupHeadline>();
                var oldGroupId = -1;
                var oldGroupname = string.Empty;

                var smaGroup = new SMAListByGroupHeadline();
                smaGroup.SMAList = new List<SMADetail>();
                var totalGroup = 0;
                var totalMembersNotAssignedToGroup = 0;
                #endregion

                #region assign sma detail
                foreach (DataRow item in dtSMAList.Rows)
                {
                    var currentGroupId = CastAide.AsInt32(item["custheadlineid"]);
                    var currentGroupName = CastAide.AsString(item["groupname"]);
                    #region assign sma list to group
                    if (oldGroupId != -1 && (oldGroupId != currentGroupId || oldGroupId == 0))
                    {
                        smaGroup.GroupId = oldGroupId;
                        smaGroup.GroupName = oldGroupname;
                        result.SMAGroupsInfo.Add(smaGroup);

                        totalGroup++;
                        totalMembersNotAssignedToGroup += (oldGroupId == 0 ? 1 : 0);
                        smaGroup = new SMAListByGroupHeadline();
                        smaGroup.SMAList = new List<SMADetail>();
                    }
                    #endregion
                    #region assign sma detail
                    smaGroup.SMAList.Add(new SMADetail()
                    {
                        Username = CastAide.AsString(item["username"]),
                        Status = GetStatusName(CastAide.AsBoolean(item["isclosed"]), CastAide.AsBoolean(item["issuspend"]), CastAide.AsBoolean(item["isenable"])),
                        Site = CastAide.AsString(item["site"]),
                        Currency = GetCurrencyName(CastAide.AsInt32(item["currency"]), getAllCurrencies),
                        Position = GetRoleName(CastAide.AsInt32(item["roleid"]))
                    });
                    #endregion
                    oldGroupId = currentGroupId;
                    oldGroupname = currentGroupName;
                }
                #endregion

                #region latest group
                if (smaGroup != null && smaGroup.SMAList.Count > 0)
                {
                    totalGroup++;
                    totalMembersNotAssignedToGroup += (oldGroupId == 0 ? 1 : 0);
                    smaGroup.GroupId = oldGroupId;
                    smaGroup.GroupName = oldGroupname;
                    result.SMAGroupsInfo.Add(smaGroup);
                }
                #endregion

                result.TotalGroups = totalGroup;
                result.TotalMemberNotAssignedToGroup = totalMembersNotAssignedToGroup;

                SessionExtension.AddSession(HttpContext.Current.Session, _smaListSessionKey, result);
            }
            return result;
        }

        private static string GetRoleName(int roleId)
        {
            var roleName = "Super";
            switch (roleId)
            {
                case 1:
                    roleName = "Member";
                    break;
                case 2:
                    roleName = "Agent";
                    break;
                case 3:
                    roleName = "Master";
                    break;
                case 4:
                    roleName = "Super";
                    break;
                default:
                    break;
            
            }
            return roleName;
        }

        private static string GetStatusName(bool isClosed, bool isSuspended, bool isEnable)
        {
            var result = string.Empty;
            //The top priority will be Disabled - Closed - Suspended - Open.
            if (isEnable == true && isClosed == false && isSuspended == false)
            {
                result = "Open";
            }
            else if (isEnable == false)
            {
                result = "Disabled";
            }
            else if (isClosed == true && isEnable == true && isSuspended == false)
            {
                result = "Closed";
            }
            else if (isSuspended == true && isEnable == true && isClosed == false)
            {
                result = "Suspended";
            }
            else if (isEnable == true && isClosed == true && isSuspended == true)
            {
                result = "Closed/Suspended";
            }            
            
            return result;
        }

        private static string GetCurrencyName(int currencyId, List<FilterInfoDetail> currencies)
        {
            var name = string.Empty;
            var currencyInfo = (from c in currencies where c.Id == currencyId select c);

            name = (currencyInfo != null && currencyInfo.Count() > 0) ? currencyInfo.Select(c => c).FirstOrDefault().Name : string.Format("{0}",  currencyId);
            return name;
        }

        private List<FilterInfoDetail> GetCurrenciesFromCache()
        {
            var currencies = new List<FilterInfoDetail>();

            if (SessionExtension.GetSession(HttpContext.Current.Session, _currenciesSessionKey) != null)
            {
                currencies = (List<FilterInfoDetail>)SessionExtension.GetSession(HttpContext.Current.Session, _currenciesSessionKey);
            }
            else
            {
                var dtCurrencies = _smaManagementAccess.SelAllCurrencies();

                foreach (DataRow currency in dtCurrencies.Rows)
                {
                    currencies.Add(new FilterInfoDetail()
                    {
                        Id = CastAide.AsInt32(currency["exchangeid"]),
                        Name = CastAide.AsString(currency["currency"])
                    });
                }
                SessionExtension.AddSession(HttpContext.Current.Session, _currenciesSessionKey, currencies);
            }
            return currencies;
        }

        public void ExportSMAListToExcelFile(string siteId, int currency, bool isOpen, bool isSusupended, bool isClosed, bool isDisabled, int roleId, string username, string groupHeadline)
        {
            try
            {
                #region local varialbles
                var startCol = 0;
                var startRow = 2;
                var reportName = "SMAListByGroupHeadline";
                var excelFile = new ExcelFile();
                var getSession = (SMAGroups)SessionExtension.GetSession(HttpContext.Current.Session, _smaListSessionKey);
                getSession = getSession ?? SelSMAListByGroupHeadlineFromDatabase(siteId, currency, isOpen, isSusupended, isClosed, isDisabled, roleId, username, groupHeadline);
                var wsSMA = excelFile.Worksheets.Add(string.Format("SMAListByGroupHeadline"));
                #endregion

                #region header
                ExportSMAListToExcelFileAssignHeader(ref wsSMA, startRow, startCol);
                #endregion

                #region content
                if (getSession != null && getSession.SMAGroupsInfo.Count > 0)
                {         
                    var nextRow = 0;
                    if (getSession.TotalMemberNotAssignedToGroup > 0)
                    {
                        var cellStyle = new ExcelGemboxUtils().GetStyleTextCenter(false);
                        wsSMA.Cells.GetSubrangeAbsolute(startRow, startCol + 1, startRow + getSession.TotalMemberNotAssignedToGroup - 1, startCol + 1).Merged = true;
                        wsSMA.Cells.GetSubrangeAbsolute(startRow, startCol + 1, startRow + getSession.TotalMemberNotAssignedToGroup - 1, startCol + 1).Style = cellStyle;
                        wsSMA.Cells.GetSubrangeAbsolute(startRow, startCol + 1, startRow + getSession.TotalMemberNotAssignedToGroup - 1, startCol + 1).Value = "Not assigned to group";

                    }
                    for (int i = 0; i < getSession.TotalGroups; i++)
                    {
                        var group = getSession.SMAGroupsInfo[i];
                        ExportSMAListToExcelFileAssignBody(ref wsSMA, startRow, ref nextRow, startCol, group, i + 1);
                    }

                    
                }
                #endregion

                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void ExportSMAListToExcelFileAssignHeader(ref ExcelWorksheet wsSMA, int startRow, int startCol)
        {
            var bgHeaderFormat = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsSMA.Cells[startRow - 1, startCol].Style = bgHeaderFormat;
            wsSMA.Cells[startRow - 1, startCol].Value = "#";
            wsSMA.Columns[startCol].Width = 255 * 10;

            wsSMA.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeaderFormat;
            wsSMA.Cells[startRow - 1, startCol + nextCol].Value = "Group Headline";
            wsSMA.Columns[startCol + nextCol].Width = 255 * 20;

            wsSMA.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsSMA.Cells[startRow - 1, startCol + nextCol].Value = "Username";
            wsSMA.Columns[startCol + nextCol].Width = 255 * 15;

            wsSMA.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsSMA.Cells[startRow - 1, startCol + nextCol].Value = "Status";
            wsSMA.Columns[startCol + nextCol].Width = 255 * 15;

            wsSMA.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsSMA.Cells[startRow - 1, startCol + nextCol].Value = "Site";
            wsSMA.Columns[startCol + nextCol].Width = 255 * 15;

            wsSMA.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsSMA.Cells[startRow - 1, startCol + nextCol].Value = "Currency";
            wsSMA.Columns[startCol + nextCol].Width = 255 * 10;

            wsSMA.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsSMA.Cells[startRow - 1, startCol + nextCol].Value = "Position";
            wsSMA.Columns[startCol + nextCol].Width = 255 * 10;
        }

        private void ExportSMAListToExcelFileAssignBody(ref ExcelWorksheet wsSMA, int startRow, ref int nextRow, int startCol, SMAListByGroupHeadline smaListByGroup, int groupIndex)
        {            
            var cellStyle = new ExcelGemboxUtils().GetStyleTextCenter(false);
            
            if (smaListByGroup != null && smaListByGroup.SMAList.Count > 0)
            {
                var totalItems = smaListByGroup.SMAList.Count;
                for (int i = 0; i < totalItems; i++)
                {
                    var smaDetail = smaListByGroup.SMAList[i];
                    var nexCol = 0;
                    var status = smaDetail.Status.ToLower();
                    var bgColor = GetBackgroudColorByStatus(status);
                    if (i == 0)
                    {
                        wsSMA.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems - 1), startCol).Merged = true;
                        wsSMA.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems - 1), startCol).Style = cellStyle;
                        wsSMA.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems - 1), startCol).Value = groupIndex;
                        if (smaListByGroup.GroupId != 0)
                        {
                            wsSMA.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nexCol + 1), startRow + nextRow + (totalItems - 1), startCol + (nexCol + 1)).Merged = true;
                            wsSMA.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nexCol + 1), startRow + nextRow + (totalItems - 1), startCol + (nexCol + 1)).Style = cellStyle;
                            wsSMA.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nexCol + 1), startRow + nextRow + (totalItems - 1), startCol + (nexCol + 1)).Value = smaListByGroup.GroupName;
                        }                        

                    }                    
                    nexCol += 1;

                    wsSMA.Cells[startRow + nextRow, startCol + (nexCol+=1)].Style = cellStyle;
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Style.FillPattern.SetSolid(bgColor);
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Value = smaDetail.Username;

                    wsSMA.Cells[startRow + nextRow, startCol + (nexCol += 1)].Style = cellStyle;
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Style.FillPattern.SetSolid(bgColor);
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Value = smaDetail.Status;

                    wsSMA.Cells[startRow + nextRow, startCol + (nexCol += 1)].Style = cellStyle;
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Style.FillPattern.SetSolid(bgColor);
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Value = smaDetail.Site;

                    wsSMA.Cells[startRow + nextRow, startCol + (nexCol += 1)].Style = cellStyle;
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Style.FillPattern.SetSolid(bgColor);
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Value = smaDetail.Currency;

                    wsSMA.Cells[startRow + nextRow, startCol + (nexCol += 1)].Style = cellStyle;
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Style.FillPattern.SetSolid(bgColor);
                    wsSMA.Cells[startRow + nextRow, startCol + nexCol].Value = smaDetail.Position;

                    nextRow++;
                }
            }

        }

        private Color GetBackgroudColorByStatus(string status)
        {
            //The top priority will be Disabled - Closed - Suspended - Open.
            if (status.IndexOf("disabled") != -1)
            {
                return Color.FromArgb(160, 169, 122);
            }
            else if (status.IndexOf("closed") != -1)
            {
                return Color.FromArgb(204, 199, 186);
            }
            else if (status.IndexOf("suspended") != -1)
            {
                return Color.FromArgb(248, 222, 215);
            }
            else
            {
                return Color.FromArgb(255, 255, 255);
            }
        }
    }
}
