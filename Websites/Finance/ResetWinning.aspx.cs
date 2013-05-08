using System;
using System.Text;
using System.Collections.Generic;
using DNAide.Web;
using DNAide.Utils;
//using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;


public partial class Finance_ResetWinning : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ltrContent.Text = GetResetWinningContent();
    }

    public string GetResetWinningContent()
    {
        StringBuilder strBuild = new StringBuilder();
        string strTpl = string.Empty;
        try
        {
            strTpl = WebUtils.LoadTemplate("Finance/ResetWinning.htm");
            IUserService userService = CommonBootStrapper.Locator.GetInstance<IUserService>();

            //Get Data here
            var list = userService.GetResetWinning();
            //List<ResetWinning> list = new List<ResetWinning>();
            
            if (list.Count > 0)
            {
                decimal dcBalance = 0;
                decimal dcAmount = 0;
                decimal dcRange = new decimal(0.01);
                int count = 0;

                foreach (var r in list)
                {   
                    string strTmp = strTpl;
                    if (count == 0)//First row
                    {
                        dcBalance = r.Amount;
                        TemplateHandler.Assign(ref strTmp, "date", r.Date.ToString("M/d/yyyy"));
                        TemplateHandler.RemoveBlock(ref strTmp, "reset");
                        TemplateHandler.ReplaceBlock(ref strTmp, "viewtrans", "-");                        
                        TemplateHandler.ReplaceBlock(ref strTmp, "view", "-");
                        TemplateHandler.Assign(ref strTmp, "amount", "-");
                        TemplateHandler.Assign(ref strTmp, "balance", NumberFormat.DecFormat(dcBalance, 2));
                    }
                    else
                    {
                        dcAmount = r.Amount;
                        dcBalance += dcAmount;

                        TemplateHandler.Assign(ref strTmp, "date", r.Date.ToString("M/d/yyyy"));
                        TemplateHandler.Assign(ref strTmp, "disabled", r.IsReset ? "disabled" : string.Empty);
                        if(r.IsReset && dcAmount != 0)
                        {
                            TemplateHandler.Assign(ref strTmp, "amtcss", "red");
                            TemplateHandler.Assign(ref strTmp, "isreset",  "1");
                        }
                        TemplateHandler.Assign(ref strTmp, "amtcss", string.Empty);
                        TemplateHandler.Assign(ref strTmp, "isreset", "0");
                        TemplateHandler.Assign(ref strTmp, "amount", NumberFormat.DecFormat(dcAmount, 2));
                        TemplateHandler.Assign(ref strTmp, "balance", NumberFormat.DecFormat(dcBalance, 2));

                        if (dcAmount <= dcRange && dcAmount >= (-dcRange))
                        {
                            TemplateHandler.RemoveBlock(ref strTmp, "reset");
                            TemplateHandler.ReplaceBlock(ref strTmp, "viewtrans", "-"); 
                        }
                        else
                            TemplateHandler.RemoveBlock(ref strTmp, "view");                            
                        
                        TemplateHandler.Assign(ref strTmp, "transactions", r.Date.ToString("M/d/yyyy"));
                        TemplateHandler.Assign(ref strTmp, "action", r.Date.ToString("M/d/yyyy"));
                    }
                    count++;
                    
                    strBuild.Append(strTmp);
                }
            }
            else
            {
                strBuild.Append(AccCommon.GenNoInformation("4"));
            }
        }
        catch (DNAide.Utils.AccException ex)
        {
            strTpl = AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);
            Logs.Write(ex.Message);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return strBuild.ToString();
    }
}

