<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterBalance.aspx.cs" Inherits="MasterBalance" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
<link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
<link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css"/>               
<link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/>      
<script type ="text/javascript" src ="../../js/common.js"></script> 
<script type ="text/javascript" src ="../../js/jQuery/jquery.js"></script>
<script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
<script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
<script src="../../js/FrozenHeader.js" type="text/javascript"></script>

    <script type="text/javascript">         
          
        function GetData(IsExport) {
            IsExport = (IsExport) ? '1' : '0';
                var param = '&IsYesterDay=' +  isYesterday;                                               
                if(obj('yestrans'))
                {
                    if(isYesterday == '1')
                    {
                        obj('yestrans').checked = 'checked';
                        obj('alltrans').checked = '';
                    }
                    else
                    {
                        obj('yestrans').checked = '';
                        obj('alltrans').checked = 'checked';
                    }                                        
                }
                
                if (!getPramJs('IsRep')) {
                    if (obj('dbRep'))
                        param += '&IsRep=' + ((obj('dbRep').checked) ? '1' : '0');
                    else
                        param += '&IsRep=0';
                }
                else
                    param += '&IsRep=' + getPramJs('IsRep');
               if(obj('txtSearch')){
                    param += '&username=' + ((obj('txtSearch')) ?  obj('txtSearch').value : '');
                    param += '&letter=' +  ((obj('listAlpha')) ?  obj('listAlpha').value : '');
                    param += '&currency=' +  ((obj('listCurrency')) ? obj('listCurrency').value : '');
               }                
                param += '&custid=' + custid;
                param += '&roleid=' + roleid;
                param += '&IsExport=' + IsExport;

                if (roleid == '2')
                    $('#spTitle').html('Agent Balance');
                if (roleid == '1')
                    $('#spTitle').html('Member Balance');    
                
                try{parent.BlockUI();}catch(e){}
                if (IsExport == '0') {

                    $.ajax(
                     {
                         type: "POST",
                         url: "MasterBalance.ashx",
                         data: param,
                         success: function(rs) {
                         obj('sData').innerHTML = rs;
                            InitFixedHeader(500, 'table', 'trFooter', 'trHeader');
                             parent.jQuery.unblockUI();
                         }
                     }
             );    
                }
                else { //export to excel
                    parent.jQuery.unblockUI();
                    window.location.href = 'MasterBalance.aspx?' + param; 
                }
        }
        
        function on_Blur()
        {
            if(obj('txtSearch').value.length == 0)
                obj('txtSearch').value = 'All';
        }
        
        function On_Focus()
        {
            if(obj('txtSearch').value == 'All')
                obj('txtSearch').value = '';
        }
        function On_KeyPress(e)
        {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which; 				
            if (code==13) GetData();
        }

        function ExportToExcel() {
            GetData(1);
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
                    <td class="maincontent_headerC">
                        <span id="spTitle">Master Balance</span>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <asp:Literal ID ="header" runat ="server"></asp:Literal>
                <asp:Literal ID ="contentmasterbalance" runat ="server"></asp:Literal>
            </div>
            <div class="bg_table">
                <div id="sData" style="width:400px"></div>
            </div>
        </div>
    </div>    
</body>
</html>