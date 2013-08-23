using System;
using System.Collections;
using Sunplus.Accounting.Core.Extensions;

public partial class Customer_V2_UpdateTransferTime : RequiredAuthorizationPage
{
    protected override bool HasPermission()
    {
        return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UpdateData();
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);

            Response.Write("<script type=\"text/javascript\">parent.showAlertMessage('err','" + IBCUtils.Encryption.RemoveSpecialChars(ex.Message) + "',1); parent.afterUpdate(false, 2000);</script>");
        }
    }

    protected override void RedirectError()
    {
        Response.Write("<script type=\"text/javascript\">parent.showAlertMessage('err','" + Resources.ErrorMsg.error_InvalidRequest + "',1);</script>");
    }

    protected void UpdateData()
    {
        try
        {
            Hashtable param = new Hashtable();
            param.Add("custid", Request["custId"]);
            param.Add("userid", Session["AccId"]);

            string strUpdTranfer = "";

            if (Request["tranfer"].ToString() == "daily")
            {
                strUpdTranfer = "00000000";
            }
            else
            {
                if (Request["chkSun"] != null)
                {
                    strUpdTranfer += "1";
                }
                else
                {
                    strUpdTranfer += "0";
                }
                if (Request["chkMon"] != null)
                {
                    strUpdTranfer += "1";
                }
                else
                {
                    strUpdTranfer += "0";
                }
                if (Request["chkTue"] != null)
                {
                    strUpdTranfer += "1";
                }
                else
                {
                    strUpdTranfer += "0";
                }
                if (Request["chkWed"] != null)
                {
                    strUpdTranfer += "1";
                }
                else
                {
                    strUpdTranfer += "0";
                }
                if (Request["chkThurs"] != null)
                {
                    strUpdTranfer += "1";
                }
                else
                {
                    strUpdTranfer += "0";
                }
                if (Request["chkFri"] != null)
                {
                    strUpdTranfer += "1";
                }
                else
                {
                    strUpdTranfer += "0";
                }
                if (Request["chkSat"] != null)
                {
                    strUpdTranfer += "1";
                }
                else
                {
                    strUpdTranfer += "0";
                }
                strUpdTranfer += "1";
            }

            param.Add("transferoption", strUpdTranfer);
            param.Add("subaccid", 0);
            param.Add("language", "E");
            param.Add("errcode", 0);
            param.Add("errmsg", "");
            param.Add("returnerrcode", 1);
            // update missing IP
            var ipClient = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
            param.Add("ip", ipClient);

            var rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.UpdTransferOption(param);
            if (rs.ErrorCode != 0)
            {
                if (rs.ErrorCode == 500)//You are not authorized on this function
                {
                    Response.Write("<script type=\"text/javascript\">parent.showAlertMessage('err','" + rs.ErrorMessage + "',1);parent.afterUpdate(true, 2000); </script>");
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">parent.showAlertMessage('err','" + rs.ErrorMessage + "',1); </script>");
                }
            }
            else
            {
                Response.Write("<script type=\"text/javascript\">parent.showAlertMessage('err','Update transfer condition successfully.',0); parent.afterUpdate(true, 2000); </script>");
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }
}