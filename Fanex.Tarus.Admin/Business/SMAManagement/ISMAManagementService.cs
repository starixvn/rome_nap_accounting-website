using System.Collections.Generic;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Admin.Criteria.SMAManagement;
using Fanex.Tarus.Admin.Entities;
using Fanex.Tarus.Admin.Entities.SMAManagement;

namespace Fanex.Tarus.Admin.Business.SMAManagement
{
    public interface ISMAManagementService
    {
        IEnumerable<IdNameEntity> GetSites();

        IEnumerable<IdNameEntity> GetCurrencies();

        SMAManagerModel GetSMAGroupHeadlines(GetSMAGroupHeadlineCriteria criteria);

        ExcelFile ExportSMAGroupHeadlinesToExcel(string sheetName, GetSMAGroupHeadlineCriteria criteria);

        Result UpdateHeadlineType(UpdateHeadlineTypeCriteria criteria);

        Result UpdateOwner(UpdateOwnerCriteria criteria);
    }
}
