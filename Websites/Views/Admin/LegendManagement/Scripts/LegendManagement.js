$(document).ready(function () {
    legendManagement.initPage();
});

var legendManagement = {
    _tables: {
        ddlMenu: null,
        btnLoad: null,
        btnClear: null,
        btnPreview: null,
        btnSave: null,
        imageLocation: null,
        btnUploadImage: null,
        informationMssg: null,
        htmlEditor: null,
        previewMenuLegendDialog: null,
        uploadImageDialog: null,
        uploadImageTabs: null,
        uploadImageTabsContent: null,
        tabURL: null,
        tabChoose: null,
        uploadImageDialogTemplate: null,
        btnCancelUpload: null,
        upload_message: null,
        saveImagePath: window.rootWebPath + "/Views/Admin/LegendManagement/UploadedImage/"
    },
    _params: {
        saveMenuLegend: {
            MenuId: null,
            MenuLegend: ''
        },
        getMenuLegend: {
            MenuId: null,
            FunctionName: null
        }
    },
    _cacheData: {
        timeToCloseMessage: 0,
        currentUploadTab: null
    },
    initPage: function () {
        legendManagement.cachedDOMElements();
        legendManagement.getMenuList();
        legendManagement.initHTMLEditor();
        legendManagement.bindingEvent();
    },
    cachedDOMElements: function () {
        legendManagement._tables.ddlMenu = $("#ddlMenu");
        legendManagement._tables.btnLoad = $("#btnLoad");
        legendManagement._tables.btnClear = $("#btnClear");
        legendManagement._tables.btnPreview = $("#btnPreview");
        legendManagement._tables.btnSave = $("#btnSave");
        legendManagement._tables.imageLocation = $("#imageLocation");
        legendManagement._tables.btnUploadImage = $("#upload-image-link");
        legendManagement._tables.informationMssg = $("#informationMssg");
        legendManagement._tables.htmlEditor = $("#htmlEditor");
        legendManagement._tables.previewMenuLegendDialog = $("#previewMenuLegendDialog");
        legendManagement._tables.uploadImageDialog = $("#uploadImageDialog");
        legendManagement._tables.uploadImageDialogTemplate = $("#uploadImageDialogTemplate");
    },
    disableInput: function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    },
    enableInput: function () {
        try {
            parent.jQuery.unblockUI();
        } catch (e) {
        }
    },
    getMenuList: function () {
        legendManagement.disableInput();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LegendManagement/GetMenuList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                legendManagement.renderMenuList(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
                legendManagement.renderMenuList([]);
            },
            complete: function () {
                legendManagement.enableInput();
            }
        });
    },
    renderMenuList: function (nemuList) {
        var len = nemuList.length;
        var ddwnMenu = [];
        var prefixId = 'ddlMenu_';
        var options = '';
        options += '<optgroup label="Template">';
        options += '<option value="-1" functionName="[TemplateLegend]">Main Template</option>';

        for (var i = 0; i < len; i++) {
            var menu = nemuList[i];
            options += '<optgroup label="' + menu.Name + '">';
            var subMenuLen = menu.SubMenu.length;
            for (var j = 0; j < subMenuLen; j++) {
                var subMenu = menu.SubMenu[j];
                options += '<option value="' + subMenu.Id + '" functionName="' + subMenu.FunctionName + '">' + subMenu.Name + '</option>';
            }

            options += '</optgroup>';
        }

        legendManagement._tables.ddlMenu.empty().append(options);
    },
    initHTMLEditor: function () {
        try {
            legendManagement._tables.htmlEditor.tinymce({
                // Location of TinyMCE script
                script_url: window.rootUrl + 'js/tiny_mce/tiny_mce.js',

                // General options
                theme: "advanced",
                plugins: "table,save,emotions,preview,media,paste",

                // Theme options
                theme_advanced_buttons1: "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,code,preview",
                theme_advanced_buttons2: "pastetext,pasteword,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,|,upload,emotions,media,|,tablecontrols,pagebreak",
                theme_advanced_buttons3: "",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",

                //theme_advanced_statusbar_location: "bottom",
                theme_advanced_resizing: false,

                // Example content CSS
                content_css: "../Views/Admin/LegendManagement/Styles/LegendManagement.css",
                paste_auto_cleanup_on_paste: true,
                paste_remove_spans: true,
                paste_remove_styles: true,
                tab_focus: ':prev,:next',
                entity_encoding: "raw",
                invalid_elements: "script,noscript,link,title,head,body,html,frame,iframe,input,textarea,select",

                setup: function (ed) {
                    // add upload button
                    ed.addButton('upload', {
                        title: 'Upload image',
                        image: '../Views/Admin/LegendManagement/Styles/Images/image-up1.png',
                        onclick: function () {
                            legendManagement.uploadImage();
                            return;
                        }
                    });
                }
            });

            setTimeout(function () {
                tinyMCE.activeEditor.setContent('aaaaaasdasdas');
            }, 2000);
        } catch (e) {
        }
    },
    bindingEvent: function () {
        legendManagement._tables.btnLoad.click(function () {
            legendManagement.loadMenuLegend();
        });

        legendManagement._tables.btnClear.click(function () {
            legendManagement.clearMenuLegend();
        });

        legendManagement._tables.btnPreview.click(function () {
            legendManagement.previewMenuLegend();
        });

        legendManagement._tables.btnSave.click(function () {
            legendManagement.saveMenuLegend();
        });

        legendManagement._tables.btnUploadImage.click(function () {
            legendManagement.uploadImage();
        });
    },
    loadMenuLegend: function () {
        legendManagement.getGetLegendParams();
        if (legendManagement.validGetLegendParams()) {
            $.ajax({
                type: "POST",
                url: window.rootUrl + "LegendManagement/GetMenuLegend",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(legendManagement._params.getMenuLegend),
                success: function (rs) {
                    tinyMCE.activeEditor.setContent(rs);
                },
                error: function (event, jqXHR, ajaxSettings) {
                    tinyMCE.activeEditor.setContent('');
                },
                complete: function () {
                    legendManagement._tables.htmlEditor.focus();
                    legendManagement.enableInput();
                }
            });
        }
    },
    getGetLegendParams: function () {
        legendManagement._params.getMenuLegend.MenuId = parseInt(legendManagement._tables.ddlMenu.val());
        legendManagement._params.getMenuLegend.FunctionName = legendManagement._tables.ddlMenu.find("option:selected")
                                                                                              .attr("functionName");
    },
    validGetLegendParams: function () {
        legendManagement._cacheData.timeToCloseMessage = 0;
        legendManagement._tables.informationMssg.empty().addClass("hidden");
        return true;
    },
    clearMenuLegend: function () {
        tinyMCE.activeEditor.setContent('');
    },
    saveMenuLegend: function () {
        legendManagement.getSaveLegendParams();
        if (legendManagement.validSaveLegendParams()) {
            legendManagement.makeInlineImages();
        }
    },
    doSavingMenuLegend: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LegendManagement/SaveMenuLegend",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(legendManagement._params.saveMenuLegend),
            success: function () {
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
                showAlertMessage("informationMssg", "Save succeeded!", 0);
                legendManagement._tables.informationMssg.removeClass("hidden");
                legendManagement._cacheData.timeToCloseMessage = 3000;

                legendManagement.enableInput();
                setTimeout(function () {
                    if (legendManagement._cacheData.timeToCloseMessage > 0) {
                        legendManagement._tables.informationMssg.empty().addClass("hidden");
                        legendManagement._cacheData.timeToCloseMessage = 0;
                    }
                }, 3000);
            }
        });
    },
    getSaveLegendParams: function () {
        legendManagement._params.saveMenuLegend.MenuId = parseInt(legendManagement._tables.ddlMenu.val());
        legendManagement._params.saveMenuLegend.MenuLegend = $.trim(tinyMCE.activeEditor.getContent());
    },
    validSaveLegendParams: function () {
        legendManagement._cacheData.timeToCloseMessage = 0;
        if (legendManagement._params.saveMenuLegend.MenuLegend == null ||
            legendManagement._params.saveMenuLegend.MenuLegend == '') {
            legendManagement._tables.htmlEditor.focus();
            showAlertMessage("informationMssg", "Please input menu lengend content!", 1);
            legendManagement._tables.informationMssg.removeClass("hidden");
            return false;
        }

        legendManagement._tables.informationMssg.empty().addClass("hidden");
        return true;
    },
    previewMenuLegend: function () {
        var menuLegendContent = $.trim(tinyMCE.activeEditor.getContent());
        var menuName = legendManagement._tables.ddlMenu.find("option:selected").html();

        legendManagement._tables.previewMenuLegendDialog.empty().html(menuLegendContent);
        legendManagement._tables.previewMenuLegendDialog.removeClass("hidden");
        legendManagement._tables.previewMenuLegendDialog.dialog({
            width: 610,
            height: 500,
            position: "center",
            modal: true,
            title: "Preview Legend For " + menuName,
            resizable: false
        });
    },
    uploadImage: function () {
        legendManagement._tables.uploadImageDialog.empty().html(
            legendManagement._tables.uploadImageDialogTemplate.html());
        legendManagement._tables.uploadImageDialog.removeClass("hidden");
        legendManagement._tables.uploadImageDialog.dialog({
            width: 350,
            height: 300,
            position: "center",
            modal: true,
            title: "Upload Image",
            resizable: false
        });

        legendManagement._tables.uploadImageTabs = legendManagement._tables.uploadImageDialog.find("#uploadbox-tab li a");
        legendManagement._tables.uploadImageTabsContent = legendManagement._tables.uploadImageDialog.find(".chooseimg");
        legendManagement._tables.tabURL = legendManagement._tables.uploadImageDialog.find(".tab-url");
        legendManagement._tables.tabChoose = legendManagement._tables.uploadImageDialog.find(".tab-choose");
        legendManagement._cacheData.currentUploadTab = legendManagement._tables.tabURL;
        legendManagement._tables.tabURL.removeClass("hidden");
        legendManagement._tables.tabChoose.addClass("hidden");
        legendManagement._tables.btnCancelUpload = legendManagement._tables.uploadImageDialog.find("#btnCancelUpload");
        legendManagement._tables.btnUpload = legendManagement._tables.uploadImageDialog.find("#btnUpload");
        legendManagement._tables.txtfile = legendManagement._tables.uploadImageDialog.find("#txtfile");
        legendManagement._tables.txt_url = legendManagement._tables.uploadImageDialog.find("#txt_url");
        legendManagement._tables.upload_message = legendManagement._tables.uploadImageDialog.find("#upload_message");

        legendManagement._tables.uploadImageTabs.unbind("click");
        legendManagement._tables.uploadImageTabs.click(function (e) {
            legendManagement.changeUploadTab($(this));
        });

        legendManagement._tables.btnCancelUpload.unbind("click");
        legendManagement._tables.btnCancelUpload.click(function (e) {
            legendManagement.cancelUploadImage();
        });

        legendManagement._tables.btnUpload.unbind("click");
        legendManagement._tables.btnUpload.click(function (e) {
            legendManagement.doUploadImage();
        });

        legendManagement._tables.uploadImageDialog.ajaxUpload({
            ajaxFile: window.rootUrl + "LegendManagement/UploadImage",
            file_types: "jpg,jpeg,gif,png,bmp"
        });
    },
    changeUploadTab: function (uploadTab) {
        legendManagement._tables.upload_message.html('');
        legendManagement._tables.uploadImageTabs.removeClass("current");
        uploadTab.addClass("current");
        legendManagement._cacheData.currentUploadTab = legendManagement._tables.uploadImageDialog.find("." + uploadTab.attr("rel"));
        legendManagement._tables.uploadImageTabsContent.addClass("hidden");
        legendManagement._cacheData.currentUploadTab.removeClass("hidden");
    },
    cancelUploadImage: function (event) {
        legendManagement._tables.txtfile.val('');
        legendManagement._tables.txt_url.val('');
        legendManagement._tables.uploadImageDialog.dialog("close");
    },
    doUploadImage: function () {
        var tabId = legendManagement._cacheData.currentUploadTab.attr("id");
        if (tabId == 'tab-choose') {
            var fileName = legendManagement._tables.txtfile.val();
            if (!legendManagement.checkFileType(fileName)) {
                legendManagement._tables.txtfile.val('');
                legendManagement._tables.upload_message.html("<div class='alertboxerr'><div>Sorry, You have chosen an invalid file extension. Please choose an image file.</div></div>");
                legendManagement._tables.upload_message.removeClass("hidden");
                legendManagement._tables.txtfile.focus();
            } else {
                var oldElement = legendManagement._tables.txtfile;
                var newElement = $(oldElement).clone();
                $(oldElement).attr('id', 'txtfile');
                $(oldElement).attr('name', 'txtfile');
                $(oldElement).attr('class', 'clone');
                $(oldElement).appendTo('#myUploadForm');
                $('#myUploadForm').submit();
                legendManagement.cancelUploadImage();

                setTimeout(function () {
                    fileName = fileName.substring(fileName.lastIndexOf("\\") + 1, fileName.length);
                    var html = '<img src="' + (window.rootWebPath + "/Views/Admin/LegendManagement/UploadedImage/" + fileName) + '"/>';
                    tinyMCE.execCommand('mceInsertContent', false, html);
                }, 500);
            }
        } else if (tabId == 'tab-url') {
            var fileName = legendManagement._tables.txt_url.val();
            if (fileName == '') {
                legendManagement._tables.txt_url.val('');
                legendManagement._tables.upload_message.html("<div class='alertboxerr'><div>Please input image url!</div></div>");
                legendManagement._tables.upload_message.removeClass("hidden");
                legendManagement._tables.txt_url.focus();
            } else {
                fileName = fileName.substring(fileName.lastIndexOf("\\") + 1, fileName.length);
                var html = '<img src="' + fileName + '"/>';
                tinyMCE.execCommand('mceInsertContent', false, html);
                legendManagement.cancelUploadImage();
            }
        }

        return false;
    },
    checkFileType: function (fileName) {
        if (fileName == '') {
            return false;
        }

        var fileExt = fileName.toLowerCase().substr(fileName.toLowerCase().lastIndexOf('.') + 1);
        var imageExt = "jpg,jpeg,gif,png,bmp";
        if (!imageExt.match(fileExt)) {
            return false;
        }

        else return true;
    },
    makeInlineImages: function () {
        var html = $(legendManagement._params.saveMenuLegend.MenuLegend);
        var image = html.find("img");
        var imageLength = image.length;
        var data = [];
        for (var i = 0; i < imageLength; i++) {
            $(image[i]).attr("_mce_src", null);
            data.push(image[i].src);
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + "LegendManagement/MakeInlineImages",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var imageLength = rs.length;
                for (var j = 0; j < imageLength; j++) {
                    $(image[j]).attr("src", "data:image/png;base64," + rs[j]);
                }

                legendManagement._params.saveMenuLegend.MenuLegend = html.html();
                legendManagement.doSavingMenuLegend();
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
            }
        });
    }
};
