<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="UpdateHRPT.aspx.cs"
    Inherits="Customer_V2_UpdateHRPT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Horce Racing Setting</title>
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />

    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../../../js/BlockUI.js" type="text/javascript"></script>

    <script src="../../../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>

    <script src="../../../js/common.js" type="text/javascript"></script>

    <script src="../../../js/jshashtable.js" type="text/javascript"></script>

    <script src="../../../js/agent/Customer.js" type="text/javascript"></script>

    <script src="../../../js/agent/HorseRacingPT.js" type="text/javascript"></script>

    <script src="../../../js/showhidepopup.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function changeDisableHR(objChk) {
            var chkdisableHRDL = obj("chkDisabledHRDL");
            if (chkdisableHRDL != null) {
                if (!objChk.checked) {
                    chkdisableHRDL.disabled = true;
                    chkdisableHRDL.checked = false;
                }
                else {
                    var oldHRStatus = obj('isDisabledHR').value;
                    var newHRStatus = objChk.checked ? "no" : "yes";
                   
                    if (oldHRStatus != newHRStatus) {
                        chkdisableHRDL.disabled = false;
                    }
                    else {
                        if (obj('isDisabledHRDL').value == "yes") {
                            chkdisableHRDL.checked = false;
                        }
                        else {
                            chkdisableHRDL.checked = true;
                        }
                        $(obj.id + "DL").disabled = true;
                    }
                }
            }
        }
        function changeDisableOpenTote(objChk) {
            var chkdisableHRDL = obj("chkDisabledOpenToteDL");
            if (chkdisableHRDL != null) {
                if (!objChk.checked) {
                    chkdisableHRDL.disabled = true;
                    chkdisableHRDL.checked = false;
                }
                else {
                  
                    var oldHRStatus = obj('isDisabledOpenTote').value;
                    var newHRStatus = objChk.checked ? "no" : "yes";
                    
                    if (oldHRStatus != newHRStatus) {
                        chkdisableHRDL.disabled = false;
                    }
                    else {
                        if (obj('isDisabledOpenToteDL').value == "yes") {
                            chkdisableHRDL.checked = false;
                        }
                        else {
                            chkdisableHRDL.checked = true;
                        }
                        $(obj.id + "DL").disabled = true;
                    }
                }
            }
        }
    </script>

    <style>
        #divtote
        {
            display: block;
        }
        #divfixed
        {
            display: block;
        }
    </style>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                        Update HorseRacing Setting
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <span id="errMsg" class="errMsg"></span><span id="errMsg151" class="error-message">
                </span>
            </div>
            <div class="bg_table">
                <form id="form1" runat="server">
                <div>
                    <asp:Literal ID="htmlContent" runat="server"></asp:Literal>
                </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
