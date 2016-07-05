<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Display.aspx.cs" Inherits="LogDisplay_SettingPT_Display" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />

    <title>View log</title>
    <link href="../../App_Themes/Log/ViewLog.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Log/Tab.css?v=<%= Sunplus.Accounting.Core.Configurations.WebSettings.Version %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/autocomplete/styles.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/autocomplete/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/Calendar2/js/jscal2.js"></script>
    <script type="text/javascript" src="../../js/Calendar2/js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../js/Calendar2/css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../js/Calendar2/css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../js/Calendar2/css/blue/blue.css" />
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/Common/Functions.js" type="text/javascript"></script>
    <script src="Scripts/LogSetting.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">
        function getQuerystring(key, default_) {
            if (default_ == null) default_ = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)", "i");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return default_;
            else
                return qs[1];
        }
    </script>
    <style type="text/css">
        .header {float: left; width: 985px; border: 1px solid #7C98A4; background-color: #B5C4D0; border-right:none;}
        .header1249 {width: 1142px; border: 1px solid #7C98A4;}
        .header div {float:left; border-right: 1px solid #7C98A4; padding: 3px 0; font-weight: bold; text-align:center;}
        .row {float: left; width: 983px; border: 1px solid #7C98A4; border-top: none;}
        .row1249 {width: 1143px;}
        .row div {float: left; border-right: 1px solid #7C98A4; padding: 3px 0; text-align:center; overflow:hidden;word-wrap: break-word;}
        .w100h {width: 78px; white-space:normal; overflow:hidden}
        .w101h {width: 78px; white-space:normal; overflow:hidden}
        .w150h {width: 100px; white-space:normal; overflow:hidden}
        .w180h {width: 130px; white-space:normal; overflow:hidden}
        .w180h_credit {width: 178px; white-space:normal; overflow:hidden;word-wrap: break-word;}
        .w180h_login {width: 157px; white-space:normal; overflow:hidden;word-wrap: break-word;}
        .w200h {width: 220px; white-space:normal; overflow:hidden}
        .w200h_credit {width: 180px; white-space:normal; overflow:hidden}
        .w400h {width: 480px; white-space:normal; overflow:hidden }
        .w100r {width: 70px; white-space:normal; overflow:hidden;word-wrap: break-word}
        .w150r {width: 90px; white-space:normal; overflow:hidden}
        .w150l {width: 117px; white-space:normal; overflow:hidden}
        .w110 {width: 101px; white-space:normal; overflow:hidden}
        .w200r {width: 200px; white-space:normal; overflow:hidden;word-wrap: break-word;}
        .w400r {width: 479px; white-space:normal; overflow:hidden;}
        .w300r {width: 300px; white-space:normal; overflow:hidden;}
        .w200c {width: 187px; overflow:hidden}
        .w200c_credit {width: 180px; overflow:hidden}
        .w200c_desc {width: 247px; overflow:hidden}
        .h35r {height: 35px;}
        .h40r {height: 70px;}
        .left {text-align:left!important}
        .detail {padding: 1px 2px!important;}
        .noborder {border: none!important;}
        .endrow {border-bottom: none!important;}
        .collabel {background-color: #EAEAEA;}
        .colW1sp {text-transform: capitalize; width: 68px; text-align: left; word-wrap: break-word; overflow:hidden}
        .colW2sp {text-transform: capitalize; width: 70px; overflow: hidden; word-wrap: break-word; text-align: center}
        /** diff tooltip **/
        .diffTip { position: absolute; display: none; padding: 2px 3px; border: 1px solid #cc9; background-color: #ffa;-moz-box-shadow: 2px 2px 11px #666; -webkit-box-shadow: 2px 2px 11px #666; color: Green;}
        .diffPopup { position: absolute; display: none; padding: 2px 3px; border: 1px solid #cc9; background-color: #fff;-moz-box-shadow: 2px 2px 11px #666; -webkit-box-shadow: 2px 2px 11px #666;}
        .diffhead { background-color: #B5C4D0; text-align: left; font-weight: bold; text-transform: capitalize; padding-left: 3px; cursor: move;}
        .diffhead .close { float: right; width: 16px; background: url(../../App_Themes/Images/close_16_16.gif) no-repeat scroll 0 0 transparent;cursor: pointer;height: 16px; display: inline; }
    </style>
</head>
<body>
    <script type="text/javascript">
        try { parent.jQuery.unblockUI(); } catch (e) { }
    </script>
    <script id="tpl_sp" type="text/html">
        <div class="header">
            <div class="w100h">Log ID</div>
            <div class="w150h">Log Date</div>
            <div class="w101h">Edited by</div>
            <div class="w200h">Action</div>
            <div class="w400h noborder">Detail</div>
        </div>
        {{tmpl "#tpl_record_sp"}}
    </script>
    <script id="tpl_record_sp" type="text/html">
        {{if Rows}}
        <div id="log_scroll" maxheight="600" style="max-height: 650px; overflow-x:hidden; overflow-y:scroll; width:985px;">
            <table cellspacing="0" cellpadding="0" class="tblcollapse" id="tbl_content" style="width:967px">
                <tbody>
                {{each Rows}}
                <tr>
	                <td valign="top"><div class="w100r">${ID}</div></td>
	                <td valign="top"><div class="w150r">${Date}</div></td>
	                <td valign="top"><div class="w100r">${UEdit}<br/>(${UEId})</div></td>
	                <td valign="top"><div class="w200r left">${Act}<br/>(${$value.Detail.SP})</div></td>
	                <td valign="top" align="right" style="width:480px;"><div class="w400r detail left">${LogPT.showTracking($value.Detail, ID, false, Act)}</div></td>
                </tr>
                {{/each}}
                </tbody>
            </table>
        </div>
        {{else}}
        <div id="log_scroll" maxheight="600" style="max-height: 650px; overflow-x:hidden; overflow-y:scroll; width:985px;border-left: 1px solid #7C98A4;">
            <p align="center">There is no information available.</p>
        </div>
        {{/if}}
    </script>
    <script id="tpl_credit" type="text/html">
        <div class="header" style="border-bottom:1px solid #7C98A4;">
            <div class="w100h">Log ID</div>
            <div class="w100h">Log Date</div>
            <div class="w100h">Edited By</div>
            <div class="w180h_credit">Action</div>
            <div class="w180h">Old Value</div>
            <div class="w180h">New Value</div>
            <div class="w150h">IP</div>
            <div class="w200h_credit  noborder">Detail</div>
        </div>
        {{tmpl "#tpl_record_credit"}}
    </script>
    <script id="tpl_record_credit" type="text/html">
        {{if Rows}}
        <div id="log_scroll" maxheight="600" style="max-height: 650px; overflow-x:hidden; overflow-y:scroll; width:985px;">
        {{each Rows}}
        <div class="row">
	        <div class="w100h h35r">${ID}</div>
	        <div class="w100h h35r">${Date}</div>
	        <div class="w100h h35r">${UEdit} <br/> (${UEId})</div>
	        <div class="w180h_credit h35r left">${Act} <br/>(${SP})</div>
	        <div class="w180h h35r">${OldVal}</div>
            <div class="w180h h35r">${NewVal}</div>
            <div class="w150h h35r">{{html IP}}</div>
            <div class="w200c h35r left">${Detail}</div>
        </div>
        {{/each}}
        </div>
        {{else}}
        <div id="log_scroll" maxheight="600" style="max-height: 600px; overflow-x:hidden; overflow-y:scroll; width:985px;border-left: 1px solid #7C98A4;">
        <p align="center">There is no information available.</p>
        </div>
        {{/if}}
    </script>
    <script id="tpl_login" type="text/html">
         <div class="header" style="border-bottom:1px solid #7C98A4;">
            <div class="w100h">Log ID</div>
            <div class="w100h">Log Date</div>
            <div class="w180h_credit">Action</div>
            <div class="w200c_desc">Description</div>
            <div class="w150h">Domain</div>
            <div class="w150h">Browser</div>
            <div class="w100h">Country</div>
            <div class="w150h noborder">IP</div>
        </div>
        {{tmpl "#tpl_record_login"}}
    </script>
    <script id="tpl_record_login" type="text/html">
        {{if Rows}}
        <div id="log_scroll" maxheight="900" style="max-height: 650px; overflow-x:hidden; overflow-y:scroll; width:985px;">
        {{each Rows}}
        <div class="row">
	        <div class="w100h h40r">${ID}</div>
	        <div class="w100h h40r">${Date}</div>
	        <div class="w180h_credit h40r left">${Act} <br/> (${SP})</div>
            <div class="w200c_desc h40r">${Desc}</div>
            <div class="w150h h40r">${Domain}</div>
            <div class="w150h h40r">${Browser}</div>
            <div class="w100h h40r">${Country}</div>
            <div class="w150h h40r">{{html IP}}</div>
        </div>
        {{/each}}
        </div>
        {{else}}
        <div id="log_scroll" maxheight="600" style="max-height: 650px; overflow-x:hidden; overflow-y:scroll; width:985px;border-left: 1px solid #7C98A4;">
        <p align="center">There is no information available.</p>
        </div>
        {{/if}}
    </script>
    <script id="tpl_diff_popup" type="text/html">
        <table cellspacing="0" cellpadding="0" class="tblcollapse" width="100%">
            <tbody>
            <tr>
	            <td>
                    <div id="diffhead" class="diffhead">View old values&nbsp;&nbsp;-&nbsp;&nbsp;Log ID:&nbsp;${ID}<a id="diffclose" class="close">&nbsp;</a></div>
                </td>
            </tr>
            <tr>
	            <td>
                    <div class="w400r detail">${LogPT.showTracking(tracking, ID, true, Act)}</div>
                </td>
            </tr>
            </tbody>
        </table>
    </script>
    <div class="main">
        <div class="maincontent">
            <form name="frmLog" action="Display.aspx" method="get" enctype="multipart/form-data">
                <div id="maincontent_box" class="maincontent_box" style="width:982px;">
                    <div class="box_header">
                        <div class="left_tab">
                            <ul>
                                <li id="setting" class="tablog">
                                    <a href="javascript:;" onclick="LogPT.redirect(this);"><span>Customer Setting </span></a></li>
                                <li id="status" class="tablog">
                                    <a href="javascript:;" onclick="LogPT.redirect(this);"><span>Status </span></a>
                                </li>
                                <li id="credit" class="tablog">
                                    <a href="javascript:;" onclick="LogPT.redirect(this);"><span>Credit </span></a>
                                </li>
                                <li id="login" class="tablog">
                                    <a href="javascript:;" onclick="LogPT.redirect(this);"><span>Login </span></a>
                                 </li>
                            </ul>
                        </div>
                    </div>
                    <div style="border-top: 0px; padding: 7px 7px 7px 7px;" id="box_header">
                        User Name:
                        <input type="text" style="width: 135px" id="custName" name="custName" class="textbox" autocomplete="off" />
                        <span class="required" id="RequiredUserName" style="display: none;">*</span> &nbsp;&nbsp;
                        From:
                        <input type="text" style="width: 70px" readonly="readonly" id="fromDate" name="fromDate" class="date_no" />
                        <a class="imaLink" href="#">
                            <img border="0" src="../../App_Themes/images/calendar.gif" class="img-date-selector"
                                title="Date selector" id="imgCalFrom" alt="" />
                        </a>&nbsp;&nbsp; To:
                        <input type="text" style="width: 70px" readonly="readonly" id="toDate" name="toDate" class="date_no" />
                        <a class="imaLink" href="#">
                            <img border="0" src="../../App_Themes/images/calendar.gif" class="img-date-selector"
                                title="Date selector" id="imgCalTo" alt="" />
                        </a>&nbsp;&nbsp; <span style="display: none;" id="log_type">View by:
                            <select onchange="LogPT.doSearch(1);" id="viewBy" name="viewBy">
                                <option value="-1">All</option>
                                <option value="1">Position Taking</option>
                                <option value="2">Commission</option>
                                <option value="3">Bet Setting</option>
                                <option value="4">General Info</option>
                            </select>
                        </span>
                        <input type="hidden" name="tabid" id="tabid" value="" />
                        <input type="button" class="btn" id="btnSearch" value="Search" />
                        <img style="display: none;" alt="" src="../../App_Themes/images/waiting7.gif" id="displayBox" />
                        <div style="padding-top: 5px; display: none;" id="lnkOld">
                            <span style="color: #90979A; font-weight: bold;">* For data before 01-Jan-2009, please
                                <a href="#" onclick="OldForm()" class="link">click here</a></span>
                        </div>
                        <div style="display: none; padding-top: 5px" id="loginMsg">
                            <span style="color: #90979A; font-weight: bold;">* The Login Log data is only available
                                within 90 days and is not applicable for member level</span>
                        </div>
                    </div>
                </div>
                <div id="log_content" style="float: left; width: 986px; margin-top: 7px;border-right: 1px solid #7C98A4;border-bottom: 1px solid #7C98A4; display:none;"></div>
                <div id="pagination" class="bg_table" style="float: left; width: 982px; display: none;"></div>
            </form>
        </div>
    </div>
    
    <script type="text/javascript" language="javascript">
        var custId = <%=custId%>;
        $(document).ready(function () {

        // set view name
            var curTab = getQuerystring('tabid', 'setting');
            LogPT.setTab(curTab);

            // set view name
            var custName = decodeURI(getQuerystring('custName'));
            $('#custName').val(custName);

            // set view by
            var viewBy = getQuerystring('viewBy');
            $('#viewBy').val(viewBy);

            // set default datetime    
            var fromDate = getQuerystring('fromDate');
            if (fromDate.length > 0) {
                $('#fromDate').val(unescape(getQuerystring('fromDate')));
            }
            else {
                $('#fromDate').val('<%=DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy")%>');
            }

            var toDate = getQuerystring('toDate');
            if (toDate.length > 0) {
                $('#toDate').val(unescape(getQuerystring('toDate')));
            }
            else {
                $('#toDate').val('<%=DateTime.Now.ToString("MM/dd/yyyy")%>');
            }

            //setup autocomplete
            var serviceUrl = "../../Handlers/QueryUsername.ashx";

            $('input#custName').autocomplete( { serviceUrl: serviceUrl, minChars: 2 } );
            $('.autocomplete').mouseover(function () {
                isonauto = true;
            }).mouseout(function () {
                isonauto = false;
            });

            $('#btnSearch').bind('click', function(e) { LogPT.doSearch(1); });

            $('#custName').bind('keypress', function(e) {
                var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                var target = e.target.tagName.toLowerCase();
                if (key === 13 && target === 'input') {
                    e.preventDefault();
                    LogPT.doSearch(1);
                }
            });
            // set page
            var page = getQuerystring('page');
            page = parseInt(page) > 0 ? page : 1;

            // calendar setup
            Calendar.setup({
                animation: false,
                showTime: false,
                min: 20090101,
                trigger: "imgCalFrom",
                inputField: "fromDate",
                dateFormat: "%m/%d/%Y",
                onSelect: function () {
                    this.hide();
                }
            });

            Calendar.setup({
                animation: false,
                showTime: false,
                min: 20090101,
                trigger: "imgCalTo",
                inputField: "toDate",
                dateFormat: "%m/%d/%Y",
                onSelect: function () {

                    this.hide();
                }
            });

            LogPT.doSearch(page);
            // check crossbrowser
            if($.browser.safari) {
                $('#maincontent_box').css({width: 987});
                $('#pagination').css({width: 987});
            }
            if($.browser.msie) {
                $('#maincontent_box').css({width: 981});
                $('#pagination').css({width: 981});
            }
        });
    </script>
</body>
</html>
