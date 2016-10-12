
//$(function () {
jQuery(function ($) {

    InitSort();

});


var prmInstance = Sys.WebForms.PageRequestManager.getInstance();

prmInstance.add_endRequest(function () {
    //you need to re-bind your jquery events here
    InitSort();
});


function InitSort() {

    $("#fetord-wrapper UL").sortable({
        placeholder: "ui-state-highlight",
        update: function (event, ui) {
            UpdateOrder("promotion");
        }
    });

    $("#fetord-wrapper UL").disableSelection();


    $("#mlrsho-wrapper UL").sortable({
        placeholder: "ui-state-highlight",
        update: function (event, ui) {
            UpdateOrder($('#hdnSortableType').val());//mailerbanner or mailershow
        }
    });

    $("#mlrshow-wrapper UL").disableSelection();   

    var ctx = $('#hdnCollectionTableName').val();
    if (ctx != undefined) {

        //get cookie from type
        var cookie =
            (ctx == 'SalePromotion') ? $.cookie('acccbnr') :
            (ctx == 'Kiosk') ? $.cookie('acccksk') :
            (ctx == 'Post') ? $.cookie('acccpst') :
            (ctx == 'Employee') ? $.cookie('acccemp') :
            (ctx == 'FaqItem') ? $.cookie('acccfaq') :
            (ctx == 'Show') ? $.cookie('acccsho') :

            //show editor collections
            (ctx == 'ShowLink' || ctx == 'JShowAct' || ctx == 'JShowPromoter') ? 'a~3' :
            null;

        if (cookie != null) {

            var idx = cookie.indexOf('a~3');//value for orderable

            if (idx != -1) {

                if (ctx != 'ShowLink' && ctx != 'JShowAct' && ctx != 'JShowPromoter') {
                    var prince = $.cookie('aepr');//admin principal cookie

                    if (prince == undefined || prince == null || prince.toLowerCase() == "all" || prince.toLowerCase() == "") {
                        alert("Collections cannot be ordered in ALL mode. Please select a more specific principal or change the viewing criteria.");
                        return;
                    }
                }

                $(".orderable-items UL").sortable({
                    placeholder: "ui-state-highlight",
                    update: function (event, ui) {
                        UpdateOrderable($(this), ctx);//mailerbanner or mailershow or kiosk or employee
                    }
                });

                //$(".orderable-items UL").disableSelection();//don't do this! messes with firefox!

            }
        }
    }

    
}

function UpdateOrderable(container, context) {

    //show editor collections
    if (context != 'ShowLink' && context != 'JShowAct' && context != 'JShowPromoter') {
        var prince = $.cookie('aepr');

        if (prince == undefined || prince == null || prince.toLowerCase() == "all") {
            alert("Collections cannot be ordered in ALL mode. Please select a more specific principal or change the viewing criteria.");
            postBackToParent('rebind');
            return;
        }
    }

    var newOrder = '';

    if (container.length > 0) {

        var listItems = $(container).children('li');

        listItems.each(function (index) {
            var ds = $(this).attr("id");
            var pieces = ds.split('_');
            var selectedItemId = pieces[1];
            newOrder += selectedItemId + ',';
        });

        if (newOrder.length > 0)
            newOrder = newOrder.slice(0, -1);
    }
    
    $.ajax({
        url: '/Admin/_customControls/PrincipalOrder.ashx',
        data: { 'entityType': context, 'newOrder': newOrder },
        datatype: "text",//handler is text
        type: 'post',
        success: function (result) {

            if (result.indexOf("Error: ") != -1) {
                alert(result);
                postBackToParent('rebind');
            }
            else {
                var lis = $(".orderable-items UL LI");

                if (lis.length > 0) {

                    lis.each(function (index) {

                        //find the span element with the row num and update the text within to the index of the row
                        $(this).find(".badge.item-number").text(index + 1);
                        $(this).find(".badge").text(index + 1);

                    });
                }

                //no need to post back - new order shows
                if (context == 'ShowLink' || context == 'JShowAct' || context == 'JShowPromoter') {
                    postBackToParent('rebindpostorder');
                }
                //else if(context == )
                //var container = getPostbackContainerIdHelper(e.currentTarget.name, 1);
                //postBack_ToControl_WithArgs(container, { "commandName": 'selectedidfromtypeahead', "newIdx": idx });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { alert('Ordering Error :' + xhr.responseText); }
    });
}


function UpdateOrder(context) {

    var newOrder = '';
    var lis = (context == 'mailershow' || context == 'mailerbanner') ? $("#mlrsho-wrapper UL LI") :
        (context == 'kiosk') ? $("#kskord-wrapper UL LI") :
        (context == 'employee') ? $("#employ-wrapper UL LI") :
        $("#fetord-wrapper UL LI");

    if (lis.length > 0) {

        lis.each(function (index) {

            var ds = $(this).attr("id");
            var pieces = ds.split('_');
            var selectedItemId = pieces[1];

            newOrder += selectedItemId + ',';
        });

        if (newOrder.length > 0)
            newOrder = newOrder.slice(0, -1);
    }

    if (context == 'mailershow' || context == 'mailerbanner') {
        dlg_PageMethod("UpdateOrderMs", initParamsMs(newOrder, context), dlg_fnSuccessMs, dlg_fnError);
    }
    else if (context == 'kiosk') {
        dlg_PageMethod("UpdateOrder", initParams(newOrder), dlg_fnSuccessRa, dlg_fnError);
    }
    else if (context == 'employee') {
        dlg_PageMethod("UpdateEmployeeOrder", initParams(newOrder), dlg_fnSuccessEmp, dlg_fnError);
    }
    else {
        dlg_PageMethod("UpdateOrder", initParams(newOrder), dlg_fnSuccess, dlg_fnError);
    }
}

function initParams(newOrder) {

    //update cart on server
    var pushParams = new Array();

    if (newOrder != undefined) {
        pushParams.push("newOrder");
        pushParams.push(newOrder);
    }

    return pushParams;
}

function initParamsMs(newOrder, context) {

    //update cart on server
    var pushParams = new Array();

    if (newOrder != undefined) {
        pushParams.push("newOrder");
        pushParams.push(newOrder);
    }

    pushParams.push('mailerContentId');
    var p1 = $('#hdnMailerContentId').val();
    pushParams.push(p1);

    pushParams.push('context');
    pushParams.push(context);

    return pushParams;
}

function dlg_fnSuccessRa(result) {
    //examine xhr for result

    //update selectionStatus
    if (result.d != "false") {

        //reorder rows
        var lis = $("#kskord-wrapper UL LI");

        if (lis.length > 0) {

            lis.each(function (index) {

                var idx = index + 1;

                //find the fetrow span element and update the text within to the index of the row
                $(this).find(".fetrow").text(idx);

            });
        }

        __doPostBack("ctl00$MainContent$ctl01", 'rebindorder');
    }
}

function dlg_fnSuccessMs(result) {
    //examine xhr for result

    //update selectionStatus
    if (result.d != "false") {

        //reorder rows
        var lis = $("#mlrsho-wrapper UL LI");

        if (lis.length > 0) {

            lis.each(function (index) {

                var idx = index + 1;

                //find the fetrow span element and update the text within to the index of the row
                $(this).find(".fetrow").text(idx);

            });
        }

        var parentControl = "ctl00$MainContent$ctl01";//FF hack
        var parentPieces = parentControl.split('&');
        var parent = parentPieces[0];
        __doPostBack(parent, 'resetContent');

    }
}

function dlg_fnSuccess(result) {
    //examine xhr for result

    //update selectionStatus
    if (result.d != "false") {
        
        //reorder rows
        var lis = $("#fetord-wrapper UL LI");

        if (lis.length > 0) {

            lis.each(function (index) {

                //var idx = index + 1;

                //find the fetrow span element and update the text within to the index of the row
                $(this).find(".fetrow").text(index);

            });
        }
    }
}

function dlg_fnError(xhr, ajaxOptions, thrownError) {
    // Boil the ASP.NET AJAX error down to JSON.
    var err = eval("(" + xhr.responseText + ")");
}

function dlg_PageMethod(fn, paramArray, successFn, errorFn) {

    var pagePath = window.location.pathname;
    //Create list of parameters in the form:   
    //{"paramName1":"paramValue1","paramName2":"paramValue2"}   
    var paramList = '';
    if (paramArray != undefined && paramArray.length > 0) {
        for (var i = 0; i < paramArray.length; i += 2) {
            if (paramList.length > 0) paramList += ',';
            paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
        }
    }
    paramList = '{' + paramList + '}';

    //Call the page method   
    $.ajax({
        type: "POST",
        url: pagePath + "/" + fn,
        contentType: "application/json; charset=utf-8",
        data: paramList,
        dataType: "json",
        success: successFn,
        error: errorFn
    });
}
