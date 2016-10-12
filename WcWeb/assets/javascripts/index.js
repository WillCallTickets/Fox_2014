ko.bindingHandlers.showVisible = {
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value) {
            $(element).show(600);
        } else {
            $(element).hide();
        }
    }
}

Date.prototype.getShortMonthName = function () {
    var m = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    return m[this.getMonth()];
}

Date.prototype.getShortDayName = function () {
    var d = ['Sun', 'Mon', 'Tue', 'Wed', 'Thur', 'Fri', 'Sat'];
    return d[this.getDay()];
}

function Event(event, selectedEvent) {
    var self = this;
    event = event || {};

    self.Id = event.Id;
    self.EventOrigin = event.EventOrigin;
    self.Url = '/' + event.Url;
    self.AnnounceTicks = event.AnnounceTicks;
    self.OnSaleTicks = event.OnSaleTicks;
    self.OnSaleDate = event.OnSaleDate;
    self.ShowDateTicks = event.ShowDateTicks;
    self.showDate = new Date(self.ShowDateTicks);
    self.isoDate = self.showDate.toISOString();
    self.Month = self.showDate.getMonth() + 1;
    self.DayName = self.showDate.getShortDayName(self.showDate.getDay());
    //showdates are not dependent on timezone, etc
    self.Day = event.DisplayDay;;
    self.MonthName = event.DisplayMonth;    
    self.ShowTime = event.ShowTime;
    self.SortTime = event.SortTime;
    self.Status = event.Status;
    self.Alert = event.Alert;
    self.Pricing = event.Pricing;
    self.TicketUrl = event.TicketUrl;
    self.Ages = event.Ages;
    self.Billing = event.Billing;
    self.BillingOpens = event.BillingOpens;
    self.DoorTime = event.DoorTime;
    
    self.ParsedStatus = function () {
        switch (self.Status.toLowerCase()) {
            case 'onsale':
                return "";
                break;
            case 'soldout':
                return "Sold Out";
                break;
            case 'cancelled':
                return "Can-Celled";
                break;
            case 'postponed':
                return "Post-Poned";
                break;
            case 'moved':
                return "Moved";
                break;
            case 'notactive':
                return "";
                break;
        }

        return self.Status;
    }

    //do self.as a function, in case we decide to let some early birds out
    self.Displayable = function () {
        var now = new Date().getTime(); 
        var showDateDate = new Date(self.showDate.getFullYear(), self.showDate.getMonth(), self.showDate.getDate());
        var showDateExpiry = showDateDate.setHours(26);
        return self.AnnounceTicks < now && now <= showDateExpiry ;//if the announce date is less than NOW - show
    }

    self.DisplaySaleDate = function () {
        if (self.OnSaleDate != '') {//if the sale date is greater than NOW - display it            
            var now = new Date().getTime();
            if (self.OnSaleTicks > now) {
                return self.OnSaleDate;
            }
        }
        return '';
    }

    //we passed in the viewModel's selectedEvent observable, so that we can mark ourself as the selected product
    //self.makes it easy to bind our item against select (data-bind="click: select") rather than using anonymous functions
    self.setSelected = function () {        
        //if we are coming from the upcoming shows - then show a load animation
        selectedEvent(self);
    };

    self.navTickets = function () {
        var href = self.TicketUrl;
        window.location.replace(href);
        return false;
    }
}

function viewModel() {

    var self = this;
    self.isInit = false;
    self.events = ko.observableArray([]);
    self.selected = ko.observable();
    self.searchValue = ko.observable(null);
    self.staticValue = ko.observable(null);
    self.dataVersion = ko.observable();
    self.agent = ko.observable();

    self.displayEventToggle = ko.computed(noSearchNoStatic, self);

    function noSearchNoStatic() {
        var s = (self.searchValue() == undefined || self.searchValue() == null) &&
            (self.staticValue() == undefined || self.staticValue() == null);

        return s;
    }

    self.dataVersion.subscribe(function (newValue) {
    });

    //when new event is selected - clearout the static content
    self.selected.subscribe(function (newValue) {
        self.searchValue(null);
        self.staticValue(null);
    });

    //called by nav functions to load static content
    loadStaticControl = function (viewmodel, event) {        
        var tgt = event.target;
        if (tgt.tagName.toLowerCase() != 'a') {
            //find closest parent A tag
            tgt = $(tgt).closest('a').get(0);
        }

        var intention = tgt.pathname.toLowerCase();
        if (isStaticPageRequest(intention)) {
            viewmodel.staticValue(parseUri(intention, true));
            viewmodel.searchValue(null);
            collapseReset();
        }
    }

    //match an event by url
    self.searchAndSelectEventByUrl = function (sUrl, defaultToFirst) {

        if (self.events().length > 0) {
            var found = false;
            var result = $.grep(self.events(), function (e) { return e.Url.toLowerCase() == sUrl.toLowerCase(); });
            if (result.length > 0 && result[0].Displayable()) {
                result[0].setSelected();
            }
            else if (defaultToFirst) {
                self.selectDefaultShow();
            }
        }

        collapseReset();
    }

    loadSearchControl = function (viewmodel, event) {

        var terms = $('#ta-searchterms')[0].value;
        if (terms != undefined) {

            terms = '?' + terms.replace(/^\s+|\s+$/g, "");

            if (terms.length > 0) {
                viewmodel.searchValue(terms);
                viewmodel.staticValue('');

                collapseReset();
            }
        }
    }

    //init the viewmodel
    self.updateData = function () {

        if (events.length == 0)
            return;

        self.dataVersion((window.wannVersion != undefined) ? window.wannVersion : new Date().getTime());
        self.agent((window.uAgent != undefined) ? window.uAgent : "");

        self.events(ko.utils.arrayMap(events, function (event) {
            return new Event(event, self.selected);
        }));

        if (self.selected() == undefined) {

            self.selectDefaultShow();
        }
    }

    self.loadData = function () {

        self.updateData();
        self.isInit = true;
        ko.applyBindings(self);
    }

    //sets the default show to either first or selected default
    self.selectDefaultShow = function () {

        //if we have specified a default show...
        if (defaultShowDateId > 0) {
            var result = $.grep(self.events(), function (e) { return e.Id == defaultShowDateId; });
            if (result.length > 0 && result[0].Displayable()) {
                result[0].setSelected();
                return;
            }
        }

        //if no match - loop thru shows and set to first displayable show
        for (var i = 0; i < self.events().length; i++) {
            if (self.events()[i].Displayable()) {

                self.events()[i].setSelected();
                break;
            }
        }
    }
}

function _redirectForMobileMailManager(vm, pathname) {

    pathname = pathname.replace(/^[\/#]+/, '');
    redirectPath = 'http://' + window.location.hostname + '/MobileMailManager.aspx?p=' + pathname;
    window.location.replace(redirectPath);
}

function _renderForMobile(viewModel, pathname) {

    pathname = pathname.replace(/^[\/#]+/, '');
    //laminate has been (at least) temporarily removed
    var contentId = (pathname.toLowerCase().indexOf('laminate') != -1) ? '#showmoremodal' : '#maincontentpanel';

    _renderContent(viewModel, pathname, 'mobile', contentId);

    if (contentId == '#showmoremodal') {
        $('#showmoremodal').modal('show');
    }

    $(".displayer").delay(400).addClass("in");
    $('.lammie-close').on('click', function (e) {
        $('#showmoremodal').modal('close');
    });
}

function _renderViewModel(viewModel, pathname) {

    var performBackstrech = false;
    var view = pathname;
    var context = '';
    var og_title = '';
    var og_image = '';
    var og_url = '';
    var og_description = '';

    //Search
    if (viewModel !== undefined && viewModel.searchValue() != null && viewModel.searchValue().trim().length > 0) {

        view = 'search';
        context = viewModel.searchValue();
        performBackstrech = true;

        og_title = 'Search';
        og_url = 'http://' + window.location.hostname + '/' + pathname;
        og_description = 'Search';// for artists and dates
    }
    //Static
    else if (viewModel !== undefined && viewModel.staticValue() != null && viewModel.staticValue().trim().length > 0) {

        view = viewModel.staticValue();
        var pieces = pathname.replace(/^[\/#]+/, '').split('/');
        if (pieces.length > 1) {

            pieces.shift();
            context = pieces.join('/');
        }
        performBackstrech = true;

        og_title = view;
        og_url = 'http://' + window.location.hostname + '/' + pathname;
        og_description = view + ' - ' + context;
    }
    else if (view != undefined && view.indexOf('MobileMailManager') != -1) {
        context = 'mobile';
        og_title = 'MobileMailManager';
        og_url = 'http://' + window.location.hostname + view;
        src = window.location.search;
        if (src == undefined || src == null)
            src = '';

        og_description = view + src + ' - ' + context;

        view = view + src;
    }

    //cleanup #homelink
    if ((view == "/" || view == "") && viewModel.selected() != null) {
        for (var i = 0; i < viewModel.events().length; i++) {
            if (viewModel.events()[i].Displayable()) {

                viewModel.events()[i].setSelected();
                break;
            }
        }

        view = viewModel.selected().Url;
    }

    _renderContent(viewModel, view, context, '#maincontentpanel');
    
    outputMeta(og_title, og_url, og_image, og_description);

    //Shows call backstretch when rendered - statics and search do not
    if (performBackstrech) {
        doBackstretch("static");
    }
}

function setPageTitle(e, vm) {

    var path = decodeURIComponent(window.location.pathname);

    var pagedef = (isStaticPageRequest(decodeURIComponent(e.path))) ? 'wctpg-static' : 'wctpg-index';

    var ttl = 'Fox Theatre - ';
    var len = e.pathNames.length;

    //set a default from current selection
    if (len == 0) {
        ttl += parsePageTitleFromPath(vm.selected().Url);
    }
    else if (len == 1) {
        ttl += parsePageTitleFromPath(decodeURIComponent(e.path));
        pagedef += ' wctpg-' + e.path.substr(1).toLowerCase();
    }
    else {
        var stringified = ''

        for (var i = 0; i < len; i++) {
            if (i == 0)
                pagedef += ' wctpg-' + e.pathNames[i].toLowerCase();
            else
                stringified += '-' + e.pathNames[i];

            //return a Propercase string "F + aq"
            ttl += e.pathNames[i].charAt(0).toUpperCase() + e.pathNames[i].slice(1);

            if (i < len - 1) {
                ttl += ' - ';
            }
        }

        pagedef += stringified.toLowerCase(); 
    }

    //max length for page title
    var maxTitleLength = 64;
    if (ttl.length > maxTitleLength) {
        ttl = ttl.slice(0, maxTitleLength - 3) + '...';
    }

    //changes the title in the tab
    $.address.title(ttl);
    //need to also change the title in the header
    $('html head').find('title').text(ttl);

    //replace all classes with new def "wctpg-[static|index] wctpg-[pagename]"
    //wctpg-static wctpg-faq-general, wctpg-static wctpg-search
    //wctpg-index wctpg-2013-09-19-0830pm-some-artist-name
    $('#pagedef').attr('class', pagedef.replace(".aspx", "_aspx"));
}

function parsePageTitleFromPath(givenPath) {

    var delimiter = '-';
    var parts = givenPath.replace(/^[\/#]+/, '').split(delimiter);
    var len = parts.length;

    if (len > 4) {
        return parts.slice(4).map(function (str) { return str.charAt(0).toUpperCase() + str.slice(1) }).join(' ') + ' ' + parts.slice(0, 3).join(delimiter);
    }
    else if (len == 1)
        return parts[0].charAt(0).toUpperCase() + parts[0].slice(1);

    return parts.slice(0, len - 1).join(delimiter);
}




// PAGE FLOW
$(document).ready(function () {

    function handleMobile(vm, pathname) {
        var smallDeviceIndicator = $('#upcoming-shows .section-header').css('display');
        if (smallDeviceIndicator == undefined || smallDeviceIndicator == 'none') {
            if (pathname == "/studentlaminate") { 
                _renderForMobile(vm, pathname);
            }
            else if (pathname == "/mailermanage" || pathname == "/subscribe" || pathname == "/unsubscribe") {
                _redirectForMobileMailManager(vm, pathname);
            }
            return;
        }
    }

    //handle back buttons and refreshes
    function externalChange(e) {
        
        var pathname = decodeURIComponent(e.path);        
        handleMobile(vm, pathname);

        if (pathname == "/" && window.location.pathname !== "/") {
            pathname = window.location.pathname.replace(/^[\/#]+/, '');
        }

        if (isSearchPageRequest(pathname)) {
            var terms = e.queryString;
            if (terms !== undefined && terms.trim().length > 0) {
                vm.searchValue('?' + terms);
            }
            vm.staticValue(null);
        }
        else if (isStaticPageRequest(pathname)) {
            var parsed = parseUri(pathname, true);
            var staticV = vm.staticValue();

            //if we are naving from one static to another - or a new static
            if (parsed !== staticV) {
                vm.staticValue(parsed);
                vm.searchValue(null);
            }
            else if (parsed === staticV) {
                vm.searchValue(null);
            }
        }
        else {

            vm.searchAndSelectEventByUrl(pathname, true);
            vm.searchValue(null);
            vm.staticValue(null);
        }

        _renderViewModel(vm, pathname);
    }

    //handle pages that have internal links - ie. contact - boxoffice, officeinfo
    function isClientRedirect(pathname) {

        //where we are        
        var comingFrom = window.location.pathname + window.location.hash;
        comingFrom = comingFrom.replace(/^[\/#]+/, '').toLowerCase();
        var comingBase = comingFrom.split('/')[0];

        //where we are going
        var goingTo = pathname.replace(/^[\/#]+/, '').toLowerCase();
        var goingBase = goingTo.split('/')[0];

        //if we are navigating within the same control then don't re-render
        if (comingBase === goingBase)
            return true;

        return false;
    }

    function internalChange(e) {
        
        var pathname = decodeURIComponent(e.path);
        
        handleMobile(vm, pathname);

        //display the search criteria in the querystring so that we can track state
        if (pathname == '/admin') {
            window.location.replace(pathname);
        }
        else if (pathname == '/logout') {
            window.location.replace('/api/logout');
        }
        else if (isSearchPageRequest(pathname)) {

            var src = vm.searchValue();
            if (src != null) {

                $.address.autoUpdate(false);
                $.address.value(pathname + '/' + src);
                $.address.autoUpdate(true);
            }

            _renderViewModel(vm, pathname);
        }
        else if (!isClientRedirect(pathname)) {
            vm.searchValue(null);

            if (pathname === "/")
                vm.staticValue(null);

            else if (isStaticPageRequest(pathname)) {

                var parsed = parseUri(pathname, true);
                var staticV = vm.staticValue();

                //if we are naving from one static to another - or a new static
                if (parsed !== staticV) {
                    vm.staticValue(parsed);
                    vm.searchValue(null);
                }
                else if (parsed === staticV) {
                    vm.searchValue(null);
                }
            }

            _renderViewModel(vm, pathname);
        }
    }

    function allAddressChanges(e) {
        setPageTitle(e, vm);
        ga('send', 'pageview');
        $('.typeahead').typeahead('val', '').blur();
    }


    //STATIC LINKS!!!!
    //update nav elements so they can bind to view model
    //only use this for local links!
    var navAs = $('#mastheader a').not('#aboutdropdown').not('#mailerdropdown').not('#admindropdown').not('#site-social A')
        .add('#textuslink');
    navAs.each(function (idx) {
        $(this).attr("href", $(this).attr("href").replace('.aspx', '').replace('?p=', '/'))
            .attr('data-bind', 'click: loadStaticControl');
    });

    //SEARCH LINK/BUTTON ACTIVATION
    $('#ta-sitesearch').each(function (idx) {
        $(this).attr("href", $(this).attr("href").replace('.aspx', ''))
            .attr('data-bind', 'click: loadSearchControl');
    });

    /***********************************************/
    // Init the knockout ViewModel
    var vm = new viewModel();
    vm.loadData();
    
    //rewrite the url for default show
    if ($.address.value() == "/" && vm.selected() != null && vm.selected() != undefined) {
        $.address.history(false).value(vm.selected().Url).history(true);
    }

    //apply address plugin to navigational elements
    var addressees = $('#mastheader a').not('#aboutdropdown').not('#mailerdropdown').not('#admindropdown').not('#site-social A')
        .add('#upcoming-shows a')
        .add('#textuslink')
        .add('#justannounced A:not([href^="http://"], [href^="https://"])') //in case upcoming contains external links
        //add local carousel links - links without http://
        .add('#cycle2carousel A:not([href^="http://"], [href^="https://"])');
    addressees.address();

    //establish callbacks for address components
    $.address.change(allAddressChanges).internalChange(internalChange).externalChange(externalChange);

    // allow just announced clicks to update the upcoming menu selection
    $('#justannounced A').on('click', function (e) {
        self = this;
        var _url = self.pathname;

        var match = ko.utils.arrayFirst(vm.events(), function (event) {
            return event.Url == _url;
        });

        if (match != undefined && match.Url == _url) {
            //update the upcoming menu by setting selection
            match.setSelected();
        }
    });
    
    //twitter typeahead
    var hometts = $('#ta-searchterms');

    if (hometts != undefined && hometts.length > 0) {
        
        var ctx = 'Act';
        var lmt = 15;

        var homeresults = new Bloodhound({
            datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.Suggestion); },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            limit: lmt,
            remote: { url: '/api/searches/' + ctx + '/%QUERY/' + lmt + '/0' }
        });

        homeresults.initialize();

        hometts.typeahead(
            {
                hint: true,
                minLength: 2,
                highlight: true,
            },
            {
                name: 'homepage-searches',
                displayKey: 'Suggestion',
                //valueKey: (ctx == 'Show') ? 'Id' : 'Suggestion',//just use datum on selection
                source: homeresults.ttAdapter()
            }
        ).on('typeahead:selected', function (event, datum) {

            $('#ta-sitesearch').click();
            //$('.typeahead').typeahead('setQuery', '').blur(); //not here! - utilize in allAddressChanges
            $('#ta-searchterms').val();

        }).keypress(function (event) {

            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {

                $('#ta-sitesearch').click();
                //$('.typeahead').typeahead('setQuery', '').blur();//not here! - utilize in allAddressChanges
                $('#ta-searchterms').val();
            }
        });
    }
    //END TWITTER TYPEAHEAD

    //https://github.com/markgoodyear/scrollup
    $.scrollUp({
        scrollName: 'backtotop',
        scrollDistance: 400,
        scrollFrom: 'top',
        scrollSpeed: 300,
        animation: 'slide',
        animationInSpeed: 300, 
        animationOutSpeed: 200, 
        scrollText: 'Back To Top <i class="glyphicon glyphicon-upload"></i>',
        zIndex: 990//keep under modal
    });
    
    //scroll to top when hitting the home link logo
    $('#linkhome').click(function () {
        $("body,html").animate({ scrollTop: 0 }, '500');
    });

    $('#upcoming-loop .collapse')
        .on('show.bs.collapse', function (e) {
            //only react if we are mobile and extra small - otherwise hide it
            var iwidth = window.innerWidth;
            if (iwidth >= 500) {
                $(this).collapse('hide');
            }            
        }).on('shown.bs.collapse', function (e) {

            //don't load twice!!
            var social = $('#' + e.target.id + ' .m-social-space').get(0);
            if (social != undefined && social.innerHTML.indexOf('Loading') == -1) {
                return;
                //social.innerHTML = 'Loading...';
            }

            //ajax call to get social            
            var showIdx = e.target.id.replace('ecollapse', '');
            
            var params = new Array();
            params.push("eventDateIdx");
            params.push(showIdx);
            var paramList = parseParams(params);

            //param builder will assign errors for inputs
            var servicePage = "wctModalService";

            //Call the page method   
            $.ajax({
                type: "POST",
                url: "/Svc/" + servicePage + ".asmx/" + "getSocial",
                contentType: "application/json; charset=utf-8",
                data: paramList,
                dataType: "json",
                success: function (result) {
                    var data = result.d;
                    $('#' + e.target.id + ' .m-social-space').html('<div class="m-social">' + data.view + '</div>');
                },
                error: function (xhr, ajaxOptions, thrownError) {

                    var err = eval("(" + xhr.responseText + ")");                    
                    $('#' + e.target.id + ' .m-social-space').html(err);
                    return;
                }
            });


        }).on('hide.bs.collapse', function (e) { });
});

$(document).ajaxComplete(function () {
    try {
        FB.XFBML.parse();
        gapi.plusone.go();
        twttr.widgets.load();
    } catch (ex) { }
});

//get the portion of thr uri relevant to our system
function parseUri(sUri, bEvaluateFirstSegmentOnly) {

    var pathname = (sUri != undefined && sUri.trim().length > 0) ? sUri :
        (window.location.hash.trim().length > 0) ? window.location.hash.toLowerCase().trim() :
        window.location.pathname.toLowerCase().trim();

    //remove leading slashes and pound
    pathname = pathname.replace(/^[\/#]+/, '').toLowerCase();

    //remove any query string
    var parts = pathname.split('?');
    pathname = parts[0];

    if (bEvaluateFirstSegmentOnly === true && pathname.indexOf('/') != -1) {

        var pieces = pathname.split('/');
        pathname = pieces[0];
    }

    return pathname;
}

function isSearchPageRequest(sPageName) {
    var parsed = parseUri(sPageName, true);
    return parseUri(sPageName, true) == 'search';
}

//lets us navigate via tabs within a control
function anchorClientLink(anchor) {
    //get the rel attribute and change address - ***REQUIRED!
    var rel = anchor.rel;
    if (rel.trim().length > 0) {

        $.address.value(rel);
    }
}