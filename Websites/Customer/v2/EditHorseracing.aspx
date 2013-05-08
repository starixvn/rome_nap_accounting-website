<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="EditHorseracing.aspx.cs" Inherits="Customer_V2_EditHorseracing" Title="Edit Horseracing Setting" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ReportContent" Runat="Server">
   <link href="../../App_Themes/SteelBlue/ibc_acounting.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/SteelBlue/Message.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/SteelBlue/Public.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/SteelBlue/report.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/SteelBlue/Table.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript" src="../../js/Customer.js"></script>
    <script type="text/javascript" src="../../js/prototype.js"></script>
    <script type="text/javascript" src="../../js/agent/Customer.js"></script>
    <script type="text/javascript">                    
                      
        function getObj(id)
        {
            return document.getElementById(id);
        }        
        
        function BtnOnClick()
		{
		    ClearAllMsg();
			if(!CheckLimitMinMaxBet(151)) return false;
//			var yes =false;	
//			var oldHRStatus = $('oldHRStatus').value;
//			var newHRStatus = $('chkDisabledHR').checked ? "0" : "1";
//            var isDL=$('IsUpdDownlineHR')
//            var isexistHRDL=$('chkDisabledHRDownLine');
//		    yes =($('chkDisabledHR').checked);
//            if(yes == true)
//            {
//                if($('roleId').value != "1")
//                {
//                    //if (!confirm(alertDisableHR))  return;
//                    if (isDL!=null)
//                    {
//                        isDL.value = "1";
//                    }
//                }
//                else
//                {
//                    if (isDL!=null)
//                    {
//                     isDL.value = "0";
//                     }
//                }
//            }
//            else if(yes == false)
//            {         
//                if($('roleId').value != "1")
//                {
//                    //if (!confirm(alertEnableHR))  return;
//                    if (isexistHRDL.checked==false)
//                    { 
//                       isDL.value = "0";
//                    }
//                    else
//                    {
//                        isDL.value = "1";
//                    }
//                }
//                else
//                {
//                   $('IsUpdDownlineHR').value = "0"; 
//                }
//                
//            }

			$("btnAdd").style.color="#cccccc";					
			$("btnAdd").disabled=true;		
			showProgressBar(true);						
			document.fUpdateHR.submit();
			return true;
		}
		function ShowErrorInsert(msg)
		{
			getObj('err').innerHTML = msg;
		}	
        function changeDisableHRDL(obj)
        {
             obj.checked ? $('newHRStatusDL').value="0" : $('newHRStatusDL').value="1";
           
        }
        
        function changeDisableHR(obj)
        {
            obj.checked ? $('newHRStatus').value="0" : $('newHRStatus').value="1";
            var isHRDL=$("chkEnabledHRDL");
            if (isHRDL!=null)
            {
                if (!obj.checked)//Disable
                {
                     isHRDL.disabled=true;
                     isHRDL.checked=false;
                }
                else//Enable
                {    
                    var oldHRStatus = $('oldHRStatus').value=="1" ? "0": "1";
	                var newHRStatus = $('chkEnabledHR').checked ? "1" : "0";
    	            
	                if (oldHRStatus != newHRStatus)
	                 {
                        isHRDL.disabled=false;
                     }
                     else
                     {
                    
                        if ($('oldHRStatusDL').value=="1")
                        {
                            isHRDL.checked=false;
                        }
                        else
                        {
                            isHRDL.checked=true;
                        }
                        isHRDL.disabled=true;
                        }
                     }
                 }
            }
</script>
    <iframe name="frUpdate" frameborder="0" class="hidden" height="150px" width="800px">
    </iframe>
    <form name="fUpdateHR" id="fUpdateHR" action="../../Handlers/<%=ControlHandler %>"
        target="frUpdate" method="post">
        <input type="hidden" name="set" id="set" value="<%=fromSet %>" />
        <input type="hidden" name="type" id="type" value="<%=type%>" />    
        <input type="hidden" name="uname" id="uname" value="<%=username%>" />    
        <!--add new v2-->
        
            <%=content.ToString() %>
        
    </form>
    
    <script type="text/javascript">
        showProgressBar(false);	
        //ShowError('<%=msg%>','err');
    </script>
</asp:Content>

