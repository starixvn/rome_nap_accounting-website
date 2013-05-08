<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calendar.aspx.cs" Inherits="Test_Calendar" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <%--<link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />--%>
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="js/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />
    
    <%--<script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>--%>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    
    <script src="js/calendar.js" type="text/javascript"></script>
    <script src="js/Core.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            Core.registerWeekPicker();

            
            Core.setFromToDateConstraint();
            Core.setWeekRangeConstraintLimit2Months();

            

            $('#Week').weekpicker('setDate', Core.maxDate);
            $('#ToWeek').weekpicker('setDate', Core.maxDate);
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:50px; margin-left:100px;">
        <b>FROM </b> <input class="txtdate" id="txtfromdate" readonly="readonly" />
        <b>TO </b> <input class="txtdate" id="txttodate" readonly="readonly" />
    </div>
    <p></p>
    <div>
        From <input id="Week" class="weekpicker lfloat txtdate" type="text" name="Week" readonly="readonly" data-maxdate="2012/12/14" data-mindate="2012/01/01" data-value="2012/12/14 3:36:44 AM"/>

        To <input id="ToWeek" class="weekpicker lfloat txtdate" type="text"  name="Week" readonly="readonly" data-maxdate="2012/12/14" data-mindate="2012/01/01" data-value="2012/12/14 3:36:44 AM" />
        
    </div>
    
    </form>
</body>
</html>
