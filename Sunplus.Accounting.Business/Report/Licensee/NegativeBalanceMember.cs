using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.Business.Report.Deposit
{
    public partial class ReportWLBySiteService
    {
        #region SelNegativeBalanceMemberFromDB
        private string sessionkeyNegativeBalance = "LicenseeReportSelNegativeBalanceMember";
        NegativeBalanceMemberDTO SelNegativeBalanceMemberFromDB(string site, bool fromCache)
        {
            NegativeBalanceMemberDTO negative = new NegativeBalanceMemberDTO();
            negative.Items = new List<NegativeBalanceMemberByLicensee>();
            negative.Total = new NegativeBalanceMemberRow();
            negative.MailList=new List<NegativeBalanceMemberMailFormat>();
            try
            {
                DataTable tb = new DataTable();
                var tbcached = AppSession.GetSession(System.Web.HttpContext.Current.Session, sessionkeyNegativeBalance);

                if (tbcached == null || fromCache == false)
                {
                    tb = _licenseReportDAO.SelNegativeBalanceMember(site);
                }
                else
                {
                    negative = (NegativeBalanceMemberDTO)tbcached;
                    return negative;
                }
                negative.totalitem = tb.Rows.Count.ToString();

                List<string> Licensees = tb.AsEnumerable().Select(s => CastAide.AsString(s["site"])).Distinct().ToList();

                foreach (string licensee in Licensees)
                {
                    DataRow[] rows = tb.Select("site='" + licensee + "'");
                    NegativeBalanceMemberByLicensee byLicensee = new NegativeBalanceMemberByLicensee();
                    byLicensee.Items = new List<NegativeBalanceMemberRow>();
                    byLicensee.Total = new NegativeBalanceMemberRow();

                    foreach (DataRow row in rows)
                    {
                        byLicensee.Items.Add(new NegativeBalanceMemberRow()
                                                 {
                                                     Licensee = CastAide.AsString(row["site"]),
                                                     Name = CastAide.AsString(row["username"]),
                                                     Currency = CastAide.AsString(row["currencyName"]),
                                                     Balance = NumberFormat.DecFormat(row["balance"], 2)
                                                 });
                    }

                    var mail = new NegativeBalanceMemberMailFormat();
                    var mailElement =
                        MailSection.Instance[MailSection.Setting.Send]["Licensee"].Mails[licensee.ToLower()];
                    if (mailElement != null)
                    {
                        mail.Site = licensee;
                        mail.Cc = mailElement.Cc;
                        mail.To = mailElement.To;
                        mail.Bcc = mailElement.Bcc;
                    }
                   
                if (byLicensee.Items.Count > 0)
                    {
                        negative.MailList.Add(mail);
                        byLicensee.Total = new NegativeBalanceMemberRow()
                        {
                            Name = licensee
                            //Balance = NumberFormat.DecFormat(byLicensee.Items.Sum(s => CastAide.AsDecimal(s.Balance)), 2)
                        };
                    }

                    negative.Items.Add(byLicensee);
                }

                //negative.Total = new NegativeBalanceMemberRow()
                //{
                //    Name = "Grand Total",
                //    Balance = NumberFormat.DecFormat(negative.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(l => CastAide.AsDecimal(l.Balance)))), 2)
                //};

                AppSession.AddSession(System.Web.HttpContext.Current.Session, sessionkeyNegativeBalance, negative);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return negative;
        }
        #endregion

        #region NegativeBalanceMemberDTO
        public NegativeBalanceMemberDTO SelNegativeBalanceMember(string site, int pageIndex, int pageSize, bool fromCache)
        {
            
            NegativeBalanceMemberDTO negativePaging = new NegativeBalanceMemberDTO();
            negativePaging.Items = new List<NegativeBalanceMemberByLicensee>();
            negativePaging.Total = new NegativeBalanceMemberRow();
            negativePaging.MailList=new List<NegativeBalanceMemberMailFormat>();
            try
            {
               var rsCached = SelNegativeBalanceMemberFromDB(site, fromCache);
                negativePaging.Total = rsCached.Total;
                negativePaging.totalitem = rsCached.totalitem;
                negativePaging.MailList = rsCached.MailList;
              
                int from = pageSize*(pageIndex - 1);
                int to = Math.Min(pageSize * pageIndex, CastAide.AsInt32(rsCached.totalitem));
                int totalmember = 0;
                bool isBreaked = false;

                foreach (var licensee in rsCached.Items)
                {
                    var l = new NegativeBalanceMemberByLicensee();
                    l.Items=new List<NegativeBalanceMemberRow>();
                    l.Total = new NegativeBalanceMemberRow();
                    l.Total = licensee.Total;
                    int total1 = licensee.Items.Count;
                  
                    if (isBreaked)
                    {
                        break;
                    }
                   
                    for (int i = 0; i < total1; i++)
                    {
                        NegativeBalanceMemberRow row = licensee.Items[i];
                        if ((total1+totalmember)<from)
                        {
                            break;
                        }
                        if (from <= (i + totalmember) && (i + totalmember) < to)
                        {
                            l.Items.Add(row);
                        }
                        if ((i + totalmember)>= to)
                        {
                            isBreaked = true;
                            break;
                        }
                    }
                    totalmember += total1;
                    if (l.Items.Count>0)
                    {
                        negativePaging.Items.Add(l);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return negativePaging;
        }
        #endregion

        #region sendmail
        
        public NegativeBalanceMemberSendMailFail SendMailNegativeBalanceReport(List<NegativeBalanceMemberMailFormat> mails, string reportTitle, string subject, string mailContent)
        {
            var sendMailFailList = new NegativeBalanceMemberSendMailFail();
            sendMailFailList.Licensees = new List<NegativeBalanceMemberSendMailResult>();
            try
            {
                var sendMailStatus = true;
                foreach (NegativeBalanceMemberMailFormat mail in mails)
                {
                    var currentLincesse = string.Format(char.ToUpper(mail.Site[0]) + mail.Site.Substring(1));
                    var title = string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                 "titleNegativeBalanceReport"),
                                              currentLincesse);

                    Export2ExcelNegativeBalanceReportMain(title, currentLincesse, false, mail.To, mail.Cc, mail.Bcc,
                                                          string.Format("{0} for {1} ({2})", subject, mail.Site,
                                                                        DateTime.Now.ToShortDateString()), mailContent, ref sendMailStatus);
                    if(sendMailStatus == false)
                    {
                        sendMailFailList.IsFail = true;
                        sendMailFailList.Licensees.Add(new NegativeBalanceMemberSendMailResult()
                                                           {
                                                               Site = mail.Site,
                                                               IsSuccess = false,
                                                               ErrorMessage = string.Format("- {0}: {1}", mail.Site,
                                                                                            ResourceExtension.
                                                                                                GetResourceValue(
                                                                                                    ResourceExtension.
                                                                                                        Resources.
                                                                                                        Language,
                                                                                                    "lblSendMailFail"))
                                                           });
                    }
                }
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return sendMailFailList;
        }
        #endregion
    }
}
