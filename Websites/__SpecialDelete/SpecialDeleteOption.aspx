<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpecialDeleteOption.aspx.cs" Inherits="SpecialDelete_SpecialDeleteOption" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />

<script type="text/javascript" src="../js/common.js"></script>

    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>

    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>

    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>

    <script src="../js/common.js" type="text/javascript"></script>

    <script src="../js/SpecialDelete.js" type="text/javascript"></script>

    <style type="text/css">
        input[type=checkbox]
        {
            cursor: pointer;
        }
    </style>
    
    
</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC">
                    Special Delete Other Condition
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="ErrMsg" class="errMsg">
            </div>
        </div>
        <div class="bg_table">
            
            <div id="dvHeader">
                <label style="font-weight:bold">English</label> <br />
                <input type="text" style="height:148px; width:673px"  id="men" /> <br />

                <label style="font-weight:bold">CH</label> <br />

                <input type="text" id="mch" style="height:148px; width:673px" /> <br />

                <label style="font-weight:bold">CS</label> <br />

                <input type="text" id="mcs" style="height:148px; width:673px" /> <br />
                <input type="button" class="btn" value="Delete Bet" onclick="DeleteBet();" />
                <input type="button" class="btn" value="Send Mail" onclick="SendMail();" /></div>
            <div id="dvContent">
            </div>

            
            <br>
            <script type="text/javascript">
                function DeleteBet() {
                    try { BlockUI(); } catch (e) { }
                    var men = $("#men").val();
                    var mch = $("#mch").val();
                    var mcs = $("#mcs").val();
                    $.ajax({
                        type: 'POST',
                        url: 'DeleteTransOtherCondition.ashx', //GetDataSpecialDelete.ashx
                        async: true,
                        data: 'men=' + men + '&mch=' + mch + '&mcs=' + mcs,
                        success: function(rs) {
                            $('#dvContent').html(rs);
                            $('#ErrMsg').html('');
                            jQuery.unblockUI();
                            InitFixedHeader(940);

                            return false;
                        }
                    });
                }

                function SendMail() {
                    try { BlockUI(); } catch (e) { }
                    var men = $("#men").val();
                    var mch = $("#mch").val();
                    var mcs = $("#mcs").val();
                    $.ajax({
                        type: 'POST',
                        url: 'SendMailDeletedBet.ashx', //GetDataSpecialDelete.ashx
                        async: true,
                        data: '',
                        success: function (rs) {
                            $('#dvContent').html(rs);
                            $('#ErrMsg').html('');
                            jQuery.unblockUI();
                            InitFixedHeader(940);

                            return false;
                        }
                    });
                }
            </script>

        </div>
    </div>
</body>
</html>
