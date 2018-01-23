<%@ Page Language="C#" AutoEventWireup="true" CodeFile="__HedgeRecordEdit.aspx.cs" Inherits="Report_HedgeRecordEdit" Title="Untitled Page" %>
<html>
    <head runat="server">
        <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
        <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
        <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>    

        <script type ="text/javascript" src ="../js/prototype-full.js"></script>
        <script type ="text/javascript" src="../js/common.js"></script>
        <script type ="text/javascript">
        function OnKP(e, val)
         {
            if (!e) e =window.event;    
	        var key =(e.keyCode) ? e.keyCode : e.which;		   
            if (key == 13 || key == 8) return true;// accept backspace
   	        //if (/^$/.test(val.value) && /0/.test(String.fromCharCode(key))) return false;
            if (/[^0-9]/.test(String.fromCharCode(key))) return false;     												        																	
	        return true;
        }
        
        function ValidDataInfo()
        {
            $('errMsg').innerHTML = '';
            var arrID = ['txtHScore', 'txtAScore', 'txtPrice', 'txtAmount'];           
            var arrMsg = ['Home Live Score', 'Away Live Score', 'Price', 'Amount'];
            for(i = 0; i < arrID.length; i++)
            {
                if($('txtLive').value == '0' && ((arrID[i] == 'txtHScore') || (arrID[i] == 'txtAScore'))) continue;
                if(!isNumeric(arrID[i])) 
                { 
                    $('errMsg').innerHTML = '*' + arrMsg[i] + ' must be a number.';
                    $(arrID[i]).focus();
                    return false;
                }                
            }
            
            for(i = 0; i < arrID.length; i++)
            {
                if($('txtLive').value == '0' && ((arrID[i] == 'txtHScore') || (arrID[i] == 'txtAScore'))) continue;
                if(arrID[i] == 'txtPrice') continue;
                if(parseFloat($(arrID[i]).value)<0)
                {
                    $('errMsg').innerHTML = '*' + arrMsg[i] + ' must be >= 0';
                    $(arrID[i]).focus();
                    return false;
                }
            }
            
            // valid data - prepair to update data into DB
            var param = 'hdp1= ' + $('ddlHDP1').value 
            param +=  '&hdp2= ' + $('ddlHDP2').value 
            
            if($('txtLive').value == '1')
            {
                param +=  '&HomeScore=' + $('txtHScore').value ;
                param +=  '&AwayScore=' + $('txtAScore').value ;
            }
            param +=  '&Price=' + $('txtPrice').value ;
            param +=  '&Amount=' + $('txtAmount').value ;
            param +=  '&Remark=' + $('txtRemark').value ;
            param +=  '&Live=' + $('txtLive').value ;
            param +=  '&TransID=' + $('txtTransID').value ;
            var myajax= new Ajax.Request("../Handlers/UpdateHedgeRecord.ashx",{
                asynchronous:true,
                method:'get',        
                parameters: param,
                onComplete:Updated
            });
            return true;
        }
        
        function Updated(rs)
        {
            var str = rs.responseText;
            if(str.split('|')[0] == 'ok')
            {
                parent.ibcWnd.close();
                parent.GetData();
            }
            else
                $('errMsg').innerHTML = '*' + str.split('|')[1];
        }
    </script>
    </head>
    <body>
        <center>
            <asp:Literal ID ="content" runat ="server"></asp:Literal>            
        </center>
    </body>
</html>
    
    
