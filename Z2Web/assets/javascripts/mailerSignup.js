
/* Functions to manage signujps at mailup */
registerMailupSubscriptionProcess = function () {

    $('.pageform').on('submit', function (e) {
        try{
            e.preventdefault();
        } catch(e){}

        //close the virt keyboard
        try{
            document.activeelement.blur();
            $('input#email').blur();
        } catch (e) { }
    });
}

function validateMailupSubmission(checkbox, strEmail) {

    try{
        if (!checkbox.checked && strEmail.trim().length == 0)
            return false;
    }
    catch (e) { }
    
    var errors = '';
    var errorDisplayElement = $('.wctmodal-error');
    var successDisplayElement = $('.wctmodal-success');

    //wctmodal-response applies to both success and error
    $('.wctmodal-response').removeClass('alert alert-danger alert-success').html('');

    if (!checkbox.checked)
        errors += '<li>You must acknowledge that you have read and understand the privacy policy.</li>';
    if(!isEmail_byMailup(strEmail))
        errors += '<li>Please enter a valid email address.</li>';

    if (errors.length > 0) {
        errors = '<ul class="list-unstyled">' + errors + '</ul>';
        errorDisplayElement.addClass('alert alert-danger').append(errors);
        return false;
    }

    return true;
}

/*built by mailup*/
function isEmail_byMailup(strEmail) {
    validRegExp = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    if (strEmail.search(validRegExp) == -1) {
        return false;
    }
    return true;
}

function isMobileDevice() {
    var isMobile = {
        Android: function () { return navigator.userAgent.match(/Android/i); },
        BlackBerry: function () { return navigator.userAgent.match(/BlackBerry/i); },
        iOS: function () { return navigator.userAgent.match(/iPhone|iPad|iPod/i); },
        Opera: function () { return navigator.userAgent.match(/Opera Mini/i); },
        Windows: function () { return navigator.userAgent.match(/IEMobile/i); },
        any: function () { return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows()); }
    };

    return isMobile;
}

function registerPrivacyPopup(originId, activatorId) {
    
    $(activatorId).on('click', function () {
        $('#privacymodal')
        .on('show.bs.modal', function () {

            ga('send', 'event', 'link', 'privacypopup', "'" + window.location.pathname + "'");

            var viewToRender = 'privacy';

            $.ajax({
                url: '/Handler/RenderView.ashx',
                data: { 'viewToRender': viewToRender },
                datatype: "text",
                type: 'post',
                success: function (result) {
                    if (result.indexOf("Error: ") != -1) {
                        alert(result);
                    }
                    else {
                        var res = JSON.parse(result);
                        $('#privacymodal .modal-body').html(res["renderedView"]);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        })
        .on('shown.bs.modal', function () {
            $(this).scrollTop(0);
        })
        .on('hidden.bs.modal', function () {

            $('#privacymodal .modal-body').html('');

            //if we activated within the signup campaign popup            
            if (originId == '#subscribePopupZ2Modal') {
                //reactivate underlying modal - get scroll working properly
                $('#infomodal').css({ 'overflow-y': 'auto' });
            }

        }).modal('show')
        ;
    });
}
