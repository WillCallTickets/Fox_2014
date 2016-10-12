


//gets keys from _Enums
function _getCooKeyFromContext(context, entityType) {
    context = context.toLowerCase();
    entityType = entityType.toLowerCase();

    var prefix = '';
    var postfix = '';

    switch (context) {
        case 'adminnavtabcontextcookie':
            prefix = 'rake';
            break;
        case 'adminnavtabcontainercookie':
            prefix = 'ract';
            break;
        case 'admincollectioncriteriacookie':
            prefix = 'accc';
            break;
    }

    switch (entityType) {
        case 'banner':
        case 'salepromotion':
            postfix = 'bnr';
            break;
        case 'kiosk':
            postfix = 'ksk';
            break;
        case 'post':
            postfix = 'pst';
            break;
        case 'employee':
            postfix = 'emp';
            break;
        case 'faqitem':
            postfix = 'faq';
            break;
        case 'show':
            postfix = 'sho';
            break;
        case 'act':
            postfix = 'act';
            break;
        case 'promoter':
            postfix = 'prm';
            break;
        case 'venue':
            postfix = 'vnu';
            break;
    }

    return prefix + postfix;
}


/**********************************************/
/*** Render a control into the given selector
/**********************************************/
function _renderContent(viewModel, view, context, contentSelector, titleSelector) {

    var retVal = '';
    var paramArray = new Array();
    paramArray.push("view");
    paramArray.push(view);
    paramArray.push("context");
    paramArray.push(context);

    var _uAgent = window.userAgent;
    paramArray.push("uAgent");
    paramArray.push(_uAgent);
    
    $.ajax({
        type: 'POST',
        url: '/Svc/controlrenderer.asmx/getView',
        contentType: 'application/json; charset=utf-8',
        data: parseParams(paramArray),
        dataType: 'json',
        success: function (result) {
            if (result.d != "false") {
                var data = result.d;

                if (titleSelector && titleSelector !== undefined && titleSelector.length > 0 && data.title.length > 0) {
                    $(titleSelector).html(data.title);
                }

                $(contentSelector).html(data.renderedView);
                //fade in component                
                $(".displayer").delay(400).addClass("in");
                registerAffix(view);
                evaluateUpdate(viewModel);

                return null;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            var err = eval("(" + xhr.responseText + ")");
            $(contentSelector).append(err.Message);
        }
    });
    
    return null;
}
/**********************************************/
/*** End RenderControl
/**********************************************/

function evaluateUpdate(viewModel) {
    var version = $("#hdnver").val();

    if (version != undefined && viewModel != undefined && version != viewModel.dataVersion()) {                
        try {
            $.getJSON("/api/eventdates/", function (data) {                
                viewModel.dataVersion(version);
                viewModel.events(
                     ko.utils.arrayMap(data, function (event) {
                        return new Event(event, viewModel.selected);
                    })
                    );
                $('#upcoming-shows a').address();
            });
        }
        catch (ex) {
            alert(ex);
        }
    }
}

//fixes a firefox issue with iframe videos in modals - zindexing
function ensureOpaqueVideo() {
    $('#showmoremodal IFRAME[src*="youtube"]').each(function () {
        var src = this.src;

        if (src.indexOf('/embed/') != -1) {
            if (src.indexOf('wmode=transparent') == -1) {
                src += (src.indexOf('?') == -1) ? '?' : '&';
                src += 'wmode=transparent';
            }
            $(this).attr({
                "src": src,
                "wmode": "transparent"
            });
        }
    });    
}

//if we are in a small viewport - close the nav 
function collapseReset() {
    var collapser = $('BUTTON.navbar-toggle');
    if (collapser != undefined && collapser.is(":visible")) {
        var navbar = $('.navbar-collapse');
        if (navbar.hasClass('in'))
            collapser.click();
    }
}

/**********************************************/
/*** Cycle Carousel functions*/
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

/**********************************************/
/* mailer funcs */
function registerMailto() {
    
    $('A.ml-mailer').each(function () {

        var self = $(this);
        var mailto = self.find('.ml-rec').get(0);
        var subject = self.find('.ml-subject').get(0);
        var body = self.find('.ml-body').get(0);

        var link = "mailto:";
        if (mailto != undefined) {
            if (mailto.innerHTML.length > 0) {
                link += mailto.innerHTML.replace("[atsign]", "@").replace("[dotsign]", ".");
            }
        }

        if (subject != undefined) {
            if (subject.innerHTML.length > 0) {
                link += "?subject=" + encodeURIComponent(subject.innerHTML);
            }
        }
        
        if (body != undefined) {
            if (body.innerHTML.length > 0) {

                if (link.indexOf("?") != -1)
                    link += "&";
                else
                    link += "?"

                link += "body=" + encodeURIComponent(body.innerHTML);
            }
        }

        self.attr("href", link);
    });
}

function onYouTubePlayerReady() {
}

function isStaticPageRequest(sPageName) {
    return $.inArray(parseUri(sPageName, true), staticPageList) != -1;
}

function scrollTheWindowUp() {
    //only way to get ie, ios and google on the same page
    var body = $("body").scrollTop() + $("html").scrollTop();
    if (body != 0) {
        $("body,html").animate({ scrollTop: 0 }, '500');
    }
}

function registerAffix(view) {

    //evaluate the height of the headliner and apply a smaller font - via class - if there are several lines of text
    var headlinerheight = $('.event-acts .head').height();
    if (headlinerheight > 180) //4 lines is 168
        $('.event-acts .head').addClass('long-text');
    scrollTheWindowUp();
    return;
}

function getBgImage() {
    return defaultImage;
}

function registerMeta() {
    var og_title = OG_title;
    var og_url = OG_url;
    var og_image = OG_image;
    var og_description = OG_description;

    outputMeta(og_title, og_url, og_image, og_description);
}

function outputMeta(og_title, og_url, og_image, og_description) {

    var currenturl = $('meta[property="og:url"]').attr('content');

    if (currenturl != og_url) {

        if (og_image.length == 0)
            og_image = getBgImage();

        if (og_image.indexOf("http") == -1)
            og_image = 'http://' + window.location.hostname + og_image;

        og_title = (og_title.Length > 70) ? og_title.substring(0, 69) : og_title;
        og_description = (og_description.Length > 200) ? og_description.substring(0, 199) : og_description;

        $('meta[property="og:title"]').attr('content', og_title);
        $('meta[property="og:image"]').attr('content', og_image.split('?')[0]);

        $('meta[property="og:url"]').attr('content', og_url);
        $('meta[property="og:description"]').attr('content', og_description);
    }
}

function doBackstretch(bgcontext) {
    
    var imgContainer = $("#site-bg");

    //don't bother with background on lo res or small devices
    if (imgContainer != undefined && imgContainer.length > 0) {
        var display = imgContainer.css('display');
        if (display == 'none')
            imgContainer = null;
    }
    if (imgContainer == undefined || imgContainer == null)
        return;

    var bgs = '';
    //if we are on a show page - get image from show
    if (bgcontext != 'static') {
        bgs = $('.show-container #hdnshowimg').val();
    }
    //if we are static or cant find a show image - use a random default
    if (bgs == undefined || bgs.length == 0) {
        bgs = getBgImage();
    }
    
    //now that we have an image....
    if (bgs.length > 0) {

        var existingHtml = imgContainer.html();

        // only change this if it is different than the current - don't reload 
        if ((existingHtml.length == 0) || (existingHtml.length > 0 && existingHtml.indexOf(bgs) == -1)) {

            imgContainer.backstretch(
                bgs,
                {
                    //set centering depending on portrait or landscape if specified not to have center specified
                    //default is true
                    centeredY: (bgs.indexOf("y=0") != -1) ? false : true, 
                    centeredX: (bgs.indexOf("x=0") != -1) ? false : true, 
                    fade: 300,
                    duration: 500//default is 5k!
                }).on("backstretch.before", function (e, instance, index) {
                }).on("backstretch.after", function (e, instance, index) {
                    $("#site-bg IMG").attr("itemprop", "image");//itemprop for google
                }
            );
        }
    }
}

/**********************************************/
/*** ShowMore jquery functionality for writeups
/**********************************************/
function registerShowMore(source) {

    if ($('.showmore-toggle').length <= 0) {

        var smh = $('.showmore').height();

        //height here needs to coincide with css
        if (smh != undefined && smh != null && smh >= 400) {

            var showMoreModal_set = $('#showmoremodal');

            if (showMoreModal_set != undefined && showMoreModal_set.length > 0 ) {

                $('.showmore')
                    .after("<a href=\"#showmoremodal\" class=\"text-info showmore-toggle\" role=\"button\" data-toggle=\"modal\"><small>[read more]</small></a>");

                // clear out any old content and make sure we have a button loader
                $('#showmoremodal .modal-body')
                    .html('<button type="button" class="hide-from-view wct-modal-action" data-toggle="button" data-loading-text="<span class=\'wct-modal-loader-spinner\'></span>Loading...">Loading writeup</button>');
                
                //set up the modal events
                showMoreModal_set.wctModal(
                       'getWriteup',
                       wct_getWriteupSuccess,
                       null,
                       wct_getWriteupParamBuilder
                       )
                   .on('show.bs.modal', function () {
                       //google event tracking
                       try {
                           ga('send', 'event', 'link', 'readmore', "'" + window.location.pathname + "'");
                       }
                       catch(e) {}
                   })
                   .on('shown.bs.modal', function () {
                       ensureOpaqueVideo();

                       $(this).scrollTop("0");//.animate({scrollTop:"0"}, 2000); animate does not work so well
                   })
                   .on('hide.bs.modal', function () {

                   }).on('hidden.bs.modal', function () {
                       $('#showmoremodal .modal-body').html('');
                   });
            }
        }//end if smh
    }
}
/**********************************************/
/*** End ShowMore
/**********************************************/
