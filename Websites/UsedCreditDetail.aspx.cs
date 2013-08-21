using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;


public partial class UsedCreditDetail : RequiredAuthorizationPage
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private int _accid = -1;
    private Boolean hasData = false;




    protected override int CustId
    {
        get
        {
            return Convert.ToInt32(Session["CustId"] as object ?? -1);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
     
        string _s = string.Empty;
        try
        {
            _accid = Convert.ToInt32(Session["AccId"] as object ?? -1);
            if (_accid == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);

            int _custid = CustId;
            int _roleid = Convert.ToInt32(Session["CustRoleId"] as object ?? -1);
            if (_custid == -1 || _roleid == -1) throw new AccException(IbcError.USERNAME_UNAVAILABLE_MSG);
            
            string _resetclosedonly = Request["resetClosed"] as string ?? string.Empty;
            string _resetall = Request["resetAll"] as string ?? string.Empty;
            if (!string.IsNullOrEmpty(_resetclosedonly))
            {
                _s = AgentResetMemberCredit(_custid, true);
            }
            else if (!string.IsNullOrEmpty(_resetall))
            {
                _s = AgentResetMemberCredit(_custid, false);
            }
            else
            {
                switch (_roleid)
                {
                    case 2: //agent
                        _s = "<form method=\"post\">" + AgentCredit(_custid) + "</form>";
                        break;
                    case 3: //master
                        _s = MasterCredit(_custid);
                        break;
                    case 4: // super
                        _s = SuperCredit(_custid);
                        break;
                    default:
                        throw new AccException(IbcError.USERID_INVALID_MSG);                        
                }
            }
        }        
        catch (AccException ex)
        {
            _s = AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);
            Logs.Write(ex.Message);
        }        
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        this.sBody.Text = _s;
    }

    private string AgentCredit(int _custid)
    {
        string sAgent = string.Empty;
        try
        {
            sAgent = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserCredit/AgentCredit.htm");
            StringBuilder sbrActive = new StringBuilder();
            StringBuilder sbrInactive = new StringBuilder();
            string _sRow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserCredit/AgentRow.htm");
            bool _hasClosed = false;
            bool hasActive = false;
            bool hasInactive = false;
            double _activeSum = 0.0, _inactiveSum = 0.0;
            string odd_even = "";
            using (DataTable dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserCredit.SelectAgentCredit(_custid))
            {                
                foreach(DataRow dr in dt.Rows)
                {
                    string _s = _sRow;                                                
                    _hasClosed =Convert.ToBoolean(dr["closed"]);
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref _s, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref _s, "{username}", dr["username"].ToString());
                    TemplateHandler.Assign(ref _s, "{firstname}", dr["firstname"].ToString());
                    TemplateHandler.Assign(ref _s, "{lastname}", dr["lastname"].ToString());
                    double _credit = (Convert.IsDBNull(dr["credit"]) ? 0.00 : Convert.ToDouble(dr["credit"]));
                    TemplateHandler.Assign(ref _s, "{amount}", string.Format(cult, "{0:#,##0.00}", _credit));
                    if (_hasClosed)
                    {
                        hasInactive = true;
                        sbrInactive.Append(_s);
                        _inactiveSum += _credit;
                    }
                    else
                    {
                        hasActive = true;
                        sbrActive.Append(_s);
                        _activeSum += _credit;
                    }
                }                
            }
            if (hasActive || hasInactive)
            {
                TemplateHandler.Assign(ref sAgent, "{sumall}", string.Format(cult, "{0:#,##0.00}", _activeSum + _inactiveSum));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref sAgent, "no-total");
            }
            if (hasActive)
            {
                TemplateHandler.Assign(ref sAgent, "{activeContent}", sbrActive.ToString());
                TemplateHandler.Assign(ref sAgent, "{sumactive}", string.Format(cult, "{0:#,##0.00}", _activeSum));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref sAgent, "no-actotal");
                TemplateHandler.Assign(ref sAgent, "{activeContent}", AccCommon.GenNoInformation("4"));
            }
            if (hasInactive)
            {
                TemplateHandler.Assign(ref sAgent, "{inactiveContent}", sbrInactive.ToString());
                TemplateHandler.Assign(ref sAgent, "{suminactive}", string.Format(cult, "{0:#,##0.00}", _inactiveSum));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref sAgent, "no-intotal");
                TemplateHandler.Assign(ref sAgent, "{inactiveContent}", AccCommon.GenNoInformation("4"));
            }            
        }
        catch (Exception ex)
        {

            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
        }
        return sAgent;
    }

    private string MasterCredit(int _custid)
    {
        string sMaster = string.Empty;
        try
        {
            sMaster = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserCredit/MasterCredit.htm");
            StringBuilder sbrRows = new StringBuilder();            
            string _sRow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserCredit/MasterRow.htm");
            double _SumGiven = 0.0, _SumUsed = 0.0;
            string odd_even = "";
            using (DataTable dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserCredit.SelectMasterCredit(_custid))
            {                
                foreach(DataRow dr in dt.Rows)
                {
                    hasData = true;
                    string _s = _sRow;                    
                    double _givenCredit = Convert.IsDBNull(dr["givencredit"]) ? 0.00 : Convert.ToDouble(dr["givencredit"]);
                    double _usedCredit = Convert.IsDBNull(dr["usedcredit"]) ? 0.00 : Convert.ToDouble(dr["usedcredit"]);
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref _s, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref _s, "{username}", dr["agentname"].ToString());
                    TemplateHandler.Assign(ref _s, "{givencredit}", string.Format(cult, "{0:#,##0.00}", _givenCredit));
                    TemplateHandler.Assign(ref _s, "{usedcredit}", string.Format(cult, "{0:#,##0.00}", _usedCredit));
                    sbrRows.Append(_s);
                    _SumGiven += _givenCredit;
                    _SumUsed += _usedCredit;
                }
            }
            if (hasData)
            {
                TemplateHandler.Assign(ref sMaster, "{content}", sbrRows.ToString());
                TemplateHandler.Assign(ref sMaster, "{sumgiven}", string.Format(cult, "{0:#,##0.00}", _SumGiven));
                TemplateHandler.Assign(ref sMaster, "{sumused}", string.Format(cult, "{0:#,##0.00}", _SumUsed));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref sMaster, "no-total");
                TemplateHandler.Assign(ref sMaster, "{content}", AccCommon.GenNoInformation("3"));
            }
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return sMaster;
    }

    private string SuperCredit(int _custid)
    {
        string sSuper = string.Empty;
        try
        {
            sSuper = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserCredit/SuperCredit.htm");
            StringBuilder sbrRows = new StringBuilder();
            string _sRow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserCredit/SuperRow.htm");
            double _SumGiven = 0.0, _SumUsed = 0.0;
            string odd_even = "";
            DataTable dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserCredit.SelectSuperCredit(_custid);
            
            foreach(DataRow dr in dt.Rows)
            {
                hasData = true;
                string _s = _sRow;
                double _givenCredit = Convert.IsDBNull(dr["givencredit"]) ? 0.00 : Convert.ToDouble(dr["givencredit"]);
                double _usedCredit = Convert.IsDBNull(dr["usedcredit"]) ?0.00 : Convert.ToDouble(dr["usedcredit"]);
                odd_even = (odd_even == "") ? "evenrow" : "";
                TemplateHandler.Assign(ref _s, "{odd_even}", odd_even);
                TemplateHandler.Assign(ref _s, "{username}", dr["mastername"].ToString());
                TemplateHandler.Assign(ref _s, "{givencredit}", string.Format(cult, "{0:#,##0.00}", _givenCredit));
                TemplateHandler.Assign(ref _s, "{usedcredit}", string.Format(cult, "{0:#,##0.00}", _usedCredit));
                sbrRows.Append(_s);
                _SumGiven += _givenCredit;
                _SumUsed += _usedCredit;
            }
           
            if (hasData)
            {
                TemplateHandler.Assign(ref sSuper, "{content}", sbrRows.ToString());
                TemplateHandler.Assign(ref sSuper, "{sumgiven}", string.Format(cult, "{0:#,##0.00}", _SumGiven));
                TemplateHandler.Assign(ref sSuper, "{sumused}", string.Format(cult, "{0:#,##0.00}", _SumUsed));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref sSuper, "no-total");
                TemplateHandler.Assign(ref sSuper, "{content}", AccCommon.GenNoInformation("3"));
            }
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return sSuper;
    }

    private string AgentResetMemberCredit(int _custid, bool _isClosedOnly)
    {
        string str = string.Empty;
        try
        {            
            Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserCredit.ResetMemberCredit(_accid, _custid, _isClosedOnly);            
            string msg = (_isClosedOnly) ? "Reset All Closed Account Credit Successfully!" : "Reset All Account Credit Successfully!";
            str = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success);
            str += "<input class='btn' type='button' value='BACK' onclick='javascript:document.location.replace(document.location);'/>";            
        }
        catch(Exception ex)
        {
            str = AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }        
        return str;
    }
}
