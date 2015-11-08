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
    self.HasDirty = ko.observable(vmData.HasDirty);
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
    }

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

        var current = self.currentModalItem();
        if (current.ObjectId() == null || current.ObjectId() == guidEmpty) {
            alert("יש לבחור סוג");
            return;
        }
        if (current.StatusRecord() == 1 || current.StatusRecord() == 4) {
            product = ko.utils.arrayFirst(vm.Items(), function (item) {
                return item.Id() == current.Id();
            });
            if (product != null) {
                var isdirty = current.Name() != product.Name() || current.Amount() != product.Amount() || product.PriceValue() != current.Total();
                if (isdirty) {
                    //    alert("בוצע שינויים");
                    vm.HasDirty(true);
                }
                product.Name(current.Name());
                product.Desc(current.Desc());
                product.IsPresent(current.IsPresent());
                product.IsDiscount(current.IsDiscount());
                product.Amount(current.Amount());
                product.ObjectId(current.ObjectId());
                product.ObjectIdType(current.ObjectIdType());
                product.PriceValue(current.Total());
                product.HasPrice(true);

            }
        }
        else {
            vm.HasDirty(true);
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

$(document).ready(function () {
    $('#btnEditPrice').click(function () {
        debugger;
        var items = ko.mapping.toJS(vm.Items);
        var offer = {
            'Id': offerClient.Id,
            'HasDirty': vm.HasDirty(),
            'OfferId': offerClient.OfferId,
            'DataItems': items
        };


        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/api/OfferService/EditOffer",
            data: offer,
            success: function (data) {
                debugger;
                alert(data);
            },
            error: function (error) {
                debugger;
                jsonValue = jQuery.parseJSON(error.responseText);
                //jError('An error has occurred while saving the new part source: ' + jsonValue, { TimeShown: 3000 });
            }
        });
    });
});