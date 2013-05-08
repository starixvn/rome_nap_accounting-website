using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.PositionTaking.Racing;
using Sunplus.Accounting.WcfServiceLibrary.PT.Poker;
using Sunplus.Accounting.Entities;
using DNAide.Core;

/// <summary>
/// Summary description for P2PPage
/// </summary>
public class P2PPage : PageBase
{
    public string custId;
    public string roleId;
    public string isDisabled;
    public string msgDisabled;

    protected override void OnInit(EventArgs e)
    {
        
        base.OnInit(e);
       
        GetPageInfo();
    }
    
    public void GetPageInfo()
    {
        try
        {
            custId = Request["custId"].ToString();
            roleId = Request["roleId"].ToString();
            var pokerService=new PokerService();
            var result = new Result();
            result = pokerService.CheckEnableCustomer(CastAide.AsInt32(custId));
            if (result.ErrorCode > 0)
            {
                isDisabled = "true";
                msgDisabled = result.ErrorMessage;
            }
            else
            {
                isDisabled = "false";
                msgDisabled = "";

            }
        }
        catch(Exception ex){}
    }
}
