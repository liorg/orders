ko.bindingHandlers.modal = {
    init: function (element, valueAccessor) {
        $(element).modal({
            show: false
        });

        var value = valueAccessor();
        if (ko.isObservable(value)) {
            $(element).on('hide.bs.modal', function () {
                value(false);
            });
        }
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).modal("destroy");
        });

    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor();
        //http://jsfiddle.net/YmQTW/1/
        var shouldBeOpen = ko.utils.unwrapObservable(allBindingsAccessor().dialogVisible);
        //if (ko.utils.unwrapObservable(value)) 
        if (shouldBeOpen) {
            $(element).modal('show');
        } else {
            $(element).modal('hide');
        }
    }
}


function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
};

//---------------------
var guidEmpty = "00000000-0000-0000-0000-000000000000";

function OfferItem(id, name, desc, priceValue,
    isPresent, isDiscount, statusRecord, amount,
    hasPrice, allowEdit, allowRemove, allowCancel,
    objectId, objectIdType, productPrice, quntityType
    ) {
    this.Id = ko.observable(id);
    this.Name = ko.observable(name);
    this.Desc = ko.observable(desc);
    this.PriceValue = ko.observable(priceValue);

    this.IsPresent = ko.observable(isPresent);
    this.IsDiscount = ko.observable(isDiscount);
    this.StatusRecord = ko.observable(statusRecord);
    this.Amount = ko.observable(amount);

    this.HasPrice = ko.observable(hasPrice);
    this.AllowEdit = ko.observable(allowEdit);
    this.AllowRemove = ko.observable(allowRemove);
    this.AllowCancel = ko.observable(allowCancel);

    this.ObjectId = ko.observable(objectId);
    this.ObjectIdType = ko.observable(objectIdType);

    this.ProductPrice = ko.observable(productPrice);

    this.QuntityType = ko.observable(quntityType);

    this.Total = ko.computed(function () {
        return this.ProductPrice() * this.Amount();
    }.bind(this));

    this.ObjectId.subscribe(function (newId) {

        var newProduct = null;
        var objectType = this.ObjectIdType();
        switch (objectType) {
            case 99:
                newProduct = ko.utils.arrayFirst(vm.Discounts(), function (item) {
                    return item.Id == newId;
                });
                break;
            case 1:
                newProduct = ko.utils.arrayFirst(vm.Products(), function (item) {
                    return item.Id == newId;
                });
                break;
            case 3:
                newProduct = ko.utils.arrayFirst(vm.Distances(), function (item) {
                    return item.Id == newId;
                });
                break;
            case 4:
                newProduct = ko.utils.arrayFirst(vm.ShipTypes(), function (item) {
                    return item.Id == newId;
                });
                break;
            default:
                newProduct = null;
                break;
        }

        if (newProduct != null) {
            this.Name(newProduct.Name);
            this.Desc(newProduct.Desc);
            this.Amount(newProduct.Amount);
            this.ProductPrice(newProduct.ProductPrice);
            this.IsPresent(newProduct.IsPresent);
            this.QuntityType(newProduct.QuntityType);

        }
    }.bind(this));
};

var vm = new AppViewModel(offerClient);

function AppViewModel(vmData) {

    var self = this;
    self.isOpen = ko.observable(false);

    self.TimeWaitSend = ko.observable(vmData.TimeWaitSend);
    self.TimeWaitGet = ko.observable(vmData.TimeWaitGet);

    self.HasDirty = ko.observable(vmData.HasDirty);
    self.AddExceptionPrice = ko.observable(vmData.AddExceptionPrice);
    self.IsAddExceptionPrice = ko.observable(vmData.IsAddExceptionPrice);


    self.HasDirtyChange = function (b) {

        self.HasDirty(b);

        if (self.HasDirty() && !offerClient.IsDemo) {
            $('#error_container').bs_alert('התבצעו שינויים ,יש לבצע שמירה', 'הערה');
        }
    };

    self.Items = ko.mapping.fromJS(vmData.Items);
    self.Discounts = ko.observableArray(vmData.Discounts);
    self.Products = ko.observableArray(vmData.Products);
    self.ShipTypes = ko.observableArray(vmData.ShipTypes);
    self.Distances = ko.observableArray(vmData.Distances);

    self.DirtyDiscounts = ko.observableArray(vmData.DirtyDiscounts);

    var pricesType = [
                { "key": false, "value": "₪" },
                { "key": true, "value": "%" }];

    self.priceTypeList = ko.observableArray(pricesType);
    self.AllowAddItem = ko.observable(vmData.AllowAddItem);
    self.AllowAddDiscount = ko.observable(vmData.AllowAddDiscount);

    self.currentModalItem = ko.observable(null);

    self.cancelOffer = function (f) {
        var items = vm.Items;
        var id = f.Id();
        var r = confirm("האם את/ה בטוח/ה רוצה להסיר פריט?");
        if (r == false) return;

        var item = ko.utils.arrayFirst(vm.Items(), function (fitem) {
            return fitem.Id() == id;
        });
        if (item == null)
            return;

        if (item.ObjectIdType() == 99) {

            var disounts = self.Discounts;
            var dirtydis = self.DirtyDiscounts;

            var dirty = ko.utils.arrayFirst(self.DirtyDiscounts(), function (dirtyItem) {
                return dirtyItem.Id == item.ObjectId();
            });
            var discount = ko.utils.arrayFirst(self.Discounts(), function (discountItem) {
                return discountItem.Id == item.ObjectId();
            });
            if (discount == null) {
                if (dirty != null)
                    disounts.push(dirty);
                //else {
                //}
            }
            if (dirty != null) {
                dirtydis.remove(dirty);
            }
        }
        items.remove(item);
    };

    self.TotalDiscount = ko.computed(function () {
        var count = getTotalDiscount();
        if (count != null) return count.toFixed(2);
        return count;
    });

    self.TotalPrice = ko.computed(function () {
        var count = getTotalPrice();
        if (count != null) return count.toFixed(2);
        return count;
    });

    function getTotalPrice() {
        var count = 0;
        for (var i = 0; i < self.Items().length; i++) {

            if (self.Items()[i].IsDiscount() == true) continue;
            if (self.Items()[i].PriceValue() == null) {
                count = null;
                break;
            }
            count += parseFloat(self.Items()[i].PriceValue());
        }
        return count;
    }

    function getTotalDiscount(total) {
        var total = total || getTotalPrice();
        var count = 0;
        var countPersent = 0;
        var result = 0;
        if (total == null) return null;

        for (var i = 0; i < self.Items().length; i++) {
            var item = self.Items()[i];
            if (item.IsDiscount() == false) continue;
            if (item.PriceValue() == null) {
                count = null;
                break;
            }
            if (item.IsPresent() == true) {
                countPersent += parseFloat(item.PriceValue());
            }
            else {
                count += parseFloat(item.PriceValue());
            }

        }
        if (countPersent == null && count == null)
            return null;
        result = ((total * (countPersent / 100)) + count);

        return result;
    };

    self.hasOneMorePriceException = function () {

        var id = offerClient.ObjectIdExcpetionPriceId;
        priceEx = ko.utils.arrayFirst(vm.Items(), function (item) {
            return item.ObjectId() == id && item.ObjectIdType() == 5;
        });
        if (priceEx == null) return false;
        return true;


    }

    self.refreshWatch = function () {
        var timeWaitSet = offerClient.TimeWaitSetProductId;
        var timeWaitGet = offerClient.TimeWaitGetProductId;


        var twsend = ko.utils.arrayFirst(vm.Items(), function (item) {
            return item.ObjectId() == timeWaitSet && item.ObjectIdType() == 2;
        });
        if (twsend != null) {
            twsend.Amount(self.TimeWaitSend());
            twsend.PriceValue(offerClient.TimeWaitSend * twsend.ProductPrice());
            self.HasDirtyChange(true);
        }
        var twget = ko.utils.arrayFirst(vm.Items(), function (item) {
            return item.ObjectId() == timeWaitGet && item.ObjectIdType() == 2;
        });
        if (twget != null) {
            twget.Amount(self.TimeWaitGet());
            twget.PriceValue(offerClient.TimeWaitGet * twget.ProductPrice());
            self.HasDirtyChange(true);
        }

        //offerClient.TimeWaitSend
    };

    self.Total = ko.computed(function () {
        var totalPrice = getTotalPrice();
        if (totalPrice == null) return null;
        var totalDiscount = getTotalDiscount(totalPrice);
        if (totalDiscount == null) return null;
        return totalPrice - totalDiscount;
    });

    self.createNewItem = function (item) {
        self.isOpen(true);
        var newt = new OfferItem(guidEmpty, "", "", null,
                false, false, 3, 1,
                false, true, true, true, guidEmpty, 1, 0, "יח'");
        self.currentModalItem(newt);
    };

    self.createExceptionPriceItem = function (item) {
        self.isOpen(true);
        var newt = new OfferItem(guidEmpty, "", "", null,
                false, false, 3, 1,
                false, true, true, true, offerClient.ObjectIdExcpetionPriceId, 5, 0, "יח'");
        self.currentModalItem(newt);
    };

    self.createNewDiscount = function (item) {
        self.isOpen(true);
        var newt = new OfferItem(guidEmpty, "", "", null,
            true, true, 3, 1,
            false, true, true, true, guidEmpty, 99, null, "יח'");
        self.currentModalItem(newt);
    };

    self.openModal = function (item) {
        self.isOpen(true);

        var editt = new OfferItem(item.Id(), item.Name(), item.Desc(), item.PriceValue(),
            item.IsPresent(), item.IsDiscount(), item.StatusRecord(), item.Amount(),
            item.HasPrice(), item.AllowEdit(), item.AllowRemove(), item.AllowCancel(),
            item.ObjectId(), item.ObjectIdType(), item.ProductPrice(), item.QuntityType());

        self.currentModalItem(editt);

    };

    self.viewModal = function (item) {
        self.isOpen(true);

        var editt = new OfferItem(item.Id(), item.Name(), item.Desc(), item.PriceValue(),
            item.IsPresent(), item.IsDiscount(), item.StatusRecord(), item.Amount(),
            item.HasPrice(), false, item.AllowRemove(), item.AllowCancel(),
            item.ObjectId(), item.ObjectIdType(), item.ProductPrice(), item.QuntityType());

        self.currentModalItem(editt);

    };

    self.save = function () {
        debugger;
        var current = self.currentModalItem();
        if (current.ObjectId() == null || current.ObjectId() == guidEmpty) {
            alert("יש לבחור סוג");
            return;
        }
        if (current.ObjectId() == offerClient.ObjectIdExcpetionPriceId)
            self.IsAddExceptionPrice(true);

        if (current.StatusRecord() == 1 || current.StatusRecord() == 4) {
            product = ko.utils.arrayFirst(vm.Items(), function (item) {
                return item.Id() == current.Id();
            });
            if (product != null) {
                var isdirty = current.Name() != product.Name() || current.Amount() != product.Amount() || product.PriceValue() != current.Total();
                if (isdirty) {
                    //    alert("בוצע שינויים");
                    self.HasDirtyChange(true);
                }
                product.Name(current.Name());
                product.Desc(current.Desc());
                product.IsPresent(current.IsPresent());
                product.IsDiscount(current.IsDiscount());
                product.Amount(current.Amount());
                product.ProductPrice(current.ProductPrice());
                product.ObjectId(current.ObjectId());
                product.ObjectIdType(current.ObjectIdType());
                product.PriceValue(current.Total());
                product.HasPrice(true);

            }
        }
        else {
            self.HasDirtyChange(true);
            if (current.ObjectId() == offerClient.ObjectIdExcpetionPriceId)
                self.IsAddExceptionPrice(true);
            var item = {};
            item.Id = generateUUID();
            item.Name = current.Name();
            item.Desc = current.Desc();
            item.PriceValue = current.Total();
            item.IsPresent = current.IsPresent();
            item.IsDiscount = current.IsDiscount();
            item.StatusRecord = 4;
            item.Amount = current.Amount();

            item.HasPrice = current.Total() != null && current.Total() != "";
            item.AllowEdit = current.AllowEdit();
            item.AllowRemove = current.AllowRemove();
            item.AllowCancel = current.AllowCancel();
            item.ProductPrice = current.ProductPrice();
            item.ObjectId = current.ObjectId();
            item.ObjectIdType = current.ObjectIdType();
            item.QuntityType = current.QuntityType();
            var ob = ko.mapping.fromJS(item);

            var dd = self.Items;
            dd.push(ob);
            // var dd = vm.Items;
            //dd.push(ob);

            var discount = ko.utils.arrayFirst(vm.Discounts(), function (discountItem) {
                return discountItem.Id == item.ObjectId;
            });
            if (discount != null) {
                var disounts = self.Discounts;
                var dirtydis = self.DirtyDiscounts;
                dirtydis.push(discount);
                disounts.remove(discount);
            }

        }
        self.isOpen(false);
    }
};

ko.applyBindings(vm);

function refreshPage() {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: "מרענן דף,נא המתן"
    });
    window.location.reload(false);
}

$(document).ready(function () {
    $('#btnOk').click(function () {
        $.unblockUI();
        refreshPage();
        // return false;
    });

    $('#btnBack').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });
    $('#btnClose').click(function () {
        var message_width = "270";
        var message_height = "200";
        var gobackurl = $(this).attr("data-url");
        $("#txtCloseActualPrice").val(vm.Total());
        $("#txtClosePrice").val(vm.TotalPrice());
        $("#txtCloseDiscountPrice").val(vm.TotalDiscount());
        $.blockUI({
            message: $('#dlgClosed'),
            css: {
                width: message_width + "px",
                height: message_height + "px",
                top: '50%',
                left: '50%',
                margin: (-message_height / 2) + 'px 0 0 ' + (-message_width / 2) + 'px'
            },
        }
        );

    });
    $('#btnNoCloseYet').click(function () {
        $.unblockUI();
        return false;
    });

    $('#btnYesClose').click(function () {
        debugger;
        var gobackurl = $(this).attr("data-url");
        var items = ko.mapping.toJS(vm.Items);
        var offer = {
            'Id': offerClient.Id,
            'HasDirty': vm.HasDirty(),
            'OfferId': offerClient.OfferId,
            'ShippingCompanyId': offerClient.ShippingCompanyId,
            'StateCode': offerClient.StateCode,
            'DiscountPrice': vm.TotalDiscount(),
            'Price': vm.TotalPrice(),
            'Total': vm.Total(),
            'IsAddExceptionPrice': vm.IsAddExceptionPrice(),
            'DataItems': items,
            'ClosedPrice': $("#txtClosePrice").val(),
            'ClosedTotal': $("#txtCloseActualPrice").val(),
            'ClosedDiscountPrice': $("#txtCloseDiscountPrice").val()
        };
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "מעבד את הבקשה,נא המתן"
        });

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/api/OfferService/CommitOffer",
            data: offer,
            success: function (data) {
                debugger;
                $.unblockUI();
                if (!data.IsError) {
                    if (data.MessageClient != null && data.MessageClient != "") {
                        $('#vmclientMessage').text(data.MessageClient);
                        $.blockUI({ message: $('#clientMessage'), css: { width: '275px' } });
                    }
                    else {
                        //alert("התהליך בוצע");
                        $.growlUI('סטאטוס', 'התהליך בוצע!');
                        refreshPage();
                    }
                }
                else
                    alert(data.ErrDesc);
            },
            error: function (error) {
                debugger;
                $.unblockUI();
                jsonValue = jQuery.parseJSON(error.responseText);
                //jError('An error has occurred while saving the new part source: ' + jsonValue, { TimeShown: 3000 });
            }
        });
    });

    $('#btnCreate, #btnConfirm, #btnGrant').click(function () {
        debugger;
        var gobackurl = $(this).attr("data-url");
        var items = ko.mapping.toJS(vm.Items);
        var offer = {
            'Id': offerClient.Id,
            'HasDirty': vm.HasDirty(),
            'OfferId': offerClient.OfferId,
            'ShippingCompanyId': offerClient.ShippingCompanyId,
            'StateCode': offerClient.StateCode,
            'DiscountPrice': vm.TotalDiscount(),
            'Price': vm.TotalPrice(),
            'Total': vm.Total(),
            'IsAddExceptionPrice': vm.IsAddExceptionPrice(),
            'DataItems': items
        };
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "מעבד את הבקשה,נא המתן"
        });

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/api/OfferService/CommitOffer",
            data: offer,
            success: function (data) {
                debugger;
                $.unblockUI();
                if (!data.IsError) {
                    if (data.MessageClient != null && data.MessageClient != "") {
                        $('#vmclientMessage').text(data.MessageClient);
                        $.blockUI({ message: $('#clientMessage'), css: { width: '275px' } });
                    }
                    else {
                        //alert("התהליך בוצע");
                        $.growlUI('סטאטוס', 'התהליך בוצע!');
                        if (gobackurl != "")
                            changeUrl(gobackurl);
                        else
                            refreshPage();
                    }
                }
                else
                    alert(data.ErrDesc);
            },
            error: function (error) {
                debugger;
                $.unblockUI();
                jsonValue = jQuery.parseJSON(error.responseText);
                //jError('An error has occurred while saving the new part source: ' + jsonValue, { TimeShown: 3000 });
            }
        });
    });

    $('#btnCancel, #btnCancelByAdmin, #btnNoCommit').click(function () {
        debugger;
        var items = ko.mapping.toJS(vm.Items);
        var offer = {
            'Id': offerClient.Id,
            'HasDirty': vm.HasDirty(),
            'OfferId': offerClient.OfferId,
            'ShippingCompanyId': offerClient.ShippingCompanyId,
            'StateCode': offerClient.StateCode,
            'DataItems': items
        };
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "מעבד את הבקשה,נא המתן"
        });

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/api/OfferService/CancelOffer",
            data: offer,
            success: function (data) {
                debugger;
                $.unblockUI();
                if (!data.IsError) {
                    alert("התהליך בוצע");
                    $.blockUI({
                        css: {
                            border: 'none',
                            padding: '15px',
                            backgroundColor: '#000',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            opacity: .5,
                            color: '#fff'
                        },
                        message: "מרענן את הדף,נא המתן"
                    });
                    window.location.reload(false);
                }
                else
                    alert(data.ErrDesc);
            },
            error: function (error) {
                debugger;
                $.unblockUI();
                jsonValue = jQuery.parseJSON(error.responseText);
                //jError('An error has occurred while saving the new part source: ' + jsonValue, { TimeShown: 3000 });
            }
        });
    });
});

(function ($) {
    $.fn.extend({
        bs_alert: function (message, title) {
            var cls = 'alert-danger';
            var html = '<div class="alert ' + cls + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>';
            if (typeof title !== 'undefined' && title !== '') {
                html += '<h4>' + title + '</h4>';
            }
            html += '<span>' + message + '</span></div>';
            $(this).html(html);
        },
        bs_warning: function (message, title) {
            var cls = 'alert-warning';
            var html = '<div class="alert ' + cls + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>';
            if (typeof title !== 'undefined' && title !== '') {
                html += '<h4>' + title + '</h4>';
            }
            html += '<span>' + message + '</span></div>';
            $(this).html(html);
        },
        bs_info: function (message, title) {
            var cls = 'alert-info';
            var html = '<div class="alert ' + cls + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>';
            if (typeof title !== 'undefined' && title !== '') {
                html += '<h4>' + title + '</h4>';
            }
            html += '<span>' + message + '</span></div>';
            $(this).html(html);
        }
    });
})(jQuery);
