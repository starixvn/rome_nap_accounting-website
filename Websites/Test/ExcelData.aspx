<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcelData.aspx.cs" Inherits="ExcelData" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    
    
    <script src="../js/jQuery/Tpl/jquery.tmpl.js" type="text/javascript"></script>

   
    
</head>
<body>
    
    <script id="movieTemplate" type="text/x-jquery-tmpl"> 
    <li><b>${Name}</b> (${ReleaseYear})</li>
    </script>
    <ul id="movieList">
    </ul>
    <script>
        var movies = [
            { Name: "The Red Violin", ReleaseYear: "1998" },
            { Name: "Eyes Wide Shut", ReleaseYear: "1999" },
            { Name: "The Inheritance", ReleaseYear: "1976" }
        ];

        /* Render the template with the movies data and insert
        the rendered HTML under the "movieList" element */
        $("#movieTemplate").tmpl(movies).appendTo("#movieList");
    </script>
</body>
</html>
