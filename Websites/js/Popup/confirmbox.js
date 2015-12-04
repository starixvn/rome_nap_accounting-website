function __confirm(title, message, width, height, popupId, okClickEvent, cancelClickEvent, closeClick, reloadOnClose) {
    var Form = document.createElement("div");
    var DivContent = document.createElement("div");
    var id = popupId ? popupId : 'windowPopup';

    Form.setAttribute('id', id);
    Form.setAttribute('style', 'display: none; border: none; font-size: 12px; z-index: 999999');

    DivContent.setAttribute('style', 'height: ' + (height - 125) + 'px');

    var divFrame = document.createElement("div");
    divFrame.setAttribute('style', 'height: ' + (height - 125) + 'px; margin: 2px 5px; line-height: 18px');

    var frame = document.createElement("span");
    frame.innerHTML = message;
    frame.setAttribute('style', 'width: 100%; height: 100%; border: none');

    divFrame.appendChild(frame);
    DivContent.appendChild(divFrame);

    Form.appendChild(DivContent);

    document.body.appendChild(Form);

    $('#' + id).css('display', 'block');
    $('#' + id).dialog({ height: height, width: width, minWidth: 300, minHeight: 150,
        title: title,
        resizable: false,
        modal: true,
        buttons: [
            {
                text: "Yes",
                click: function () {
                    if (okClickEvent && typeof okClickEvent === 'function') {
                        okClickEvent();
                    }
                    else {
                        __close(reloadOnClose, null, id);
                    }
                }
            },
            {
                text: "No",
                click: function () {
                    if (cancelClickEvent && typeof cancelClickEvent === 'function') {
                        cancelClickEvent();
                    }
                    else {
                        __close(reloadOnClose, null, id);
                    }
                }
            }],
        close: function (event) {
            __close(reloadOnClose, null, id);
            if (typeof closeClick == 'function') {
                closeClick();
            }
        }
    });
}

function __close(reload, delay, popupId, callBack) {
    var delayTime = delay ? delay : 100;
    var id = popupId ? popupId : 'windowPopup';

    reload && location.reload(true);

    var timeout = setTimeout(function () {
        $('#' + id).remove();
        clearTimeout(timeout);
    }, delayTime);

    if (typeof callBack == 'function') {
        callBack();
    }
}