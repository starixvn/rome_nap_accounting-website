using Fanex.Tarus.Business.Interface.Admin;
using Fanex.Tarus.DataAccess.Implement.Admin;
using Fanex.Tarus.DataAccess.Interface.Admin;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Admin;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Fanex.Tarus.Business.Implement.Admin
{
    public class TicketToHeadlineService : ITicketToHeadlineService
    {
        private ITicketToHeadlineRepository _ticketToHeadlineRepository;

        public TicketToHeadlineService()
        {
            this._ticketToHeadlineRepository = new TicketToHeadlineRepository();
        }

        public TicketToHeadlineViewModel GetData(HttpPostedFileBase uploadFile, TicketToHeadlineCriteria criteria)
        {
            var result = new TicketToHeadlineViewModel();
            int rowCount = 1;
            string xmlParam = "<ROOT>";
            try
            {
                IWorkbook workbook = WorkbookFactory.Create(uploadFile.InputStream);
                ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                rows.MoveNext();
                while (rows.MoveNext())
                {
                    rowCount++;
                    IRow row = (IRow)rows.Current;
                    string userName = HttpUtility.HtmlEncode(row.GetCell(0) == null ? string.Empty : row.GetCell(0).ToString());
                    string refno = HttpUtility.HtmlEncode(row.GetCell(1) == null ? string.Empty : row.GetCell(1).ToString());
                    xmlParam += string.Format("<File UserName=\"{0}\" RefNo=\"{1}\" />", userName.Trim(), refno.Trim());
                }

                xmlParam += "</ROOT>";
                criteria.File = xmlParam;
                result = this._ticketToHeadlineRepository.GetData(criteria);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log.Error("Invalid file uploaded. Error at line: " + rowCount.ToString(), ex);
                result.ErrorMessage = "Invalid file uploaded.";
                return result;
            }

            return result;
        }

        public IWorkbook ExportExcel(TicketToHeadlineViewModel data)
        {
            TicketToHeadlineExcelService excelService = new TicketToHeadlineExcelService(data);
            return excelService.Export();
        }


        public TicketToHeadlineViewModel GetFakeData(int? pageSize, int? pageIndex, int rowNo = 400)
        {
            return _ticketToHeadlineRepository.GetFakeData(pageSize, pageIndex, rowNo);
        }
    }
}
