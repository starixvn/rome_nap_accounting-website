<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckRedis.aspx.cs" Inherits="CheckRedis" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="txtKey">Key: </label><input id="txtKey" runat="server" type="text" />
        </div>
        <div>
            <label for="txtValue">Value: </label><input id="txtValue" runat="server" type="text" />
        </div>
        <div>
            <input id="actionType" type="hidden" runat="server" runat="server" value="Get" />
            <input id="btnGet" type="submit" runat="server" value="Get" />
            <input id="btnSet" type="submit" runat="server" value="Set" />
        </div>
    </form>
</body>
    <script>
        document.getElementById('btnGet').addEventListener('click', function () {
            var actionType = document.getElementById('actionType');
            actionType.value = 'get';
        });
        document.getElementById('btnSet').addEventListener('click', function () {
            var actionType = document.getElementById('actionType');
            actionType.value = 'set';
        });
    </script>
</html>
