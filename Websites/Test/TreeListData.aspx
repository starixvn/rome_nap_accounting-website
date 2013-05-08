<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TreeListData.aspx.cs" Inherits="Test_TreeListData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../js/jquery-easyui-1.2.1/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../js/jquery-easyui-1.2.1/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-easyui-1.2.1/jquery.easyui.min.js" type="text/javascript"></script>
    <style>
         .datagrid
         {
             font-family: Tahoma,Arial,helvetica,sans-serif;
             border-color:#CCCCCC !important;
             
             }   
         .datagrid-footer
        {
            border-top:solid 1px #CCCCCC;
            border-bottom:none;
            }    
         .datagrid td span
         {
                font-size:11px !important; 
             }
         .datagrid-body td 
         {
             font-size:11px !important; 
             }        
         .datagrid-header-inner div.datagrid-cell
         {
             text-align:center !important;
             font-weight:bold;
             background-color:#B5C4D0  !important;
             }
         .datagrid-footer .datagrid-header-inner div.datagrid-cell
         {
             text-align:right !important;
             
             }          
    </style>
    <script>
        $(function () {
            $.ajax({
                type: "POST",
                url: "../Services/WCF/Report/ServiceReportNumberGame.svc/WinLoss_ByMatch_NumberGame",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"date":"' + '12/12/2010' + '"}',
                success: function (rs) {
                    $('#test').treegrid({
                        width: 828,
                        height: 600,
                        nowrap: false,
                        enableSort: false,
                        showIcon: false,
                        collapsible: true,
                        url: '',
                        idField: 'Id',
                        treeField: 'Name',
                        columns: [[
                            { title: 'Match - Bet Type', field: 'Name', width: 160, formatter: function (value, row, index) {
                                var id = row.Id.split("_");
                                if (id.length < 3) {
                                    return '<b>' + value + '</b>';
                                }
                                else {
                                    return '<span class="canchange" onclick="viewdetail();">' + value + '</span>';
                                }
                            }
                            },
					        { field: 'Winlost', title: 'Winlost', width: 80, align: 'right', formatter: function (value, row, index) {
					            return formatContent(value, row, index);
					        } 
					        },
                            { field: 'Position', title: 'Position', width: 80, align: 'right', formatter: function (value, row, index) {
                                return formatContent(value, row, index);
                            } 
                            },
                            { field: 'Commission', title: 'Commission', width: 80, align: 'right', formatter: function (value, row, index) {
                                return formatContent(value, row, index);
                            } 
                            },
                            { field: 'LineTotal', title: 'Line Total', width: 80, align: 'right', formatter: function (value, row, index) {
                                return formatContent(value, row, index);
                            } 
                            },
                            { field: 'VirtualLineTotal', title: 'Virtual Total', width: 80, align: 'right', formatter: function (value, row, index) {
                                return formatContent(value, row, index);
                            } 
                            },
                            { field: 'Variance', title: 'Variance', width: 80, align: 'right', formatter: function (value, row, index) {
                                return formatContent(value, row, index);
                            } 
                            },
                            { field: 'TurnOver', title: 'TurnOver', width: 80, align: 'right', formatter: function (value, row, index) {
                                return formatContent(value, row, index);
                            } 
                            },
                            { field: 'Margin', title: 'Margin', width: 80, align: 'right', formatter: function (value, row, index) {
                                return formatContent(value, row, index);
                            } 
                            }
				        ]],
                        onLoadSuccess: function (data) {
                            var footer = $(".datagrid-view2 .datagrid-header").clone().addClass("datagrid-footer");
                            footer.insertAfter($(".datagrid-view2 .datagrid-body"));
                            footer.find('td[field="Name"] div').html('<span>' + rs.totalrow.Name + '</span>');
                            footer.find('td[field="Winlost"] div').html('<span>' + rs.totalrow.Winlost + '</span>');
                            footer.find('td[field="Position"] div').html('<span>' + rs.totalrow.Position + '</span>');
                            footer.find('td[field="Commission"] div').html('<span>' + rs.totalrow.Commission + '</span>');
                            footer.find('td[field="LineTotal"] div').html('<span>' + rs.totalrow.LineTotal + '</span>');
                            footer.find('td[field="VirtualLineTotal"] div').html('<span>' + rs.totalrow.VirtualLineTotal + '</span>');
                            footer.find('td[field="Variance"] div').html('<span>' + rs.totalrow.Variance + '</span>');
                            footer.find('td[field="TurnOver"] div').html('<span>' + rs.totalrow.TurnOver + '</span>');
                            footer.find('td[field="Margin"] div').html('<span>' + rs.totalrow.Margin + '</span>');
                            setTimeout('resizeGrid()', 500);
                        }
                    });
                    $('#test').treegrid('loadData', rs.rows);

                }
            });

        });

        function formatContent(value, row, index) {
            var _value = "";
            if (parseInt(value) > 0) {
                _value = value;
            }
            else {
                _value = '<font style="color:red;">' + value + '</font>';
            }

            var id = row.Id.split("_");
            if (id.length < 3) {
                return '<b>' + _value + '</b>';
            }
            else {
                return _value;
            }
        }

        function resizeGrid() {
            var newheight = $(".datagrid-view").height();
            $(".datagrid-wrap").css("height", newheight + "px");
            $(".datagrid").css("height", (newheight +2) + "px");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table id="test"></table>
    </div>
    </form>
</body>
</html>
