<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormDeleteBet.aspx.cs" Inherits="SpecialDelete_NumberGame_FormDeleteBet" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../js/common.js"></script>

    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../../js/stdlib.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="js/SpecialDelete.js" type="text/javascript"></script>

    
</head>
<body>
    <div id='ErrorMessage' class='error-message'></div>
    <div>
        <table>
            <tr>
                <td>UserName:</td>
                <td><input type="text" id="username" disabled="disabled" /></td>
            </tr>
            <tr>
                <td>Password:</td>
                <td><input type="password" id="userpwd" /></td>
            </tr>
        </table>
    </div>
  <table border="0px">
    <tr>
      <th colspan="3" bgcolor="#88aaff">Please include "@@" in message content for replace ticket numbers</th>
    </tr>
    <tr bgcolor="#ADADAD" class="tableBw">
      <th  align="center">
        <span style="color:Red;">*</span>Message En
      </th>
      <th  align="center">
        <span style="color:Red;">*</span>Message Ch
      </th>
      <th align="center">Message Cs</th>
    </tr>
    <tr>
      <td>
        <textarea rows="10" id="men" name="men" cols="36"></textarea>
      </td>
      <td>
        <textarea rows="10" id="mch" name="mch" cols="36"></textarea>
      </td>
      <td>
        <textarea rows="10" id="mcs" name="mcs" cols="36"></textarea>
      </td>
    </tr>
    <tr>
      <th colspan="3" bgcolor="#88aaff">
        <a id="other_display" onclick="hideLanguage(this)" href="#">
          <span style="color: Black;">Other Language</span> [+]
        </a>
      </th>
    </tr>
    <tr bgcolor="#ADADAD" class="tableBw" id="otherLanguage_title" style="display:none;">
      <th  align="center">Message Th</th>
      <th  align="center">Message It</th>
      <th  align="center">Message Jp</th>
    </tr>
    <tr id="otherLanguage_content" style="display:none;">
      <td>
        <textarea rows="10" id="mth" name="mth" cols="36"></textarea>
      </td>
      <td>
        <textarea rows="10" id="mit" name="mit" cols="36"></textarea>
      </td>
      <td>
        <textarea rows="10" id="mjp" name="mjp" cols="36"></textarea>
      </td>
    </tr>

    <tr bgcolor="#ADADAD" class="tableBw" id="otherLanguage_title2" style="display:none;">
      <th  align="center">Message Es</th>
    </tr>
      <tr style="display: none;" id="otherLanguage_content2">
          <td>
              <textarea cols="36" name="mes" id="mes" rows="10"></textarea>
          </td>
          <td>
          </td>
          <td>
          </td>
      </tr>

  </table>
  <table border="0px" style="margin-top:6px;">
    <tr>
      <td colspan="2" class='c'>
        <input class="btn" type='submit' value='Delete' onclick='return on_delete();' />
            <input type="hidden" id="accId" name="accId" value="<%=AppSession.AccId %>" />
      </td>
    </tr>    
  </table>
    <script type="text/javascript">
        var AccUserName = '<%=Username %>'
        $('#username').val(AccUserName);
    </script>
</body>
</html>
