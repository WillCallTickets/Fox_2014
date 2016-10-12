
var prmOvInstance = Sys.WebForms.PageRequestManager.getInstance();

//we need to do this because we can only have one event attached to onload
jQuery(function ($) {

    $(document).ajaxStart(BlockUISetup).ajaxStop($.unblockUI);
    registerBlockingElements();


    registerPublish();

    registerDateTimePickers();

    registerAdminDisplayModal();
    registerShowCopyModal();

    //registerVDTypeahead();

    registerImageCropAndRotate();
    registerImageUploadify();
    registerMailerImageUploader();

    registerImageBase_Uploader();

    registerDraggables();

    registerShowSelectClick();
    registerVdRowSelect();

    registerCustomPagerWithTabNav();
    registerCollectionContextPager();

    registerAdminSearches();

    registerShowMgmt();

});




//Normal page load
// $()
// add_pageLoaded

//AJAX postback
// add_beginRequest
// add_pageLoaded
// add_endRequest

//Page.add_beginRequest(OnBeginRequest);
//Page.add_pageLoaded(OnPageLoaded);
//Page.add_endRequest(OnEndRequest);

prmOvInstance.add_beginRequest(function () {

    //alert('begin req');
    //BlockUISetup();
    registerCustomPagerWithTabNav();

    //$(document).ajaxStart(BlockUISetup).ajaxStop($.unblockUI);
});

prmOvInstance.add_pageLoaded(function () {

    registerCustomPagerWithTabNav();
    //registerDateTimePickers();
});

prmOvInstance.add_endRequest(function () {

    //alert('instance end request');
    //registerPublish();causes dupe popup/confirms

    registerDateTimePickers();

    registerAdminDisplayModal();
    registerShowCopyModal();

    //registerVDTypeahead();

    registerImageCropAndRotate();
    registerImageUploadify();
    registerMailerImageUploader();

    registerImageBase_Uploader();

    registerDraggables();

    registerShowSelectClick();
    registerVdRowSelect();

    registerCustomPagerWithTabNav();

    registerCollectionContextPager();

    registerAdminSearches();

    registerShowMgmt();

    $.unblockUI();
});


function postBackToParent(commandName) {
    __doPostBack("ctl00$MainContent$ctl01", commandName);
}

function postBackToShowSelector(argumentArray) {
    __doPostBack("ctl00$MainContent$ShowSelector1",
    //__doPostBack("ctl00$MainContent$ShowSelector",
            JSON.stringify(argumentArray));
}

//todo figure out a way to have multiple args
//call like this: postBackWithArgs({ "commandName": 'selectCurrentAct', "idx": id });
//receiving page needs to be ready to handle
//void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
//{
//    Dictionary<string, string> args = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgument);
//    string commandName = args["commandName"].ToLower();
function postBackWithArgs(argumentArray) {
    __doPostBack("ctl00$MainContent$ctl01", JSON.stringify(argumentArray));
}

/*****************************************************************/
/****************************POSTBACK*****************************/
/*****************************************************************/
//numRemoveControls 1 for page, 1+ when the control is nested in another control within the page
function getPostbackContainerIdHelper(controlName, numRemoveControls) {
    var pieces = controlName.split('$');
    pieces.splice(-(numRemoveControls), numRemoveControls)
    return pieces.join('$');
}
function postBack_ToControl_WithCommand(controlName, commandName) {
    __doPostBack(controlName, commandName);
}
function postBack_ToControl_WithArgs(controlName, argumentArray) {
    __doPostBack(controlName, JSON.stringify(argumentArray));
}
/*****************************************************************/


registerBlockingElements = function () {

    //$('.panel-selector .btn, .panel-actions .btn').on('click', function () {

    //    BlockUISetup();
    //});
}


//TODO remove after admin redo to coll editors
registerCustomPagerWithTabNav = function () {

    //not only do we need to register with the tabs - we need to register with the principal picker
    var pickers = $('#principalpicker a.btn');
    var tabs = $('.pager-context-nav');


    $('#pnlNavTab a[data-toggle="tab"]').on('show.bs.tab', function (e) {

        var self = $(this);

        var cooKey = _getCooKeyFromContext('AdminNavTabContextCookie',
            ($(self).parents('#bannereditor').length) ? "SalePromotion" :
            ($(self).parents('#kioskeditor').length) ? "Kiosk" :
            ($(self).parents('#posteditor').length) ? "Post" :
            ($(self).parents('#employeeeditor').length) ? "Employee" :
            ($(self).parents('#faqeditor').length) ? "FaqItem" :
            ($(self).parents('#showeditor').length) ? "Show" :
            ($(self).parents('#acteditor').length) ? "Act" :
            ($(self).parents('#promotereditor').length) ? "Promoter" :
            ($(self).parents('#venueeditor').length) ? "Venue" :
            "");

        var value = self.text();

        toggleTabRowControls(cooKey, value);

    });


    //evaluate page - or controls for context - #bannereditor, #kioskeditor
    pickers.on("click", function (e) {

        var self = $(this);

        var cooKey = _getCooKeyFromContext('AdminNavTabContextCookie',
            ($(self).parents('#bannereditor').length) ? "SalePromotion" :
            ($(self).parents('#kioskeditor').length) ? "Kiosk" :
            ($(self).parents('#posteditor').length) ? "Post" :
            ($(self).parents('#employeeeditor').length) ? "Employee" :
            ($(self).parents('#faqeditor').length) ? "FaqItem" :
            ($(self).parents('#showeditor').length) ? "Show" :
            ($(self).parents('#acteditor').length) ? "Act" :
            ($(self).parents('#promotereditor').length) ? "Promoter" :
            ($(self).parents('#venueeditor').length) ? "Venue" :
            "");

        var value = $.cookie(cooKey);

        toggleTabRowControls(cooKey, value);

    });
}
function toggleTabRowControls(contextKey, value) {

    //look to _Enums.CookEnums for registered values
    if (contextKey == "none" || contextKey.indexOf("rake") != -1) {

        //find pager and other links to disable
        var pager = $('.admincustompager');
        var linkNew = $('.tab-navrow-control');

        if (value == undefined || value == false || value == "" || value.toLowerCase() == "edit") {
            pager.unblock();
            linkNew.unblock();
        }
        else {
            pager.block({ message: null });
            linkNew.block({ message: null });
        }
    }
}

// Sets timeouts for fading in and out of well within the pager
registerCollectionContextPager = function() {

    var container = $('#coll-criteria-editor');
    var well = $('#coll-criteria-editor .well-outfront');

    $('#criteriaapply, #criteriacancel').on('click', function (e) {

        well.removeClass('in');
        setTimeout(function () {
            container.fadeOut(500);
        }, 500);
    });

    $('#criteriatoggle').on('click', function (e) {

        //if the panel is visible - start fading out
        if ($(container).css('display') == 'block') {
            well.removeClass('in');
        }

        container.toggle(200, function (e) {
            if ($(container).css('display') == 'block') {
                well.addClass('in');
            }
        });
    });
}


// allows the row to be clicked and triggers the select mechanism
// ensures the edit and delete buttons do their jobs without triggering the parent mechanism
registerVdRowSelect = function () {

    $('.vdquery-row-select .badge, .vdquery-row-select .item-delete').click(function (event) {
        event.stopPropagation();
    });
}


//retrieves appropriate info for context from a dropdownlist with show selections
registerShowSelectClick = function () {

    $('#insertShowSelection').click(function (event) {

        event.preventDefault();

        var elem = $(this);

        //retrieve ids from the link itself
        //href contains the id for the selection
        //rel contains the id for the textbox/target
        var ddl = $(elem.attr("href"));
        var txt = $(elem.attr("rel"));

        //get value from dropdown - if its 0 then return
        if(ddl.val() == "0")
            return false;

        //call service to get show url
        $.ajax({
            url: '/Admin/_customControls/ShowInformationRetrieval.ashx',
            data: {
                'commandOperation' : "getShowUrl",
                'showId': ddl.val()
            },
            datatype: "text",//handler is text
            type: 'post',
            success: function (result) {
                if (result.indexOf("Error: ") != -1)
                    alert(result);
                else {

                    //insert url into click textbox
                    var res = JSON.parse(result);                    
                    txt.val(res["url"]);

                    //reset the ddl
                    ddl.val(0);

                    return true;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                
                alert('showinfo retrieval Error :' + xhr.responseText);
            }
        });
        
        //don't let it do anything to the page cycle!
        return true;
    });
}



registerDraggables = function () {

    $('.kiosk-dragger')
        .draggable({
            containment: "parent",
            cursor: "crosshair",
            stop: recordPosChange
        })
        .resizable({
            containment: "parent",
            stop: recordPosChange
        });

    function recordPosChange(event, ui) {

    }
}

registerImageUploadify = function () {

    //hidden fields
    //$('#hdnEntityType') - kiosk or show

    var imgupload = $('#imageuploadify');

    if (imgupload != null && imgupload != undefined && imgupload.length > 0) {

        imgupload.uploadifive({

            'uploadScript': '/Admin/_customControls/ImageManager_Upload.ashx',
            'auto': true,
            'buttonClass': 'my-uploadify',
            'buttonText': 'Upload',
            'multi': false,
            //'fileType': 'Image Files',
            'fileType': ['image/gif', 'image/jpeg', 'image/png'], //thumbnail resizers cant handle pdf  'application/pdf'],
            //'fileType': ['jpg','jpeg','gif','png','pdf'],
            //'fileTypeExts': '*.jpg;*.jpeg;*.gif;*.png',
            'fileTypeExt': '*.jpg;*.jpeg;*.gif;*.png',
            'formData': { 'entityType': $('#hdnEntityType').val() },
            'removeCompleted': true,
            'simUploadLimit': 1,
            'queueSizeLimit': 1,
            'itemTemplate': '<div class="uploadifive-queue-item"><span class="filename"></span> | <span class="fileinfo"></span><div class="close"></div></div>',

            'onProgress': function (file, e) {
                if (e.lengthComputable) {
                    var percent = Math.round((e.loaded / e.total) * 100);
                }
                file.queueItem.find('.fileinfo').html(' - ' + percent + '%');
                file.queueItem.find('.progress-bar').css('width', percent + '%');
            },
            'onError': function (file, errorCode, errorMsg, errorString) {
                
                var error = "error occurred";

                try {
                    error = errorCode.xhr.response;

                    imgupload.uploadifive('clearQueue');//this will error if file was bad
                }
                catch (e) {
                }
                
                alert(error);
            },
            'onCancel': function () {
                imgupload.uploadifive('clearQueue');
            },
            'onQueueComplete': function (uploads) {
                //Triggered when all files in the queue have completed uploading.
                
                //postBackToParent('refresh');
            },
            'onUploadComplete': function (file, data) {

                postBackToParent('rebind');
            }
        });
    }
}

//for image manager parents??
registerImageCropAndRotate = function () {

    //image display
    //$('#imageeditbox');
    //transform buttons
    //$('#performcrop');
    //$('#performrotate');
    //hidden fields
    //$('#hdnEntityType')
    //$('#hdnDisplayWidth')
    //$('#x1'), $('#y1'), $('#x2'), $('#y2'), $('#w1'), $('#h1')

    var jcrop_api;
    var editimg = $('#imageeditbox');
    var performcrop = $('#performcrop');
    var performrotate = $('#performrotate');
    var entityType = $('#hdnEntityType').val();
    //this just resets aspect ratio - does not save/alter anything
    //var performsquare_cropratio = $('#performcrop_ratio_square');//not used for rotaionalad


    function getCropRatio() {
        var cropRatio = $('#hdnCropRatio');
        if (cropRatio != undefined && cropRatio.length == 1)
            return cropRatio[0].value;

        return 0;//default to ???
    }



    if (editimg != null && editimg != undefined && editimg.length > 0) {

        //setup jcrop
        var cropimg = editimg.Jcrop({
            aspectRatio: getCropRatio(),
            //aspectRatio:0,
            onChange: showCropCoords,
            onSelect: showCropCoords
        },
        function () {
            jcrop_api = this;
        });

        $('#performcrop_ratio_square').click(function() {
            
            //toggle current selection
            jcrop_api.release();
            jcrop_api.disable();
            jcrop_api.enable();

            var txt = this.innerText;

            if (txt == 'Square Crop') {
                this.innerText = 'Reset Crop';
                jcrop_api.setOptions({ aspectRatio: 1 });
            }
            else {
                this.innerText = 'Square Crop';
                jcrop_api.setOptions({ aspectRatio: 0 });
            }
        });

        //setup crop routine
        if (performcrop != null && performcrop != undefined && performcrop.length > 0) {
            
            performcrop.click(function () {
                
                if ($("#w1").val() != "" && $("#h1").val() != "") {
                    $.ajax({
                        url: '/Admin/_customControls/ImageManager_Transform.ashx',
                        data: { 'transformType': 'crop', 'entityType': entityType,//use table name
                            'displayWidth': $('#hdnDisplayWidth').val(),
                            'x1': Math.floor($("#x1").val()), 'y1': Math.floor($("#y1").val()), 'x2': Math.floor($("#x2").val()),
                            'y2': Math.floor($("#y2").val()), 'w': Math.floor($("#w1").val()), 'h': Math.floor($("#h1").val())
                        },
                        datatype: "text",//handler is text
                        type: 'post',
                        success: function (result) {
                            if (result.indexOf("Error: ") != -1) 
                                alert(result);
                            else 
                                postBackToParent('rebind');//posts back to first control under director
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            
                            alert('Cropping Error :' + xhr.responseText);
                        }
                    });
                }
                return false;
            });
        }//end of performcrop

        //setup rotate routine
        if (performrotate != null && performrotate != undefined && performrotate.length > 0) {

            performrotate.click(function () {
                $.ajax({
                    url: '/Admin/_customControls/ImageManager_Transform.ashx',
                    data: { 'transformType': 'rotate', 'entityType': entityType },
                    datatype: "text",
                    type: 'post',
                    success: function (result) {
                        if (result.indexOf("Error: ") != -1) 
                            alert(result);
                        else 
                            postBackToParent('rebind');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Rotating Error :' + xhr.responseText);
                    }
                });
                return false;
            });
        }

    }//end if editimg
}

/*requires type and id from page - and an atx.Current***Record that has path info - handler handles diff object types(todo) 
    control's parent needs to handle postback to "refresh_post_upload"
    uploadify file upload - #basicUploadify => only handles single uploads
*/
registerImageBase_Uploader = function () {

    var imageupload = $('#basicUploadify');
    
    if (imageupload != null && imageupload != undefined && imageupload.length > 0) {

        imageupload.uploadifive({

            'uploadScript': '/Admin/_customControls/ImageBase_Upload.ashx',
            'auto': true,
            'buttonClass': 'btn btn-success',
            'buttonText': 'Upload File',
            'multi': false,
            'fileType': 'Image Files',
            'formData': { 'entityType': $('#hdnEntityType').val(), 'itemId': $('#hdnEntityId').val() },
            'removeCompleted': true,
            'simUploadLimit': 1,
            'queueSizeLimit': 1,
            'itemTemplate': '<div class="uploadifive-queue-item"><span class="filename"></span> | <span class="fileinfo"></span><div class="close"></div></div>',

            'onProgress': function (file, e) {
                if (e.lengthComputable) {
                    var percent = Math.round((e.loaded / e.total) * 100);
                }
                file.queueItem.find('.fileinfo').html(' - ' + percent + '%');
                file.queueItem.find('.progress-bar').css('width', percent + '%');
            },
            'onError': function (file, errorCode, errorMsg, errorString) {

                var error = "error occurred";

                try {
                    error = errorCode.xhr.response;
                }
                catch (e) { }

                imageupload.uploadifive('clearQueue');
                alert(error);
            },
            'onCancel': function () {
                imageupload.uploadifive('clearQueue');
            },
            'onQueueComplete': function (uploads) {
                //Triggered when all files in the queue have completed uploading.                
                //postBackToParent('refresh');
            },
            'onUploadComplete': function (file, data) {

                postBackToParent('refresh_post_upload');
            }
        });
    }
}

registerMailerImageUploader = function () {

    var jcrop_api;
    var mailimgupload = $('#mailerimageupload');
    var editimg = $('#cropmailimage');

    //setup jcrop
    if (editimg != null && editimg != undefined && editimg.length > 0) {

        var cropimg = editimg.Jcrop({
            aspectRatio: 1,
            onChange: showCropCoords,
            onSelect: showCropCoords
        },
        function () {
            jcrop_api = this;
        });

        var rotateimage = $('#rotatemailerimage');

        if (rotateimage != null && rotateimage != undefined && rotateimage.length > 0) {

            rotateimage.click(function () {
                $.ajax({
                    url: '/Admin/_customControls/MailerImageRotate.ashx',                    
                    data : { 'entityType': 'mailerimage', 'itemId': $('#hdnMailerShowId').val() },
                    datatype: "text",
                    type: 'post',
                    success: function (result) {

                        if (result.indexOf("Error: ") != -1) {

                            alert(result);
                        }
                        else {

                            postBackToParent('resetimageupload');
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                        alert('Rotating Error :' + xhr.responseText);
                    }
                });
                return false;
            });
        }

        var performcrop = $('#performmailercrop');

        if (performcrop != null && performcrop != undefined && performcrop.length > 0) {

            performcrop.click(function () {

                if ($("#w1").val() != "" && $("#h1").val() != "") {

                    $.ajax({
                        url: '/Admin/_customControls/MailerImageCrop.ashx',
                        data: { 'entityType': 'mailerimage', 'itemId': $('#hdnMailerShowId').val(), 'displayWidth': $('#hdnDisplayWidth').val(), 'x1': $("#x1").val(), 'y1': $("#y1").val(), 'x2': $("#x2").val(), 'y2': $("#y2").val(), 'w': $("#w1").val(), 'h': $("#h1").val() },
                        datatype: "text",
                        type: 'post',
                        success: function (result) {
                            if (result.indexOf("Error: ") != -1) {

                                alert(result);
                            }
                            else {

                                postBackToParent('resetimageupload');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert('Cropping Error :' + xhr.responseText);
                        }
                    });
                }
                return false;
            });
        }
    }
    
    if (mailimgupload != null && mailimgupload != undefined && mailimgupload.length > 0) {

        mailimgupload.uploadifive({

            'uploadScript': '/Admin/_customControls/MailerImageUpload.ashx',
            'auto': true,
            'buttonClass': 'btn btn-success',
            'buttonText': 'Upload File',
            'multi': false,
            'fileType': 'Image Files',
            'formData': { 'entityType': 'mailerimage', 'itemId' : $('#hdnMailerShowId').val() },
            'removeCompleted': true,
            'simUploadLimit': 1,
            'queueSizeLimit': 1,
            'itemTemplate': '<div class="uploadifive-queue-item"><span class="filename"></span> | <span class="fileinfo"></span><div class="close"></div></div>',

            'onProgress': function (file, e) {
                if (e.lengthComputable) {
                    var percent = Math.round((e.loaded / e.total) * 100);
                }
                file.queueItem.find('.fileinfo').html(' - ' + percent + '%');
                file.queueItem.find('.progress-bar').css('width', percent + '%');
            },
            'onError': function (file, errorCode, errorMsg, errorString) {

                var error = "error occurred";

                try {
                    error = errorCode.xhr.response;
                }
                catch (e) { }

                mailimgupload.uploadifive('clearQueue');
                //showimgupload.uploadifive('cancel', $('.uploadifive-queue-item').first().data('file'));
                alert(error);
            },
            'onCancel': function () {
                mailimgupload.uploadifive('clearQueue');
            },
            'onQueueComplete': function (uploads) {
                //Triggered when all files in the queue have completed uploading.
                //postBackToParent('refresh');
            },
            'onUploadComplete': function (file, data) {

                postBackToParent('resetimageupload');
            }
        });
    }
}

registerPublish = function () {

    //assign a web service event to the click of the publish button
    //$('#publishMenuButton, #publishFloater').click(function (clk) {
    $('#publishMenuButton').click(function (clk) {

        BlockUISetup();

        var reply = confirm('Are you positive that you want to publish?');

        if (reply) {

            var params = new Array();

            exec_CallAdminService("PublishButton", params, publishSuccess, publishError);

            return true;
        }

        return false;
    });
}

registerShowCopyModal = function () {
    
    var copyModal = $('#showcopymodal');

    if (copyModal.length > 0) {

        $('.showcopy-modal-launcher').on('click', function () {
            $('#showcopymodal').modal('show');
        });

        $('#showcopymodal').wctModal(
            'showCopy',
            //define success
            wct_showCopySuccess,
            //define inputs
            ['#copyshowdate', '#hdnUserName', '#hdnCurrentShowId'],
            wct_showCopyParamBuilder
            ).on('show.bs.modal', function () {
                
                //help ie with init - also need to perform "update"
                $('#copyshowdate').val($('#startdateinput').val());
            });

        $(".form_startdatetime").datetimepicker({
            format: "dd mm yyyy - hh:ii p",
            showmeridian: true,
            autoclose: true,
            todaybtn: true
        });
    }    
}

registerAdminDisplayModal = function () {

    loadAdminDisplayModal = function () {
        _renderContent(null, "adminshow", "", "#admindisplaymodal .modal-body", "#admindisplaymodal #admindisplaymodallabel");
        $('.desc-control IFRAME').toggle();//hide underlying iframes - doesn't work in shown  
    }

    $('#admindisplaymodal .modal-body')
        .html('<button type="button" class="hide-from-view" data-toggle="button" data-loading-text="<span class=\'wct-modal-loader-spinner\'></span>Loading...">Loading show...</button>');

    $('.showdisplay-modal-launcher').on('click', function () {
        $('#admindisplaymodal').modal('show');
        loadAdminDisplayModal();
    });

    $('#admindisplaymodal')
        .on('show.bs.modal', function () {

            var content = $('#admindisplaymodal .modal-body').html();
            if (content == "") {
                loadAdminDisplayModal();
            }
        })
        .on('shown.bs.modal', function () {
            registerShowMore('admin');

            //var scrollers = $(".content-scrollable");
            //scrollers.mCustomScrollbar("update");
        })
        .on('hidden.bs.modal', function () {
            $('#admindisplaymodal .modal-body').html('');
            $('.desc-control IFRAME').toggle();//show underlying iframes
        })
    ;
}

function AddSelectedBanners() {

    var bannerselections = new Array();
    $('#bannerlist input:checked').each(function () {
        bannerselections.push($(this).attr('value'));
    });

    //convert inputs to strings
    params = new Array();

    params.push("mailerContentId");
    var p1 = $("#hdnMailerContentId").val();
    params.push(p1);

    params.push("bannerList");
    params.push(bannerselections);

    //do ajax call to add selections to currently selected MailContent
    exec_CallAdminService("AddMailerBanners", params, function (result) { addMailerBannerSuccess(result); }, addMailerBannerError);
}

function AddSelectedShows () {

    var foxselections = new Array();
    var betselections = new Array();
    var chqselections = new Array();

    $('#foxshowlist input:checked').each(function () {
        foxselections.push($(this).attr('value'));
    });
    $('#betshowlist input:checked').each(function () {        
        betselections.push( $(this).attr('value'));
    });
    $('#chqshowlist input:checked').each(function () {
        chqselections.push($(this).attr('value'));
    });

    //convert inputs to strings
    params = new Array();

    params.push("mailerContentId");
    var p1 = $("#hdnMailerContentId").val();
    params.push(p1);

    params.push("foxShowList");    
    params.push(foxselections);

    params.push("betShowList");
    params.push(betselections);

    params.push("chqShowList");
    params.push(chqselections);
    
    //do ajax call to add selections to currently selected MailContent
    exec_CallAdminService("AddMailerShows", params, function (result) { addMailerShowSuccess(result); }, addMailerShowError);
}
function ResetSelectedShows() {

    $('#foxshowlist input:checked, #betshowlist input:checked').each(function () {
        $(this).removeAttr('checked');
    });
}
function ResetSelectedBanners() {

    $('#bannerlist input:checked').each(function () {
        $(this).removeAttr('checked');
    });
}
function addMailerShowSuccess(result) {

    if (result.d != "false") {

        var data = result.d;
        if (data.indexOf("ERROR") == 0) {
            alert(data);
        }
        else {

            postBackToParent('resetcontent');

            //reset selections
            ResetSelectedShows();

        }
    }
}


function addMailerBannerSuccess(result) {

    if (result.d != "false") {

        var data = result.d;
        if (data.indexOf("ERROR") == 0) {
            alert(data);
        }
        else {

            postBackToParent('resetcontent');

            //reset selections
            ResetSelectedBanners();

        }
    }
}
function addMailerShowError(xhr, ajaxOptions, thrownError) {
    console.log(xhr);
    alert('AddMailerShow Error :' + xhr.responseText);
}
function addMailerBannerError(xhr, ajaxOptions, thrownError) {
    alert('AddMailerBanner Error :' + xhr.responseText);
}

registerDateTimePickers = function () {

    $(".mailer-dtpicker").datetimepicker({
        format: "M/dd/yyyy HH:ii P",
        showMeridian: true,
        autoclose: true,
        todayBtn: true,
        startDate: new Date()
    });

    $(".banner-dtpicker, .kiosk-dtpicker").datetimepicker({
        format: "M dd yyyy HH:ii P",
        showMeridian: true,
        autoclose: true,
        todayBtn: true
    });

    $(".showeditor-dtpicker").datetimepicker({
        format: "M dd yyyy HH:ii P",
        showMeridian: true,
        autoclose: true,
        todayBtn: true
    });

    ///* picker with date only */
    //$(".showselector-dtpicker").datetimepicker({
    //    format: "M dd yyyy"


    //    search on ie datepicker smalot
    //    https://github.com/smalot/bootstrap-datetimepicker/pull/332
    //    format: "yyyy",
    //weekStart: 1,
    //todayBtn: true,
    //autoclose: true,
    //todayHighlight: true,
    //startView: 4,
    //minView: 4,
    //maxView: 4,
    //forceParse: true



    //});

    

    /*hidetext .datetimepicker .datetimepicker-minutes thead .switch {} */
    $(".showeditor-showtime-dtpicker").datetimepicker({
        format: "HH:ii P",
        showMeridian: true,
        autoclose: true,
        todayBtn: false,
        startView: 'day'//hour forces us to cycle through am and pm

        //hide date display when dealing with a time picker
    }).on('show', function (e, ok) {
        $('.datetimepicker-hours thead, .datetimepicker-minutes thead').toggle();
    }).on('hide', function (e, ok) {
        $('.datetimepicker-hours thead, .datetimepicker-minutes thead').toggle();
    });

    var startdate = $(".showselector-dtpicker").datetimepicker({
        format: "M dd yyyy",
        showMeridian: false,
        autoclose: true,
        todayBtn: false,
        minView: 'month'
    }).on('changeDate', function (ev) {

        var container = getPostbackContainerIdHelper(ev.currentTarget.name, 1);
        postBack_ToControl_WithArgs(container, { "commandName": 'dtpickerchange', "newDate": ev.date.valueOf().toString() });
    });
}

function showCropCoords(c) {
    jQuery('#x1').val(c.x);
    jQuery('#y1').val(c.y);
    jQuery('#x2').val(c.x2);
    jQuery('#y2').val(c.y2);
    jQuery('#w1').val(c.w);
    jQuery('#h1').val(c.h);
};

//http://sanghimz.blogspot.com/2012/09/jquery-block-ui-in-aspnet-using-update.html
function BlockUISetup() {
    var msg = $get('hdnProcessingMessage').value;

    $.blockUI({
        fadeIn: 1000,
        fadeOut: 1000,
        message: '<h1><img src=\"/assets/images/loaderCirc.gif\" alt=\"\" />&nbsp;&nbsp;' + msg + '</h1>'
    });
}

function publishSuccess(result) {
    alert('Publish complete!');
}

function publishError(xhr, ajaxOptions, thrownError) {
    alert('Publish Error :');
}

function exec_CallAdminService(fn, paramArray, successFn, errorFn) {
    
    var pagePath = window.location.pathname;
    //Create list of parameters in the form:   
    //{"paramName1":"paramValue1","paramName2":"paramValue2"}   
    //Call the page method   
    $.ajax({
        type: "POST",
        url: "/Svc/Admin/AdminServices.asmx/" + fn,
        contentType: "application/json; charset=utf-8",
        data: parseParams(paramArray),
        dataType: "json",
        success: successFn,
        error: errorFn
    });
}
//775