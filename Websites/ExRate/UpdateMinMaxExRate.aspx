<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateMinMaxExRate.aspx.cs" Inherits="UpdateMinMaxExRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
     <link rel="stylesheet" type="text/css" href="../App_Themes/Public/Message.css"/>     
     
    <script src="../js/prototype.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>

    <script src="../js/showhidepopup.js" type="text/javascript"></script>
</head>
<body style="margin:5px 5px 0px 5px">   
    <script type="text/javascript" language="javascript">
        var isEdit = false;
        function ValidateForm() {
            
            if($('txtmin').value=='')
            {
                showAlertMessage("errMsg", "Please input Min Ex.Rate", 1);
                $('txtmin').focus();
                return false;
            }
            else if ($('txtmax').value=='')
            {
                showAlertMessage("errMsg", "Please input Max Ex.Rate", 1);
                $('txtmax').focus();
                return false;
            }
            else if(parseFloat($('txtmin').value) > parseFloat($('txtmax').value) ) {
                showAlertMessage("errMsg", "The value of Max Ex.Rate cannot be smaller than Min Ex.Rate!", 1);
                return false;
            }
            else if ($('fromcur').value==$('tocur').value) {
                showAlertMessage("errMsg", "Please choose another currency to exchange", 1);
                return false;
            }
            if($('txtmin').value<-10 || $('txtmin').value>0) {
                showAlertMessage("errMsg", "Please input Min Ex.Rate between -10% and 0", 1);
                $('txtmin').focus();
                return false;
            }
            if($('txtmax').value<0 || $('txtmax').value>10) {
                showAlertMessage("errMsg", "Please input Max Ex.Rate between 0 and 10%", 1);
                $('txtmax').focus();
                return false;
            }
             if (!isFloat($('txtmin').value))
            {
                 showAlertMessage("errMsg", "Invalid data in Min Ex.Rate", 1);
                $('txtmin').focus();
                return false;
            }
            if (!isFloat($('txtmax').value))
            {
                 showAlertMessage("errMsg", "Invalid data in Max Ex.Rate", 1);
                $('txtmax').focus();
                return false;
            }
            
          
            return true;
        }
        function OnSubmit(status) {
            if(ValidateForm()==false)
            return;
            else {
                var strC=confirm("Are you sure you want to update min-max exchange rate of this currency?");
                if (!strC)
                {     
                    return false;
                }
                try{parent.BlockUI();}catch(e){}
                var myajax = new Ajax.Request("../Handlers/MinMaxExRate.ashx", {
                    asynchronous: true,
                    method: 'post',
                    parameters: { sta: status, frmId: $('fromcur').value, toId: $('tocur').value, min: $('txtmin').value, max: $('txtmax').value },
                    onComplete: function(strdata) {
                    
                        if (strdata.responseText == 'Ok') {
                            if (status=="Add")
                                showAlertMessage("errMsg", "Add min max exchange rate successfully.", 0);
                            else
                                showAlertMessage("errMsg", "Update min max exchange rate successfully.", 0);
                            isEdit = true;
                            afterUpdate(false,2000);
                        }
                        else if (strdata.responseText == 'You are not authorized on this function')
                        {
                             showAlertMessage("errMsg", strdata.responseText, 1);
                             afterUpdate(false,2000);
                        }
                        else {
                            try{parent.jQuery.unblockUI();}catch(e){}
                            showAlertMessage("errMsg", strdata.responseText, 1);
                        }

                    }
                });
            }
        }
        
     
        function FixValue(e){                        
            var target = e.target || e.srcElement;              
            if(target.id=='txtmin'){ 
               if(target.value.indexOf('-')<0){
                    target.value= "-" + target.value;                    
                }
                else if (target.value.indexOf('-')>0)
                    target.value= "-";
            }
            
        }
        function Blur(obj){        
            if(obj.value==0 || obj.value=='-' || obj.value=='-.' )
                obj.value='0.00';                
        }
        function CheckDecNumber_ExRate(e){
            var code=e.which||e.keyCode;
            if (code == 8) {
                return true;
            }
           if(code ==13 )
            {
                $('btnsubmit').click();
            }            
            if ( (code < 48 || code > 57 ||code==45 || code==110 || code==46) && code != 8 && code!=0 && code!=9 && (code<37 || code >40))
            {		
                var target = e.target || e.srcElement;                  
                if(code==45)//input hyphen/subtract '-'
                    return (target.value.indexOf('-')<0 && target.id != 'txtmax'); 
                if(code==46)
                    return (target.value.indexOf('.')<0);    
                return false;
            }
            
            return true;
        }
      
        window.onload=function()
        {
             $('txtmin').focus(); 
        }
        
    </script> 
    <div id="_sContent">
        <asp:Literal id="sContent" runat="server" />  
    </div> 
</body>
</html>
