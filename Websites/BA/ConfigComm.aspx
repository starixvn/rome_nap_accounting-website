<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfigComm.aspx.cs" Inherits="BA_ConfigComm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/jquery-easyui-1.2.1/themes/default/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Style/Commission.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>  
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC">
                            BA Setting
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box" style="width:1011px" >
                    <div id="dvMsg">
                    </div>
                <div id="gridConfigComm" class="content-warper hidden">
                     <%-- <div class="content-toolbox bgtitle">
                            <div id="gridtitle" class="gridtitle"></div><div class="btngroup">
                                <input type="button" value="  " name="btnBack" id="btnAdd" class="btnAdd" title="Add new Licensee" onclick="showPopup();" />
                            </div>
                        </div>--%>
                    <div class="content-header clearfix">
                        <div id="headerscrollbettype" class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w120 c">
                                            Licensee</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w260 c">
                                            Asian HDP, OU, OE Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w80 c">
                                            1x2 Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w80 c">
                                            Other Bettypes Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w60 c">
                                            Group</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w60 c">
                                            Allow BA</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w100 c">
                                        Last Modified By
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w120 c">
                                            Last Modified Date</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w40 c">
                                        Save
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" id="colViewLog">
                                        <div class="w40 c">
                                        Log
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div id="CommLicensee" class="body-scroll">
                             <table class="tbl-body-scroll" >
                                <tbody id="tblComm" data-bind='template:{name:"CommissionTpl", foreach: items}'></tbody>
                             </table>
                            <script id="CommissionTpl" type="text/html">
                                <tr>
                                    <td><div class="w120 c">${Name}</div></td>
                                    <td><div class="w260 c">
                                        <table width="100%" >   
                                            <tr>
                                                <td style="border:none 0px;">Group A</td>
                                                <td style="border:none 0px;"><select style="width:70px" data-bind="options:getBACommissionValues(1), value:ValueGroupA"></td>
                                                <td style="border:none 0px;">Group B</td>
                                                <td style="border:none 0px;"><select style="width:70px" data-bind="options:getBACommissionValues(2), value:ValueGroupB"></select></td>
                                            </tr>
                                            <tr>
                                                <td style="border:none 0px;">Group C</td>
                                                <td style="border:none 0px;"><select style="width:70px" data-bind="options:getBACommissionValues(3), value:ValueGroupC"></td>
                                                <td style="border:none 0px;">Group D</td>
                                                <td style="border:none 0px;"><select style="width:70px" data-bind="options:getBACommissionValues(4), value:ValueGroupD"></select></td>
                                            </tr>
                                        </table>
                                        </div></td>
                                    <td><div class="w80 c"><select style="width:70px" data-bind="options: getBACommissionValues(5), value:Value1X2Comm"></select></div></td>
                                    <td><div class="w80 c"><select style="width:70px" data-bind="options: getBACommissionValues(6), value:ValueOtherComm"></select></div></td>
                                    <td><div class="w60 c"><select style="width:40px" data-bind="options: ConfigComm.VM.group, value:ValueGroup"></select></div></td>
                                    <td><div class="w60 c"><input type="checkbox" data-bind="checked:EnableBA"  /></div></td>
                                    <td><div class="w100 c" data-bind="text:ModifyBy"></div></td>
                                    <td><div class="w120 c" data-bind="text:ModifyDate"></div></td>
                                    <td><div class="w40 c"><input type="button" title="Update Licensee Commission" class="btnSave" data-bind="enable: isEnable() == true ? true:false , click: function (){ConfigComm.VM.updateConfig($data);}  " /><span style="display:none;">${isEnable}</span></td>                            
                                    <td style="display:{{if !showLog}}none{{/if}}"><div class="w40 c"><span class="canchange" onclick="ConfigComm.viewLog(${Id})">[View]</span></td>
                                </tr>
                            </script>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
   
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../js/jQuery/knockout-1.2.0.js" type="text/javascript"></script>
    <script src="../js/jQuery/Jlinq/jlinq.js" type="text/javascript"></script>
    <script src="../js/jQuery/Jlinq/data.js" type="text/javascript"></script>
    <script src="../js/dialogbox.js" type="text/javascript"></script>
    <script src="Script/ConfigComm.js" type="text/javascript"></script>
    <script type="text/javascript">
         
        var MinGroupAComm = <%=MinGroupAComm %>;
        var MaxGroupAComm = <%=MaxGroupAComm %>;
        var StepGroupAComm = <%=StepGroupAComm %>;
        var MinGroupBComm = <%=MinGroupBComm %>;
        var MaxGroupBComm = <%=MaxGroupBComm %>;
        var StepGroupBComm = <%=StepGroupBComm %>;
        var MinGroupCComm = <%=MinGroupCComm %>;
        var MaxGroupCComm = <%=MaxGroupCComm %>;
        var StepGroupCComm = <%=StepGroupCComm %>;
        var MinGroupDComm = <%=MinGroupDComm %>;
        var MaxGroupDComm = <%=MaxGroupDComm %>;
        var StepGroupDComm = <%=StepGroupDComm %>;
        var Min1x2Comm = <%=Min1X2Comm %>;
        var Max1x2Comm = <%=Max1X2Comm %>;
        var Step1x2Comm = <%=Step1X2Comm %>;
        var MinOtherComm = <%= MinOtherComm %>;
        var MaxOtherComm =<%=MaxOtherComm %>;
        var StepOtherComm = <%=StepOtherComm %>;

        var showLog=<%=isAllowViewLog%>;
    </script>
</body>
</html>
