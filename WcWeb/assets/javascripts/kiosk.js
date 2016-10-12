
$(document).ready(function () {
    var vm = new viewModel();
    vm.loadData();
    $('.cycle-slidesho').cycle();
});

function KioskEvent(kioskevent) {

    var self = this;
    kioskevent = kioskevent || {};

    self.Id = kioskevent.Id;
    self.DisplayOrder = kioskevent.DisplayOrder;
    self.Timeout = kioskevent.Timeout;
    self.Name = kioskevent.Name;
    self.DisplayUrl = kioskevent.DisplayUrl;
    self.CtrX = kioskevent.CtrX;
    self.CtrY = kioskevent.CtrY;
    self.StartDate = kioskevent.StartDate;
    self.EndDate = kioskevent.EndDate;
    self.DivText = kioskevent.DivText;
    //do self.as a function, in case we decide to let some early birds out
    self.IsDisplayable = function () {
        var now = new Date().getTime();
        return self.StartDate < now && now <= self.EndDate;//if the announce date is less than NOW - show
    }
}

function viewModel() {

    var self = this;
    self.isInit = false;
    self.kioskEvents = ko.observableArray([]);
    self.dataVersion = ko.observable();
    self.dataVersion.subscribe(function (newValue) {
    });

    self.updateData = function () {
        self.dataVersion((window.wannVersion != undefined) ? window.wannVersion : new Date().getTime());
        kiosks = kiosks || {};
        if (kiosks != undefined && kiosks.length > 0) {

            self.kioskEvents(ko.utils.arrayMap(kiosks, function (kioskevent) {
                return new KioskEvent(kioskevent);
            }));
        }
    }

    self.loadData = function () {
        self.updateData();
        self.isInit = true;
        ko.applyBindings(self);
    }
}
