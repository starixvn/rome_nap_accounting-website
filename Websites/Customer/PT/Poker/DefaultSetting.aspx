<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultSetting.aspx.cs" Inherits="Customer_PT_Poker_DefaultSetting" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Poker Default Setting</title>
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="Style/grid.css" rel="stylesheet" />
    <link href="Style/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../../js/jQuery/jquery.js"></script>
    <script src="../../../js/jQuery/Number/jquery.number_format.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../../js/jQuery/Template/jquery.tmpl.js"></script>
    <script type="text/javascript" src="../../../js/jQuery/knockout-1.2.0.js"></script>
    <script src="../../../js/jQuery/gridscroller.js" type="text/javascript"></script>

    <script src="JS/CommonPT.js" type="text/javascript"></script>
    <script src="JS/PokerData.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/DefaultSetting.js"></script>
    <script>
        var maxSComm = 0.67;
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
                        <div id="divTitle">Poker Default Setting</div>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:810px;" >
                <div id="err"></div>
            </div>
            <div id="divMainBody" class="content-warper hidden" style="margin-top:5px;">
                 <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col20 c b">
                                            #</div>
                                    </td>
                                   <td style="border-bottom: none 0px;">
                                        <div class="col100 c b">
                                            Currency</div>
                                    </td>
                                     <td style="border-bottom: none 0px;">
                                        <div class="col100 c b">
                                            Member Min<br />Transfer Credit</div>
                                    </td>
                                     <td style="border-bottom: none 0px;">
                                        <div class="col100 c b">
                                            Member Max<br />Transfer Credit</div>
                                    </td>
                                     <td style="border-bottom: none 0px;">
                                        <div class="col100 c b">
                                            Super Max Comm</div>
                                    </td>
                                     <td style="border-bottom: none 0px;">
                                        <div class="col100 c b">
                                            Master Max Comm</div>
                                    </td>
                                     <td style="border-bottom: none 0px;">
                                        <div class="col100 c b">
                                            Agent Max Comm</div>
                                    </td>
                                     <td style="border-bottom: none 0px;">
                                        <div class="col100 c b">
                                            Member Max Comm</div>
                                    </td>
                                     <td style="border-bottom: none 0px;">
                                        <div class="col50 c b"><input type="button" id="btnUpdateAll" onclick="pokerData.updateDefaultSettingMultiple();" disabled="disabled" title="Update" class="btnEdit" /></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
               <div class="content-body clearfix">
                    <div id="defaultSetting" class="body-scroll" >
                        <table class="tbl-body-scroll">
                            <tbody id="tblDefault" data-bind='template:{name:"tplRow", foreach: items}'>
                            </tbody>
                        </table>
                       <script id="tplRow" type="text/html">
                            <tr>
                                <td><div class="col20 c">${No}</div></td>
                                <td><div class="col100 c">${Currency}<div></td>
                                <td><div class="col100 c"><input id="txtmintransfer" class="noneDecimal col60" type="text" data-bind="value:MinCredit, valueUpdate:'afterkeydown'"  /></div></td>
                                <td><div class="col100 c"><input id="txtmaxtransfer" class="noneDecimal col60" type="text" data-bind="value:MaxCredit, valueUpdate:'afterkeydown'"  /></div></td>
                                <td><div class="col100 c"><select class="col60" data-bind="options: Arrsdiscount, value:Maxsdiscount" /></div></td>
                                <td><div class="col100 c"><select class="col60" data-bind="options: getAdminCommissionValues(0,Maxsdiscount()), value: Maxadiscount" /></div></td>
                                <td><div class="col100 c"><select class="col60" data-bind="options: getAdminCommissionValues(0,Maxadiscount()), value: Maxdiscount" /></div></td>
                                <td><div class="col100 c"><select class="col60" data-bind="options: getAdminCommissionValues(0,Maxdiscount()), value: Maxplayerdiscount" /></div></td>
                                <td><div class="col50 c"><input type="button" data-bind="enable: isUpdate()== false ? false:true, click: function(){pokerData.updateDefaultSetting($data);}" title="Update" class="btnEdit" /></div></td>
                            </tr>
                       </script>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
