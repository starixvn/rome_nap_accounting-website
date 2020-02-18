<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Footer.aspx.cs" Inherits="_Footer" %>
<%@ Import Namespace="System.Configuration" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Admin Site</title>
    <style type="text/css">
        div.env
        {
            float: left;
            color: #ffffff;
            font-family: Tahoma;
            font-size: 12px;
            text-align: left;
            padding-left: 6px;
            vertical-align: bottom;
            text-transform: uppercase;
            font-weight:bold;
        }

        .dev {
            background:#3d758a;
        }
        .sit {
            background:#3d8a3f;
        }
        .uat {
            background:#f48320;
        }
        .mnl {
            background:#e5101d;
        }

        div.copyright
        {
            float: right;
            color: #434343;
            font-family: Tahoma;
            font-size: 11px;
            text-align: right;
            padding-right: 6px;
            vertical-align: bottom;
        }       
    </style>
</head>
<body style="margin: 0px" class="<%=Env%>">    
    <%=EnvTheme%>
    <div class="copyright" >
       &nbsp; Copyright &copy;2010
    </div>
</body>
</html>
