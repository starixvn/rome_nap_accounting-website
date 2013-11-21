namespace Fanex.Tarus.Finance.Business.Statement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Web;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Finance.DataAccess.Statement;
    using Fanex.Tarus.Finance.Entities;
    using Fanex.Tarus.Finance.Model.Statement;
    using Fanex.Tarus.Infrastructure.Utils;
    using Sunplus.Accounting.Core.Extensions;

    public class FinanceStatementBusiness : IFinanceStatementBusiness
    {
        private IFinanceStatementAccess _FinanceStatementAccess = null;
        private const string FinanceStatementAccess = "FinanceStatement";
        private const string FianceStatementSession = "fanex.tarus.business.statement.financestatement";

        public FinanceStatementBusiness()
            : this(null)
        {
        }

        private Helper _helper;

        public FinanceStatementBusiness(IFinanceStatementAccess FinanceStatementAccess)
        {
            _FinanceStatementAccess = FinanceStatementAccess ?? new FinanceStatementAccess();
            _helper = new Helper();
        }

        public List<StatementDetail> GetFinanceStatementBusiness(string refNoParam, string custName, string filterBy, string fromdate, string toDay, string balanceUpDay, int IncludeAccBall)
        {
            try
            {
                var dtStatement = _FinanceStatementAccess.GetFinanceStatementAccess(refNoParam, custName, filterBy, fromdate, toDay, balanceUpDay, IncludeAccBall);

                var statement = new List<StatementDetail>();
                if (dtStatement.Tables.Count > 0)
                {
                    var tableStatement = dtStatement.Tables[0];

                    var accumAmount = 0.0;
                    foreach (DataRow row in tableStatement.Rows)
                    {
                        var statementDetail = new StatementDetail();
                        var refNo = row["RefNo"].ToString();
                        var checkerName = row["CheckerName"].ToString();

                        DateTime winlossDatetmp = CastAide.AsDateTime(row["WinlostDate"]);
                        var winlossDate = winlossDatetmp.DayOfWeek + " <br> " + String.Format("{0:MM/dd/yyyy}", winlossDatetmp);
                        statementDetail.RefNo = refNo;
                        statementDetail.WinLossDate = winlossDate;

                        statementDetail.CheckerName = checkerName == "0" ? string.Empty : checkerName;
                        statementDetail.CheckerTime = String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", row["CheckTime"]);
                        statementDetail.IsVerify = CastAide.AsBoolean(row["IsAllowVerify"]);
                        statementDetail.IsReverse = CastAide.AsBoolean(row["IsReversal"]);
                        statementDetail.IsExpand = CastAide.AsBoolean(row["IsExpand"]);
                        statementDetail.IsShowReversal = (winlossDatetmp >= CastAide.AsDateTime(balanceUpDay) && (statementDetail.IsReverse == false)) ? true : false;

                        var trans = new Transaction();

                        var amount = CastAide.AsDouble(row["TotalAmount"]);
                        trans.Total = IBCUtils.FormatHandler.FormatNumber20(amount);

                        var creator = row["Creator"].ToString();
                        trans.Creator = creator == "" ? "--" : creator;
                        trans.TransId = row["TransID"].ToString();
                        trans.TransType = row["transtype"].ToString();
                        trans.Remark = trans.TransType.ToUpper() + " <br> -" + row["TransDescription"].ToString();

                        DateTime transDatetmp = CastAide.AsDateTime(row["TransDate"]);
                        var transDate = transDatetmp.DayOfWeek + " <br> " + String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", transDatetmp);
                        trans.TransDate = transDate;

                        statementDetail.ListTrans.Add(trans);

                        if (statementDetail.IsOpeningBalance)
                        {
                            if (accumAmount == 0.0)
                            {
                                trans.AccumTotal = "<div style=\"color: #4CC552;\" class=\"b\">OK</div>";
                                trans.AccumTotalExcel = "OK";
                            }
                            else
                            {
                                var diffAmount = NumberFormat.DoubleFormat(amount - accumAmount, 6);
                                if (diffAmount == "0.000000")
                                {
                                    trans.AccumTotal = "<div style=\"color: #4CC552;\" class=\"b\">OK</div>";
                                    trans.AccumTotalExcel = "OK";
                                }
                                else
                                {
                                    trans.AccumTotal = "NOT EQUAL <br>" + "<div class=\"b\" style=\"overflow: hidden;width:110px;\">RESET Total<br>Diff: " + diffAmount + "</div>";
                                    trans.AccumTotalExcel = "NOT EQUAL - RESET Total - Diff: " + diffAmount;
                                }
                            }
                            accumAmount = amount;
                        }
                        else
                        {
                            trans.AccumTotal = "";
                            if ((statementDetail.IsAccummulated || !statementDetail.IsReverse) && (refNoParam == "-1"))
                            {
                                accumAmount = amount + accumAmount;
                                trans.AccumTotal = IBCUtils.FormatHandler.FormatNumber20(accumAmount);
                            }
                        }

                        InsertToStatement(ref statement, statementDetail, trans);
                    }
                }
                SessionExtension.AddSession(HttpContext.Current.Session, FianceStatementSession, statement);
                return statement;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return new List<StatementDetail>();
        }

        public StatementDetail GetFinanceStatementDetail(StatementDetail statementDetail, string username)
        {
            try
            {
                var winlossDatetmp = statementDetail.WinLossDate.Split(' ');
                var dtTrans = _FinanceStatementAccess.GetFinanceStatementDetailAccess(statementDetail.RefNo, winlossDatetmp[2], username);

                var isForex = _helper.GetForexAccounts().Contains(username);

                var listTrans = new List<Transaction>();
                if (dtTrans.Tables.Count > 0)
                {
                    var transTable = dtTrans.Tables[0];
                    foreach (DataRow row in transTable.Rows)
                    {
                        var subTrans = new SubTransaction();
                        var trans = new Transaction();

                        var transDate = row["TransDate"].ToString();
                        var transId = row["TransID"].ToString();

                        trans.TransDate = transDate;
                        trans.Remark = row["TransDescription"].ToString();
                        trans.TransId = transId;

                        var isOwner = CastAide.AsBoolean(row["IsOwner"]);
                        var totalAmount = row["TotalAmount"].ToString();

                        trans.IsOwner = isOwner;
                        trans.Total = totalAmount;

                        subTrans.TransDate = transDate;
                        subTrans.Amount = totalAmount;
                        subTrans.Remark = row["transtype"].ToString().ToUpper() + " <br> -" + row["TransDescription"].ToString();
                        subTrans.IsOwner = isOwner;
                        subTrans.TransId = transId;

                        InsertToTrans(ref listTrans, trans, subTrans, isForex);
                    }
                }
                var transCount = statementDetail.ListTrans.Count;

                for (var i = 0; i < transCount; i++)
                {
                    var trans = listTrans.Where(t => t.TransId == statementDetail.ListTrans[i].TransId).FirstOrDefault();
                    if (trans != null)
                    {
                        statementDetail.ListTrans[i].ListSubTrans = trans.ListSubTrans;
                    }
                }

                return statementDetail;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return new StatementDetail();
        }

        private void InsertToStatement(ref List<StatementDetail> statement, StatementDetail statementDetail, Transaction trans)
        {
            if (statement != null && statementDetail != null && trans != null)
            {
                //If this statement detail is not exist in statement, add statement detail to statement

                var isExistedStatementDetail = (statement.Find(item => item.RefNo.Equals(statementDetail.RefNo) && item.WinLossDate.Equals(statementDetail.WinLossDate)) != null);

                if (!isExistedStatementDetail)
                {
                    statement.Add(statementDetail);
                }
                else
                {
                    statement = (statement.Select(item =>
                    {
                        if (item.RefNo.Equals(statementDetail.RefNo) && item.WinLossDate.Equals(statementDetail.WinLossDate))
                        {
                            item.ListTrans.Add(trans);
                            return item;
                        }
                        else return item;
                    })).ToList();
                }
            }
        }

        private void InsertToTrans(ref List<Transaction> listTrans, Transaction trans, SubTransaction subTrans, bool isForex)
        {
            if (listTrans != null && subTrans != null)
            {
                var subTransAmount = Convert.ToDouble(subTrans.Amount);

                subTrans.Amount = DNAide.Utils.NumberFormat.DoubleFormat(subTrans.Amount, 2);

                if (isForex)
                {
                    var isExistedTrans = (listTrans.Find(item => item.TransDate.Equals(subTrans.TransDate) && (item.ListSubTrans.Count < 2) && IsEqualToRemarkAmount(item.Remark, Convert.ToDouble(item.Total), subTrans.Remark, subTransAmount) && subTrans.IsOwner != item.IsOwner) != null);
                    if (!isExistedTrans)
                    {
                        trans.ListSubTrans = new List<SubTransaction>();
                        trans.ListSubTrans.Add(subTrans);
                        listTrans.Add(trans);
                    }
                    else
                    {
                        listTrans = (listTrans.Select(item =>
                        {
                            if (item.TransDate.Equals(trans.TransDate) && (item.ListSubTrans.Count < 2) && IsEqualToRemarkAmount(item.Remark, Convert.ToDouble(item.Total), subTrans.Remark, subTransAmount) && subTrans.IsOwner != item.IsOwner)
                            {
                                item.ListSubTrans.Add(subTrans);
                                if (subTrans.IsOwner)
                                {
                                    item.TransId = subTrans.TransId;
                                }
                                return item;
                            }
                            else return item;
                        })).ToList();
                    }
                }
                else
                {
                    var isExistedTrans = (listTrans.Find(item => item.TransDate.Equals(subTrans.TransDate)) != null);
                    if (!isExistedTrans)
                    {
                        trans.ListSubTrans = new List<SubTransaction>();
                        trans.ListSubTrans.Add(subTrans);
                        listTrans.Add(trans);
                    }
                    else
                    {
                        listTrans = (listTrans.Select(item =>
                        {
                            if (item.TransDate.Equals(subTrans.TransDate))
                            {
                                item.ListSubTrans.Add(subTrans);
                                if (subTrans.IsOwner)
                                {
                                    item.TransId = subTrans.TransId;
                                }
                                return item;
                            }
                            else return item;
                        })).ToList();
                    }
                }
            }
        }

        private static bool IsEqualToRemarkAmount(string remark, double transAmount, string subTransRemark, double subTransAmount)
        {
            if (string.IsNullOrEmpty(remark)) return false;

            var arrSplitExRate = remark.Split(new string[] { "Ex.Rate" }, StringSplitOptions.None);

            var arrRemark = arrSplitExRate[0].Trim().Split(' ');

            if (arrSplitExRate.Count() > 1)
            {
                var arrRate = arrSplitExRate[1].Split(':');
                if (arrRate.Count() > 1)
                {
                    var exRate = Convert.ToDouble(arrRate[1].Trim());

                    if ((subTransAmount).Equals(RoundNumber(-exRate * transAmount, 4))
                        || RoundNumber(subTransAmount, 2).Equals(RoundNumber(-exRate * transAmount, 2))) return true;
                    else
                    {
                        var arrSplitSubTransRemark = subTransRemark.Split(
                            new string[] { "Ex.Rate" }, StringSplitOptions.None);
                        if (arrSplitSubTransRemark.Count() > 1)
                        {
                            var arrSubTransExRate = arrSplitSubTransRemark[1].Split(':');

                            var subExRate = Convert.ToDouble(arrSubTransExRate[1].Trim());

                            if (transAmount.Equals(RoundNumber(-subTransAmount * subExRate, 4))
                                || RoundNumber(transAmount, 2).Equals(RoundNumber(-subTransAmount * subExRate, 2))) return true;
                        }
                    }
                }
            }
            else
            {
                foreach (string s in arrRemark)
                {
                    if (NumberUtil.IsNumber(s))
                    {
                        var remarkAmount = Math.Abs(Convert.ToDouble(s));
                        if (remarkAmount.Equals(subTransAmount) || remarkAmount.Equals(Math.Round(subTransAmount))
                            || remarkAmount.Equals(Math.Round(subTransAmount, 2)) || remarkAmount.Equals(Math.Round(subTransAmount, 1))) return true;
                    }
                }
            }

            return false;
        }

        private static double RoundNumber(double number, uint digits = 0u)
        {
            if (Math.Abs(number) < 1)
                return Math.Round(number, 2);
            else if (!digits.Equals(0))
                return Math.Round(number, (int)digits);
            else
                return Math.Round(number);
        }

        public Result VerifyTransactionBusiness(string refNo, bool isVerify, string checkerName, int checkerId)
        {
            try
            {
                return _FinanceStatementAccess.UpdateVerifyStatusAccess(refNo, isVerify, checkerName, checkerId);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return new Result { ErrorCode = 1, ErrorMessage = "Update Error" };
            }
        }

        public Result DeleteTransBusiness(string refNo, int custId, int userId)
        {
            try
            {
                return _FinanceStatementAccess.DeleteTransAccess(refNo, custId, userId);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return new Result { ErrorCode = 1, ErrorMessage = "Update Error" };
            }
        }

        #region export 2 excel

        public void ExportExcelFinanceStatement(string refNoParam, string custName, string filterBy, string fromdate, string toDay, string balanceUpDay, int IncludeAccBall, string reportTitle)
        {
            var startCol = 0;
            var startRow = 2;
            var reportName = "FinanceStatement";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);

            #region assign header

            ExportExcelFinanceStatementAssignHeader(startRow, startCol, ref wsReport, reportTitle);

            #endregion assign header

            #region content

            List<StatementDetail> finaceStatement = (List<StatementDetail>)SessionExtension.GetSession(HttpContext.Current.Session, FianceStatementSession);
            finaceStatement = finaceStatement != null ? finaceStatement : GetFinanceStatementBusiness(refNoParam, custName, filterBy, fromdate, toDay, balanceUpDay, IncludeAccBall);
            if (finaceStatement != null && finaceStatement.Count > 0)
            {
                var nextRow = 0;
                foreach (StatementDetail statementDetail in finaceStatement)
                {
                    ExportExcelFinanceStatementAssignBody(startRow, startCol, ref nextRow, ref wsReport, statementDetail);
                }
            }
            else
            {
                wsReport.Cells[startRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage"); ;
            }

            #endregion content

            #region response file

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion response file
        }

        private void ExportExcelFinanceStatementAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string title)
        {
            var bgHeaderFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol].Value = "WinLoss Date";
            wsReport.Columns[0].Width = 255 * 17;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Remark";
            wsReport.Columns[nextCol].Width = 255 * 50;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Total";
            wsReport.Columns[nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Accumulated Total";
            wsReport.Columns[nextCol].Width = 255 * 20;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Transaction Date";
            wsReport.Columns[nextCol].Width = 255 * 17;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Ref.no";
            wsReport.Columns[nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Creator";
            wsReport.Columns[nextCol].Width = 255 * 10;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Verify by";
            wsReport.Columns[nextCol].Width = 255 * 10;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = "Checked time";
            wsReport.Columns[nextCol].Width = 255 * 17;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title;
        }

        private void ExportExcelFinanceStatementAssignBody(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport, StatementDetail statementDetail)
        {
            if (statementDetail != null && statementDetail.ListTrans != null && statementDetail.ListTrans.Count > 0)
            {
                var bgColor = Color.FromArgb(255, 255, 255);
                if (statementDetail.IsAccummulated)
                    bgColor = Color.FromArgb(253, 248, 181);
                else if (statementDetail.IsReverse)
                {
                    bgColor = Color.FromArgb(234, 196, 196);
                    if (statementDetail.IsOpeningBalance)
                    {
                        bgColor = Color.FromArgb(255, 255, 255);
                    }
                }
                if (statementDetail.IsOpeningBalance || statementDetail.IsAccummulated)
                {
                    statementDetail.RefNo = "";
                }
                var bgFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor,
                     false, false, true);
                bgFormat.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //bgFormat.Font.Color = statementDetail.IsAccummulated ? Color.FromArgb(6, 6, 255) : Color.FromArgb(0, 0, 0);

                var detailIndex = 0;
                var totalTrans = statementDetail.ListTrans.Count;
                foreach (Transaction transaction in statementDetail.ListTrans)
                {
                    var nextCol = 0;

                    #region winloss date

                    if (detailIndex == 0)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, (startRow + nextRow + totalTrans - 1), startCol).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, (startRow + nextRow + totalTrans - 1), startCol).Style = bgFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, (startRow + nextRow + totalTrans - 1), startCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, (startRow + nextRow + totalTrans - 1), startCol).Value = statementDetail.WinLossDate.Replace("<br>", "\n");
                    }

                    #endregion winloss date

                    #region remark

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = transaction.Remark.Replace("<br>", "\n"); ;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Rows[startRow + nextRow].Height = 255 * 3;

                    #endregion remark

                    #region total

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(transaction.Total);

                    #endregion total

                    #region accumulated total

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                    if (statementDetail.IsOpeningBalance)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = transaction.AccumTotalExcel.Replace("<br>", "\n");
                    }
                    else
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(transaction.AccumTotal);

                    #endregion accumulated total

                    #region transaction date

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = transaction.TransDate.Replace("<br>", "\n");
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                    #endregion transaction date

                    if (detailIndex == 0)
                    {
                        #region refno

                        nextCol += 1;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Style = bgFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Value = statementDetail.RefNo;

                        #endregion refno
                    }

                    #region creator

                    if (statementDetail.HasMultipleCreator)
                    {
                        nextCol += detailIndex == 0 ? 1 : 2;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = transaction.Creator;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    }
                    else
                    {
                        if (detailIndex == 0)
                        {
                            nextCol += 1;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Style = bgFormat;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Value = transaction.Creator;
                        }
                    }

                    #endregion creator

                    if (detailIndex == 0)
                    {
                        #region verify by

                        nextCol += 1;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Style = bgFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Value = statementDetail.CheckerName;

                        #endregion verify by

                        #region checked time

                        nextCol += 1;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Style = bgFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), (startRow + nextRow + totalTrans - 1), startCol + (nextCol)).Value = statementDetail.CheckerTime;

                        #endregion checked time
                    }

                    detailIndex++;
                    nextRow++;
                }
                //nextRow++;
            }
        }

        #endregion export 2 excel
    }
}