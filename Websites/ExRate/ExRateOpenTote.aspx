<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExRateOpenTote.aspx.cs" Inherits="ExRate_ExRateOpenTote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>   
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script src="../js/jQuery/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript">
           var ExRate;
           $(document).ready(function () {
               $.ajax({
                   type: "POST",
                   url: "Services/ServiceExchangeRate.svc/LoadExchangeRateOpenTote",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   data: '',
                   success: function (rs) {
                       LoadData(rs);
                   }
               });
           });
            function LoadData(rs)
            {
                 //alert(rs[0].Currency);
                 if (rs!=null)
                 {
                     if (rs.length>0)
                     {
                         ExRate=rs;
                         var contentwraper = $("#ExRate");
                         contentwraper.setTemplateURL("../App_Templates/Default/ExRate/ExRateOpenTote.htm");
                         contentwraper.processTemplate(rs);
                         var isValid=true;
                         for (var i=0;i<rs.length;i++)
                         {
                            if (rs[i].Result.ErrorCode != 0)
                            {
                                isValid=false;
                            }
                         }
                         if (!isValid)
                         {
                            $(".btn").attr("disabled","disabled");
                         }
                     }
                     else
                     {
                         showAlertMessage("err","Cannot load excel file.",1);
                         $(".btn").attr("disabled","disabled");
                         $("#ExRate").html("");
                     }
                     
                    
                 }
                 else
                 {
                    showAlertMessage("err","Cannot load excel file.",1);
                 }
                  try{parent.jQuery.unblockUI();}catch(e){}
            }
            function UpdateOpenToteExRate()
            {
                if (ExRate!=null)
                {
                    try{parent.BlockUI();}catch(e){}
                     $.ajax({
                            type: "POST",
                            url: "Services/ServiceExchangeRate.svc/UpdateExRateOpenTote",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data:  '{"ExRateList":' + JSON.stringify(ExRate) + '}',
                            success: function (rs) {
                                if (rs.ErrorCode>0)
                                {
                                    showAlertMessage("err",rs.ErrorMessage,1);
                                }
                                else
                                {
                                    showAlertMessage("err",rs.ErrorMessage,0);
                                    afterUpdate(true,2000);
                                }
                                try{parent.jQuery.unblockUI();}catch(e){}
                            }
                        });
                }
            }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="maincontent">
            <div class="bg_table" style="width:95%;">
              <div style="margin-bottom:6px">
                <input value="Update" onclick="UpdateOpenToteExRate()" type="button" class="btn">
              </div>  
              <div class="errMsg" id="err"></div>
              <div id="ExRate">Loading...</div> 
              <div style="margin-top:6px">
                <input value="Update" onclick="UpdateOpenToteExRate()" type="button" class="btn">
              </div>
              
            </div>
        </div>
    </div>
    </form>
</body>
</html>
