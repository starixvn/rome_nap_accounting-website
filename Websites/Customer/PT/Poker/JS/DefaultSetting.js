$(document).ready(function() {
    $('#tblDefault').delegate('tr','mouseover', function( event ) {
        $(this).addClass("mouseon");  
     });
    $('#tblDefault').delegate('tr','mouseout', function( event ) {
        $(this).removeClass("mouseon");  
     });
    pokerData.getDefaultSetting();
});

var DefaultSetting = {
   defaultItem:function(i, r){
        this.No=i+1;
        this.CurrencyID=r.CurrencyID;
        this.Currency=r.Currency;
        this.MinCredit=ko.observable(autoFormatNumber(r.MinCredit));
        this._MinCredit=ko.observable(autoFormatNumber(r.MinCredit));
        this.MaxCredit=ko.observable(autoFormatNumber(r.MaxCredit));
        this._MaxCredit=ko.observable(autoFormatNumber(r.MaxCredit));
        this.Maxsdiscount=ko.observable(parseFloat(r.Maxsdiscount));
        this._Maxsdiscount = parseFloat(r.Maxsdiscount);
        this.Arrsdiscount = ko.observableArray(getAdminCommissionValues(0, parseFloat(maxSComm)));
        this.Maxadiscount=ko.observable(parseFloat(r.Maxadiscount));
        this._Maxadiscount = parseFloat(r.Maxadiscount);
        this.Maxdiscount=ko.observable(parseFloat(r.Maxdiscount));
        this._Maxdiscount = parseFloat(r.Maxdiscount);
        this.Maxplayerdiscount=ko.observable(parseFloat(r.Maxplayerdiscount));
        this._Maxplayerdiscount = parseFloat(r.Maxplayerdiscount);
        this.isUpdate = false;
   }, 
   VM:{
        items: ko.observableArray()
   },
   createMainDependentObservable:function(){
        DefaultSetting.VM.isUpdate=ko.dependentObservable(function(){
            var result=false;
            var len=DefaultSetting.VM.items().length;
            for (var i=0;i<len;i++)
            {
                var item=DefaultSetting.VM.items()[i];              
                if (item.isUpdate())
                {
                    result=true; 
                    $("#btnUpdateAll").removeAttr("disabled");                 
                    return result;
                }
            }
            $("#btnUpdateAll").attr("disabled","disabled");
            return result;    
        },DefaultSetting.VM);
        
   },
   createDependentObservable: function (item) {
        item.isUpdate = ko.dependentObservable(function () {
            if (
                    item.MinCredit() != item._MinCredit() ||
                    item.MaxCredit() != item._MaxCredit() ||
                    item.Maxsdiscount() != item._Maxsdiscount || 
                    item.Maxadiscount() != item._Maxadiscount ||
                    item.Maxdiscount() != item._Maxdiscount ||
                    item.Maxplayerdiscount() != item._Maxplayerdiscount
                )
                return true;
            return false;
        }, DefaultSetting.VM);
    },
    
   mappingData:function(data){
        DefaultSetting.VM.items.removeAll();
        for (var i = 0; i < data.length; i++) {
            var item=new DefaultSetting.defaultItem(i,data[i]);
            DefaultSetting.createDependentObservable(item);
            DefaultSetting.VM.items.push(item);
        }
        DefaultSetting.createMainDependentObservable();
        
        ko.applyBindings(DefaultSetting.VM, document.getElementById("defaultSetting"));
        $('#divMainBody').removeClass("hidden").reRenderScroller(797, 602, true);
        var t=setTimeout(function(){
            $('.noneDecimal').each(function(){
                 $(this).bind('keypress',function(e){ 
                    allowInt(e);
                 });   
            });
        },500);
   }
};