<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckEmail.aspx.cs" Inherits="CheckEmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Check Email</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="txtKey">MailTo: </label><input id="txtMailTo" runat="server" type="text" />
        </div>
        <div>
            <input id="btnSend" type="submit" runat="server" value="Send" />
        </div>
    </form>
</body>
</html>
