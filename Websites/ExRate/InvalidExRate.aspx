<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvalidExRate.aspx.cs" Inherits="InvalidExRate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>     
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript" src="../js/prototype.js"></script>
    <script type="text/javascript" src="../js/common.js"></script> 
    <script type="text/javascript">     
    
        function ConvertToDate(date){
             var arr=date.split('/');                       
             var date=new Date();
             date.setYear(arr[2]);
             date.setMonth(arr[0]-1);
             date.setDate(arr[1]);                          
             return date;
        }
        function SearchInvalidExRate(){             
            if(ConvertToDate($('fdate').value) > ConvertToDate($('tdate').value)){
                //$('errMsg').innerHTML = "'To date' must be greater than or equal 'from date'!";
                showAlertMessage("ErrMsg", "'To date' must be greater than or equal 'from date'!", 1);
            }
            else{
                $('errMsg').innerHTML='';
                try{parent.BlockUI();}catch(e){}
                
                try{
                                                          
                    var  params='params=' + encodeURIComponent($('fdate').value.trim()) + ';' + encodeURIComponent($('tdate').value.trim()) ; 
                    
                    var ajax= new Ajax.Request("../Handlers/InvalidExRate.ashx",{
                        method:'post',  
                        parameters:params.replace('',''),     
                        onComplete:function(data){
                                     ProcessResult(data.responseText);                               
                        }
                   });  
                      
               }
               catch(e){
                    alert('Invalid data!');
               }
   
            }
                
        }     
        function ProcessResult(data){
         try{     
               
                 var arr=data.split("$$$");                
                 if(arr[0]=='1'){
                         sXmlBody = "<table><tbody id='content'>" + arr[1] + "</tbody></table>";
                         var tempDiv = document.createElement("DIV");
                         tempDiv.innerHTML = sXmlBody;
                         $("tblResult").removeChild($("content"));
                         $("tblResult").appendChild(tempDiv.firstChild.firstChild);
                 }
                 else
                 {

                     //$('errMsg').innerHTML = arr[0];
                     showAlertMessage("ErrMsg", arr[0], 1);
                 }
                 try{parent.jQuery.unblockUI();}catch(e){}
                 
             }
                 catch(e){
                    alert(data);
          }
       }
    </script>
</head>
<body>    
    <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    <asp:Literal ID="InvalidExchangeRate" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
            <div id="ErrMsg" style="margin-bottom: 2px;"></div>
            <%=sSearch%>
            </div>
            <div class="bg_table">
                <asp:Literal ID="InvalidExRateListContentHtml" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</body>
</html>    