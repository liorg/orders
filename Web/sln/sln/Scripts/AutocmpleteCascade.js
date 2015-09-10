
$(window).load(
       function () {

           //$.ui.autocomplete.prototype.options.autoSelect = true; 
           // Doesn't appear to work in ui 1.10.3
           // http://github.com/scottgonzalez/jquery-ui-extensions

           $(".ui-autocomplete-input").bind('autocompleteresponse',
                   function (event, ui) {
                       $(this).data('menuItems', ui.content);
                   });

           $(".ui-autocomplete-input").on(
                   "blur",
                   null,
                   function (event) {
                       var autocomplete = $(this).data("uiAutocomplete");
                       if (!autocomplete.options.autoSelect
                               || autocomplete.selectedItem) {
                           return;
                       }

                       var matcher = new RegExp("^"
                               + $.ui.autocomplete.escapeRegex($(this).val())
                               + "$", "i");
                       var menuItems = $(this).data('menuItems');
                       for (idx in menuItems) {
                           var item = menuItems[idx];
                           if (matcher.test(item.value)) {
                               autocomplete.selectedItem = item;
                               break;
                               // return false;
                           }
                       }
                       if (autocomplete.selectedItem) {
                           autocomplete._trigger("select", event, {
                               item: autocomplete.selectedItem
                           });
                       } else {
                           this.value = '';
                       }
                   });

       });


(function ($) {

    function activateChild() {
        var child = $(this);
        child.prop("disabled", false).removeClass("disabled");
        if (child.autocomplete) {
            child.autocomplete("enable");
        }
        var childval = $("#" + child[0].id + "Code");
        childval.prop("disabled", false).removeClass("disabled");
    }

    function deactivateChild() {
        var child = $(this);
        child.prop("disabled", true).val("").addClass("disabled");
        if (child.autocomplete) {
            child.autocomplete("disable");
        }
        var childval = $("#" + child[0].id + "Code");
        childval.prop("disabled", true).val("").addClass("disabled");
        
        var subChild = child.data("cascade-child");
        if (subChild) {
            deactivateChild.call(subChild);
        }
       
    }

    function eventOverride(originalEvent) {
        var child = this;
        return function (event, ui) {
            var isChildActivate = child.prop("disabled") === false;
            if (!ui.item && isChildActivate) {
                deactivateChild.call(child);
            } else if (ui.item && !isChildActivate) {
                activateChild.call(child);
            }
            if (typeof originalEvent === "function") {
                originalEvent.call(child, event, ui);
            }
        };
    }

    $.fn.cascade = function (child) {
        var parent = this;
        child = $(child);

        if (parent.autocomplete) {
            var originalChange = parent.autocomplete("option", "change");
            var originalSelect = parent.autocomplete("option", "select");
            parent.data("cascade-child", child);
            parent.autocomplete("option", "change", eventOverride.call(child, originalChange));
            parent.autocomplete("option", "select", eventOverride.call(child, originalSelect));
        }

        deactivateChild.call(child);

        return parent;
    };

    $.cascadingAutocompletes = function (autocompletes) {
        for (var i = 0; i < autocompletes.length - 1; i++) {
            if (autocompletes[i]) {
                $(autocompletes[i]).cascade(autocompletes[i + 1]);
            }
        }
    };
})(jQuery);