let form;
const fileSelector = "#USDTExcelFile";
const choseFileMsg = "containerMsg1";
const submitMsg = "submitMsg";
const mainContent = "main_content";
const calendarInput = "EffectiveFromDate";
const btnSubmit = "btnSubmitUSDTFile";
const btnOpenFile = "btnOpenFile";
const billingCircle = "billingCircle";
const conditionSubmit = {
    "RequiredEffectiveField": {message: "Effective from is required field.", isActive: false},
    "InvalidEffectiveField": {message: "Effective from is invalid.", isActive: false},
    "InvalidFile": {message: "All errors must be fixed before uploading USDT rate.", isActive: false},
    "Another": {message: "Something went wrong, please try again.", isActive: false}
}

const ImportExRate = {
    ValidateFile() {
        const filename = $(fileSelector).val();
        const errorMess = [];

        if (filename === '' || filename === undefined) {
            return;
        }

        let fileExtension = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
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

    ResetValidateEffectFromDate(){
        conditionSubmit["InvalidFile"].isActive = false;
        conditionSubmit["Another"].isActive = false;
        conditionSubmit["RequiredEffectiveField"].isActive = false;
        conditionSubmit["InvalidEffectiveField"].isActive = false;
    },
    
    OpenUSDTRateExcel() {
        ImportExRate.HideElementById(submitMsg);
        ImportExRate.ResetValidateEffectFromDate();
        
        if (!ImportExRate.ValidateFile()) {
            return;
        }

        parent.BlockUI();
        form = $('#mainForm');
        let formData = new FormData(form[0]);
        $.ajax({
            url: 'OpenUSDTRateExcel',
            type: 'POST',
            datatype: 'json',
            contentType: false,
            processData: false,
            enctype: 'multipart/form-data',
            data: formData,
            success: function (data) {
                const {Items, Result, CompareBillingCycleResult} = data;
                ImportExRate.SetupCalendar(CompareBillingCycleResult);
                if (Items) {
                    ImportExRate.ShowElementById(mainContent);
                    ImportExRate.RenderMapping(Items);
                    const isHaveInvalidRow = Items.some(e => e.RemarkErrors.length > 0);
                    if (isHaveInvalidRow) {
                        conditionSubmit["InvalidFile"].isActive = true
                        ImportExRate.ClearFile();
                    }
                }

                if (Result.ErrorCode && Result.ErrorMessage) {
                    ImportExRate.ShowElementById(choseFileMsg);
                    ImportExRate.HideElementById(mainContent);
                    showAlertMessage(choseFileMsg, Result.ErrorMessage, 1);
                    conditionSubmit["Another"].isActive = true
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

    Submit: function () {
        const effectFrom = ImportExRate.GetEffectFromDate($("#" + calendarInput).MonthPicker('GetSelectedDate'));
        
        const validateMessage = Object.values(conditionSubmit).map((e) => { if (e.isActive) return e.message;}).filter(e => e);
        if (validateMessage.length) {
            ImportExRate.ShowElementById(submitMsg)
            showAlertMessage(submitMsg, validateMessage.join("</br>"), 1);
            return;
        }
        const data = {effectFrom};
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
                showAlertMessage(submitMsg, 'Internal Server Error.', 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    },

    RenderMapping(data) {
        let bodyRows = [];
        for (const element of data) {
            let row = {Columns: []};
            const {Currency, USDTValue, RemarkErrors, MultipliedBy, USDTAfterMultiply} = element;
            const remarkErrorFormatted = `<span class="text-red">${RemarkErrors.join(", ")}</span>`
            row.Columns.push({Name: 'currency', Value: Currency});
            row.Columns.push({Name: 'usdtUnit', Value: USDTValue});
            row.Columns.push({Name: 'multipliedBy', Value: MultipliedBy});
            row.Columns.push({Name: 'usdtAfterMultiply', Value: USDTAfterMultiply});
            row.Columns.push({Name: 'remarks', Value: remarkErrorFormatted});

            bodyRows.push(row);
        }

        let gridOptions = {
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
                    Width: 230,
                    Name: "usdtUnit",
                    HeaderAlign: "center",
                    CellAlign: "right"
                },
                {
                    HeaderText: "Multiplied By",
                    Width: 150,
                    Name: "multipliedBy",
                    HeaderAlign: "center",
                    CellAlign: "right"
                },
                {
                    HeaderText: "USDT/Unit\n(after Multiplied By)",
                    Width: 230,
                    Name: "usdtAfterMultiply",
                    HeaderAlign: "center",
                    CellAlign: "right"
                },
                {
                    HeaderText: "Remark",
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
        let today = new Date();
        let year = today.getFullYear();
        let checkTimeWithoutTZ = new Date(year, today.getMonth(), 5, 17, 0, 0, 0);
        checkTime = new Date(checkTimeWithoutTZ.getTime() - (480 + today.getTimezoneOffset()) * 60 * 1000);
        if (+checkTime > +today) {
            checkTime = new Date(checkTime.getFullYear(), checkTime.getMonth() - 1, checkTime.getDate(), checkTime.getHours(), 0, 0);
        }

        let monthCircle = checkTime.getMonth();

        let startDate = new Date(year, monthCircle, 1);
        let endDate = new Date(year, monthCircle + 1, 0);

        return {
            startDate:startDate,
            endDate:endDate,
            billingCircle: startDate.toLocaleDateString('en-US', {dateStyle: 'medium'}) + ' - ' + endDate.toLocaleDateString('en-US', {dateStyle: 'medium'})
        };
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

    InitCalendar(id, minSelectedMonth, maxSelectedMonth) {
        $("#" + id).MonthPicker({
            MonthFormat: 'M yy',
            AltFormat: 'mm/01/yy',
            AltField: '#value' + id,
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
        ImportExRate.InitCalendar(calendarInput, minMonth, currentMonth);
    },

    GetEffectFromDate(dateString) {
        if (!dateString) {
            conditionSubmit["RequiredEffectiveField"].isActive = "";
            return;
        }
        const date = new Date(dateString);
        const fromDate = new Date(date.getFullYear(), date.getMonth(), 1);
        
        const {startDate,endDate} =ImportExRate.GetBillingCircle();
        if (fromDate > endDate  || fromDate < startDate) {
            conditionSubmit["InvalidEffectiveField"].isActive = true;
        }
        else {
            conditionSubmit["InvalidEffectiveField"].isActive = false;
        }
        
        return fromDate.toLocaleDateString('en-US', {month: '2-digit', day: '2-digit', year: 'numeric'});
    },

};

$(document).ready(function () {
    $("#" + btnOpenFile).click(function (e) {
        e.preventDefault();
        ImportExRate.OpenUSDTRateExcel();
    });
    $("#" + btnSubmit).click(function (e) {
        e.preventDefault();
        ImportExRate.Submit();
    });
    $("#" + billingCircle).html(ImportExRate.GetBillingCircle().billingCircle);
}); 