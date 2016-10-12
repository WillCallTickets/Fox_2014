

//sets the active nav item
function _updateCarouselNavbar(pathname) {
    
    var currentActiveNav = $('.carousel-nav .nav li.active A').attr('href');
    if (currentActiveNav == undefined || currentActiveNav != pathname) {
        
        pathname = '/' + pathname;

        var listItems = $('#main-nav li, #nav-carousel .nav li, #carousel-tron .nav li').removeClass();
        var navs = $('#main-nav a, #nav-carousel .nav a, #carousel-tron .nav a');

        navs.each(function (i, val) {

            if (val.pathname == pathname) {
                $(val).closest('li').addClass('active');
            }
        });

        var def = listItems.filter('.active');
        if (def.length == 0) {
            navs.filter("[href^='/newsletter']").closest('li').addClass('active');
        }
    }
}

//sets visibility of content panel within the carousel
function _updateCarouselActiveItem(pathname) {
    
    var currentActiveItem = $('.carousel .carousel-inner div.item.active').attr('data-view');
    if (currentActiveItem == undefined || currentActiveItem != pathname) {

        $('.carousel .carousel-inner div.item').removeClass('active');
        var items = $('.carousel .carousel-inner div.item');

        items.each(function (i, val) {

            var vw = $(val).attr('data-view');
            if (vw == pathname) {
                $(val).addClass('active');
            }
        });

        var def = items.filter('.active');
        if (def.length == 0) {
            items.filter("[data-view^='newsletter']").addClass('active');
        }
    }
}

function _setCarouselItemScrollHeight() {
    var item = $(".carousel .carousel-wrapper DIV.item.active");
    $(item).each(function (idx, val) {

        var thisHeight = val.clientHeight;
        var car = $(val).closest('.carousel-inner');
        var carHeight = car.height();
        if (thisHeight > carHeight)
            car.scrollTop(0).css({ 'overflow-y': 'auto' });
        else
            car.scrollTop(0).css({ 'overflow-y': 'hidden' });
    });
}

function setCarousel(e) {

    var pathname = sanitizePath(e);
    var intention = pathname;
    if (pathname == 'index' || pathname == '' || pathname == 'subscribe' || pathname == 'unsubscribe' || pathname == 'mailerconfirm' || pathname == 'mailermanage' || pathname == 'signupcampaign')
        pathname = 'newsletter';

    //ensure the item is loaded correctly - intention
    //if we are on a different active carousel item then reload
    //always reload newsletter
    _setPageTitle(e);
    _updateCarouselNavbar(pathname);
    _updateCarouselActiveItem(pathname);
    _loadCarouselByItemView(intention);
    _showBottomContent(pathname);
}

function _showBottomContent(pathname) {

    if (pathname == 'newsletter') {
        $('#venue-lineup').removeClass('in active');
        $('#mlr-example').addClass('in active');
        $('#mlr-example').show("slow");
        $('#venue-lineup').hide("slow");
    }
    else {        
        $('#mlr-example').removeClass('in active');
        $('#venue-lineup').addClass('in active');
        $('#mlr-example').hide("slow");
        $('#venue-lineup').show("slow");
    }
}

//keep this simple and make it render the intended view
function _loadCarouselByItemView(intendedView) {
    
    var item = $(".carousel .carousel-wrapper DIV.item.active");

    $.ajax({
        url: '/Handler/RenderView.ashx',
        data: { 'viewToRender': intendedView },
        datatype: "text",
        type: 'post',
        success: function (result) {
            if (result.indexOf("Error: ") != -1) {
                alert(result);
            }
            else {
                var res = JSON.parse(result);
                $(item).html(res["renderedView"]);
                _setCarouselItemScrollHeight();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

$(document).ready(function () {

    
    function externalChange(e) {    
    }

    function internalChange(e) {
    }

    function allAddressChanges(e) {
        setCarousel(e);
        ga('send', 'pageview');
    }

    //apply address plugin to navigational elements
    var addressees = $('#main-nav a, #nav-carousel .nav a, #carousel-tron .nav a');    
    addressees.address();

    //register callbacks for address components
    $.address.change(allAddressChanges).internalChange(internalChange).externalChange(externalChange);
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

//lets us navigate via tabs within a control
function anchorClientLink(anchor) {
    var rel = anchor.rel;
    if (rel.trim().length > 0) {

        $.address.value(rel);
    }
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

function _setPageTitle(e) {

    var path = window.location.pathname;

    var pagedef = 'wctpg-index';
    var ttl = 'Z2 Entertainment - ';
    var len = e.pathNames.length;

    if (len == 0) {
        //set a default from current selection
        ttl += parsePageTitleFromPath('Home');
        pagedef += ' wctpg-home';
    }
    else if (len == 1) {
        ttl += parsePageTitleFromPath(e.path);
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
        
    var maxTitleLength = 64;//max length for page title
    if (ttl.length > maxTitleLength) {
        ttl = ttl.slice(0, maxTitleLength - 3) + '...';
    }

    //changes the title in the tab
    $.address.title(ttl);

    //need to also change the title in the header
    $('html head').find('title').text(ttl);

    //replace all classes with new def "wctpg-[static|index] wctpg-[pagename]"
    $('#pagedef').attr('class', pagedef.replace(".aspx", "_aspx"));

}

function sanitizePath(e) {
    var pathname = (e.path) ? e.path : e;

    //make sure pathname is in sync with window.location
    if (pathname == "/" && window.location.pathname !== "/") {
        pathname = window.location.pathname.replace(/^[\/#]+/, '');
    }

    //sanitize pathname
    return pathname.replace(".aspx", "").replace("_aspx", "").replace(/^[\/#]+/, '');
}

function doPagePopup(url, employBrowserFeatures) {

    try {
        var H
        if (employBrowserFeatures == undefined || employBrowserFeatures == 'false')
            H = window.open("", 'Information', 'toolbar=1,scrollbars=1,location=0,status=0,menubar=0,resizable=1,width=1000,height=800,screenX=50,screenY=50,top=50,left=50');
        else
            H = window.open("", 'Information', 'toolbar=1,scrollbars=1,location=1,status=1,menubar=1,resizable=1,width=1000,height=800,screenX=50,screenY=50,top=50,left=50');
        H.location.href = url;
        H.focus();
    }
    catch (Exception) { }
}

parseParams = function (paramArray) {

    var paramList = "";
    if (paramArray != undefined && paramArray.length > 0) {
        for (var i = 0; i < paramArray.length; i += 2) {
            if (paramList.length > 0) paramList += ',';
            paramList += "'" + paramArray[i] + "' : '" + paramArray[i + 1] + "'";
        }
    }

    paramList = "{" + paramList + "}";
    return paramList;
}

/*** Cycle Carousel functions  */
function calculateTimeout(currElement, nextElement, opts, isForward) {
    var ret = getRelValue(nextElement.innerHTML);
    return (ret != -1) ? ret : 2400;
}

function getRelValue(input) {
    if (input != undefined && input.indexOf("rel=") != -1) {
        var parts = input.split('rel="');
        if (parts.length > 1) {
            var perts = parts[1].split('"');
            var value = perts[0];
            try { return parseInt(value); }
            catch (e) { }
        }
    }
    return -1;
}

function pagerFactory(idx, slide) {
    return '<li><a href="#">' + (idx + 1) + '</a></li>';
};