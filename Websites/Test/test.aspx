<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" Async="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/Test1.js" type="text/javascript"></script>
    <script src="js/Test2.js" type="text/javascript"></script>
</head>
<body>
    
    <span id="console" data-logid="1495"></span>
    <span id="console2" data-logid="1111"></span>
    <span id="Span1" ></span>
    <input type="text" id="password"/>
    
    <input type="button" value="Change To Pass" onclick="ChangeToPassword()" />
    <input type="button" value="Change to Test" onclick="ChangeToText()" />
    <script type="text/javascript">



        var setPassword = function ($input) {

            var inputPassword = $('<input type="password" id="password" name="password" class="password" value="' + $input.val() +'"/>');
            $input.replaceWith(inputPassword);
            inputPassword.focus();

        };

        var resetPassword = function ($input) {

            $input.replaceWith('<input type="text" id="password" name="password" class="password" value="' + $input.val() + '"/>');
        };

        function ChangeToText() {

            
            resetPassword($('#password'));
        }

        function ChangeToPassword() {
                setPassword($('#password'));
        }
        

            
        // calls Console using the fully qualified name (fqn)
        //Sphinx.Login.log("hell");
        //Sphinx.PlaceHolder('hello world2');
    </script>
</body>
</html>
