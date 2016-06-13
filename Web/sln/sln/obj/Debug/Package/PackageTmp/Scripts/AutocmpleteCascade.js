$(document).ready(function () {
  //  debugger;
    $('.autp').each(initialize);
});
var initialize = function (i, el) {
   // debugger;
    // el is the input element that we need to initialize a map for, jQuery-ize it,
    //  and cache that since we'll be using it a few times.
    var $input = $(el);
    var city = "#" + $input.val() + "_City";
    var street = "#" + $input.val() + "_Street";
    var citycode = city + "code";
    var streetcode = street + "code";

    $(city).bind('autocompleteresponse',
         function (event, ui) {
             $(city).data('menuItems', ui.content);
         }).blur(function () {
            // debugger;
             var autocomplete = $(city).data("uiAutocomplete");
             var matcher = new RegExp("^"
                     + $.ui.autocomplete.escapeRegex($(this).val())
                     + "$", "i");
             var menuItems = $(this).data('menuItems');
             for (idx in menuItems) {
                 var item = menuItems[idx];
                 if (matcher.test(item.value)) {
                     autocomplete.selectedItem = item;
                     break;
                 }
             }
             if (autocomplete.selectedItem) {
                 autocomplete._trigger("select", event, {
                     item: autocomplete.selectedItem
                 });
             } else {
                 $(streetcode).val("");
                 $(street).val("");
                 $(citycode).val("");
                 $(city).val("");
             }
         }).autocomplete({
             source: "/api/address/SearchCities",
             html: true, autoFocus: true, selectFirst: true, autoSelect: true,
             minLength: 2,
             open: function (event, ui) {
                 $(".ui-autocomplete").css("z-index", 1000);
             },
             select: function (event, ui) {
                 debugger;
                 log(ui.item ? "Selected: " + ui.item.value + " aka " + ui.item.id : "Nothing selected, input was " + this.value);
                 $(citycode).val(ui.item.id);

                 $(streetcode).val("")
                 $(street).val("");//.prop("disabled", false);
             }


         });
    $(street).bind('autocompleteresponse',
         function (event, ui) {
             $(street).data('menuItems', ui.content);
         }).blur(function () {
             // debugger;
             var autocomplete = $(street).data("uiAutocomplete");
             var matcher = new RegExp("^"
                     + $.ui.autocomplete.escapeRegex($(this).val())
                     + "$", "i");
             var menuItems = $(this).data('menuItems');
             for (idx in menuItems) {
                 var item = menuItems[idx];
                 if (matcher.test(item.value)) {
                     autocomplete.selectedItem = item;
                     break;
                 }
             }
             if (autocomplete.selectedItem) {
                 autocomplete._trigger("select", event, {
                     item: autocomplete.selectedItem
                 });
             } else {
                 $(streetcode).val("");
                 $(street).val("");
                 //$(citycode).val("");
                 //$(city).val("");
             }
         }).autocomplete({
       html: true, autoFocus: true, selectFirst: true, autoSelect: true,
        open: function (event, ui) {
            $(".ui-autocomplete").css("z-index", 1000);
        },
        source: function (request, response) {
            var pfId = $(citycode).val();
            if (pfId == "") {
              //  $(street).removeClass("ui-autocomplete-loading");
                alert("יש לבחור עיר");
                request.preventDefault();
                return false;
            }
           // $(street).addClass("ui - autocomplete - loading");
            $.ajax({
                url: "/api/address/SearchStreets",
                dataType: "json",
                type: 'GET',
                data: {
                    MaxItems: 12,
                    ParentFilterId: pfId,
                    Term: request.term
                },
                success: function (data) {
                    //$(street).removeClass("ui-autocomplete-loading");
                    response(data);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            $(streetcode).val(ui.item.id);
            log(ui.item ? "Selected: " + ui.item.value + " aka " + ui.item.id : "Nothing selected, input was " + this.value);
        },
    });

}
function log(message) {
    $("<div>").text(message).prependTo("#log");
    $("#log").scrollTop(0);
}