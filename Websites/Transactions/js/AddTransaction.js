var addWhiteLabelTrans = new function() {
    var self = this;
    self.Tabs = [
            { id: 'add-trans', tabId: 'tab-add-trans' },
            { id: 'add-trans-white-label', tabId: 'tab-add-trans-white-label' },
        ];

    self.DOMElements = {};

    self.cachedDOMElements = function () {
        self.DOMElements = {
            btnWhiteLabelAdd: $('#btnWhiteLabelAdd'),
            txtWhiteLabelAmount: $('#txtWhiteLabelAmount'),
            txtWhiteLabelDesc: $('#txtWhiteLabelDesc'),
            txtWhiteLabelDate: $('#txtWhiteLabelDate'),
        };
    }
    
    self.init = function (){
        self.cachedDOMElements();
        self.DOMElements.btnWhiteLabelAdd.click(function () {
            return self.validate();
        });
    };
    
    self.validate = function () {
        if (self.DOMElements.txtWhiteLabelDate.val() == '') {
            showAlertMessage("errMsg", "Please select date.", 1);
            self.DOMElements.txtWhiteLabelDate.focus();
            return false;
        }
        if (self.DOMElements.txtWhiteLabelAmount.val() == '') {
            showAlertMessage("errMsg", "Invalid Amount.", 1);
            self.DOMElements.txtWhiteLabelAmount.focus();
            return false;
        }

        return true;
    };
    
    self.addTrans = function () {

    };

    self.switchTab = function(index){
        var tab = self.Tabs[index];
        self.Tabs.forEach(function (item) {
            $('#' + item.id).hide();
            $('#' + item.tabId).removeClass('current');
        });

        $('#' + tab.id).show();
        $('#' + tab.tabId).addClass('current');
    };
};

$(document).ready(function (){
    addWhiteLabelTrans.init();
});