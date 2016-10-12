
(function ($) {

    /*  presupposes
        .wctmodal-response      => generic class for the error and success 
        .wctmodal-error         => error div
        .wctmodal-success       => success div
        .control-group          => surrounds lines of label input box
        xxxGroup                => is the id of the control group. xx is same as input
        .wct-modal-action       => modal submit button **required - hide/indent in css if necessary
        fnHandlerPath           => this is the full path to the handler
        fnSuccess               => success callback
        inputs                  => a list of input elements to handle clearing error indicators
        fnParamValidate         => function to ubild input from params and set errors
    */

    $.fn.z2Modal = function (_fnHandlerPath, _fnSuccess, _inputs, _fnParamValidate) {

        //ensure we are just dealing with a single element
        var self = $(this).get(0);
        if (self != undefined) {
            var selfId = self.id;
            var sender = $('#' + selfId + ' .wct-modal-action');
            var errorDisplayElement = $('#' + selfId + ' .wctmodal-error');
            var successDisplayElement = $('#' + selfId + ' .wctmodal-success');

            //private props
            var inputs = _inputs;
            var errors = new Array();

            //private funcs
            var fnHandlerPath = _fnHandlerPath;
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
                    _setCarouselItemScrollHeight();
                }
            }

            function deactivateSender() {
                sender.button('loading');
                //reset and clear any error indicators
                $('.control-group').add('.wctmodal-response').removeClass('alert alert-danger alert-success');
                $('.wctmodal-response').html('');
            }

            function reactivateSender() {
                sender.button('reset');
            }

            function loadAjax() {

                errors = new Array();//init for every call
                //param builder will assign errors for inputs
                var paramList = parseParams(fnParamValidate(errors, selfId));

                if (errors.length > 0) {
                    displayAndResetErrors();
                    return;
                }

                //Call the page method   
                $.ajax({
                    url: fnHandlerPath + '.ashx',
                    data: (selfId == 'mailerConfirmModal') ? { 'email': $("#hdnConfirmId").val(), 'source': 'Confirmation Click', 'privacy': 'true', 'requestType': 'subscribe' } :
                        (selfId == 'unsubscribeZ2Modal') ? { 'email': $("#unsubscribeEmail").val(), 'source': 'Mailer Manage', 'privacy': 'true', 'requestType': 'unsubscribe' } :
                        (selfId == 'subscribeZ2Modal') ? { 'email': $("#subscribeEmail").val(), 'source': 'Website Signup', 'privacy': ($("#chkPrivacy").val() == 'on') ? 'true' : 'false', 'requestType': 'subscribe' } :
                        (selfId == 'subscribePopupZ2Modal') ? { 'email': $("#" + selfId + " #subscribeEmail").val(), 'source': 'Website Signup', 'privacy': ($("#" + selfId + " #chkPrivacy").val() == 'on') ? 'true' : 'false', 'requestType': 'subscribe' } :
                         {}
                        ,
                    datatype: "text",
                    type: 'post',
                    success: function (result) {
                        fnSuccess(result, selfId, successDisplayElement, errors, displayAndResetErrors, inputs, sender);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                        var err = eval("(" + xhr.responseText + ")");
                        errors.push({ '': err });
                        displayAndResetErrors();
                        return;
                    }
                });
            }

            //now find the btnModalAction within the caller
            sender.click(function () {

                deactivateSender();
                loadAjax();
            });

            //return function binds the modal to the show/hide functions        
            return $(self).on('show.bs.modal', function () {
                
                $('#' + selfId + ' .wctmodal-body').removeClass('hide');

            }).on('hidden.bs.modal', function () {

                //reset elements to their original state
                $('.form-group').add('.wctmodal-response').removeClass('alert alert-danger alert-success');
                $('.wctmodal-response').html('');

                //clean up inputs on close - unknown terr here - wrap in try
                try {
                    if (inputs != undefined) {
                        for (var i = 0; i < inputs.length; i++) {

                            $(inputs[i]).html('').val('');
                        }
                    }
                }
                catch (ex) { }

            });
        }
    }
})(jQuery);


///////////////////////////////////////////////////////////////////////////
//  MailerConfirm Modal
///////////////////////////////////////////////////////////////////////////
function z2_mailerConfirmParamBuilder(errArr) {

    var params = new Array();

    params.push("email");
    var p1 = $("#hdnConfirmId").val();
    params.push(p1);

    if (p1.trim().length == 0)
        errArr.push('email was not found and is required.');

    return params;
}

function z2_mailerConfirmSuccess(result, callerId, successElement, errArr, fnError, inputs, sender) {

    //if we get a return message that says we are already signed up - then show success
    var res = JSON.parse(result);

    var status = res["status"];
    var msg = res["messages"];

    if (status == 'Success' || (status == 'Error' && msg[0].indexOf('is currently subscribed') != -1)) {
        successElement.addClass('alert alert-success').html('You are now subscribed to the Z2 Entertainment Newsletter!');                
        sender.hide();
    }
    else {
        for (var i = 0; i < msg.length; i++) {
            errArr.push(msg[i]);
        }        
        fnError();
    }
}

///////////////////////////////////////////////////////////////////////////
//  END MailerConfirm
///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
//  Mailer Manage Modal
///////////////////////////////////////////////////////////////////////////
function z2_mailupApiManageParamBuilder(errArr) {

    var params = new Array();
    
    params.push("emailAddress");
    var p1 = $("#unsubscribeEmail").val();

    if (p1 == undefined || p1.trim().length == 0) {
        errArr.push({ '#unsubscribeEmail': "Email address is required." });
    }
    params.push(p1);

    return params;
}

function z2_mailupApiSuccess(result, callerId, successElement, errArr, fnError, inputs, sender) {

    //if we get a return message that says we are already signed up - then show success
    var res = JSON.parse(result);

    var status = res["status"];
    var msg = res["messages"];

    if (status == 'Success') {

        var output = '';
        for (var i = 0; i < msg.length; i++) {
            output += '<li>' + msg[i] + '</li>';
        }
        if (output.length > 0)
            output = '<ul class="list-unstyled">' + output + '</ul>';
        
        // show result msg
        successElement.addClass('alert alert-success').html(output);

        //reset inputs (blank text) and hide modal body
        for (var i = 0; i < inputs.length; i++) {
            $(inputs[i]).html('');
        }

        //hide any errors we may have had in process
        $(callerId + ' .alert-alert-danger').html('').css({ 'display': 'none' });
        var bod = $('#' + callerId + ' .wctmodal-body').hide();
        sender.button('reset');
    }
    else {
        for (var i = 0; i < msg.length; i++) {
            errArr.push(msg[i]);
        }

        fnError();
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

            //handle known errors
            if (data.indexOf("Please enter a valid email address") != -1)
                errArr.push({ '#contactEmail' : data.slice(8, data.length) });
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
//  END Contact
///////////////////////////////////////////////////////////////////////////

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
//  END Show Display Modal
///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
//  Subscribe Modal
///////////////////////////////////////////////////////////////////////////
function z2_subscribeParamBuilder(errArr, source) {

    var params = new Array();

    params.push("emailAddress");
    var p1 = $("#" + source + " #subscribeEmail").val();
    if (p1 == undefined || p1.trim().length == 0) {
        errArr.push({ '#subscribeEmail': "Email address is required." });
    }
    params.push(p1);

    params.push("chkPrivacy");
    var p2 = $("#" + source + " #chkPrivacy").get(0).checked;
    if (!p2) {
        errArr.push({ '#chkPrivacy': 'You must acknowledge that you have read and understand the privacy policy.' });
    }
    params.push(p2);

    return params;
}

function z2_subscribeSuccess(result, callerId, successElement, errArr, fnError, inputs, sender) {
    
    var res = JSON.parse(result);

    var status = res["status"];
    var msg = res["messages"];

    if (status == 'Success' || (msg.length == 0 || msg[0] == '')) {
        
        // show result msg
        successElement.addClass('alert alert-success').html('You will receive a confirmation email shortly. Please respond to complete the subscription process.');
        
        //reset inputs (blank text) and hide modal body
        for (var i = 0; i < inputs.length; i++) {
            $(inputs[i]).html('');
        }

        //hide any errors we may have had in process
        $('#' + callerId + ' .alert-alert-danger').html('').css({ 'display': 'none' });
        var bod = $('#' + callerId + ' .wctmodal-body').hide();
        sender.button('reset');
    }
    else {
        //handle known errors
        for (var i = 0; i < msg.length; i++) {
            errArr.push(msg[i]);
        }

        fnError();
    }
}
///////////////////////////////////////////////////////////////////////////
//  END Subscribe
///////////////////////////////////////////////////////////////////////////