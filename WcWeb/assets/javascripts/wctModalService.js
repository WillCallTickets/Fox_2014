
(function ($) {    

    /*  presupposes
        .wctmodal-response      => generic class for the error and success 
        .wctmodal-error         => error div
        .wctmodal-success       => success div
        .control-group          => surrounds lines of label input box
        xxxGroup                => is the id of the control group. xx is same as input
        .wct-modal-action       => modal submit button **required - hide/indent in css if necessary
        fnService               => this is a string that matches the service call
        fnSuccess               => success callback
        inputs                  => a list of input elements to handle clearing error indicators
        fnParamValidate         => function to ubild input from params and set errors
    */

    $.fn.wctModal = function (_fnService, _fnSuccess, _inputs, _fnParamValidate) {

        var self = $(this).get(0);
        if (self != undefined) {
            var selfId = self.id;
            var sender = $('#' + selfId + ' .wct-modal-action');
            var errorDisplayElement = $('#' + selfId + ' .wctmodal-error');
            var successDisplayElement = $('#' + selfId + ' .wctmodal-success');

            var inputs = _inputs;
            var errors = new Array();

            var fnService = _fnService;
            var fnSuccess = _fnSuccess;
            var fnParamValidate = _fnParamValidate;

            function displayAndResetErrors() {

                $('.control-group').add('.wctmodal-response').removeClass('alert alert-danger alert-success');

                //loop through errors and display
                if (errors != undefined && errors.length > 0) {

                    var output = '';
                    for (var i = 0; i < errors.length; i++) {
                        if (typeof errors[i] == 'string') {
                            output += '<li>' + errors[i] + '</li>';
                        }
                        else {
                            for (var k in errors[i]) {
                                if (k != null && k.trim().length > 0) {
                                    var elem = $(k + 'Group');
                                    $(elem).addClass('alert alert-danger');
                                    output += '<li>' + errors[i][k] + '</li>';
                                }
                            }
                        }
                    }

                    if (output.length > 0) {
                        output = '<ul class="list-unstyled">' + output + '</ul>';
                        errorDisplayElement.addClass('alert alert-danger').append(output);
                    }

                    errors = null;
                    reactivateSender();
                }
            }

            function deactivateSender() {
                sender.button('loading');
                $('.control-group').add('.wctmodal-response').removeClass('alert alert-danger alert-success');
                $('.wctmodal-response').html('');
            }

            function reactivateSender() {
                sender.button('reset');
            }

            function loadAjax() {

                errors = new Array();
                //param builder will assign errors for inputs
                var paramList = parseParams(fnParamValidate(errors));

                if (errors.length > 0) {
                    displayAndResetErrors();
                    return;
                }

                var servicePage = "wctModalService";

                $.ajax({
                    type: "POST",
                    url: "/Svc/" + servicePage + ".asmx/" + fnService,
                    contentType: "application/json; charset=utf-8",
                    data: paramList,
                    dataType: "json",
                    success: function (result) {
                        fnSuccess(result, selfId, successDisplayElement, errors, displayAndResetErrors, inputs);
                        reactivateSender();                        
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        var err = eval("(" + xhr.responseText + ")");
                        errors.push({ '': err });
                        displayAndResetErrors();
                        return;
                    }
                });
            }

            sender.click(function () {
                deactivateSender();
                loadAjax();
            });

            //return function binds the modal to the show/hide functions        
            return $(self).on('show.bs.modal', function (e) {                
                
                if (fnService === "getWriteup") {
                    var content = $('#' + selfId + ' .modal-body').html();
                    if (content == "") {
                        loadAjax();
                    }

                    //enable the modal by "clicking" on the open link
                    $('#' + self.id + ' .wct-modal-action').click();
                    $('body').css('overflow', 'hidden');

                    //IE Hack
                    $('.showmore IFRAME').css({ 'display': 'none', 'visibility': 'hidden' });
                }

                $('#' + selfId + ' .wctmodal-body').removeClass('hide');

            }).on('hidden.bs.modal', function () {
                //reset elements to their original state
                $('.form-group').add('.wctmodal-response').removeClass('alert alert-danger alert-success');
                $('.wctmodal-response').html('');

                //clean up inputs on close
                try {
                    if (inputs != undefined) {
                        for (var i = 0; i < inputs.length; i++) {
                            $(inputs[i]).html('').val('');
                        }
                    }
                }
                catch (ex) { }

                if (fnService == "getWriteup") {
                    $('body').css('overflow', 'auto');
                    //IE Hack
                    $('.showmore IFRAME').css({ 'display': 'block', 'visibility': 'visible' });
                }
            });
        }
    }
})(jQuery);


///////////////////////////////////////////////////////////////////////////
//  Copy Show Modal
///////////////////////////////////////////////////////////////////////////
function wct_showCopyParamBuilder(errArr) {

    var params = new Array();

    params.push("newShowDate");
    var p1 = $("#copyshowdate").val();
    params.push(p1);
    params.push("userName");
    var p2 = $("#hdnUserName").val();
    params.push(p2);
    params.push("currentShowId");
    var p3 = $("#hdnCurrentShowId").val();
    if (p3 == "0")
        errArr.push({ '': "Current show is not specified." });
    params.push(p3);

    return params;
}

function wct_showCopySuccess(result, callerId, successElement, errArr, fnError, inputs) {

    //update selectionStatus
    if (result.d != "false") {
        var data = result.d;

        if (data.error.length > 0) {
            errArr.push({ '#showdatecopycontrol': data.error });
            fnError();
        }
        else {
            //reset inputs and hide modal body
            for (var i = 0; i < inputs.length; i++) {
                $(inputs[i]).html('');
            }

            //redirect back to page/refresh/set new show
            postBackLoadNewShow(data.newShowId);
        }
    }
}

function postBackLoadNewShow(newShowId) {
    //go somewhere where the Session and other collections can be refreshed
    var parentControl = "ctl00$MainContent$ctl01";
    __doPostBack(parentControl, 'reload~' + newShowId);
}

///////////////////////////////////////////////////////////////////////////
//  Show Display Modal
///////////////////////////////////////////////////////////////////////////
function wct_showDisplayParamBuilder(errArr) {

    var params = new Array();

    params.push("url");
    params.push("show");
    params.push("context");
    params.push("");

    return params;
}

function wct_showDisplaySuccess(result, callerId, successElement, errArr, fnError, inputs) {

    //update selectionStatus
    if (result.d != "false") {
        var data = result.d;
        //reset body text
        $('#' + callerId + ' .modal-body').html(data);
    }
}
///////////////////////////////////////////////////////////////////////////
//  END Subscribe
///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
//  MailerConfirm Modal
///////////////////////////////////////////////////////////////////////////
function wct_mailerConfirmParamBuilder(errArr) {

    var params = new Array();

    params.push("confirmId");
    var p1 = $("#hdnConfirmId").val();
    params.push(p1);
    params.push("userIp");
    var p2 = $("#hdnUserIp").val();
    params.push(p2);

    return params;
}

function wct_mailerConfirmSuccess(result, callerId, successElement, errArr, fnError, inputs) {

    if (result.d != "false") {

        var data = result.d;
        if (data.indexOf("ERROR - ") == 0) {
            errArr.push(data.slice(8, data.length));
            fnError();
        }
        else {
            successElement.addClass('alert alert-success').html(data);            
            $('#' + callerId + ' .wctmodal-body').addClass('hide');
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//  END MailerConfirm
///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
//  Subscribe Modal
///////////////////////////////////////////////////////////////////////////
function wct_subscribeParamBuilder(errArr) {

    var params = new Array();

    params.push("emailAddress");
    var p1 = $("#subscribeEmail").val();
    if (p1 == undefined || p1.trim().length == 0) {
        errArr.push({ '#subscribeEmail': "Email address is required." });
    }
    params.push(p1);
    params.push("profileName");
    var p2 = $("#hdnProfile").val();
    params.push(p2);
    params.push("userIp");
    var p3 = $("#hdnUserIp").val();
    params.push(p3);

    return params;
}

function wct_subscribeSuccess(result, callerId, successElement, errArr, fnError, inputs) {

    //update selectionStatus
    if (result.d != "false") {
        var data = result.d;

        if (data.indexOf("ERROR - ") == 0) {

            //handle known errors
            if (data.indexOf("Please enter a valid email address") != -1)
                errArr.push({ '#subscribeEmail': data.slice(8, data.length) });
            else
                errArr.push(data.slice(8, data.length));

            fnError();
        }
        else {
            //display result
            successElement.addClass('alert alert-success').html(data);

            //reset inputs and hide modal body
            for (var i = 0; i < inputs.length; i++) {
                $(inputs[i]).html('');
            }

            $('#' + callerId + ' .wctmodal-body').addClass('hide');
        }
    }
}

///////////////////////////////////////////////////////////////////////////
//  END Subscribe
///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
//  Unsubscribe Modal
///////////////////////////////////////////////////////////////////////////

function wct_unsubscribeParamBuilder(errArr) {

    var params = new Array();
    //errArr.push('test a single string');

    params.push("emailAddress");
    var p1 = $("#unsubscribeEmail").val();
    if (p1 == undefined || p1.trim().length == 0) {
        errArr.push({ '#unsubscribeEmail': "Email address is required." });
    }
    params.push(p1);

    return params;
}

function wct_unsubscribeSuccess(result, callerId, successElement, errArr, fnError, inputs) {

    //update selectionStatus
    if (result.d != "false") {
        var data = result.d;

        if (data.indexOf("ERROR - ") == 0) {
            //handle known errors
            if (data.indexOf("Please enter a valid email address") != -1)
                errArr.push({ '#unsubscribeEmail': data.slice(8, data.length) });
            else
                errArr.push(data.slice(8, data.length));
            fnError();
        }
        else {
            //display result
            successElement.addClass('alert alert-success').html(data);

            //reset inputs and hide modal body
            for (var i = 0; i < inputs.length; i++) {
                $(inputs[i]).html('');
            }

            $('#' + callerId + ' .wctmodal-body').addClass('hide');
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//  END Unsubscribe
///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
//  Contact Email Modal
///////////////////////////////////////////////////////////////////////////
/** contactParamBuilder
    build and validate necessary params for sending a contact email.
    The array indicates which element is in error, but a single string can 
    be used as well
*/
function wct_contactParamBuilder(errArr) {
    
    var params = new Array();

    params.push("fromName");
    var p1 = $("#contactName").val();
    if (p1 == undefined || p1.trim().length == 0) {
        errArr.push({ '#contactName': "From name is required." });
    }
    params.push(p1);

    params.push("emailAddress");
    var p2 = $("#contactEmail").val();
    if (p2 == undefined || p2.trim().length == 0) {
        errArr.push({ '#contactEmail' : "Email address is required." });
    }
    params.push(p2);

    params.push("subject");
    var p3 = $("#contactSubject").val();
    if (p3 == undefined || p3.trim().length == 0) {
        errArr.push({ '#contactSubject': "Subject is required." });
    }
    params.push(p3);

    params.push("message");
    var p4 = $("#contactMessage").val();
    if (p4 == undefined || p4.trim().length == 0) {
        errArr.push({ '#contactMessage': "Please provide a message." });
    }    
    params.push(p4);

    return params;
}

function wct_contactSuccess(result, callerId, successElement, errArr, fnError, inputs) {

    if (result.d != "false") {
        var data = result.d;

        if (data.indexOf("ERROR - ") == 0) {
            if (data.indexOf("Please enter a valid email address") != -1)
                errArr.push({ '#contactEmail' : data.slice(8, data.length) });
            else 
                errArr.push(data.slice(8, data.length));
            fnError();
        }
        else {
            successElement.addClass('alert alert-success').html(data);

            //reset inputs and hide modal body
            for (var i = 0; i < inputs.length; i++) {
                $(inputs[i]).html('');
            }

            $('#' + callerId + ' .wctmodal-body').addClass('hide');
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//  END Contact
///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
//  Get Writeup Modal
///////////////////////////////////////////////////////////////////////////
function wct_getWriteupParamBuilder(errArr) {

    var params = new Array();

    params.push("idx");
    var p1 = $("#hdnid").val();
    if (p1 == undefined || p1.trim().length == 0) {
        errArr.push({ '#hdn': "ShowId is required." });
    }
    params.push(p1);

    return params;
}

function wct_getWriteupSuccess(result, callerId, successElement, errArr, fnError, inputs) {

    var data = result.d;
    //hide certain uglies in overlapped area
    $('.showmore IFRAME').css('display', 'none');
    $("#" + callerId + " #showmoremodallabel").html( data.title );
    $("#" + callerId + " .modal-body").html( data.renderedView );
}
///////////////////////////////////////////////////////////////////////////
//  END GetWriteup
///////////////////////////////////////////////////////////////////////////