var form;
const fileSelector = "#USDTExcelFile";
const choseFileMsg = "containerMsg1";
const submitMsg = "submitMsg";
const mainContent = "main_content";
const calendarInput = "EffectiveFromDate";
const btnSubmit = "btnSubmitUSDTFile";
const btnOpenFile = "btnOpenFile";
const billingCircle = "billingCircle";

const ImportExRate = {
    ValidateFile() {
        const filename = $(fileSelector).val();
        const errorMess = [];

        if (filename === '' || filename === undefined) {
            return;
        }

        var fileExtension = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
        if (!["xls", "xlsx"].includes(fileExtension)) {
            const invalidExtensionMess = "Sorry, You choose invalid file extension. Please choose an Excel file (.xls or .xlsx)";
            errorMess.push(invalidExtensionMess);
        }

        if (ImportExRate.GetFileSize(fileSelector) > 1024000) {
            const invalidFileSizeMess = "Your uploaded file's size can't be larger than 1 MB";
            errorMess.push(invalidFileSizeMess);
        }

        if (errorMess.length) {
            ImportExRate.ClearFile();
            ImportExRate.ShowElementById(choseFileMsg);
            showAlertMessage(choseFileMsg, errorMess.join("</br> "), 1);
            return false;
        } else {
            ImportExRate.HideElementById(choseFileMsg);
            ImportExRate.HideElementById(mainContent);
            return true;
        }
    },
    
    GetFileSize(eName) {
        return parseFloat($(eName).get(0).files[0].size);
    },

    ShowElementById(id) {
        $("#" + id).show();
    },
    
    HideElementById(id) {
        $("#" + id).hide();
    },

    ClearFile() {
        $(fileSelector).val("");
    },
    
    OpenUSDTRateExcel() {
        if (!ImportExRate.ValidateFile()) {

            return;
        }

        parent.BlockUI();
        form = $('#mainForm');
        var formData = new FormData(form[0]);
        $.ajaxFileUpload({
            type: 'POST',
            url: 'OpenUSDTRateExcel',
            secureuri: false,
            fileElementId: 'USDTExcelFile',
            dataType: 'jsonResult',
            contentType: false,
            processData: false,
            enctype: 'multipart/form-data',
            data: formData,
            success: function (data) {
                const {FileData, Result, CompareBillingCycleResult} = data;

                ImportExRate.SetupCalendar(CompareBillingCycleResult);

                if (FileData) {
                    ImportExRate.ShowElementById(mainContent);
                    ImportExRate.RenderMapping(FileData);
                    const isHaveInvalidRow = FileData.some(e => e.RemarkErrors.length > 0);
                    !isHaveInvalidRow && $("#" + btnSubmit).removeAttr("disabled");
                    isHaveInvalidRow && ImportExRate.ClearFile();
                }

                if (Result.ErrorCode && Result.ErrorMessage) {
                    ImportExRate.ShowElementById(choseFileMsg);
                    ImportExRate.HideElementById(mainContent);
                    showAlertMessage(choseFileMsg, Result.ErrorMessage, 1);
                    $("#" + btnSubmit).attr("disabled", "true")
                }
            },
            error: function (xhr) {
                showAlertMessage(choseFileMsg, 'Internal Server Error.', 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    },

    Submit() {
        const data = {effectFrom: ImportExRate.GetEffectFromDate($("#" + calendarInput).MonthPicker('GetSelectedDate'))};
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "UpdateUSDTCurrencyCode",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (data) {
                if (data.ErrorCode === 0) {
                    ImportExRate.ShowElementById(submitMsg)
                    showAlertMessage(submitMsg, "File uploaded successfully.", 0);
                    setTimeout(function () {
                        parent.gotoURL('centerFr', 'ExchangeRate/ImportUSDTRateView');
                    }, 3000);
                } else {
                    ImportExRate.ShowElementById(submitMsg)
                    showAlertMessage(submitMsg, data.ErrorMessage, 1);
                }
            },
            error: function (xhr) {
                ImportExRate.ShowElementById(submitMsg)
                showAlertMessage('submitMsg', 'Internal Server Error.', 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    },
    
    RenderMapping(data) {
        var bodyRows = [];
        for (var i = 0; i < data.length; i++) {
            var row = {Columns: []};
            const {CurrencyCode, USDTValue, RemarkErrors} = data[i];
            const remarkErrorFormatted = `<span class="text-red">${RemarkErrors.join(", ")}</span>`
            row.Columns.push({Name: 'currency', Value: CurrencyCode});
            row.Columns.push({Name: 'usdtunit', Value: USDTValue});
            row.Columns.push({Name: 'remarks', Value: remarkErrorFormatted});

            bodyRows.push(row);
        }

        var gridOptions = {
            columns: [
                {
                    HeaderText: "Currency",
                    Width: 100,
                    Name: "currency",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                {
                    HeaderText: "USDT/Unit",
                    Width: 300,
                    Name: "usdtunit",
                    HeaderAlign: "center",
                    CellAlign: "right"
                },
                {
                    HeaderText: "Remarks",
                    Width: 360,
                    Name: "remarks",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
            ],
            headerColumnHeight: 30,
            bodyRowHeight: 20,
            bodyRows: bodyRows,
            gridExpandHeight: 200,
            showDistinctRow: false
        };
        $('#importUSTDGrid').removeClass("hidden").fanexGrid(gridOptions);
    },

    GetBillingCircle() {
        var today = new Date();
        var year = today.getFullYear();
        var checkTimeWithoutTZ = new Date(year, today.getMonth(), 5, 17, 0, 0, 0);
        checkTime = new Date(checkTimeWithoutTZ.getTime() - (480 + today.getTimezoneOffset()) * 60 * 1000);
        if (+checkTime > +today) {
            checkTime = new Date(checkTime.getFullYear(), checkTime.getMonth() - 1, checkTime.getDate(), checkTime.getHours(), 0, 0);
        }

        var monthCircle = checkTime.getMonth();

        var startDate = new Date(year, monthCircle, 1);
        var endDate = new Date(year, monthCircle + 1, 0);

        return startDate.toLocaleDateString('en-US', {dateStyle: 'medium'}) + ' - ' + endDate.toLocaleDateString('en-US', {dateStyle: 'medium'});
    },

    OpenNoteMessage() {
        $('#noteMessage').dialog({
            title: 'Info',
            modal: true,
            width: 500,
            height: 110,
            position: {
                my: "left top+10",
                at: "left+300 top"
            }
        });
    },

    GetDisplayMonthOnly(dateString) {
        const date = new Date(dateString);

        return date.toLocaleDateString('en-US', {month: 'short', year: 'numeric'});
    },
   
    InitCalendar(id, defaultSelectedMonth, minSelectedMonth, maxSelectedMonth) {
        $("#" + id).MonthPicker({
            MonthFormat: 'M yy',
            AltFormat: 'mm/01/yy',
            AltField: '#value' + id,
            SelectedMonth: defaultSelectedMonth,
            MinMonth: minSelectedMonth,
            MaxMonth: maxSelectedMonth,
            Button: '<img class="img-date-selector" alt="" id="' + id + '_Trigger" title="Month selector" width="18" height="17" src="../App_Themes/Images/calendar.gif" /> '
        })
    },

    SetupCalendar(compareBillingCycleResult = undefined) {
        let minMonth = "-0m";
        if (compareBillingCycleResult <= 0) {
            minMonth = "-1m";
        }
        const currentMonth = ImportExRate.GetDisplayMonthOnly(new Date(checkTime.getFullYear(), checkTime.getMonth(), 1));
        ImportExRate.InitCalendar(calendarInput, currentMonth, minMonth, currentMonth);
    },
    
    GetEffectFromDate(dateString) {
        const date = new Date(dateString);
        const fromDate = new Date(date.getFullYear(), date.getMonth(), 1);

        return fromDate.toLocaleDateString('en-US', {month: '2-digit', day: '2-digit', year: 'numeric'});
    },

};

$(document).ready(function () {
    $("#"+btnOpenFile).click(function (e) {
        e.preventDefault();
        ImportExRate.OpenUSDTRateExcel();
    });
    $("#"+btnSubmit).click(function (e) {
        e.preventDefault();
        ImportExRate.Submit();
    });
    $("#"+billingCircle).html(ImportExRate.GetBillingCircle());
}); 