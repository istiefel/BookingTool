//Override some jquery.validate methods with the globalized versions 
$.validator.methods.date = function (value, element) { 
    return this.optional(element) || !isNaN(Globalize.parseDate(value)); 
}; 
$.validator.methods.number = function (value, element) { 
    return this.optional(element) || !isNaN(Globalize.parseFloat(value)); 
};

$(function() {
    //Ask ASP.NET what culture we prefer, because we stuck it in the html tag 
    var cultureName = $("html").attr("lang");
    //Set the datepicker culture 
    //if ($.datepicker !== undefined)
    //    $.datepicker.setDefaults($.datepicker.regional[cultureName]);
    //Tell the Globalize plugin the prefered culture 
    Globalize.culture(cultureName);
});