<%@ WebHandler Language="C#" Class="Transfer" %>

using System;
using System.Web;
using System.Web.SessionState;
//using IBC.Core.LoggingLib;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using IBCUtils.Web;
using DNAide.Data;


public class Transfer : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        try
        {
            int _accid = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (_accid == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);
            int _fromcustid = Convert.ToInt32(context.Request.QueryString["fromCustID"]);
            string _fromusername = context.Request.QueryString["fromCustName"] as string ?? string.Empty;
            _fromusername = _fromusername.Trim();
            string _touser = context.Request["touser"].ToString().Trim();
            double _exRate = Convert.ToDouble(context.Request["exrate"].ToString());
            double _amount = Convert.ToDouble(context.Request["txtamount"].ToString());
            if (_amount == 0.0) throw new AccException(IbcError.INVALID_INPUT_VALUE_MSG);
            string _remark = context.Request["remark"] as string ?? string.Empty;

            string msg = "";
            bool IsTransfer = Permissions.IsFiTransfer(_fromcustid, _touser, ref msg); 
            if (IsTransfer)
            {
                var dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Transfer.BettransInsertTransfer(_accid, _fromcustid, _touser, _exRate, _amount, _remark, context.Request.UserHostAddress);
                context.Response.Write("<script type='text/javascript'>window.location.replace('../TransferResult.aspx?fuser=" + _fromusername + "&touser=" + _touser + "&exrate=" + _exRate + "');</script>");
            }
            else
            {
                context.Response.Write("<script type='text/javascript'>  parent.$('#errMsg').show() ;parent.showAlertMessage('errMsg','" + msg + "',1);</script>");
            }
        }
        catch (AccException ex)
        {
            //context.Response.Write("<script type='text/javascript'> parent.showAlertMessage('errMsg','" + ex.Message + "', 1)</script>");
            context.Response.Write("<script type='text/javascript'>parent.$('#errMsg').show() ;parent.showAlertMessage('errMsg','" + ex.Message + "', 1)</script>");
             
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            context.Response.Write("<script type='text/javascript'>parent.$('#errMsg').show() ;parent.showAlertMessage('errMsg','" + IbcError.INTERNAL_ERROR_MSG + "',1);</script>");
        }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}